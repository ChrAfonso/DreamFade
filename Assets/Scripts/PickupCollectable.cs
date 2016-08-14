using UnityEngine;
using System.Collections;

public class PickupCollectable : MonoBehaviour {

	private GameObject objectInRange;
	private GameObject carryingObject;

	public GameObject anchor;

	// TEST
	public GameObject TESTTREE;

	void Start()
	{
		if (anchor == null) anchor = gameObject;
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(0)) {
			if (objectInRange != null && carryingObject == null)
			{
				// Pickup
				carryingObject = objectInRange;
				objectInRange.transform.parent = anchor.transform;

				// TEST
				if(TESTTREE != null)
				{
					Debug.Log("Start Growing");
					TESTTREE.SendMessage("StartGrowing");
				}
			}
			else if (carryingObject)
			{
				// check dropTarget - TODO only drop it at correct point, once picked up?
				GameObject dropTarget = carryingObject.GetComponent<Collectable>().dropTarget;
				if (dropTarget != null && dropTarget.GetComponent<Collider>().bounds.Intersects(carryingObject.GetComponent<Collider>().bounds))
				{
					// trigger trees awakening animation
					if(dropTarget.tag == "MagicTree")
					{
						Debug.Log("Flower dropped, awaken trees...");
						dropTarget.GetComponent<OnFlowerDrop>().FlowerDropped(carryingObject);
					}
					Camera.main.GetComponent<AudioSource>().Play(); // play pickup sound
					carryingObject = null;
				}
				else
				{
					// Drop
					//carryingObject.transform.parent = null; // TODO activate gravity?
					//carryingObject = null;
				}
			}
		}
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
