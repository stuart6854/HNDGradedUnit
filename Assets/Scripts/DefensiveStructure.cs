using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefensiveStructure : MonoBehaviour {

	public List<GameObject> validTargets;

	public int manaCost;

	private void Start(){
		validTargets = new List<GameObject> ();
	}

	private void OnCollisionEnter(Collision _col){
		Monster m = _col.gameObject.GetComponent<Monster> ();
		if(m != null){
			validTargets.Add (m.gameObject);
		}
	}

	private void OnCollisionExit(Collision _col){
		Monster m = _col.gameObject.GetComponent<Monster> ();
		if(m != null){
			if(validTargets.Contains(m.gameObject))
				validTargets.Remove (m.gameObject);
		}
	}

}
