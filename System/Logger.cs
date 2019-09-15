using System.Collections.Generic;
namespace Rogue{
	class Logger{
		private static List<string> buf=new List<string>{};
		public static void Post(string message){
			buf.Insert(0,message);
			if(buf.Count>5){
				buf.RemoveAt(buf.Count-1);
			}
			draw();
		}
		public static void draw(){
			int y=Window.h-2;
			foreach(string s in buf){
				Window.setString(0,y,s);
				y--;
			}
		}
	}
}