using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DungeonGraphicsManager : MonoBehaviour {

	private readonly Dictionary<int, int> bitmaskToTile = new Dictionary<int, int>(){
		{0, 0}, {16, 0}, {32, 0}, {64, 0}, {48, 0}, {80, 0}, {96, 0}, {112, 0}, {128, 0}, {144, 0}, {160, 0}, {176, 0}, {192, 0}, {208, 0}, {224, 0}, {240, 0},				// Tile 0

		{1, 1}, {2, 1}, {4, 1}, {8, 1}, {17, 1}, {18, 1}, {20, 1}, {24, 1}, {33, 1}, {34, 1}, {36, 1}, {40, 1}, {49, 1}, {50, 1}, {52, 1}, {56, 1},							// Tile 1
		{65, 1}, {66, 1}, {68, 1}, {72, 1}, {81, 1}, {82, 1}, {84, 1}, {88, 1}, {97, 1}, {98, 1}, {100, 1}, {104, 1}, {113, 1}, {114, 1}, {116, 1}, {120, 1},
		{129, 1}, {130, 1}, {132, 1}, {136, 1}, {145, 1}, {146, 1}, {148, 1}, {152, 1}, {161, 1}, {162, 1}, {164, 1}, {168, 1}, {177, 1}, {178, 1}, {180, 1}, {184, 1}, 
		{193, 1}, {194, 1}, {196, 1}, {200, 1}, {209, 1}, {210, 1}, {212, 1}, {216, 1}, {225, 1}, {226, 1}, {228, 1}, {232, 1}, {241, 1}, {242, 1}, {244, 1}, {248, 1}, 

		{3, 2}, {6, 2}, {12, 2}, {9, 2}, {35, 2}, {22, 2}, {28, 2}, {25, 2}, {67, 2}, {44, 2}, {41, 2}, {99, 2}, {70, 2}, {60, 2}, {57, 2},									// Tile 2
		{163, 2}, {86, 2}, {140, 2}, {73, 2}, {195, 2}, {172, 2}, {89, 2}, {227, 2}, {118, 2}, {188, 2}, {105, 2}, {131, 2}, {134, 2}, {156, 2}, {121, 2}, {150, 2}, 
		{198, 2}, {214, 2}, 

		{5, 3}, {10, 3}, {21, 3}, {26, 3}, {37, 3}, {42, 3}, {53, 3}, {58, 3}, {69, 3}, {74, 3}, {85, 3}, {90, 3}, {101, 3}, {106, 3}, {117, 3}, {122, 3}, 					// Tile 3
		{133, 3}, {138, 3}, {149, 3}, {154, 3}, {165, 3}, {170, 3}, {181, 3}, {186, 3}, {197, 3}, {202, 3}, {213, 3}, {218, 3}, {229, 3}, {234, 3}, {245, 3}, {250, 3}, 

		{7, 4},	{14, 4}, {13, 4}, {11, 4}, {30, 4}, {29, 4}, {27, 4}, {45, 4}, {43, 4}, {87, 4}, {142, 4}, {61, 4}, 								// Tile 4
		{71, 4}, {158, 4}, {75, 4}, {135, 4}, {91, 4}, {199, 4}, {107, 4}, {123, 4}, 

		{15, 5}, 									// Tile 5

		{19, 6}, {54, 6}, {76, 6}, {169, 6}, {51, 6}, {166, 6}, {92, 6}, {201, 6}, {83, 6}, {182, 6}, {108, 6}, {185, 6}, {115, 6}, {230, 6}, {124, 6}, {217, 6}, 			// Tile 6
		{147, 6}, {246, 6}, {204, 6}, {233, 6}, {179, 6}, {38, 6}, {220, 6}, {137, 6}, {211, 6}, {102, 6}, {236, 6}, {153, 6}, {243, 6}, {249, 6}, {252, 6},

		{31, 7}, {47, 7}, {79, 7}, {143, 7}, 		// Tile 7

		{55, 8}, {110, 8}, {205, 8}, {155, 8}, {119, 8}, {126, 8}, {237, 8}, {251, 8}, {183, 8}, {254, 8}, {253, 8}, {219, 8}, {247, 8}, {221, 8},   									// Tile 8

		{77, 9}, {139, 9}, {151, 9}, {174, 9}, {93, 9}, {203, 9}, {215, 9}, {190, 9}, {109, 9}, {171, 9}, {23, 9}, {46, 9}, {125, 9}, {235, 9}, {62, 9},							// Tile 9

		{78, 10}, {141, 10}, {187, 10}, {103, 10}, {94, 10}, {157, 10}, {167, 10}, {206, 10}, {173, 10}, {231, 10}, {222, 10}, {189, 10}, {59, 10}, {39, 10}, {238, 10},	// Tile 10

		{95, 11}, {175, 11},						// Tile 11

		{111, 12}, {207, 12}, {159, 12}, {63, 12},	// Tile 12

		{127, 13}, {239, 13}, {223, 13}, {191, 13},	// Tile 13

		{255, 14},									// Tile 14

		{256, 15}									// Tile 15

	};
	private readonly Dictionary<int, int> dungeonTileRot = new Dictionary<int, int>(){
		{0, 0}, {16, 1}, {32, 2}, {64, 3}, {48, 0}, {80, 1}, {96, 2}, {112, 3}, {128, 0}, {144, 1}, {160, 2}, {176, 3}, {192, 0}, {208, 1}, {224, 2}, {240, 3},			// Tile 0

		{1, 0}, {2, 1}, {4, 2}, {8, 3}, {17, 0}, {18, 1}, {20, 2}, {24, 3}, {33, 0}, {34, 1}, {36, 2}, {40, 3}, {49, 0}, {50, 1}, {52, 2}, {56, 3},						// Tile 1
		{65, 0}, {66, 1}, {68, 2}, {72, 3}, {81, 0}, {82, 1}, {84, 2}, {88, 3}, {97, 0}, {98, 1}, {100, 2}, {104, 3}, {113, 0}, {114, 1}, {116, 2}, {120, 3},
		{129, 0}, {130, 1}, {132, 2}, {136, 3}, {145, 0}, {146, 1}, {148, 2}, {152, 3}, {161, 0}, {162, 1}, {164, 2}, {168, 3}, {177, 0}, {178, 1}, {180, 2}, {184, 3}, 
		{193, 0}, {194, 1}, {196, 2}, {200, 3}, {209, 0}, {210, 1}, {212, 2}, {216, 3}, {225, 0}, {226, 1}, {228, 2}, {232, 3}, {241, 0}, {242, 1}, {244, 2}, {248, 3}, 

		{3, 0}, {6, 1}, {12, 2}, {9, 3}, {35, 0}, {22, 1}, {28, 2}, {25, 3}, {67, 0}, {44, 2}, {41, 3}, {99, 0}, {70, 1}, {60, 2}, {57, 3},					// Tile 2
		{163, 0}, {86, 1}, {140, 2}, {73, 3}, {195, 0}, {172, 2}, {89, 3}, {227, 0}, {118, 1}, {188, 2}, {105, 3}, {131, 0}, {134, 1}, {156, 2}, {121, 3}, {150, 1}, 
		{198, 1}, {214, 1}, 

		{5, 0}, {10, 1}, {21, 2}, {26, 3}, {37, 0}, {42, 1}, {53, 2}, {58, 3}, {69, 0}, {74, 1}, {85, 2}, {90, 3}, {101, 0}, {106, 1}, {117, 2}, {122, 3}, 				// Tile 3
		{133, 0}, {138, 1}, {149, 2}, {154, 3}, {165, 0}, {170, 1}, {181, 2}, {186, 3}, {197, 0}, {202, 1}, {213, 2}, {218, 3}, {229, 0}, {234, 1}, {245, 2}, {250, 3}, 

		{7, 0},	{14, 1}, {13, 2}, {11, 3}, {30, 1}, {29, 2}, {27, 3}, {45, 2}, {43, 3}, {87, 0}, {142, 1}, {61, 2}, 				// Tile 4
		{71, 0}, {158, 1}, {75, 3}, {135, 0}, {91, 3}, {199, 0}, {107, 3}, {123, 3}, 

		{15, 0}, 									// Tile 5

		{19, 0}, {54, 1}, {76, 2}, {169, 3}, {51, 0}, {166, 1}, {92, 2}, {201, 3}, {83, 0}, {182, 1}, {108, 2}, {185, 3}, {115, 0}, {230, 1}, {124, 2}, {217, 3}, 		// Tile 6
		{147, 0}, {246, 1}, {204, 2}, {233, 3}, {179, 0}, {38, 1}, {220, 2}, {137, 3}, {211, 0}, {102, 1}, {236, 2}, {153, 3}, {243, 0}, {249, 2}, {252, 2},

		{31, 0}, {47, 1}, {79, 2}, {143, 3}, 		// Tile 7

		{55, 0}, {110, 1}, {205, 2}, {155, 3}, {119, 0}, {126, 1}, {237, 2}, {251, 3}, {183, 0}, {254, 1}, {253, 2}, {219, 3}, {247, 0}, {221, 2},   								// Tile 8

		{77, 0}, {139, 1}, {151, 2}, {174, 3}, {93, 0}, {203, 1}, {215, 2}, {190, 3}, {109, 0}, {171, 1}, {23, 2}, {46, 3}, {125, 0}, {235, 1}, {62, 3},				// Tile 9

		{78, 0}, {141, 1}, {187, 2}, {103, 3}, {94, 0}, {157, 1}, {167, 3}, {206, 0}, {173, 1}, {231, 3}, {222, 0}, {189, 1}, {59, 2}, {39, 3}, {238, 0},			// Tile 10

		{95, 0}, {175, 1},							// Tile 11

		{111, 0}, {207, 1}, {159, 2}, {63, 3},		// Tile 12

		{127, 0}, {239, 1}, {223, 2}, {191, 3},		// Tile 13

		{255, 0},									// Tile 14

		{256, 0}									// Tile 15

	};

	public Dungeon dungeon;
	public NavMeshHandler navMeshHandler;

	public int tileSizeSqrd = 4;

	public GameObject nullTilePrefab;
	public GameObject floorPrefab;
	public GameObject[] dungeonTiles;
	public GameObject[] towersAndTraps;

	private Dictionary<Vec2I, GameObject> gridTileObjs;
	private Dictionary<Vec2I, GameObject> defensiveStructureObjs;

	private bool isDirty;

	private void Awake () {
		gridTileObjs = new Dictionary<Vec2I, GameObject>();
		defensiveStructureObjs = new Dictionary<Vec2I, GameObject>();

		dungeon.RegisterOnGridTileChangedCallback(OnDungeonGridTileChangedCallback);
		dungeon.dsm.RegisterOnGridStructureChangedCallback(OnDefensiveStructureChangedCallback);
	}

	private void Start(){
		for(int i = 0; i < 256; i++){
			if(!bitmaskToTile.ContainsKey(i))
				Debug.LogError("No Bitmask for " + i + " in dictionary!");

			if(!dungeonTileRot.ContainsKey(i))
				Debug.LogError("No Tile Rotation for " + i + " in dictionary!");
		}
	}

	private void Update(){
		if (Input.GetKeyDown (KeyCode.F1))
			isDirty = true;

		if(isDirty){
			AutoTileDungeon();
			navMeshHandler.BuildNavMesh();
			isDirty = false;
		}
	}

	private void OnDungeonGridTileChangedCallback(int _x, int _y, int _type){
		//Debug.Log("DGM -> Tile Dirtied(" + _x + ", " + _y + ")");

		isDirty = true;
	}

	private void OnDefensiveStructureChangedCallback(int _x, int _y, GameObject _ds){
		Vec2I pos = new Vec2I(_x, _y);
		if(defensiveStructureObjs.ContainsKey(pos)){
			Destroy(defensiveStructureObjs[pos]);
			defensiveStructureObjs.Remove(pos);
		}	
		
		defensiveStructureObjs.Add(pos, CreateObj((_ds != null) ? _ds : floorPrefab, pos.ToVec3() * tileSizeSqrd, Quaternion.identity));

		navMeshHandler.BuildNavMesh();
	}

	private void AutoTileDungeon(){
		//print ("Cleaning Graphics!");

		for(int y = 0; y < dungeon.dungeonHeight; y++){
			for(int x = 0; x < dungeon.dungeonWidth; x++){
				if(!dungeon.isValidIndex(x, y))
					continue;

				Vec2I tilePos = new Vec2I(x, y);
				if(gridTileObjs.ContainsKey(tilePos)){
					Destroy(gridTileObjs[tilePos]);
					gridTileObjs.Remove(tilePos);
				}	

				int bitmaskVal = getAutoTileVal(x, y);
				gridTileObjs.Add(tilePos, CreateTile(x, y, bitmaskVal));
			}
		}
	}

	private int getAutoTileVal(int _x, int _y){
		int bitmask = 0;
		int tileVal = dungeon.getGridTile(_x, _y);

		//Cross
		int val = dungeon.getGridTile(_x, _y + 1);
		if(!dungeon.isValidIndex(_x, _y) || val == tileVal)
			bitmask += 1;

		val = dungeon.getGridTile(_x + 1, _y);
		if(!dungeon.isValidIndex(_x, _y) || val == tileVal)
			bitmask += 2;

		val = dungeon.getGridTile(_x, _y - 1);
		if(!dungeon.isValidIndex(_x, _y) || val == tileVal)
			bitmask += 4;

		val = dungeon.getGridTile(_x - 1, _y);
		if(!dungeon.isValidIndex(_x, _y) || val == tileVal)
			bitmask += 8;

		//Diagonal
		val = dungeon.getGridTile(_x + 1, _y + 1);
		if(!dungeon.isValidIndex(_x, _y) || val == tileVal)
			bitmask += 16;

		val = dungeon.getGridTile(_x + 1, _y - 1);
		if(!dungeon.isValidIndex(_x, _y) || val == tileVal)
			bitmask += 32;

		val = dungeon.getGridTile(_x - 1, _y - 1);
		if(!dungeon.isValidIndex(_x, _y) || val == tileVal)
			bitmask += 64;

		val = dungeon.getGridTile(_x - 1, _y + 1);
		if(!dungeon.isValidIndex(_x, _y) || val == tileVal)
			bitmask += 128;

		bitmask += tileVal * 256;
		bitmask = Mathf.Clamp (bitmask, 0, 256);

		return bitmask;
	}

	private GameObject CreateTile(int _x, int _y, int _bitmaskVal){
		GameObject tilePrefab;
		Vector3 pos = new Vector3(_x * tileSizeSqrd, 0, _y * tileSizeSqrd);
		Quaternion rot = Quaternion.identity;

		if(bitmaskToTile.ContainsKey(_bitmaskVal)) {

			int tileIndex = bitmaskToTile[_bitmaskVal];
			if(tileIndex < dungeonTiles.Length){
				tilePrefab = dungeonTiles[tileIndex];
				if (dungeonTileRot.ContainsKey (_bitmaskVal))

					rot = Quaternion.Euler (0, dungeonTileRot [_bitmaskVal] * 90, 0);
				else {

					Debug.LogError ("Bitmask Rotation not found(" + _x + "," + _y + "): " + _bitmaskVal);
				}

			}else{

				tilePrefab = nullTilePrefab;
				Debug.LogError("Dungeon Tile " + tileIndex + " does not exist! (" + _x + "," + _y + ")(" + _bitmaskVal + ")");
			}

		}else{

			tilePrefab = nullTilePrefab;
			Debug.LogError ("Bitmask Tile not found(" + _x + "," + _y + "): " + _bitmaskVal);
		}

		GameObject obj = (GameObject)Instantiate(tilePrefab, pos, rot);
		obj.name = "tile_" + _bitmaskVal;
		obj.transform.SetParent(transform);

		return obj;
	}

	private GameObject CreateObj(GameObject _prefab, Vector3 _pos, Quaternion _rot){
		GameObject obj = (GameObject)Instantiate(_prefab, _pos, _rot);
		obj.transform.SetParent(transform);

		return obj;
	}

	private void OnGUI(){
		GUI.Label (new Rect(0, 50, 100, 30), "Bitmask: " + getAutoTileVal (dungeon.getHoveredX(), dungeon.getHoveredY()));
	}

}
