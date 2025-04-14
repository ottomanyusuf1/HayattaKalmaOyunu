using UnityEngine;

public class Village : MonoBehaviour
{
    public Checkpoint reachVillage_Melina;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            reachVillage_Melina.isComplated = true;
        }
    }
}
