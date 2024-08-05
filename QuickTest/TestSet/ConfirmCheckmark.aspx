<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ConfirmCheckmark.aspx.vb"
    Inherits="QuickTest.ConfirmCheckmark" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0" name="viewport" />
    <script type="text/javascript" src="../js/jquery-1.7.1.js"></script>
    <script src="../js/jquery-ui-1.8.18.js" type="text/javascript"></script>
    <script src="../js/jquery.qtip-1.0.0-rc3.js" type="text/javascript"></script>
    <script src="../js/jquery-ui-sliderAccess.js" type="text/javascript"></script>
    <script src="../js/jquery-ui-offset_th.js" type="text/javascript"></script>
    <script src="../js/jquery-ui-timepicker-addon.js" type="text/javascript"></script>
    <script src="../js/jquery.blockUI.js" type="text/javascript"></script>
    <script src="../js/jquery.prettyPhoto.js" type="text/javascript"></script>
    <link href="../css/jquery-ui-1.8.18.custom.css" rel="stylesheet" type="text/css" />
    <link href="../css/jquery-ui-timepicker-addon.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        @import url(../fonts/thsarabunnew.css);
        body
        {
            /*width: 500px;
            height: 300px;
            margin-left: auto;
            margin-right: auto;
            background-color: white; */
            font: normal 20px thsarabunnew;
            text-align: center;
        }
        #divUseTemplate
        {
            height: 70px;
            text-align: center;
            width: 574px;
            padding: 30px;
            padding-bottom: 15px;
            background-image: url('../Images/Activity/Logocheckmark.png');
            background-repeat: no-repeat;
            background-position: center 60px;
            position: relative;margin-top: 80px;
        }
        /*.useTemplate
        {
            margin-top: 80px;
        }*/
        #divFormTemplate
        {
            display: none;
            height: 90px;
            text-align: center;
            padding-left: 30px;
            padding-right: 30px;
            padding-bottom: 30px;
            position: relative;
        }
        input[type=checkbox]
        {
            position: absolute;
            left: -999em;
        }
        input[type=checkbox] + label
        {
            height: 21px;
            padding-left: 21px;
            background: url(../images/bullet.gif) center left no-repeat;
        }
        input[type=checkbox]:checked + label
        {
            background-image: url(../images/bullet_checked.gif);
        }
        input[type=checkbox]:disabled + label
        {
            color: Gray;
            background-image: url(../images/bullet-disable.gif);
        }
        table
        {
            width: 100%;
        }
        .submit
        {
            font: 100% 'THSarabunNew';
            width: 99px;
            margin: 0 0 0 440px;
            margin-top: 20px;
            height: 40px;
            padding: 2px 0 3px 0;
            cursor: pointer;
            background: #1EC9F4;
            -moz-border-radius: .5em;
            -webkit-border-radius: .5em;
            border-radius: .5em; /*behavior:url(border-radius.htc);*/
            -webkit-box-shadow: 0 1px 2px rgba(0,0,0,.2);
            -moz-box-shadow: 0 1px 2px rgba(0,0,0,.2);
            box-shadow: 0 1px 2px rgba(0,0,0,.2);
            color: #FFF;
            border: solid 1px #0D8AA9;
            background: #46C4DD;
            background: -webkit-gradient(linear, left top, left bottom, from(#63CFDF), to(#17B2D9));
            background: -moz-linear-gradient(top,  #63CFDF,  #17B2D9);
            text-shadow: 1px 1px #178497;
            position: relative;
            behavior: url('../css/PIE.htc');
            -pie-track-active: false;
            top: 15px;
        }
        .txtbox
        {
            padding: 1px;
            font: 80% 'THSarabunNew';
            border: 1px solid #C6E7F0;
            background: #EFF8FB;
            color: #47433F;
            border-radius: 7px; /*behavior:url('../css/PIE.htc');*/
            -moz-border-radius: 7px;
            -webkit-border-radius: 7px;
            behavior: url(border-radius.htc);
        }
        div.ui-datepicker, .ui-datepicker td
        {
            font: normal 14px 'THSarabunNew';
            left:145px;
        }
        .ui-datepicker .ui-datepicker-buttonpane button.ui.datepicker-current
        {
            float:left;
            background:#900;
            display:none;}
    </style>
    <script type="text/javascript">
        $(document).ready(function () {            
            // datepickup
            $('#txtQuizdate').datepicker({
                monthNames: ['มกราคม', 'กุมภาพันธ์', 'มีนาคม', 'เมษายน', 'พฤษภาคม', 'มิถุนายน', 'กรกฎาคม', 'สิงหาคม', 'กันยายน', 'ตุลาคม', 'พฤศจิกายน', 'ธันวาคม'],
                dayNames: ['จันทร์', 'อังคาร', 'พุธ', 'พฤหัส', 'ศุกร์', 'เสาร์', 'อาทิตย์'],
                dayNamesMin: ['อา', 'จ', 'อ', 'พ', 'พฤ', 'ศ', 'ส'],
                dateFormat: 'dd/mm/yy',
                isBuddhist: true,
                defaultDate: toDay,
                onSelect: function (dateText, inst) {
                    var newformatDate = dateText.split('/');
                    var formatYear = newformatDate[2].substring('2', newformatDate[2].length);
                    var newDate = newformatDate[0] + '/' + newformatDate[1] + '/' + formatYear;
                    $(this).val(newDate);
                },
                beforeShow:function(input,inst){
                    $.datepicker._pos = $.datepicker._findPos(input);
                    $.datepicker._pos[0] = 66;
                    $.datepicker._pos[1] = 50;
                }
            });
            var d = new Date();
            var toDay = d.getDate() + '/' + (d.getMonth() + 1) + '/' + (d.getFullYear() + 543);

            //        $.datepicker.regional['th'] = {
            //            monthNames: ['มกราคม', 'กุมภาพันธ์', 'มีนาคม', 'เมษายน', 'พฤษภาคม', 'มิถุนายน', 'กรกฎาคม', 'สิงหาคม', 'กันยายน', 'ตุลาคม', 'พฤศจิกายน', 'ธันวาคม'],
            //            dayNames: ['จันทร์', 'อังคาร', 'พุธ', 'พฤหัส', 'ศุกร์', 'เสาร์', 'อาทิตย์'],
            //            dayNamesMin: ['อา', 'จ', 'อ', 'พ', 'พฤ', 'ศ', 'ส'],
            //            dateFormat: 'dd/mm/yy',
            //            isBuddhist: true,
            //            defaultDate: toDay
            //        };
            //        $.datepicker.setDefaults($.datepicker.regional['th']);

            $.timepicker.regional['th'] = {
                timeOnlyTitle: 'เลือกเวลาค่ะ',
                timeText: 'เวลา',
                hourText: 'ชั่วโมง',
                minuteText: 'นาที',
                closeText: 'ปิด',
                isRTL: false
            };
            $.timepicker.setDefaults($.timepicker.regional['th']);

            // timepickup
            $('#txtQuiztime').timepicker({
                hourMin: 8,
                hourMax: 17,
                stepMinute: 30,
                addSliderAccess: true,
                sliderAccessArgs: { touchonly: false },
                timeOnly:true,
                showButtonPanel:false,
                myPosition:'left top'                
            });

            

            $('#btnConfirm').click(function (e) {
                e.preventDefault();
                var chk = $('#chkUseTemplate').attr('checked');
                if(chk){
                    var setupN = getSetupName();
                    var className = $('#ddClassName').val();
                    //alert(setupN);
                    $.ajax({ type: "POST",
                        url: "<%=ResolveUrl("~")%>TestSet/ConfirmCheckmark.aspx/saveToChekmark",                    
                        async: false, // ทำงานให้เสร็จก่อน
                        data: "{setupName : '" + setupN + "', className : '" + className + "' }",  //" 
                        contentType: "application/json; charset=utf-8", dataType: "json",   
                        success: function (data) {
                            valReturnFromCodeBehide = data.d;
                            if(valReturnFromCodeBehide == true){  
                                window.parent.closeTheIFrame();     
                            }
                            else{
                                alert('ไม่สามารถบันทึกได้ค่ะ');
                            }                    
                        },
                        error: function myfunction(request, status)  {  
                            alert('ไม่สามารถบันทึกได้ค่ะ');      
                        }
                    });
                }
                else{
                    $.ajax({ type: "POST",
                        url: "<%=ResolveUrl("~")%>TestSet/ConfirmCheckmark.aspx/updateNeedConnectCheckmarkWhenNotNeed",                    
                        async: false, // ทำงานให้เสร็จก่อน                        
                        contentType: "application/json; charset=utf-8", dataType: "json",   
                        success: function (data) {
                            valReturnFromCodeBehide = data.d;
                            if(valReturnFromCodeBehide == true){  
                                window.parent.closeTheIFrame();     
                            }
                            else{
                                alert('ไม่สามารถบันทึกได้ค่ะ');
                            }                    
                        },
                        error: function myfunction(request, status)  {  
                            alert('ไม่สามารถบันทึกได้ค่ะ');      
                        }
                    });                    
                }
            });

            function getSetupName() {
                var className = $('#ddClassName').val();
                var roomName = $('#txtRoomName').val();
                if (roomName != '') {
                    roomName = '/' + roomName;
                }
                var quizDate = $('#txtQuizdate').val();
                var quizTime = $('#txtQuiztime').val();
                var setupName = $.trim($('#lblTestsetName').text()) + '(' + $.trim(className) + $.trim(roomName) + ' - ' + $.trim(quizDate) + ' ' + $.trim(quizTime) + ')';
                return setupName;
            }

            $('#chkUseTemplate').click(function () {
                var chk = $(this).attr('checked');
                if (chk) {
                    //$('#divUseTemplate').removeClass('useTemplate');
                    $('#divUseTemplate').css('margin-top','0px');
                    $('#divFormTemplate').show(200);
                }
                else {
                    //$('#divUseTemplate').addClass('useTemplate');
                    $('#divUseTemplate').css('margin-top','80px');
                    $('#divFormTemplate').hide();
                }
            });
            var checked = $('#chkUseTemplate').attr('checked');
            if(checked){
                //$('#divUseTemplate').removeClass('useTemplate');
                $('#divUseTemplate').css('margin-top','0px');
                $('#divFormTemplate').show(200);
            }
            
            if($('#chkUseTemplate').attr('disabled')){ 
                var content = 'ใช้กระดาษคำตอบไม่ได้ค่ะ เพราะจำนวนข้อสอบเกิน 120 ข้อ';
                if($('#divUseTemplate').hasClass('TypeError')){                                    
                    content = 'ใช้กระดาษคำตอบไม่ได้ค่ะ ข้อสอบต้องเป็นแบบตัวเลือกไม่เกิน 5 ช้อยส์หรือถูกผิดเท่านั้น';
                }                             
                //$('label[for="chkUseTemplate"]').qtip({
                $('form').qtip({                     
                    content: content.toString(),
                    show: { ready: true },
                    style: {
                        width: 250, padding: 5, background: '#F68500', color: 'white', textAlign: 'center', border: { width: 7, radius: 5, color: '#F68500' }, tip: 'topMiddle', name: 'dark', 'font-weight': 'bold','font-size':14
                    },                     
                    position: {corner:{tooltip:'bottomMiddle',target: 'bottomMiddle' }},
                    hide:false
                }); 
            }          
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <%--<div id="main">--%>
    <div id="m">
    
    <div id="divUseTemplate"  runat="server">
        <asp:CheckBox ID="chkUseTemplate" runat="server" Text="ให้เด็กทำด้วยกระดาษคำตอบคอมพิวเตอร์" /></div></div>
    <div id="divFormTemplate">
        <span>ชื่อชุดข้อสอบ "</span>
        <asp:Label ID="lblTestsetName" runat="server" Text="Label"></asp:Label><span>"</span><br />
        <span>(ใช้กระดาษคำตอบคอมพิวเตอร์)</span>
        <table>
            <tr>
                <td>
                    <span>ชั้น</span>
                    <asp:DropDownList ID="ddClassName" runat="server" Width="60px" class="txtbox">
                    </asp:DropDownList>
                </td>
                <td>
                    <span>ห้อง</span>
                    <asp:TextBox ID="txtRoomName" runat="server" Text="" Width="60px" class="txtbox"></asp:TextBox>
                </td>
                <td>
                    <span>วันที่</span>
                    <asp:TextBox ID="txtQuizdate" runat="server" Width="100px" class="txtbox"></asp:TextBox>
                </td>
                <td>
                    <span>เวลา</span>
                    <asp:TextBox ID="txtQuiztime" runat="server" Width="50px" class="txtbox"></asp:TextBox>
                </td>
            </tr>
        </table>
    </div>
    <asp:Button ID="btnConfirm" runat="server" Text="ถัดไป" class="submit" />
    <%--</div>--%>
    </form>
</body>
</html>
