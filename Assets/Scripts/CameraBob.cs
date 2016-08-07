using UnityEngine;
using System.Collections;

public class CameraBob : MonoBehaviour {

	private float originalY;

	public float SpeedY = 0.8f;
	public float SpeedX = 1f;
	public float BobHeight = 0.5f;
	public float BobWidth = 0.4f;

	// Use this for initialization
	void Start () {
		originalY = transform.localPosition.y;
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 pos = transform.localPosition;
		pos.y = originalY + Mathf.Sin(Time.time * SpeedY) * BobHeight;
		pos.x = Mathf.Sin(Time.time * SpeedX) * BobWidth;
		transform.localPosition = pos;
	}
}
