using UnityEngine;
using System.Collections;

public class OnFlowerDrop : MonoBehaviour {

	public int AreaID;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void FlowerDropped(GameObject flower)
	{
		flower.transform.SetParent(transform, false); // attach to target
		flower.transform.localPosition = Vector3.zero;

		Component.Destroy(flower.GetComponent<Glow>());

		// notify game controller
		GameController.instance.AwakenArea(AreaID);

		// awaken trees
		for (int t = 0; t < transform.parent.childCount; t++)
		{
			GameObject tree = transform.parent.GetChild(t).gameObject;
			if (tree.tag == "Tree")
			{
				tree.GetComponent<GrowTree>().StartGrowing();
			}
		}
	}
}
