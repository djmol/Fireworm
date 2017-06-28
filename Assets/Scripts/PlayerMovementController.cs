using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour {
		
	public float accel = 0.5f;
	public float maxSpeed = 5f;

	Rigidbody rb;
	float velX = 0f;
	float velY = 0f;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {

	}
	
	void FixedUpdate () {
		// Change all this to AddForce so we can get curving and stuff.

		velX += Input.GetAxisRaw("Horizontal") * accel;
		velY += Input.GetAxisRaw("Vertical") * accel;
		
		if (velX > maxSpeed)
			velX = maxSpeed;
		else if (velX < -maxSpeed)
			velX = -maxSpeed;

		if (velY > maxSpeed)
			velY = maxSpeed;
		else if (velY < -maxSpeed)
			velY = -maxSpeed;
		
		if (Input.GetAxis("Jump") > 0) {
			velX *= 3;
			velY *= 3;
		}

		rb.velocity = new Vector3(velX, velY, 0);

		velX = Mathf.Lerp(velX, 0, Time.deltaTime * .2f);
		velY = Mathf.Lerp(velY, 0, Time.deltaTime * .2f);
	}
}
