using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.IO;

public abstract class Monster : MonoBehaviour {

	public float moveSpeed = 1;
	public float health = 1;
	public int manaFromDeath;

	private bool isDead;

	[HideInInspector]
	public WaveManager waveManager;

	private NavMeshAgent navAgent;
	private NavMeshPath path;

	private void Awake(){
		navAgent = GetComponent<NavMeshAgent>();
		navAgent.speed = moveSpeed;
	}

	private void Start(){
		if (waveManager != null) {
			path = waveManager.dungeon.getDungeonPath ();
			navAgent.SetPath (path);
		}
	}

	private void Update(){
		if(health <= 0 && !isDead){
			isDead = true;

			FindObjectOfType<WaveManager>().MonsterReachEnd(this);
			navAgent.enabled = false;
		}
	}

	public void OnHit(float _dmg){
		health -= _dmg;
	}

	public abstract int getMonstersInWave(int wave);

	public bool IsDead(){
		return isDead;
	}

	public float GetRemainingDistance(){
		float distance = 0.0f;
		Vector3[] corners = navAgent.path.corners;
		for (int c = 0; c < corners.Length - 1; ++c) {
			distance += Mathf.Abs((corners[c] - corners[c + 1]).magnitude);
		}
		return distance;
	}

	private void OnDrawGizmosSelected(){
		if (navAgent == null || navAgent.destination == null)
			return;
		
		Gizmos.color = Color.magenta;
		Gizmos.DrawWireSphere(navAgent.destination, 0.25f);

		if(navAgent.path != null){
			Gizmos.color = Color.cyan;
			for(int i = 1; i < navAgent.path.corners.Length; i++){
				Gizmos.DrawLine(navAgent.path.corners[i - 1], navAgent.path.corners[i]);
			}
		}

	}



}
