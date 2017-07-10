using System.Collections;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour {
		
	public float accel = 0.5f;
	public float maxVelocity = 5f;
	public float boostDuration = 1f;
	public float boostCooldown = 1f;
	public Material boostMaterial;
	public Light backLight;
	public ParticleSystem psTrail;
	public ParticleSystem psBurst;

	[System.Flags]
	enum PlayerState {
		Idle = 1,
		Moving = 2,
		Boosting = 4,
	};

	PlayerState state;
	Rigidbody rb;
	Renderer rend;
	float velX = 0f;
	float velY = 0f;
	float boostTime;
	Material stdPlayerMaterial;
	Color stdBackLightColor;
	
	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody>();
		rend = GetComponent<Renderer>();
		state = PlayerState.Idle;

		boostTime = boostDuration;
		stdBackLightColor = backLight.color;
		stdPlayerMaterial = rend.material;
	}
	
	// Update is called once per frame
	void Update () {

	}
	
	void FixedUpdate () {
		velX = Input.GetAxis("Horizontal") * accel;
		velY = Input.GetAxis("Vertical") * accel;
					
		if (Input.GetAxis("Jump") > 0 && (state & PlayerState.Boosting) != PlayerState.Boosting) {

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
		psBurst.Emit(10);
		yield return StartBoost();
		FinishBoost();
	}

	IEnumerator StartBoost() {
		stdBackLightColor = backLight.color;
		stdPlayerMaterial = rend.material;

		while (boostTime > 0.0f) {
			state |= PlayerState.Boosting;
			backLight.color = new Color(255/255f, 80/255f, 0/255f, 255/255f);
			rend.material = boostMaterial;
			velX *= 10;
			velY *= 10;
			// Take pausing into consideration if implemented...
			boostTime -= Time.deltaTime;
			Debug.Log(boostTime);
			yield return null;
		}
	}

	void FinishBoost() {
		state &= ~PlayerState.Boosting;
		
		backLight.color = stdBackLightColor;
		rend.material = stdPlayerMaterial;
		boostTime = boostDuration;
		// TODO: boostCooldown
		return;
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
