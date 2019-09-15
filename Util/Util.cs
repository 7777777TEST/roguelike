namespace Rogue{
	enum Dir{
		Up,Down,Left,Right,None
	}
	enum Command{
		D7,D8,D9,
		D4,D5,D6,
		D1,D2,D3,
		D0,None
	}
	delegate void Func(ref Actor actor);
}