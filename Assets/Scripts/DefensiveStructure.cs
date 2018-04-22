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
		if(m != null && !m.IsDead()){
			validTargets.Add (m.gameObject);
		}
	}

	private void OnCollisionStay(Collision _col){
		Monster m = _col.transform.GetComponent<Monster>();
		if(m != null){
			if(m.IsDead() && validTargets.Contains(_col.gameObject))
				validTargets.Remove(_col.gameObject);
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
