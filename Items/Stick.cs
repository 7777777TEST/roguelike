namespace Rogue{
	class Stick:Item{
		static Status[] data={
			new Status(){hp=0,food=0,atk=10,range=10,extval=10,lv=1,used=0,armor=20,name="Light"},
			new Status(){hp=0,food=0,atk=100,range=10,extval=50,lv=1,used=0,armor=20,name="Fire"},
			new Status(){hp=0,food=0,atk=10,range=10,extval=50,lv=1,used=0,armor=20,name="Poison"},
			new Status(){hp=0,food=0,atk=10,range=10,extval=50,lv=1,used=0,armor=20,name="Stun"},
		};
		public Stick(int id){
			status=data[id];
			status.id=id;
			status.type=Type.Stick;
			ch='/';
		}
		public static Item Generate(){
			return new Stick(random.Next(data.Length));
		}
		public override bool Use(ref Actor actor){
			if(actor.status.type==Actor.Type.Hero)
			set(Flags.Identified);
			actor.status.hp-=status.atk;
			status.used++;
			switch(status.name){
				case "Poison":
					actor.addEffect(Actor.Flags.Poison,10);
					break;
				case "Sleep":
					actor.addEffect(Actor.Flags.NoCommand,10);
					break;
				case "Light":
					actor.world.current.draw();
					Window.Render();
					break;
				case "Stun":
					actor.addEffect(Actor.Flags.Stun,10);
					break;
			}
			if(status.extval<status.used){
				Item item=this;
				actor.unpack(ref item,false);
			}
			return true;
		}
	}
}