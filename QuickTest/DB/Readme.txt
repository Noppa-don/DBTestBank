script sql ��� �зӡ�� replace tag <o:p></o:p> �͡�ҡ tblquestion ��� tblanswer

step ��� sync db ��������Ҩҡ �Ԫҡ��
1. �Դ����� redgate sql field compare �� master db �繵�ǵ�駵� ���Ƿӡ�� sync db �Ԫҡ�� (੾�� db �Ԫҡ������ master db �����)
2. �Դ����� redgate data compare �� master db �繵�ǵ�駵� ���Ƿӡ�� sync db �ҡ�Ԫҡ�� ੾�� 
  tblBook, tblEfficiency tblEfficiencySet tblQuestion, tblAnswer 
tblEvaluationIndex tblEvaluationIndexGroup tblEvaluationIndexItem tblEvaluationIndexLevel tblEvaluationIndexNew
tblEvaluationIndexSubject tblEvaluationIndexWithSubject tblGroupSubject tblIntro tblIntroQuestionSet tblIntroQuestionSetQuestion
tblLayoutConfirmed tblLevel tblMultimediaObject tblMultimediaQuestion tblMultimediaQuestionSet tblQuestion tblQuestion40
tblQuestionCategory tblQuestionEvaluationIndexItem tblQuestionset tblQuestionSetPassword
3. �Դ script ��� master db ���ͷӡ�� replace tag <o:p> ���� 
4. �ӡ�� backup ��͹ master db ���� �������� master ���� ������� db �ҡ����Ԫҡ�������� 