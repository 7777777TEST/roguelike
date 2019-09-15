using System.Collections.Generic;
namespace Rogue{
	class Sequence{
		public enum Ctrl{
			Push,Pop,Clear
		}
		private class pair{
			public State state;
			public Ctrl ctrl;
		}
		private static List<State> States=new List<State>{};
		private static List<pair> ctrls=new List<pair>{};
		public static void Pop(){
			ctrls.Add(new pair(){ctrl=Ctrl.Pop,state=null});
		}
		public static void Push(State _state){
			ctrls.Add(new pair(){ctrl=Ctrl.Push,state=_state});
		}
		public static void Clear(){
			ctrls.Add(new pair(){ctrl=Ctrl.Clear,state=null});
		}
		public static void apply(){
			foreach (pair item in ctrls){
				switch(item.ctrl){
					case Ctrl.Clear:
						States.Clear();
						break;
					case Ctrl.Pop:
						States.RemoveAt(States.Count-1);
						break;
					case Ctrl.Push:
						States.Add(item.state);
						break;
				}
			}
			ctrls.Clear();
		}
		public static bool Empty(){
			return States.Count<=0;
		}
		public static void update(Command command){
			if(!Empty()){
				States[States.Count-1].update(command);
			}
			apply();
		}
		public static void draw(){
			foreach(State s in States){
				s.draw();
			}
		}
	}
}