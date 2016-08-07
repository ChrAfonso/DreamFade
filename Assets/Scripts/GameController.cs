using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	/* ------ TODO - Game control: ---------
	
		-Start the Game: Click/press something in the title screen

		-Game loop:
		OK Start day 1-5(?)
		OK Timer changes lighting, colors from morning until night
		-- fadeout (Camera effect??), apply color changes to the environment (Fade/Grey out not-awakened areas)
		-- fadein, start a new day (repeat from above)
		-- if after the last day unawakened areas remain: Gameover, the island fades to nothing.
		-- else: Win, make all colors vibrant, play some nice music

		-Player loop:
		-- Find the magic tablet or flowers in each area and bring them to the dying tree. (plant/cast spell)
		-- When all 3(?) are applied there: Tree awakens and blooms, area gets colorful (partial terrain texture??)
		-- Explore the environment, discover ruins, inscriptions, learn about the backstory

	---------------------------------------- */

	// Singleton
	public static GameController instance;

	// Settings
	private int day = 0;
	private int days = 5;

	private float timeOfDay = 6; // start in the morning
	public float hoursPerMinute = 10;

	private static float timeOfDawn = 6;
	private static float timeOfSunrise = 8;
	private static float timeOfSunDown = 18;
	private static float timeOfDusk = 20;
	private static float nightTime = 22; // fadeout here

	// objects
	public Light sun;
	public Camera mainCamera;

	public Color dawnColor;
	public Color sunriseColor;
	public Color noonColor;
	public Color sundownColor;
	public Color duskColor;
	public Color nightColor;

	public bool useSunColorLighing = false;
	private Vector3 sunAngles;

	// game state
	private int numAreas = 3;
	private bool[] areaAwakened;
	private int currentArea = 0; // starting area

	// Use this for initialization
	void Start () {
		instance = this;

		Debug.Log("Starting GameController");
		timeOfDay = timeOfDawn;

		sunAngles = sun.transform.eulerAngles;

		// initial state
		areaAwakened = new bool[numAreas];
		for(int a = 0; a < numAreas; a++) areaAwakened[a] = false;

		// TESTING
		areaAwakened[2] = true;
	}
	
	// Update is called once per frame
	void Update () {
		// User Input - DEBUG
		if(Input.GetKey(KeyCode.F9))
		{
			hoursPerMinute = 50;
		}
		else
		{
			hoursPerMinute = 12; // HACK default
		}

		if(Input.GetKeyDown(KeyCode.N))
		{
			// next day
			EndOfDay();
			return;
		}

		// TESTING!
		if (Input.GetKeyDown(KeyCode.Alpha2)) OnAreaEnter(2);
		else if (Input.GetKeyDown(KeyCode.Alpha1)) OnAreaEnter(1);
		else if (Input.GetKeyDown(KeyCode.Alpha0)) OnAreaEnter(0);

		// Game State
		timeOfDay += (hoursPerMinute * Time.deltaTime / 60);
		//Debug.Log("time of day: " + timeOfDay);

		UpdateLighting();

		if (timeOfDay > nightTime)
		{
			EndOfDay();
		}
	}

	private void UpdateLighting()
	{
		if (useSunColorLighing)
		{
			if (timeOfDay < timeOfSunrise)
			{
				float factor = (timeOfDay - timeOfDawn) / (timeOfSunrise - timeOfDawn);
				sun.color = (1 - factor) * dawnColor + factor * sunriseColor;
			}
			else if (timeOfDay < 12)
			{
				float factor = (timeOfDay - timeOfSunrise) / (13 - timeOfSunrise);
				sun.color = (1 - factor) * sunriseColor + factor * noonColor;
			}
			else if (timeOfDay < timeOfSunDown)
			{
				float factor = (timeOfDay - 13) / (timeOfSunDown - 13);
				sun.color = (1 - factor) * noonColor + factor * sundownColor;
			}
			else if (timeOfDay < timeOfDusk)
			{
				float factor = (timeOfDay - timeOfSunDown) / (timeOfDusk - timeOfSunDown);
				sun.color = (1 - factor) * sundownColor + factor * duskColor;
			}
			else if (timeOfDay < nightTime)
			{
				float factor = (timeOfDay - timeOfDusk) / (nightTime - timeOfDusk);
				sun.color = (1 - factor) * duskColor + factor * nightColor;
			}
			else // nighttime
			{
				sun.color = nightColor;
			}

			// HACK the sky
			mainCamera.backgroundColor = sun.color;
		}

		// direction
		sunAngles.x = (165 - ((timeOfDay - timeOfDawn)/(nightTime - timeOfDawn))*160); // HACK play around with values - original: 185 to -15
		//Debug.Log("sun angle at "+timeOfDay+": " + sunAngles.x);
		sun.transform.eulerAngles = sunAngles;
	}

	private void EndOfDay()
	{
		day++;
		if (day >= days)
		{
			GameOver();
		}
		else // A new day
		{
			// reset clock and degrade visuals
			timeOfDay = timeOfDawn;
			UpdateDaySettings(currentArea);
		}
	}

	public void AwakenArea(int area)
	{
		areaAwakened[area] = true;
		UpdateDaySettings(area, 5f);
		// TODO play music
	}

	public void OnAreaEnter(int area)
	{
		currentArea = area;
		UpdateDaySettings(area);
	}

	private void UpdateDaySettings(int area=-1, float duration=-1)
	{
		float targetSaturation;
		if (area > -1 && areaAwakened[area])
		{
			targetSaturation = 1f;
		}
		else
		{
			targetSaturation = 0.5f - (0.5f * ((float)day / (days - 1))); //Mathf.Pow(1f - ((float)day / (days - 1)), 2);
		}

		Debug.Log("Set target saturation: " + targetSaturation);
		Camera.main.GetComponent<CameraFilter>().SetTargetSaturation(targetSaturation);

		float targetNoise = (float)day/(days-1) * 2f; // TODO reduce in awakened areas?
		Debug.Log("Set target noise: " + targetNoise);
		Camera.main.GetComponent<CameraFilter>().SetTargetNoise(targetNoise);
	}

	private bool CheckGameOver()
	{
		// TODO check if all areas are awakened

		return false;
	}

	void GameOver()
	{
		// TODO
	}

	void Win()
	{
		// TODO
		// play win music
	}
}
