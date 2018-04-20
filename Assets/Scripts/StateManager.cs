using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour {

	public enum GameState{ Build, Wave}
	private GameState gameState;

	public void SetGameState(GameState _state){
		gameState = _state;
	}

	public GameState GetGameState(){
		return gameState;
	}

}
