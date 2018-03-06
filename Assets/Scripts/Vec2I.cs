using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Vec2I {

	public int x, y;

	public Vec2I(int x, int y){
		this.x = x;
		this.y = y;
	}

	public Vector2 ToVec2(){
		return new Vector2(x, y);
	}

	public Vector3 ToVec3(){
		return new Vector3(x, 0, y);
	}

}
