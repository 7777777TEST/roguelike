using System.Collections.Generic;
namespace Rogue{
	class Inventory:State{
		List<string> commands=new List<string>(){"Use","Drop","Throw"};
		int main_selected=-1,sub_selected=-1;
		public Inventory(){
		}
		public override void update(Command command){
			if(sub_selected<0){
				MainMenu(command);
			}else{
				SubMenu(command);
			}
		}
		void MainMenu(Command command){
			if(command==Command.D0){
				Sequence.Pop();
				return;
			}
			if(world.player.Items.Count==0)return;
			switch(command){
				case Command.D2:
					if(main_selected>=0)
					main_selected=(main_selected+1)%world.player.Items.Count;
					break;
				case Command.D8:
					main_selected=(main_selected-1+world.player.Items.Count)%world.player.Items.Count;
					break;
				case Command.D5:
				case Command.D6:
					MainEnter();
					break;
			}
		}
		void MainEnter(){
			switch(world.player.Items[main_selected].status.type){
				case Item.Type.Armor:
					if(world.player.GetEquipment(Actor.Slot.Armor)==world.player.Items[main_selected]){
						commands[0]="Remove";
					}else{
						commands[0]="Equip";
					}
					break;
				case Item.Type.Weapon:
					if(world.player.GetEquipment(Actor.Slot.Weapon)==world.player.Items[main_selected]){
						commands[0]="Remove";
					}else{
						commands[0]="Equip";
					}
					break;
				case Item.Type.Ring:
					if(world.player.GetEquipment(Actor.Slot.Ring)==world.player.Items[main_selected]){
						commands[0]="Remove";
					}else{
						commands[0]="Equip";
					}
					break;
				case Item.Type.Food:
					commands[0]="Eat";
					break;
				case Item.Type.Potion:
					commands[0]="Drink";
					break;
				case Item.Type.Stick:
					commands[0]="Zap";
					break;
				default:
					break;
			}
			sub_selected=0;
		}
		void SubMenu(Command command){
			switch(command){
				case Command.D2:
					if(sub_selected>=0)
					sub_selected=(sub_selected+1)%commands.Count;
					break;
				case Command.D8:
					sub_selected=(sub_selected-1+commands.Count)%commands.Count;
					break;
				case Command.D5:
				case Command.D6:
					SubEnter();
					break;
				case Command.D0:
				sub_selected=-1;
					break;
			}
		}
		void SubEnter(){
			Sequence.Pop();
			Item item=world.player.Items[main_selected];
			switch(commands[sub_selected]){
				case "Use":
				case "Drink":
				case "Eat":
					item.set(Item.Flags.Identified);
					world.player.unpack(ref item,false).Use(ref world.player);
					break;
				case "Remove":
				case "Equip":
					item.set(Item.Flags.Identified);
					item.Use(ref world.player);
					break;
				case "Zap":
					item.set(Item.Flags.Identified);
					Logger.Post("In what direction?");
					Sequence.Push(new ZapState(ref world.player,ref item));
					return;
				case "Drop":
					if(item.get(Item.Flags.Equip)){
						if(!item.unequip(ref world.player)){
							break;
						}
					}
					world.current.attach(world.player.unpack(ref item,false));
					break;
				case "Throw":
					Logger.Post("In what direction?");
					Item throw_item=world.player.unpack(ref item,false);
					if(throw_item==null){
						Logger.Post("It appears cursed.");
						Logger.Post("You can't throw it.");
						break;
					}
					Sequence.Push(new ThrowState(ref world.player,ref throw_item));
					return;
				default:
					break;
			}
			world.endPlayerTurn();
		}
		public override void draw(){
			if(main_selected<0){
				main_selected=0;
			}
			if(world.player.Items.Count<=0){
				Window.Rect(Window.w/4,1,Window.w/2,3);
				Window.setString(Window.w/4,2,"your bag is empty.");
				return;
			}
			Window.Rect(Window.w/4,1,Window.w/2,world.player.Items.Count+2);
			for(int i=0;i<world.player.Items.Count;i++){
				if(i==main_selected){
					Window.setChar(Window.w/4+1,2+i,'>');
				}
				Window.setString(Window.w/4+2,2+i,world.player.Items[i].Count.ToString()+" "+world.player.Items[i].Name);
				if(world.player.GetEquipment(Actor.Slot.Armor)==world.player.Items[i]||
					world.player.GetEquipment(Actor.Slot.Weapon)==world.player.Items[i]||
					world.player.GetEquipment(Actor.Slot.Ring)==world.player.Items[i]){
						Window.setChar(Window.w*3/4-2,2+i,'+');
					}
			}
			if(sub_selected<0)return;
			Window.Rect(Window.w*3/4,1,10,commands.Count+2);
			for(int i=0;i<commands.Count;i++){
				if(i==sub_selected){
					Window.setChar(Window.w*3/4+1,2+i,'>');
				}
				Window.setString(Window.w*3/4+2,2+i,commands[i]);
			}
		}
	}
}