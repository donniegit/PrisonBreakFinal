using UnityEngine;
using System.Collections;
using System.IO;

public class Map {
	public int[,] map;

	public Map(){
		int index = 0;
		map = new int[100, 100];
		try{
			TextAsset textFile = Resources.Load("text/result5") as TextAsset;
			StringReader sr = new StringReader(textFile.text);
			string source = sr.ReadLine();
			Debug.Log (source.Length);
			while(source != null){
				for(int j = 0; j < source.Length; j++){
					map[index,j] = (int)char.GetNumericValue(source[j]);
				}
				index++;
				source = sr.ReadLine();
			}
		}catch(System.Exception ee){
			Debug.Log("Exception");
			return;
		}
	}

	public int[,] getMap(){
		return map;
	}
}
