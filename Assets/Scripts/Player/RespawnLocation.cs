using UnityEngine;

public class RespawnLocation : MonoBehaviour
{
    public bool isRegistered;
    public GameObject indicator;


    void Start()
    {
        PlayerState.Instance.OnRespawnRegistered += UnRegisterLocation;
    }

    private void RegisterLocation()
    {
        PlayerState.Instance.SetRegisteredLocation(this);
        isRegistered = true;
        indicator.SetActive(true);
    }

    private void UnRegisterLocation()
    {
        if (PlayerState.Instance.registeredRespawnLocation != this)
        {
            isRegistered = false;
            indicator.SetActive(false);
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.F))
        {
            if (!isRegistered)
            {
                RegisterLocation();
            }
        }
    }
}
