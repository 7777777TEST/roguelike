namespace Rogue{
	class Potion:Item{
		static Status[] data={
			new Status(){hp=1,name="speed potion",food=1,lv=7},
			new Status(){hp=1,name="water",food=1,lv=1},
			new Status(){hp=1,name="sleep",food=1,lv=3},
			new Status(){hp=-10,name="poison",food=1,lv=3},
			new Status(){hp=3,name="fruit juice",food=5,lv=1},
			new Status(){hp=10,name="healing",food=2,lv=1},
			new Status(){hp=100,name="extra potion",food=4,lv=10},
			new Status(){hp=50,name="gain level",food=1,lv=7},
		};
		public Potion(int id){
			status=data[id];
			status.id=id;
			status.type=Type.Potion;
			Count=1;
			ch='!';
		}
		public static Item Generate(){
			return new Potion(random.Next(data.Length));
		}
		public override bool Use(ref Actor actor){
			if(actor.status.type==Actor.Type.Hero)
			set(Flags.Identified);
			actor.Hp+=status.hp;
			actor.Food+=status.food;
			switch(status.name){
				case "gain level":
					actor.LevelUp();
					break;
				case "sleep":
					actor.addEffect(Actor.Flags.NoCommand,10);
					break;
				case "poison":
					actor.addEffect(Actor.Flags.Poison,15);
					break;
				case "speed potion":
					Sequence.Push(new RunState(ref actor));
					break;
			}
			return true;
		}
	}
}