using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

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

	private CameraFilter cameraFilterControl;

	// game state
	private int numAreas = 3;
	private bool[] areaAwakened;
	private int currentArea = 0; // starting area

	private bool endOfDayTriggered = false;

	// Use this for initialization
	void Start () {
		instance = this;

		Debug.Log("Starting GameController");
		timeOfDay = timeOfDawn;

		sunAngles = sun.transform.eulerAngles;

		cameraFilterControl = Camera.main.GetComponent<CameraFilter>();

		// initial state
		areaAwakened = new bool[numAreas];
		for(int a = 0; a < numAreas; a++) areaAwakened[a] = false;

		// TESTING
		areaAwakened[2] = true;
	}
	
	// Update is called once per frame
	void Update () {
		// User Input - DEBUG -----------------------------------
		if(Input.GetKey(KeyCode.F9))
		{
			hoursPerMinute = 50;
		}
		else
		{
			hoursPerMinute = 6; // HACK default
		}

		if(Input.GetKeyDown(KeyCode.N))
		{
			// next day
			EndOfDay();
			return;
		}

		if(Input.GetKeyDown(KeyCode.F))
		{
			cameraFilterControl.FadeOut(3f);
		}
		else if (Input.GetKeyDown(KeyCode.G))
		{
			cameraFilterControl.FadeIn(3f);
		}

		// TESTING!
		if (Input.GetKeyDown(KeyCode.Alpha2)) OnAreaEnter(2);
		else if (Input.GetKeyDown(KeyCode.Alpha1)) OnAreaEnter(1);
		else if (Input.GetKeyDown(KeyCode.Alpha0)) OnAreaEnter(0);
		else if (Input.GetKeyDown(KeyCode.Alpha3)) OnAreaEnter(3);
		else if (Input.GetKeyDown(KeyCode.Alpha4)) OnAreaEnter(4);
		else if (Input.GetKeyDown(KeyCode.Alpha5)) OnAreaEnter(5);
		// User Input - DEBUG -----------------------------------


		// Game State
		timeOfDay += (hoursPerMinute * Time.deltaTime / 60);
		//Debug.Log("time of day: " + timeOfDay);

		UpdateLighting();

		if (timeOfDay > nightTime && !endOfDayTriggered)
		{
			EndOfDay();
		}
	}

	private void UpdateLighting()
	{
		if (useSunColorLighing)
		{
			// TODO: Compress this
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
		// lock player?

		// TODO FadeOut
		cameraFilterControl.FadeOut(2f, "StartNewDay");

		endOfDayTriggered = true;
	}

	// callback
	public void StartNewDay()
	{
		Debug.Log("StartNewDay");
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

		cameraFilterControl.FadeIn(2f, "NewDay");

		endOfDayTriggered = false;
	}

	// callback
	public void NewDay()
	{
		// unlock player?
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
		cameraFilterControl.SetTargetSaturation(targetSaturation);

		float targetNoise = (float)day/(days-1) * 2f; // TODO reduce in awakened areas?
		Debug.Log("Set target noise: " + targetNoise);
		cameraFilterControl.SetTargetNoise(targetNoise);
	}

	void GameOver()
	{
		// TODO
		SceneManager.LoadScene("Credits");
	}

	void Win()
	{
		// TODO
		// play win music
		SceneManager.LoadScene("Credits");
	}
}
