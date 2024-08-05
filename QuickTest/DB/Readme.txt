script sql นี้ จะทำการ replace tag <o:p></o:p> ออกจาก tblquestion และ tblanswer

step การ sync db เมื่อได้มาจาก วิชาการ
1. เปิดโปรแกรม redgate sql field compare ใช้ master db เป็นตัวตั้งต้น แล้วทำการ sync db วิชาการ (เฉพาะ db วิชาการมีแต่ master db ไม่มี)
2. เปิดโปรแกรม redgate data compare ใช้ master db เป็นตัวตั้งต้น แล้วทำการ sync db จากวิชาการ เฉพาะ 
  tblBook, tblEfficiency tblEfficiencySet tblQuestion, tblAnswer 
tblEvaluationIndex tblEvaluationIndexGroup tblEvaluationIndexItem tblEvaluationIndexLevel tblEvaluationIndexNew
tblEvaluationIndexSubject tblEvaluationIndexWithSubject tblGroupSubject tblIntro tblIntroQuestionSet tblIntroQuestionSetQuestion
tblLayoutConfirmed tblLevel tblMultimediaObject tblMultimediaQuestion tblMultimediaQuestionSet tblQuestion tblQuestion40
tblQuestionCategory tblQuestionEvaluationIndexItem tblQuestionset tblQuestionSetPassword
3. เปิด script ที่ master db เพื่อทำการ replace tag <o:p> ที่งไป 
4. ทำการ backup ก้อน master db ใหม่ เพื่อใช้เป็น master ต่อไป เมื่อมี db จากฝั่งวิชาการมาใหม่ 