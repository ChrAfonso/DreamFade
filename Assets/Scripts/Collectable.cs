using UnityEngine;
using System.Collections;

public class Collectable : MonoBehaviour {

	public GameObject GlowTemplate;
	private GameObject myGlow;

	// Use this for initialization
	void Start () {
	
	}
	
	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			StartGlowing();
		}
	}

	void OnTriggerExit(Collider other)
	{
		if (other.tag == "Player")
		{
			StopGlowing();
		}
	}

	void StartGlowing()
	{
		if (GlowTemplate != null)
		{
			Debug.Log("StartGlowing");
			myGlow = GameObject.Instantiate(GlowTemplate);
			myGlow.transform.SetParent(transform, false);
		}
		else
		{
			Debug.Log("Not GlowTemplate assigned!");
		}
	}

	void StopGlowing()
	{
		GameObject.Destroy(myGlow);
		myGlow = null;
	}
}
