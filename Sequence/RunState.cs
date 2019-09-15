namespace Rogue{
	class RunState:State{
		int dx=0,dy=0;
		Actor actor;
		public RunState(ref Actor _actor){
			actor=_actor;
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
				}
				if(!isMove(actor.pos.X+dx,actor.pos.Y+dy)){
						Sequence.Pop();
					}
			}else{
				Move();
			}
		}
		public void Move(){
			System.Drawing.Point nextPos=new System.Drawing.Point(actor.pos.X+dx,actor.pos.Y+dy);
			actor.world.draw();
			if(!isMove(nextPos.X,nextPos.Y)){
				if(actor.status.type==Actor.Type.Hero){
					Sequence.Pop();
					actor.world.endPlayerTurn();
				}
				actor.draw();
				return;
			}
			actor.pos=nextPos;
			actor.draw();
		}
		bool isMove(int x,int y){
			return actor.world.current.isMove(x,y)&&
				actor.world.current.atItem(new System.Drawing.Point(x,y))==null&&
				actor.world.current.atTrap(new System.Drawing.Point(x,y))==null&&
				actor.world.current.atActor(new System.Drawing.Point(x,y))==null;
		}
		public override void draw(){}
	}
}