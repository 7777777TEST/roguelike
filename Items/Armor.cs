namespace Rogue{
	class Armor:Item{
		static Status[] data={
			new Status(){hp=0,food=0,atk=10,range=10,extval=0,lv=1,used=0,armor=20,name="RingMail"},
			new Status(){hp=0,food=0,atk=10,range=10,extval=0,lv=2,used=0,armor=25,name="ChainMail"},
			new Status(){hp=0,food=0,atk=10,range=10,extval=0,lv=5,used=0,armor=30,name="PlateMail"},
		};
		public Armor(int id){
			status=data[id];
			status.id=id;
			status.type=Type.Armor;
			Count=1;
			ch='[';
		}
		public static Item Generate(){
			return new Armor(random.Next(data.Length));
		}
		public override bool Use(ref Actor actor){
			Item armor=actor.GetEquipment(Actor.Slot.Armor);
			if(armor==this){
				if(unequip(ref actor)){
					if(actor.status.type==Actor.Type.Hero)
					Logger.Post("You used to be wearing "+Name);
					rm(Flags.Equip);
				}
				return true;
			}else if(armor!=null){
				if(armor.unequip(ref actor)){
					if(actor.status.type==Actor.Type.Hero)
					Logger.Post("You used to be wearing "+armor.Name);
				}else{
					return false;
				}
			}
			if(get(Flags.Cursed)){
				status.armor=-System.Math.Abs(status.armor);
			}
			set(Flags.Equip);
			Item target=this;
			actor.Equipment(Actor.Slot.Armor,ref target);
			if(actor.status.type==Actor.Type.Hero){
				set(Flags.Identified);
				Logger.Post("You are now wearing "+Name);
			}
			return true;
		}
	}
}