public class Vector2{
	public int x;
	public int y;
	Vector2(int x, int y){
		this.x = x;
		this.y = y;
	}

	public boolean Equals(Vector2 v){
		return this.x == v.x && this.y == v.y;
	}
}