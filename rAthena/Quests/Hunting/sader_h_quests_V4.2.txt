//===== rAthena Script =======================================
//= saders hunting quest
//===== By: ==================================================
//= Sader1992
//https://rathena.org/board/profile/30766-sader1992/
//===== Current Version: =====================================
//= 4.2
//===== Compatible With: ===================================== 
//= rAthena Project
//https://rathena.org/board/files/file/3579-saders-hunting-quest/
//https://github.com/sader1992/sader_scripts
//===== Description: =========================================
//============================================================
//============================================================
prontera,155,176,4	script	sader q	667,{
	if(.accharlimit ==0){.@time_needed = #s_timeq - gettimetick(2);}else{.@time_needed = s_timeq - gettimetick(2);}
	if(.@time_needed > 0 && sader_quest == 0) {
		mes "You can only do the quest once every "+.s_timeqf+" hours.";
		.@hours   = .@time_needed / 60 / 60;
		.@minutes = (.@time_needed % (60 * 60)) / 60;
		.@seconds = .@time_needed % 60;
		mes "Please come back in " + sprintf("%02d:%02d:%02d", .@hours, .@minutes, .@seconds) + " hours." ;
		//next;
		if(.shopEnabled !=0){
			switch(select(""+"shop"+":"+"close"+"")){
						Case 1: callsub Q_shop;
						Case 2: close;
					}
		}else{
			close;
		}
		close;
	}else{
	if(!sader_quest){set sader_quest,0;}
	if(sader_quest != 0 ){ callsub have_sq;}
	if(sader_quest == 0 && quest_done == 0){ callsub need_sq;}
	if(quest_done != 0){callsub done_sq;}
	}
Q_easy:
	if (BaseLevel < .qs_elevel[0] || BaseLevel > .qs_elevel[1]){
		mes "Your level is too low or too high.";
		mes "   ";
		mes "Minimum level "+.qs_elevel[0]+".";
		mes "Maximum level "+.qs_elevel[1]+".";
		close;
	}
	mes " plz kill "+ easycmon +" from " + strmobinfo( 1,easymon) +".";
	set sader_quest,1;
	set monster_count,0;
	close;
end;		
Q_normal:
	if (BaseLevel < .qs_nlevel[0] || BaseLevel > .qs_nlevel[1]){
		mes "Your level is too low or too high.";
		mes "   ";
		mes "Minimum level "+.qs_nlevel[0]+".";
		mes "Maximum level "+.qs_nlevel[1]+".";
		close;
	}
	mes " plz kill "+ normalcmon +" from " + strmobinfo( 1,normalmon) +".";
	set sader_quest,2;
	set monster_count,0;
	close;
end;
Q_hard:
	if (BaseLevel < .qs_hlevel[0] || BaseLevel > .qs_hlevel[1]){
		mes "Your level is too low or too high.";
		mes "   ";
		mes "Minimum level "+.qs_hlevel[0]+".";
		mes "Maximum level "+.qs_hlevel[1]+".";
		close;
	}
	mes " plz kill "+ hardcmon +" from " + strmobinfo( 1,hardmon) +".";
	set sader_quest,3;
	set monster_count,0;
	close;
end;
Q_indeterminate:
	if (BaseLevel < .qs_ilevel[0] || BaseLevel > .qs_ilevel[1]){
		mes "Your level is too low or too high.";
		mes "   ";
		mes "Minimum level "+.qs_ilevel[0]+".";
		mes "Maximum level "+.qs_ilevel[1]+".";
		close;
	}
	mes " plz kill "+ indeterminatecmon +" from " + strmobinfo( 1,indeterminatemon) +".";
	set sader_quest,4;
	set monster_count,0;
	close;
end;
Q_shop:
	mes "You have ^00F9FF" + #SdM_PQ + "^000000 sader quest points.";
	callshop "sader q s",1;
end;
OnNPCKillEvent:
if(sader_quest != 0){
	if( killedrid == easymon ){
		if(monster_count < easycmon){
			monster_count++;
			dispbottom "You killed "+monster_count+" "+ strmobinfo( 1,easymon) +".";
			if(monster_count >= easycmon){
				dispbottom "Your quest is done";
				set quest_done,1;
				set sader_quest,0;
				if(.complete_without_npc == 1){
					set quest_done,0;
					set #SdM_PQ,#SdM_PQ + .easy_points;
					callsub sader_q_eitem;
				}
			}
		}
	}else if( killedrid == normalmon ){
		if(monster_count < normalcmon){
			monster_count++;
			dispbottom "You killed "+monster_count+" "+ strmobinfo( 1,normalmon) +".";
			if(monster_count >= normalcmon){
				dispbottom "Your quest is done";
				set quest_done,2;
				set sader_quest,0;
				if(.complete_without_npc == 1){
					set quest_done,0;
					set #SdM_PQ,#SdM_PQ + .normal_points;
					callsub sader_q_nitem;
				}
			}
		}
	}else if( killedrid == hardmon ){
		if(monster_count < hardcmon){
			monster_count++;
			dispbottom "You killed "+monster_count+" "+ strmobinfo( 1,hardmon) +".";
			if(monster_count >= hardcmon){
				dispbottom "Your quest is done";
				set quest_done,3;
				set sader_quest,0;
				if(.complete_without_npc == 1){
					set quest_done,0;
					set #SdM_PQ,#SdM_PQ + .hard_points;
					callsub sader_q_hitem;
				}
			}
		}
	}else if( killedrid == indeterminatemon ){
		if(monster_count < indeterminatecmon){
			monster_count++;
			dispbottom "You killed "+monster_count+" "+ strmobinfo( 1,indeterminatemon) +".";
			if(monster_count >= indeterminatecmon){
				dispbottom "Your quest is done";
				set quest_done,4;
				set sader_quest,0;
				if(.complete_without_npc == 1){
					set quest_done,0;
					set #SdM_PQ,#SdM_PQ + .indeterminate_points;
					callsub sader_q_iitem;
				}
			}
		}
	}
}
end;
have_sq:	
		mes "hi, how can i hep you ?";
		if(sader_quest == 1) {mes " plz kill "+ easycmon +" from " + strmobinfo( 1,easymon) +".";
		mes "you have killed "+monster_count+" "+ strmobinfo( 1,easymon) +".";}
		if(sader_quest == 2) {mes " plz kill "+ normalcmon +" from " + strmobinfo( 1,normalmon) +".";
		mes "you have killed "+monster_count+" " + strmobinfo( 1,normalmon) +".";}
		if(sader_quest == 3) {mes " plz kill "+ hardcmon +" from " + strmobinfo( 1,hardmon) +".";
		mes "you have killed "+monster_count+" " + strmobinfo( 1,hardmon) +".";}
		if(sader_quest == 4) {mes " plz kill "+ indeterminatecmon +" from " + strmobinfo( 1,indeterminatemon) +".";
		mes "you have killed "+monster_count+" " + strmobinfo( 1,indeterminatemon) +".";}
		next;
		if(.shopEnabled !=0){
			switch(select(""+"shop"+":"+"Cansel the Quest"+":"+"close"+"")){
						Case 1: callsub Q_shop;
						Case 2:	mes "are you sure ?";
						switch(select(""+"yes"+":"+"no"+"")){
							Case 1:
							mes "AS YOU WISH";
							set sader_quest,0;
							if(.accharlimit ==0){#s_timeq = gettimetick(2) + (.s_timeqf * 3600);}else{s_timeq = gettimetick(2) + (.s_timeqf * 3600);}
							close;
							Case 2: close;
						}
						Case 3: close;
			}
		}else{
			switch(select(""+"Cansel the Quest"+":"+"close"+"")){
						Case 1: mes "are you sure ?";
						switch(select(""+"yes"+":"+"no"+"")){
							Case 1:
							mes "AS YOU WISH";
							set sader_quest,0;
							if(.accharlimit ==0){#s_timeq = gettimetick(2) + (.s_timeqf * 3600);}else{s_timeq = gettimetick(2) + (.s_timeqf * 3600);}
							close;
							Case 2: close;
						}
						Case 3: close;
			}
		}
end;
need_sq:
	easymon = .easy[rand(getarraysize(.easy))];
	easycmon = .easyc[rand(getarraysize(.easyc))];
	normalmon = .normal[rand(getarraysize(.normal))];
	normalcmon = .normalc[rand(getarraysize(.normalc))];
	hardmon = .hard[rand(getarraysize(.hard))];
	hardcmon = .hardc[rand(getarraysize(.hardc))];
	indeterminatemon = .indeterminate[rand(getarraysize(.indeterminate))];
	indeterminatecmon = .indeterminatec[rand(getarraysize(.indeterminatec))];
			mes "hello";
			mes "i will give you a task";
			mes "to kill some monsters";
			mes "can you help us ?";
			next;
				if(.shopEnabled !=0){
					switch(select(""+"easy difficulty"+":"+"normal difficulty"+":"+"hard difficulty"+":"+"indeterminate difficulty"+":"+"shop"+":"+"close"+"")){
						Case 1: callsub Q_easy;
						Case 2: callsub Q_normal;
						Case 3: callsub Q_hard;
						Case 4: callsub Q_indeterminate;
						Case 5: callsub Q_shop;
						Case 6: close;
					}
				}else{
					switch(select(""+"easy difficulty"+":"+"normal difficulty"+":"+"hard difficulty"+":"+"indeterminate difficulty"+":"+"close"+"")){
						Case 1: callsub Q_easy;
						Case 2: callsub Q_normal;
						Case 3: callsub Q_hard;
						Case 4: callsub Q_indeterminate;
						Case 5: close;
					}
				}
end;
done_sq:
	mes "Thank you for helping me .";
	set monster_count,0;
	set sader_quest,0;
	if(.accharlimit ==0){#s_timeq = gettimetick(2) + (.s_timeqf * 3600);}else{s_timeq = gettimetick(2) + (.s_timeqf * 3600);}
		if(quest_done == 1){set #SdM_PQ,#SdM_PQ + .easy_points; set quest_done,0; callsub sader_q_eitem;}
		else if(quest_done == 2){set #SdM_PQ,#SdM_PQ + .normal_points; set quest_done,0; callsub sader_q_nitem;}
		else if(quest_done == 3){set #SdM_PQ,#SdM_PQ + .hard_points; set quest_done,0; callsub sader_q_hitem;}
		else if(quest_done == 4){set #SdM_PQ,#SdM_PQ + .indeterminate_points; set quest_done,0; callsub sader_q_iitem;}
	
	close;
end;
sader_q_eitem: //item rewards leave it empty if you don't want it you can add item exp etc
	//getitem 501,20;//item_id,amount
	//getexp 1000,1000;//base,job
	//for easy mode
end;
sader_q_nitem: //item rewards leave it empty if you don't want it you can add item exp etc
	//for normal mode
end;
sader_q_hitem: //item rewards leave it empty if you don't want it you can add item exp etc
	//for hard mode
end;
sader_q_iitem: //item rewards leave it empty if you don't want it you can add item exp etc
	//for indeterminate mode
end;
//============================================================
//========================| Config |==========================
//============================================================	
OnInit:
	////v2.0 update
	setarray .easy,1275,1737;	//Easy Mode Monsters ID (you can add as many as you want IDs ID,ID,ID,ID;)
	setarray .easyc,50,75;	//Easy Mode Monsters Count((Random)) (you can add as many as you want Numbers Number,Number,Number,Number;)
	setarray .normal,1275,1737;	//Normal Mode Monsters ID (you can add as many as you want IDs ID,ID,ID,ID;)
	setarray .normalc,100,150;	//Normal Mode Monsters Count((Random)) (you can add as many as you want Numbers Number,Number,Number,Number;)
	setarray .hard,1735,1736;	//Hard Mode Monsters ID (you can add as many as you want IDs ID,ID,ID,ID;)
	setarray .hardc,75,100,150;	// Hard Mode Monsters Count((Random)) (you can add as many as you want Numbers Number,Number,Number,Number;)
	setarray .indeterminate,1735,1736;	//Indeterminate Mode Monsters ID (you can add as many as you want IDs ID,ID,ID,ID;)
	setarray .indeterminatec,250,300,350;	// Indeterminate Mode Monsters Count((Random)) (you can add as many as you want Numbers Number,Number,Number,Number;)
	.complete_without_npc = 0;	//if you want to complete the quest without the npc set it to 1
	.easy_points = 50;	//the point rewards for easy mode
	.normal_points = 100;	//the point rewards for normal mode
	.hard_points = 150;	//the point rewards for hard mode
	.indeterminate_points = 200;	//the point rewards for indeterminate mode
	.s_timeqf = 10;	//quest daley in hr
	////v3.0 update
	.accharlimit = 0;	// 0 = once per account , 1 = once pet character
	////v4.1 update
	.shopEnabled = 1;	// 0 = Disable shop , 1 = Enable shop
	////v4.2 update
	setarray .qs_elevel,1,175;	//the Minimum level and the Maximum level for easy mode quest (minimum,maximum)
	setarray .qs_nlevel,1,175;	//the Minimum level and the Maximum level for normal mode quest (minimum,maximum)
	setarray .qs_hlevel,1,175;	//the Minimum level and the Maximum level for hard mode quest (minimum,maximum)
	setarray .qs_ilevel,1,175;	//the Minimum level and the Maximum level for indeterminate mode quest (minimum,maximum)
	////
	setarray .sader_q_shop[0],512,20,513,30,514,40; // rewards in the shop <item_id>,<prise>,<item_id>,<prise>,<item_id>,<prise>;
	npcshopdelitem "sader q s",512;
	for (.@i = 0; .@i < getarraysize(.sader_q_shop); .@i += 2)
		npcshopadditem "sader q s", .sader_q_shop[.@i], .sader_q_shop[.@i+1];
	end;
}
-	pointshop	sader q s	-1,#SdM_PQ,512:1; //set the items you need in your shop
