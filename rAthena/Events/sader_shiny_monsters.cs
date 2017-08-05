//===== rAthena Script =======================================
//= saders Shiny Monsters
//===== By: ==================================================
//= Sader1992
//https://rathena.org/board/profile/30766-sader1992/
//===== Current Version: =====================================
//= 0.0
//===== Compatible With: ===================================== 
//= rAthena Project
//https://rathena.org/board/  not submited yet
//===== Description: =========================================
//=
//============================================================
-	script	s_shiny_monster	-1,{

	
OnNPCKillEvent:
for (.@i = 0; .@i < getarraysize(.s_shiny); .@i ++){
	if( killedrid == .s_shiny[.@i] ){
		if(monster_shiny_count[.@i] < .s_shiny_amount[.@i]){
			monster_shiny_count[.@i]++;
			dispbottom "You killed "+ monster_shiny_count[.@i]+" "+ strmobinfo( 1,.s_shiny[.@i]) +".";
			if(monster_shiny_count[.@i] >= .s_shiny_amount[.@i]){
				announce "the shiny monster " + strmobinfo( 1,.s_shiny[.@i]) + " spowned" ,bc_self,0xFFFFFF, 0x190, 15;
				set monster_shiny_count[.@i],0;
		}
		}
	}
}

	
	

OnInit:
	
	setarray .s_shiny,1275,1737,1735,1736;
	setarray .s_shiny_amount,100,200,300,400;
end;
}