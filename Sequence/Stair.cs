namespace Rogue{
	class Stair:State{
		string cmd="";
		int selected=-1;
		public Stair(char ch){
			if(ch=='<'){
				cmd="Down";
			}else if(ch=='>'){
				cmd="Up";
			}else{
				Sequence.Pop();
			}
		}
		public override void update(Command command){
			switch(command){
				case Command.D8:
				case Command.D2:
					selected=(selected+1)%2;
					break;
				case Command.D5:
				case Command.D6:
					if(selected==0){
						switch(cmd){
							case "Up":
								world.climb();
								break;
							case "Down":
								world.descend();
								break;
						}
					}
					Sequence.Pop();
					break;
				case Command.D0:
					Sequence.Pop();
					break;
			}
		}
		public override void draw(){
			if(selected<0)selected=0;
			Window.Rect(Window.w/4,1,Window.w/2,4);
			Window.setString(Window.w/4+2,2,cmd);
			Window.setString(Window.w/4+2,3,"Cancel");
			Window.setChar(Window.w/4+1,selected+2,'>');
		}
	}
}