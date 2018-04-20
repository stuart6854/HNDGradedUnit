using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : DefensiveStructure {

	public float activationCooldown;

	public float cooldownTimer;

	private Animator anim;

	private void Start(){
		anim = GetComponent<Animator> ();
	}

	private void Update(){
		if (cooldownTimer > 0)
			cooldownTimer -= Time.deltaTime;
	}

	private void OnTriggerEnter(Collider _col){
		if (_col.CompareTag ("Monster") && cooldownTimer <= 0)
			TriggerTrap ();
	}

	private void TriggerTrap(){
		cooldownTimer = activationCooldown;
		anim.SetTrigger ("trapTrigger");

		foreach(GameObject obj in validTargets){
			Monster m = obj.GetComponent<Monster> ();
			if (m != null)
				m.OnHit (m.health);
		}
	}

}
