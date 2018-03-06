using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.IO;

public abstract class Monster : MonoBehaviour {

	public float moveSpeed = 1;
	public float health = 1;
	public int manaFromDeath;

	[HideInInspector]
	public WaveManager waveManager;

	private NavMeshAgent navAgent;

	private void Awake(){
		navAgent = GetComponent<NavMeshAgent>();
		navAgent.speed = moveSpeed;
	}

	private void Start(){
		navAgent.SetDestination(waveManager.dungeon.getDungeonEndPos());
	}

	private void Update(){
		if(health <= 0){
			FindObjectOfType<WaveManager>().MonsterReachEnd(this);
			this.enabled = false;
		}

		if(!navAgent.hasPath){
			navAgent.SetDestination(waveManager.dungeon.getDungeonEndPos());
		}
	}

	public void OnHit(float _dmg){
		health -= _dmg;
	}

	public abstract int getMonstersInWave(int wave);

	private void OnDrawGizmosSelected(){
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
