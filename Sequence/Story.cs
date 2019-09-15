namespace Rogue{
	class Story:State{
		public override void update(Command command){
			if(command!=Command.D5)return;
			Sequence.Pop();
      Window.clear();
			Sequence.Push(new Game());
		}
		public override void draw(){
			Window.setString(0,0,"It is written in the Book of Thoth:\n  After the Creation, the cruel god Moloch rebelled\n  against the authority of Marduk the Creator.\n  Moloch stole from Marduk the most powerful of all\n  the artifacts of the gods, the Amulet of Yendor,\n  and he hid it in the dark cavities of Gehennom, the\n  Under World, where he now lurks, and bides his time.\n\nYour god Thoth seeks to possess the Amulet, and with it\nto gain deserved ascendance over the other gods.\n\nYou, a newly trained Evoker, have been heralded\nfrom birth as the instrument of Thoth.  You are destined\nto recover the Amulet for your deity, or die in the\nattempt.  Your hour of destiny has come.  For the sake\nof us all:  Go bravely with Thoth!\n\n--More--");
		}
	}
}