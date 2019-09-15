using System.Drawing;
namespace Rogue{
	abstract class Object{
		public Point pos;
		protected static System.Random random=new System.Random(System.Environment.TickCount);
		private int flags;
		protected char ch;
		public void set(int f){
			flags|=f;
		}
		public void rm(int f){
			flags&=~f;
		}
		public bool get(int f){
			return (flags&f)!=0;
		}
		public int Flag{
			get{
				return flags;
			}
			set{
				flags=value;
			}
		}
		public void move(int dx,int dy){
			pos.X+=dx;
			pos.Y+=dy;
		}
		public void move(Point dir){
			move(dir.X,dir.Y);
		}
		public void draw(){
			Window.setChar(pos,ch);
		}
	}
}