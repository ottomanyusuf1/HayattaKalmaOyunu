using UnityEngine;
using UnityEngine.UI;

public class OxygenBar : MonoBehaviour
{
    private Slider slider;
    public Text oxygenCounter;


    private float currentOxygen, maxOxygen;
    
    void Awake()
    {
        slider = GetComponent<Slider>();
    }

    
    void Update()
    {
        currentOxygen = PlayerState.Instance.currentOxygenPercent;
        maxOxygen = PlayerState.Instance.maxOxygenPercent;

        float fillValue = currentOxygen / maxOxygen;
        slider.value = fillValue;

        oxygenCounter.text = currentOxygen +"%";
    }
}