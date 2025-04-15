using UnityEngine;


[RequireComponent(typeof(BoxCollider))]
public class ChoppableTree : MonoBehaviour
{
    public bool playerInRange;
    public bool canBeChopped;

    public float treeMaxHealth;
    public float treeHealth;


    void Start()
    {
        treeHealth = treeMaxHealth;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            playerInRange = true;
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
         if(other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    public void GetHit()
    {
        treeHealth -= 1;
    }
}
