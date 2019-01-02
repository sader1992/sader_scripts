//===== rAthena Script =======================================
//= getgpmapunits command
//= getipmapunits command
//===== By: ==================================================
//= Sader1992
//= Free!!
// https://rathena.org/board/profile/30766-sader1992/
//===== Current Version: =====================================
//= 1.0
//===== Compatible With: ===================================== 
//= rAthena Project
// https://github.com/sader1992/sader_scripts
//===== Description: =========================================
// getgpmapunits("map"); return the number of players with the same GePard id in the given map.
// getipmapunits("map"); return the number of players with the same IP in the given map.
//============================================================
//==== please report any error you find
//============================================================
//============================================================

//==============================================
// GePard
// Add it to your rAthena/npc/other/Global_Functions.txt
// Only if you have GePard!
//==========================================>>>>
-	script	GePard_Registry	0,{
OnPCLoginEvent:
	unique_id$ = get_unique_id();
end;
OnPCLogoutEvent:
	unique_id$ = "";
end;
}

function	script	getgpmapunits	{
	.@map_name$ = getarg(0);
	.@unique_id$ = unique_id$;
	getmapunits(BL_PC,.@map_name$,.@AccountID);
	for(.@i=0;.@i<getarraysize(.@AccountID);.@i++)
		if(getvar(unique_id$,getcharid(0,rid2name(.@AccountID[.@i]))) == .@unique_id$)
			.@count++;
	return .@count;
}
//==========================================<<<<


//==============================================
// IP
// Add it to your rAthena/npc/other/Global_Functions.txt
//==========================================>>>>
function	script	getipmapunits	{
	.@map_name$ = getarg(0);
	.@ip$ = getcharip();
	getmapunits(BL_PC,.@map_name$,.@name$);
	
	for(.@i=0;.@i<getarraysize(.@name$);.@i++)
		if(getcharip(.@name$[.@i]) == .@ip$)
			.@count++;
	return .@count;
}
//==========================================<<<<

/*
From Here is only for test
*/

//==============================================
//NPC EXAMPLE FOR IP FUNCTION
//==============================================
prontera,152,183,5	script	Warper IP Test	446,{
	if(getipmapunits("prontera") > 0){
		mes "you already have a char inside this map";
	}else{
		warp "prontera",152,183;
	}
end;
}
//==============================================


//==============================================
//NPC EXAMPLE FOR GEPARD FUNCTION
//==============================================
prontera,155,183,5	script	Warper GePard Test	446,{
	if(getipmapunits("prontera") > 0){
		mes "you already have a char inside this map";
	}else{
		warp "prontera",155,183;
	}
end;
}
//==============================================
