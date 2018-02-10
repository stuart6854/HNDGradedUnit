using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshHandler : MonoBehaviour {

	public NavMeshSurface dungeonNavSurface;

	private bool navMeshDirty;

	private void Start () {
		BuildNavMesh();
	}

	private void Update () {
		if(Input.GetKeyDown(KeyCode.F2))
			BuildNavMesh();

		if(navMeshDirty){
			dungeonNavSurface.BuildNavMesh();
			navMeshDirty = false;
		}
	}

	public void BuildNavMesh(){
		navMeshDirty = true;
	}

}
