<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="FAQ.aspx.vb" Inherits="QuickTest.FAQ" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadStyleContent" runat="server">
    <form runat="server" id='formChooseTestSet'>
        <div id="main">
            <div id="site_content">
                <div class="content" style="width: 930px; height:620px; overflow:scroll;">
                    <section id="select">
		                <center>
		                    <h2 >คำถามที่พบบ่อย</h2>
                  
                            <table>
                                <tr>
                                    <td style="background: #39bbd0; padding-left: 35px; font-weight: bold;">
                                        ในกรณีที่ต้องการพิมพ์สระ หรือ วรรณยุกต์ในภาษาไทยแบบเดี่ยวๆ (พบส่วนมากในวิชาภาษาไทย) 
                                    </td>
                                </tr>
                                <tr>
                                    <td style="background: #D3F2F7; padding-left: 20px;">
                                        ตอบ ในแถบเครื่องมือการใช้งานจะมีปุ่มสำหรับกดเพื่อแสดงสระและวรรณยุกต์แบบเดี่ยวๆ  เพื่อให้ท่านสามารถกดเลือกใช้งานแทนการพิมพ์ได้สะดวกและรวดเร็วยิ่งขึ้น
                                    </td>
                                </tr>

                            </table>
                            <table>
                                <tr>
                                    <td style="background: #39bbd0; padding-left: 35px; font-weight: bold;">
                                        การแสดงผลหลังจากเลือกใช้งานปุ่มสระ หรือ วรรณยุกต์แล้ว  เมื่อนำออกมาเป็นเอกสาร Word ทางผู้ใช้งานมีการเลือกเปลี่ยนรูปแบบอักษร  และพบว่าข้อความดังกล่าวกลายเป็นรูปสี่เหลี่ยม หรือเปลี่ยนเป็นภาษาที่ไม่สามารถอ่านได้
                                    </td>
                                </tr>
                                <tr>
                                    <td style="background: #D3F2F7; padding-left: 20px;">
                                       ตอบ  ท่านควรหลีกเลี่ยงการแก้ไขปรับเปลี่ยนรูปแบบอักษร ในส่วนนี้  เนื่องด้วยการใช้งานปุ่มสระ หรือ วรรณยุกต์ของเรานั้นจะแสดงผลเป็นรูปแบบอักษรเฉพาะ ซึ่งไม่สามารถปรับเปลี่ยนได้  หรือหากท่านจำเป็นต้องแก้ไขรูปแบบอักษรในข้อความ ท่านก็สามารถทำได้ แต่รบกวนให้ทำการแก้ไขรูปแบบอักษรในส่วนอื่น  โดยไม่ทำการเปลี่ยนในส่วนของรูปแบบอักษรเฉพาะที่กล่าวมา
                                    </td>
                                </tr>

                            </table>
                             <table>
                                <tr>
                                    <td style="background: #39bbd0; padding-left: 35px; font-weight: bold;">
                                        เมื่อทำการ Download เอกสาร Word แล้วพบว่าบางข้อความ  มีการตัดคำขึ้นบรรทัดใหม่ โดยทำให้รูปประโยคขาดช่วง ไม่ได้ความหมาย มีวิธีแก้ไขอย่างไร 
                                    </td>
                                </tr>
                                <tr>
                                    <td style="background: #D3F2F7; padding-left: 20px;">
                                        ตอบ  เนื่องจากเราจะทำการตัดคำหรือประโยคตามขนาดหน้ากระดาษที่ได้ตั้งค่าไว้  ทางผู้จัดทำจึงมีการสร้างข้อความแจ้งเตือนในส่วนนี้ก่อนทำการ Download เอกสารไว้   ขอความร่วมมือจากทางผู้ใช้งาน  รบกวนทำการตรวจสอบความถูกต้องของรูปแบบ การสะกดคำซ้ำอีกครั้งในไฟล์ Word  เพื่อความถูกต้องและสมบูรณ์ครบถ้วน
                                    </td>
                                </tr>

                            </table>


                             <table>
                                <tr>
                                    <td style="background: #39bbd0; padding-left: 35px; font-weight: bold;">
                                        ปัญหาในส่วนการแนบรูปภาพประกอบในคำถาม  เมื่อแสดงผลในเอกสารWord แล้วพบว่ารูปภาพมีขนาดใหญ่กว่าที่แสดงบนหน้าจอโปรแกรม 
                                    </td>
                                </tr>
                                <tr>
                                    <td style="background: #D3F2F7; padding-left: 20px;">
                                       ตอบ  เราได้มีข้อความแนะนำขนาดที่เหมาะสมของรูปภาพไว้ที่หน้าแก้ไขคำถามคำตอบ   ท่านสามารถทำการตรวจสอบและปรับขนาดรูปภาพตามคำแนะนำ เพื่อการแสดงผลที่ถูกต้องและสวยงาม
                                    </td>
                                </tr>

                            </table>
                             <table>
                                <tr>
                                    <td style="background: #39bbd0; padding-left: 35px; font-weight: bold;">
                                       หากระหว่างใช้งาน พบว่าโปรแกรมเกิดความล่าช้าหรือไม่ตอบสนองใดๆ   ควรปฏิบัติอย่างไร
                                    </td>
                                </tr>
                                <tr>
                                    <td style="background: #D3F2F7; padding-left: 20px;">
                                        ตอบ การแก้ไขปัญหาเบื้องต้น คือ ให้ทางผู้ใช้งานทำการ Log out  และทำการ Login เข้าใช้งานใหม่อีกครั้ง หากยังใช้งานไม่ได้  สาเหตุอาจจะเกิดจากมีผู้ใช้งาน เครือข่ายในช่วงเวลานั้นในปริมาณมาก หรือเครือข่ายที่ท่านใช้  เกิดความผิดปกติในการเชื่อมต่อ แนะนำให้ท่านติดต่อไปที่ฝ่ายที่มีความเชี่ยวชาญด้าน Network ในองค์กร ของท่าน ให้ทำการตรวจสอบปัญหานี้ เพื่อให้เกิดการใช้งานโปรแกรมที่มีประสิทธิภาพยิ่งขึ้น
                                    </td>
                                </tr>

                            </table>
                             <table>
                                <tr>
                                    <td style="background: #39bbd0; padding-left: 35px; font-weight: bold;">
                                        กรณีที่ท่านต้องการสอบถามการใช้งานเพิ่มเติม หรือพบปัญหาด้านการใช้งานอื่นๆ
                                    </td>
                                </tr>
                                <tr>
                                    <td style="background: #D3F2F7; padding-left: 20px;">
                                        ตอบ ท่านสามารถติดต่อสอบถามและแจ้งปัญหาที่พบ มาที่  support@wpp.co.th  โดยแนบรายละเอียดอธิบายขั้นตอนการใช้งานที่ทำให้พบปัญหาดังกล่าว พร้อมรูปภาพประกอบ(ถ้ามี) และช่องทางติดต่อกลับของท่าน เพื่อที่ทางเราจะได้รีบดำเนินการและติดต่อกลับถึงท่านให้เร็วที่สุด
                                    </td>
                                </tr>

                            </table>
                
                           

                        </center> 
                    </section>
                 </div>
            </div>
        </div>
    </form>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ExtraScript" runat="server">
</asp:Content>
