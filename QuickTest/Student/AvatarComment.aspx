<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="AvatarComment.aspx.vb" Inherits="QuickTest.AvatarComment" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <style type="text/css">
        #MainDiv {
        width:700px;
        margin-left:auto;
        margin-right:auto;
        height:395px;
        text-align: center;
        }
        #ContentDiv {
        padding:10px;
        border:1px solid;
        border-radius:6px;
        overflow:auto;
        height:290px;
        }
        #CommentDiv {
        margin-top:20px;
        height:90px;
        }
        #txtComment {
        font-size:20px;
        width:575px;
        }
        #BtnComment {
        position: relative;
        top: -16px;
        height: 57px;
        font-size: 25px;
        width:110px;
        font-size:40px;
        }
        .ForDivComment {
        width:79%;
        display:inline-block;
        font-size:18px;
        vertical-align:top;
        height:100px;
        overflow:auto;
        text-align:left;
        }
        .ForDivAvt {
        width:10%;
        display:inline-block;
        height:100px;
        }
        .ForDivCommentCover {
        margin-top:10px;
        margin-bottom:10px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div id="MainDiv">
        <div id="ContentDiv" runat="server">
         <%--   <div id="Div1" class="ForDivCommentCover">
                <div id="comment" class="ForDivComment">
                    <div>
                        <span>15/4/12 12:51</span>
                    </div>
                    <div style="margin-left:15px;margin-top:10px;">
                        กหกฟหกฟหก่ฟสหก่สฟหก่สฟหก่สฟหื่าฟหกadasdasdasdadasdasdasda
                    dasdasdasdadasdasdasdadasdasdasdadasdasdasdadasdasdasdada
                    sdasdasd
                    adasdasdasdadasdasdasdadasdasdasdadasdasdasd
                    adasdasdasdadasdasdasdadasdasdasdadasdasdasd
                    </div>
                </div>
                <div id="picture" style="background:url('../UserData/1000001/{6F2D0501-9B9C-4D32-9A27-5EC5D51EC76D}/avt.png');background-size:cover;" class="ForDivAvt">
                </div>
            </div>--%>

        </div>
        <div id="CommentDiv">
            <asp:TextBox ID="txtComment" ClientIDMode="Static" runat="server" TextMode="MultiLine" MaxLength="500"></asp:TextBox>
            <asp:Button ID="BtnComment" ClientIDMode="Static" runat="server" Text="ส่ง" />
        </div>
    </div>
    </form>
</body>
</html>
