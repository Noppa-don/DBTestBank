select * from tblQuestion where Question_Name like '%<o:p></o:p>%' or Question_Expain like '%<o:p></o:p>%'
select * from tblAnswer where Answer_Name like '%<o:p></o:p>%' or Answer_Expain like '%<o:p></o:p>%'

update tblQuestion set Question_Name = replace(cast(Question_Name as varchar(max)),'<o:p></o:p>','') where Question_Name like '%<o:p></o:p>%';
update tblQuestion set Question_Expain = replace(cast(Question_Expain as varchar(max)),'<o:p></o:p>','') where Question_Expain like '%<o:p></o:p>%';
update tblAnswer set Answer_Name = replace(cast(Answer_Name as varchar(max)),'<o:p></o:p>','') where Answer_Name like '%<o:p></o:p>%';
update tblAnswer set Answer_Expain = replace(cast(Answer_Expain as varchar(max)),'<o:p></o:p>','') where Answer_Expain like '%<o:p></o:p>%';