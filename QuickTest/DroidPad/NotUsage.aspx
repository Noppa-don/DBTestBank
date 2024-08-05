<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="NotUsage.aspx.vb" Inherits="QuickTest.NotUsage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        div {
            width: 100%;
            height: 100%;
            text-align: center;
            /*padding-top: 50px;*/
        }

        span {
            font-size: 24px;
        }

        .divWarningMaxOnet {
            padding-top: 0;
            color: red;
            text-align: left;
            width: 720px;
            margin-left: auto;
            margin-right: auto;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <img src="../Images/NotApproveButton.png" /><br />
            <%If IsMaxOnet Then %>
            <h1 style="color: red;">ไม่สามารถใช้งานได้ค่ะ</h1>
            <div class="divWarningMaxOnet">
                <span>- ใช้ได้แค่เครื่องเดียวนะคะ</span><br /><br />
                <span>- ถ้าลงทะเบียนเครื่องอื่นไว้ จะใช้ได้จากเครื่องล่าสุดเครื่องเดียวค่ะ</span><br /><br />
                <span>- หากต้องการสลับมาใช้เครื่องนี้ ทำการกรอกชื่อผู้ใช้และรหัสผ่านที่ใช้ลงทะเบียนอีกครั้งค่ะ</span><br /><br />
                <div class="ChangeDevice">
                    <span>ชื่อผู้ใช้         :          </span><input type="text" id="txtUserName" />
                    <br />
                    <br />
                    <span>รหัสลับ         :          </span><input type="password" id="txtPassword"  />
                    <br />
                    <br />
                    <br />
                    <input type="button" id="btnChangeDevice" value="สลับเครื่อง" style="width: 150px;" onclick="onBtnChangeDevice();" />
                </div>
                <br /><br />
            </div>
            <%Else %>
            <span>ไม่สามารถใช้งานได้ค่ะ</span><br />
            <span>กรุณาติดต่อศูนย์คอม ให้ทำการลงทะเบียนใหม่ค่ะ</span>
            <%End If %>
        </div>
    </form>
</body>
</html>
