<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="CreateEvaluationIndex.aspx.vb"
    Inherits="QuickTest.CreateEvaluationIndex" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <script src="../js/jquery-1.7.1.js" type="text/javascript"></script>
    <script src="../js/jquery-ui-1.8.18.js" type="text/javascript"></script>
    <link href="../css/jquery-ui-1.8.18.custom.css" rel="stylesheet" type="text/css" />

    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <script type="text/javascript">

            $(function () {
                $('#NewIndexName').dialog({
                    height: 390,
                    width: 497,
                    modal: true,
                    autoOpen: false,
                    draggable: false
                });
                $('#UpdateIndexName').dialog({
                    height: 390,
                    width: 497,
                    modal: true,
                    autoOpen: false,
                    draggable: false
                });

                $('#NewIndexGroup').dialog({
                    height: 313,
                    width: 370,
                    modal: true,
                    autoOpen: false,
                    draggable: false
                });
                $('#UpdateIndexGroup').dialog({
                    height: 313,
                    width: 370,
                    modal: true,
                    autoOpen: false,
                    draggable: false
                });

                $('#NewIndexItem').dialog({
                    height: 313,
                    width: 370,
                    modal: true,
                    autoOpen: false,
                    draggable: false
                });
                $('#UpdateIndexItem').dialog({
                    height: 313,
                    width: 370,
                    modal: true,
                    autoOpen: false,
                    draggable: false
                });
                                      
                //------------------------------------------------------------------------------------------------------------------------------------------------
                $('#btnOpenNewIndex').click(function () {
                    $('#NewIndexName').dialog('open');
                });
                $('#btnEditIndexName').click(function () {
                    console.log('<%=Session("IndexNametxt")%>');
                    $('#txtUpdateIndexGroup').text('<%=Session("IndexNametxt")%>');
                    $('#UpdateIndexName').dialog('open');
                });

                $('#btnOpenNewIndexGroup').click(function () {
                    $('#NewIndexGroup').dialog('open');
                });
                $('#btnEditIndexGroup').click(function () {
                    $('#UpdateIndexGroup').dialog('open');
                });

                $('#btnOpenNewIndexItem').click(function () {
                    $('#NewIndexItem').dialog('open');
                });
                $('#btnEditIndexItem').click(function () {
                    $('#UpdateIndexItem').dialog('open');
                });

                //------------------------------------------------------------------------------------------------------------------------------------------------
                $('#btnSaveNewIndex').click(function () {
                    var DataNewIndex = $('#txtIndexname').val();
                    OnsaveIndexName(DataNewIndex);
                });
                $('#btnUpdateIndexName').click(function () {
                    var DataNewIndex = $('#txtUpdateIndexName').val();
                    OnUpdateIndexName(DataNewIndex);
                });

                $('#btnSaveNewIndexGroup').click(function () {
                    var DataNewIndexGroup = $('#txtIndexGroup').val();
                    OnsaveIndexGroupName(DataNewIndexGroup);
                });
                $('#btnUpdateIndexGroup').click(function () {
                    var DataNewIndexGroup = $('#txtUpdateIndexGroup').val();
                    OnUpdateIndexGroup(DataNewIndexGroup);
                });

                $('#btnSaveNewIndexItem').click(function () {
                    var DataNewIndexItem = $('#txtIndexItem').val();
                    OnsaveIndexItem(DataNewIndexItem);
                });
                $('#btnUpdateIndexItem').click(function () {
                    var DataNewIndexItem = $('#txtUpdateIndexItem').val();
                    OnUpdateIndexItem(DataNewIndexItem);
                });
            });


            //------------------------------------------------------------------------------------------------------------------------------------------------
            function OnsaveIndexName(DataNewIndex) {
                $.ajax({
                    type: "POST",
                    url: "<%=ResolveUrl("~")%>testset/CreateEvaluationIndex.aspx/SaveNewIndexCodeBehind",
                    data: "{ dataNewIndexName: '" + DataNewIndex + "'}",
                    contentType: "application/json; charset=utf-8", dataType: "json",
                    success: function (msg) {
                        if (msg.d != 0) {
                            alert("เพิ่มดัชนีใหม่เรียบร้อยแล้ว")
                            window.location = "CreateEvaluationIndex.aspx"
                        }
                    },
                    error: function myfunction(request, status) {
                        alert('บางอย่างผิดพลาดโปรดลองอีกครั้ง')
                    }
                });
            }

            function OnUpdateIndexName(DataNewIndex) {
                $.ajax({
                    type: "POST",
                    url: "<%=ResolveUrl("~")%>testset/CreateEvaluationIndex.aspx/UpdateNewIndexName",
                    data: "{ dataNewIndexName: '" + DataNewIndex + "'}",
                    contentType: "application/json; charset=utf-8", dataType: "json",
                    success: function (msg) {
                        if (msg.d != 0) {
                            alert("แก้ไขดัชนีใหม่เรียบร้อยแล้ว")
                            window.location = "CreateEvaluationIndex.aspx"
                        }
                    },
                    error: function myfunction(request, status) {
                        alert('บางอย่างผิดพลาดโปรดลองอีกครั้ง')
                    }
                });
            }

            function OnsaveIndexGroupName(DataNewIndexGroup, DataSelectedIndexName) {
                $.ajax({
                    type: "POST",
                    url: "<%=ResolveUrl("~")%>testset/CreateEvaluationIndex.aspx/SaveNewIndexGroupCodeBehind",
                    data: "{ dataNewIndexGroupName : '" + DataNewIndexGroup + "'}",  //" 
                    contentType: "application/json; charset=utf-8", dataType: "json",
                    success: function (msg) {
                        if (msg.d != 0) {
                            alert("เพิ่มกลุ่มดัชนีใหม่เรียบร้อยแล้ว")
                            window.location = "CreateEvaluationIndex.aspx"
                        }
                    },
                    error: function myfunction(request, status) {
                        alert('บางอย่างผิดพลาดโปรดลองอีกครั้ง')
                    }
                });
            }

            function OnUpdateIndexGroup(DataNewIndexGroup) {
                $.ajax({
                    type: "POST",
                    url: "<%=ResolveUrl("~")%>testset/CreateEvaluationIndex.aspx/UpdateIndexGroup",
                    data: "{ dataNewIndexGroupName : '" + DataNewIndexGroup + "'}",
                        contentType: "application/json; charset=utf-8", dataType: "json",
                        success: function (msg) {
                            if (msg.d != 0) {
                                alert("แก้ไขกลุ่มดัชนีเรียบร้อยแล้ว")
                                window.location = "CreateEvaluationIndex.aspx"
                            }
                        },
                        error: function myfunction(request, status) {
                            alert('บางอย่างผิดพลาดโปรดลองอีกครั้ง')
                        }
                    });
                }


                function OnsaveIndexItem(DataNewIndexItem) {
                    $.ajax({
                        type: "POST",
                        url: "<%=ResolveUrl("~")%>testset/CreateEvaluationIndex.aspx/SaveNewIndexItemCodeBehind",
                    data: "{ dataNewIndexItemName : '" + DataNewIndexItem + "' }",
                    contentType: "application/json; charset=utf-8", dataType: "json",
                    success: function (msg) {
                        if (msg.d != 0) {
                            alert("เพิ่มตัวชี้วัดใหม่เรียบร้อยแล้ว")
                            window.location = "CreateEvaluationIndex.aspx"
                        }
                    },
                    error: function myfunction(request, status) {
                        alert('บางอย่างผิดพลาดโปรดลองอีกครั้ง')
                    }
                });
            }

            function OnUpdateIndexItem(DataNewIndexItem) {
                $.ajax({
                    type: "POST",
                    url: "<%=ResolveUrl("~")%>testset/CreateEvaluationIndex.aspx/UpdateIndexItem",
                    data: "{ dataNewIndexItemName : '" + DataNewIndexItem + "' }",
                    contentType: "application/json; charset=utf-8", dataType: "json",
                    success: function (msg) {
                        if (msg.d != 0) {
                            alert("แก้ไขตัวชี้วัดเรียบร้อยแล้ว")
                            window.location = "CreateEvaluationIndex.aspx"
                        }
                    },
                    error: function myfunction(request, status) {
                        alert('บางอย่างผิดพลาดโปรดลองอีกครั้ง')
                    }
                });
            }
        </script>
    </telerik:RadCodeBlock>

    <style type="text/css">
        html .RadListBox .rlbItem {
            border-bottom: 1px solid;
        }

        html .RadListBox .rlbText {
            font-size: large;
            text-align: center;
        }

        html .RadListBox .rlbItem {
            text-align: center;
            height: 40px;
        }

        html .RadListBox .rlbGroupContainer {
            overflow: auto !important;
        }
    </style>

    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server" EnablePartialRendering="true">
        </telerik:RadScriptManager>

        <div id='MainDiv'>
            <asp:Label ID="lblWarn" runat="server" Visible="False" ForeColor="Red"></asp:Label>
            <div id='ContentDiv' style='width: 1100px; margin-left: auto; margin-right: auto'>
                <table>
                    <tr>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    <telerik:RadListBox ID="ListIndexName" runat="server" Style='width: 300px; height: 400px;' AutoPostBack="True">
                                        <HeaderTemplate>
                                            <h1>ชื่อดัชนี</h1>
                                        </HeaderTemplate>
                                    </telerik:RadListBox>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                <ContentTemplate>
                                    <telerik:RadListBox ID="ListIndexGroupName" runat="server" Style='width: 300px; height: 400px; margin-left: 100px'
                                        AutoPostBack="True">
                                        <HeaderTemplate>
                                            <h1>ชื่อกลุ่มดัชนี</h1>
                                        </HeaderTemplate>
                                    </telerik:RadListBox>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                <ContentTemplate>
                                    <telerik:RadListBox ID="ListIndexItem" runat="server" Style='width: 300px; height: 400px; margin-left: 100px'
                                        AutoPostBack="True">
                                        <HeaderTemplate>
                                            <h1>ตัวชี้วัด</h1>
                                        </HeaderTemplate>
                                    </telerik:RadListBox>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <input id='btnOpenNewIndex' type="button" style='float: left; margin-top: 10px;' value='เพิ่มดัชนี' />
                            <input id='btnEditIndexName' type="button" style='margin-top: 10px; margin-left: 30px;' value='แก้ดัชนี' />
                            <asp:Button ID="btnDeleteIndexName" Style='float: right; margin-top: 10px' runat="server" Text="ลบดัชนี" OnClientClick="return confirm('ต้องการลบดัชนีนี้ใช่หรือไม่ ?')" />
                        </td>
                        <td>
                            <input id='btnOpenNewIndexGroup' type="button" style='float: left; margin-top: 10px; margin-left: 100px' value='เพิ่มกลุ่มดัชนี' />
                            <input id='btnEditIndexGroup' type="button" style='margin-top: 10px; margin-left: 20px;' value='แก้กลุ่มดัชนี' />
                            <asp:Button ID="btnDeleteIndexGroupName" Style='float: right; margin-top: 10px' runat="server" Text="ลบกลุ่มดัชนี" OnClientClick='return confirm("ต้องการลบกลุ่มดัชนีใช่หรือไม่ ?")' />
                        </td>
                        <td>
                            <input id='btnOpenNewIndexItem' type="button" style='float: left; margin-top: 10px; margin-left: 100px' value='เพิ่มกลุ่มตัวชี้วัด' />
                            <input id='btnEditIndexItem' type="button" style='margin-top: 10px; margin-left: 30px;' value='แก้ตัวชี้วัด' />
                            <asp:Button ID="btnDeleteIndexItem" Style='float: right; margin-top: 10px' runat="server" Text="ลบตัวชี้วัด" OnClientClick='return confirm("ต้องการลบตัวชี้วัดนี้ใช่หรือไม่ ?")' />
                        </td>
                    </tr>
                </table>
            </div>

        </div>

        <div id='NewIndexName' title='เพิ่มชื่อดัชนีใหม่'>
            <table>

                <tr>
                    <td>
                        <textarea id='txtIndexname' cols='30' rows='5' style='width: 400px'></textarea>
                    </td>
                </tr>
                <tr>
                    <td>
                        <input type='button' value='ยกเลิก' id='btnCloseNewinDex' onclick='$("#NewIndexName").dialog("close");' style='float: right' />
                        <input type="button" value='ตกลง' id='btnSaveNewIndex' style='float: right' />
                    </td>
                </tr>
            </table>
        </div>
        <div id='UpdateIndexName' title='แก้ไขชื่อดัชนี'>
            <table>
                <tr>
                    <td style='border-bottom: 2px solid #9FDA58'>
                        <div id='Div3' style='width: 480px; margin-left: auto; margin-right: auto'>
                            <table>
                                <tr>
                                    <td>
                                        <textarea id='txtUpdateIndexName' cols='30' rows='5' style='width: 400px' runat="server"></textarea>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <input type='button' value='ยกเลิก' id='btnCancelUpdateIndexName' onclick='$("#UpdateIndexName").dialog("close");' style='float: right' />
                                        <input type="button" value='ตกลง' id='btnUpdateIndexName' style='float: right' />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                </tr>
            </table>
        </div>

        <div id='NewIndexGroup' title='เพิ่มกลุ่มดัชนีใหม่'>
            <table>
                <tr>
                    <td>
                        <textarea id='txtIndexGroup' cols='30' rows='5'></textarea>
                    </td>
                </tr>
                <tr>
                    <td>
                        <input type='button' value='ยกเลิก' id='CloseNewIndexGroup' onclick='$("#NewIndexGroup").dialog("close");' style='float: right' />
                        <input type="button" value='ตกลง' id='btnSaveNewIndexGroup' style='float: right' />
                    </td>
                </tr>
            </table>
        </div>
        <div id='UpdateIndexGroup' title='แก้ไขกลุ่มดัชนี'>
            <table>
                <tr>
                    <td>
                        <textarea id='txtUpdateIndexGroup' cols='30' rows='5'></textarea>
                    </td>
                </tr>
                <tr>
                    <td>
                        <input type='button' value='ยกเลิก' id='btnCancelUpdateIndexGroup' onclick='$("#UpdateIndexGroup").dialog("close");' style='float: right' />
                        <input type="button" value='ตกลง' id='btnUpdateIndexGroup' style='float: right' />
                    </td>
                </tr>
            </table>
        </div>

        <div id='NewIndexItem' title='เพิ่มตัวชี้วัดใหม่'>
            <table>
                <tr>
                    <td>
                        <textarea id='txtIndexItem' cols='30' rows='5'></textarea>
                    </td>
                </tr>
                <tr>
                    <td>
                        <input type='button' value='ยกเลิก' id='CloseIndexItem' onclick='$("#NewIndexItem").dialog("close");' style='float: right' />
                        <input type="button" value='ตกลง' id='btnSaveNewIndexItem' style='float: right' />
                    </td>
                </tr>
            </table>

        </div>
        <div id='UpdateIndexItem' title='แก้ไขตัวชี้วัด'>
            <table>
                <tr>
                    <td>
                        <textarea id='txtUpdateIndexItem' cols='30' rows='5'></textarea>
                    </td>
                </tr>
                <tr>
                    <td>
                        <input type='button' value='ยกเลิก' id='btnCancelUpdateIndexItem' onclick='$("#UpdateIndexItem").dialog("close");' style='float: right' />
                        <input type="button" value='ตกลง' id='btnUpdateIndexItem' style='float: right' />
                    </td>
                </tr>
            </table>
        </div>
    </form>

</body>
</html>

