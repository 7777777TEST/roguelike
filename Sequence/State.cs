namespace Rogue
{
	abstract class State{
		public abstract void update(Command command);
		public abstract void draw();
		public static World world;
	}
}