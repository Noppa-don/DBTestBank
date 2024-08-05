<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="SelectSession.aspx.vb"
    Inherits="QuickTest.SelectSession" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0" name="viewport" />
    <script src="../js/jquery-1.7.1.js" type="text/javascript"></script>
    <script src="../js/GFB.js" type="text/javascript"></script>
    <script src="../js/Animation.js"></script>
    <script src="../js/jquery-ui-1.8.18.js" type="text/javascript"></script>
    <link href="../css/jquery-ui-1.8.18.custom.css" rel="stylesheet" type="text/css" />
    <telerik:RadScriptBlock ID="RadScriptBlock1" runat="server">
        <script type="text/javascript">

            var selected = '<%=HaveSelected%>';
            var ua = navigator.userAgent.toLowerCase();
            var isAndroid = ua.indexOf("android") > -1;

            $(function () {

                //hover animation
                InjectionHover('.btnSelectSession', 5);

                //ปุ่ม เปิดห้องใหม่
                new FastButton(document.getElementById('BtnNewSession'), TriggerServerButton);

                //ปุ่ม เข้าห้องเดิม
                new FastButton(document.getElementById('btnUseOldSession'), TriggerClickOldSession);

                if (isAndroid) {
                    var ww = ($(window).width() < window.screen.width) ? $(window).width() : window.screen.width; //get proper width
                    var mw = 480; // min width of site
                    var ratio = ww / mw; //calculate ratio
                    if (ww < mw) { //smaller than minimum size
                        $('#Viewport').attr('content', 'initial-scale=' + ratio + ', maximum-scale=' + ratio + ', minimum-scale=' + ratio + ', user-scalable=yes, width=' + ww);
                    } else { //regular size
                        $('#Viewport').attr('content', 'initial-scale=1.0, maximum-scale=2, minimum-scale=1.0, user-scalable=yes, width=' + ww);
                    }

                    $('#BtnNewSession').css({ 'width': '250px', 'font-size': '25px', 'height': '90px' });
                    $('#btnUseOldSession').css({ 'width': '250px', 'font-size': '25px', 'height': '90px' });
                }
                else {
                    $(':submit').click(function () {
                        FadePageTransitionOut();
                    });
                }

                //$('#Dialog-Confirm').dialog({
                //    autoOpen: false,
                //    resizable: false,
                //    modal: true,
                //    buttons: {                        
                //        "ตกลง": function () {
                //            UseSessionIsSelected();
                //        },
                //        "ยกเลิก": function () {
                //            $(this).dialog('close');
                //        }            
                //    }
                //});

                //เมื่อเรากดคลิกปุ่มเข้าห้องเดิมต้องไปยังหน้าที่อยู่ใน Session ที่เลือกตอนนั้น
                //$('#btnUseOldSession').click(function () {
                //if (selected == "False" || $('#HdPkInfo').val() != selected) {
                //    $('#Dialog-Confirm').html('<p>ต้องการเข้าควบคุมการสอนในห้องนี้นะคะ ?</p>').dialog({
                //        autoOpen: false,
                //        resizable: false,
                //        modal: true,
                //        buttons: {
                //            "ตกลง": function () {
                //                if (selected == "False") {
                //                    UseSessionIsSelected("False");
                //                } else {
                //                    UseSessionIsSelected("True");
                //                }                                    
                //            },
                //            "ยกเลิก": function () {
                //                $(this).dialog('close');
                //            }
                //        }
                //    }).dialog('open');
                //} else {
                //    $('#Dialog-Confirm').html('<p>อยู่ในห้องที่เลือกแล้วค่ะ</p>').dialog({
                //        autoOpen: false,
                //        resizable: false,
                //        modal: true,
                //        buttons: {
                //            "ตกลง": function () {
                //                $(this).dialog('close');
                //            }
                //        }
                //    }).dialog('open');
                //}
                //});
                $('#DialpgAlert').dialog({
                    autoOpen: false, draggable: false, resizable: false, modal: true, width: 'auto', buttons: {
                        "ตกลง": function () {
                            $(this).dialog('close');
                        }
                    }
                });
            });

            //เมื่อกดปุ่ม เปิดห้องใหม่
            //function TriggerClickNewSession(e) {
            //    var obj = e.target;
            //    $(obj).trigger('click');
            //}

            //เมื่อกดปุ่ม เข้าห้องเดิม
            function TriggerClickOldSession(e) {
                var PkData = $('#HdPkInfo').val();
                if (PkData === '') {
                    $('#DialpgAlert').dialog('open');
                    return false;
                }
                var obj = e.target;
                if (selected == "False" || $('#HdPkInfo').val() != selected) {
                    $('#Dialog-Confirm').html('<p>ต้องการเข้าควบคุมการสอนในห้องนี้นะคะ ?</p>').dialog({
                        autoOpen: false,
                        resizable: false,
                        modal: true,
                        buttons: {
                            "ตกลง": function () {
                                //if (selected == "False") {
                                //    UseSessionIsSelected("False");
                                //} else {
                                //    UseSessionIsSelected("True");
                                //}
                            },
                            "ยกเลิก": function () {
                                //$(this).dialog('close');
                            }
                        }
                    }).dialog('open');
                    if ($('.ui-button') != 0) {
                        $('.ui-button').each(function () {
                            if ($(this).text() == 'ตกลง') {
                                new FastButton(this, ConfirmUseOldSession);
                            }
                            else {
                                new FastButton(this, CloseDialogOldSession);
                            }

                        });
                    }
                } else {
                    $('#Dialog-Confirm').html('<p>อยู่ในห้องที่เลือกแล้วค่ะ</p>').dialog({
                        autoOpen: false,
                        resizable: false,
                        modal: true,
                        buttons: {
                            "ตกลง": function () {
                                $(this).dialog('close');
                            }
                        }
                    }).dialog('open');
                }
            }

            function ConfirmUseOldSession() {
                if (selected == "False") {
                    UseSessionIsSelected("False");
                } else {
                    UseSessionIsSelected("True");
                }
            }

            function CloseDialogOldSession() {
                $('#Dialog-Confirm').dialog('close');
            }

            //Get ค่าว่าหน้าปัจจุบันของ Session นั้น 
            function SendRowSelected(send, args) {
                //$('#HdScreenName').val(args.getDataKeyValue("หน้าจอ"));
                $('#HdPkInfo').val(args.getDataKeyValue("PK"));
                //alert(args.getDataKeyValue("PK"));
            }
            var lb = '<%=lb%>';

            var querystring = "<%=Request.QueryString("u")%>";
            //console.log(querystring);
            querystring = (querystring == null || querystring === "") ? "" : "?u=" + querystring;
            //console.log(querystring);
            function UseSessionIsSelected(ClearSession) {
                //var UrlLocation = $('#HdScreenName').val();
                var PkData = $('#HdPkInfo').val();
                if (PkData !== '') {
                    //////////////////////////////////////////////////////// Set ค่าให้ Session
                    $.ajax({
                        type: "POST",
                        url: "<%=ResolveUrl("~")%>WebServices/DashboardSignalRService.asmx/JoinSession",
                        data: "{ PkSession:'" + PkData + "', ClearSession : '" + ClearSession + "' }",
                        contentType: "application/json; charset=utf-8", dataType: "json",
                        async: false,
                        success: function (msg) {
                            var currentPage = msg.d;
                            if (lb == 'True') {
                            
                                window.parent.location = '<%=ResolveUrl("~")%>' + currentPage + querystring;
                            } else {
                                if (!isAndroid) {
                                    FadePageTransitionOut();
                                }
                                window.location = '<%=ResolveUrl("~")%>' + currentPage + querystring;
                            }

                        },
                        error: function myfunction(request, status) {
                            alert('บางอย่างผิดพลาดโปรดลองอีกครั้ง');
                        }
                    });
                    ///////////////////////////////////////////////////////
                }
                else {
                    alert('โปรดเลือก Session ก่อน'); $('#Dialog-Confirm').dialog('close');
                }
            }
            function ParentReload() {
                window.parent.location = '<%=ResolveUrl("~")%>Student/DashboardStudentPage.aspx';
            }

        </script>
    </telerik:RadScriptBlock>
    <title></title>

    <style type="text/css">
        .RadGrid_Hay .rgHeader, .RadGrid_Hay th.rgResizeCol {
            background: #FD8800 !important;
            border-color: #BB782B !important;
        }

        .RadGrid_Hay .rgRow td {
            text-align: center;
        }

        .RadGrid_Hay .rgAltRow td {
            text-align: center;
        }

        tr.rgRow td:first-child {
            /*width:50px;*/
            width: 5%;
        }

        tr.rgRow td:nth-child(2) {           
            width: 40%;
            text-align:left;
        }
        tr.rgRow td:nth-child(3) {            
            width: 15%;
        }

        /*tr.rgRow td:last-child {
            width: 200px;
        }*/
    </style>
    <script type="text/javascript">
        $(function () {
            $('hh').parent('td').css('text-align', 'left');
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
        </telerik:RadScriptManager>
        <div>
            <telerik:RadGrid ID="GVSession" runat="server" GridLines="Vertical" PagerStyle-VerticalAlign="Middle"
                Skin="Hay" AutoGenerateColumns="false">
                <ClientSettings>
                    <Selecting AllowRowSelect="true" />
                    <ClientEvents OnRowClick='SendRowSelected' />
                </ClientSettings>
                <%--<MasterTableView ClientDataKeyNames='หน้าจอ,PK'>
                </MasterTableView>--%>
                <MasterTableView ClientDataKeyNames='PK'>
                </MasterTableView>

            </telerik:RadGrid>
        </div>
        <div style='width: 100%; margin-left: auto; margin-right: auto; margin-top: 50px;'>
            <%--<asp:Button ID="BtnNewSession" runat="server" Text="เปิดของใหม่ +" Style='width: 170px;
            height: 70px; font-size: 25px; background-color: #5ECE0D; border-radius: 15px;
            color: White;' />
        <input type="button" id='btnUseOldSession' value='เข้าของเดิม' style='float: right;
            width: 170px; height: 70px; font-size: 25px; background-color: #5ECE0D; border-radius: 15px;
            color: White;' />  OnClientClick="ParentReload();" --%>
            <asp:Button ID="BtnNewSession" ClientIDMode="Static" runat="server" Text="เปิดห้องใหม่" Style='margin-left: 100px;'
                CssClass="btnSelectSession" />
            <input type="button" id='btnUseOldSession' value='เข้าห้องเดิม' style='float: right; margin-right: 100px;'
                class="btnSelectSession" />
        </div>
        <input type="hidden" id='HdScreenName' />
        <input type='hidden' id='HdPkInfo' />
        <div id='Dialog-Confirm' title='เลือกเข้าห้องนี้'>
        </div>
        <div id="DialpgAlert" title="โปรดเลือก Session ก่อน"></div>
    </form>
</body>
</html>
<style type="text/css">
    @import url(../fonts/thsarabunnew.css);

    #GVSession {
        width: auto!important;
    }

    .RadGrid, .RadGrid table {
        border-radius: .5em;
    }

        .RadGrid table th:first-child {
            border-radius: 5px 0 0 0;
        }

        .RadGrid table th:last-child {
            border-radius: 0 5px 0 0;
        }

        .RadGrid table tr:last-child td:first-child {
            border-radius: 0 0 0 5px;
        }

        .RadGrid table tr:last-child td:last-child {
            border-radius: 0 0 5px 0;
        }

        .RadGrid table th, .RadGrid table tr {
            height: 70px;
            font-size: 25px;
        }

    .rgMasterTable {
        font: 100% 'THSarabunNew' !important;
    }

    .RadGrid_Hay .rgHeader {
        text-align: center;
    }

    .btnSelectSession {
        width: 170px;
        height: 70px;
        font-size: 25px;
        background: -webkit-gradient(linear, left top, left bottom, from(#FAA51A), to(#F47A20));
        background-color: #F68500;
        border-radius: 0.5em;
        color: white;
        border: solid 1px #DA7C0C;
        text-shadow: 1px 1px #7E4D0E;
        box-shadow: 0 1px 2px rgba(0,0,0,.2);
        font: 150% 'THSarabunNew';
    }

    body {
        /*background-color: rgb(216, 255, 216);*/
        background-color: #FFDE8C;
    }

    form {
        margin-top: 5%;
    }

    .RadGrid_Hay .rgSelectedRow {
        background: #FFEB00 !important;
    }
</style>
