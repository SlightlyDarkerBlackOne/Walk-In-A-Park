using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.LWRP;

public class DayNight : MonoBehaviour
{
    public UnityEngine.Experimental.Rendering.Universal.Light2D sun;
    public float cycleSpeed = 0.1f;

    void Start() {
        sun = GetComponent<UnityEngine.Experimental.Rendering.Universal.Light2D>();
        StartCoroutine(Sunrise());
    }

    IEnumerator Sunset(){
        while(sun.intensity > 0.08f){
            sun.intensity -= 0.001f;
            yield return new WaitForSeconds(cycleSpeed);
        }
        StartCoroutine(Sunrise());
        StopCoroutine(Sunset());
    }

    IEnumerator Sunrise(){
        while(sun.intensity < 0.95f){
            sun.intensity += 0.001f;
            yield return new WaitForSeconds(cycleSpeed);
        }
        StartCoroutine(Sunset());
        StopCoroutine(Sunrise());
    }
}
