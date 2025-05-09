using UnityEngine;
using UnityEngine.UI;

public class HydrationBar : MonoBehaviour
{
    private Slider slider;
    public Text hydrationCounter;

    public GameObject playerState;

    private float currentHydration, maxHydration;
    
    void Awake()
    {
        slider = GetComponent<Slider>();
    }

    
    void Update()
    {
        currentHydration = playerState.GetComponent<PlayerState>().currentHydrationPersent;
        maxHydration = playerState.GetComponent<PlayerState>().maxHydrationPersent;

        float fillValue = currentHydration / maxHydration;
        slider.value = fillValue;

        hydrationCounter.text = currentHydration +"%";
    }
}
