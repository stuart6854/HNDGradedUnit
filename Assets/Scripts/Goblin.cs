using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goblin : Monster {

	public override int getMonstersInWave(int wave) {
		return wave * 1;
	}

}
