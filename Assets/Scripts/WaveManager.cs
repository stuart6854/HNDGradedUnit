using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour {

	public enum WaveState{ Intermission, Wave };

	public StateManager stateManager;
	public UIManager uiManager;
	public Dungeon dungeon;
	public BuildController buildController;

	public Monster[] monsters;

	private int waveCounter;
	private WaveState waveState;

	private Queue<WaveMonster> wave;
	private int monstersRemaining;

	private void Start(){
		waveCounter = 1;
		wave = new Queue<WaveMonster>();
	}

	private void Update(){
		if(Input.GetKeyDown(KeyCode.F5) && waveState == WaveState.Intermission)
			StartWave();

		if(waveState == WaveState.Wave && monstersRemaining <= 0)
			EndWave();
	}

	public void StartWave(){
		if (waveState != WaveState.Intermission)
			return;

		if(!dungeon.doesPathExist()){
			Debug.Log("WaveManager -> No valid path through dungeon. Cannot start Wave!");
			return;
		}

		uiManager.waveUI.SetActive (true);
		uiManager.buildUI.SetActive (false);

		stateManager.SetGameState (StateManager.GameState.Wave);
		buildController.SetBuildMode(BuildController.BuildMode.None);

		GenerateWave();
		StartCoroutine(SpawnWave());
		waveState = WaveState.Wave;
	}

	private void EndWave(){
		waveState = WaveState.Intermission;
		waveCounter++;

		uiManager.waveUI.SetActive (false);
		uiManager.buildUI.SetActive (true);

		stateManager.SetGameState (StateManager.GameState.Build);
	}

	private void GenerateWave(){
		wave.Clear();
		monstersRemaining = 0;
		foreach(Monster m in monsters){
			int amnt = m.getMonstersInWave(waveCounter);
			if(amnt > 0){
				monstersRemaining += amnt;
				wave.Enqueue(new WaveMonster(m, amnt));
			}
		}

	}

	private IEnumerator SpawnWave(){
		foreach(WaveMonster wm in wave){
			for(int i = 0; i < wm.amnt; i++){
				Vector3 spawnPos = dungeon.getDungeonStartPos();

				GameObject monster = (GameObject)Instantiate(wm.m.gameObject, spawnPos, Quaternion.identity);
				monster.transform.SetParent(this.transform);

				monster.GetComponent<Monster>().waveManager = this;

				yield return new WaitForSeconds(1.0f);
			}
		}
	}

	public void MonsterReachEnd(Monster m){
		monstersRemaining--;

		//Remove player health or something
	}

	public void OnGUI(){
		GUI.Label(new Rect(0, 75, 250, 30), "WaveState: " + waveState.ToString());
		GUI.Label(new Rect(0, 90, 250, 30), "Wave: " + waveCounter);
		GUI.Label(new Rect(0, 105, 250, 30), "Monsters Remaining: " + monstersRemaining);
		GUI.Label(new Rect(0, 120, 250, 30), "Dungeon Start: " + monstersRemaining);
		GUI.Label(new Rect(0, 135, 250, 30), "DungeonEnd: " + monstersRemaining);
	}

	private class WaveMonster{
		public readonly Monster m;
		public readonly int amnt;

		public WaveMonster(Monster m, int amnt){
			this.m = m;
			this.amnt = amnt;
		}
	}

}
