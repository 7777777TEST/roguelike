namespace Rogue{
	class Effect{
		static System.Random random=new System.Random(System.Environment.TickCount);
		public Effect(Actor.Flags _flag,int _turn){
			flag=_flag;
			turn=_turn;
		}
		public Actor.Flags flag{get;set;}
		public bool isFinished(){
			return turn<=0;
		}
		public void add(int _turn){
			turn+=_turn;
		}
		public int Turn{
			get{
				return turn;
			}
		}
		public void start(ref Actor actor){
			switch(flag){
				case Actor.Flags.Stun:
					Logger.Post(actor.status.name+" said \"Who am I ?\"");
					break;
				case Actor.Flags.NoCommand:
					break;
				case Actor.Flags.Blind:
					actor.world.fovStrength=0.0f;
					Logger.Post(actor.status.name+" lost light.");
					break;
				case Actor.Flags.Poison:
					Logger.Post(actor.status.name+" felt sick.");
					if(random.Next(10)==1){
						actor.addEffect(Actor.Flags.Blind,3);
					}
					break;
			}
		}
		public void update(ref Actor actor){
			switch(flag){
				case Actor.Flags.Poison:
					actor.status.hp-=3;
					if(actor.status.type==Actor.Type.Hero){
						if(actor.status.hp<=0){
							Sequence.Pop();
							actor.dead_reason="  died from\n   illness.";
							Sequence.Push(new GameOver(actor.dead_reason));
						}
					}
					break;
			}
			turn--;
		}
		public void end(ref Actor actor){
			switch(flag){
				case Actor.Flags.Poison:
				case Actor.Flags.Stun:
					Logger.Post(actor.status.name+" felt better.");
					break;
				case Actor.Flags.NoCommand:
				case Actor.Flags.NoMove:
					Logger.Post(actor.status.name+" can move again.");
					break;
				case Actor.Flags.Blind:
					actor.world.fovStrength=1.0f;
					Logger.Post(actor.status.name+" regained light.");
					break;
			}
		}
		int turn=-1;
	}
}