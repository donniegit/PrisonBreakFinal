using UnityEngine;
using System.Collections;

public static class ConvertLocation {
	public static Vector3 ConvertToReal(int i, float y, int j){
		return new Vector3(((j - 50f) + 0.5f) * 10, y, ((49f-i) + 0.5f) * 10);
	}
	public static Vector2 ConvertTo2D(Vector3 position){
		int cI = 0, cJ = 0;
		float tempX, tempZ;
		tempX = position.x * 0.1f - 0.5f + 50f;
		tempZ = 49f - (position.z * 0.1f - 0.5f);
		if(tempX < 0){
			cJ = 0;
		}
		if(tempZ < 0){
			cI = 0;
		}
		if(tempX > 0){
			if(tempX * 10f % 10f > 5){
				cJ = ((int)tempX) + 1;
			}else{
				cJ = (int)tempX;
			}
		}
		if(tempZ > 0){
			if(tempZ * 10 % 10 > 5){
				cI = ((int)tempZ) + 1;
			}else{
				cI = (int) tempZ;
			}
		}

		return new Vector2(cI, cJ);
	}
}
