using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using TMPro;
using UnityEngine;

public class DayNightSystem : MonoBehaviour
{
    
    public Light directionalLight;

    public float dayDurationInSeconds = 24.0f; // Adjust the duration of a full day in seconds
    public int currentHour;
    float currentTimeOfDay = 0.35f;

    public List<SkyboxTimeMapping> timeMappings;
    float blendedValue = 0.0f;
    bool lockNextDayTrigger = false;
    public TextMeshProUGUI timeUI;
    

    // Update is called once per frame
    void Update()
    {
        // Calculate the current time of day based on the game time
        currentTimeOfDay += Time.deltaTime / dayDurationInSeconds;
        currentTimeOfDay %= 1; // Ensure it stays between 0 and 1;

        currentHour = Mathf.FloorToInt(currentTimeOfDay * 24);

        timeUI.text = $"{currentHour}:00";

        // Update the directional light's rotation
        directionalLight.transform.rotation = Quaternion.Euler(new Vector3((currentTimeOfDay * 360) - 90, 170, 0));

        // Update the skybox material based on the time of day
        UpdateSkybOX();
    }

    private void UpdateSkybOX()
    {
        // Find the appropriate skybox material for the current hour.
        Material currentSkybox = null;
        foreach (SkyboxTimeMapping mapping in timeMappings)
        {
            if (currentHour == mapping.hour)
            {
                currentSkybox = mapping.skyboxMaterial;

                if (currentSkybox.shader != null)
                {
                    if (currentSkybox.shader.name == "Custom/SkyboxTransition")
                    {
                        blendedValue += Time.deltaTime;
                        blendedValue = Mathf.Clamp01(blendedValue);

                        currentSkybox.SetFloat("_TransitionFactor", blendedValue);
                    }
                    else
                    {
                        blendedValue = 0;
                    }
                }
                break;
            }
        }

        if (currentHour == 0 && lockNextDayTrigger == false)
        {
            TimeManager.Instance.TriggerNextDay();
            lockNextDayTrigger = true;
        }

        if (currentHour != 0)
        {
            lockNextDayTrigger = false;
        }


        if (currentSkybox != null)
        {
            RenderSettings.skybox = currentSkybox;
        }
    }
}
[System.Serializable]
public class SkyboxTimeMapping
{
    public string phaseName;
    public int hour; // The hour of the day (0-23)
    public Material skyboxMaterial; // The corresponding skybox material for this hour;
}
