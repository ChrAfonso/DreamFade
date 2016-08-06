using UnityEngine;
using System.Collections;

public class GrowTree : MonoBehaviour {

	private float duration = 5;
	private float t = 0;

	private float pTrunkStart = 0f;
	private float pTrunkStop = .5f;

	private float pTopStart = .3f;
	private float pTopStop = .8f;

	private float pLeavesStart = .5f;
	private float pLeavesStop = 1f;

	public Vector3 startScaleTrunk;
	private Vector3 targetScaleTrunk, targetScaleTop;
	private Vector3[] targetScaleLeaves;
	private Vector3 startScaleTop;
	private Vector3[] startScaleLeaves;

	Transform tTrunk, tTop;
	Transform ttLeaves;

	bool active = false;

	// Use this for initialization
	void Start () {
		tTrunk = transform.FindChild("TreeTrunk");
		tTop = tTrunk.transform.FindChild("TreeTop");
		ttLeaves = tTop.transform.FindChild("TreeLeaves");

		// use actual scale as target, then shrink as starting value
		targetScaleTrunk = tTrunk.localScale;
		targetScaleTop = tTop.localScale;
		targetScaleLeaves = new Vector3[ttLeaves.childCount];
		for (int l = 0; l < ttLeaves.childCount; l++)
		{
			targetScaleLeaves[l] = ttLeaves.GetChild(l).localScale;
		}

		tTrunk.localScale = startScaleTrunk;
		tTop.localScale = Vector3.zero;
		for(int l = 0; l < ttLeaves.childCount; l++)
		{
			ttLeaves.GetChild(l).localScale = Vector3.zero;
		}

		startScaleTop = Vector3.zero;
		startScaleLeaves = new Vector3[ttLeaves.childCount];
		for (int l = 0; l < ttLeaves.childCount; l++)
		{
			startScaleLeaves[l] = Vector3.zero;
		}

	}

	public void StartGrowing()
	{
		active = true;
	}

	// Update is called once per frame
	void Update () {
		if (active)
		{
			if (t < duration)
			{
				// TODO staggered
				float p = (t / duration); // default

				tTrunk.localScale = (1 - p) * startScaleTrunk + p * targetScaleTrunk;
				tTop.localScale = (1 - p) * startScaleTop + p * targetScaleTop;
				for (int l = 0; l < ttLeaves.childCount; l++)
				{
					ttLeaves.GetChild(l).localScale = (1 - p) * startScaleLeaves[l] + p * targetScaleLeaves[l];
				}

				t += Time.deltaTime;
			}

			if (t > duration) {
				active = false;

				tTrunk.localScale = targetScaleTrunk;
				tTop.localScale = targetScaleTop;
				for (int l = 0; l < ttLeaves.childCount; l++)
				{
					ttLeaves.GetChild(l).localScale = targetScaleLeaves[l];
				}
			}
		}
	}
}
