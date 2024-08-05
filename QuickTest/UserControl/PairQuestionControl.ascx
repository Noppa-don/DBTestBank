<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="PairQuestionControl.ascx.vb" Inherits="QuickTest.PairQuestionControl" %>
<div style="margin-top: 10px;">
    <div class="divQuestionAndAnswer">
        <div id="mainQuestion" class="Question">
            <div class="btnQuestionExplain" style="width: 25px; height: 25px; top: 10px; right: 40px; position: absolute; background: url('../images/activity/btnExplain-85.png'); background-size: cover; cursor: pointer;z-index:9;"></div>
            <div class="btnEvalutionQtip" style="width: 25px; height: 25px; top: 10px; right: 10px; position: absolute; background: url('../images/Alert.png'); background-size: cover;z-index:9;"></div>
            <span class="spnQuestionNumber"></span>
            <div id="divQuestion" class="EditQuestion" style="display: block; position: relative;margin-left: 0.1px;width:650px;" tabindex="0" contenteditable="true" defaulttext="คลิกที่นี่เพื่อใส่คำถาม"></div>
        </div>
        <div id="mainAnswer">
            <table id="Table1" style="width: 650px; border-collapse: collapse;">
                <tbody>
                    <%-- <tr class="3" style="">
                        <td style="width: 45%; border-bottom: 1px solid Gray; padding-right: 10px;">พลิกกระดาษกลับมาด้านหลัง แล้วทากาวส่วนที่พับไว้</td>
                        <td style="width: 10%; border-bottom: 1px solid Gray; text-align: center; font-weight: bold;">คู่กับ</td>
                        <td id="4ce33c78-9654-4df3-8cf5-0d8433c7299d" class="drop ui-droppable" style="width: 45%; border-bottom: 1px solid Gray; padding-left: 10px;"><span id="23cf1a2b-76ca-4193-931d-a36a49a76a4c" class="drag ui-draggable" style="">3</span></td>
                    </tr>
                    <tr class="3" style="">
                        <td style="width: 45%; border-bottom: 1px solid Gray; padding-right: 10px;">วาดหน้าสุนัข แล้วตัดเจาะช่องลูกตา</td>
                        <td style="width: 10%; border-bottom: 1px solid Gray; text-align: center; font-weight: bold;">คู่กับ</td>
                        <td id="ff9caa3c-005c-4f40-8ddc-4d636371d37c" class="drop ui-droppable" style="width: 45%; border-bottom: 1px solid Gray; padding-left: 10px;"><span id="d743a766-e3f0-4abf-a807-1497231e2c10" class="drag ui-draggable" style="">6</span></td>
                    </tr>
                    <tr class="3" style="">
                        <td style="width: 45%; border-bottom: 1px solid Gray; padding-right: 10px;">พับมุมบนทั้งสองข้างลงมาด้านหน้า</td>
                        <td style="width: 10%; border-bottom: 1px solid Gray; text-align: center; font-weight: bold;">คู่กับ</td>
                        <td id="012e92f2-dee3-4819-82e2-746bc45670d8" class="drop ui-droppable" style="width: 45%; border-bottom: 1px solid Gray; padding-left: 10px;"><span id="ac699ccb-da62-4338-a53d-f3f3a7ce6e4b" class="drag ui-draggable" style="">1</span></td>
                    </tr>
                    <tr class="3" style="">
                        <td style="width: 45%; border-bottom: 1px solid Gray; padding-right: 10px;">เจาะรูที่ใบหู ร้อยยางรัด แล้วทดลองคล้องหู</td>
                        <td style="width: 10%; border-bottom: 1px solid Gray; text-align: center; font-weight: bold;">คู่กับ</td>
                        <td id="bee0b33f-d7a7-4a2d-9d75-84f93aaaf956" class="drop ui-droppable" style="width: 45%; border-bottom: 1px solid Gray; padding-left: 10px;"><span id="776ad9ed-2afd-4b7c-a89f-0deea00c7d89" class="drag ui-draggable" style="">4</span></td>
                    </tr>
                    <tr class="3" style="">
                        <td style="width: 45%; border-bottom: 1px solid Gray; padding-right: 10px;">ตัดกระดาษเป็นรูปสี่เหลี่ยมจัตุรัส พับครึ่งตามแนวทแยง</td>
                        <td style="width: 10%; border-bottom: 1px solid Gray; text-align: center; font-weight: bold;">คู่กับ</td>
                        <td id="d7002936-4f09-4d2c-8ef7-b8c5c19b668d" class="drop ui-droppable" style="width: 45%; border-bottom: 1px solid Gray; padding-left: 10px;"><span id="7f976ceb-6714-4345-b47d-3114c0894742" class="drag ui-draggable" style="">5</span></td>
                    </tr>
                    <tr class="3" style="">
                        <td style="width: 45%; border-bottom: 1px solid Gray; padding-right: 10px;">พับมุมล่างไปด้านหลังประมาณ 1 ใน 3</td>
                        <td style="width: 10%; border-bottom: 1px solid Gray; text-align: center; font-weight: bold;">คู่กับ</td>
                        <td id="3b2b5ccd-64cb-49e6-957a-fa5813b1af09" class="drop ui-droppable" style="width: 45%; border-bottom: 1px solid Gray; padding-left: 10px;"><span id="865960d4-7d05-4804-961d-8352c3d3aa17" class="drag ui-draggable" style="">2</span></td>
                   </tr>--%>
                    <%--<tr class="6" id="Answer">
                        <td>
                            <ul id="sortable" style="margin-left: -40px;" class="ui-sortable">
                                <li id="ff9caa3c-005c-4f40-8ddc-4d636371d37c" style="">วาดหน้าสุนัข แล้วตัดเจาะช่องลูกตา</li>
                                <li id="bee0b33f-d7a7-4a2d-9d75-84f93aaaf956" style="">เจาะรูที่ใบหู ร้อยยางรัด แล้วทดลองคล้องหู</li>
                                <li id="012e92f2-dee3-4819-82e2-746bc45670d8" class="">พับมุมบนทั้งสองข้างลงมาด้านหน้า</li>
                                <li id="4ce33c78-9654-4df3-8cf5-0d8433c7299d" style="">พลิกกระดาษกลับมาด้านหลัง แล้วทากาวส่วนที่พับไว้</li>
                                <li id="3b2b5ccd-64cb-49e6-957a-fa5813b1af09" class="">พับมุมล่างไปด้านหลังประมาณ 1 ใน 3</li>
                                <li id="d7002936-4f09-4d2c-8ef7-b8c5c19b668d" class="">ตัดกระดาษเป็นรูปสี่เหลี่ยมจัตุรัส พับครึ่งตามแนวทแยง</li>
                            </ul>
                        </td>
                    </tr>--%>
                </tbody>
            </table>
        </div>
    </div>

    <div class="divQuestionAndAnswerExplain" style="display: none;">
        <div id="mainQuestionExplain" class="Question">
            <div class="btnQuestionExplain" style="width: 25px; height: 25px; top: 10px; right: 40px; position: absolute; background: url('../images/activity/btnExplain-85.png'); background-size: cover; cursor: pointer;"></div>
            <span class="spnQuestionNumber"></span>
            <div id="divQuestionExp" style="display: inline;word-wrap: break-word"></div>
            <div id="QuestionExp" class="CanEdit EditQuestionExplain" contenteditable="true" defaulttext="คลิกที่นี่เพื่อใส่อธิบายคำถาม">
            </div>
        </div>
        <div id="mainAnswerExplain">
            <div id="AnswerExp" style="font-size: 24px;">
                <%--<div>
                    <div class="answerExplain">
                        <table>
                            <tr>
                                <td style="width: 45px;">
                                    <input type="checkbox" id="rightAnswer1" class="rightAnswer" /><label for="rightAnswer1"></label><span>ก.</span></td>
                                <td><span>ถูก</span>
                                 <div class="CanEdit EditAnswerExplain" contenteditable="true">
                                 </div>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
                <div>
                    <div class="answerExplain">
                        <table>
                            <tr>
                                <td style="width: 45px;">
                                    <input type="checkbox" id="rightAnswer2" class="rightAnswer" /><label for="rightAnswer2"></label><span>ข.</span></td>
                                <td><span>ผิด</span><div class="CanEdit EditAnswerExplain" contenteditable="true"></div>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </div>--%>
            </div>
        </div>
    </div>
</div>
