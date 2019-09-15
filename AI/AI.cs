using System.Collections.Generic;
using System.Drawing;
namespace Rogue{
	class AI{
		public static Level level;
		static Node[,] nodes=new Node[0,0];
		static bool[,] actors=new bool[0,0];
		public static Point goal;
		static void Reset(){
			nodes=new Node[level.width,level.height];
			for(int x=0;x<level.width;x++){
				for(int y=0;y<level.height;y++){
					nodes[x,y]=new Node();
					nodes[x,y].C=0;
					nodes[x,y].parent=new Point(-1,-1);
					nodes[x,y].type=type.none;
					nodes[x,y].H=System.Math.Max(System.Math.Abs(x-goal.X),System.Math.Abs(y-goal.Y));
				}
			}
			actors=new bool[level.width,level.height];
			for(int x=0;x<level.width;x++){
				for(int y=0;y<level.height;y++){
					actors[x,y]=false;
				}
			}
			foreach(Actor a in level.actors){
				actors[a.pos.X,a.pos.Y]=true;
			}
			actors[goal.X,goal.Y]=false;
		}
		static bool Open(Point best){
			nodes[best.X,best.Y].type=type.closed;
			for(int dx=-1;dx<2;dx++){
				for(int dy=-1;dy<2;dy++){
					int x=best.X+dx,y=best.Y+dy;
					if(!level.isMove(x,y))continue;
					if(nodes[x,y].type!=type.none)continue;
					if(actors[x,y])continue;
					nodes[x,y].parent=best;
					if(x==goal.X&&y==goal.Y)return true;
					nodes[x,y].type=type.open;
					nodes[x,y].C=nodes[best.X,best.Y].C+1;
				}
			}
			return false;
		}
		static Point Best(){
			int min=System.Int32.MaxValue;
			Point best=new Point(-1,-1);
			for(int x=0;x<level.width;x++){
				for(int y=0;y<level.height;y++){
					if(nodes[x,y].type==type.open){
						if(min>nodes[x,y].S){
							min=nodes[x,y].S;
							best=new Point(x,y);
						}else if(min==nodes[x,y].S&&nodes[best.X,best.Y].C>nodes[x,y].C){
							best=new Point(x,y);
						}
					}
				}
			}
			return best;
		}
		public static Point calc(Point pos,int range){
			if(System.Math.Max(System.Math.Abs(pos.X-goal.X),System.Math.Abs(pos.Y-goal.Y))>20){
				return pos;
			}
			Reset();
			Point current=pos;
			nodes[pos.X,pos.Y].type=type.open;
			while(!Open(current)){
				current=Best();
				if(current.X==-1)return pos;//failed
			}
			List<Point> list=new List<Point>{};
			current=goal;
			while(true){
				list.Add(current);
				if(current==pos){
					break;
				}
				current=nodes[current.X,current.Y].parent;
			}
			list.Reverse();
			if(range==1||list.Count==2)
			return list[1];
			if(range>=list.Count)
				range=list.Count-1;
			for(int i=1;i<range;i++){
				if(level.atTrap(list[i])!=null)
					return list[i];
			}
			System.Console.WriteLine(range-1);
			return list[range-1];
		}
		enum type{
			none,
			open,
			closed
		}
		class Node{
			public int C,H;
			public int S{
				get{return C+H;}
			}
			public Point parent;
			public type type=type.none;
		}
	}
}