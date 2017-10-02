//===== rAthena Script =======================================
//= saders Reward
//===== By: ==================================================
//= Sader1992
//https://rathena.org/board/profile/30766-sader1992/
//===== Current Version: =====================================
//= 1.3b
//===== Compatible With: ===================================== 
//= rAthena Project
//https://rathena.org/board/files/file/3623-saders-reward-manager/
//https://github.com/sader1992/sader_scripts
//===== Description: =========================================
//==== all the configuration from the npc in game
//==== you can change the GM level in the start of the script
//==== if(getgroupid() >= 90 ){ change the 90 to what you want
//==== support gepard / ip / or without them
//==== support rewards for vip only
//==== support max players can get the reward
//==== support up to 10 items per 1 variable
//==== reward name will be seen from the normal players when they get the reward
//==== please send me a message if you find error
//==== if you like my work maybe consider support me at paypal
//==== sader1992@gmail.com
//============================================================
//==== 1.1 Fix IP Check , add check for if the item id wrong , you can now edit the reward options from the npc(Name , Abuse Protection , Max Playrs , VIP) .
//==== 1.2 Adding Weight check (you have to be less then 80% Weight to get the rewards) , Fix looping massege , Remove Adding more items option when the item count is 10 .
//==== 1.3a Improve Gepard and IP Protection (i recommend removeing all the rewords before updating if you use it) , adding command @MyReward to access to the npc (any player can use this command) .
//==== 1.3b Adding log database 'sader_variables_log' you can check who did get his reward from the npc , Changing some text .
//============================================================
prontera,153,176,5	script	sader Reward	997,{
OnMyReward:
	if(getgroupid() >= 90 ){
		switch( select("Add Reward:Check and Remove a Reward:Claim Reward:Close") ){
					case 1: callsub S_Rest_Value; callsub S_AddReward; end;
					case 2: callsub S_CheckRewards; end;
					case 3: callsub S_ClaimReward; end;
					case 4:
		}
	}else{
		callsub S_ClaimReward; end;
	}
end;
S_ClaimReward:
	query_sql( "SELECT `value` FROM `sader_rewards` ", .@s_Value_Name$ );
	for(.@i = 0;.@i < getarraysize(.@s_Value_Name$);.@i++){
		.@checkp = 0;
		deletearray .@s_player_num;
		query_sql("SELECT `name`,`gepard`,`vip`,`max_players`,`itemid1`, `amount1`, `itemid2`, `amount2`, `itemid3`, `amount3`, `itemid4`, `amount4`, `itemid5`, `amount5`, `itemid6`, `amount6`, `itemid7`, `amount7`, `itemid8`, `amount8`, `itemid9`, `amount9`, `itemid10`, `amount10` FROM `sader_rewards` WHERE `value` = '" +.@s_Value_Name$[.@i]+ "' ",.@s_name$,.@s_gepard,.@s_vip,.@s_max_players,.@s_Item_id1,.@s_Item_Amount1,.@s_Item_id2,.@s_Item_Amount2,.@s_Item_id3,.@s_Item_Amount3,.@s_Item_id4,.@s_Item_Amount4,.@s_Item_id5,.@s_Item_Amount5,.@s_Item_id6,.@s_Item_Amount6,.@s_Item_id7,.@s_Item_Amount7,.@s_Item_id8,.@s_Item_Amount8,.@s_Item_id9,.@s_Item_Amount9,.@s_Item_id10,.@s_Item_Amount10);
		query_sql("SELECT `account_id` FROM `acc_reg_num` WHERE `key` = '" +.@s_Value_Name$[.@i]+"' AND `value` = '1'",.@s_player_num);
		if(getarraysize(.@s_player_num) < .@s_max_players){
			.@checkp += 1;
		}
		if(getd(.@s_Value_Name$[.@i]) != 1){
			.@checkp += 2;
		}
		if(.@s_vip == 1){
			if(vip_status(VIP_STATUS_ACTIVE)){
			.@checkp += 4;
			}
		}else{
			.@checkp += 4;
		}
		if(.@s_gepard == 1){
			query_sql("SELECT `last_unique_id` FROM `login` WHERE `account_id` = '"+getcharid(3)+"'", .@s_last_unique_id$);
			query_logsql("SELECT value FROM `sader_variables_log` WHERE unique_id = '"+@s_last_unique_id$+"' AND `variable` = '"+.@s_Value_Name$[.@i]+"'", .@unique_id);
			if(.@unique_id == 0){
				.@checkp += 8;
			}
		}else if(.@s_gepard == 2){
			query_logsql("SELECT value FROM `sader_variables_log` WHERE ip = '"+getcharip()+"' AND `variable` = '"+.@s_Value_Name$[.@i]+"'", .@unique_ip);
			if(.@unique_ip == 0){
				.@checkp += 8;
			}
		}else{
			.@checkp += 8;
		}
		if(.@checkp & 1 && .@checkp & 2 && .@checkp & 4 && .@checkp & 8){
			if (((Weight*100)/MaxWeight) < 79) {
				for(.@s=1;.@s<11;.@s++){
					if(getd(".@s_Item_id" + .@s) != 0){
						getitem getd(".@s_Item_id" + .@s),getd(".@s_Item_Amount" + .@s);	
					}
				}
				setd .@s_Value_Name$[.@i],1;
				query_logsql("INSERT INTO `sader_variables_log` (`unique_id`,`ip`,`variable`,`index`,`value`, `account_id`, `char_id`, `char_name`) VALUES ('"+.@s_last_unique_id$+"', '"+getcharip()+"', '"+.@s_Value_Name$[.@i]+"', '0', '"+getd(.@s_Value_Name$[.@i])+"', '"+getcharid(3)+"', '"+getcharid(0)+"', '"+strcharinfo(0)+"')");
				message strcharinfo(0),"You Got " + .@s_name$ + " Reward.";
				.@get = 1;
			}else{
				mes"Woow!";
				mes"how you can move like that !!?";
				mes"you must be less then 80% Weight to get the Reward;";
				break;
			}
		}
	}
	if(.@get){message strcharinfo(0),"[Rewards Manager]: You got all your Rewards.";}
	else{message strcharinfo(0),"[Rewards Manager]: You Don't have any new Rewards for now.";}
end;
S_CheckRewards:
	query_sql( "SELECT `value` FROM `sader_rewards` ", .@s_Value_Name$ );
	for(.@i = 0;.@i < getarraysize(.@s_Value_Name$);.@i++){
		deletearray .@s_player_num;
		query_sql("SELECT `name`,`gepard`,`vip`,`max_players`,`itemid1`, `amount1`, `itemid2`, `amount2`, `itemid3`, `amount3`, `itemid4`, `amount4`, `itemid5`, `amount5`, `itemid6`, `amount6`, `itemid7`, `amount7`, `itemid8`, `amount8`, `itemid9`, `amount9`, `itemid10`, `amount10` FROM `sader_rewards` WHERE `value` = '" +.@s_Value_Name$[.@i]+ "' ",.@s_name$,.@s_gepard,.@s_vip,.@s_max_players,.@s_Item_id1,.@s_Item_Amount1,.@s_Item_id2,.@s_Item_Amount2,.@s_Item_id3,.@s_Item_Amount3,.@s_Item_id4,.@s_Item_Amount4,.@s_Item_id5,.@s_Item_Amount5,.@s_Item_id6,.@s_Item_Amount6,.@s_Item_id7,.@s_Item_Amount7,.@s_Item_id8,.@s_Item_Amount8,.@s_Item_id9,.@s_Item_Amount9,.@s_Item_id10,.@s_Item_Amount10);
		query_sql("SELECT `account_id` FROM `acc_reg_num` WHERE `key` = '" +.@s_Value_Name$[.@i]+"' AND `value` = '1'",.@s_player_num);
		mes "The Name is : " + .@s_name$ + " .";
		mes "The Value is : " + .@s_Value_Name$[.@i] + " .";
		if(.@s_gepard == 1){mes "Abuse Protection : Gepard.";}else if(.@s_gepard == 2){mes "Abuse Protection : IP.";}else{mes "Abuse Protection : No Protection.";}
		if(.@s_vip ==1){mes "VIP : ON";}else{mes "VIP : OFF";}
		mes "Max Player : " + .@s_max_players + " .";
		mes "[ " + getarraysize(.@s_player_num) + " ] has got the reward.";
		for(.@a=1;.@a<11;.@a++){
			if(getd(".@s_Item_id" + .@a) != 0){
				mes getitemname(getd(".@s_Item_id" + .@a)) + " = " + getd(".@s_Item_Amount" + .@a);
			}
		}
		switch( select("Next:Edit:Delete:close") ){
			case 1: next; break;
			case 2:
			next;
				mes"what you want to edit ?";
				switch(select("Name:Max Players:Abuse Protection:VIP")){
					case 1:
						mes"input the New Name";
						input .@e_s_Name$;
						mes"The Name will be "+.@e_s_Name$;
						mes"Are you sure?";
							if(select("yes:no") == 1){query_sql("UPDATE `sader_rewards` SET name= '" +.@e_s_Name$ +"' WHERE value ='"+.@s_Value_Name$[.@i]+"'");}
							close;
					case 2:
						mes"input the New max players number who will get the reward";
						input .@e_s_max_players;
						mes"The Max Players number will be "+.@e_s_max_players;
						mes"Are you sure?";
							if(select("yes:no") == 1){query_sql("UPDATE `sader_rewards` SET max_players= '" +.@e_s_max_players +"' WHERE value ='"+.@s_Value_Name$[.@i]+"'");}
							close;
					case 3:
						mes"New Abuse Protection";
						switch( select("Gepard:IP:No Protection") ){case 1: set .@e_s_gepard,1;break; case 2: set .@e_s_gepard,2;break; case 3: set .@e_s_gepard,0;break;}
						if(.@e_s_gepard == 1){mes "Abuse Protection : Gepard.";}else if(.@e_s_gepard == 2){mes "Abuse Protection : IP.";}else{mes "Abuse Protection : No Protection.";}
						mes"Are you sure?";
							if(select("yes:no") == 1){query_sql("UPDATE `sader_rewards` SET gepard= '" +.@e_s_gepard +"' WHERE value ='"+.@s_Value_Name$[.@i]+"'");}
							close;
					case 4:
						mes "VIP ?"; 
						switch( select("Disable:Enable") ){case 1: set .@e_s_vip,0;break; case 2: set .@e_s_vip,1;break;}
						if(.@e_s_vip ==1){mes "VIP : ON";}else{mes "VIP : OFF";}
						mes"Are you sure?";
							if(select("yes:no") == 1){query_sql("UPDATE `sader_rewards` SET vip= '" +.@e_s_vip +"' WHERE value ='"+.@s_Value_Name$[.@i]+"'");}
							close;
				}
			case 3:
				mes "Are You Sure ?";
					if( select("yes:no") ==1 ){
						query_sql("DELETE FROM `sader_rewards` WHERE `value` = '" +.@s_Value_Name$[.@i]+ "'"); end;
					}
			case 4:	
		}
	}
	close;
end;
S_AddReward:
	callsub S_Rest_Value;
	mes "input Name";
	mes "The reward Name will be seen by the players.";
	input @s_Name$;
	mes "Input Variable";
	mes"Example : #SADER";
	mes"when it's start with # it's an account variable";
	mes"Example : SADER";
	mes"when it's start with nothing it's a charcter variable";
	input @s_Value_Name$;
	mes "Max Player";
	input @s_max_players;
	mes "Abuse Protection";
	switch( select("Gepard:IP:No Protection") ){case 1: set @s_gepard,1;break; case 2: set @s_gepard,2;break; case 3: set @s_gepard,0;break;}
	mes "VIP ?";
	switch( select("No:Yes") ){case 1: set @s_vip,0;break; case 2: set @s_vip,1;break;}
	next;
	for(.@i=1;.@i<11;.@i++){
		mes "Input ITEM ID " + .@i + " .";
		input @s_Item_id[.@i];
		if ( getitemname( @s_Item_id[.@i] ) == "null" ) {
			next;
			mes "The item ID not right please re input the item id";
			set .@i,.@i-1;
		}else{
			next;
			mes "Input ITEM " + .@i + " Amount .";
			input @s_Item_Amount[.@i];
			if(@s_Item_Amount[.@i] == 0){
				mes "you can't use item amount 0 it must be 1 or more!";
				mes "please start re enter the item id and the right amount";
				next;
				set .@i,.@i-1;
			}else{
				set @s_item_count,.@i;
				next;
				callsub s_addmore;
			}
		}
	}
s_addmore:
	if(@s_item_count < 10){
		switch( select("Add More items:No More items:Close") ){
			case 1: return;
			case 2: break;
			case 3: close;
		}
	}else{ switch( select("No More items:Close") == 1 ){ case 1: break; case 2: close;} }
	mes "Are You Sure ?";
	mes "The Name is : " + @s_Name$ + " .";
	mes "The Variable is : " + @s_Value_Name$ + " .";
	if(@s_gepard == 1){mes "Abuse Protection : Gepard.";}else if(@s_gepard == 2){mes "Abuse Protection : IP.";}else{mes "Abuse Protection : No Protection.";}
	if(@s_vip ==1){mes "VIP : ON";}else{mes "VIP : OFF";}
	mes "Max Player : " + @s_max_players + " .";
	for(.@i=1;.@i<11;.@i++){
		if ( getitemname( @s_Item_id[.@i] ) != "null" ) {
			mes getitemname(@s_Item_id[.@i]) + " = " + @s_Item_Amount[.@i];
		}
	}
	if( select("yes:no") ==1 ){
		query_sql("INSERT INTO `sader_rewards` (`value`,`name`,`gepard`,`vip`,`max_players`, `itemid1`, `amount1`, `itemid2`, `amount2`, `itemid3`, `amount3`, `itemid4`, `amount4`, `itemid5`, `amount5`, `itemid6`, `amount6`, `itemid7`, `amount7`, `itemid8`, `amount8`, `itemid9`, `amount9`, `itemid10`, `amount10`) VALUES ('"+@s_Value_Name$+"', '"+@s_Name$+"', '"+@s_gepard+"', '"+@s_vip+"', '"+@s_max_players+"', '"+@s_Item_id[1]+"', '"+@s_Item_Amount[1]+"', '"+@s_Item_id[2]+"', '"+@s_Item_Amount[2]+"', '"+@s_Item_id[3]+"', '"+@s_Item_Amount[3]+"', '"+@s_Item_id[4]+"', '"+@s_Item_Amount[4]+"', '"+@s_Item_id[5]+"', '"+@s_Item_Amount[5]+"', '"+@s_Item_id[6]+"', '"+@s_Item_Amount[6]+"', '"+@s_Item_id[7]+"', '"+@s_Item_Amount[7]+"', '"+@s_Item_id[8]+"', '"+@s_Item_Amount[8]+"', '"+@s_Item_id[9]+"', '"+@s_Item_Amount[9]+"', '"+@s_Item_id[10]+"', '"+@s_Item_Amount[10]+"')");
		mes"Done .";																																																																											//`value`,`name`,`gepard`,`vip`,`max_players`,	@s_Name$		@s_max_players		@s_gepard		@s_vip													
	}
	callsub S_Rest_Value;
	close;
end;
S_Rest_Value:
	deletearray @s_Item_id;
	deletearray @s_Item_Amount;
	deletearray @s_Name$;
	deletearray @s_Value_Name$;
	deletearray @s_max_players;
	deletearray @s_gepard;
	deletearray @s_vip;
	return;
end;
OnInit:
	query_sql("CREATE TABLE IF NOT EXISTS `sader_rewards` (`name` VARCHAR(32) NOT NULL,`value` VARCHAR(32) NOT NULL, `gepard` ENUM('0','1','2') NOT NULL, `vip` ENUM('0','1') NOT NULL,`max_players` INT NOT NULL,`itemid1` INT NOT NULL, `amount1` INT NOT NULL, `itemid2` INT NOT NULL, `amount2` INT NOT NULL, `itemid3` INT NOT NULL, `amount3` INT NOT NULL, `itemid4` INT NOT NULL, `amount4` INT NOT NULL, `itemid5` INT NOT NULL, `amount5` INT NOT NULL, `itemid6` INT NOT NULL, `amount6` INT NOT NULL, `itemid7` INT NOT NULL, `amount7` INT NOT NULL, `itemid8` INT NOT NULL, `amount8` INT NOT NULL, `itemid9` INT NOT NULL, `amount9` INT NOT NULL, `itemid10` INT NOT NULL, `amount10` INT NOT NULL , UNIQUE `value` (`value`(32))) ENGINE=MyISAM");
	query_logsql("CREATE TABLE IF NOT EXISTS `sader_variables_log` (`account_id` INT NOT NULL,`char_id` INT NOT NULL,`char_name` VARCHAR(30) NOT NULL,`unique_id` INT( 11 ) UNSIGNED NOT NULL DEFAULT  '0',`ip` VARCHAR(100) NOT NULL,`variable` VARCHAR(32) NOT NULL, `index` INT NOT NULL, `value` INT NOT NULL) ENGINE=MyISAM");
	bindatcmd("MyReward",strnpcinfo(3)+"::OnMyReward",0,99);
end;
}