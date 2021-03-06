﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Dungeon : MonoBehaviour {

	private static readonly int Starting_Mana = 100;

	public static int mana;

	public DefensiveStructureManager dsm;
	public DungeonGraphicsManager dungeonGraphicsManager;

	public int dungeonWidth, dungeonHeight;
	public Texture2D dungeonLayoutTexture; //Used to set the layout of the dungeon at start

	private int[,] grid;
	public Vec2I dungeonStart, dungeonEnd;

	private Vec2I hoveredIndex = new Vec2I();

	private Action<int, int, int> onGridTileChangedCallback;

	private NavMeshPath dungeonPath;

	public Text manaText;

	private void Start () {
		mana = Starting_Mana;

		dungeonPath = new NavMeshPath();

		if(dungeonLayoutTexture != null){
			dungeonWidth = dungeonLayoutTexture.width;
			dungeonHeight = dungeonLayoutTexture.height;
		}

		grid = new int[dungeonWidth, dungeonHeight];

		for(int y = 0; y < dungeonHeight; y++){
			for(int x = 0; x < dungeonWidth; x++){
				if(dungeonLayoutTexture != null) {
					setGridTile(x, y, (dungeonLayoutTexture.GetPixel(x, y) == Color.black) ? 1 : 0);
					if(dungeonLayoutTexture.GetPixel(x, y) == Color.green)
						dungeonStart = new Vec2I(x, y);

					if(dungeonLayoutTexture.GetPixel(x, y) == Color.red)
						dungeonEnd = new Vec2I(x, y);

					//TODO: Dungeon Start/End Graphics
				} else {
					setGridTile(x, y, 1);
				}
			}
		}

		dsm.Initialise();
	}

	private void Update () {
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if(Physics.Raycast(ray, out hit, 1000)){
			hoveredIndex.x = Mathf.RoundToInt(hit.point.x / dungeonGraphicsManager.tileSizeSqrd);
			hoveredIndex.y = Mathf.RoundToInt(hit.point.z / dungeonGraphicsManager.tileSizeSqrd);

			hoveredIndex.x = Mathf.Clamp(hoveredIndex.x, 0, dungeonWidth - 1);
			hoveredIndex.y = Mathf.Clamp(hoveredIndex.y, 0, dungeonHeight - 1);
		}else{
			hoveredIndex.x = -1;
			hoveredIndex.y = -1;
		}

		manaText.text = mana + " Mana";
	}

	public bool doesPathExist(){
		NavMesh.CalculatePath(dungeonStart.ToVec3() * dungeonGraphicsManager.tileSizeSqrd, dungeonEnd.ToVec3() * dungeonGraphicsManager.tileSizeSqrd, NavMesh.AllAreas, dungeonPath);

		if(dungeonPath.status == NavMeshPathStatus.PathComplete)
			return true;
		else
			return false;
	}

	public void setGridTile(int _x, int _y, int _type){
		if(!isValidIndex(_x, _y))
			return;
		grid[_x, _y] = _type;
		onGridTileChangedCallback(_x, _y, _type);
	}

	public int getGridTile(int _x, int _y){
		if(!isValidIndex(_x, _y))
			return -1;

		return grid[_x, _y];
	}

	public bool isValidIndex(int _x, int _y){
		if(_x < 0 || _x >= dungeonWidth)
			return false;
		if(_y < 0 || _y >= dungeonHeight)
			return false;

		return true;
	}

	public Vector3 getDungeonStartPos(){
		return dungeonStart.ToVec3() * dungeonGraphicsManager.tileSizeSqrd;
	}

	public Vector3 getDungeonEndPos(){
		return dungeonEnd.ToVec3() * dungeonGraphicsManager.tileSizeSqrd;
	}

	public int getHoveredX(){
		return hoveredIndex.x;
	}

	public int getHoveredY(){
		return hoveredIndex.y;
	}

	public NavMeshPath getDungeonPath(){
		return dungeonPath;
	}
		
	public void RegisterOnGridTileChangedCallback(Action<int, int, int> _callback){
		onGridTileChangedCallback -= _callback;
		onGridTileChangedCallback += _callback;
	}

	private void OnGUI(){
//		GUILayout.Label ("Tile: " + hoveredIndex.x + "," + hoveredIndex.y);
//		GUILayout.Label ("Tile Type: " + getGridTile (hoveredIndex.x, hoveredIndex.y));
//		GUI.Label(new Rect(0, Screen.height - 20, 100, 20), "Mana: " + mana);
	}

}
