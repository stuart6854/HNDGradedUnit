using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEditor;

[RequireComponent(typeof(SphereCollider))]
public class Tower : DefensiveStructure {

	private float TARGET_UPDATE_TIME = 1f/10f;

	public float range = 1f;
	public float rateOfFireSec = 1.0f; //Projectiles a second

	public Transform yaw, pitch;
	private float yawOffset = -90f;

	public Transform projecticleOrigin;
	public GameObject projectilePrefab;

	private GameObject target;

	private float targetUpdateTimer;

	private float fireCooldown;
	private float fireCooldownTimer;

	private AudioSource audioSrc;

	// Use this for initialization
	void Start () {
		audioSrc = GetComponent<AudioSource> ();

		fireCooldown = 1.0f / rateOfFireSec;
	}
	
	// Update is called once per frame
	void Update () {
		targetUpdateTimer += Time.deltaTime;
		if (targetUpdateTimer >= TARGET_UPDATE_TIME) {
			targetUpdateTimer = 0;
			PickTarget ();
		}

		if(fireCooldownTimer > 0.0f)
			fireCooldownTimer -= Time.deltaTime;

		if(target != null){
			//Rotation
			float targetYaw = Quaternion.LookRotation((target.transform.position - yaw.transform.position).normalized).eulerAngles.y;
			Vector3 newYawRot = new Vector3(0, targetYaw + yawOffset, 0);
			yaw.rotation = Quaternion.Euler(newYawRot);

			float targetPitch = Quaternion.LookRotation((target.transform.position - pitch.transform.position).normalized).eulerAngles.x;
			Vector3 newPitchRot = new Vector3(pitch.eulerAngles.x, pitch.eulerAngles.y, -targetPitch);
			pitch.rotation = Quaternion.Euler(newPitchRot);

			//Firing
			if(fireCooldownTimer <= 0.0f){
				Fire();
				fireCooldownTimer = fireCooldown;
			}

		}

	}

	private void Fire(){
		GameObject projectile = (GameObject)Instantiate(projectilePrefab, projecticleOrigin.position, Quaternion.LookRotation((target.transform.position - projecticleOrigin.position).normalized));
		audioSrc.Play ();
	}

	private void PickTarget(){
		float minDist = float.MaxValue;
		GameObject closestObj = null;
		foreach(GameObject obj in validTargets){
			if(!Physics.Raycast(projecticleOrigin.position, obj.transform.position - projecticleOrigin.position))
				continue;
			
			//float dist = Vector3.Distance (transform.position, obj.transform.position);
			float dist = obj.GetComponent<Monster>().GetRemainingDistance();
			if (dist < minDist){
				closestObj = obj;
				minDist = dist;
			}
		}
		target = closestObj;
	}

	private void OnValidate(){
		GetComponent<SphereCollider>().radius = range * 0.25f;
	}

	private void OnDrawGizmos(){
		//Handles.DrawWireDisc(transform.position, Vector3.up, range);
		if(target != null){
			Gizmos.color = Color.red;
			Gizmos.DrawSphere(target.transform.position, 0.1f);
			Gizmos.DrawLine(transform.position, target.transform.position);
		}
	}

}
