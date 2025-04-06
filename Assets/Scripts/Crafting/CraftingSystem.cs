using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingSystem : MonoBehaviour
{
    public GameObject craftingScreenUI;
    public GameObject toolsScreenUI, survivalScreenUI;
    public List<string> inventoryItemList = new List<string>();

    //Category Buttons
    Button toolsBTN, survivalBTN;
    
    //Craft Buttons
    Button craftAxeBTN;
    Button craftSwordBTN;

    //Requirement Text
    Text AxeReq1, AxeReq2;
    Text SwordReq1, SwordReq2;
    public bool isOpen;

    //All Blueprint
    public Blueprint AxeBLP = new Blueprint("Axe",1, 2, "Stone", 3, "Stick", 3);
    public Blueprint SwordBLP = new Blueprint("Sword",1, 2, "Stone", 2, "Iron", 2);




public static CraftingSystem Instance {get; set;}

    private void Awake()
    {
        if(Instance !=null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isOpen =false;

        toolsBTN = craftingScreenUI.transform.Find("ToolsButton").GetComponent<Button>();
        toolsBTN.onClick.AddListener(delegate{OpenToolsCategory();});
        
        survivalBTN = craftingScreenUI.transform.Find("SurvivalButton").GetComponent<Button>();
        survivalBTN.onClick.AddListener(delegate{OpenSurvivalCategory();});
        
        //AXE
        AxeReq1 = toolsScreenUI.transform.Find("Axe").transform.Find("req1").GetComponent<Text>();
        AxeReq2 = toolsScreenUI.transform.Find("Axe").transform.Find("req2").GetComponent<Text>();

        craftAxeBTN = toolsScreenUI.transform.Find("Axe").transform.Find("Button").GetComponent<Button>();
        craftAxeBTN.onClick.AddListener(delegate{CraftAnyItem(AxeBLP);});
        //SWORD
        SwordReq1 = toolsScreenUI.transform.Find("Sword").transform.Find("req1").GetComponent<Text>();
        SwordReq2 = toolsScreenUI.transform.Find("Sword").transform.Find("req2").GetComponent<Text>();

        craftSwordBTN = toolsScreenUI.transform.Find("Sword").transform.Find("Button").GetComponent<Button>();
        craftSwordBTN.onClick.AddListener(delegate{CraftAnyItem(SwordBLP);});

    }

    void OpenToolsCategory()
    {
        survivalScreenUI.SetActive(false);
        craftingScreenUI.SetActive(false);

        toolsScreenUI.SetActive(true);
    }
    void OpenSurvivalCategory()
    {
        toolsScreenUI.SetActive(false);
        craftingScreenUI.SetActive(false);

        survivalScreenUI.SetActive(true);
    }

    void CraftAnyItem(Blueprint blueprintToCraft)
    {      
        SoundManager.Instance.PlaySound(SoundManager.Instance.craftingSound);
        // Produce the amount of items according to the blueprint

        StartCoroutine(craftedDelayForSound(blueprintToCraft));

        if(blueprintToCraft.numberOfItemsToProduce ==1)
        {
            InventorySystem.Instance.AddToInventory(blueprintToCraft.itemName);
        }
        else if(blueprintToCraft.numberOfItemsToProduce ==2)
        {
            InventorySystem.Instance.AddToInventory(blueprintToCraft.itemName);
            InventorySystem.Instance.AddToInventory(blueprintToCraft.itemName);
        }
        

        if(blueprintToCraft.numOfRequirements == 1)
        {
            InventorySystem.Instance.RemoveItem(blueprintToCraft.Req1, blueprintToCraft.Req1amount);
        }
        else if(blueprintToCraft.numOfRequirements == 2)
        {
            InventorySystem.Instance.RemoveItem(blueprintToCraft.Req1, blueprintToCraft.Req1amount);
            InventorySystem.Instance.RemoveItem(blueprintToCraft.Req2, blueprintToCraft.Req2amount);
        }

    StartCoroutine(calculate());

    
    }

    public IEnumerator calculate()
    {
        yield return 0;

        InventorySystem.Instance.ReCalculateList();
        RefleshNeededItems();
    }

    IEnumerator craftedDelayForSound(Blueprint blueprintToCraft)
    {
        yield return new WaitForSeconds(1f);

        // Produce the amount of items according to the blueprint
        for (var i =0; i < blueprintToCraft.numberOfItemsToProduce; i++)
        {
            InventorySystem.Instance.AddToInventory(blueprintToCraft.itemName);
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.C) && !isOpen)
        {
 
		      	Debug.Log("i is pressed");
            craftingScreenUI.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            SelectionManager.Instance.DisableSelection();
            SelectionManager.Instance.GetComponent<SelectionManager>().enabled = false;

            isOpen = true;
 
        }
        else if (Input.GetKeyDown(KeyCode.C) && isOpen)
        {
            craftingScreenUI.SetActive(false);
            toolsScreenUI.SetActive(false);
            survivalScreenUI.SetActive(false);
            if(!InventorySystem.Instance.isOpen)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                SelectionManager.Instance.EnableSelection();
                SelectionManager.Instance.GetComponent<SelectionManager>().enabled = true;
            }
            isOpen = false;
        }
    }
    
    public void RefleshNeededItems()
    {
        int stone_count = 0;
        int stick_count = 0;
        int iron_count = 0;

        inventoryItemList = InventorySystem.Instance.itemList;

        foreach(string ItemName in inventoryItemList)
        {

            switch(ItemName)
            {
                case"Stone":
                    stone_count += 1;
                    break;
                case"Stick":
                    stick_count += 1;
                    break;
                case "Iron":
                    iron_count += 1;
                    break;
            }
        }

        //-----AXE----//

    AxeReq1.text = "3 Stone[" + stone_count +"]";
    AxeReq2.text = "3 Stick[" + stick_count +"]";
    
    if(stone_count >= 3 && stick_count >= 3 && InventorySystem.Instance.CheckSlotsAvailable(1))
    {
        craftAxeBTN.gameObject.SetActive(true);
    }
    else
    {
        craftAxeBTN.gameObject.SetActive(false);
    }
        //-----SWORD-----//
    SwordReq1.text = "2 Stone[" + stone_count + "]";
    SwordReq2.text = "2 Iron[" + iron_count + "]";

    if (stone_count >= 2 && iron_count >= 2 && InventorySystem.Instance.CheckSlotsAvailable(1))
    {
        craftSwordBTN.gameObject.SetActive(true);
    }
    else
    {
        craftSwordBTN.gameObject.SetActive(false);
    }
    
    }



    




}
