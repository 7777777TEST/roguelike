using System;
using System.Drawing;
namespace Rogue{
	class Window{
		private static char[][] buf;
		public static int w,h;
		static bool changed=false;
		public static void Init(int _w,int _h){
			w=_w;
			h=_h;
			buf=new char[h][];
			for(int i=0;i<h;i++){
				buf[i]=new char[w];
				for(int j=0;j<w;j++){
					buf[i][j]=' ';
				}
			}
		}
		public static void clear(){
			changed=true;
			for(int i=0;i<h;i++)
				for(int j=0;j<w;j++)
					buf[i][j]=' ';
		}
		public new static string ToString(){
			string s="";
			foreach(char[] carray in buf){
				s+=(new string(carray)+"\n");
			}
			return s;
		}
		public static void setChar(int _x,int _y,char c){
			changed=true;
			if(_x<0||_x>=w||_y<0||_y>=h)return;
			buf[_y][_x]=c;
		}
		public static void setChar(Point pos,char c){
			setChar(pos.X,pos.Y,c);
		}
		public static void setString(int _x,int _y,string s){
			int dx=0,dy=0;
			foreach(char c in s){
				if(c=='\n'){
					dx=0;
					dy++;
					continue;
				}
				setChar(_x+dx,_y+dy,c);
				dx++;
			}
		}
		public static void Rect(int l,int t,int _w,int _h){
			int r=l+_w,b=t+_h;
			for(int x=l;x<r-1;x++)for(int y=t;y<b-1;y++)setChar(x,y,' ');
			for(int x=l+1;x<r-1;x++){setChar(x,t,'-');setChar(x,b-1,'-');}
			for(int y=t+1;y<b-1;y++){setChar(l,y,'|');setChar(r-1,y,'|');}
			setChar(l,t,'+');setChar(r-1,t,'+');setChar(l,b-1,'+');setChar(r-1,b-1,'+');
		}
		public static void Render(){
			if(!changed)return;
			Logger.draw();
			Console.SetCursorPosition(0,0);
			Console.Write(ToString());
			changed=false;
		}
	}
}