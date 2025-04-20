using UnityEngine;

public class SwimArea : MonoBehaviour
{
    public GameObject oxygenBar;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<FPSController>().isSwimming = true;
        }

        if (other.CompareTag("MainCamera"))
        {
            other.GetComponentInParent<FPSController>().isUnderwater = true;
            oxygenBar.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<FPSController>().isSwimming = false;
        }

         if (other.CompareTag("MainCamera"))
        {
            other.GetComponentInParent<FPSController>().isUnderwater = false;
            oxygenBar.SetActive(false);
            // Reset the oxygen
            PlayerState.Instance.currentOxygenPercent = PlayerState.Instance.maxOxygenPercent;
        }
    }
}
