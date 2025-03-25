using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    public SCInventory playerInventory;

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Item"))
        {
            if(playerInventory.AddItem(other.gameObject.GetComponent<Item>().item))
            {
                Destroy(other.gameObject);
            }
            
        }
    }
}
