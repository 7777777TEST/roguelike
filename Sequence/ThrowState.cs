namespace Rogue{
	class ThrowState:State{
		Item item=null;
		Actor actor=null;
		int dx=0,dy=0;
		public ThrowState(ref Actor _actor,ref Item _item){
			actor=_actor;
			item=_item;
		}
		public override void update(Command command){
			if(dx==0&&dy==0){
				switch(command){
					case Command.D1:
						dx=-1;dy=1;
						break;
					case Command.D2:
						dy=1;
						break;
					case Command.D3:
						dx=1;dy=1;
						break;
					case Command.D4:
						dx=-1;
						break;
					case Command.D6:
						dx=1;
						break;
					case Command.D7:
						dx=-1;dy=-1;
						break;
					case Command.D8:
						dy=-1;
						break;
					case Command.D9:
						dx=1;dy=-1;
						break;
					case Command.None:
						return;
					default:
						Sequence.Pop();
						return;
				}
				if(!actor.world.current.isMove(item.pos.X+dx,item.pos.Y+dy)){
						Sequence.Pop();
					}
			}else{
				Move();
			}
		}
		int Move(){
			System.Drawing.Point nextPos=new System.Drawing.Point(item.pos.X+dx,item.pos.Y+dy);
			actor.world.draw();
			if(actor.world.current.atActor(nextPos)!=null){
				actor.world.current.atActor(nextPos).status.hp-=item.status.atk;
				if(item.get(Item.Flags.Poisoned)){
					actor.addEffect(Actor.Flags.Poison,10);
				}
				Logger.Post(actor.status.name+" attacked "+actor.world.current.atActor(nextPos).status.name);
				Sequence.Pop();
				if(actor.status.type==Actor.Type.Hero){
					actor.world.endPlayerTurn();
				}
				return 1;
			}else if(!actor.world.current.isMove(nextPos)){
				if(actor.status.type==Actor.Type.Hero){
					actor.world.endPlayerTurn();
				}
				Sequence.Pop();
				actor.world.current.attach(item);
				item.draw();
				return -1;
			}
			Window.setChar(item.pos,' ');
			item.pos=nextPos;
			item.draw();
			return 0;
		}
		public override void draw(){}
	}
}