namespace Rogue{
	class Weapon:Item{
		static Status[] data={
			new Status(){hp=0,food=0,atk=10,range=10,extval=0,lv=1,used=0,armor=0,name="arrow"},
			new Status(){hp=0,food=0,atk=20,range=10,extval=0,lv=1,used=0,armor=0,name="dagger"},
			new Status(){hp=0,food=0,atk=30,range=10,extval=0,lv=3,used=0,armor=0,name="mace"},
			new Status(){hp=0,food=0,atk=50,range=10,extval=0,lv=5,used=0,armor=0,name="sword"},
		};
		public Weapon(int id){
			status=data[id];
			status.id=id;
			status.type=Type.Weapon;
			Count=1;
			ch='(';
		}
		public static Item Generate(){
			return new Weapon(random.Next(data.Length));
		}
		public override bool Use(ref Actor actor){
			Item weapon=actor.GetEquipment(Actor.Slot.Weapon);
			if(weapon==this){
				if(unequip(ref  actor)){
					int origin=actor.MaxStr;
					actor.MaxStr=origin-status.atk;
					origin=actor.Str;
					actor.Str=origin-status.atk;
					if(actor.status.type==Actor.Type.Hero)
					Logger.Post("You used to be wearing "+Name);
					rm(Flags.Equip);
				}
				return true;
			}else if(weapon!=null){
				if(weapon.unequip(ref actor)){
					int origin=actor.MaxStr;
					actor.MaxStr=origin-weapon.status.atk;
					origin=actor.Str;
					actor.Str=origin-weapon.status.atk;
					if(actor.status.type==Actor.Type.Hero)
					Logger.Post("You used to be wearing "+weapon.Name);
				}else{
					return false;
				}
			}
			set(Flags.Equip);
			Item target=this;
			actor.Equipment(Actor.Slot.Weapon,ref target);
			actor.MaxStr+=status.atk;
			actor.Str+=status.atk;
			if(actor.status.type==Actor.Type.Hero){
				set(Flags.Identified);
				Logger.Post("You are now wearing "+Name);
			}
			return true;
		}
	}
}