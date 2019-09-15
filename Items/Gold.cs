namespace Rogue{
	class Gold:Item{
		public Gold(int id){
			status.type=Type.Gold;
			Count=1;
			ch='$';
		}
		public override bool Use(ref Actor actor){
			return true;
		}
	}
}