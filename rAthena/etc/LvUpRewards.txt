//===== rAthena Script =======================================
//= saders LvUpRewards
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
//==== level up rewards
//==== if the player is over he can use @LevelUpReward command
//==== to get the rewards
//============================================================
//==== please report any error you find
//============================================================
//============================================================
-	script	LvUpRewards	-1,{
OnInit:
	function LevelUpReward;
	bindatcmd("LevelUpReward",strnpcinfo(3)+"::OnLevelUpReward",0,99);
	//LevelUpReward(1,<base level>,<job id>,<zeny>,<item id>,<count>,<item id>,<count>,etc)
	//LevelUpReward(2,<job level>,<job id>,<zeny>,<item id>,<count>)
	//example:
	LevelUpReward(1,5,1,999,1002,5,1005,2);	//give 999 zeny 5 1002 and 2 1005 to Swordman (job id 1) when he level up to base level 5
	LevelUpReward(2,10,1,999,1002,5,1005,2);	//give 999 zeny 5 1002 and 2 1005 to Swordman (job id 1) when he level up to job level 10
	
	
	
	
end;
	function LevelUpReward {
		switch(getarg(0)){
			case 1:
				setd ".bZeny" + getarg(1) + "_" + getarg(2),getarg(3);
				for(.@i=4;.@i<getargcount();.@i += 2){
					.@size = getarraysize(getd(".bItem" + getarg(1) + "_" + getarg(2)));
					setd ".bItem" + getarg(1) + "_" + getarg(2) + "[" + .@size + "]", getarg(.@i);
					setd ".bCount" + getarg(1) + "_" + getarg(2) + "[" + .@size + "]", getarg(.@i + 1);
				}
				break;
			case 2:
				setd ".jZeny" + getarg(1) + "_" + getarg(2),getarg(3);
				for(.@i=4;.@i<getargcount();.@i += 2){
					.@size = getarraysize(getd(".jItem" + getarg(1) + "_" + getarg(2)));
					setd ".jItem" + getarg(1) + "_" + getarg(2) + "[" + .@size + "]", getarg(.@i);
					setd ".jCount" + getarg(1) + "_" + getarg(2) + "[" + .@size + "]", getarg(.@i + 1);
				}
				break;
		}
		return;
	}
end;

OnPCBaseLvUpEvent:
	if(getd(".bItem" + BaseLevel + "_" + class)){
		if(((Weight*100)/MaxWeight) < 79) {
			Zeny += getd(".bZeny" + BaseLevel + "_" + class);
			for(.@i=0;.@i<getarraysize(getd(".bItem" + BaseLevel + "_" + class));.@i++){
				getitem getd(".bItem" + BaseLevel + "_" + class + "[" + .@i + "]"),getd(".bCount" + BaseLevel + "_" + class + "[" + .@i + "]");
			}
			message strcharinfo(0),"Level Up Rewards : You Got Base Level " + BaseLevel + " Reward";
		}else{
			WaittingZeny = getd(".bZeny" + JobLevel + "_" + class);
			copyarray WaittingItem,getd(".bItem" + JobLevel + "_" + class),getarraysize(getd(".bItem" + JobLevel + "_" + class));
			copyarray WaittingCount,getd(".bCount" + JobLevel + "_" + class),getarraysize(getd(".bCount" + JobLevel + "_" + class));
			message strcharinfo(0),"Level Up Rewards : You are Over Weight,use @LevelUpReward to get Base Level " + BaseLevel + " Reward";
		}
	}
	end;
OnPCJobLvUpEvent:
	if(getd(".jItem" + JobLevel + "_" + class)){
		if(((Weight*100)/MaxWeight) < 79) {
			Zeny += getd(".jZeny" + JobLevel + "_" + class);
			for(.@i=0;.@i<getarraysize(getd(".jItem" + JobLevel + "_" + class));.@i++){
				getitem getd(".jItem" + JobLevel + "_" + class + "[" + .@i + "]"),getd(".jCount" + JobLevel + "_" + class + "[" + .@i + "]");
			}
			message strcharinfo(0),"[Level Up Rewards]: You Got Job Level " + JobLevel + " Reward";
		}else{
			WaittingZeny = getd(".jZeny" + JobLevel + "_" + class);
			copyarray WaittingItem,getd(".jItem" + JobLevel + "_" + class),getarraysize(getd(".jItem" + JobLevel + "_" + class));
			copyarray WaittingCount,getd(".jCount" + JobLevel + "_" + class),getarraysize(getd(".jCount" + JobLevel + "_" + class));
			message strcharinfo(0),"Level Up Rewards : You are Over Weight,use @LevelUpReward to get Base Level " + BaseLevel + " Reward";
		}
	}
	end;
OnLevelUpReward:
	if(((Weight*100)/MaxWeight) < 79) {
		if(getarraysize(WaittingItem)){
			Zeny += WaittingZeny;
			for(.@i=0;.@i<getarraysize(WaittingItem);.@i++){
				getitem WaittingItem[.@i],WaittingCount[.@i];
			}
			WaittingZeny = 0;
			deletearray WaittingItem,getarraysize(WaittingItem);
			deletearray WaittingCount,getarraysize(WaittingCount);
			message strcharinfo(0),"Level Up Rewards : You got all the rewards!";
		}else{
			message strcharinfo(0),"Level Up Rewards : You don't have any reward!";
		}
	}else{
		message strcharinfo(0),"Level Up Rewards : You are Over Weight , use the storage before you use the command!";
	}
}