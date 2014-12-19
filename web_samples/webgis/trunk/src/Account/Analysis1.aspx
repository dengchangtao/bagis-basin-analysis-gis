<%@ Page Title="Analysis Tool" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="Analysis1.aspx.vb" Inherits="webgis.Analysis1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        /* See psu.css for site styles */        

    </style>

    <script type="text/javascript">
            dojo.require("dojo.parser");
            dojo.require("dijit.form.MultiSelect");
            dojo.require("dojo.dom");
            dojo.require("dijit.form.Button");
            dojo.require("dijit.form.RadioButton");
            dojo.require("dijit.form.Select");
            dojo.require("esri.tasks.AreasAndLengthsParameters");
            dojo.require("esri.tasks.LengthsParameters");
            dojo.require("esri.tasks.query");
            dojo.require("esri.geometry.Geometry");
            dojo.require("dojox.grid.DataGrid");
            dojo.require("dojo.data.ItemFileWriteStore");
            dojo.require("dojo/_base/array");
            dojo.require("esri.geometry.Multipoint");
            dojo.require("dojo.sniff");
            dojo.require("esri.request");
            dojo.require("esri.tasks.Geoprocessor");
            dojo.require("esri.layers.FeatureLayer");
    </script>

    <script type="text/javascript">
        //Global variables
        var store1;
        var listStore;
        var jsonObject;
        var idxMessage = 0;
        var aoiGeometry;
        var selectedLayers;
        var webAoiLayer;
        var timestamp;
        var queryStatement;

        function init() {
            geometryService = new esri.tasks.GeometryService("http://atlas.geog.pdx.edu/arcgis/rest/services/Utilities/Geometry/GeometryServer");
            unzipGp = new esri.tasks.Geoprocessor("http://atlas.geog.pdx.edu/arcgis/rest/services/USACE/Unzip/GPServer/Unzip");

            //identify proxy page to use if the toJson payload to the geometry service is greater than 2000 characters.      
            //If this null or not available the project and lengths operation will not work.  Otherwise it will do a http post to the proxy.
            esriConfig.defaults.io.proxyUrl = "http://atlas.geog.pdx.edu/DotNet/proxy.ashx";
            esriConfig.defaults.io.alwaysUseProxy = false;

            // Retrieve all values from WebservicesObjectDataSource
            var myValues = '<%= WebservicesObjectDataSource.Select %>';
            // Parse values into an ItemFileReadStore
            jsonObject = JSON.parse(myValues);

            var data = { identifier: "ID", label: "DisplayName", items: jsonObject };
            listStore = new dojo.data.ItemFileReadStore({ data: data });
            createAddAllSelect();
            createAoiServiceSelect()

            //disable query button
            dijit.byId('runQuery').set('disabled', true);
            dijit.byId('BtnMap').set('disabled', true);

        }

        function createAddAllSelect() {
            var sel = dojo.byId('AddDataDiv');
            listStore.fetch({
                onComplete: function (items, request) {
                    dojo.forEach(items, function (item) {
                        //console.log(item);
                        // May be faster to query the listStore if this gets slow
                        if (listStore.getValue(item, 'ServiceType') == 'Map') {
                            var c = dojo.doc.createElement('option');
                            c.innerHTML = listStore.getValue(item, 'DisplayName');
                            c.value = listStore.getValue(item, 'ID');
                            sel.appendChild(c);
                        }
                    });
                }
            });

            var myMultiSelect = new dijit.form.MultiSelect({ id: 'AddData', name: 'AddData', size: 10, multiple: 'true', style: 'width:300px' }, sel).startup();
        }

        function createAoiServiceSelect() {
            var sel = dojo.byId('AoiServiceDiv');
            listStore.fetch({
                query: { Shape: "polygon" },
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

            var mySelect = new dijit.form.Select({ id: 'AoiService', name: 'AoiService', style: 'width:300px', intermediateChanges: true }, sel)
            mySelect.startup();
            // connect onChange event to updateQueryText function
            dojo.connect(mySelect, "onChange", "updateQueryText");
        }

        function updateQueryText() {
            var aoiSelect = dijit.byId('AoiService');
            webAoiLayer = aoiSelect.attr('displayedValue');
            var idx = aoiSelect.attr('value');
            if (idx > 0) {
                listStore.fetchItemByIdentity({
                    identity: idx,
                    onItem: function (item) {
                        indexColumn = listStore.getValue(item, "IndexColumn");
                        dojo.byId('query-text').innerHTML = 'WHERE ' + indexColumn + ' = ';
                        dojo.byId('index-column').value = indexColumn;
                        url = listStore.getValue(item, 'MapServiceUrl');
                        var menuTask = new esri.tasks.QueryTask(url);
                        // Set the mapServiceUrl in a hidden field so we can use it to run the query or display the map later
                        dojo.byId('map-service-url').value = url;
                        var menuQuery = new esri.tasks.Query();
                        menuQuery.returnGeometry = false;
                        menuQuery.outFields = [indexColumn];
                        menuQuery.where = indexColumn + " like '%'";
                        menuQuery.orderByFields = [indexColumn];
                        menuTask.execute(menuQuery, buildMenu, errorHandler);
                        manageButtons();
                    }
                })
                // Clear any old options from the query select list
                var querySel = dijit.byId("query-select");
                //alert(querySel.options.length);
                for (var i = querySel.options.length - 1; i >= 0; i--) {
                    querySel.removeOption(i);
                }

            }
        }

        function AddAll(srcDijit, targetDijit) {
            var selectItem1 = dijit.byId(srcDijit);

            // Deselect all and invert to Select all
            selectItem1.set("value", []);
            selectItem1.invertSelection();

            //Move items to right box
            var selectItem2 = dijit.byId(targetDijit);
            selectItem2.addSelected(selectItem1);

            manageButtons();
        }

        function sortSelect(selElem) {
            var tmpAry = new Array();
            for (var i = 0; i < selElem.options.length; i++) {
                tmpAry[i] = new Array();
                tmpAry[i][0] = selElem.options[i].text;
                tmpAry[i][1] = selElem.options[i].value;
            }
            tmpAry.sort();
            while (selElem.options.length > 0) {
                selElem.options[0] = null;
            }
            for (var i = 0; i < tmpAry.length; i++) {
                var op = new Option(tmpAry[i][0], tmpAry[i][1]);
                selElem.options[i] = op;
            }
            manageButtons();
            return;
        }

        function doQuery() {
            // clear the data store
            var emptyData = { identifier: "sid", label: "sid", items: [] };
            store1 = new dojo.data.ItemFileWriteStore({ data: emptyData });
            var btnWeb = dijit.byId('webAoi');
            if (btnWeb.checked == true) {
                // set the search query from the selected record
                var indexCol = dojo.byId('index-column').value;
                var selectValue = dijit.byId('query-select').attr('value')
                queryStatement = indexCol + ' = ' + selectValue;    //global variable to be used by Results1.htm
                var query = new esri.tasks.Query();
                query.returnGeometry = true;
                query.where = indexCol + " = '" + selectValue + "'";
                var queryTask = new esri.tasks.QueryTask(dojo.byId('map-service-url').value);
                queryTask.execute(query, showResults, errorHandler);
            }
            else {
                // assume aoiGeometry was already set when the local file was uploaded
                querySelectedServices();
            }
        }

        // FeatureSet returned
        function showResults(results) {
            if (!results.hasOwnProperty("features") || results.features.length === 0) {
                alert("No Features Returned"); // no features, something went wrong
            }
            else {
                // We assume that only one record was returned
                aoiGeometry = results.features[0].geometry;
                querySelectedServices();
            }
        }

        function querySelectedServices() {
            allOptions = dojo.byId("UseData")
            //@ToDo Add comments here
            dojo.forEach(allOptions.options,
                    function (anOption) {
                        listStore.fetchItemByIdentity({
                            identity: anOption.value,
                            //onItem: function (item) { updateAreaInStore(total.toFixed(0), units, item); }
                            onItem: function (item) {
                                fieldName = listStore.getValue(item, "IndexColumn");
                                var url = listStore.getValue(item, "MapServiceUrl");
                                var queryPoint = new esri.tasks.Query();
                                queryPoint.returnGeometry = true;
                                queryPoint.spatialRelationship = esri.tasks.Query.SPATIAL_REL_CONTAINS;
                                queryPoint.geometry = aoiGeometry;
                                queryPoint.where = fieldName + " like '%'";
                                var queryTaskPoint = new esri.tasks.QueryTask(url);
                                dojo.byId('query-status').innerHTML = '<p style="color:red; font-weight:bold;}">Executing queries ... </p>';
                                idxMessage = idxMessage + 1;
                                queryTaskPoint.execute(queryPoint, function (result) { countResults(anOption.value, anOption.text, url, result); }, errorHandler);
                            }
                        })
                    }
                )
        }

        function buildMenu(results) {
            if (!results.hasOwnProperty("features") || results.features.length === 0) {
                dojo.byId('query-status').innerHTML = "<p style='color:red; font-weight:bold;}'>Invalid service and/or index column. No Aoi's found.</p>";
            }
            else {
                for (var i = 0, il = results.features.length; i < il; i++) {
                    var featureAttributes = results.features[i].attributes;
                    for (var attr in featureAttributes) {
                        var myText = featureAttributes[attr];
                        dijit.byId("query-select").addOption({ label: myText, value: myText });
                    }
                }

            }
        }

        function countResults(currentId, currentName, currentUrl, results) {
            if (!results.hasOwnProperty("features") || results.features.length === 0) {
                //alert("No Features Returned"); // no features, something went wrong
                var gRow = { sid: currentId, name: currentName, url: currentUrl, featureType: 'N/A', total: '0' };
                store1.newItem(gRow);
                //store1.save();
                manageStatusMessage();
            }
            else {
                //Get the geometry for the first feature
                var fGeometry = results.features[0].geometry;
                var featureType = fGeometry.type;
                var units = "";
                var total = 0;
                if (featureType == "point") {
                    total = results.features.length;
                    manageStatusMessage();
                }
                else if (featureType == "polygon") {
                    var geometries = [];
                    for (var i = 0; i < results.features.length; i++) {
                        fGeometry = results.features[i].geometry;
                        geometries.push(fGeometry);
                    }
                    //alert(geometries.length);
                    getArea(geometries, currentId);
                }
                else if (featureType == "multipoint") {
                    var fPoints = 0;
                    for (var i = 0; i < results.features.length; i++) {
                        var feature = results.features[i].geometry;
                        var mp = feature.points;
                        fPoints = fPoints + mp.length;  //count all points in each feature
                    }
                    total = results.features.length + " (" + fPoints + ")";
                    manageStatusMessage();
                }
                else if (featureType == "polyline") {
                    var geometries = [];
                    for (var i = 0; i < results.features.length; i++) {
                        fGeometry = results.features[i].geometry;
                        geometries.push(fGeometry);
                    }
                    //alert(geometries.length);
                    getLength(geometries, currentId);
                }
                //var gRow = new gridRow(currentName, currentUrl, "featureType", "total", "units");
                // set the properties for the new item:
                var gRow = { sid: currentId, name: currentName, url: currentUrl, featureType: featureType, total: total };
                store1.newItem(gRow);
                //store1.save({ onComplete: function() { grid.sort(); } });
                store1.save();

            }
        }

        function getArea(geometry, currentId) {
            //setup the parameters for the areas and lengths operation      
            var areasAndLengthParams = new esri.tasks.AreasAndLengthsParameters();
            //areasAndLengthParams.lengthUnit = esri.tasks.GeometryService.UNIT_METER;
            var aUnits = dijit.byId('areaUnits').attr('value')
            if (aUnits == "Acres") {
                areasAndLengthParams.areaUnit = esri.tasks.GeometryService.UNIT_ACRES;
            }
            else {
                areasAndLengthParams.areaUnit = esri.tasks.GeometryService.UNIT_HECTARES;
            }
            //Geoffrey says planar is correct; See ArcGIS doc for definition of calculationType
            //May depend on projection
            areasAndLengthParams.calculationType = "planar";
            geometryService.simplify(geometry, function (simplifiedGeometries) {
                areasAndLengthParams.polygons = simplifiedGeometries;
                // Assume String representations of units all start with esri prefix
                var units = areasAndLengthParams.areaUnit.substring("esri".length);
                geometryService.areasAndLengths(areasAndLengthParams, function (result) { outputArea(currentId, units, result); });
            });
        }

        function outputArea(cId, units, result) {
            //var result = evtObj.result;
            var total = 0;
            for (var i = 0; i < result.areas.length; i++) {
                total = total + result.areas[i];
            }
            // http://livedocs.dojotoolkit.org/dojo/data/ItemFileWriteStore
            store1.fetchItemByIdentity({
                identity: cId,
                //onItem: function (item) { updateAreaInStore(total.toFixed(0), units, item); }
                onItem: function (item) {
                    //updateAreaInStore(total.toFixed(0), units, item);
                    store1.setValue(item, "total", total.toFixed(0));
                    store1.setValue(item, "units", units);
                    manageStatusMessage();
                }
            })
        }

        function getLength(geometry, currentId) {
            //setup the parameters for the areas and lengths operation
            var lengthParams = new esri.tasks.LengthsParameters();
            var lUnits = dijit.byId('linearUnits').attr('value')
            if (lUnits == "Feet") {
                lengthParams.lengthUnit = esri.tasks.GeometryService.UNIT_FOOT;
            }
            else {
                lengthParams.lengthUnit = esri.tasks.GeometryService.UNIT_METER;
            }
            //Geoffrey says planar is correct; See ArcGIS doc for definition of calculationType
            //May depend on projection
            lengthParams.calculationType = "planar";
            geometryService.simplify(geometry, function (simplifiedGeometries) {
                lengthParams.polylines = simplifiedGeometries;
                // lengthParams stores units as a 4 digit code
                geometryService.lengths(lengthParams, function (result) { outputLength(currentId, lUnits, result); });
            });
        }


        function outputLength(cId, units, result) {
            //var result = evtObj.result;
            var total = 0;
            for (var i = 0; i < result.lengths.length; i++) {
                total = total + result.lengths[i];
            }
            // http://livedocs.dojotoolkit.org/dojo/data/ItemFileWriteStore
            store1.fetchItemByIdentity({
                identity: cId,
                onItem: function (item) {
                    store1.setValue(item, "total", total.toFixed(0));
                    store1.setValue(item, "units", units);
                    manageStatusMessage();
                }
            })
        }

        function manageStatusMessage() {
            idxMessage = idxMessage - 1;
            //alert(idxMessage);
            if (idxMessage == 0) {
                // Clear status message
                dojo.byId('query-status').innerHTML = '';
                // Calculate timestamp
                currentTimestamp();
                // Open window with results
                x = window.open(writeTargetUrl("Results1.htm"));
            }
        }

        function manageButtons() {
            var idx = dijit.byId('AoiService').attr('value');
            if (idx > 0 || aoiGeometry != null) {
                myList = dojo.byId('UseData');
                if (myList.options.length < 1) {
                    dijit.byId('runQuery').set('disabled', true);
                    dijit.byId('BtnMap').set('disabled', true);
                }
                else {
                    dijit.byId('runQuery').set('disabled', false);
                    dijit.byId('BtnMap').set('disabled', false);
                }
            }
            else {
                dijit.byId('runQuery').set('disabled', true);
                dijit.byId('BtnMap').set('disabled', true);
            }
        }

        function showMap() {
            // Copy the selected layer ids into an global array for Map.htm to access
            selectedLayers = [];
            var allItems = dojo.byId("UseData");
            for (var i = 0; i < allItems.options.length; i++) {
                selectedLayers.push(allItems.options[i].value);
            }
            // Add the aoi layer to selectedLayers array if from web service
            var btnWeb = dijit.byId('webAoi');
            if (btnWeb.checked == true) {
                var mapId = dijit.byId('AoiService').attr('value');
                // Only add the aoi layer if it isn't already in the selected layers
                if (selectedLayers.indexOf(mapId) < 0) {
                    selectedLayers.push(mapId);
                }
            }
            // If we are using a map service AOI, need to query for the geometry
            if (btnWeb.checked == true) {
                // set the search query
                var indexCol = dojo.byId('index-column').value;
                var selectValue = dijit.byId('query-select').attr('value')
                var query = new esri.tasks.Query();
                query.returnGeometry = true;
                query.where = indexCol + " = '" + selectValue + "'";
                var queryTask = new esri.tasks.QueryTask(dojo.byId('map-service-url').value);
                queryTask.execute(query, mapResults, errorHandler);
            }
            else {
                // Otherwise we assume the aoiGeometry was set when the local file was uploaded
                // Open window with results
                x = window.open(writeTargetUrl("Map.htm"));
            }
        }

        function mapResults(results) {
            if (!results.hasOwnProperty("features") || results.features.length === 0) {
                dojo.byId('query-status').innerHTML = '<p style="color:red; font-weight:bold;}">Unable to locate AOI</p>'; // no features, something went wrong
            }
            else {
                // We assume that only one record was returned
                aoiGeometry = results.features[0].geometry;

                // Open window with results
                x = window.open(writeTargetUrl("Map.htm"));
            }
        }

        function manageAoiLayers(layerType) {
            if (layerType == 'localAoi') {
                dojo.byId('web-aoi-div').style.display = 'none';
                dojo.byId('local-aoi-div').style.display = 'block';
            }
            else {
                dojo.byId('web-aoi-div').style.display = 'block';
                dojo.byId('local-aoi-div').style.display = 'none';
            }
        }

        function setLocalFileName() {
            var fileName = dojo.byId('inFile').value.toLowerCase();
            if (dojo.sniff("ie")) { //filename is full path in IE so extract the file name              
                var arr = fileName.split("\\");
                fileName = arr[arr.length - 1];
            }
            if (fileName.indexOf(".zip") !== -1) {//is file a zip - if not notify user            
                generateFeatureCollection(fileName);
            }
            else {
                dojo.byId('upload-status').innerHTML = '<p style="color:red">Add shapefile as .zip file</p>';
            }
        }

        function generateFeatureCollection(fileName) {
            var name = fileName.split(".");
            //Chrome and IE add c:\fakepath to the value - we need to remove it            
            //See this link for more info: http://davidwalsh.name/fakepath            
            name = name[0].replace("c:\\fakepath\\", "");
            dojo.byId('upload-status').innerHTML = '<b>Loading </b>' + name;
            //http://forums.arcgis.com/threads/82604-Upload-File-to-ArcGIS-Server
            var gpUploadURL = "http://atlas.geog.pdx.edu/arcgis/rest/services/USACE/UnzipTest/GPServer/uploads/upload";
            //upload the zip file and get back the itemID (via uploadSucceeded)
            var requestHandle = esri.request({
                url: gpUploadURL,
                form: dojo.byId("form1"),
                content: { f: "json" },
                load: uploadSucceeded,
                error: errorHandler
            });
        }

        function uploadSucceeded(response) {
            var itemID = response["item"].itemID;
            dojo.byId('query-status').innerHTML = '<p style="color:red; font-weight:bold;}">Upload succeeded</p>';
            //submit the GP Task by passing the itemID info the input parameter
            var params = { 'Input_file': '{"itemID":"' + itemID + '"}' };
            // sample format:
            // var params = { 'Input_file': '{"itemID": "i7422c29e-6966-4508-a214-7deb8df2b828"}' };
            unzipGp.submitJob(params, gpJobComplete, statusCallback, function (error) {
                alert(error);
            });
        }

        function writeTargetUrl(aPage) {
            var loc = window.location.href;
            if (loc.indexOf("Account") > -1) {
                return "./" + aPage;
            }
            else {
                return loc + "Account/" + aPage;
            }
        }

        function gpJobComplete(jobInfo) {
            dojo.byId('query-status').innerHTML = '<p style="color:red; font-weight:bold;}"></p>';
            unzipGp.getResultData(jobInfo.jobId, "Aoi_", function (results) {
                var localFeatureSet = results.value;
                if (localFeatureSet.features.length > 0) {
                    // Always get the first one; Assume there is only one
                    aoiGeometry = localFeatureSet.features[0].geometry;
                    dojo.byId('upload-status').innerHTML = '<b>Aoi set to local feature</b>' + name;
                    manageButtons();
                }
                else {
                    dojo.byId('upload-status').innerHTML = '<b>Unable to retrieve aoi feature</b>' + name;
                }
            }, errorHandler);
        }

        function currentTimestamp() {
            // http://stackoverflow.com/questions/1531093/how-to-get-current-date-in-javascript
            var rightNow = new Date();
            var month = rightNow.getMonth() + 1; //January is 0!
            if (month < 10) {
                month = '0' + month;
            }
            var day = rightNow.getDate();
            if (day < 10) {
                day = '0' + day;
            }
            var year = rightNow.getFullYear();
            var hour = rightNow.getHours();
            if (hour > 12) {
                hour = hour - 12;
            }
            if (hour < 10) {
                hour = '0' + hour;
            }
            var curMinute = rightNow.getMinutes();
            if (curMinute < 10) {
                curMinute = '0' + curMinute;
            }
            var meridiem = " PM";
            if (rightNow.getHours() < 12 || rightNow.getHours() > 23) {
                meridiem = " AM"
            }
            //timestamp format: 08/27/2014, 10:25 AM
            timestamp = month + '/' + day + '/' + year + ", " + hour + ":" + curMinute + meridiem;
        }

        function statusCallback(jobInfo) {
            dojo.byId('query-status').innerHTML = '<p style="color:red; font-weight:bold;}">' + jobInfo.jobStatus + '</p>';
        }

        function errorHandler(err) {
            console.log('Oops, error: ', err);
            dojo.byId('query-status').innerHTML = '<p style="color:red; font-weight:bold;}">Error: ' + err + '</p>';
        }

        dojo.ready(init);

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
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

    <p class="style1">Analysis Tool</p>

    <div id="menu" align="left" style="padding: 5px">
    <form id="form1" method="post" action="Edit.aspx">
        <table class="style2">
            <tr>
                <td>
                    <span class="style3">Available data</span>
                    <div id="AddDataDiv"></div>
                 </td>
        <td valign="middle" style="width: 200px;">
             <button id="services_add" dojoType="dijit.form.Button" type="button" class="editServices"><div style="width:60px">&gt;</div>
                <script type="dojo/method" event="onClick" args="evt">
                    dijit.byId("UseData").addSelected(dijit.byId("AddData"));
                    sortSelect(dojo.byId("UseData"));
                </script>
            </button>
            <br/>
            <button id="services_remove" dojoType="dijit.form.Button" type="button"><div style="width:60px">&lt;</div>
                <script type="dojo/method" event="onClick" args="evt">
                    dijit.byId("AddData").addSelected(dijit.byId("UseData"));
                    sortSelect(dojo.byId("AddData"));
                </script>
            </button>
            <br/>
            <button id="services_add_all" dojoType="dijit.form.Button" type="button"><div style="width:60px">&gt;&gt;</div>
                <script type="dojo/method" event="onClick" args="evt">
                    AddAll('AddData','UseData');
                </script>
            </button>
            <br/>
            <button id="services_remove_all" dojoType="dijit.form.Button" type="button"><div style="width:60px">&lt;&lt;</div>
                <script type="dojo/method" event="onClick" args="evt">
                    AddAll('UseData','AddData');
                </script>
            </button>
            <br/>
            </td>
            <td>
                <span class="style3">Data in analysis</span>
                <select name="UseData" id="UseData" dojoType="dijit.form.MultiSelect" size="10" multiple="true" style="width: 300px; font-family: Arial, Helvetica, sans-serif;">

                </select>
            </td>
            <td style="padding-left:10px; vertical-align:top">
                <span class="style3">Set AOI</span>
                <br />
                <input type="radio" data-dojo-type="dijit/form/RadioButton" name="AoiSource" 
                    id="localAoi" value="local" style="font-family:Arial, Helvetica, sans-serif;" onclick="manageAoiLayers('localAoi');"/ 
                   /> 
                <label for="local">Local</label>
                <br />
                <input type="radio" data-dojo-type="dijit/form/RadioButton" name="AoiSource" onclick="manageAoiLayers('webAoi');"
                    id="webAoi" value="web" checked="checked" lang="aa"/> 
                <label for="local">Web</label>
                <br /><br />
                <div id="local-aoi-div" style="display:none">
                    <span class="style3">Add File</span>                      
                    <input type="file" name="file" id="inFile" onchange="setLocalFileName()" />
                    <span class="file-upload-status" style="opacity:1;" id="upload-status"></span>              
                    <div id="fileInfo"> </div>
                </div>
                <div id="web-aoi-div">
                    <span class="style3">AOI map service:</span>  
                    <br />
                        <div id="AoiServiceDiv"></div>
                    <br /><br />
                    <input id="index-column" type="hidden" />
                    <input id="map-service-url" type="hidden" />
                    <span class="style3" style="opacity:1;" id="query-text"></span><br />
                    <select name="query-select" id="query-select" dojoType="dijit.form.Select" style="width: 200px; font-family: Arial, Helvetica, sans-serif;">
                    </select>
                </div>
                <br /><br />
               <button id="runQuery" dojoType="dijit.form.Button" type="button">Run query
                <script type="dojo/method" event="onClick" args="evt">
                    doQuery();
                </script>
                </button>
                <button id="BtnMap" dojoType="dijit.form.Button" type="button">Show map
                    <script type="dojo/method" event="onClick" args="evt">
                        showMap();
                    </script>
                </button>
               </td>
            </tr>
            <tr>
                <td colspan="2">Area units: 
                <select name="areaUnits" id="areaUnits" dojoType="dijit.form.Select" style="width: 100px; font-family: Arial, Helvetica, sans-serif;">
                    <option value="Acres" selected="selected">Acres</option>
                    <option value="Hectares">Hectares</option>
                </select>

                </td>
                <td></td>
                <td></td>
            </tr>
            <tr>
                <td colspan="2">Linear units: 
                <select name="linearUnits" id="linearUnits" dojoType="dijit.form.Select" style="width: 100px; font-family: Arial, Helvetica, sans-serif;">
                    <option value="Feet" selected="selected">Feet</option>
                    <option value="Meters">Meters</option>
                </select>
                </td>
                <td></td>
                <td></td>
            </tr>            
        </table>  
        </form>
         <br />
        <span style="opacity:1;" id="query-status"></span>
    </div>
</asp:Content>
