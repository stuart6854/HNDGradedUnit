using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class StateManager : MonoBehaviour {

	public enum GameState{ Build, Wave, Paused, GameOver}
	private GameState gameState;
	private GameState lastState;

	public void SetGameState(GameState _state){
		lastState = gameState;
		gameState = _state;

		if(gameState == GameState.Paused || gameState == GameState.GameOver)
			Time.timeScale = 0;
		else
			Time.timeScale = 1;

	}

	public GameState GetGameState(){
		return gameState;
	}

	public GameState GetLastState(){
		return lastState;
	}

	public void TogglePause(){
		if(gameState != GameState.Paused)
			SetGameState(GameState.Paused);
		else
			SetGameState(GetLastState());
	}

}
