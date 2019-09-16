namespace Rogue{
	class Monster:Actor{
		static Status[] data={
			new Status(){name="bug",exp=0,level=1,armor=0,hp=5,maxhp=5,str=5,maxstr=5,default_flag=0,ch='B',speed=3,attack=null},
			new Status(){name="snake",exp=0,level=2,armor=2,hp=5,maxhp=5,str=5,maxstr=5,default_flag=0,ch='S',speed=3,attack=null},
			new Status(){name="hogoblin",exp=0,level=6,armor=5,hp=20,maxhp=20,str=10,maxstr=10,default_flag=0,ch='H',speed=1,attack=null},
			new Status(){name="goblin",exp=0,level=3,armor=3,hp=30,maxhp=30,str=15,maxstr=15,default_flag=0,ch='G',speed=1,attack=null},
			new Status(){name="toroll",exp=0,level=8,armor=13,hp=30,maxhp=30,str=25,maxstr=25,default_flag=0,ch='T',speed=1,attack=null},
			new Status(){name="mimic",exp=0,level=4,armor=10,hp=50,maxhp=50,str=3,maxstr=3,default_flag=0,ch='M',speed=1,attack=null},
			new Status(){name="yeti",exp=0,level=9,armor=10,hp=50,maxhp=50,str=3,maxstr=3,default_flag=0,ch='Y',speed=1,attack=(ref Actor actor)=>{
				if(random.Next(10)==1){
					Logger.Post("yeti iced you.");
					actor.addEffect(Flags.NoCommand,5);
				}
			}},
			new Status(){name="quagga",exp=0,level=6,armor=5,hp=20,maxhp=20,str=13,maxstr=13,default_flag=0,ch='Q',speed=1,attack=(ref Actor actor)=>{
				if(random.Next(10)==1){
					Logger.Post("yeyi iced you.");
					actor.addEffect(Flags.NoCommand,5);
				}
			}},
			new Status(){name="wizard",exp=0,level=6,armor=10,hp=10,maxhp=10,str=3,maxstr=3,default_flag=0,ch='W',speed=1,attack=(ref Actor actor)=>{
				if(random.Next(10)>5){
					Item item=null;
					if(actor.GetEquipment(Slot.Weapon)!=null){
						item=actor.GetEquipment(Slot.Weapon);
					}else if(actor.GetEquipment(Slot.Armor)!=null){
						item=actor.GetEquipment(Slot.Armor);
					}else if(actor.GetEquipment(Slot.Ring)!=null){
						item=actor.GetEquipment(Slot.Ring);
					}else{
						return;
					}
					item.set(Item.Flags.Cursed);
					Logger.Post("Your "+item.status.name+" is cursed.");
				}
			}},
		};
		Monster(Status _status,System.Drawing.Point pos):base(_status,pos){
			status.type=Type.Enemy;
		}
		public static Actor createMonster(System.Drawing.Point pos){
			Actor monster=new Monster(data[random.Next(data.Length)],pos);
			switch(monster.status.name){
				case "hogoblin":
				case "goblin":
					if(random.Next(10)>7){
						Item item=null;
						item=Weapon.Generate();
						monster.addPack(ref item);
						if(random.Next(10)<4)
							item.set(Item.Flags.Cursed);
						item.Use(ref monster);
					}
					if(random.Next(10)>7){
						Item item=null;
						item=Armor.Generate();
						monster.addPack(ref item);
						if(random.Next(10)<4)
							item.set(Item.Flags.Cursed);
						item.Use(ref monster);
					}
					break;
			}
			return monster;
		}
	}
}