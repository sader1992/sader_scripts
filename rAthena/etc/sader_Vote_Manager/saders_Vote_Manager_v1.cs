//===== rAthena Script =======================================
//= saders Vote Manager
//===== By: ==================================================
//= Sader1992
//https://rathena.org/board/profile/30766-sader1992/
//===== Current Version: =====================================
//= 1.0
//===== Compatible With: ===================================== 
//= rAthena Project
//https://github.com/sader1992/sader_scripts
//===== Description: =========================================
//==== GEPARD OR IP (NOT BOTH!) [SEE THE LINES UNDER THE NPC NAME TO ENABLE ONE AND DISABLE ANOTHER!]
//==== vote npc , you can add up to 5 options for each vote and you can add multiple votes
//==== test it before using it so you know how it work
//==== if you use 'false' as an option the option would be empty so you can use less than 5 options
//==== sader1992@gmail.com
//============================================================
//==== I DON'T SUPPORT FREE SCRIPTS ON MY PM , IF YOU HAVE QUESTION YOU CAN POST AT THE TOPIC IN RATHENA!
//============================================================
prontera,148,192,4	script	saders Vote Manager::SADERVOTE	10163,{
OnCommand:
//==============================================
	//here if gepard
	//.@player_unique_id$ = get_unique_id();
	//here if ip
	.@player_unique_id$ = getcharip();
//==============================================
	.@npcName$ = "[" + strnpcinfo(1) + "]";
	if(getgroupid() >= 10){
		if(select("Skip:Admin Options") == 2){
			callsub OnGMOptions;
		}
	}
	mes .@npcName$;
	mes "welcome to the vote station!";
	query_sql("SELECT `vote_id`,`vote_active`,`vote_show`,`vote_main_view`,`vote_message`,`vote_option1`,`vote_option2`,`vote_option3`,`vote_option4`,`vote_option5`,`show_result` FROM `sader_vote` ",.@vote_id,.@vote_active$,.@vote_show$,.@vote_main_view$,.@vote_message$,.@vote_option1$,.@vote_option2$,.@vote_option3$,.@vote_option4$,.@vote_option5$,.@show_result);
	if(!getarraysize(.@vote_show$) || inarray(.@vote_show$, "Y") == -1){
		mes "there is no votes right now!";
		close;
	}
	mes "do you want to check out the votes ?";
	for(.@i=0;.@i<getarraysize(.@vote_id);.@i++){
		if(.@vote_show$[.@i] == "Y"){
			.@g = getarraysize(.@option$);
			.@option$[.@g] = .@vote_main_view$[.@i];
			.@votes[.@g] = .@vote_id[.@i];
		}
	}
	.@s = select(implode(.@option$, ":"))-1;
	clear;
	.@index = .@votes[.@s];
	if(.@vote_active$[.@s] == "N"){
		mes "this vote has been closed";
		if(select("Result")){
			clear;
			callsub VoteResult,.@vote_id[.@s];
		}
		end;
	}
	query_logsql("SELECT `unique_id`,`vote_option` FROM `sader_vote_log` WHERE `vote_id` = '" +.@vote_id[.@s]+"'",.@unique_id$,.@vote_option$);
	if(inarray(.@unique_id$,.@player_unique_id$) != -1){//unique id here
		mes "you already voted on this with";
		mes .@vote_option$[inarray(.@unique_id$,.@player_unique_id$)];//unique id here
		mes "you will see the results win the vote is closed";
		if(getgroupid() >= 10 || .@show_result[.@s]){
			if(select("Result")){
				clear;
				callsub VoteResult,.@vote_id[.@s];
			}
		}
		close;
	}
	
	mes .@vote_message$[.@s];
	for(.@i=1;.@i<6;.@i++){
		if(getd(".@vote_option" + .@i + "$[" + .@s + "]") != ""){
			.@votes$[getarraysize(.@votes$)] = getd(".@vote_option" + .@i + "$[" + .@s + "]");
		}else{
			break;
		}
	}
	.@s2 = select(implode(.@votes$, ":")) -1;
	clear;
	mes "your vote is ";
	mes .@votes$[.@s2];
	mes "are you sure ?";
	if(select("yes:no") == 2)
		close;
	clear;
	query_logsql("SELECT `unique_id`,`vote_option` FROM `sader_vote_log` WHERE `vote_id` = '" +.@vote_id[.@s]+"'",.@unique_id$,.@vote_option$);
	if(inarray(.@unique_id$,.@player_unique_id$) != -1){//unique id here
		mes "you already voted on this with";
		mes .@vote_option$[inarray(.@unique_id$,.@player_unique_id$)];//unique id here
		mes "you will see the results win the vote is closed";
		if(getgroupid() >= 10 || .@show_result[.@s]){
			if(select("Result")){
				clear;
				callsub VoteResult,.@vote_id[.@s];
			}
		}
		close;
	}
	query_logsql("INSERT INTO `sader_vote_log` (`vote_id`,`unique_id`,`vote_option`) VALUES ('"+.@vote_id[.@s]+"', '"+.@player_unique_id$+"', '"+.@votes$[.@s2]+"')");
	mes "all done";
	if(getgroupid() >= 10 || .@show_result[.@s]){
		if(select("Result")){
			clear;
			callsub VoteResult,.@vote_id[.@s];
		}
	}
end;
VoteResult:
	.@voteid = getarg(0);
	query_sql("SELECT `vote_option1`,`vote_option2`,`vote_option3`,`vote_option4`,`vote_option5` FROM `sader_vote` WHERE `vote_id` = '" +.@voteid+"'",.@vote_option1$,.@vote_option2$,.@vote_option3$,.@vote_option4$,.@vote_option5$);
	query_logsql("SELECT `vote_option` FROM `sader_vote_log` WHERE `vote_id` = '" + .@voteid + "'",.@vote_option$);
	mes "[Result]";
	for(.@i = 1;.@i<6;.@i++){
		if(getd(".@vote_option" + .@i + "$") != ""){
			mes countinarray(getd(".@vote_option" + .@i + "$"),.@vote_option$) + " " + getd(".@vote_option" + .@i + "$");
		}
	}
end;
OnGMOptions:
	switch(select("Add Vote:Close Vote:Remove Vote:Show Vote Results")){
		case 1:
			mes "Vote Main Option";
				input .@vote_main_view$;
			if(select("continue:close") == 2)
				close;
			
			mes "Vote Message";
				input .@vote_message$;
			if(select("continue:close") == 2)
				close;
			
			mes "Vote Option 1";
				input .@vote_option1$;
			if(select("continue:close") == 2)
				close;
			
			mes "Vote Option 2";
				input .@vote_option2$;
			if(select("continue:close") == 2)
				close;
			
			mes "Vote Option 3";
				input .@vote_option3$;
			if(select("continue:close") == 2)
				close;
			
			mes "Vote Option 4";
				input .@vote_option4$;
			if(select("continue:close") == 2)
				close;
			
			mes "Vote Option 5";
				input .@vote_option5$;
			if(select("continue:close") == 2)
				close;
			clear;
			mes "Vote is Active ?";
			if(select("yes:no") == 1)
				.@vote_active$ = "Y";
			else
				.@vote_active$ = "N";
			
			mes "Show the Vote ?";
			if(select("yes:no") == 1)
				.@vote_show$ = "Y";
			else
				.@vote_show$ = "N";
			
			mes "Show result to the players after voting ?";
			if(select("yes:no") == 1)
				.@show_result = 1;
	
			clear;
			
			mes "[Vote Main Option]";
			mes .@vote_main_view$;
			mes "[Vote Message]";
			mes .@vote_message$;
			mes "[Vote Option 1]";
			mes .@vote_option1$;
			mes "[Vote Option 2]";
			mes .@vote_option2$;
			mes "[Vote Option 3]";
			mes .@vote_option3$;
			mes "[Vote Option 4]";
			mes .@vote_option4$;
			mes "[Vote Option 5]";
			mes .@vote_option5$;
			
			if(select("continue:close") == 2)
				close;
			for(.@i=1;.@i<6;.@i++){
				if(strtolower(getd(".@vote_option" + .@i + "$")) == "false"){
					setd ".@vote_option" + .@i + "$","";
				}
			}
			query_sql("INSERT INTO `sader_vote` (`vote_active`,`vote_show`,`vote_main_view`,`vote_message`,`vote_option1`,`vote_option2`,`vote_option3`,`vote_option4`,`vote_option5`,`show_result`) VALUES ('"+escape_sql(.@vote_active$)+"', '"+escape_sql(.@vote_show$)+"', '"+escape_sql(.@vote_main_view$)+"', '"+escape_sql(.@vote_message$)+"', '"+escape_sql(.@vote_option1$)+"', '"+escape_sql(.@vote_option2$)+"', '"+escape_sql(.@vote_option3$)+"', '"+escape_sql(.@vote_option4$)+"', '"+escape_sql(.@vote_option5$)+"', '"+escape_sql(.@show_result)+"')");
			
			mes "done!";
			close;

		case 2:
			query_sql("SELECT `vote_id`,`vote_active`,`vote_show`,`vote_main_view`,`vote_message`,`vote_option1`,`vote_option2`,`vote_option3`,`vote_option4`,`vote_option5` FROM `sader_vote` ",.@vote_id,.@vote_active$,.@vote_show$,.@vote_main_view$,.@vote_message$,.@vote_option1$,.@vote_option2$,.@vote_option3$,.@vote_option4$,.@vote_option5$);
			if(!getarraysize(.@vote_id)){
				mes "there is no votes yet!";
				close;
			}
			mes "select one";
			.@s = select(implode(.@vote_main_view$, ":"))-1;
			clear;
			switch(select("hide/show:close/open:show/hide result")){
				case 1:
					if(select("hide:show") == 1){
						.@vote_show$ = "N";
					}else{
						.@vote_show$ = "Y";
					}
					query_sql("UPDATE `sader_vote` SET `vote_show` = '" + .@vote_show$ +"' WHERE `vote_id` = '" + .@vote_id[.@s] + "'");
					break;
				case 2:
					if(select("close vote:open vote") == 1){
						.@vote_active$ = "N";
					}else{
						.@vote_active$ = "Y";
					}
					query_sql("UPDATE `sader_vote` SET `vote_active` = '" + .@vote_active$ +"' WHERE `vote_id` = '" + .@vote_id[.@s] + "'");
					break;
				case 3:
					if(select("show result:hide result") == 1){
						.@show_result = 1;
					}else{
						.@show_result = 0;
					}
					query_sql("UPDATE `sader_vote` SET `show_result` = '" + .@show_result +"' WHERE `vote_id` = '" + .@vote_id[.@s] + "'");
					break;
			}
			
			end;
		
		case 3:
			query_sql("SELECT `vote_id`,`vote_active`,`vote_show`,`vote_main_view`,`vote_message`,`vote_option1`,`vote_option2`,`vote_option3`,`vote_option4`,`vote_option5` FROM `sader_vote` ",.@vote_id,.@vote_active$,.@vote_show$,.@vote_main_view$,.@vote_message$,.@vote_option1$,.@vote_option2$,.@vote_option3$,.@vote_option4$,.@vote_option5$);
			if(!getarraysize(.@vote_id)){
				mes "there is no votes yet!";
				close;
			}
			mes "select one";
			.@s = select(implode(.@vote_main_view$, ":"))-1;
			clear;
			query_sql("DELETE FROM `sader_vote` WHERE `vote_id` = '" + .@vote_id[.@s] + "'");
			
			query_logsql("DELETE FROM `sader_vote_log` WHERE `vote_id` = '" + .@vote_id[.@s] + "'");
			
			end;
		case 4:
			query_sql("SELECT `vote_id`,`vote_active`,`vote_show`,`vote_main_view`,`vote_message`,`vote_option1`,`vote_option2`,`vote_option3`,`vote_option4`,`vote_option5` FROM `sader_vote` ",.@vote_id,.@vote_active$,.@vote_show$,.@vote_main_view$,.@vote_message$,.@vote_option1$,.@vote_option2$,.@vote_option3$,.@vote_option4$,.@vote_option5$);
			if(!getarraysize(.@vote_id)){
				mes "there is no votes yet!";
				close;
			}
			mes "select one";
			.@s = select(implode(.@vote_main_view$, ":"))-1;
			clear;
			callsub VoteResult,.@vote_id[.@s];
			end;
	}

end;

OnInit:
	waitingroom "[" + strnpcinfo(1) + "]",0;
	bindatcmd("Vote",strnpcinfo(3)+"::OnCommand",0,99);
	query_sql("CREATE TABLE IF NOT EXISTS `sader_vote` (`vote_id` int(11) NOT NULL AUTO_INCREMENT,`vote_active` enum('Y','N') DEFAULT NULL,`vote_show` enum('Y','N') DEFAULT NULL,`vote_main_view` varchar(100) DEFAULT NULL,`vote_message` varchar(100) DEFAULT NULL,`vote_option1` varchar(100) DEFAULT NULL,`vote_option2` varchar(100) DEFAULT NULL,`vote_option3` varchar(100) DEFAULT NULL,`vote_option4` varchar(100) DEFAULT NULL,`vote_option5` varchar(100) DEFAULT NULL,`show_result` int(11) NOT NULL DEFAULT '0',PRIMARY KEY (`vote_id`),UNIQUE KEY `vote_id_UNIQUE` (`vote_id`)) ENGINE=InnoDB  DEFAULT CHARSET=latin1 AUTO_INCREMENT=2 ;");
	query_logsql("CREATE TABLE IF NOT EXISTS `sader_vote_log` (`unique_id` int(11) unsigned NOT NULL DEFAULT '0',`vote_id` int(11) NOT NULL,`vote_option` varchar(100) NOT NULL) ENGINE=MyISAM DEFAULT CHARSET=latin1;");
}
