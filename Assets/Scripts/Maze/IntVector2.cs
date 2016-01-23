[System.Serializable]
public struct IntVector2 {

	public int x, z;

	public IntVector2 (int x, int z) {
		this.x = x;
		this.z = z;
	}

	public static IntVector2 operator + (IntVector2 a, IntVector2 b) {
		a.x += b.x;
		a.z += b.z;
		return a;
	}

	public static bool operator == (IntVector2 a, IntVector2 b) {
		try {
			return (a.x == b.x && a.z == b.z) ? true : false;
		} catch {
			return false;
		}
	}

	public static bool operator != (IntVector2 a, IntVector2 b) {
		try {
			return (a.x != b.x || a.z != b.z) ? true : false;
		} catch {
			return false;
		}
	}

	public override bool Equals(object a) {
		throw new System.NotImplementedException ("Not Implemented yet!");
	}

	public override int GetHashCode() {
		throw new System.NotImplementedException ("Not Implemented yet!");
	}
}