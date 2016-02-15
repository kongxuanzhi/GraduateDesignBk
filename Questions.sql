select * from Questions
--提出问题
		--公开提问
		insert into Questions values(NEWID(), '75df7052-da7b-4931-9ee0-bcd53a8bc3b0' ,'0',  1,GETDATE(), 'how graduateDesign to do?','I will graduate now and then?',0,0,0)
		--私密提问 
		insert into Questions values(NEWID(), '75df7052-da7b-4931-9ee0-bcd53a8bc3b0' ,'742b33e9-07e0-462b-ac6f-3486cc5b7ef4',  1,GETDATE(), 'how graduateDesign to do?','I will graduate now and then?',0,0,0)
--评论问题
	   select  * from Answers
       select * from Questions
        --need  PQID  
		insert into Answers values(NEWID(), 'e749ccc5-e7dd-465d-955e-380e5834df74','75df7052-da7b-4931-9ee0-bcd53a8bc3b0', '0','B3A3ED51-C58E-4490-BC18-D423259CCB6E',GETDATE(),'woyebuzhidao', 0)
		insert into Answers values(NEWID(),'75df7052-da7b-4931-9ee0-bcd53a8bc3b0','e749ccc5-e7dd-465d-955e-380e5834df74','14BCFFAF-6C41-4B88-847B-0DC11E7FF361','B3A3ED51-C58E-4490-BC18-D423259CCB6E',GETDATE(),'还好吧',0)
