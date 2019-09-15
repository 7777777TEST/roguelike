namespace Rogue{
	class Scroll:Item{
		static Status[] data={
			new Status(){hp=0,food=0,atk=0,range=10,extval=0,lv=1,used=0,armor=0,name="identify weapon"},
			new Status(){hp=0,food=0,atk=0,range=10,extval=0,lv=1,used=0,armor=0,name="identify potion"},
			new Status(){hp=0,food=0,atk=0,range=10,extval=0,lv=1,used=0,armor=0,name="identify ring"},
			new Status(){hp=0,food=0,atk=0,range=10,extval=0,lv=1,used=0,armor=0,name="identify stick"},
			new Status(){hp=0,food=0,atk=0,range=10,extval=0,lv=1,used=0,armor=0,name="identify armor"},
			new Status(){hp=0,food=0,atk=0,range=10,extval=0,lv=2,used=0,armor=0,name="identify scroll"},
			new Status(){hp=0,food=0,atk=0,range=10,extval=0,lv=1,used=0,armor=0,name="enchant armor"},
			new Status(){hp=0,food=0,atk=0,range=10,extval=0,lv=1,used=0,armor=0,name="enchant weapon"},
			new Status(){hp=0,food=0,atk=0,range=10,extval=0,lv=1,used=0,armor=0,name="enchant ring"},
		};
		public Scroll(int id){
			status=data[id];
			status.id=id;
			status.type=Type.Scroll;
			ch='?';
			if(random.Next(100)<70||status.name=="identify scroll"){
				set(Flags.Identified);
			}
		}
		public override bool Use(ref Actor actor){
			switch(status.name){
				case "identify potion":
					Sequence.Push(new Diagnostic(Type.Potion));
					break;
				case "identify ring":
					Sequence.Push(new Diagnostic(Type.Ring));
					break;
				case "identify stick":
					Sequence.Push(new Diagnostic(Type.Stick));
					break;
				case "identify weapon":
					Sequence.Push(new Diagnostic(Type.Weapon));
					break;
				case "identify armor":
					Sequence.Push(new Diagnostic(Type.Armor));
					break;
				case "identify scroll":
					set(Flags.Identified);
					Sequence.Push(new Diagnostic(Type.Scroll));
					break;
				case "enchant armor":
					Item armor=actor.GetEquipment(Actor.Slot.Armor);
					if(armor!=null){
						if(armor.get(Flags.Cursed)){
							armor.rm(Flags.Cursed);
						}else{
							armor.set(Flags.Blessed);
						}
					}
					break;
				case "enchant weapon":
					Item weapon=actor.GetEquipment(Actor.Slot.Weapon);
					if(weapon!=null){
						if(weapon.get(Flags.Cursed)){
							weapon.rm(Flags.Cursed);
						}else{
							weapon.set(Flags.Blessed);
						}
					}
					break;
				case "enchant ring":
					Item ring=actor.GetEquipment(Actor.Slot.Ring);
					if(ring!=null){
						if(ring.get(Flags.Cursed)){
							ring.rm(Flags.Cursed);
						}else{
							ring.set(Flags.Blessed);
						}
					}
					break;
			}
			return true;
		}
		public static Item Generate(){
			return new Scroll(random.Next(data.Length));
		}
	}
}