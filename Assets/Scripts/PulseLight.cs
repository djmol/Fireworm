using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulseLight : MonoBehaviour {

	// Credit to Solomon Lutze
	// http://www.gamasutra.com/blogs/SolomonLutze/20170601/299062/One_Month_Polish_Taking_a_Game_from_Ugly_to_Pretty_As_A_NonArtist.php
	
	public float flickerTime = 1.0f;  // time it takes to go from min to max intensity
    public float initialIntensity = 1.0f; // minimum intensity
    public float targetIntensity = 2.0f; // maximum intensity
    float t = 0f;
    Light light;

    // Use this for initialization
    void Start () {
        light = GetComponent<Light> ();  // assign the light component to a reusable variable
    }

    void Update() {
        light.intensity = Mathf.Lerp(initialIntensity, targetIntensity, t); // set intensity to t * 100% between initial and target intensity;
        t +=  Time.deltaTime / flickerTime ; // increase t
        if (t > 1.0f) // we have completed an intensity cycle
        {
            float temp = targetIntensity; // switch initial and target intensity
            targetIntensity = initialIntensity;
            initialIntensity = temp;
            t = 0.0f; // reset t
        }
    }    
}
