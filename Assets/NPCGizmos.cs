using UnityEngine;

public class NPCGizmos : MonoBehaviour
{
    private void OawGizmos()
    {

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 7f); // Attacking distance

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, 18f); // Star Cheasing Distance

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, 21f); // Stop Chasing Distance
    }
}
