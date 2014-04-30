using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GUIManager : MonoBehaviour {
	
	public GUIText gameOverText, instructionsText, playerWin;
	
	// this seems pretty dodgy, although I guess if you know there is just one ... ugh
	private static GUIManager instance;
	private bool isGame;
	private List<GameObject> guardObjects = new List<GameObject>();
	private List<GameObject> ratObjects = new List<GameObject>();
	private List<GameObject> miniGuardObjects = new List<GameObject>();
	private List<Coin> coinObjects = new List<Coin>();
	private float startTime;
	private float endTime;
	public static GameObject cellmateO;
	public static GameObject cellmateM;
	public static GameObject camera;
	public static GameObject textureMaking;
	public static GameObject player;
	private int[,] tempMap;
	private bool result;
	
	void Start () {
		// perhaps should check here to make sure only one?
		/* Map Generation */
		GameMap.CreateMap();
		UIGameMap mapG = new UIGameMap();
		tempMap = mapG.CreateMap();
		generateMap ();

		/* Game Event Maneger activate*/
		instance = this;
		GameEventManager.GameStart += GameStart;
		GameEventManager.GameOver += GameOver;
		GameEventManager.GameUpdate += GameUpdate;

		/* Text Of the Game*/ 
		gameOverText.enabled = false;
		instructionsText.enabled = true;
		playerWin.enabled = false;

		/* Check Game is on or not*/
		isGame = false;

		cellmateO = GameObject.Find("CellMate");
		cellmateM = GameObject.Find ("MiniCell");
		player = GameObject.Find("Character");
		camera = GameObject.Find ("Camera");
		textureMaking = GameObject.Find ("TextureMaking");
	}
	
	void Update () {
		endTime = Time.time;
		if(Input.GetButtonDown("Jump") && !isGame){
			isGame = true;
			GameEventManager.TriggerGameStart();
		}
		if(isGame){
			if(GameEngine.cellmate.Alive == 0){
				cellmateO.SetActive(false);
				cellmateM.SetActive(false);
				//Debug.Log (GameEngine.RemainingCellmateLives);
				if(GameEngine.RemainingCellmateLives > 0){
					camera.GetComponent<CellMateButton>().show = true;
					camera.GetComponent<CellMateButton>().enabled = true;
				}
			}
			if((endTime - startTime) > 0.1){
				result = GameEngine.RunGame();
				if(result == false){
					GameEventManager.TriggerGameOver();
				}
				//GameDebugger.PrintArray(0, "Particle Filtering check", ParticleFilteringEstimator.FloorCellProbabilities);
				for(int i = 0 ; i < GameEngine.NumberOfGuards_PCG; i++){
					GuardAgent tempG = GameEngine.guards[i];
					guardObjects[i].transform.localPosition = ConvertLocation.ConvertToReal(tempG.LocationX, 4.58f, tempG.LocationY);
				}
				/*for(int i = 0; i < GameEngine.NumberOfRats_PCG; i++){
					RatAgent tempR = GameEngine.rats[i];
					ratObjects[i].transform.localPosition = ConvertLocation.ConvertToReal(tempR.LocationX, 0.25f, tempR.LocationY);
				}*/
				if(GameEngine.cellmate.Alive != 0){
					camera.GetComponent<CellMateButton>().show = false;
					camera.GetComponent<CellMateButton>().enabled = false;
					cellmateO.SetActive(true);
					cellmateM.SetActive(true);
					//startTime = Time.time;
					int x = GameEngine.cellmate.LocationX;
					int y = GameEngine.cellmate.LocationY;
					cellmateO.transform.localPosition = ConvertLocation.ConvertToReal(x, cellmateO.transform.localPosition.y, y);
					textureMaking.GetComponent<ControlPlane>().makeRoute(x, y);
				}
				startTime = Time.time;
			}

			if(GameEngine.player.Alive == 0){
				//GameEventManager.TriggerGameOver();
			}
		}
	}
	
	IEnumerator FadeInstructions() {
		for (float f = 5f; f >= 0; f -= 0.05f) {
			Color c = instructionsText.color;
			c.a = f/5f;
			instructionsText.color = c;
			yield return new WaitForSeconds(.01f);
		}
		instructionsText.enabled = false;
	}
	private void generateMap(){
		GameObject wallO = GameObject.Find("Wall");
		for(int i = 0; i < 100 ; i++){
			for(int j = 0; j < 100; j++){
				if(tempMap[i,j] != 0 && tempMap[i,j] != 4 && tempMap[i,j] != 3 && tempMap[i,j] != 5 && tempMap[i,j] != 2){
					GameObject cube = (GameObject)Instantiate(wallO, ConvertLocation.ConvertToReal(i, 155f, j), Quaternion.identity);
					cube.transform.localScale = new Vector3(10, 310f, 10);
				}else if(tempMap[i,j] == 3){
					tempMap = checkJail(tempMap, i, j);
				}else if(tempMap[i, j] == 2){
					for(int k = 1; k < 7; k++){
						tempMap[i, j + k] = 0;
					}
					GameObject exitO = GameObject.Find ("EXIT");
					GameObject exit = (GameObject) Instantiate(exitO, ConvertLocation.ConvertToReal(i, 0f, j), Quaternion.Euler (0, 180, 0));
					exit.transform.localPosition += new Vector3(30f, 0f, 0f);
				}
			}
		}
		
	}

	private int[,] checkJail(int[,] tempMap, int i, int j){
		if(tempMap[i + 1, j] == 3){
			for(int k = 1 ; k < 6; k++){
				tempMap[i + k, j] = 0;
			}
			GameObject jailO = GameObject.Find("JailHorizontal");
			GameObject jailWO = GameObject.Find ("JailWall");
			GameObject jail = (GameObject)Instantiate(jailO, ConvertLocation.ConvertToReal(i, 25f, j), Quaternion.Euler(0, 90, 0));
			GameObject jailW = (GameObject)Instantiate(jailWO, ConvertLocation.ConvertToReal(i, 300f, j), Quaternion.Euler(0, 90, 0));
			jailW.transform.localPosition -= new Vector3(0f, 0f, 30f);
			//jail.SetActive(true);
		}
		if(tempMap[i, j + 1] == 3){
			for(int k = 1 ; k < 6; k++){
				tempMap[i, j + k] = 0;
			}
			GameObject jailO = GameObject.Find("JailHorizontal");
			GameObject jailWO = GameObject.Find ("JailWall");
			GameObject jail = (GameObject)Instantiate(jailO, ConvertLocation.ConvertToReal(i, 25f, j), Quaternion.identity);
			GameObject jailW = (GameObject)Instantiate(jailWO, ConvertLocation.ConvertToReal(i, 300f, j), Quaternion.identity);
			jailW.transform.localPosition += new Vector3(30f, 0f, 0);
			//jail.SetActive(true);
		}

		return tempMap;
	}
	private void GameStart(){
		Debug.Log ("GAME START CALL!!!!!");
		result = true;
		/* Init of the Game Engine */
		GameEngine.InitGame();
		gameOverText.enabled = false;
		playerWin.enabled = false;
		//instructionsText.enabled = false;
		StartCoroutine(FadeInstructions());
		/* Making of Guards and intialize their position from Game Engine */
		GameObject guardO = GameObject.Find ("Guard");
		GameObject miniO = GameObject.Find ("MiniGuard");
		for(int i = 0 ; i < GameEngine.NumberOfGuards_PCG; i++){
			GuardAgent tempG = GameEngine.guards[i];
			GameObject Guard = (GameObject) Instantiate (guardO, ConvertLocation.ConvertToReal(tempG.LocationX, 4.58f, tempG.LocationY), Quaternion.identity);
			guardObjects.Add (Guard);
			GameObject Mini = (GameObject) Instantiate (miniO,miniO.transform.localPosition, Quaternion.identity);
			Mini.GetComponent<MiniMapGuard>().target = Guard;
			miniGuardObjects.Add (Mini);
		}
		
		/* Making of Rats and intialize their position from Game Engine */
		/*GameObject ratO = GameObject.Find ("Rat");
		for(int i = 0; i < GameEngine.NumberOfRats_PCG; i++){
			RatAgent tempR = GameEngine.rats[i];
			GameObject Rat = (GameObject) Instantiate (ratO, ConvertLocation.ConvertToReal(tempR.LocationX, 0.25f, tempR.LocationY), Quaternion.identity);
			ratObjects.Add (Rat);
		}*/
		/* Making of Cell Mate */
		cellmateO.SetActive(false);
		cellmateM.SetActive(false);

		/* Making of Coins and intialize their position from Game Engine */

		startTime = Time.time;
		textureMaking.GetComponent<ControlPlane>().makeStart();
	}

	private void GameUpdate() {
	}
	private void GameOver () {
		Debug.Log ("GAME OVER CALL!!!!!");
		isGame = false;
		GameEngine.player.Alive = 1;
		if(result == true){
			instructionsText.enabled = true;
			gameOverText.enabled = true;
			Debug.Log (guardObjects.Count);
			int count = guardObjects.Count;
			for(int i = 0 ; i < count; i++){
				Destroy(guardObjects[i]);
				Destroy(miniGuardObjects[i]);
			}
			while(guardObjects.Count!=0){
				guardObjects.RemoveAt(0);
				miniGuardObjects.RemoveAt(0);
			}	
		}else if(result == false){
			playerWin.enabled = true;
			instructionsText.enabled = true;
			int count = guardObjects.Count;
			for(int i = 0 ; i < count; i++){
				Destroy(guardObjects[i]);
				Destroy(miniGuardObjects[i]);
			}
			while(guardObjects.Count!=0){
				guardObjects.RemoveAt(0);
				miniGuardObjects.RemoveAt(0);
			}	
		}
	}

}