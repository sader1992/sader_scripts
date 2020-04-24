//===== rAthena Script =======================================
//= saders enchant npc
//===== By: ==================================================
//= Sader1992
//https://rathena.org/board/profile/30766-sader1992/
//===== Current Version: =====================================
//= 2.0
//===== Compatible With: ===================================== 
//= rAthena Project
//https://rathena.org/board/files/file/3602-saders-enchantment-npc/
//https://github.com/sader1992/sader_scripts
//===== Description: =========================================
//============================================================
//============================================================
prontera,157,176,6	script	sader enchant	998,{
	callsub s_check_menu;
end;

s_check_menu:
		if(.s_only_vip == 1 && vip_status(1)){
			mes "this service only for vip";
			close;
		}
		if (BaseLevel < .s_level_required[0] || BaseLevel > .s_level_required[1]){
			mes "Your level is too low or too high.";
			mes "   ";
			mes "Minimum level "+.s_level_required[0]+".";
			mes "Maximum level "+.s_level_required[1]+".";
			close;
		}
	callsub s_start_menu;
end;

s_start_menu:
	mes "Hello!";
	mes "Do you want to enchant you items!";
	mes "I am the best enchanter in the world!";
	next;
	if (.s_zeny  > 0) {
			mes "this will cost you " + .s_zeny + " Zeny only!";
		}
	if (.item_is_required == 1) {
			mes "and an enchantment orb";
		}
	mes "i will do my best to enchant it Successfully!";
	mes "but remember";
	mes "There is luck in this work too.";
	next;
	mes "please if you have items same";
	mes "as the item you want to enchant";
	mes "but them in the storage and come back to me!";
	next;
	set s_all_selected,0;
	set s_orb_selected,0;
	set selected_orb_id ,0;
	set s_slot_selected,0;
	set remove_orbs,0;
	set selected_orb_size,0;
	set s_item_refine,0;
	if(.remove_enchant == 1){
		callsub s_select_menu;
	}else{
		callsub s_select_armor;
		}
close;
end;

s_select_menu:
	mes "so what you want to do!";
	if(.enable_the_shop == 1){
		switch(select("Enchant:Remove Enchant:The Items you can enchant")){
			case 2: remove_orbs = 1; 
			case 3: callsub Q_shop;
		}
	}else{
		switch(select("Enchant:Remove Enchant")){
			case 2: remove_orbs = 1; 
		}
	}
	
	callsub s_select_armor;
end;
	
s_select_armor:
	next;
	mes "please select the item you want to enchant";
	for(.@i=0; .@i<getarraysize(.s_all$); .@i++)
		if(getequipid(.s_all_loc[.@i])>-1) {
			set .@armor_menu$, .@armor_menu$ + .s_all$[.@i] + " - [ ^E81B02" + getitemname(getequipid(.s_all_loc[.@i])) + "^000000 ]:";
		}else{
			set .@armor_menu$, .@armor_menu$ + .s_all$[.@i] + " - [ ^D6C4E8" + "No Equip" + "^000000 ]:";
		}
	set s_all_selected, select(.@armor_menu$) -1;
	if(getequipid(.s_all_loc[s_all_selected])< 0) {
		mes "you don't have item equiped there";
		close;
	}
	if (countitem(getequipid(.s_all_loc[s_all_selected])) > 1){
			mes "you have more then one item";
			mes "from the item that you want to enchant";
			close;
	}
	s_item_refine = getequiprefinerycnt(.s_all_loc[s_all_selected]);
	callsub s_specific_gears;
end;

s_specific_gears:
	if( getd(".specific_" + .s_all$[s_all_selected] + "s") ==1){
		for(.@i=0;.@i<getarraysize(getd("." + .s_all$[s_all_selected] + "s$"));.@i++){
			if(getequipid(.s_all_loc[s_all_selected]) == atoi(getd("." + .s_all$[s_all_selected] + "s$["+.@i+"]"))){
				.@good_to_go = 1;
			}
		}
	}else{
		for(.@i=0;.@i<getarraysize(.black_list$);.@i++){
			if(getequipid(.s_all_loc[s_all_selected]) == atoi(.black_list$[.@i])){
				.@black_list_item = 1;
			}
		}
		.@good_to_go = 1;
	}
	if(!.@good_to_go || .@black_list_item == 1){
		mes "sorry";
		mes " i can't enchant this item.";
		close;
	}
	if(remove_orbs == 1){callsub s_select_remove;}
	if(.chosse_orb == 1){callsub s_select_orb;}else{callsub s_random_orb;}
end;

Q_shop:
	switch(select("Weapons:Armors:Shields:Germents:Shoses:Accessarys:Uppers:Middels:Lowers")){
	case 1: callshop "enchantable_items_Weapon",1; break;
	case 2: callshop "enchantable_items_Armor",1; break;
	case 3: callshop "enchantable_items_Shield",1; break;
	case 4: callshop "enchantable_items_Germent",1; break;
	case 5: callshop "enchantable_items_Shose",1; break;
	case 6: callshop "enchantable_items_Accessary",1; break;
	case 7: callshop "enchantable_items_Upper",1; break;
	case 8: callshop "enchantable_items_Middel",1; break;
	case 9: callshop "enchantable_items_Lower",1; break;
	}
end;

s_select_orb:
	next;
	mes "select the orb you want";
	for(.@i=0; .@i<getarraysize(getd("." + .s_all$[s_all_selected] + "$")); .@i++)
			set .@orb_menu$, .@orb_menu$ + getitemname(atoi(getd("." + .s_all$[s_all_selected] + "$["+.@i+"]"))) + ":";
	set s_orb_selected, select(.@orb_menu$) -1;
	selected_orb_id = getd("." + .s_all$[s_all_selected] + "$["+s_orb_selected+"]");
		callsub s_select_slot;
end;


s_select_slot:
	next;
	mes "which slot ?";
		.@card0 = getequipcardid(.s_all_loc[s_all_selected],0);
		.@card1 = getequipcardid(.s_all_loc[s_all_selected],1);
		.@card2 = getequipcardid(.s_all_loc[s_all_selected],2);
		.@card3 = getequipcardid(.s_all_loc[s_all_selected],3);
	for(.@i=getd(".slot_count_" + .s_all$[s_all_selected]); .@i<4; .@i++)
		if(getequipcardid(.s_all_loc[s_all_selected],.@i)!= null) {
			set .@slot_menu$, .@slot_menu$ + " [ ^E81B02" + getitemname(getequipcardid(.s_all_loc[s_all_selected],.@i)) + "^000000 ]:";
		}else{
			set .@slot_menu$, .@slot_menu$ + " [ ^D6C4E8" + "Empty" + "^000000 ]:";
		}
	set s_slot_selected, select(.@slot_menu$) -1;
	if(.s_enchant_overwrite == 0){
		if(getequipcardid(.s_all_loc[s_all_selected],s_slot_selected) > 0){
			mes "you already have orb in this slot";
			close;
		}
	}
	callsub s_enchant_progress;
end;

s_enchant_progress:
		.@card0 = getequipcardid(.s_all_loc[s_all_selected],0);
		.@card1 = getequipcardid(.s_all_loc[s_all_selected],1);
		.@card2 = getequipcardid(.s_all_loc[s_all_selected],2);
		.@card3 = getequipcardid(.s_all_loc[s_all_selected],3);
	if (Zeny < .s_zeny) {
			mes "Sorry, but you don't have enough zeny.";
			close;
		}
	if(.item_is_required == 1){
		if (countitem(selected_orb_id) < 1){
			mes"you don't have enchant orb";
			close;
		}
	}
	close2;
	specialeffect2 EF_MAPPILLAR;
	progressbar "ffff00",.progress_time;
	set Zeny, Zeny-.s_zeny;
	if(.item_is_required == 1){delitem selected_orb_id,1;}
	if (rand(100) < .success_chanse[s_slot_selected]) callsub s_enchant_success;
	else callsub s_brack;
end;

s_brack:
	specialeffect2 155;
	mes "I am sorry";
	mes "We did Fail";
	specialeffect2 EF_PHARMACY_FAIL;
	if (rand(100) < .brack_chance){
		set .@item, getequipid(.s_all_loc[s_all_selected]);
		delitem .@item,1;
		mes "and it broke!!";
		specialeffect EF_SUI_EXPLOSION;
	}
	close;
end;

s_enchant_success:
		.@card0 = getequipcardid(.s_all_loc[s_all_selected],0);
		.@card1 = getequipcardid(.s_all_loc[s_all_selected],1);
		.@card2 = getequipcardid(.s_all_loc[s_all_selected],2);
		.@card3 = getequipcardid(.s_all_loc[s_all_selected],3);
	mes "We did it!";
		specialeffect2 154;
		setd(".@card" + s_slot_selected, selected_orb_id);
		set .@item, getequipid(.s_all_loc[s_all_selected]);
		delitem .@item,1;
		getitem2 .@item, 1, 1, s_item_refine, 0, .@card0, .@card1, .@card2, .@card3;
		equip .@item;
	close;
end;	


s_select_remove:
		.@card0 = getequipcardid(.s_all_loc[s_all_selected],0);
		.@card1 = getequipcardid(.s_all_loc[s_all_selected],1);
		.@card2 = getequipcardid(.s_all_loc[s_all_selected],2);
		.@card3 = getequipcardid(.s_all_loc[s_all_selected],3);
		next;
		mes "this will remove all the cards and orbs inside the item!";
		if (.s_zeny_remove > 0) {
			mes "this will cost you " + .s_zeny_remove + " Zeny.";
		}
		mes "are you sure?";
			switch(select("NO:Yes")){
				case 2:
					mes "for the last time!";
					mes "are you sure?";
					switch(select("NO:Yes")){
						if (Zeny < .s_zeny_remove) {
							mes "Sorry, but you don't have enough zeny.";
							close;
						}
					case 2: 
						specialeffect2 EF_REPAIRWEAPON;
						set .@item, getequipid(.s_all_loc[s_all_selected]);
						delitem .@item,1;
						getitem2 .@item, 1, 1, s_item_refine, 0, 0, 0, 0, 0;
						set Zeny, Zeny-.s_zeny_remove;
					}
			}	
	close;
end;

s_random_orb:
selected_orb_size = rand(getarraysize(getd("." + .s_all$[s_all_selected] + "$")));
selected_orb_id = getd("." + .s_all$[s_all_selected] + "$["+selected_orb_size+"]");
	callsub s_select_slot;
end;


OnInit:
	//--------------------------------------------------------------//
	//--------------------------------------------------------------//
	//--------------------   configuration   -----------------------//
	//--------------------------------------------------------------//
	//--------------------------------------------------------------//
	
	//--------------------------------------------------------------//
	//if you want to remove one from the menu you need to remove it down too!! /or add
	//--------------------------------------------------------------//
	setarray .s_all$,"Weapon","Armor","Shield","Germent","Shose","Accessary","Upper","Middel","Lower";
	setarray .s_all_loc,EQI_HAND_R,EQI_ARMOR,EQI_HAND_L,EQI_GARMENT,EQI_SHOES,EQI_ACC_L,EQI_HEAD_TOP,EQI_HEAD_MID,EQI_HEAD_LOW;
	
	//--------------------------------------------------------------//
	//Orbs IDs (Note : Shield = left hand so the weapon on the left hand count as Shield too!
	//--------------------------------------------------------------//
	setarray .Weapon$,4741,4933,4861,4762,4934;	//right handed weapons
	setarray .Armor$,4933,4861,4762,4934;	//Armors
	setarray .Shield$,4861,4762,4934;	//Shields and left hand weapons
	setarray .Germent$,4741,4933,4861,4762,4934;	//Germent
	setarray .Shose$,4741,4933,4861,4762,4934;	//Shose
	setarray .Accessary$,4741,4933,4861,4762,4934;	//orbs id
	setarray .Upper$,4741,4933,4861,4762,4934;	//Accessary
	setarray .Middel$,4741,4933,4861,4762,4934;	//Middel
	setarray .Lower$,4741,4933,4861,4762,4934;	//Lower
	
	//--------------------------------------------------------------//
	//if you want to put specific IDs for kind of gear put it to 1
	//--------------------------------------------------------------//
	.specific_Weapons = 1;
	.specific_Armors = 1;
	.specific_Shields = 1;
	.specific_Germents = 1;
	.specific_Shoses = 1;
	.specific_Accessarys = 1;
	.specific_Uppers = 1;
	.specific_Middels =1;
	.specific_Lowers = 1;
	
	//--------------------------------------------------------------//
	//if specific put the IDs here
	//--------------------------------------------------------------//
	setarray .Weapons$,1601,1201,1204,1207,1210,1213,1216,1219,1222,1247,1248,1249;	//right handed weapons
	setarray .Armors$,2301,2303,2305,2307,2307,2309,2312,2314,2316,2321,2323,2325,2328,2330,2332;	//Armors
	setarray .Shields$,2101,2103,2105,2107,2113,2117;	//Shields and left hand weapons
	setarray .Germents$,2512,2501,2503,2505;	//Germents
	setarray .Shoses$,2416,2401,2403,2405,2411;	//Shoses
	setarray .Accessarys$,2628,2608,2609,2612,2613,2627;	//Accessarys
	setarray .Uppers$,2206,2208,2211,2216;	//Uppers
	setarray .Middels$,2218,2241;	//Middels
	setarray .Lowers$,2628,2206;	//Lowers
	
	//--------------------------------------------------------------//
	//if not specific put the black list IDs here (if you want
	//--------------------------------------------------------------//
	setarray .black_list$,2335,2338,2340,2341;
	
	//--------------------------------------------------------------//
	//here you can make a specific slot number for each kind
	//0 = all 4 slot ,1 = last 3 slot ,2 = last 2 slot ,3 = last 1 slot
	//--------------------------------------------------------------//
	.slot_count_Weapon = 0;
	.slot_count_Armor = 0;
	.slot_count_Shield = 0;
	.slot_count_Germent = 0;
	.slot_count_Shose = 0;
	.slot_count_Accessary = 0;
	.slot_count_Upper = 0;
	.slot_count_Middel = 0;
	.slot_count_Lower = 0;
	
	//--------------------------------------------------------------//
	//other configuration
	//--------------------------------------------------------------//
	setarray .s_level_required,0,175;	//the level required to use the npc
	.s_only_vip = 0;	//if you want only vip to use it put it to 1
	setarray .success_chanse,100,80,60,40;	//success chanse [1st_slot_chanse,2nd_slot_chanse,3rd_slot_chanse,4th_slot_chanse]
	.s_zeny = 10000;	//if you don't want zeny requirment set it to 0
	.s_zeny_remove = 100000;	//this for enchantment reset
	.item_is_required = 0;	//if you want the orb it self to be required 1 = yes , 0 = no
	.s_enchant_overwrite = 0;	//if 1 then you can overwrite the enchant
	.progress_time = 7;	//the time that needed to wait until the socket end
	.chosse_orb = 0;	//0 = random ,1 = yes
	.brack_chance = 50;	//the chanse that it will brack if it fail
	.remove_enchant = 1;	//0 = no ,1 = yes
	
	//--------------------------------------------------------------//
	//this will only show the items that the npc can enchant in a shop but no one can buy from it as long as you don't give them the value
	//--------------------------------------------------------------//
	.enable_the_shop = 1;


	
	
	
	//--------------------------------------------------------------//
	//Do not edit here
	//--------------------------------------------------------------//
	npcshopdelitem "enchantable_items_Weapon",512;
	npcshopdelitem "enchantable_items_Armor",512;
	npcshopdelitem "enchantable_items_Shield",512;
	npcshopdelitem "enchantable_items_Germent",512;
	npcshopdelitem "enchantable_items_Shose",512;
	npcshopdelitem "enchantable_items_Accessary",512;
	npcshopdelitem "enchantable_items_Upper",512;
	npcshopdelitem "enchantable_items_Middel",512;
	npcshopdelitem "enchantable_items_Lower",512;
	
	for (.@i = 0; .@i < getarraysize(.Weapons$); .@i++)
			npcshopadditem "enchantable_items_Weapon", atoi(.Weapons$[.@i]),1;
	for (.@i = 0; .@i < getarraysize(.Armors$); .@i++)
			npcshopadditem "enchantable_items_Armor", atoi(.Armors$[.@i]),1;
	for (.@i = 0; .@i < getarraysize(.Shields$); .@i++)
			npcshopadditem "enchantable_items_Shield", atoi(.Shields$[.@i]),1;
	for (.@i = 0; .@i < getarraysize(.Germents$); .@i++)
			npcshopadditem "enchantable_items_Germent", atoi(.Germents$[.@i]),1;	
	for (.@i = 0; .@i < getarraysize(.Shoses$); .@i++)
			npcshopadditem "enchantable_items_Shose", atoi(.Shoses$[.@i]),1;	
	for (.@i = 0; .@i < getarraysize(.Accessarys$); .@i++)
			npcshopadditem "enchantable_items_Accessary", atoi(.Accessarys$[.@i]),1;	
	for (.@i = 0; .@i < getarraysize(.Uppers$); .@i++)
			npcshopadditem "enchantable_items_Upper", atoi(.Uppers$[.@i]),1;		
	for (.@i = 0; .@i < getarraysize(.Middels$); .@i++)
			npcshopadditem "enchantable_items_Middel", atoi(.Middels$[.@i]),1;	
	for (.@i = 0; .@i < getarraysize(.Lowers$); .@i++)
			npcshopadditem "enchantable_items_Lower", atoi(.Lowers$[.@i]),1;	
	end;
}
-	pointshop	enchantable_items_Weapon	-1,#YOU_CAN_ENCHANT_Weapons,512:1;
-	pointshop	enchantable_items_Armor	-1,#YOU_CAN_ENCHANT_Armors,512:1;
-	pointshop	enchantable_items_Shield	-1,#YOU_CAN_ENCHANT_Shields,512:1;
-	pointshop	enchantable_items_Germent	-1,#YOU_CAN_ENCHANT_Germents,512:1;
-	pointshop	enchantable_items_Shose	-1,#YOU_CAN_ENCHANT_Shoses,512:1;
-	pointshop	enchantable_items_Accessary	-1,#YOU_CAN_ENCHANT_Accessarys,512:1;
-	pointshop	enchantable_items_Upper	-1,#YOU_CAN_ENCHANT_Uppers,512:1;
-	pointshop	enchantable_items_Middel	-1,#YOU_CAN_ENCHANT_Middels,512:1;
-	pointshop	enchantable_items_Lower	-1,#YOU_CAN_ENCHANT_Lowers,512:1;



