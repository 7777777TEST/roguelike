using System.Collections.Generic;
namespace Rogue{
	class MapGenerater{
		struct Room{
			public int x,y,w,h;
		}
		enum room_id{
			room='.',
			way='.',
			outside_wall=' ',
			inside_wall='#',
			upstair='<',
			downstair='>'
		}
		private static room_id[,] buf=new room_id[0,0];
		private static System.Random random=new System.Random();
		private static List<Room> room_rect=new List<Room>{},branch_point=new List<Room>{};
		public static string MapGen(int w,int h,int max){
			random=new System.Random(System.Environment.TickCount);
			buf=new room_id[w,h];
			room_rect.Clear();
			branch_point.Clear();
			for(int x=0;x<w;x++){
				for(int y=0;y<h;y++){
					buf[x,y]=room_id.outside_wall;
				}
			}
			Dir dir=(Dir)System.Enum.GetValues(typeof(Dir)).GetValue(random.Next(0,3));
			if(makeRoom(w/2,h/2,dir))
				for(int i=0;i<max;i++){
					if(!createNext())break;
				}
			putDownStair();
			putUpStair();
			string str="";
			for(int y=0;y<h;y++){
				for(int x=0;x<w;x++){
					char[] a={(char)buf[x,y]};
					str+=new string(a);
				}
				str+="\n";
			}
			return str;
		}
		private static int width(){
			return buf.GetLength(0);
		}
		private static int height(){
			return buf.GetLength(1);
		}
		private static void putDownStair(){
			for(int x=width()-1;x>=0;x--)
				for(int y=height()-1;y>=0;y--)
					if(buf[x,y]==room_id.room||buf[x,y]==room_id.way){
						buf[x,y]=room_id.downstair;
						return;
					}
		}
		private static void putUpStair(){
			for(int x=0;x<width();x++)
				for(int y=0;y<height();y++)
					if(buf[x,y]==room_id.room||buf[x,y]==room_id.way){
						buf[x,y]=room_id.upstair;
						return;
					}
		}
		private static room_id getTile(int x,int y){
			if(x<0||x>=width()||y<0||y>=height())return room_id.outside_wall;
			return buf[x,y];
		}
		private static void setTile(int x,int y,room_id c){
			buf[x,y]=c;
		}
		private static bool createNext(){
			for(int i=0,r=0;i<0xffff;i++){
				if(branch_point.Count<=0)break;
				r=random.Next(branch_point.Count);
				int x=random.Next(branch_point[r].x,branch_point[r].x+branch_point[r].w-1);
				int y=random.Next(branch_point[r].y,branch_point[r].y+branch_point[r].h-1);
				if(createNext(x,y,Dir.Right)){
					branch_point.RemoveAt(r);
					return true;
				}
				if(createNext(x,y,Dir.Down)){
					branch_point.RemoveAt(r);
					return true;
				}
				if(createNext(x,y,Dir.Left)){
					branch_point.RemoveAt(r);
					return true;
				}
				if(createNext(x,y,Dir.Up)){
					branch_point.RemoveAt(r);
					return true;
				}
			}
			return false;
		}
		private static bool createNext(int x,int y,Dir dir){
			if(random.Next(3)==1){
				if(!makeRoom(x,y,dir))return false;
			}else{
				if(!makeWay(x,y,dir))return false;
			}
			setTile(x,y,room_id.room);
			return true;
		}
		static bool makeRoom(int x,int y,Dir dir){return makeRoom(x,y,dir,false);}
		static bool makeRoom(int x,int y,Dir dir,bool firstRoom){
			const int minRoomSize=3,maxRoomSize=10;
			Room room=new Room(){w=random.Next(minRoomSize,maxRoomSize*2),h=random.Next(minRoomSize,maxRoomSize)};
			switch(dir){
				case Dir.Up:
					room.x=x-room.w/2;
					room.y=y-room.h;
					break;
				case Dir.Down:
					room.x=x-room.w/2;
					room.y=y+1;
					break;
				case Dir.Left:
					room.x=x-room.w;
					room.y=y-room.h/2;
					break;
				case Dir.Right:
					room.x=x+1;
					room.y=y-room.h/2;
					break;
			}
			if(placeRect(room,room_id.room)){
				room_rect.Add(room);
				if(dir!=Dir.Left||firstRoom)
					branch_point.Add(new Room{ x=room.x-1, y=room.y , w=1, h=room.h });
				if(dir!=Dir.Down||firstRoom)
					branch_point.Add(new Room{ x=room.x, y=room.y-1, w=room.w, h=1 });
				if(dir!=Dir.Up||firstRoom)
					branch_point.Add(new Room{ x=room.x, y=room.y+room.h, w=room.w, h=1 });
				if(dir!=Dir.Right||firstRoom)
					branch_point.Add(new Room{ x=room.x+room.w, y=room.y, w=1, h=room.h });
				return true;
			}
			return false;
		}
		static bool makeWay(int x,int y,Dir dir){
			int minLength=3,maxLength=10;
			Room way=new Room(){x=x,y=y};
			if(random.Next(2)==1){
				way.w=random.Next(minLength,maxLength);
				way.h=1;
				switch(dir){
					case Dir.Up:
						way.y=y-1;
						if(random.Next(2)==1)way.x=x-way.w+1;
						break;
					case Dir.Down:
						way.y=y+1;
						if(random.Next(2)==1)way.x=x-way.w+1;
						break;
					case Dir.Left:
						way.x=x-way.w;
						break;
					case Dir.Right:
						way.x=x+1;
						break;
				}
			}else{
				way.w=1;
				way.h=random.Next(minLength,maxLength);
				switch(dir){
					case Dir.Up:
						way.y=y-way.h;
						break;
					case Dir.Down:
						way.y=y+1;
						break;
					case Dir.Left:
						way.x=x-1;
						if(random.Next(2)!=1)way.y=y-way.h+1;
						break;
					case Dir.Right:
						way.x=x+1;
						if(random.Next(2)==1)way.y=y-way.h+1;
						break;
				}
			}
			if(!placeRect(way,room_id.way))return false;
			if(dir!=Dir.Up&&way.w!=1)
				branch_point.Add(new Room(){x=way.x,y=way.y+way.h,w=way.w,h=1});
			if(dir!=Dir.Left&&way.h!=1)
				branch_point.Add(new Room(){x=way.x-1,y=way.y,w=1,h=way.h});
			if(dir!=Dir.Right&&way.h!=1)
				branch_point.Add(new Room(){x=way.x+way.w,y=way.y,w=1,h=way.h});
			if(dir!=Dir.Down&&way.w!=1)
				branch_point.Add(new Room(){x=way.x,y=way.y-1,w=way.w,h=1});
			return true;
		}
		static bool placeRect(Room rect,room_id tile_){
			if(rect.x < 1 || rect.y < 1 || rect.x+rect.w>width()-1 || rect.y+rect.h>height()-1)
				return false;
			for (int y = rect.y; y < rect.y + rect.h; ++y)
				for (int x = rect.x; x < rect.x + rect.w; ++x)
					if (getTile(x, y) != room_id.outside_wall) return false;
			for (int y = rect.y - 1; y < rect.y + rect.h + 1; ++y)
				for (int x = rect.x - 1; x < rect.x + rect.w + 1; ++x) {
					if (x == rect.x - 1 || y == rect.y - 1 || x == rect.x + rect.w || y == rect.y + rect.h)
						setTile(x, y, room_id.inside_wall);
					else setTile(x, y, tile_);
				}
			return true;
		}
		private MapGenerater(){}
	}
}