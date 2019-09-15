namespace Rogue{
	class AttackAction:Action{
		Actor actor;
		static System.Random random=new System.Random(System.Environment.TickCount);
		public AttackAction(ref Actor _actor){
			actor=_actor;
		}
		public override bool perform(ref Actor target){
			if(actor.Dead||target.Dead)return false;
			int hit=actor.status.str;
			if(random.Next(0,20)<target.status.speed+1){
				hit=0;
				Logger.Post(actor.status.name+" missed "+target.status.name);
			}else{
				hit=(int)(hit*System.Math.Pow(0.9735,target.status.armor));
				actor.attack(ref target);
				target.Hp=target.Hp-hit;
				if(!target.Dead)
				Logger.Post(actor.status.name+" hit "+target.status.name);
				else{
					actor.status.exp+=target.status.level;
					int[] expTable={10,50,100,200,300,500,1000,2000,3000,5000,10000};
					if(expTable[actor.status.level-1]<=actor.status.exp){
						actor.LevelUp();
					}
					target.dead_reason="  killed by\n";
					for(int i=0;i<6-actor.status.name.Length/2;i++){
						target.dead_reason+=" ";
					}
					target.dead_reason+=actor.status.name;
					Logger.Post(actor.status.name+" killed "+target.status.name);
				}
			}
			return true;
		}
	}
}