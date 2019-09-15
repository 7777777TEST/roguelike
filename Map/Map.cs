using System.Drawing;
namespace Rogue{
	class Map{
		private char[,] map;
		public Map(string data){
			int x=0,y=0,w=0,h=0;
			foreach(char c in data){
				switch(c){
					case '\n':
						y++;
						w=System.Math.Max(x,w);
						x=0;
						break;
					case '#':
					case ' ':
					case '.':
					case '<':
					case '>':
						x++;
						break;
				}
			}
			w=System.Math.Max(w,x);
			h=y;
			map=new char[w,h];
			for(int i=0;i<map.GetLength(0);i++){
				for(int j=0;j<map.GetLength(1);j++){
					map[i,j]=' ';
				}
			}
			x=y=0;
			foreach(char c in data){
				switch(c){
					case '\n':
						++y;
						x=0;
						break;
					case '#':
					case ' ':
					case '.':
					case '<':
					case '>':
						map[x,y]=c;
						x++;
						break;
					default:
						break;
				}
			}
		}
		public char Get(int x,int y){
			if(x<0||x>=width||y<0||y>=height)return '?';
			return map[x,y];
		}
		public char Get(Point pos){
			return Get(pos.X,pos.Y);
		}
		public bool isMove(int x,int y){
			if(x<0||x>=width||y<0||y>=height)return false;
			if(map[x,y]=='#'||map[x,y]==' ')return false;
			return true;
		}
		public bool isMove(Point pos){
			return isMove(pos.X,pos.Y);
		}
		public char getChar(int x,int y){
			if(x<0||x>=width||y<0||y>=height)return' ';
			return map[x,y];
		}
		public int width{
			get{
				return map.GetLength(0);
			}
		}
		public int height{
			get{
				return map.GetLength(1);
			}
		}
		public void draw(){
			for(int i=0;i<width;i++){
				for(int j=0;j<height;j++){
					Window.setChar(i,j,Get(i,j));
				}
			}
		}
	}
}