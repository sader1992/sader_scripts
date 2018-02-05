//===== rAthena Script =======================================
//= saders Mini Endless Tower 25 level
//===== By: ==================================================
//= Sader1992
//https://rathena.org/board/profile/30766-sader1992/
//===== Current Version: =====================================
//= 1.0
//===== Compatible With: ===================================== 
//= rAthena Project
//https://github.com/sader1992/sader_scripts
//===== Description: =========================================
//==== cooldown is 1day can be changed via rathena/db/import/quest_db.txt (86400 == 1 day)
//==== you can add rewards in the start of the script function named 'F_Mini_Endless_Tower_Rewards'
//==== Support Gepard! (see line 185) or search for ( .gepard = false; ) set it to true to use it
//============================================================
//==== please send me a message if you find error
//============================================================
//============================================================

/*

///SERVER SIDE

rathena/db/import/instance_db.txt
100,Mini Endless Tower,14400,300,1@Mtower,50,355

rathena/db/import/quest_db.txt
150000,14400,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,"Mini Endless Tower Time Limit"
150001,86400,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,"Mini Endless Tower Effect"

rathena/db/import/map_index.txt
1@Mtower

rathena/conf/maps_athena.conf
map: 1@Mtower

(DON'T FORGET TO BUILD THE mapcache.dat !)

///CLIENT SIDE

data/questid2display.txt
150000#Mini Endless Tower Time Limit#SG_FEEL#QUE_NOIMAGE#
The time limit for access to the Mini Endless Tower is 4 hours.#
#
150001#Mini Endless Tower Effect#SG_FEEL#QUE_NOIMAGE#
The Mini Endless Tower has strange after-effect properties that only allow you to enter it once a day. Wait until the time limit is up before re-entering again.#
#
data/resnametable.txt
1@mtower.gnd#1@tower.gnd#
1@mtower.gat#1@tower.gat#
1@mtower.rsw#1@tower.rsw#
유저인터페이스\map\1@mtower.bmp#유저인터페이스\map\1@tower.bmp#

*/
function	script	F_Mini_Endless_Tower_Rewards	{
	//you can add rewards here !! 
	//getitem 502,10;	//	10 apple
	//set Zeny,Zeny + 1;	//	1 Zeny
	//getexp 10000,5000;	//	10K Base EXP AND 5K Job EXP
	
	return;
}
prontera,155,186,3	script	Mini Endless Tower	406,{
	if(getgroupid() >= 90 ){
		mes"^C70847[GM OPTIONS]^000000";
		mes"^C70847you are GM^000000";
		mes"^C70847do you want to reset the tower for your char?^000000";
		switch(select("^C70847skip^000000:^C70847reset the tower^000000:^C70847end the tower that running^000000:^C70847Gepard Reset")){
			case 1:
				break;
			case 2:
				if(isbegin_quest(150000))
					erasequest 150000;
				if(isbegin_quest(150001))
					erasequest 150001;
				close;
			case 3:
				if(instance_id())
					instance_destroy;
				close;
			case 4:
				query_sql("SELECT `last_unique_id` FROM `login` WHERE `char_id` = '"+getcharid(3)+"'", .@last_unique_id$);
				query_sql("delete from `MiniEtower` where `unique_id` = '"+.@last_unique_id$+"'");
				close;
		}
		next;
	}
	if(checkquest(150001,PLAYTIME) == 2){
		erasequest 150000;
		erasequest 150001;
	}
	if(!isbegin_quest(150001)){
		set MiniEtower_partyid,0;
	}
	if(checkquest(150000,PLAYTIME) == 1 && checkquest(150001,PLAYTIME) == 0){
		mes "you have the Mini Endless Tower Effect";
		mes "you can't enter again until the effect is removed";
		close;
	}
	if(!getcharid(1)){
		mes "Where is your party ?";
		close;
	}
	if (getcharid(0) == getpartyleader(getcharid(1),2)) {
		mes "Confirmed the party has been made. Would you like to reserve entrance to the Mini Endless Tower?";
		next;
		switch(select("Generate dungeon Mini Endless Tower:Enter the dungeon:Cancel")) {
		case 1:
			if(isbegin_quest(150001)){
				mes "You have Mini Endless Tower Effect";
				mes "You can't make instance before it go from you";
				mes "Check the Quest Log for more information";
				close;
			}
			if (instance_create("Mini Endless Tower") < 0) {
				mes "Party Name: "+ getpartyname(.@party_id);
				mes "Party Leader: "+strcharinfo(0);
				mes "^0000ffMini Endless Tower ^000000- Reservation Failed!";
				close;
			}
			mes "^0000ffMini Endless Tower^000000 - Try to reserve";
			mes "After making a reservation, you have to talk to NPC behind and select the menu 'Enter the Dungeon' to enter the dungeon.";
			close;
		case 2:
			callsub L_Enter;
		case 3:
			close;
		}
	}
	switch(select("Enter the Mini Endless Tower:Cancel")) {
	case 1:
		callsub L_Enter;
	case 2:
		end;
	}
L_Enter:
	if(MiniEtower_partyid != getcharid(1) && MiniEtower_partyid){
		mes"You Changed your Party";
		mes"i am not a fool like you !";
		close;
	}
	if(.gepard){
		query_sql("SELECT `last_unique_id` FROM `login` WHERE `char_id` = '"+getcharid(3)+"'", .@last_unique_id$);
		query_sql("SELECT `time`,`char_id` FROM `MiniEtower` WHERE `unique_id` = '"+.@last_unique_id$+"'", .@gptime ,.@charid);
		.@all_time = (.@gptime - gettimetick(2));
		if(.@all_time > 0 && .@charid != getcharid(1) ){
			mes"you can't enter yet";
			mes"you already did enter on another account";
			.@hours   = .@all_time / 60 / 60;
			.@minutes = (.@all_time % (60 * 60)) / 60;
			.@seconds = .@all_time % 60;
			mes "Please come back in " + sprintf("%02d:%02d:%02d", .@hours, .@minutes, .@seconds) + " hours." ;
			close;
		}
	}
	switch(instance_enter("Mini Endless Tower")) {
	case IE_OTHER:
		mes "An unknown error has occurred.";
		close;
	case IE_NOINSTANCE:
		mes "The memorial dungeon Mini Endless Tower does not exist.";
		mes "The party leader did not generate the dungeon yet.";
		close;
	case IE_NOMEMBER:
		mes "You can enter the dungeon after making the party.";
		close;
	case IE_OK:
		mapannounce "e_tower", strcharinfo(0) +" of the party, "+ getpartyname( getcharid(1) ) +", is entering the dungeon, Mini Endless Tower.",bc_map,"0x00ff99",FW_NORMAL,12;
		if(checkquest(150001,PLAYTIME) == -1){
			if(.gepard){
				.@cooldown = (86400 + gettimetick(2));
				query_sql("delete from `MiniEtower` where `unique_id` = '"+.@last_unique_id$+"'");
				query_sql("INSERT INTO `MiniEtower` (`unique_id`,`time`,`char_id`) VALUES ('"+.@last_unique_id$+"', '"+.@cooldown+"', '"+getcharid(1)+"')");
			}
			set MiniEtower_partyid, getcharid(1);
			setquest 150000;
			setquest 150001;
			set Floor21,0;
			set Floor25,0;
		}
	}
end;
	
OnInit:
	.gepard = false;	//	if you want to use gepaard and have gepard on your server set this to true
	if(.gepard){
		query_sql("CREATE TABLE IF NOT EXISTS `MiniEtower` (`unique_id` INT( 11 ) UNSIGNED NOT NULL DEFAULT '0',`char_id` INT NOT NULL,`time` INT NOT NULL) ENGINE=MyISAM");
	}
}


1@Mtower,50,360,0	script	MImmortal Brazier#	844,{
	if(getgroupid() >= 90 ){
		mes"^C70847[GM OPTIONS]^000000";
		mes"^C70847Jump to Floor ?^000000";
		if(select("^C70847yes^000000:^C70847no^000000") == 1){
			input .@floor;
			if(.@floor <= 25){
				set .@map$, strnpcinfo(4);
				set .@level,.@floor;
				set .@level, atoi(replacestr(strnpcinfo(0),"FMGate102tower","")) +(.@floor - 1);
				mapannounce strnpcinfo(4), "All Monsters on the "+callfunc("F_GetNumSuffix",.@level)+" Level have been defeated.",bc_map,"0xffff00";
				donpcevent instance_npcname(.@level+"FMGate102tower",instance_id())+"::OnEnable";
				callfunc "F_Mini_Tower_Warp",.@floor,strnpcinfo(4);
			}else{
				mes"^C70847max is 25^000000";
				close;
			}
		}
	}
	end;
OnInstanceInit:
	initnpctimer;
	end;

OnTimer10000:
	mapannounce instance_mapname("1@Mtower",instance_id()), "Notice : Taming a monster does not count towards defeating them.",bc_map,"0xff0000";
	stopnpctimer;
	end;
}

1@Mtower,29,365,1	script	#1F MController	844,{
	end;
OnInstanceInit:
	callfunc "F_Mini_Tower_Monster",
		1,
		instance_mapname("1@Mtower",instance_id()),
		instance_npcname("#1F MController",instance_id())+"::OnMyMobDead";
	end;

OnMyMobDead:
	set .@map$, instance_mapname("1@Mtower",instance_id());
	set .@mob_dead_num,mobcount(.@map$,instance_npcname("#1F MController",instance_id())+"::OnMyMobDead");
	if (.@mob_dead_num < 1) {
		initnpctimer;
	} else
		mapannounce .@map$, "Remaining Monsters on the 1st Level - "+.@mob_dead_num,bc_map,"0x00ff99";
	end;

OnTimer5000:
	mapannounce instance_mapname("1@Mtower",instance_id()), "All Monsters on the 1st Level have been defeated.",bc_map,"0xffff00";
	donpcevent instance_npcname("1FMGate102tower",instance_id())+"::OnEnable";
	stopnpctimer;
	end;
}

1@Mtower,12,393,0	script	1FMGate102tower	45,2,2,{
	end;
OnInstanceInit:
	disablenpc instance_npcname(strnpcinfo(0),instance_id());
	end;
OnEnable:
	enablenpc instance_npcname(strnpcinfo(0),instance_id());
	callfunc "F_Mini_Tower_Monster",
		atoi(replacestr(strnpcinfo(0),"FMGate102tower","")) + 1,
		strnpcinfo(4),
		instance_npcname(strnpcinfo(0),instance_id())+"::OnMyMobDead";
	end;
OnTouch_:
	callfunc "F_Mini_Tower_Warp",
		atoi(replacestr(strnpcinfo(0),"FMGate102tower","")) + 1,
		strnpcinfo(4);
	end;
OnMyMobDead:
	set .@map$, strnpcinfo(4);
	set .@level, atoi(replacestr(strnpcinfo(0),"FMGate102tower","")) + 1;
	set .@mob_dead_num,mobcount(.@map$,instance_npcname(strnpcinfo(0),instance_id())+"::OnMyMobDead");
	if (.@mob_dead_num < 1) {
		initnpctimer;
	} else
		mapannounce .@map$, "Remaining Monsters on the "+callfunc("F_GetNumSuffix",.@level)+" Level - "+.@mob_dead_num,bc_map,"0x00ff99";
	end;
OnTimer5000:
	set .@level, atoi(replacestr(strnpcinfo(0),"FMGate102tower","")) + 1;
	mapannounce strnpcinfo(4), "All Monsters on the "+callfunc("F_GetNumSuffix",.@level)+" Level have been defeated.",bc_map,"0xffff00";
	if(.@level != 25){
		donpcevent instance_npcname(.@level+"FMGate102tower",instance_id())+"::OnEnable";
	}else{
		enablenpc instance_npcname("Warper#25FMGate102tower",instance_id());
		mapannounce strnpcinfo(4), "All Monsters on all the Levels have been defeated.",bc_map,"0xffff00";
		sleep 3000;
		mapannounce strnpcinfo(4), "Until the Next Time!.",bc_map,"0xffff00";
	}
	stopnpctimer;
	end;
}
1@Mtower,96,393,0	duplicate(1FMGate102tower)	2FMGate102tower	45,2,2
1@Mtower,184,393,0	duplicate(1FMGate102tower)	3FMGate102tower	45,2,2
1@Mtower,270,393,0	duplicate(1FMGate102tower)	4FMGate102tower	45,2,2
1@Mtower,355,393,0	duplicate(1FMGate102tower)	5FMGate102tower	45,2,2
1@Mtower,12,309,0	duplicate(1FMGate102tower)	6FMGate102tower	45,2,2
1@Mtower,96,309,0	duplicate(1FMGate102tower)	7FMGate102tower	45,2,2
1@Mtower,184,309,0	duplicate(1FMGate102tower)	8FMGate102tower	45,2,2
1@Mtower,270,309,0	duplicate(1FMGate102tower)	9FMGate102tower	45,2,2
1@Mtower,355,309,0	duplicate(1FMGate102tower)	10FMGate102tower	45,2,2
1@Mtower,12,222,0	duplicate(1FMGate102tower)	11FMGate102tower	45,2,2
1@Mtower,96,222,0	duplicate(1FMGate102tower)	12FMGate102tower	45,2,2
1@Mtower,184,222,0	duplicate(1FMGate102tower)	13FMGate102tower	45,2,2
1@Mtower,270,222,0	duplicate(1FMGate102tower)	14FMGate102tower	45,2,2
1@Mtower,355,222,0	duplicate(1FMGate102tower)	15FMGate102tower	45,2,2
1@Mtower,12,138,0	duplicate(1FMGate102tower)	16FMGate102tower	45,2,2
1@Mtower,96,138,0	duplicate(1FMGate102tower)	17FMGate102tower	45,2,2
1@Mtower,184,138,0	duplicate(1FMGate102tower)	18FMGate102tower	45,2,2
1@Mtower,270,138,0	duplicate(1FMGate102tower)	19FMGate102tower	45,2,2
1@Mtower,355,138,0	duplicate(1FMGate102tower)	20FMGate102tower	45,2,2
1@Mtower,12,51,0	duplicate(1FMGate102tower)	21FMGate102tower	45,2,2
1@Mtower,96,51,0	duplicate(1FMGate102tower)	22FMGate102tower	45,2,2
1@Mtower,184,51,0	duplicate(1FMGate102tower)	23FMGate102tower	45,2,2
1@Mtower,270,51,0	duplicate(1FMGate102tower)	24FMGate102tower	45,2,2


1@Mtower,353,48,4	script	Warper#25FMGate102tower	874,{
	if(getpartyleader(getcharid(1),2) == getcharid(0)){
		mes"Are you sure you want to get out from here ?";
		if(select("No:Yes") == 1)
			close;
	}
	callfunc "F_Mini_Endless_Tower_Rewards";
	completequest 150000;
	warp "SavePoint",0,0;
	end;
	
OnInstanceInit:
	disablenpc instance_npcname(strnpcinfo(0),instance_id());
	end;
}

1@Mtower,71,4,0	script	#MBroadcast Mode1	844,{
	end;

OnInstanceInit:
	initnpctimer;
	end;

OnTimer15000:
OnTimer60000:
	mapannounce instance_mapname("1@Mtower",instance_id()), "Notice : In any abnormal situation where you defeat a monster, you can't advance to the next level!",bc_map,"0xff0000";
	end;
OnTimer120000:
	mapannounce instance_mapname("1@Mtower",instance_id()), "Notice : In any abnormal situation where you defeat a monster, you can't advance to the next level!",bc_map,"0xff0000";
	stopnpctimer;
	end;
}

function	script	F_Mini_Tower_Warp	{

	set .@level, getarg(0);
	set .@map$, getarg(1);

	switch(.@level) {
		case 2: warp .@map$,136,354; break;
		case 3: warp .@map$,224,354; break;
		case 4: warp .@map$,310,354; break;
		case 5: warp .@map$,395,354; break;
		case 6: warp .@map$,52,270; break;
		case 7: warp .@map$,136,270; break;
		case 8: warp .@map$,224,270; break;
		case 9: warp .@map$,310,270; break;
		case 10: warp .@map$,395,270; break;
		case 11: warp .@map$,52,183; break;
		case 12: warp .@map$,136,183; break;
		case 13: warp .@map$,224,183; break;
		case 14: warp .@map$,310,183; break;
		case 15: warp .@map$,395,183; break;
		case 16: warp .@map$,52,99; break;
		case 17: warp .@map$,136,99; break;
		case 18: warp .@map$,224,99; break;
		case 19: warp .@map$,310,99; break;
		case 20: warp .@map$,395,99; break;
		case 21: warp .@map$,52,12; break;
		case 22: warp .@map$,136,12; break;
		case 23: warp .@map$,224,12; break;
		case 24: warp .@map$,310,12; break;
		case 25: warp .@map$,395,12; break;
	}
	return;
}

function	script	F_Mini_Tower_Monster	{

	set .@level, getarg(0);
	set .@map$, getarg(1);
	set .@label$, getarg(2);

	switch(.@level) {
	case 1:
		areamonster .@map$,7,351,17,387,"Moonlight Flower",1150,1,.@label$;
		areamonster .@map$,7,351,17,387,"Golden Thief Bug",1086,1,.@label$;
		break;
	case 2:
		areamonster .@map$,93,351,103,387,"Eddga",1115,1,.@label$;
		areamonster .@map$,93,351,103,387,"Maya",1147,1,.@label$;
		break;
	case 3:
		areamonster .@map$,181,351,191,387,"Phreeoni",1159,1,.@label$;
		areamonster .@map$,181,351,191,387,"Tao Gunka",1583,1,.@label$;
		break;
	case 4:
		areamonster .@map$,267,351,277,387,"Garm",1252,1,.@label$;
		areamonster .@map$,267,351,277,387,"Mistress",1059,1,.@label$;
		break;
	case 5:
		areamonster .@map$,352,351,362,387,"Ygnizem",1658,1,.@label$;
		areamonster .@map$,352,351,362,387,"Incantation Samurai",1492,1,.@label$;
		break;
	case 6:
		areamonster .@map$,9,267,19,303,"Stormy Knight",1251,1,.@label$;
		areamonster .@map$,9,267,19,303,"Doppelganger",1046,1,.@label$;
		break;
	case 7:
		areamonster .@map$,93,267,103,303,"White Lady",1630,1,.@label$;
		areamonster .@map$,93,267,103,303,"Evil Snake Lord",1418,1,.@label$;
		break;
	case 8:
		areamonster .@map$,181,267,191,303,"Gopinich",1885,1,.@label$;
		areamonster .@map$,181,267,191,303,"Dracula",1389,1,.@label$;
		break;
	case 9:
		areamonster .@map$,267,267,277,303,"Turtle General",1312,1,.@label$;
		areamonster .@map$,267,267,277,303,"Drake",1112,1,.@label$;
		break;
	case 10:
		areamonster .@map$,352,267,362,303,"Orc Hero",1087,1,.@label$;
		areamonster .@map$,352,267,362,303,"Orc Lord",1190,1,.@label$;
		break;
	case 11:
		areamonster .@map$,9,180,19,216,"Osiris",1038,1,.@label$;
		areamonster .@map$,9,180,19,216,"Thanatos",1708,1,.@label$;
		break;
	case 12:
		areamonster .@map$,93,180,103,216,"Pharaoh",1157,1,.@label$;
		areamonster .@map$,93,180,103,216,"Lady Tanee",1688,1,.@label$;
		break;
	case 13:
		areamonster .@map$,181,180,191,216,"RSX 0806",1623,1,.@label$;
		areamonster .@map$,181,180,191,216,"Lord of Death",1373,1,.@label$;
		break;
	case 14:
		areamonster .@map$,267,180,277,216,"Lost Dragon",2131,1,.@label$;
		areamonster .@map$,267,180,277,216,"Vesper",1685,1,.@label$;
		break;
	case 15:
		areamonster .@map$,352,180,362,216,"Baphomet",1039,1,.@label$;
		areamonster .@map$,352,180,362,216,"Dark Lord",1272,1,.@label$;
		break;
	case 16:
		areamonster .@map$,9,96,19,132,"Detardeurus",1719,1,.@label$;
		areamonster .@map$,9,96,19,132,"Atroce",1785,1,.@label$;
		break;
	case 17:
		areamonster .@map$,93,96,103,132,"Amon Ra",1511,1,.@label$;
		areamonster .@map$,93,96,103,132,"Leak",2156,1,.@label$;
		break;
	case 18:
		areamonster .@map$,181,96,191,132,"Boitata",2068,1,.@label$;
		areamonster .@map$,181,96,191,132,"Kiel D-01",1734,1,.@label$;
		break;
	case 19:
		areamonster .@map$,267,96,277,132,"Gloom Under Night",1768,1,.@label$;
		areamonster .@map$,267,96,277,132,"Queen Scaraba",2087,1,.@label$;
		break;
	case 20:
		areamonster .@map$,352,96,362,132,"Nightmare Amon Ra",2362,1,.@label$;
		areamonster .@map$,352,96,362,132,"Fallen Bishop",1871,1,.@label$;
		break;
	case 21:
		areamonster .@map$,9,9,19,45,"Nidhoggr's Shadow",2022,1,.@label$;
		areamonster .@map$,9,9,19,45,"Valkyrie Randgris",1751,1,.@label$;
		break;
	case 22:
		areamonster .@map$,93,9,103,45,"Ktullanux",1779,1,.@label$;
		areamonster .@map$,93,9,103,45,"Kraken",2202,1,.@label$;

		break;
	case 23:
		areamonster .@map$,181,9,191,45,"Gold Queen Scaraba",2165,1,.@label$;
		areamonster .@map$,181,9,191,45,"Ifrit",1832,1,.@label$;
		break;
	case 24:
		areamonster .@map$,267,9,277,45,"Wounded Morroc",1917,1,.@label$;
		areamonster .@map$,267,9,277,45,"Great Demon Baphomet",1929,1,.@label$;
		break;
	case 25:
		areamonster .@map$,352,9,362,45,"Naght Sieger",1956,1,.@label$;
		areamonster .@map$,352,9,362,45,"Beelzebub",1873,1,.@label$;
		break;
	}
	return;
}


1@Mtower	mapflag	noreturn
1@Mtower	mapflag	restricted	6
1@Mtower	mapflag	partylock
1@Mtower	mapflag	nobranch
1@Mtower	mapflag	noteleport
1@Mtower	mapflag	nowarpto
1@Mtower	mapflag	pvp	off
1@Mtower	mapflag	nosave	SavePoint
1@Mtower	mapflag	nosave	SavePoint
1@Mtower	mapflag	nowarp
1@Mtower	mapflag	nomemo
1@Mtower	mapflag	nochat
1@Mtower	mapflag	novending
1@Mtower	mapflag	monster_noteleport
