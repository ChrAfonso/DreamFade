using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

public class CameraFilter : MonoBehaviour {

	private float targetSaturation; // TODO change by area
	private float startSaturation;
	private float tChange = -1f;
	private float changeDuration = 5f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (tChange != -1)
		{
			float p = tChange / changeDuration;
			float s = p * p * (3 - 2 * p);
			GetComponent<ColorCorrectionCurves>().saturation = (1 - p) * startSaturation + p * targetSaturation;

			tChange += Time.deltaTime;
			if (tChange >= changeDuration)
			{
				GetComponent<ColorCorrectionCurves>().saturation = targetSaturation;
				tChange = -1;
			}
		}
	}

	public void SetTargetSaturation(float s)
	{
		startSaturation = GetComponent<ColorCorrectionCurves>().saturation;
		targetSaturation = s;
		tChange = 0;
	}
}
