using System.Collections;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour {
		
	public float accel = 0.5f;
	public float maxVelocity = 5f;

	[System.Flags]
	enum PlayerState {
		Idle = 1,
		Moving = 2,
		Boosting = 4,
	};

	Rigidbody rb;
	float velX = 0f;
	float velY = 0f;
	PlayerState state;
	
	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody>();
		state = PlayerState.Idle;
	}
	
	// Update is called once per frame
	void Update () {

	}
	
	void FixedUpdate () {
		velX = Input.GetAxis("Horizontal") * accel;
		velY = Input.GetAxis("Vertical") * accel;
					
		if (Input.GetAxis("Jump") > 0 && (state & PlayerState.Boosting) != PlayerState.Boosting) {
			// 6/28: Fix this coroutine.
			StartCoroutine(Boost());
		}

		rb.AddForce(new Vector3(velX, velY, 0), ForceMode.Acceleration);

		float vMag = GetComponent<Rigidbody>().velocity.magnitude;
		if(vMag > maxVelocity && (state & PlayerState.Boosting) != PlayerState.Boosting) {
				GetComponent<Rigidbody>().velocity = GetComponent<Rigidbody>().velocity.normalized * maxVelocity;
		}
		
		if (vMag == 0f) {
			state &= ~PlayerState.Moving;
			state |= PlayerState.Idle;
		} else {
			state &= ~PlayerState.Idle;
			state |= PlayerState.Moving;
		}

	}

	IEnumerator Boost() {
		state |= PlayerState.Boosting;
		velX *= 10;
		velY *= 10;
		yield return new WaitForSeconds(1.0f);
	}

	/// <summary>
	/// OnGUI is called for rendering and handling GUI events.
	/// This function can be called multiple times per frame (one call per event).
	/// </summary>
	void OnGUI() {
		GUI.Box(new Rect(5, 5, 100, 35), "X:" + rb.velocity.x + "\nY:" + rb.velocity.y);
		GUI.Box(new Rect(5, 45, 100, 35), state.ToString());
	}
}
