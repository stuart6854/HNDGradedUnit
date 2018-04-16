﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildController : MonoBehaviour {

	public static readonly int Wall_Place_Cost = 10;
	public static readonly int Wall_Remove_Cost = (int)(Wall_Place_Cost * 0.9f);

	public Dungeon dungeon;

	public GameObject tileCursor;

	public enum BuildMode{ None, Wall, DefenceStructure,  DefenceStructure_Destroy}
	private BuildMode buildMode = BuildMode.DefenceStructure;

	private int selectedDefStructure;

	private void Update(){
		int x = dungeon.getHoveredX();
		int y = dungeon.getHoveredY();

		tileCursor.transform.position = new Vector3(x * dungeon.dungeonGraphicsManager.tileSizeSqrd, 0, y * dungeon.dungeonGraphicsManager.tileSizeSqrd);
		if(y != -1 && y != -1) {
			tileCursor.SetActive(true);

			if(Input.GetMouseButtonDown(0)) {
				if(buildMode == BuildMode.Wall) {
					int tile = dungeon.getGridTile(x, y);
					if(tile == 0) {
						if(Dungeon.Mana >= Wall_Place_Cost) {
							dungeon.setGridTile(x, y, 1);
							Dungeon.Mana -= Wall_Place_Cost;
						}
					} else if(tile == 1) {
						if(Dungeon.Mana >= Wall_Remove_Cost) {
							dungeon.setGridTile(x, y, 0);
							Dungeon.Mana -= Wall_Remove_Cost;
						}
					}				
				} else if(buildMode == BuildMode.DefenceStructure) {
					DefensiveStructure ds = dungeon.dungeonGraphicsManager.towersAndTraps[selectedDefStructure].GetComponent<DefensiveStructure>();

					if(Dungeon.Mana >= ds.manaCost) {
						dungeon.dsm.setDefensiveStructure(x, y, ds);
						Dungeon.Mana -= ds.manaCost;
					}
				}else if(buildMode == BuildMode.DefenceStructure_Destroy){
					DefensiveStructure ds = dungeon.dsm.getDefensiveStructure(x, y);

					if(ds != null) {
						dungeon.dsm.setDefensiveStructure(x, y, null);
						Dungeon.Mana += (int)(ds.manaCost * 0.9f);
					}
				}
			}
		}else{
			tileCursor.SetActive(false);
		}
	}

	private void OnGUI(){
		if(buildMode == BuildMode.DefenceStructure) {
			int towerTrapCount = dungeon.dungeonGraphicsManager.towersAndTraps.Length;

			if(buildMode == BuildMode.DefenceStructure) {
				if(GUI.Button(new Rect(200, 10, 20, 20), "<"))
					selectedDefStructure--;
			
				if(GUI.Button(new Rect(320, 10, 20, 20), ">"))
					selectedDefStructure++;

				if(selectedDefStructure < 0)
					selectedDefStructure = towerTrapCount - 1;
			
				if(selectedDefStructure >= towerTrapCount)
					selectedDefStructure = 0;
			
				if(GUI.Button(new Rect(220, 10, 100, 20), dungeon.dungeonGraphicsManager.towersAndTraps[selectedDefStructure].name)){
					buildMode = BuildMode.Wall;
				}
			}
		}else if(buildMode == BuildMode.Wall){
			if(GUI.Button(new Rect(220, 10, 100, 20), "Wall Mode")){
				buildMode = BuildMode.DefenceStructure_Destroy;
			}
		}else if(buildMode == BuildMode.DefenceStructure_Destroy){
			if(GUI.Button(new Rect(220, 10, 100, 20), "DS Destroy")){
				buildMode = BuildMode.DefenceStructure;
			}
		}
	}

	public void SetBuildMode(BuildMode _mode){
		buildMode = _mode;
	}

}