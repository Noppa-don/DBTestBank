<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="LocalControl.ascx.vb" Inherits="QuickTestGenUserData.LocalControl" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<style type="text/css">
    .style1
    {
        width: 100%;
    }
</style>
<div>
    <table>
        <tr>
            <td>
                จังหวัด
            </td>
            <td>
                <telerik:RadComboBox ID="CbProvince" runat="server" Width="130px" AutoPostBack="True"
                    MarkFirstMatch="true">
                </telerik:RadComboBox>
            </td>
            <td>
                อำเภอ
            </td>
            <td>
                <telerik:RadComboBox ID="CbDistrict" runat="server" Width="130px" AutoPostBack="True"
                    MarkFirstMatch="true">
                </telerik:RadComboBox>
            </td>
            <td>
                ตำบล
            </td>
            <td>
                <telerik:RadComboBox ID="CbSubDistrict" runat="server" Width="130px" 
                    MarkFirstMatch="true">
                </telerik:RadComboBox>
            </td>
        </tr>
    </table>
</div>