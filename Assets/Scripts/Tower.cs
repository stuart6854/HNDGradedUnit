using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : DefensiveStructure {

	private void TARGET_UPDATE_TIME = 1f/10f;

	public Transform yaw, pitch;

	private GameObject target;

	private float timer;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		timer += Time.deltaTime;

		if (timer >= TARGET_UPDATE_TIME) {
			print ("T");
			timer = 0;
			PickTarget ();
		}

	}

	private void PickTarget(){
		float minDist = float.MaxValue;
		GameObject closestObj = null;
		foreach(GameObject obj in validTargets){
			Vector3 dist = Vector3.Distance (transform.position, obj.transform.position);
			if (dist < minDist)
				closestObj = obj;
		}
		target = closestObj;
	}

}
