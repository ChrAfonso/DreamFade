using UnityEngine;
using System.Collections;

public class AreaTrigger : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other)
	{
		GameController.instance.SendMessage("OnAreaEnter", 1);
	}
}
