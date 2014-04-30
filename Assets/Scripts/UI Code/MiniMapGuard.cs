using UnityEngine;
using System.Collections;

public class MiniMapGuard : MonoBehaviour {
	public GameObject target;
	public float yPosition;
	// Use this for initialization
	void Start () {
		transform.localPosition = new Vector3(target.transform.localPosition.x, yPosition, target.transform.localPosition.z);
	}
	
	// Update is called once per frame
	void Update () {
		transform.localPosition = new Vector3(target.transform.localPosition.x, yPosition, target.transform.localPosition.z);
	}
}
