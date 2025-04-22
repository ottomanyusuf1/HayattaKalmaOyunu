using System;
using UnityEngine;
using UnityEngine.UI;

public class ShopKeeper : MonoBehaviour
{
    public bool playerInRange;
    public bool isTalkingWithPlayer;

    public GameObject shopkepperDialogUI;
    public Button buyBTN;
    public Button sellBTN;
    public Button exitBTN;

    public GameObject buyPanelUI;
    public GameObject sellPanelUI;

    void Start()
    {
        shopkepperDialogUI.SetActive(false);
        buyBTN.onClick.AddListener(BuyMode);
        sellBTN.onClick.AddListener(SellMode);
        exitBTN.onClick.AddListener(StopTalking);
    }

    public void BuyMode()
    {
        sellPanelUI.SetActive(false);
        buyPanelUI.SetActive(true);

        HideDialogUI();
    }
    public void SellMode()
    {
        sellPanelUI.SetActive(true);
        buyPanelUI.SetActive(false);

        HideDialogUI();
    }
    

    public void DialogMode()
    {
        DisplayDialogUI();

        sellPanelUI.SetActive(false);
        buyPanelUI.SetActive(false);
    }

    public void Talk()
    {
        isTalkingWithPlayer = true;
        DisplayDialogUI();

        MovmentManager.Instance.EnableMovement(false);
        MovmentManager.Instance.EnableLook(false);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void StopTalking()
    {
        isTalkingWithPlayer = false;
        HideDialogUI();

        MovmentManager.Instance.EnableMovement(true);
        MovmentManager.Instance.EnableLook(true);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void DisplayDialogUI()
    {
        shopkepperDialogUI.SetActive(true);
    }

    private void HideDialogUI()
    {
        shopkepperDialogUI.SetActive(false);
    }

    #region || --- Ontrigger Events --- ||
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
    #endregion
}
