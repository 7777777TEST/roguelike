namespace Rogue{
	class ZapState:State{
		Item item;
		System.Drawing.Point pos=new System.Drawing.Point(0,0);
		Actor actor;
		int dx=0,dy=0;
		public ZapState(ref Actor _actor,ref Item _item){
			actor=_actor;
			item=_item;
			pos=actor.pos;
		}
		public ZapState(ref Actor _actor,ref Item _item,int _dx,int _dy){
			actor=_actor;
			item=_item;
			dx=_dx;
			dy=_dy;
			pos=_actor.pos;
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
				if(!actor.world.current.isMove(pos.X+dx,pos.Y+dy)){
						Sequence.Pop();
					}
			}else{
				Move();
			}
		}
		int Move(){
			System.Drawing.Point nextPos=new System.Drawing.Point(pos.X+dx,pos.Y+dy);
			actor.world.draw();
			if(actor.world.current.atActor(nextPos)!=null){
				Actor target=actor.world.current.atActor(nextPos);
				item.Use(ref target);//Attack target
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
				Window.setChar(pos,'*');
				return -1;
			}
			Window.setChar(pos,' ');
			pos=nextPos;
			Window.setChar(pos,'*');
			return 0;
		}
		public override void draw(){}
	}
}