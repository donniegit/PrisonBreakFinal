using UnityEngine;
using System.Collections;

public class CellMateButton : MonoBehaviour {

	public Rect rectangle_0 = new Rect(0, 0, 100, 50);
	public bool show;
	// Use this for initialization
	void Start () {
		//this.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
//		if(GameEngine.cellmate.Alive == 0){
//			show = false;
//		}else{
//			show = true;
//		}
	}	
	void OnGUI(){
		if(show){
			//Creating Button
			if(GUI.Button (rectangle_0, "Help!" + GameEngine.RemainingCellmateLives)){
				GameEngine.cellmate.Respawned = true;
				GUIManager.cellmateO.SetActive(true);
				show = false;
				this.enabled = false;
			}
		}
	}
}
