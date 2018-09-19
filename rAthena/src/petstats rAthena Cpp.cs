//===== rAthena Script =======================================
//= saders @petstats command
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
//==== @petstats command
//============================================================

//ACMD_DEF(petstats),

ACMD_FUNC(petstats)
{
	struct pet_data *pd;
	s_pet_db *pet_db_ptr;
	nullpo_retr(-1, sd);
	char output[CHAT_SIZE_MAX];
	char petname[100];
	char mobname[100];
	char infostring1[100];
	char infostring2[100];
	int i;
	struct {
		const char* format;
		int value;
	} output_table[] = {
		{ NULL, 0 },
		{ NULL, 0 },
		{ "Pet ID - %d", 0 },
		{ "Pet Level - %d", 0 },
		{ "Pet Loyalty - %d", 0 },
		{ "Pet equip - %d", 0 },
		{ "Pet Hungry - %d", 0 },
		{ NULL, 0 },
		{ NULL, 0 },
		{ "Mob ID - %d", 0 },
		{ "Egg ID - %d", 0 },
		{ "Food ID - %d", 0 },
		{ "Equip ID - %d", 0 },
	};
	
	pd = sd->pd;
	if (!pd) {
		clif_displaymessage(fd, msg_txt(sd, 184)); // Sorry, but you have no pet.
		return -1;
	}
	pet_db_ptr = pd->get_pet_db();
	memset(infostring1, '\0', sizeof(infostring1));
	memset(output, '\0', sizeof(output));
	memset(petname, '\0', sizeof(petname));
	memset(infostring2, '\0', sizeof(infostring2));
	memset(mobname, '\0', sizeof(mobname));
	sprintf(infostring2, "- Pet Information -");
	output_table[0].format = infostring2;
	sprintf(petname, "Pet Name - %s", pd->pet.name);
	output_table[1].format = petname;
	output_table[2].value = pd->pet.pet_id;
	output_table[3].value = pd->pet.level;
	output_table[4].value = pd->pet.intimate;
	output_table[5].value = pd->pet.equip;
	output_table[6].value = pd->pet.hungry;
	sprintf(infostring1, "- Database Information -");
	output_table[7].format = infostring1;
	sprintf(mobname, "Mob Name - %s", pet_db_ptr->name);
	output_table[8].format = mobname;
	output_table[9].value = pet_db_ptr->class_;
	output_table[10].value = pet_db_ptr->EggID;
	output_table[11].value = pet_db_ptr->FoodID;
	output_table[12].value = pet_db_ptr->AcceID;
	for (i = 0; i<13; i++) {
		sprintf(output, output_table[i].format, output_table[i].value);
		clif_displaymessage(fd, output);
	}
	return 0;
}