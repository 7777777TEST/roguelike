namespace Rogue{
	class Trap:Object{
		public enum Type{
			Arrow,
			Bear,
			Sleep,
		}
		Type type;
		public Trap(Type _type){
			type=_type;
			ch='^';
		}
		public static Trap Generate(){
			return new Trap((Type)System.Enum.ToObject(typeof(Type),random.Next(System.Enum.GetValues(typeof(Type)).Length)));
		}
		public void Action(ref Actor actor){
			switch(type){
				case Type.Arrow:
					if(random.Next(0,100)>actor.status.speed){
						if(actor.status.type==Actor.Type.Hero){
							Logger.Post("An arrow shoots out at you!");
							Logger.Post("You are hit by an arrow.");
						}
						actor.status.hp-=10;
						if(random.Next(10)>3){
							actor.world.current.attach(new Weapon(0));
						}
					}
					break;
				case Type.Bear:
					if(random.Next(0,100)>actor.status.speed){
						if(actor.status.type==Actor.Type.Hero){
							Logger.Post("you are caught in a bear trap.");
						}
						actor.status.hp-=10;
						actor.addEffect(Actor.Flags.NoMove,5);
					}
					break;
				case Type.Sleep:
					if(random.Next(0,100)>actor.status.speed){
						if(actor.status.type==Actor.Type.Hero){
							Logger.Post("You are surrounded by stranger mist");
							Logger.Post("You felt sleepy...");
						}
						actor.addEffect(Actor.Flags.NoCommand,5);
					}
					break;
			}
		}
	}
}