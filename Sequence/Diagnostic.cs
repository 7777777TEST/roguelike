using System.Collections.Generic;
namespace Rogue{
	class Diagnostic:State{
		int main_selected=-1;
		List<Item> items=new List<Item>();
		public Diagnostic(Item.Type type){
			for(int i=0;i<world.player.Items.Count;i++){
				if(world.player.Items[i].status.type==type){
					items.Add(world.player.Items[i]);
				}
			}
		}
		public override void update(Command command){
			if(command==Command.D0){
				Sequence.Pop();
				return;
			}
			if(world.player.Items.Count==0)return;
			switch(command){
				case Command.D2:
					if(main_selected>=0)
					main_selected=(main_selected+1)%items.Count;
					break;
				case Command.D8:
					main_selected=(main_selected-1+items.Count)%items.Count;
					break;
				case Command.D5:
				case Command.D6:
					items[main_selected].set(Item.Flags.Identified);
					Logger.Post("It's "+items[main_selected].Name);
					Sequence.Pop();
					break;
			}
		}
		public override void draw(){
			if(main_selected<0){
				main_selected=0;
			}
			Window.Rect(Window.w/4,1,Window.w/2,items.Count+2);
			for(int i=0;i<items.Count;i++){
				if(i==main_selected){
					Window.setChar(Window.w/4+1,2+i,'>');
				}
				Window.setString(Window.w/4+2,2+i,items[i].Count.ToString()+" "+items[i].Name);
			}
		}
	}
}