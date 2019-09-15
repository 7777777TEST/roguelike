using System.Collections.Generic;
namespace Rogue{
	class Level:Map{
		public Level(string data):base(data){}
		public List<Actor> actors=new List<Actor>();
		public List<Item> items=new List<Item>();
		public List<Trap> traps=new List<Trap>();
		public void attach(Actor actor){
			actors.Add(actor);
			actor.world=world;
		}
		public World world=null;
		public void detach(Actor actor){
			int index=actors.IndexOf(actor);
			if(index<0)
				return;
			actors.RemoveAt(index);
		}
		public Actor atActor(System.Drawing.Point pos){
			for(int i=0;i<actors.Count;i++){
				if(actors[i].pos==pos){
					return actors[i];
				}
			}
			return null;
		}
		public void removeDeadActors(){
			DeadAction deadAction=new DeadAction();
			for (int i = actors.Count - 1; i >= 0 ; i--){
				if(actors[i].Dead){
					Actor actor=actors[i];
					deadAction.perform(ref actor);
				}
			}
		}
		public int Move(ref Actor actor,System.Drawing.Point target){
			if(actor.pos==target)return 0;
			if(isMove(target)){
				if(atActor(target)==null){
					actor.pos=target;
					if(atTrap(target)!=null){
						atTrap(target).Action(ref actor);
						if(actor.status.type==Actor.Type.Hero){
							//Set Visible
						}
					}
					return 0;
				}else{
					if(atActor(target).status.type!=actor.status.type){
						Actor t=atActor(target);
						AttackAction action=new AttackAction(ref actor);
						action.perform(ref t);
						return 1;
					}
				}
			}
			removeDeadActors();
			return -1;
		}
		public void attach(Item item){
			items.Add(item);
		}
		public Item detach(Item item){
			int index=items.IndexOf(item);
			if(index<0)
				return null;
			Item ret=items[index];
			items.RemoveAt(index);
			return ret;
		}
		public Item atItem(System.Drawing.Point pos){
			// If there are more than 2 items, pick up the top.
			for(int i=items.Count-1;i>=0;i--){
				if(items[i].pos==pos){
					return items[i];
				}
			}
			return null;
		}
		public void attach(Trap trap){
			traps.Add(trap);
		}
		public Trap atTrap(System.Drawing.Point pos){
			for(int i=0;i<traps.Count;i++){
				if(traps[i].pos==pos){
					return traps[i];
				}
			}
			return null;
		}
		public new void draw(){
			base.draw();
			foreach(Trap trap in traps){
				trap.draw();
			}
			foreach(Item item in items){
				item.draw();
			}
			foreach(Actor actor in actors){
				actor.draw();
			}
		}
	}
}