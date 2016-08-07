using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

public class CameraFilter : MonoBehaviour {

	private static float defaultSaturationDuration = 2f;
	private static float defaultNoiseDuration = 2f;

	private float targetSaturation; // TODO change by area
	private float startSaturation;
	private float tChange = -1f;
	private float changeDuration = 2f;

	private float targetNoise;
	private float startNoise;
	private float nChange = -1f;
	private float nChangeDuration = 2f;

	private ColorCorrectionCurves colorFilter;
	private NoiseAndScratches noiseFilter;

	// Use this for initialization
	void Start () {
		colorFilter = GetComponent<ColorCorrectionCurves>();
		noiseFilter = GetComponent<NoiseAndScratches>();
	}
	
	// Update is called once per frame
	void Update () {
		if (tChange != -1)
		{
			float p = tChange / changeDuration;
			float s = p * p * (3 - 2 * p);
			colorFilter.saturation = (1 - p) * startSaturation + p * targetSaturation;

			tChange += Time.deltaTime;
			if (tChange >= changeDuration)
			{
				colorFilter.saturation = targetSaturation;
				tChange = -1;
			}
		}

		if (nChange != -1)
		{
			float p = nChange / nChangeDuration;
			float s = p * p * (3 - 2 * p);
			noiseFilter.grainSize = (1 - p) * startNoise + p * targetNoise;

			nChange += Time.deltaTime;
			if (nChange >= nChangeDuration)
			{
				noiseFilter.grainSize = targetNoise;
				nChange = -1;
			}
		}
	}

	public void SetTargetSaturation(float s, float duration=-1)
	{
		changeDuration = (duration > -1 ? duration : defaultSaturationDuration);

		startSaturation = colorFilter.saturation;
		targetSaturation = Mathf.Clamp01(s);
		tChange = 0;
	}

	public void SetTargetNoise(float n, float duration = -1)
	{
		nChangeDuration = (duration > -1 ? duration : defaultNoiseDuration);

		startNoise = noiseFilter.grainSize;
		targetNoise = n;
		nChange = 0;
	}
}
