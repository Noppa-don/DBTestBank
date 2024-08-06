<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ApproveEvaluationIndex.aspx.vb" Inherits="QuickTest.ApproveEvaluationIndex" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <script src="../js/jquery-1.7.1.js" type="text/javascript"></script>
    <script src="../js/jquery-ui-1.8.18.js" type="text/javascript"></script>
    <link href="../css/jquery-ui-1.8.18.custom.css" rel="stylesheet" type="text/css" />
    <title></title>

    <style type="text/css">
        .dtChild {
            padding: 20px;
            width: 80%;
        }

        .dtContainer {
            height: 600px;
            overflow-y: scroll;
        }

        .dtDetail1 {
            display: flex;
            background-color: #fde1c5;
        }

        .dtDetail2 {
            display: flex;
            background-color: #fff4eb;
        }

        .btn {
            border-style: solid;
            border-radius: 1em;
            line-height: 20px;
            padding: 10px;
            height: 20px;
            border-color: #f7891b;
            cursor: pointer;
            background-color: #ffffff;
            box-shadow: 0 4px 6px -1px #00000038, 0 2px 4px -1px #00000038;
            width: fit-content;
            margin: auto;
            text-align: center;
            border-width: 1px;
            margin-right: 15px;
            margin-top: 8px;
        }

        .ui-dialog {
            width: 290px !important;
            height: 160px !important;
        }

        #dialogApprove .btn, #dialogNotApprove .btn {
            margin-right: 30px !important;
            margin-top: 20px !important;
            border-radius: 0.5em !important;
        }

        #dialogApprove, #dialogNotApprove {
            height: 70px !important;
        }

        .ui-widget-header {
            border: #bb6611;
            background: #bb6611;
            color: #FFFFFF;
            font-weight: bold;
        }
    </style>

    <script type="text/javascript">

        $(function () {

            $('#dialogApprove').dialog({
                height: 200,
                width: 100,
                modal: true,
                resizable: false,
                autoOpen: false,
                draggable: false
            });
            $('#dialogNotApprove').dialog({
                height: 200,
                width: 100,
                modal: true,
                resizable: false,
                autoOpen: false,
                draggable: false
            });
            $('.btnApproved').click(function () {
                var eiid = $(this).attr("Eiid");
                var AEId = $(this).attr("AEId");
                $('#dialogApprove .btnOK').attr('eiid', eiid)
                $('#dialogApprove .btnOK').attr('AEId', AEId)
                $('#dialogApprove').dialog('open');
            });
            $('.btnNotApproved').click(function () {
                var eiid = $(this).attr("Eiid");
                var AEId = $(this).attr("AEId");
                $('#dialogNotApprove .btnOK').attr('eiid', eiid)
                $('#dialogNotApprove .btnOK').attr('AEId', AEId)
                $('#dialogNotApprove').dialog('open');
            });

            $('#dialogApprove .btncancel').click(function () {
                $('#dialogApprove').dialog('close');
            });

            $('#dialogNotApprove .btncancel').click(function () {
                $('#dialogNotApprove').dialog('close');
            });

            $('#dialogApprove .btnOK').click(function () {
                //Approve
                var eiid = $(this).attr("Eiid");
                var AEId = $(this).attr("AEId");
                SaveApproveStatus('1', eiid, AEId);
            });
            $('#dialogNotApprove .btnOK').click(function () {
                //NotApprove
                var eiid = $(this).attr("Eiid");
                var AEId = $(this).attr("AEId");
                SaveApproveStatus('2', eiid, AEId);
            });

            function SaveApproveStatus(ApproveType, EIId, AEId) {
                $.ajax({
                    type: "POST",
                    url: "<%=ResolveUrl("~")%>TestSet/ApproveEvaluationIndex.aspx/UpdateApproveEI",
                    data: "{ ApproveType: '" + ApproveType + "',EIId:'" + EIId + "',AEId:'" + AEId + "' }",
                    contentType: "application/json; charset=utf-8", dataType: "json",
                    success: function (msg) {
                        if (msg.d == 'complete') {
                            alert('บันทึกช้อมูลเรียบร้อยค่ะ')
                            $('#dialogApprove').dialog('close');
                            $('#dialogNotApprove').dialog('close');
                        }
                    },
                    error: function myfunction(request, status) {
                        //alert('บางอย่างผิดพลาดโปรดลองอีกครั้ง')
                    }
                                });
            }
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server" EnablePartialRendering="true">
        </telerik:RadScriptManager>

        <div id='MainDiv'>
            <asp:Label ID="lblWarn" runat="server" Visible="False" ForeColor="Red"></asp:Label>
            <h3 style='width: 1100px; margin-left: auto; margin-right: auto'>อนุมัติการแก้ไขและลบดัชนีชี้วัด</h3>
            <div id='ContentDiv' runat="server" style='width: 1100px; margin-left: auto; margin-right: auto'>
            </div>
        </div>
        <div id='dialogApprove' title='ยืนยัน อนุมัติ นะคะ'>
            <div style="display: flex;">
                <div class='btn btnOK'>ยืนยัน</div>
                <div class='btn btncancel'>ยกเลิก</div>
            </div>
        </div>
        <div id='dialogNotApprove' title='ยืนยัน ไม่อนุมัติ นะคะ'>
            <div style="display: flex;">
                <div class='btn btnOK'>ยืนยัน</div>
                <div class='btn btncancel'>ยกเลิก</div>
            </div>
        </div>
    </form>
</body>
</html>
