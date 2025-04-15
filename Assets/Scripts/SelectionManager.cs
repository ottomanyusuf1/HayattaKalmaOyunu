using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
 
public class SelectionManager : MonoBehaviour
{
    
    public static SelectionManager Instance {get; set;}
    public bool onTarget;
    public GameObject selectedObject;
    public GameObject interaction_Info_UI;
    Text interaction_text;

    public Image centerDotImage;
    public Image handIcon;
    public bool handIsVisible;

    public GameObject selectedTree;
    public GameObject chopHolder;
 
    private void Start()
    {
        onTarget = false; 
        interaction_text = interaction_Info_UI.GetComponent<Text>();
    }

    private void Awake()
    {
        if(Instance !=null && Instance !=this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            var selectionTransform = hit.transform;

            

            NPC npc = selectionTransform.GetComponent<NPC>();

            if(npc && npc.playerInRange)
            {
                interaction_text.text = "Talk";
                interaction_Info_UI.SetActive(true);

                if (Input.GetMouseButton(0) && npc.isTalkingWithPlayer == false)
                {
                    npc.StartConversation();
                }

                if (DialogSystem.Instance.dialogUIActivate)
                {
                    interaction_Info_UI.SetActive(false);
                    centerDotImage.gameObject.SetActive(false);
                }
            }

            InteractableObject interactable = selectionTransform.GetComponent<InteractableObject>();
 
            if (interactable && interactable.playerInRange)
            {
                onTarget = true;
                selectedObject = interactable.gameObject;
                interaction_text.text = interactable.GetItemName();
                interaction_Info_UI.SetActive(true);

                centerDotImage.gameObject.SetActive(false);
                handIcon.gameObject.SetActive(true);

                handIsVisible = true;               
            }

            ChoppableTree choppableTree = hit.transform.GetComponentInParent<ChoppableTree>();


            if (choppableTree && choppableTree.playerInRange)
            {
                choppableTree.canBeChopped = true;
                selectedTree = choppableTree.gameObject;
                chopHolder.gameObject.SetActive(true);
            }
            else
            {
                if (selectedTree != null)
                {
                    selectedTree.gameObject.GetComponent<ChoppableTree>().canBeChopped = false;
                    selectedTree = null;
                    chopHolder.gameObject.SetActive(false);
                }
            }

            Animal animal = selectionTransform.GetComponent<Animal>();

            if (animal && animal.playerInRange)
            {
                if(animal.isDead)
                {
                    interaction_text.text = "Loot";
                    interaction_Info_UI.SetActive(true);

                    centerDotImage.gameObject.SetActive(false);
                    handIcon.gameObject.SetActive(true);

                    handIsVisible = true;

                    if (Input.GetMouseButtonDown(0))
                    {
                        Lootable lootable = animal.GetComponent<Lootable>();
                        Loot(lootable);
                    }
                }
                else
                {
                    interaction_text.text = animal.animalName;
                    interaction_Info_UI.SetActive(true);

                    centerDotImage.gameObject.SetActive(false);
                    handIcon.gameObject.SetActive(false);

                    handIsVisible = false;

                    if (Input.GetMouseButtonDown(0) && EquipSystem.Instance.IsHoldingWeapon() && EquipSystem.Instance.IsThereASwingLock() == false)
                    {
                        StartCoroutine(DealDamageTo(animal, 0.3f, EquipSystem.Instance.GetWeaponDamage()));
                    }
                }
                
            }
            
            if (!interactable && !animal)
            {
                onTarget = false;
                handIsVisible = false;

                centerDotImage.gameObject.SetActive(true);
                handIcon.gameObject.SetActive(false);
            }
            if (!npc && !interactable && !animal && !choppableTree)
            {
                interaction_text.text = "";
                interaction_Info_UI.SetActive(false);
            }

            
        }
        
    }

    private void Loot(Lootable lootable)
    {
        Debug.Log("Loot başlatıldı");

        if (lootable.wasLootCalculated == false)
        {
            List<LootRecieved> recievedLoot = new List<LootRecieved>();

            foreach (LootPossibility loot in lootable.possibleLoot)
            
            // 0 -> 1 (50% drop rate) 1/2 0,1
            // -1 -> 1 (30% drop rate) 1/3  -1, 0, 1
            // -2 -> 1 (25% drop rate) 1/4  -2, -1, 0, 1
            // -3 -> 1 (20% drop rate) 1/5  -3, -2, -1, 0, 1

            // -3 -> 2 (1/6 1/6  1/7) -3, -2, -1, 0, 1,(%17) 2(17%) (%33)

            
            { 
                var lootAmount = UnityEngine.Random.Range(loot.amountMin, loot.amountMax+1);
                if (lootAmount > 0)
                {
                    LootRecieved lt = new LootRecieved();
                    lt.item = loot.item;
                    lt.amount = lootAmount;

                    recievedLoot.Add(lt);
                }
            }
            lootable.finalLoot = recievedLoot;
            lootable.wasLootCalculated = true;
        }

        // Spawning the loot on the ground
        Vector3 lootSpawnPosition = lootable.gameObject.transform.position;

        foreach (LootRecieved lootRecieved in lootable.finalLoot)
        {
            for (int i =0; i< lootRecieved.amount; i++)
            {
                    GameObject lootSpawn = Instantiate(Resources.Load<GameObject>(lootRecieved.item.name+"_Model"),
                    new Vector3(lootSpawnPosition.x, lootSpawnPosition.y+0.2f, lootSpawnPosition.z),
                    Quaternion.Euler(0,0,0));
            }
        }

        // If we want the blood puddle to stay on the ground

        if (lootable.GetComponent<Animal>())
        {
            lootable.GetComponent<Animal>().bloodPuddle.transform.SetParent(lootable.transform.parent);
        }

        // Destroy Lootad body
        Destroy(lootable.gameObject);
    }

    IEnumerator DealDamageTo(Animal animal, float delay, int damage)
    {
        yield return new WaitForSeconds(delay);

        animal.TakeDamage(damage);
    }

    public void DisableSelection()
    {
        handIcon.enabled = false;
        centerDotImage.enabled = false;
        interaction_Info_UI.SetActive(false);

        selectedObject = null;
    }

    public void EnableSelection()
    {
        handIcon.enabled = true;
        centerDotImage.enabled = true;
        interaction_Info_UI.SetActive(true);
    }
}

