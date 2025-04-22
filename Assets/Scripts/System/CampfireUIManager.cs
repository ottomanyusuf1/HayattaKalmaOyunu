using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CampfireUIManager : MonoBehaviour
{
    public static CampfireUIManager Instance {get; set;}

    public Button cookButton;
    public Button exitButton;
    public GameObject foodSlot;
    public GameObject fuelSlot;

    public GameObject campfirePanel;
    public bool isUiOpen;

    public Campfire selectedCampfire;
    public CookingData cookingData;
   private void Awake()
    {
        if (Instance != null && Instance != this)
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
        if (FuelAndFoodAreVali())
            cookButton.interactable = true;
        else
            cookButton.interactable = false;
    }

    private bool FuelAndFoodAreVali()
    {
        InventoryItem fuel = fuelSlot.GetComponentInChildren<InventoryItem>();
        InventoryItem food = foodSlot.GetComponentInChildren<InventoryItem>();


        if (fuel != null && food != null)
        {
            if (cookingData.validFuels.Contains(fuel.thisName)&&
                cookingData.validFoods.Any(cookableFood => cookableFood.name == food.thisName))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        return false;
    }

    public void CookButtonPressed()
    {
        InventoryItem food = foodSlot.GetComponentInChildren<InventoryItem>();
        selectedCampfire.StartCooking(food);

        InventoryItem fuel = fuelSlot.GetComponentInChildren<InventoryItem>();

        Destroy(food.gameObject);
        Destroy(fuel.gameObject);

        ClodeUI();
    }

    public void OpenUI()
    {
        campfirePanel.SetActive(true);
        isUiOpen = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
 
        SelectionManager.Instance.DisableSelection();
        SelectionManager.Instance.GetComponent<SelectionManager>().enabled = false;

        InventorySystem.Instance.OpenUI();
    }

    public void ClodeUI()
    {
        
        campfirePanel.SetActive(false);
        isUiOpen = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
 
        SelectionManager.Instance.EnableSelection();
        SelectionManager.Instance.GetComponent<SelectionManager>().enabled = true;
    }

}
