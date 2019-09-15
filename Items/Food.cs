namespace Rogue{
	class Food:Item{
		static Status[] data={
			new Status(){hp=3,food=10,atk=3,range=0,extval=0,lv=1,used=0,armor=0,name="meat"},
			new Status(){hp=3,food=5,atk=4,range=0,extval=0,lv=1,used=0,armor=0,name="apple"},
			new Status(){hp=5,food=15,atk=5,range=0,extval=0,lv=1,used=0,armor=0,name="big apple"},
		};
		public Food(int id){
			status=data[id];
			status.id=id;
			status.type=Type.Food;
			ch='%';
		}
		public static Item Generate(){
			return new Food(random.Next(data.Length));
		}
		public override bool Use(ref Actor actor){
			set(Flags.Identified);
			int tmp=actor.Food;
			Logger.Post("Player is eating "+Name);
			actor.Food=tmp+status.food;
			actor.Hp+=status.hp;
			if(get(Flags.Poisoned)){
				actor.addEffect(Actor.Flags.Poison,10);
			}
			return true;
		}
	}
}