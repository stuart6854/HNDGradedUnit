using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Dungeon : MonoBehaviour {

	public DungeonGraphicsManager dungeonGraphicsManager;

	public int dungeonWidth, dungeonHeight;
	public Texture2D dungeonLayoutTexture; //Used to set the layout of the dungeon at start

	private int[,] grid;

	private Vec2I hoveredIndex = new Vec2I();

	private Action<int, int, int> onGridTileChangedCallback;

	private void Start () {
		if(dungeonLayoutTexture != null){
			dungeonWidth = dungeonLayoutTexture.width;
			dungeonHeight = dungeonLayoutTexture.height;
		}

		grid = new int[dungeonWidth, dungeonHeight];

		for(int y = 0; y < dungeonHeight; y++){
			for(int x = 0; x < dungeonWidth; x++){
				if(dungeonLayoutTexture != null) {
					setGridTile(x, y, (dungeonLayoutTexture.GetPixel(x, y) == Color.white) ? 0 : 1);
				}else
					setGridTile(x, y, 1);
			}
		}
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

		if(Input.GetMouseButtonDown(0)){
			if(hoveredIndex.x != -1 && hoveredIndex.y != -1){
				setGridTile(hoveredIndex.x, hoveredIndex.y, (getGridTile(hoveredIndex.x, hoveredIndex.y) == 0) ? 1 : 0);
			}
		}


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

	public void RegisterOnGridTileChangedCallback(Action<int, int, int> _callback){
		onGridTileChangedCallback -= _callback;
		onGridTileChangedCallback += _callback;
	}

	public int getHoveredX(){
		return hoveredIndex.x;
	}

	public int getHoveredY(){
		return hoveredIndex.y;
	}

	private void OnGUI(){
		GUILayout.Label ("Tile: " + hoveredIndex.x + "," + hoveredIndex.y);
		GUILayout.Label ("Tile Type: " + getGridTile (hoveredIndex.x, hoveredIndex.y));
	}

}
