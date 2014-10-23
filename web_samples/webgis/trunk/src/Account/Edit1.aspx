<%@ Page Title="Edit Webservices" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="Edit1.aspx.vb" Inherits="webgis.Edit1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    <script runat="server">
    
        Sub Page_Load(sender As Object, e As System.EventArgs) Handles MyBase.Load
            'Add client-side script to validate map service with ArcGIS Javascript API
            'These scripts always return false so that the javascript can trigger the final submission
            'after the ArcGIS Javascript completes
            BtnUpdate.Attributes.Add("onclick", "return validateExistingService();")
            BtnInsert.Attributes.Add("onclick", "return validateNewService();")
            'This javascript method sets the password in a hidden field
            BtnUpdatePassword.Attributes.Add("onclick", "return setPassword();")
            UserNameBox.Attributes.Add("onblur", "return userNameChanged();")
            Msg.Text = ""
         End Sub
        
        Sub BtnCommitUpdate_Click(ByVal sender As Object, _
                          ByVal e As EventArgs) Handles BtnCommitUpdate.Click
            Page.Validate()
            If Page.IsValid Then
                WebservicesDataSource.Update()
            Else

            End If
        End Sub
    
        Sub BtnDelete_Click(ByVal sender As Object, _
                      ByVal e As EventArgs) Handles BtnDelete.Click
            WebservicesDataSource.Delete()
        End Sub
    
        Sub BtnCommitInsert_Click(ByVal sender As Object, _
                  ByVal e As EventArgs) Handles BtnInsert.Click
            Page.Validate()
            If Page.IsValid Then
                WebservicesDataSource.Insert()
            End If
        End Sub
        
        Sub ListBox1_OnSelectedIndexChanged(sender As Object, e As EventArgs) Handles ListBox1.SelectedIndexChanged
            If ListBox1.SelectedItem.Value > 0 Then
                DetailsObjectDataSource.SelectParameters("Id").DefaultValue = _
                ListBox1.SelectedItem.Value.ToString()
                ' Be sure the text boxes are initialized with
                ' data from the currently selected webservice.
                Dim objects() As Object = DetailsObjectDataSource.Select()
                Dim ws As webgis.Webservice = TryCast(objects(0), webgis.Webservice)
                If Not ws Is Nothing Then
                    DisplayNameBox.Text = ws.DisplayName
                    MapServiceUrlBox.Text = ws.MapServiceUrl
                    IndexColumnBox.Text = ws.IndexColumn
                    DateModifiedBox.Text = ws.DateModifiedText
                    UserNameBox.Text = ws.UserName
                    ShapeBox.Text = ws.Shape
                    ShapeBoxDisplay.Text = ws.Shape
                    If Not String.IsNullOrEmpty(UserNameBox.Text) Then
                        BtnUpdatePassword.Style.Add("display", "inline")
                    Else
                        BtnUpdatePassword.Style.Add("display", "none")
                    End If
                    If String.IsNullOrEmpty(ws.Password) Then
                        HasPassword.Text = False
                    Else
                        HasPassword.Text = True
                    End If
                    BtnUpdate.Enabled = True
                    BtnDelete.Enabled = True
                    BtnClear.Enabled = True
                Else
                    BtnUpdate.Enabled = False
                    BtnDelete.Enabled = False
                    BtnClear.Enabled = False
                End If
            End If
        End Sub

        Sub DetailsObjectDataSource_OnInserted(sender As Object, e As ObjectDataSourceStatusEventArgs) Handles DetailsObjectDataSource.Inserted
            DetailsObjectDataSource.SelectParameters("Id").DefaultValue = _
          e.ReturnValue.ToString()
        End Sub

        Sub WebservicesDataSource_OnUpdated(sender As Object, e As ObjectDataSourceStatusEventArgs) Handles WebservicesDataSource.Updated
            If CInt(e.ReturnValue) = 0 Then
                Msg.Text = "Service was not updated. Please try again."
            Else
                ListBox1.DataBind()
                ResetTextboxes()
            End If
        End Sub
    
        Sub WebservicesDataSource_OnDeleted(sender As Object, e As ObjectDataSourceStatusEventArgs) Handles WebservicesDataSource.Deleted
            If CInt(e.ReturnValue) = 0 Then
                Msg.Text = "Service was not deleted. Please try again."
            Else
                ListBox1.DataBind()
                ResetTextboxes()
            End If
        End Sub
    
        Sub WebservicesDataSource_OnInserted(sender As Object, e As ObjectDataSourceStatusEventArgs) Handles WebservicesDataSource.Inserted
            If CInt(e.ReturnValue) = 0 Then
                Msg.Text = "Service was not added. Please try again."
            Else
                ListBox1.DataBind()
                ResetTextboxes()
            End If
        End Sub

    Sub ResetTextboxes()
        DisplayNameBox.Text = Nothing
        MapServiceUrlBox.Text = Nothing
        IndexColumnBox.Text = Nothing
        DateModifiedBox.Text = Nothing
        UserNameBox.Text = Nothing
        BtnUpdatePassword.Style.Add("display", "none")
            ShapeBox.Text = Nothing
            ShapeBoxDisplay.Text = Nothing
        ListBox1.ClearSelection()
        BtnDelete.Enabled = False
        BtnUpdate.Enabled = False
        BtnClear.Enabled = False
    End Sub
    
    Protected Sub UrlValidator_ServerValidate(source As Object, args As System.Web.UI.WebControls.ServerValidateEventArgs)
        Try
            Dim url As String = MapServiceUrlBox.Text
            'If the url is empty the required validation will catch it
            If Not String.IsNullOrEmpty(url) Then
                Dim req As System.Net.HttpWebRequest = TryCast(System.Net.WebRequest.Create(url), System.Net.HttpWebRequest)
                req.Timeout = 30000
                Dim resp As System.Net.HttpWebResponse = TryCast(req.GetResponse, System.Net.HttpWebResponse)
                If resp.StatusCode = System.Net.HttpStatusCode.OK Then
                    args.IsValid = True
                Else
                    args.IsValid = False
                End If
            End If
        Catch ex As Exception
            args.IsValid = False
        End Try
    End Sub

    Protected Sub BtnClear_Click(sender As Object, e As System.EventArgs)
        ResetTextboxes()
    End Sub
  
</script>

<script type="text/javascript">

    dojo.require("dojo.parser");
    dojo.require("dojo.dom");
    dojo.require("esri.tasks.query");

    function init() {
        //identify proxy page to use if the toJson payload to the geometry service is greater than 2000 characters.      
        //If this null or not available the project and lengths operation will not work.  Otherwise it will do a http post to the proxy.
        esriConfig.defaults.io.proxyUrl = "http://atlas.geog.pdx.edu/proxy/proxy.ashx"
        esriConfig.defaults.io.alwaysUseProxy = false;

    }

    function validateNewService() {
        return validateService('New');
    }

    function validateExistingService() {
        return validateService('Existing');
    }

    function validateService(sType) {
        // Check for password if user name is not empty
        var uName = dojo.byId("<%= UserNameBox.ClientID %>").value
        var pWord = dojo.byId("<%= HasPassword.ClientID %>").value
        if (uName && pWord == false) {
            resp = confirm("You entered a user name but no password, do you wish to continue? ");
            if (resp == false) {
                return;
            }
        }
        var url = dojo.byId("<%= MapServiceUrlBox.ClientID %>").value
        var indexColumn = dojo.byId("<%= IndexColumnBox.ClientID %>").value
        queryTask = new esri.tasks.QueryTask(url);
        query = new esri.tasks.Query();
        query.returnGeometry = true;
        query.where = indexColumn + " like '%'";
        dojo.byId('<%= Msg.ClientID %>').innerHTML = '<p">Processing ...</p>';
        queryTask.execute(query, function (result) { processResults(sType, result); }, errorHandler);
        return false;
    }

    function processResults(sType, results) {
        if (!results.hasOwnProperty("features") || results.features.length === 0) {
            dojo.byId('<%= Msg.ClientID %>').innerHTML = '<p>Could not validate map service. Either the url or index column is invalid</p>';
        }
        else {
            //Get the geometry for the first feature
            var fGeometry = results.features[0].geometry;
            var featureType = fGeometry.type;
            dojo.byId('<%= ShapeBox.ClientID %>').value = featureType;
            //alert(dojo.byId('<%= ShapeBox.ClientID %>').value);
        }
        if (sType == 'Existing') {
            dojo.byId('<%= BtnCommitUpdate.ClientID %>').click();
        }
        else {
            dojo.byId('<%= BtnCommitInsert.ClientID %>').click();
        }
        dojo.byId('<%= Msg.ClientID %>').innerHTML = '';
    }

    function setPassword() {
        var pword = prompt("Please enter a password. You must click the New or Update button to permanently save the password: ");
        if (pword != null) {
            dojo.byId('<%= PasswordBox.ClientID %>').value = pword.trim();
            dojo.byId('<%= HasPassword.ClientID %>').value = true;
        }
        else {
            dojo.byId('<%= PasswordBox.ClientID %>').value = null;
            dojo.byId('<%= HasPassword.ClientID %>').value = false;
        }
    }

    //Client-side behavior to manage the password button and fields
    function userNameChanged() {
        var uName = dojo.byId('<%= UserNameBox.ClientID %>').value
        if (uName) {
            //alert('uName is populated');
            uName = uName.trim();
            if (uName) {
                dojo.byId('<%= BtnUpdatePassword.ClientID %>').style.display = 'inline';
            }
            else {
                disablePassword();
            }
        }
        else {
            //alert('uName is empty');
            disablePassword();
        }
    }

    function disablePassword() {
        dojo.byId('<%= BtnUpdatePassword.ClientID %>').style.display = 'none';
        dojo.byId('<%= PasswordBox.ClientID %>').value = '';
        dojo.byId('<%= HasPassword.ClientID %>').value = false;
    }

    function errorHandler(err) {
        console.log('Oops, error: ', err);
        dojo.byId('<%= Msg.ClientID %>').innerHTML = '<p>Error: ' + +'</p>';
    }


    dojo.ready(init);
</script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
      <p class="style1">Edit Webservices</p>
      <asp:Label id="Msg" runat="server" CssClass="validStyle" />

      <asp:ObjectDataSource 
        ID="WebservicesDataSource" 
        runat="server" 
        TypeName="webgis.WebservicesData" 
        SortParameterName="SortColumns"
        EnablePaging="true"
        StartRowIndexParameterName="StartRecord"
        MaximumRowsParameterName="MaxRecords" 
        SelectMethod="GetAllServices"
        UpdateMethod="UpdateWebService"
        DeleteMethod="DeleteWebservice"
        InsertMethod="InsertWebservice"
        OnUpdated="WebservicesDataSource_OnUpdated"
        OnDeleted="WebservicesDataSource_OnDeleted"
        OnInserted="WebservicesDataSource_OnInserted">
        <UpdateParameters>
            <asp:ControlParameter name="Id" controlid="ListBox1" propertyname="SelectedValue" />
            <asp:ControlParameter ControlID="DisplayNameBox" Name="DisplayName" Type="string" />
            <asp:ControlParameter ControlID="MapServiceUrlBox" Name="MapServiceUrl" Type="string" />
            <asp:ControlParameter ControlID="IndexColumnBox" Name="IndexColumn" Type="string" />
            <asp:ControlParameter ControlID="UserNameBox" Name="UserName" Type="string" />
            <asp:ControlParameter ControlID="HasPassword" Name="HasPassword" Type="boolean" />
            <asp:ControlParameter ControlID="PasswordBox" Name="Password" Type="string" />
            <asp:ControlParameter ControlID="ShapeBox" Name="Shape" Type="string" />
        </UpdateParameters>
        <DeleteParameters>
            <asp:ControlParameter name="Id" controlid="ListBox1" propertyname="SelectedValue" />
         </DeleteParameters>
         <InsertParameters>
            <asp:ControlParameter ControlID="DisplayNameBox" Name="DisplayName" Type="string" />
            <asp:ControlParameter ControlID="MapServiceUrlBox" Name="MapServiceUrl" Type="string" />
            <asp:ControlParameter ControlID="IndexColumnBox" Name="IndexColumn" Type="string" />
            <asp:ControlParameter ControlID="UserNameBox" Name="UserName" Type="string" />
            <asp:ControlParameter ControlID="PasswordBox" Name="Password" Type="string" />
            <asp:ControlParameter ControlID="ShapeBox" Name="Shape" Type="string" />
        </InsertParameters>
      </asp:ObjectDataSource>

      <asp:ObjectDataSource 
        ID="DetailsObjectDataSource" 
        runat="server" 
        TypeName="webgis.WebservicesData" 
        DataObjectTypeName="webgis.Webservice"
        SelectMethod="GetWebservice"
        InsertMethod="InsertService"
        DeleteMethod="DeleteService"
        OnInserted="DetailsObjectDataSource_OnInserted" >
        <SelectParameters>
          <asp:Parameter Name="Id" Type="Int32" DefaultValue="-1"/>
        </SelectParameters>
       </asp:ObjectDataSource>

      <table style="width:900px;">
        <tr>
            <td style="width:30%;">
            <span class="style3">Current services</span>
            <asp:ListBox    
            ID="ListBox1"  
            runat="server"  
            DataSourceID="WebservicesDataSource"  
            DataTextField="DisplayName"  
            DataValueField="Id" Width="200px" Rows="10"
            OnSelectedIndexChanged = "ListBox1_OnSelectedIndexChanged" AutoPostBack="True" >
        </asp:ListBox>  
            </td>
            <td style="width:70%; vertical-align:top; padding-left:10px;">
                <table class="dijitTabContainerNoLayout">
                    <tr>
                        <td>
            <span class="style3">Details</span></td>
                    </tr>
                    <tr>
                        <td>
            <asp:Label id="label" AssociatedControlId="DisplayNameBox" Text="Display name  " 
                    runat="server" CssClass="style3" />
            <asp:textbox id="DisplayNameBox" runat="server" text='' Width="200px" />
            <asp:RequiredFieldValidator ID="DisplayNameRequired" runat="server" 
                ControlToValidate="DisplayNameBox" CssClass="validStyle" 
                ErrorMessage="Required" ValidationGroup="AllValidators" Display="Dynamic"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
            <asp:Label id="label2" AssociatedControlId="MapServiceUrlBox" Text="Map service url  " 
                    runat="server" CssClass="style3" />
            <asp:textbox id="MapServiceUrlBox" runat="server" text='' Width="450px" />
                <asp:CustomValidator ID="UrlValidator" runat="server" 
                    ControlToValidate="MapServiceUrlBox" CssClass="validStyle" 
                    ErrorMessage="Cannot connect to url" 
                    onservervalidate="UrlValidator_ServerValidate" 
                    ValidationGroup="AllValidators" ValidateEmptyText="True" 
                    Display="Dynamic"/>
            &nbsp;<asp:RequiredFieldValidator ID="MapServiceUrlRequired" runat="server" 
                ControlToValidate="MapServiceUrlBox" CssClass="validStyle" 
                ErrorMessage="Required" ValidationGroup="AllValidators" Display="Dynamic"/>
                        </td>
                    </tr>
                    <tr>
                        <td>
            <asp:Label id="label3" AssociatedControlId="IndexColumnBox" Text="Index column  " 
                    runat="server" CssClass="style3" />
            <asp:textbox id="IndexColumnBox" runat="server" text='' /> 
            <asp:RequiredFieldValidator ID="IndexColumnValidator" runat="server" 
                ControlToValidate="IndexColumnBox" CssClass="validStyle" 
                ErrorMessage="Required" ValidationGroup="AllValidators" Display="Dynamic"/>
                        </td>
                    </tr>
                    <tr>
                        <td>
            <asp:Label id="label6" AssociatedControlId="ShapeBoxDisplay" Text="Feature type  " 
                    runat="server" CssClass="style3" />
            <asp:textbox id="ShapeBoxDisplay" runat="server"
                    Enabled="False" style="color:GrayText;"/> 
                        </td>
                    </tr>
                    <tr>
                        <td>
            <asp:Label id="label4" AssociatedControlId="UserNameBox" Text="Service user name  " 
                    runat="server" CssClass="style3" />
            <asp:textbox id="UserNameBox" runat="server" text='' />
             &nbsp;&nbsp;&nbsp;
            <asp:Button ID="BtnUpdatePassword" 
                Text="Update Password"
                runAt ="server" Width="150px" 
                CausesValidation="False"
                style="display:none;">
            </asp:Button>

                        </td>
                    </tr>
                    <tr>
                        <td>
            <asp:Label id="label1" AssociatedControlId="DateModifiedBox" Text="Date modified  " 
                    runat="server" CssClass="style3" />
            <asp:textbox id="DateModifiedBox" runat="server" text='' ReadOnly="True" 
                    Enabled="False" />
                        </td>
                    </tr>
                    <tr>
                        <td>


                <asp:Button ID="BtnInsert" 
                            Text="Create"
                            runAt ="server" Width="60px">
                </asp:Button>
                                 &nbsp; &nbsp;
                <asp:Button ID="BtnUpdate" 
                            Text="Update"
                            runAt ="server" Width="60px" 
                            Enabled="False">
                </asp:Button>
                           &nbsp; &nbsp;
                <asp:Button ID="BtnClear" 
                            Text="Clear"
                            runAt ="server" Width="60px" 
                            OnClick="BtnClear_Click" Enabled="False">
                </asp:Button>
                    &nbsp; &nbsp;
                <asp:Button ID="BtnDelete" 
                            Text="Delete"
                            CommandName="Edit"
                            runAt ="server" Width="60px" 
                            OnClick="BtnDelete_Click" Enabled="False" CausesValidation="False">
                </asp:Button>

                        </td>
                    </tr>
                </table>
            <asp:textbox id="PasswordBox" runat="server" text='' style="display:none;"/>
            <asp:textbox id="HasPassword" runat="server" text='' style="display:none;"/>
            <asp:textbox id="ShapeBox" runat="server" text='' style="display:none;"/>
               <asp:Button ID="BtnCommitUpdate"
                            CommandName="Edit"
                            runAt ="server" 
                            OnClick="BtnCommitUpdate_Click"
                            style="display:none;"/>
           
               <asp:Button ID="BtnCommitInsert"
                            CommandName="Insert"
                            runAt ="server" 
                            OnClick="BtnCommitInsert_Click"
                            style="display:none;"/>
                </td>
        </tr>
    </table>
</asp:Content>
