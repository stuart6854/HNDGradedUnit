using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DefensiveStructureManager : MonoBehaviour {

	public Dungeon dungeon;

	public DefensiveStructure[,] grid;

	private Action<int, int, DefensiveStructure> onGridStructureChangedCallback;

	public void Initialise(){
		grid = new DefensiveStructure[dungeon.dungeonWidth, dungeon.dungeonHeight];
	}

	private void Update(){

	}

	public void setGridStructure(int _x, int _y, DefensiveStructure _struct){
		if(!isValidIndex(_x, _y))
			return;
		grid[_x, _y] = _struct;
		onGridStructureChangedCallback(_x, _y, _struct);
	}

	public DefensiveStructure getGridStructure(int _x, int _y){
		if(!isValidIndex(_x, _y))
			return null;

		return grid[_x, _y];
	}

	public bool isValidIndex(int _x, int _y){
		if(_x < 0 || _x >= dungeon.dungeonWidth)
			return false;
		if(_y < 0 || _y >= dungeon.dungeonHeight)
			return false;

		return true;
	}

	public void RegisterOnGridStructureChangedCallback(Action<int, int, DefensiveStructure> _callback){
		onGridStructureChangedCallback -= _callback;
		onGridStructureChangedCallback += _callback;
	}

}
