using UnityEngine;
using System.Collections;

public class Glow : MonoBehaviour {

	private Material material;

	public float Speed = 5;
	public float MinAlpha = 0.0f;
	public float MaxAlpha = 0.2f;

	// Use this for initialization
	void Start () {
		material = GetComponent<MeshRenderer>().material;
	}
	
	// Update is called once per frame
	void Update () {
		Color glowColor = material.color;
		glowColor.a = MinAlpha + (MaxAlpha * (Mathf.Sin(Time.time * Speed) + 1) / 2);
		Debug.Log(glowColor.a);
		material.color = glowColor;
	}
}
