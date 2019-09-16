namespace Rogue{
	class Ring:Item{
		static Status[] data={
			new Status(){hp=0,food=0,atk=1,range=10,extval=0,lv=3,used=0,armor=50,name="protect ring"},
			new Status(){hp=0,food=0,atk=30,range=10,extval=0,lv=5,used=0,armor=0,name="attack ring"},
			new Status(){hp=0,food=50,atk=1,range=10,extval=0,lv=2,used=0,armor=0,name="food ring"},
			new Status(){hp=50,food=0,atk=1,range=10,extval=0,lv=2,used=0,armor=0,name="restore ring"},
		};
		public Ring(int id){
			status=data[id];
			status.id=id;
			status.type=Type.Ring;
			Count=1;
			ch='=';
		}
		public static Item Generate(){
			return new Ring(random.Next(data.Length));
		}
		public override bool Use(ref Actor actor){
			set(Flags.Identified);
			Item ring=actor.GetEquipment(Actor.Slot.Ring);
			if(ring==this){
				if(unequip(ref actor)){
					if(actor.status.type==Actor.Type.Hero)
					Logger.Post("You used to be wearing "+Name);
					rm(Flags.Equip);
				}
				return true;
			}else if(ring!=null){
				if(ring.unequip(ref actor)){
					if(actor.status.type==Actor.Type.Hero)
					Logger.Post("You used to be wearing "+ring.Name);
				}else{
					return false;
				}
			}
			if(get(Flags.Cursed)){
				status.atk=-System.Math.Abs(status.atk);
				status.armor=-System.Math.Abs(status.armor);
			}
			set(Flags.Equip);
			Item target=this;
			actor.Equipment(Actor.Slot.Ring,ref target);
			if(actor.status.type==Actor.Type.Hero){
				set(Flags.Identified);
				Logger.Post("You are now wearing "+Name);
			}
			return true;
		}
	}
}