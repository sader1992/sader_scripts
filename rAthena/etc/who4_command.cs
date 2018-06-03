//===== rAthena Script =======================================
//= saders who4 command
//===== By: ==================================================
//= Sader1992
//= Free!!
//https://rathena.org/board/profile/30766-sader1992/
//===== Current Version: =====================================
//= 1.0
//===== Compatible With: ===================================== 
//= rAthena Project
//https://github.com/sader1992/sader_scripts
//===== Description: =========================================
//==== allow you to see how many players are connected via PCs (This only work with GePard)
//============================================================
//==== please report any error you find
//============================================================
//============================================================
-	script	who4	-1,{
OnCommand:	
	freeloop(1);
	query_sql("SELECT `account_id`,`name` FROM `char` WHERE `online` = '1'",.@account_id,.@char_name$);
	for(.@i=0;.@i<getarraysize(.@account_id);.@i++){
		query_sql("SELECT `last_unique_id` FROM `login` WHERE `account_id` = '" +.@account_id[.@i]+"'",.@last_unique_id$);
		.@exx = false;
		for(.@s=0;.@s<getarraysize(.@id_list$);.@s++){
			if(.@id_list$[.@s] == .@last_unique_id$){
				.@exx = true;
			}
		}
		if(!.@exx){
			.@id_list$[getarraysize(.@id_list$)] = .@last_unique_id$;
			.@charName$[getarraysize(.@charName$)] = .@char_name$[.@i];
		}
	}
	freeloop(0);
	for(.@i=0;.@i<getarraysize(.@id_list$);.@i++){
		message strcharinfo(0),.@id_list$[.@i] + " ( " + .@charName$[.@i] + " )";
	}
	message strcharinfo(0),getarraysize(.@id_list$) + " Players Found.";
end;
OnInit:
	bindatcmd("who4",strnpcinfo(3)+"::OnCommand",99,99);
end;
}