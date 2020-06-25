using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lightning : MonoBehaviour
{
    public float MaxReduction;
    public float MaxIncrease;
    public float RateDamping;
    public float Strength;
    public bool StopFlickering;

    public float TimeMin;
    public float TimeMax;

    public Light[] LightAlternate;


    public void Start()
    {
        StartCoroutine(FlashLight(LightAlternate[0]));
    }
    

    private IEnumerator FlashLight(Light whichFlash)
    {
        float Length = Random.Range(TimeMin, TimeMax);

        for (float i = 0; i < Length; i+= RateDamping)
        {
            whichFlash.intensity = Mathf.Lerp(whichFlash.intensity, Random.Range( MaxReduction, MaxIncrease), Strength * Time.deltaTime);
            yield return new WaitForSeconds(RateDamping);
        }

        while (whichFlash.intensity > 0)
        {
            whichFlash.intensity = Mathf.Lerp(whichFlash.intensity, 0, Strength * Time.deltaTime);
            yield return new WaitForSeconds(RateDamping);
        }

        yield return new WaitForSeconds(Random.Range(2,10));

        StartCoroutine(FlashLight(LightAlternate[Random.Range(0, LightAlternate.Length)]));

    }

}
