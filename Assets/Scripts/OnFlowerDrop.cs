using UnityEngine;
using System.Collections;

public class OnFlowerDrop : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void FlowerDropped()
	{
		// awaken trees
		for(int t = 0; t < transform.parent.childCount; t++)
		{
			GameObject tree = transform.parent.GetChild(t).gameObject;
			if (tree.tag == "Tree")
			{
				tree.GetComponent<GrowTree>().StartGrowing();
			}
		}
	}
}
