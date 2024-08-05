<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="AddNewQuestionCatAndSet.aspx.vb"
    Inherits="QuickTest.AddNewQuestionCatAndSet" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <asp:Label ID="lblWarnQuestionCat" Font-Size="X-Large"  runat="server" ForeColor="Red" Visible="false" Text=""></asp:Label>
    <div id='MainDiv'>
        <asp:Button ID="btnBack" Font-Size="X-Large" runat="server" Text="กลับ" />
        <div id='DivQuestionCat' style='text-align:center;' >
            <asp:Label ID="Label1" Font-Size="X-Large" Font-Underline="True" runat="server" Text="เพิ่มหน่วยการเรียนรู้"></asp:Label>
            <br />
         
            <br />
            <asp:Label ID="Label8"  runat="server" Text="เลือกหนังสือ"></asp:Label>
            <asp:DropDownList ID="ddlBook" runat="server" AutoPostBack="True">
            </asp:DropDownList>
            <br />
            <br />
            <asp:Label ID="Label9"  runat="server" Text="เลือกหน่วยการเรียนรู้ที่ซ้อนอยู่"></asp:Label>
            <br />
            <asp:DropDownList ID="ddlChildCategory" runat="server">
            </asp:DropDownList>
            <br />
            <br />
            <asp:Label ID="Label6"  runat="server" Text="เพิ่มหน่วยการเรียนรู้"></asp:Label>
            <br />
            <br />
            <asp:TextBox ID="txtQuestionCat" runat="server" TextMode="MultiLine" 
                Height="100px" Width="350px" ></asp:TextBox>
            <br />
            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="จำกัดตัวอักษร 255 ตัวอักษร" ValidationExpression="^[\s\S]{0,255}$"  ControlToValidate="txtQuestionCat"></asp:RegularExpressionValidator>
            <br />
            <asp:Button ID="btnAddQuestionCat" style='float:right;' Height='40px' runat="server" Text="เพิ่มหน่วยการเรียนรู้" />
        </div>
        <br />
        <br />
        <hr />
        <asp:Label ID="lblWarnQuestinoSet" Font-Size="X-Large"  runat="server" ForeColor="Red" Visible="false" Text=""></asp:Label>
        <div id='DivQuestionSet' style='text-align:center;margin-top:20px;'>
        <asp:Label ID="Label2" Font-Size="X-Large" runat="server" Font-Underline="True" Text="เพิ่มชุดข้อสอบ"></asp:Label>
        <br />
        <br />
        <asp:Label ID="Label3" runat="server" Font-Bold="true"  Text="เลือกหน่วยการเรียนรู้"></asp:Label>
        <br />
           <br />
            <asp:DropDownList ID="ddlQCat" runat="server">
            </asp:DropDownList>
            <br />
            <br />
            <asp:Label ID="Label5"  runat="server" Font-Bold="true" Text="ชื่อชุดข้อสอบ" ></asp:Label>
            <br />
            <asp:Label ID="Label10"  runat="server" ForeColor="Red" Font-Bold="true" Text="(ในกรณีที่ข้อสอบเป็น จับคู่ , เรียงลำดับ ให้พิมพ์คำถามแทนชื่อชุดข้อสอบได้เลย เช่น 'จงเรียงลำดับข้อความต่อไปนี้','จงจับคู่สิ่งของต่อไปนี้')" ></asp:Label>
            <br />
            <br />
         <asp:TextBox ID="txtQuestionSet" runat="server" Height='100px' Width='350px' TextMode="MultiLine"></asp:TextBox>
         <br />
            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ErrorMessage="จำกัดตัวอักษร 8000 ตัวอักษร" ValidationExpression="^[\s\S]{0,8000}$" ControlToValidate="txtQuestionSet" Display="Static"></asp:RegularExpressionValidator>
         <br />
         <table style='margin-left:auto;margin-right:auto;'>
         <tr>
         <td>
            <asp:CheckBox ID="ChkDisplayCorrect"  Text='ให้แสดงคำตอบที่ถูกต้อง' runat="server" />
            </td>
            <td>
            <asp:CheckBox ID="ChkRandomQuestion"  Text='ให้สลับคำถามได้' runat="server" />
            </td>
            <td>
            <asp:CheckBox ID="ChkRandomAnswer"  Text='ให้สลับคำตอบได้' runat="server" />
            </td>
                        </tr>
            </table>
            <br />
            <br />
                      <asp:DropDownList ID="ddlQtype" runat="server">
                                <asp:ListItem Value="C8E51E6B-AFFC-4DD3-922A-15347F062307">แบบทดสอบท้ายบท</asp:ListItem>
                                <asp:ListItem Value="92F07F7D-519B-41EA-9022-69FC2089720E">แบบทดสอบปลายภาคเรียน</asp:ListItem>
                                <asp:ListItem Value="D9F17986-6A86-4515-ADF7-8501F775B6D6">แบบทดสอบการประเมินคุณภาพการศึกษาระดับชาติ(National Test: NT)</asp:ListItem>
                            </asp:DropDownList>
                            <br />
                            <br />
                            <asp:Label ID="Label7"  runat="server" Font-Bold="true" Text="จำนวนคำถาม"></asp:Label>
                             <asp:DropDownList ID="ddlQuantityQuestion" runat="server">
                                <asp:ListItem Value="5">5 ข้อ</asp:ListItem>
                                <asp:ListItem Value="10">10 ข้อ</asp:ListItem>
                                <asp:ListItem Value="15">15 ข้อ</asp:ListItem>
                            </asp:DropDownList>
            <br />
            <br />
          <asp:Label ID="Label4"  runat="server" Font-Bold="true" Text="เลือกชนิดข้อสอบ"></asp:Label>
            <br />
            <br />
            <asp:RadioButton ID="rdType1"  GroupName='QCatType' Text='ปรนัย' runat="server" />
            <asp:RadioButton ID="rdType2" GroupName='QCatType' Text='ถูก-ผิด'  runat="server" />
            <asp:RadioButton ID="rdType3" GroupName='QCatType' Text='จับคู่' runat="server" />
            <asp:RadioButton ID="rdType6" GroupName='QCatType' Text='เรียงลำดับ' runat="server" />
                  <br />
            <br />
        <asp:Button ID="btnAddQuestionSet" style='float:right;'  Height='40px' runat="server" Text="เพิ่มชุดข้อสอบ"  />
        </div>
    </div>
    </form>
</body>
</html>
