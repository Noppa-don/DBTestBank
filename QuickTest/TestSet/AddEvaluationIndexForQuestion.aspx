<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="AddEvaluationIndexForQuestion.aspx.vb"
    Inherits="QuickTest.AddEvaluationIndexForQuestion" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    
    <script src="../js/jquery-1.7.1.js" type="text/javascript"></script>
    <script src="../js/jquery-ui-1.8.18.js" type="text/javascript"></script>
    <link href="../css/jquery-ui-1.8.18.custom.css" rel="stylesheet" type="text/css" />
    <title></title>

 <telerik:RadCodeBlock ID='RadCodeblock' runat="server">
    <script type="text/javascript">

        $(function () {
            $('#HdDirty').val('0');

            //20240731 เพิ่มปุ่มจัดการตัวชี้วัด
            $('#btnAddNewEvalution').click(function () {
                window.location = '<%=ResolveUrl("~")%>testset/CreateEvaluationIndex.aspx';

            });
            //20240802 เพิ่มปุ่มจัดการอนุมัติตัวชี้วัด
            $('#btnApproveEvalution').click(function () {
                window.location = '<%=ResolveUrl("~")%>testset/ApproveEvaluationIndex.aspx';
            });

            $('#btnPrintEvalution').click(function () {
                $.ajax({
                    type: "POST",
                    url: "<%=ResolveUrl("~")%>TestSet/AddEvaluationIndexForQuestion.aspx/PrintEvalution",
                    data: "{ EI_Id: '1' }",
                    contentType: "application/json; charset=utf-8", dataType: "json",
                    success: function (msg) {
                        if (msg.d == 'Complete') {                                                                                                                                                                          
                        }
                    },
                    error: function myfunction(request, status) {
                        //alert('บางอย่างผิดพลาดโปรดลองอีกครั้ง')
                    }
                });
            });

            
            $('#DivEditEIName').dialog({
                autoOpen: false,
                width:580,
                height:260
            })

            $('#btnCloseDiv').click(function () {
                $('#DivEditEIName').dialog('close');
            });


            $('#btnSaveEditEIName').click(function () {
            if (confirm('ต้องการแก้ไขชื่อดัชนี ?') == true) {
                SaveEditEIName($('#HDEditEIName').val(),$('#HDIsNewEva').val());
            }
            });
        });

        function CheckTabChange(sender, args) {
            if ($('#HdDirty').val() == '1') {
                if (confirm('มีการแก้ไขข้อมูลและยังไม่ได้บันทึก ต้องการเปลี่ยนไปหน้าอื่นใช่หรือไม่ ? ') == true) {
                    $('#HdDirty').val('0');
                }
                else {
                    args.set_cancel(true);
                }
            }
        };

        function SaveLogSelectedTab(sender, args) {
                var tabstrip = $find('<%=RadTabMainEvaluationIndex.ClientID%>');
            var TabName = tabstrip.get_selectedTab().get_text();
                $.ajax({
                    type: "POST",
                    url: "<%=ResolveUrl("~")%>TestSet/AddEvaluationIndexForQuestion.aspx/SaveLogWhenSelectedTab",
                    data: "{ EI_Name: '" + TabName + "' }",
                    contentType: "application/json; charset=utf-8", dataType: "json",
                    success: function (msg) {
                        if (msg.d == 'Complete') {
                         
                        }
                    },
                    error: function myfunction(request, status) {
                        //alert('บางอย่างผิดพลาดโปรดลองอีกครั้ง')
                    }
                });
            }
     

        function SetDirty() {
            $('#HdDirty').val('1');
        };

        function EditEiName(EI_Id, EI_Name,IsNewEva) {
            $('#DivEditEIName').dialog('open');
            $('#HDEditEIName').val(EI_Id);
            $('#HDIsNewEva').val(IsNewEva);
            $('#txtEditEIName').val(EI_Name); 
        };


        function SaveEditEIName(EI_Id,NewEva) {
            var EiName = $('#txtEditEIName').val();
            if (NewEva == 'True') {
                $.ajax({ type: "POST",
	            url: "<%=ResolveUrl("~")%>TestSet/AddEvaluationIndexForQuestion.aspx/SaveEditEIName",
	            data: "{ EI_Id: '" + EI_Id + "',EI_Name:'" + EiName + "' }",
	            contentType: "application/json; charset=utf-8", dataType: "json",   
	            success: function (msg) {
                    if (msg.d !== '') {
                        window.location = msg.d;
                        }
	                },
	                error: function myfunction(request, status)  {
                    //alert('บางอย่างผิดพลาดโปรดลองอีกครั้ง')
	                }
	            });
            }
            else {
                 $.ajax({ type: "POST",
	            url: "<%=ResolveUrl("~")%>TestSet/AddEvaluationIndexForQuestion.aspx/SaveEditEICode",
	            data: "{ EI_Id: '" + EI_Id + "',EI_Code:'" + EiName + "' }",
	            contentType: "application/json; charset=utf-8", dataType: "json",   
	            success: function (msg) {
                    if (msg.d !== '') {
                        
                        window.location = msg.d;
                        }
	                },
	                error: function myfunction(request, status)  {
                    //alert('บางอย่างผิดพลาดโปรดลองอีกครั้ง')
	                }
	            });
            }
               
        }
        
    </script>
 </telerik:RadCodeBlock>

    <style type="text/css">
    .TestClass
    {
        /* background-color:#2DCDFF*/
        background-color:White;
        }
    
    </style>



</head>
<body>
    <form id="form1" runat="server">
    <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
    </telerik:RadScriptManager>
    <div id='MainDiv' style='width: 800px; margin-left: auto; margin-right: auto'>
    
        <table>
                <tr>
                    <td>

                        <input type="button" id="btnPrintEvalution" style='width:120px;height:35px;float: right;' 
                            value='พิมพ์ดัชนีของข้อนี้' />
                         <input type="button" id="btnApproveEvalution" style='width:120px;height:35px;float: right;margin-right: 5px;' value='เพิ่ม/แก้ไขดัชนี' runat="server" visible="false" />
                        <input type="button" id="btnAddNewEvalution" style='width:120px;height:35px;float: right;margin-right: 5px;' value='เพิ่ม/แก้ไขดัชนี' runat="server" visible="true" />
                    </td>

                </tr>

        <tr>
            <td>
                <telerik:RadTabStrip ID="RadTabMainEvaluationIndex"  Skin='Simple' 
                    SelectedIndex='0' runat="server" OnClientTabSelecting="CheckTabChange" OnClientTabSelected="SaveLogSelectedTab" MultiPageID='RadAllMultiPage' Width='790px'>
                    <Tabs>
                    </Tabs>
                </telerik:RadTabStrip>
                     
                <telerik:RadMultiPage ID="RadAllMultiPage" runat="server" SelectedIndex='0' Height='100%' Width='789px'>
                </telerik:RadMultiPage>
            </td>
           

        </tr>
        <tr><td>
         <%--   <asp:Button ID="btnConfirmSave" OnClientClick='return confirm("ต้องการยืนยันการบันทึกดัชนีข้อนี้ใช่หรือไม่ ?")' style='float:right;margin-top:20px;margin-left:20px' runat="server" Text="ยืนยันการบันทึก" Visible="false" />
            <asp:Button ID="btnSave" OnClientClick='return confirm("ต้องการบันทึกดัชนีข้อนี้ใช่หรือไม่ ?")' style='float:right;margin-top:20px' runat="server" Text="บันทึก" />--%>
        </td></tr>
    </table>
    </div>
    <input type="hidden" id='HdDirty' />
 
    <div id='DivEditEIName' title='แก้ไขข้อความดัชนี' style='text-align:center;'>

   <textarea id='txtEditEIName' rows='5' cols='50'></textarea>
    <input type="button" style='width:80px;height:35px;margin-right:40px;' id='btnSaveEditEIName' value='ตกลง' />
    <input type="button" id='btnCloseDiv' style='width:80px;height:35px;margin-top:15px;' value='ยกเลิก' /> 
    </div>
    <input type="hidden" id='HDEditEIName' />
    <input type="hidden" id='HDIsNewEva' />

    </form>
</body>
</html>
