//===== rAthena Script =======================================
//= saders enchant npc
//===== By: ==================================================
//= Sader1992
//https://rathena.org/board/profile/30766-sader1992/
//===== Current Version: =====================================
//= 1.0
//===== Compatible With: ===================================== 
//= rAthena Project
//https://rathena.org/board/files/file/
//https://github.com/sader1992/sader_scripts
//===== Description: =========================================
//============================================================
prontera,157,176,6	script	sader enchant	998,{
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
		callsub s_select_armor;}
close;

s_select_menu:
	mes "so what you want to do!";
	switch(select("Enchant:Remove Enchant")){
	case 2: remove_orbs = 1; 
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
	if(remove_orbs == 1){callsub s_select_remove;}
	if(.chosse_orb == 1){callsub s_select_orb;}else{callsub s_random_orb;}
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
	for(.@i=.slot_count; .@i<4; .@i++)
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
	//if you want to remove one from the menu you need to remove it down too!! /or add
	setarray .s_all$,"Weapon","Armor","Shield","Germent","Shose","Accessary","Upper","Middel","Lower";
	setarray .s_all_loc,EQI_HAND_R,EQI_ARMOR,EQI_HAND_L,EQI_GARMENT,EQI_SHOES,EQI_ACC_L,EQI_HEAD_TOP,EQI_HEAD_MID,EQI_HEAD_LOW;
	setarray .Weapon$,4741,4933,4861,4762,4934;	//orbs id
	setarray .Armor$,4933,4861,4762,4934;	//orbs id
	setarray .Shield$,4861,4762,4934;	//orbs id
	setarray .Germent$,4741,4933,4861,4762,4934;	//orbs id
	setarray .Shose$,4741,4933,4861,4762,4934;	//orbs id
	setarray .Accessary$,4741,4933,4861,4762,4934;	//orbs id
	setarray .Upper$,4741,4933,4861,4762,4934;	//orbs id
	setarray .Middel$,4741,4933,4861,4762,4934;	//orbs id
	setarray .Lower$,4741,4933,4861,4762,4934;	//orbs id

	setarray .success_chanse,100,80,60,40;	//success chanse
	.s_zeny = 10000;	//if you don't want zeny requirment set it to 0
	.s_zeny_remove = 100000;	//this for enchantment reset
	.item_is_required = 0;	//if you want the orb it self to be required 1 = yes , 0 = no
	.s_enchant_overwrite = 0;	//if 1 then you can overwrite the enchant
	.progress_time = 7;	//the time that needed to wait until the socket end
	.chosse_orb = 0;	//0 = random ,1 = yes
	.brack_chance = 50;	//the chanse that it will brack if it fail
	.remove_enchant = 1;	//0 = no ,1 = yes
	.slot_count = 0;	//0 = all 4 slot ,1 = last 3 slot ,2 = last 2 slot ,3 = last 1 slot
	end;
}