using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	/* ------ TODO - Game control: ---------
	
		-Start the Game: Click/press something in the title screen

		-Game loop:
		-- Start day 1-5(?)
		-- Timer changes lighting, colors from morning until night
		-- fadeout (Camera effect??), apply color changes to the environment (Fade/Grey out not-awakened areas)
		-- fadein, start a new day (repeat from above)
		-- if after the last day unawakened areas remain: Gameover, the island fades to nothing.
		-- else: Win, make all colors vibrant, play some nice music

		-Player loop:
		-- Find the magic tablet or flowers in each area and bring them to the dying tree. (plant/cast spell)
		-- When all 3(?) are applied there: Tree awakens and blooms, area gets colorful (partial terrain texture??)
		-- Explore the environment, discover ruins, inscriptions, learn about the backstory

	---------------------------------------- */

	// Settings
	private int day = 0;
	private int days = 5;

	private float timeOfDay = 16; // start in the morning
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


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		timeOfDay += (hoursPerMinute * Time.deltaTime / 60);
		Debug.Log("time of day: " + timeOfDay);

		UpdateLighting();

		if(timeOfDay > nightTime)
		{
			EndOfDay();
		}
	}

	private void UpdateLighting()
	{
		if(timeOfDay < timeOfSunrise)
		{
			float factor = (timeOfDay - timeOfDawn) / (timeOfSunrise - timeOfDawn);
			sun.color = (1 - factor) * dawnColor + factor * sunriseColor;
		}
		else if (timeOfDay < 12)
		{
			float factor = (timeOfDay - timeOfSunrise) / (13 - timeOfSunrise);
			sun.color = (1 - factor) * sunriseColor + factor * noonColor;
		}
		else if(timeOfDay < timeOfSunDown)
		{
			float factor = (timeOfDay - 13) / (timeOfSunDown - 13);
			sun.color = (1 - factor) * noonColor + factor * sundownColor;
		}
		else if (timeOfDay < timeOfDusk)
		{
			float factor = (timeOfDay - timeOfSunDown) / (timeOfDusk - timeOfSunDown);
			sun.color = (1 - factor) * sundownColor + factor * duskColor;
		}
		else if(timeOfDay < nightTime)
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

		// TODO direction?
		Vector3 newAngles = sun.transform.eulerAngles;
		newAngles.x = (180 - ((timeOfDay - timeOfDusk)/(nightTime - timeOfDusk))); // HACK play around with values
		Debug.Log("sun angle: " + newAngles);
		//sun.transform.eulerAngles = newAngles;
	}

	private void EndOfDay()
	{
		day++;
		if(day >= days)
		{
			CheckGameOver();
		}

		// reset clock
		timeOfDay = timeOfDawn;

		// TODO
	}

	private void CheckGameOver()
	{
		// TODO
	}
}
