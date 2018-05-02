using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveManager : MonoBehaviour {

	private static int Waves_To_Win = 10;

	public enum WaveState{ Intermission, Wave };

	public StateManager stateManager;
	public UIManager uiManager;
	public Dungeon dungeon;
	public BuildController buildController;

	public Text waveCounterTxt;
	public Text gameLoseWaveTxt;

	public Monster[] monsters;

	private int waveCounter;
	private WaveState waveState;

	private Queue<WaveMonster> wave;
	private int monstersRemaining;

	private void Start(){
		waveCounter = 1;
		wave = new Queue<WaveMonster>();

		uiManager.ChangeUIState (0);
	}

	private void Update(){
		if(Input.GetKeyDown(KeyCode.F5) && waveState == WaveState.Intermission)
			StartWave();

		if (waveCounterTxt != null)
			waveCounterTxt.text = "Wave " + waveCounter;

		if(gameLoseWaveTxt != null)
			gameLoseWaveTxt.text = "You reached wave " + waveCounter + "!";

	}

	public void StartWave(){
		if (waveState != WaveState.Intermission)
			return;

		if(!dungeon.doesPathExist()){
			Debug.Log("WaveManager -> No valid path through dungeon. Cannot start Wave!");
			return;
		}

		uiManager.ChangeUIState (1);

		stateManager.SetGameState (StateManager.GameState.Wave);
		buildController.SetBuildMode(BuildController.BuildMode.None);

		GenerateWave();
		StartCoroutine(SpawnWave());
		waveState = WaveState.Wave;
	}

	private void EndWave(){
		waveState = WaveState.Intermission;
		waveCounter++;

		uiManager.ChangeUIState (0);
		stateManager.SetGameState (StateManager.GameState.Build);

		if(waveCounter - 1 == Waves_To_Win){
			stateManager.SetGameState(StateManager.GameState.Paused);
			uiManager.ChangeUIState (4);
		}
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
		stateManager.SetGameState(StateManager.GameState.GameOver);
		uiManager.ChangeUIState (3);
	}

	public void MonsterDied(Monster m){
		monstersRemaining--;
		Dungeon.mana += m.manaFromDeath;

		if(waveState == WaveState.Wave && monstersRemaining <= 0)
			EndWave();
	}

	public void OnGUI(){
//		GUI.Label(new Rect(0, 75, 250, 30), "WaveState: " + waveState.ToString());
//		GUI.Label(new Rect(0, 90, 250, 30), "Wave: " + waveCounter);
//		GUI.Label(new Rect(0, 105, 250, 30), "Monsters Remaining: " + monstersRemaining);
//		GUI.Label(new Rect(0, 120, 250, 30), "Dungeon Start: " + monstersRemaining);
//		GUI.Label(new Rect(0, 135, 250, 30), "DungeonEnd: " + monstersRemaining);
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
