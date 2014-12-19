<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="TabulateArea.aspx.vb" Inherits="webgis.TabulateArea" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Tabulate Area</title>

    <link rel="stylesheet" href="http://js.arcgis.com/3.9/js/esri/css/esri.css"/>
    <link rel="stylesheet" href="http://js.arcgis.com/3.9/js/dojo/dijit/themes/claro/claro.css"/>
    <link rel="stylesheet" href="../Styles/psu.css"/>

    <script src="http://js.arcgis.com/3.9/" type="text/javascript" djConfig="parseOnLoad: true"></script>
    <script type="text/javascript">

        dojo.require("dojo.parser");
        dojo.require("dojox.grid.DataGrid");
        dojo.require("dojo.data.ItemFileWriteStore");
        dojo.require("dijit.form.Select");

        var gp;
        var district = 'Nashville';
        var listStore;

        function init() {
            //gp = new esri.tasks.Geoprocessor("http://atlas.geog.pdx.edu/arcgis/rest/services/LANDFIRE/TabulateArea/GPServer/MyTool");
            gp = new esri.tasks.Geoprocessor("http://atlas.geog.pdx.edu/arcgis/rest/services/LANDFIRE/TabulateAreaWithFeature/GPServer/TabulateAreaPsu");

            // Retrieve all values from WebservicesObjectDataSource
            var myValues = '<%= WebservicesObjectDataSource.Select %>';
            // Parse values into an ItemFileReadStore
            jsonObject = JSON.parse(myValues);

            var listData = { identifier: "ID", label: "DisplayName", items: jsonObject };
            listStore = new dojo.data.ItemFileReadStore({ data: listData });
            createImageServiceSelect();
        }

        function tabulateArea() {
            var imageSelect = dijit.byId('ImageService');
            var idx = imageSelect.attr('value');
            if (idx > 0) {
                // clear query status
                dojo.byId('query-status').innerHTML = '<p style="color:red; font-weight:bold;}"></p>';
                // clear grid
                var emptyCells = { items: "" };
                var emptyStore = new dojo.data.ItemFileReadStore({ data: emptyCells });
                grid.setStore(emptyStore);
                //initialize query task
                var queryTask = new esri.tasks.QueryTask("http://atlas.geog.pdx.edu/arcgis/rest/services/USACE/USACE_admin_maps/MapServer/6");
                var query = new esri.tasks.Query();
                query.returnGeometry = true;
                query.where = "NAME = '" + district + "'";
                //execute query
                queryTask.execute(query, showResults, errorHandler);
            }
        }

        function tabTest() {
            //var jsonResults = '[{"value":"50","area":100},{"value":"75","area":200}]';
            var jsonResults = '[{"pctArea":"1","value":"11","area":"2,302,020,000"},{"pctArea":"0","value":"12","area":"119,070,000"}]';
            // Parse values into an ItemFileReadStore
            var jsonObject = JSON.parse(jsonResults);
            var data = { identifier: "value", label: "value", items: jsonObject };
            listStore = new dojo.data.ItemFileReadStore({ data: data });
            //Bind data store to grid
            grid.setStore(listStore);
        }

        function gpJobComplete(jobInfo) {
            dojo.byId('query-status').innerHTML = '<p style="color:red; font-weight:bold;}"></p>';
            gp.getResultData(jobInfo.jobId, "json_results", function (results) {
                var jsonResults = results.value;
                if (jsonResults.length > 0) {
                    // Parse values into an ItemFileReadStore
                    var strResults = JSON.stringify(jsonResults);
                    var jsonObject = JSON.parse(strResults);

                    var data = { identifier: "value", label: "value", items: jsonObject };
                    listStore = new dojo.data.ItemFileReadStore({ data: data });
                    //Bind data store to grid
                    grid.setStore(listStore);
                }
                else {
                    dojo.byId('upload-status').innerHTML = '<b>Unable to retrieve results</b>' + name;
                }
            }, errorHandler);

        }

        function showResults(featureSet) {
            //submit the GP Task by passing the itemID info the input parameter
            var indexCol = 'Name';
            var imageSelect = dijit.byId('ImageService');
            var idx = imageSelect.attr('value');
            listStore.fetchItemByIdentity({
                identity: idx,
                onItem: function (item) {
                    var classColumn = listStore.getValue(item, "IndexColumn");
                    var url = listStore.getValue(item, 'MapServiceUrl');
                    var params = { 'soap_url': url, 'aoi_feature_set': featureSet, 'index_column': indexCol, 'index_value': district, 'class_column': classColumn };
                    // sample format:
                    // var params = { 'Input_zip_file': '{"itemID": "i7422c29e-6966-4508-a214-7deb8df2b828"}' };
                    gp.submitJob(params, gpJobComplete, statusCallback, errorHandler);
                }
            })
        }

        function statusCallback(jobInfo) {
            dojo.byId('query-status').innerHTML = '<p style="color:red; font-weight:bold;}">' + jobInfo.jobStatus + '</p>';
        }

        function createImageServiceSelect() {
            var sel = dojo.byId('ImageServiceDiv');
            listStore.fetch({
                query: { ServiceType: "Image" },
                onComplete: function (items, request) {
                    // Add empty item so nothing is selected by default
                    var c = dojo.doc.createElement('option');
                    c.innerHTML = '';
                    c.value = '';
                    sel.appendChild(c);
                    dojo.forEach(items, function (item) {
                        //console.log(item);
                        var c = dojo.doc.createElement('option');
                        c.innerHTML = listStore.getValue(item, 'DisplayName');
                        c.value = listStore.getValue(item, 'ID');
                        sel.appendChild(c);
                    });
                }
            });
            var mySelect = new dijit.form.Select({ id: 'ImageService', name: 'ImageService', style: 'width:300px', intermediateChanges: true }, sel)
            mySelect.startup();
        }

        function errorHandler(err) {
            // ERROR 010010: Field not found. Invalid field index obtained. Means the query (indexCol/indexValue) didn't return any features
            console.log('Oops, error: ', err);
            dojo.byId('query-status').innerHTML = '<p style="color:red; font-weight:bold;}">Error message: ' + err.message + '</p>';
        }

        dojo.ready(init);
    </script>

    <style type="text/css">
        #Text1
        {
            width: 479px;
        }
        #serviceUrl
        {
            width: 461px;
        }
    </style>

</head>
<body class="claro">
       <!-- This is the data source that populates the available data sources list --> 
       <asp:ObjectDataSource 
        ID="WebservicesObjectDataSource" 
        runat="server" 
        TypeName="webgis.WebservicesData" 
        SortParameterName="SortColumns"
        EnablePaging="true"
        SelectCountMethod="SelectCount"
        StartRowIndexParameterName="StartRecord"
        MaximumRowsParameterName="MaxRecords" 
        SelectMethod="GetAllServicesJson" >
       </asp:ObjectDataSource>

    <form id="form1" method="post" action="">  
        <span style="font-weight:bold;" class="style3">Image service url: </span>
        <!--<input id="serviceUrl" type="text" 
            value="http://atlas.geog.pdx.edu/arcgis/services/LANDFIRE/Landfire_EVT_Resample/ImageServer" />--><br />
        <div id="ImageServiceDiv"></div>
        <button id="BtnSubmit" dojoType="dijit.form.Button" type="button">Tabulate
            <script type="dojo/method" event="onClick" args="evt">
                tabulateArea();
            </script>
        </button>

        <br />
        <span style="opacity:1;" id="query-status"></span>
        </form>
                <table data-dojo-type="dojox.grid.DataGrid" jsid="grid" id="grid" style="height:300px; width:925px; font-size:small;">
          <thead>
            <tr>
              <th field="value" width="150px" styles="text-align:right;">Value</th>
              <th field="area" width="450px" styles="text-align:right;">Area</th>
              <th field="pctArea" width="150px">Percent Area</th>
            </tr>
          </thead>
        </table>
  

</body>
</html>
