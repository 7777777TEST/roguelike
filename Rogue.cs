using System;
namespace Rogue
{
	class Rogue{
		public static void Main(){
			Console.Clear();
			Window.Init(60, 36);
			System.Diagnostics.Stopwatch sw=new System.Diagnostics.Stopwatch();
			State.world = new World(40, 30);
			Sequence.Push(new Story());
			Sequence.update(Command.None);
			Window.clear();
			Sequence.draw();
			draw();
			sw.Start();
			while(true){
				draw();
				if(Console.KeyAvailable){
					input();
				}
				if(sw.Elapsed.Milliseconds>=0.2){
					Sequence.update(Command.None);
					sw.Reset();
					sw.Start();
				}
			}
		}
		static void draw(){
			Window.Render();
		}
		static void input(){	
			ConsoleKeyInfo c=Console.ReadKey(true);
			Command command=Command.None;
			switch(c.Key){
				case ConsoleKey.Home:
				case ConsoleKey.NumPad7:
					command=Command.D7;
					break;
				case ConsoleKey.UpArrow:
				case ConsoleKey.NumPad8:
					command=Command.D8;
					break;
				case ConsoleKey.PageUp:
				case ConsoleKey.NumPad9:
					command=Command.D9;
					break;
				case ConsoleKey.LeftArrow:
				case ConsoleKey.NumPad4:
					command=Command.D4;
					break;
				case ConsoleKey.Enter:
				case ConsoleKey.NumPad5:
					command=Command.D5;
					break;
				case ConsoleKey.RightArrow:
				case ConsoleKey.NumPad6:
					command=Command.D6;
					break;
				case ConsoleKey.End:
				case ConsoleKey.NumPad1:
					command=Command.D1;
					break;
				case ConsoleKey.DownArrow:
				case ConsoleKey.NumPad2:
					command=Command.D2;
					break;
				case ConsoleKey.PageDown:
				case ConsoleKey.NumPad3:
					command=Command.D3;
					break;
				case ConsoleKey.D0:
				case ConsoleKey.NumPad0:
				case ConsoleKey.Escape:
					command=Command.D0;
					break;
			}
			Sequence.update(command);
			Window.clear();
			Sequence.draw();
		}
	}
}