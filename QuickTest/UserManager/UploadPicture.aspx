<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="UploadPicture.aspx.vb" Inherits="QuickTest.UploadPicture" %>
<%@ Register assembly="Telerik.Web.UI" namespace="Telerik.Web.UI" tagprefix="telerik" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .style1
        {
            width: 100%;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    
    <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
    </telerik:RadScriptManager>
    <div>
       <telerik:RadWindowManager ID="RadWindowManager1" VisibleStatusbar="false" runat="server"
                    Skin="Outlook" EnableShadow="true">
                    <Windows>
     
                        <telerik:RadWindow ID="RadDialogAlert" runat="server" Behaviors="move" Modal="false"
                            Height="200" Width="500" EnableShadow="true" VisibleOnPageLoad="false">
                        </telerik:RadWindow>
                    </Windows>
                </telerik:RadWindowManager>
        <table class="style1">
            <tr>
                <td style="text-align: center">
                    <br />
                    <asp:Image ID="Image" runat="server" Height="365px" Width="323px" 
                        style="margin-left: 0px" />
                    <br />
                    <br />
                    <br />
                    <asp:FileUpload ID="fileupload" runat="server" Width="800px" />
                    <br />
                    <br />
                    <asp:Button ID="btnSave" runat="server" Text="บันทึก" style="height: 26px"  />
                    <br />
                    <br />
                </td>
            </tr>
        </table>
    
    </div>
    </form>
</body>
</html>
