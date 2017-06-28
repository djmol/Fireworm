using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	public Transform target; // What camera is following
	public float smoothing; // Dampening effect

	Vector3 offset;
	float lowY; // Lowest point camera will go

	// Use this for initialization
	void Start () {
		offset = transform.position - target.position;
		lowY = transform.position.y - 50f;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		Vector3 targetCamPos = target.position + offset;
		transform.position = Vector3.Lerp (transform.position, targetCamPos, smoothing * Time.deltaTime);
		if (transform.position.y < lowY) {
			transform.position = new Vector3 (transform.position.x, lowY, transform.position.z);
		}
	}
}
