using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwayWithWater : MonoBehaviour {

    // Transforms to act as start and end markers for the journey.
    private Vector3 startMarker;
    private Vector3 endMarker;

    // Movement speed in units/sec.
    public float speed = 1.0F;

    // Time when the movement started.
    private float startTime;

    // Total distance between the markers.
    private float journeyLength;
    
    
    void Start()
    {
        endMarker = transform.position;
    }
    // Update is called once per frame
    void Update () {
        if (transform.position == endMarker)
        {
            startMarker = transform.position;
            endMarker = new Vector3(transform.position.x, Random.Range(-2, 3), transform.position.z);
        journeyLength = Vector3.Distance(startMarker, endMarker);
            startTime = Time.time;
        }
        // Distance moved = time * speed.
        float distCovered = (Time.time - startTime) * speed;

        // Fraction of journey completed = current distance divided by total distance.
        
        float fracJourney = distCovered / journeyLength;
        
        if (journeyLength!=0)
        transform.position = Vector3.Lerp(startMarker, endMarker, fracJourney);

        transform.rotation = new Quaternion(Mathf.Sin(Time.time / 1.7f) / 40f, Mathf.Sin(Time.time / 1.2f) / 30f, Mathf.Sin(Time.time/1.5f)/30f, transform.rotation.w);
    }
}
