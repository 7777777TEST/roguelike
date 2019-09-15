namespace Rogue{
	class GameOver:State{
		string data="";
		public GameOver(string cause){
			data=cause;
		}
		public override void update(Command command){}
		public override void draw(){
			Window.setString(1,1, "  /----------\\  ");
			Window.setString(1,2, " /            \\ ");
			Window.setString(1,3, "/     REST     \\");
			Window.setString(1,4, "|      IN      |");
			Window.setString(1,5, "|    PEACE     |");
			Window.setString(1,6, "|              |");
			Window.setString(1,7, "|              |");
			Window.setString(1,8, "|              |");
			Window.setString(1,9, "|              |");
			Window.setString(1,10,"|              |");
			Window.setString(1,11,"|              |");
			Window.setString(1,12,"|              |");
			Window.setString(1,13,"|* *  *  *   * |");
			Window.setString(0,14,"--/)/)()-/)-/)(\\--");
			Window.setString(5,7,world.player.status.name);
			Window.setString(2,9,data);
		}
	}
}