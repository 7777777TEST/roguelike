using System.Collections.Generic;
namespace Rogue{
	class Actor:Object{
		public enum Type{
			Hero,
			Ally,
			Enemy,
			Neutral
		}
		public enum Slot{
			Weapon,
			Armor,
			Ring,
		}
		public enum Flags{
			None,
			NoMove,// Number of turns held in place
			NoCommand,// Number of turns asleep
			Poison,//meat of creature has been poisoned
			IsInvisible,
			Stun,
			Blind
		}
		public struct Status{
			public Type type;
			public string name;
			public int exp,level,armor,hp,maxhp,str,maxstr,default_flag,speed;
			public char ch;
			public Func attack;
		}
		private int food=100,maxfood=100;
		public string dead_reason="";
		public int Food{
			get{
				return food;
			}
			set{
				food=value;
				if(value>maxfood)food=maxfood;
				if(food<0&&System.Math.Abs(value)%10==2){
					addEffect(Flags.NoCommand,10);
					Logger.Post("You can't move because of hunger.");
				}
				if(food<-50){
					status.hp=0;
					dead_reason="died by hunger";
					Logger.Post("You died by hunger.");
				}
			}
		}
		public int MaxFood{
			get{
				return maxfood;
			}
			set{
				food=value;
				if(maxfood<food)food=maxfood;
			}
		}
		public int Hp{
			get{
				return status.hp;
			}
			set{
				if(value>status.maxhp)status.hp=status.maxhp;
				else status.hp=value;
			}
		}
		public int MaxHp{
			get{
				return status.maxhp;
			}
			set{
				status.maxhp=value;
				if(status.maxhp<status.hp)status.hp=status.maxhp;
			}
		}
		public int Str{
			get{
				return status.str;
			}
			set{
				if(value>status.maxstr)status.str=status.maxstr;
				else status.str=value;
			}
		}
		public int MaxStr{
			get{
				return status.maxstr;
			}
			set{
				status.maxstr=value;
			}
		}
		public void LevelUp(){
				MaxHp=MaxHp+random.Next(0,3)+3;
				MaxStr=MaxStr+random.Next(0,3);
				status.armor+=random.Next(0,3);
				status.level++;
		}
		public int Level{
			get{
				return status.level;
			}
		}
		public bool Dead{
			get{
				return Hp<=0;
			}
		}
		public List<Item> Items=new List<Item>();
		public World world=null;
		public Item addPack(ref Item item){
			switch(item.status.type){
				case Item.Type.Gold:
					gold+=item.Count;
					return null;
				case Item.Type.Potion:
				case Item.Type.Scroll:
				case Item.Type.Food:
				case Item.Type.Weapon:
					for(int i=0;i<Items.Count;i++){
						if(Items[i].status==item.status){
							int flag=item.Flag;
							if(Items[i].get(Item.Flags.Identified))
								item.set(Item.Flags.Identified);
							if(Items[i].get(Item.Flags.Many))
								item.set(Item.Flags.Many);
							if(item.Flag!=Items[i].Flag){
								item.Flag=flag;
								continue;
							}
							Items[i].set(Item.Flags.Many);
							Items[i].Count+=item.Count;
							return Items[i];
						}
					}
					break;
			}
			Items.Add(item);
			return item;
		}
		public Item unpack(ref Item item,bool hurl){
			int i=Items.IndexOf(item);
			if(i<0)return null;
			if(item.get(Item.Flags.Equip)){
				Actor target=this;
				item.Use(ref target);
				if(item.get(Item.Flags.Equip))return null;
			}
			if(Items[i].Count>1){
				if(Items[i].Count==1){
					Items[i].rm(Item.Flags.Many);
				}
				Item ret=item.detachOne();
				ret.pos=pos;
				return ret;
			}else{
				Item ret=Items[i];
				Items.RemoveAt(i);
				ret.pos=pos;
				return ret;
			}
		}
		private List<Item> equipment=new List<Item>(){null,null,null};
		public void Equipment(Slot slot,ref Item item){
			equipment[(int)slot]=item;
		}
		public Item GetEquipment(Slot slot){
			return equipment[(int)slot];
		}
		public bool IsEquipment(Slot slot,int id){
			if(equipment[(int)slot]!=null){
				return equipment[(int)slot].status.id==id;
			}
			return false;
		}
		public bool hasRing(int id){
			return IsEquipment(Slot.Ring,id);
		}
		private List<Effect> Effects=new List<Effect>();
		public void addEffect(Flags flag,int turn){
			set(1<<(int)flag);
			for (int i = 0; i < Effects.Count; i++)
			{
				if(Effects[i].flag==flag){
					Effects[i].add(turn);
				}
			}
			Effect effect=new Effect(flag,turn);
			Actor target=this;
			effect.start(ref target);
			Effects.Add(effect);
		}
		public void updateEffect(){
			Actor target=this;
			for (int i = Effects.Count - 1; i >= 0 ; i--)
			{
				Effects[i].update(ref target);
				if(Effects[i].isFinished()){
					Effects[i].end(ref target);
					Effects.RemoveAt(i);
				}
			}
		}
		public void rmEffect(Flags flag){
			rm(1<<(int)flag);
			Actor target=this;
			for (int i = Effects.Count - 1; i >= 0 ; i--)
			{
				if(Effects[i].flag==flag){
					Effects[i].end(ref target);
					Effects.RemoveAt(i);
				}
			}
		}
		public int getEffectTurn(Flags flag){
			set(1<<(int)flag);
			for (int i = 0; i < Effects.Count; i++)
			{
				if(Effects[i].flag==flag){
					return Effects[i].Turn;
				}
			}
			return 0;
		}
		public Actor(Status _status,System.Drawing.Point point){
			status=_status;
			ch=status.ch;
			pos=point;
			if(status.speed==0)status.speed=1;
			set(status.default_flag);
		}
		public virtual void attack(ref Actor actor){
			if(status.attack!=null){
				status.attack(ref actor);
			}
		}
		public string HungryStatus{
			get{
				if(Food<0){
					return "Faint";
				}else if(Food<10){
					return "Weak";
				}else if(Food<30){
					return "Hungry";
				}else{
					return "";
				}
			}
		}
		public int gold=0;
		public Status status;
		public System.Drawing.Point nextPos;//for AI
	}
}