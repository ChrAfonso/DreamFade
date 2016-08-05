using UnityEngine;
using System.Collections;

public class WalkCamera : MonoBehaviour
{

	/**
	 * Writen by Windexglow 11-13-10.  Use it, edit it, steal it I don't care.
	 * Converted to C# 27-02-13 - no credit wanted.
	 * Added resetRotation, RF control, improved initial mouse position, 2015-03-11 - Roi Danton.
	 * Simple flycam I made, since I couldn't find any others made public.
	 * Made simple to use (drag and drop, done) for regular keyboard layout
	 * wasdrf : basic movement
	 * shift : Makes camera accelerate
	 * space : Moves camera on X and Z axis only.  So camera doesn't gain any height
	 */

	float mainSpeed = 10f; // Regular speed.
	float shiftAdd = 25f;  // Multiplied by how long shift is held.  Basically running.
	float maxShift = 100f; // Maximum speed when holding shift.
	float camSens = .35f;  // Camera sensitivity by mouse input.
	public float JumpHeight = 8f;
	private Vector3 lastMouse = new Vector3(Screen.width / 2, Screen.height / 2, 0); // Kind of in the middle of the screen, rather than at the top (play).
	private float totalRun = 1.0f;
	private Rigidbody rb;
	private float distToGround;

	private Transform cameraTransform;
	private Transform feetTransform;

	void Start()
	{

		rb = GetComponent<Rigidbody> ();
		//distToGround = GetComponent<Collider>().bounds.extents.y;

		cameraTransform = transform.FindChild("MainCamera");
		feetTransform = transform.FindChild("Feet");
		distToGround = transform.position.y - feetTransform.position.y;
		Debug.Log("distToGround:" + distToGround);
	}

	void Update()
	{

		// Mouse input.
		Vector3 mouseDiff = Input.mousePosition - lastMouse;
		mouseDiff = new Vector3(mouseDiff.x * camSens, mouseDiff.y * camSens, 0);

		// apply pitch rotation to camera, not player
		cameraTransform.Rotate(Vector3.right, mouseDiff.y);
		transform.Rotate(Vector3.up, mouseDiff.x);

		lastMouse = Input.mousePosition;

		// Keyboard commands.
		Vector3 p = getDirection();
		if (Input.GetKey(KeyCode.LeftShift))
		{
			totalRun += Time.deltaTime;
			p = p * totalRun * shiftAdd;
			p.x = Mathf.Clamp(p.x, -maxShift, maxShift);
			p.y = Mathf.Clamp(p.y, -maxShift, maxShift);
			p.z = Mathf.Clamp(p.z, -maxShift, maxShift);
		}
		else
		{
			totalRun = Mathf.Clamp(totalRun * 0.5f, 1f, 1000f);
			p = p * mainSpeed;
		}

		p = p * Time.deltaTime;
		
		// player wants to move on X and Z axis only
		p.y = 0;

		transform.Translate(p);
		//newPosition.x = transform.position.x;
		//newPosition.z = transform.position.z;
		//newPosition.y = transform.position.y;
		Vector3 newPosition = transform.position;

		// lock to ground
		RaycastHit[] rayHits = Physics.RaycastAll(new Ray(transform.position + new Vector3(0, 1000, 0), Vector3.down)); // HACK: raycast from up high to hit the ground even if player walked through it
		foreach(RaycastHit hit in rayHits)
		{
			if(hit.collider.tag == "Terrain")
			{
				float groundHeight = hit.point.y;
				newPosition.y = groundHeight + distToGround;

				Debug.Log("Hit terrain at " + groundHeight + "!");
				break;
			}
		}

		transform.position = newPosition;


		Jump (); // remove?
	}

	private Vector3 getDirection()
	{
		Vector3 p_Velocity = new Vector3();
		if (Input.GetKey(KeyCode.W))
		{
			p_Velocity += new Vector3(0, 0, 1);
		}
		if (Input.GetKey(KeyCode.S))
		{
			p_Velocity += new Vector3(0, 0, -1);
		}
		if (Input.GetKey(KeyCode.A))
		{
			p_Velocity += new Vector3(-1, 0, 0);
		}
		if (Input.GetKey(KeyCode.D))
		{
			p_Velocity += new Vector3(1, 0, 0);
		}
		return p_Velocity;
	}

	public void resetRotation(Vector3 lookAt)
	{
		transform.LookAt(lookAt);
	}

	void Jump()
	{

		if (Input.GetKeyDown (KeyCode.Space)) {

			Vector3 upwardPos = new Vector3 (0, JumpHeight, 0);
			rb.velocity = upwardPos;

		}

	}

	bool IsGrounded()
	{
		return Physics.Raycast(transform.position, -Vector3.up, distToGround + 0.3f);
	}
}