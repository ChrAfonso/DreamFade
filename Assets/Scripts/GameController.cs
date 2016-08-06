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

	private float timeOfDay = 6; // start in the morning
	public static float hoursPerMinute = 2;

	private static float timeOfDawn = 6;
	private static float timeOfDusk = 18;
	private static float nightTime = 20; // fadeout here


	// objects
	public Light sun;
	public Camera mainCamera;

	public Color dawnColor;
	public Color noonColor;
	public Color duskColor;
	public Color nightColor;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		timeOfDay += (hoursPerMinute * Time.deltaTime / 60);

		UpdateLighting();

		if(timeOfDay > nightTime)
		{
			EndOfDay();
		}
	}

	private void UpdateLighting()
	{
		if(timeOfDay < 12)
		{
			float factor = (timeOfDay - timeOfDawn) / (12 - timeOfDawn);
			sun.color = (1 - factor) * dawnColor + factor * noonColor;
		}
		else if(timeOfDay < timeOfDusk)
		{
			float factor = (timeOfDay - 12) / (timeOfDusk - 12);
			sun.color = (1 - factor) * noonColor + factor * duskColor;
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
		sun.transform.eulerAngles = newAngles;
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
