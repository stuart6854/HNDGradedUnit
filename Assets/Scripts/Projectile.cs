using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Security.Cryptography;

public class Projectile : MonoBehaviour {

	public float speed = 1.0f;

	private bool isFlying = true;

	private float cleanupTimer = 5.0f;

	void Update () {
		if(isFlying) {
			transform.position += transform.forward * speed * Time.deltaTime;
		}else{
			cleanupTimer -= Time.deltaTime;
			if(cleanupTimer <= 0.0f)
				Destroy(gameObject);
		}
	}

	private void OnCollisionEnter(Collision _col){
		if(!_col.transform.CompareTag("Monster") && !_col.transform.CompareTag("DungeonAsset"))
			return;

		isFlying = false;
		transform.SetParent(_col.gameObject.transform);

		Monster m = _col.transform.GetComponent<Monster>();
		if(m != null)
			m.OnHit(m.health);

		Destroy (this);
	}


}
