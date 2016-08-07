using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

public class CameraFilter : MonoBehaviour {

	private static float defaultSaturationDuration = 2f;
	private static float defaultNoiseDuration = 2f;
	private static float defaultFadeDuration = 5f;

	private float targetSaturation; // TODO change by area
	private float startSaturation;
	private float tChange = -1f;
	private float changeDuration = 2f;

	private float targetNoise;
	private float startNoise;
	private float nChange = -1f;
	private float nChangeDuration = 2f;

	private float tFade = -1f;
	private float fadeDuration = 5f;
	private float fadeStart = 1f;
	private float fadeTarget = 0f;

	private string fadeCallback = "";

	private ColorCorrectionCurves colorFilter;
	private NoiseAndScratches noiseFilter;
	private VignetteAndChromaticAberration vignetteFilter;

	// Use this for initialization
	void Start () {
		colorFilter = GetComponent<ColorCorrectionCurves>();
		noiseFilter = GetComponent<NoiseAndScratches>();
		vignetteFilter = GetComponent<VignetteAndChromaticAberration>();
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

		if (tFade != -1)
		{
			float p = tFade / fadeDuration;
			float s = p * p * (3 - 2 * p);
			vignetteFilter.intensity = (1 - p) * fadeStart + p * fadeTarget;
			vignetteFilter.blur = (1 - p) * fadeStart + p * fadeTarget;

			tFade += Time.deltaTime;
			if (tFade >= fadeDuration)
			{
				vignetteFilter.intensity = fadeTarget;
				vignetteFilter.blur = fadeTarget;
				tFade = -1;

				if (fadeCallback != "")
				{
					GameController.instance.SendMessage(fadeCallback);
				}
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
	
	public void FadeOut(float duration=-1, string callback="")
	{
		fadeDuration = (duration == -1 ? defaultFadeDuration : duration);
		if (callback != "") fadeCallback = callback;

		tFade = 0;
		fadeStart = vignetteFilter.intensity;
		fadeTarget = 1f;
	}

	public void FadeIn(float duration = -1, string callback="")
	{
		fadeDuration = (duration == -1 ? defaultFadeDuration : duration);
		if (callback != "") fadeCallback = callback;

		tFade = 0;
		fadeStart = vignetteFilter.intensity;
		fadeTarget = 0f;
	}
}
