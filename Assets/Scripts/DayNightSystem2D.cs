using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
/*
- Creator:    Two TV Games (@gallighanmaker)
- Script:     Day And Night 2D System
- Unity:      2019 LTS Version
- Email:      leandrovieira92@gmail.com
- Github:     https://github.com/leandrovieiraa
*/

public enum DayCycles // Enum with day and night cycles, you can change or modify with whatever you want
{
    Sunrise = 0,
    Day = 1,
    Sunset = 2,
    Night = 3,
    Midnight = 4
}

public class DayNightSystem2D : MonoBehaviour
{
    [Header("Controllers")]
    
    [Tooltip("Global light 2D component, we need to use this object to place light in all map objects")]
    public UnityEngine.Experimental.Rendering.Universal.Light2D globalLight; // global light
    
    [Tooltip("This is a current cycle time, you can change for private float but we keep public only for debug")]
    public float cycleCurrentTime = 0; // current cycle time
    
    [Tooltip("This is a cycle max time in seconds, if current time reach this value we change the state of the day and night cyles")]
    public float cycleMaxTime = 60; // duration of cycle

    [Tooltip("Enum with multiple day cycles to change over time, you can add more types and modify whatever you want to fits on your project")]
    public DayCycles dayCycle = DayCycles.Sunrise; // default cycle

    [Header("Cycle Colors")]
    
    [Tooltip("Sunrise color, you can adjust based on best color for this cycle")]
    public Color sunrise; // Eg: 6:00 at 10:00
    
    [Tooltip("(Mid) Day color, you can adjust based on best color for this cycle")]
    public Color day; // Eg: 10:00 at 16:00
    
    [Tooltip("Sunset color, you can adjust based on best color for this cycle")]
    public Color sunset; // Eg: 16:00 20:00
    
    [Tooltip("Night color, you can adjust based on best color for this cycle")]
    public Color night; // Eg: 20:00 at 00:00
    
    [Tooltip("Midnight color, you can adjust based on best color for this cycle")]
    public Color midnight; // Eg: 00:00 at 06:00

    public float sunriseIntensity;
    public float dayIntensity;
    public float nightIntensity;
    public float midnightIntensity;

    [Header("Objects")]
    [Tooltip("Objects to turn on and off based on day night cycles, you can use this example for create some custom stuffs")]
    public Light2D[] mapLights; // enable/disable in day/night states
    private List<Color> mapLightsColors = new List<Color>();

    void Start() 
    {
        dayCycle = DayCycles.Night; // start with sunrise state
        globalLight.color = night; // start global color at sunrise
        SFXManager.Instance.PlayAtmosphere();

        foreach (Light2D light in mapLights) {
            if(light.transform.parent.GetComponent<SpriteRenderer>() != null && light.gameObject.name == "LampLight") {
                Color colorToStore = light.transform.parent.GetComponent<SpriteRenderer>().color;
                mapLightsColors.Add(colorToStore);
            }
        }
        ControlLightMaps(false);
    }

     void Update()
     {
        // Update cycle time
        cycleCurrentTime += Time.deltaTime;

        // Check if cycle time reach cycle duration time
        if (((dayCycle == DayCycles.Sunrise || dayCycle == DayCycles.Sunset) && cycleCurrentTime >= cycleMaxTime/2) 
            || (cycleCurrentTime >= cycleMaxTime && (dayCycle != DayCycles.Sunrise || dayCycle != DayCycles.Sunset))) 
        {
            cycleCurrentTime = 0; // back to 0 (restarting cycle time)
            dayCycle++; // change cycle state
            if(dayCycle == DayCycles.Night || dayCycle == DayCycles.Sunrise) {
                SFXManager.Instance.PlayAtmosphere();
            }
        }

        // If reach final state we back to sunrise (Enum id 0)
        if(dayCycle > DayCycles.Midnight) {
            //Show Endgame screen
            dayCycle = 0;
        }
            

        // percent it's a value between current and max time to make a color lerp smooth
        float percent = cycleCurrentTime / cycleMaxTime;

        // Sunrise state (you can do a lot of stuff based on every cycle state, like enable animals only in sunrise )
        if(dayCycle == DayCycles.Sunrise)
        {
            ControlLightMaps(false); // disable map light (keep enable only at night)
            globalLight.color = Color.Lerp(sunrise, day, percent);
            globalLight.intensity = Mathf.Lerp(sunriseIntensity, dayIntensity, percent);

        }

        // Mid Day state
        if(dayCycle == DayCycles.Day) {
            globalLight.color = Color.Lerp(day, sunset, percent);
            globalLight.intensity = Mathf.Lerp(dayIntensity, sunriseIntensity, percent);
    }
            
        // Sunset state
        if (dayCycle == DayCycles.Sunset) {
            globalLight.color = Color.Lerp(sunset, night, percent);
            globalLight.intensity = Mathf.Lerp(sunriseIntensity, nightIntensity, percent);
        }

        // Night state
        if(dayCycle == DayCycles.Night)
        {
            ControlLightMaps(true); // enable map lights (disable only in day states)
            globalLight.color = Color.Lerp(night, midnight, percent);
            globalLight.intensity = Mathf.Lerp(nightIntensity, midnightIntensity, percent);

        }

        // Midnight state
        if (dayCycle == DayCycles.Midnight) {
            globalLight.color = Color.Lerp(midnight, sunrise, percent);
            globalLight.intensity = Mathf.Lerp(midnightIntensity, sunriseIntensity, percent);
        }
    }

     void ControlLightMaps(bool status)
     {
        int i = 0;
        // loop in light array of objects to enable/disable
        if (mapLights.Length > 0)
            foreach(Light2D _light in mapLights) {
                _light.gameObject.SetActive(status);
                if(_light.gameObject.name == "LampLight") {
                    if (status) {
                        _light.transform.parent.GetComponent<SpriteRenderer>().color = mapLightsColors[i];
                    } else {
                        _light.transform.parent.GetComponent<SpriteRenderer>().color = Color.grey;
                    }
                    i++;
                }
            }
     }
}