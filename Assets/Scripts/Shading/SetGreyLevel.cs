using UnityEngine;
using System.Collections;

public class SetGreyLevel : MonoBehaviour {

	Material material;
	Color originalColor;
	float grey;

	// Use this for initialization
	void Start () {
		material = GetComponent<MeshRenderer>().material;
		originalColor = material.color;
		grey = originalColor.grayscale;
	}

	void setGreyLevel(float g)
	{
		g = Mathf.Clamp01(g);

		material.color = new Color(originalColor.r, originalColor.g, originalColor.b);
	}
}
