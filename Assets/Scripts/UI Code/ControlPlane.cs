using UnityEngine;
using System.Collections;
using System.IO;

[System.Serializable]
public class ControlPlane : MonoBehaviour {
	public Texture2D plane;
	int height;
	int width;
	
	void Start() {
		height = plane.height;
		width = plane.width;

		plane.Resize(100, 100);
		makeStart();
//		plane.SetPixel(0, 100, Color.gray);
//		plane.SetPixel(0, 100, Color.gray);
//		plane.SetPixel(1, 100, Color.gray);
//		plane.SetPixel(1, 100, Color.gray);
//		plane.SetPixel(2, 100, Color.gray);
//		plane.SetPixel(2, 100, Color.gray);
//		plane.SetPixel(0, 100, Color.gray);
//		//plane.GetPixel(0, 100).a = 0.5;
//		plane.SetPixel(0, 100, Color.white);
//		plane.SetPixel(1, 100, Color.white);
//		plane.SetPixel(1, 100, Color.white);
//		plane.SetPixel(2, 100, Color.white);
//		plane.SetPixel(2, 100, Color.white);

		
//		for(int i = 0 ; i < 10; i++){
//			plane.SetPixel (i, 100, Color.white);
//		}
		
		plane.Apply();
		
		//		for(int i = 0; i < plane.height ; i++){
		//			for(int j = 0 ; j < plane.width; j++){
		//				plane.SetPixel(i, j , Color.black);
		//			}
		//		}
		//		plane.Apply();
		//		plane.SetPixel(1, 1, Color.black);
		//		plane.SetPixel(1, 2, Color.black);
		//		plane.SetPixel(2, 1, Color.black);
		//		plane.SetPixel(2, 2, Color.black);
		//		plane.SetPixel(3, 1, Color.black);
		//		plane.SetPixel(3, 2, Color.black);
		//		plane.Apply();
		//plane.texelSize.;
	}
	
	void Update(){
	}
	public void makeStart(){
		for(int i = 0; i < plane.height ; i++){
			for(int j = 0 ; j < plane.width; j++){
				plane.SetPixel(i, j , Color.black);
			}
		}
		plane.Apply();
	}

	public void makeRoute(int x , int y){
		for(int i = 0; i < 6; i++){
			for(int j = 0; j < 6 ; j++){
				plane.SetPixel(x - i, y - j, Color.white);
				plane.SetPixel(x + i, y + j, Color.white);
				plane.SetPixel(x - i, y + j, Color.white);
				plane.SetPixel(x + i, y - j, Color.white);
			}
		}

		plane.Apply();
	}
}
