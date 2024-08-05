<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="CreateEvaluationIndex.aspx.vb"
    Inherits="QuickTest.CreateEvaluationIndex" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <script src="../js/jquery-1.7.1.js" type="text/javascript"></script>
    <script src="../js/jquery-ui-1.8.18.js" type="text/javascript"></script>
    <script src="../js/json2.js" type="text/javascript"></script>
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
                $('#NewIndexGroup').dialog({
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

                //------------------------------------------------------------------------------------------------------------------------------------------------
                $('#OpenNewIndex').click(function () {
                    $('#NewIndexName').dialog('open');
                });
                //------------------------------------------------------------------------------------------------------------------------------------------------
                $('#OpenNewIndexGroup').click(function () {
                    $('#NewIndexGroup').dialog('open');
                });
                //------------------------------------------------------------------------------------------------------------------------------------------------
                $('#OpenNewIndexItem').click(function () {
                    $('#NewIndexItem').dialog('open');
                });
                //------------------------------------------------------------------------------------------------------------------------------------------------

                $('#SaveNewIndex').click(function () {
                    var DataNewIndex = $('#txtIndexname').val();
                    OnsaveIndexName(DataNewIndex);
                });

                $('#SaveNewIndexGroup').click(function () {
                    var DataNewIndexGroup = $('#txtIndexGroup').val();
                    var DataNeedSingleChoice;
                    if ($('#chkSingleChoice').attr('checked') == 'checked') {
                        DataNeedSingleChoice = "True"
                    } else {
                        DataNeedSingleChoice = "False"
                    }
                    OnsaveIndexGroupName(DataNewIndexGroup, DataNeedSingleChoice);
                });

                $('#SaveNewIndexItem').click(function () {
                    var DataNewIndexItem = $('#txtIndexItem').val();
                    OnsaveIndexItem(DataNewIndexItem);
                });

        });

    function OnsaveIndexName(DataNewIndex, DataSubjectId) {
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
        function OnsaveIndexGroupName(DataNewIndexGroup, DataNeedSingleChoice, DataSelectedIndexName) {

               $.ajax({
                   type: "POST",
                   url: "<%=ResolveUrl("~")%>testset/CreateEvaluationIndex.aspx/SaveNewIndexGroupCodeBehind",
           data: "{ dataNewIndexGroupName : '" + DataNewIndexGroup + "',NeedSingleChoice: '" + DataNeedSingleChoice + "'}",  //" 
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
       function OnsaveIndexItem(DataNewIndexItem) {
           $.ajax({
               type: "POST",
               url: "<%=ResolveUrl("~")%>testset/CreateEvaluationIndex.aspx/SaveNewIndexItemCodeBehind",
        data: "{ dataNewIndexItemName : '" + DataNewIndexItem + "' }",  //" 
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
                            <input id='OpenNewIndex' type="button" style='float: left; margin-top: 10px;' value='เพิ่มดัชนี +' />
                        </td>
                        <td>
                            <input id='OpenNewIndexGroup' type="button" style='float: left; margin-top: 10px; margin-left: 100px' value='เพิ่มกลุ่มดัชนี +' />
                        </td>
                        <td>
                            <input id='OpenNewIndexItem' type="button" style='float: left; margin-top: 10px; margin-left: 100px' value='เพิ่มกลุ่มตัวชี้วัด +' />
                        </td>
                    </tr>
                    <tr>
                        <td colspan='3' style='text-align: center; padding-top: 30px'>
                            <asp:Label ID="lblWarn" runat="server" Visible="False" ForeColor="Red"></asp:Label>
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
                        <input type='button' value='ยกเลิก' id='CloseNewinDex' onclick='$("#NewIndexName").dialog("close");' style='float: right' />
                        <input type="button" value='ตกลง' id='SaveNewIndex' style='float: right' />
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
                        <input type="button" value='ตกลง' id='SaveNewIndexGroup' style='float: right' />
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
                        <input type="button" value='ตกลง' id='SaveNewIndexItem' style='float: right' />
                    </td>
                </tr>
            </table>

        </div>

        <%------------------------------------------------------------------------------------------------------------------------------------------------------%>

    </form>

</body>
</html>

