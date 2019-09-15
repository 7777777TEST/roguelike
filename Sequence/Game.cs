namespace Rogue{
	class Game:State{
		public override void update(Command command){
			int dx=0,dy=0;
			if(world.player.getEffectTurn(Actor.Flags.NoCommand)+world.player.getEffectTurn(Actor.Flags.NoMove)+world.player.getEffectTurn(Actor.Flags.Stun)==0){
				switch(command){
					case Command.D0:
						Sequence.Push(new Inventory());
						return;
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
					case Command.D5:
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
					default:
						return;
				}
			}
			System.Drawing.Point target=new System.Drawing.Point(world.player.pos.X+dx,world.player.pos.Y+dy);
			int ret=0;
			ret=world.current.Move(ref world.player,target);
			Item item=world.current.atItem(world.player.pos);
			if(item!=null&&world.player.Items.Count<20){
				world.current.detach(item);
				world.player.addPack(ref item);
				Logger.Post("You now have "+item.Name);
			}
			if(ret>=0)
				world.endPlayerTurn();
			if(ret<=0&&world.player.getEffectTurn(Actor.Flags.NoCommand)+world.player.getEffectTurn(Actor.Flags.NoMove)+world.player.getEffectTurn(Actor.Flags.Stun)==0){
				char ch=world.current.getChar(world.player.pos.X,world.player.pos.Y);
				if(ch=='<'){
					Sequence.Push(new Stair('<'));
				}else if(ch=='>'){
					Sequence.Push(new Stair('>'));
				}
			}
		}
		public override void draw(){
			world.draw();
		}
	}
}