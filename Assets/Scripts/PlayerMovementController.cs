using System.Collections;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour {
		
	public float accel = 0.5f;
	public float maxVelocity = 5f;
	public float boostDuration = 1f;
	public float boostCooldown = 1f;
	public Material boostMaterial;
	public Light backLight;
	public TrailRenderer boostTrail;
	public ParticleSystem psTrail;
	public ParticleSystem psBurstShower;
	public ParticleSystem psBurstSphere;

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
		yield return StartBoost();
		FinishBoost();
	}

	IEnumerator StartBoost() {
		// Paint player trail while boosting
		boostTrail.gameObject.layer = LayerMask.NameToLayer("Paint");

		// Change player appearance to boost mode
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

		// Set off a 'splosion!
		int psNum = Random.Range(0,2);
		if (psNum == 0) {
			var col = psBurstShower.colorOverLifetime;
			col.color = CreateRandomFireworkGradient();
			psBurstShower.Emit(Random.Range(30,50));
		} else {
			var col = psBurstSphere.colorOverLifetime;
			col.color = CreateRandomFireworkGradient();
			psBurstSphere.Emit(Random.Range(50,80));
		}

		// Stop painting player trail
		boostTrail.gameObject.layer = 0;
		
		// Reset player appearance
		backLight.color = stdBackLightColor;
		rend.material = stdPlayerMaterial;

		boostTime = boostDuration;
		// TODO: boostCooldown
		return;
	}

	Gradient CreateRandomFireworkGradient() {
		Gradient grad = new Gradient();
		Color color = Random.ColorHSV(0f, 1f, 1f, 1f, 1f, 1f, 1f, 1f);
		Color lightColor = color;
		lightColor.a = .8f;
		lightColor.r += .5f;
		lightColor.g += .5f;
		lightColor.b += .5f;
		grad.SetKeys( new GradientColorKey[] {
			new GradientColorKey(lightColor, 0f),
			new GradientColorKey(color, 1f)
			},
			new GradientAlphaKey[] {
				new GradientAlphaKey(.5f, 1f),
				new GradientAlphaKey(1f, 1f)
			}
		);

		return grad;
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
