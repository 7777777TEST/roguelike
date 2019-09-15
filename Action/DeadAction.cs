using System.Collections.Generic;
namespace Rogue{
	class DeadAction:Action{
		public override bool perform(ref Actor actor){
			foreach(Item item in actor.Items){
				item.pos=actor.pos;
				actor.world.current.attach(item);
			}
			System.Random random=new System.Random(System.Environment.TickCount);
			if(random.Next(300)>250){
				return true;
			}
			Item meat=new Food(0);
			meat.pos=actor.pos;
			if(random.Next(300)>270||actor.getEffectTurn(Actor.Flags.Poison)>0){
				meat.set(Item.Flags.Poisoned);
			}
			actor.world.current.attach(meat);
			World world=actor.world;
			world.current.detach(actor);
			return true;
		}
	}
}