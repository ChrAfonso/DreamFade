using UnityEngine;
using System.Collections;

public class PickupCollectable : MonoBehaviour {

	GameObject objectInRange;

	// Update is called once per frame
	void Update () {
		
	}

	public void InRange(GameObject collectable)
	{
		objectInRange = collectable;
	}

	public void OutOfRange(GameObject collectable)
	{
		if (objectInRange == collectable)
		{
			objectInRange = null;
		}
	}
}
