using System.Collections.Generic;

namespace Rogue{
	abstract class Item:Object{
		public enum Flags{
			None,
			Cursed,
			Blessed,
			Identified,
			Many,
			Equip,
			Poisoned
		}
		public enum Type{
			Potion,
			Food,
			Armor,
			Weapon,
			Gold,
			Scroll,
			Ring,
			Stick,
			Amulet
		}
		public struct Status{
			public static bool operator ==(Status s1,Status s2){
				return s1.type==s2.type&&s1.id==s2.id&&s1.lv==s2.lv;
			}
			public static bool operator !=(Status s1,Status s2){
				return s1.type!=s2.type||s1.id!=s2.id||s1.lv!=s2.lv;
			}
			public Type type;
			public int id,hp,food,atk,range,extval,lv,used,armor;
			public string name;
			public override bool Equals(object obj)
			{
				Status status= (Status)obj;
				return type == status.type && id == status.id && lv == status.lv;
			}
			public override int GetHashCode()
			{
				var hashCode = -951334205;
				hashCode = hashCode * -1521134295 + type.GetHashCode();
				hashCode = hashCode * -1521134295 + id.GetHashCode();
				hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(name);
				return hashCode;
			}
		}
		public abstract bool Use(ref Actor actor);
		public string Name{
			get{
				if(!get(Flags.Identified))
					return status.type.ToString();
				string str="";
				if(get(Flags.Cursed)){
					str+="cursed ";
				}else if(get(Flags.Blessed)){
					str+="blessed ";
				}
				if(get(Flags.Poisoned)){
					str+=" poisoned ";
				}
				return str+status.name;
			}
		}
		public Item detachOne(){
			if(Count>1){
				Count--;
				Item item=createItem(status.type,status.id);
				item.Count=1;
				item.Flag=Flag;
				return item;
			}
			return null;
		}
		public static Item createItem(){
			switch(random.Next(System.Enum.GetValues(typeof(Type)).Length)){
				case (int)Type.Armor:
					return Armor.Generate();
				case (int)Type.Food:
					return Food.Generate();
				case (int)Type.Gold:
					return new Gold(0);
				case (int)Type.Potion:
					return Potion.Generate();
				case (int)Type.Ring:
					return Ring.Generate();
				case (int)Type.Stick:
					return Stick.Generate();
				case (int)Type.Weapon:
					return Weapon.Generate();
				case (int)Type.Scroll:
					return Scroll.Generate();
			}
			return Scroll.Generate();
		}
		static Item createItem(Type type,int id){
			switch(type){
				case Type.Armor:
					Armor armor=new Armor(id);
					armor.Count=1;
					return armor;
				case Type.Weapon:
					Weapon weapon=new Weapon(id);
					weapon.Count=1;
					return weapon;
				case Type.Ring:
					Ring ring=new Ring(id);
					ring.Count=1;
					return ring;
				case Type.Potion:
					Potion potion=new Potion(id);
					potion.Count=1;
					return potion;
				case Type.Food:
					Food food=new Food(id);
					food.Count=1;
					return food;
				case Type.Stick:
					Stick stick=new Stick(id);
					stick.Count=1;
					return stick;
				case Type.Scroll:
					Scroll scroll=new Scroll(id);
					scroll.Count=1;
					return scroll;
			}
			return null;
		}
		public bool unequip(ref Actor actor){
			for(int i=0;i<System.Enum.GetNames(typeof(Actor.Slot)).Length;i++){
				Actor.Slot slot=(Actor.Slot)System.Enum.ToObject(typeof(Actor.Slot),i);
				if(actor.GetEquipment(slot)==this){
					if(get(Flags.Cursed)){
						Logger.Post("you can't. Your "+Name+" appears to be cursed.");
						return false;
					}else{
						Item item=null;
						actor.Equipment(slot,ref item);
						break;
					}
				}
			}
			return true;
		}
		public int Count=1;
		public void set(Flags f){
			set(1<<(int)f);
			if(f==Flags.Cursed){
				rm(1<<(int)Flags.Blessed);
			}else if(f==Flags.Blessed){
				rm(1<<(int)Flags.Cursed);
			}
		}
		public bool get(Flags f){
			 return get(1<<(int)f);
		}
		public void rm(Flags f){
			rm(1<<(int)f);
		}
		public Status status;
	}
}