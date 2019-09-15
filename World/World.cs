using System.Collections.Generic;
namespace Rogue{
	class World{
		private List<Level> Levels=new List<Level>();
		public Level current{
			get{
				return Levels[depth];
			}
		}
		Fov fov;
		public float fovStrength=1.0f;
		static System.Random random=new System.Random(System.Environment.TickCount);
		public Actor player=new Actor(new Actor.Status(){hp=100,maxhp=100,level=1,str=5,maxstr=5,armor=0,default_flag=0,name="player",ch='@',speed=1,type=Actor.Type.Hero,exp=0,attack=null},new System.Drawing.Point(0,0));
		public int turn=0;
		int w=0,h=0;
		public World(int _w,int _h){
			w=_w;
			h=_w;
			Level level=new Level(MapGenerater.MapGen(_w,_h,100));
			level.world=this;
			Levels.Add(level);
			//Add items
			int items=random.Next(3,10);
			for(int i=0;i<items;i++){
				while(true){
					int x=random.Next(current.width),y=random.Next(current.height);
					if(current.isMove(x,y)){
						Item item=Item.createItem();
						item.pos=new System.Drawing.Point(x,y);
						if(random.Next(100)<30){
							item.set(Item.Flags.Cursed);
						}else if(random.Next(100)<30){
							item.set(Item.Flags.Blessed);
						}
						current.attach(item);
						break;
					}
				}
			}
			fov=new Fov(_w,_h);
			fov.load(level);
			fov.setIntensity(10.0f);
			Item weapon=new Weapon(0);
			weapon.Count=10+random.Next(10);
			weapon.set((1<<(int)Item.Flags.Many)|(1<<(int)Item.Flags.Blessed)|(1<<(int)Item.Flags.Identified));
			Item stick=new Stick(1);
			weapon.set((1<<(int)Item.Flags.Identified)|(1<<(int)Item.Flags.Blessed));
			Item potion=new Potion(0);
			potion.set((1<<(int)Item.Flags.Many)|(1<<(int)Item.Flags.Blessed)|(1<<(int)Item.Flags.Identified));
			potion.Count=10+random.Next(10);
			weapon.set(Item.Flags.Many);
			player.addPack(ref weapon);
			player.addPack(ref stick);
			player.addPack(ref potion);
			for(int x=current.width-1;x>=0;x--){
				for(int y=current.height-1;y>=0;y--){
					if(current.isMove(x,y)){
						player.pos.X=x;
						player.pos.Y=y;
						level.attach(player);
						return;
					}
				}
			}
		}
		public void endPlayerTurn(){
			if(player.Dead)return;
			if(turn%5==3)
				player.status.hp++;
			if(player.getEffectTurn(Actor.Flags.NoCommand)==0){
				int tmp=player.Food;
				player.Food=tmp-1;
			}
			enemiesThink();
			updateEnemies();
			player.updateEffect();
			current.removeDeadActors();
			if(player.Dead){
				//GameOver
				Sequence.Clear();
				Sequence.Push(new GameOver(player.dead_reason));
				return;
			}
			fov.compute(player.pos,fovStrength);
			int timeout=100;
			System.Console.Title="HP:"+player.Hp.ToString()+" Str:"+player.Str+" Level:"+player.status.level+" "+player.HungryStatus;
			if(random.Next(0,70)<10&&current.actors.Count<20){
				System.Drawing.Point pos=new System.Drawing.Point(0,0);
				while(!(current.isMove(pos)&&current.atActor(pos)==null&&fov.mask[pos.X,pos.Y]==0.0f&&timeout>0)){
					pos.X=random.Next(0,current.width-1);
					pos.Y=random.Next(0,current.height-1);
					timeout--;
				}
				//create actor
				Actor monster=Monster.createMonster(pos);
				while(monster.status.level-depth>3){
					monster=Monster.createMonster(pos);
				}
				current.attach(monster);
			}
		}
		public void draw(){
			current.draw();
			fov.compute(player.pos,fovStrength);
			for(int x=0;x<current.width;x++){
				for(int y=0;y<current.height;y++){
					if(fov.mask[x,y]==0.0f){
						Window.setChar(x,y,' ');
					}
				}
			}
		}
		public void updateEnemies(){
			for(int i=0;i<current.actors.Count;i++){
				if(player.Dead)break;
				if(current.actors[i].status.type!=Actor.Type.Hero&&!current.actors[i].Dead){
					Actor actor=current.actors[i];
					current.Move(ref actor,actor.nextPos);
					actor.updateEffect();
				}
			}
		}
		public void enemiesThink(){
			if(player.Dead)return;
			AI.goal=player.pos;
			AI.level=current;
			for(int i=0;i<current.actors.Count;i++){
				if(current.actors[i].status.type!=Actor.Type.Hero&&!current.actors[i].Dead){
					Actor actor=current.actors[i];
					if(current.actors[i].get(1<<(int)Actor.Flags.Stun)){
						actor.nextPos=new System.Drawing.Point(actor.pos.X+random.Next(-1,1),actor.pos.Y+random.Next(-1,1));
					}else{
						actor.nextPos=AI.calc(actor.pos,actor.status.speed);
					}
				}
			}
		}
		public void descend(){
			current.detach(player);
			if(depth+1>=Levels.Count){
				Level level=new Level(MapGenerater.MapGen(current.width,current.height,100));
				Levels.Add(level);
				level.world=this;
				//Add items
				for(int i=0;i<15;i++){
					while(true){
						int x=random.Next(0,level.width),y=random.Next(0,level.height);
						if(current.isMove(x,y)){
							Item item=Item.createItem();
							if(random.Next(100)<30){
								item.set(Item.Flags.Cursed);
							}else if(random.Next(100)<30){
								item.set(Item.Flags.Blessed);
							}
							item.pos=new System.Drawing.Point(x,y);
							current.attach(item);
							break;
						}
					}
				}
				while(true){
					if(random.Next(100)>25)break;
					while(true){
						int x=random.Next(0,level.width),y=random.Next(0,level.height);
						if(level.isMove(x,y)&&level.atTrap(new System.Drawing.Point(x,y))==null){
							Trap trap=Trap.Generate();
							trap.pos=new System.Drawing.Point(x,y);
							level.attach(trap);
							break;
						}
					}
				}
			}
			depth++;
			fov.load(current);
			for(int x=current.width-1;x>=0;x--){
				for(int y=current.height-1;y>=0;y--){
					if(current.isMove(x,y)){
						player.pos.X=x;
						player.pos.Y=y;
						current.attach(player);
						return;
					}
				}
			}
		}
		public void climb(){
			if(depth==0)return;
			depth--;
			for(int x=0;x<current.width;x++){
				for(int y=0;y<current.height;y++){
					if(current.isMove(x,y)){
						player.pos.X=x;
						player.pos.Y=y;
						current.attach(player);
						fov.load(current);
						return;
					}
				}
			}
		}
		public int depth=0;
	}
}