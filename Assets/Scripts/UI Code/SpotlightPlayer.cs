using UnityEngine;
using System.Collections;

public class SpotlightPlayer : MonoBehaviour {
	public GameObject target;
	public float yPosition;
	// Use this for initialization
	void Start () {
		transform.localPosition = new Vector3(target.transform.localPosition.x + 10f, yPosition, target.transform.localPosition.z);
	}
	
	// Update is called once per frame
	void Update () {
		transform.localPosition = new Vector3(target.transform.localPosition.x + 10f, yPosition, target.transform.localPosition.z);
	}
}
