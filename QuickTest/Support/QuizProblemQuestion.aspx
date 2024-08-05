<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="QuizProblemQuestion.aspx.vb" Inherits="QuickTest.QuizProblemQuestion" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    
    <style type="text/css">
        @import url(../fonts/thsarabunnew.css);

        iframe {
            width: 500px!important;
        }

        html {
            height: 100%;
        }
   </style>
</head>
<body>
    <form runat="server">
        <div>
            <table>
                <tr>
                    <td>
                        <span>ปัญหา</span>
                    </td>
                    <td>
                        <select id="id" name="dropdown" style="width:300px;">
                            <option value="0" selected="selected">เลือกหัวข้อปัญหาค่ะ...</option>
                            <option value="1">คำถาม/คำตอบ ไม่ชัดเจน</option>
                            <option value="2">คำอธิบายคำถาม/คำอธิบายคำตอบ ไม่ชัดเจน</option>
                            <option value="3">เฉลยผิด</option>
                            <option value="4">สับสน ไม่เข้าใจว่าใช้งานยังไง</option>
                            <option value="5">ปัญหาอื่นๆ</option>
                        </select>
                    </td>
                    <td style="color:red;">****</td>
                </tr>
                <tr>
                    <td><span>รายละเอียด</span></td>
                    <td> <textarea rows="4" cols="1" id="txtDescript" name="descript" style="width:375px; height:400px;"></textarea> </td>
                    <td style="color:red;">****</td>
                </tr>
                <tr>
                    <td colspan="3" style="text-align: center;">
                        <asp:Button ID="btnSubmit" runat="server" text="แจ้งปัญหา" class="Forbtn" style="width: 95px; height: 40px; margin: 0px 20px; 
                                                                                                    line-height: 40px; border-radius: 10px; color: white; 
                                                                                                    position: initial; top: 0px; text-align: center;" />
                    </td>
                </tr>
            </table>      
        </div>
    </form>
</body>
</html>
