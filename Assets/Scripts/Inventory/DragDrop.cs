using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
 
public class DragDrop : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
 
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
 
    public static GameObject itemBeingDragged;
    Vector3 startPosition;
    Transform startParent;
 
 
 
    private void Awake()
    {
        
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
 
    }
 
 
    public void OnBeginDrag(PointerEventData eventData)
    {
 
        Debug.Log("OnBeginDrag");
        canvasGroup.alpha = .6f;
        //So the ray cast will ignore the item itself.
        canvasGroup.blocksRaycasts = false;
        startPosition = transform.position;
        startParent = transform.parent;
        transform.SetParent(transform.root);
        itemBeingDragged = gameObject;
 
    }
 
    public void OnDrag(PointerEventData eventData)
    {
        //So the item will move with our mouse (at same speed)  and so it will be consistant if the canvas has a different scale (other then 1);
        rectTransform.anchoredPosition += eventData.delta;
 
    }

    [System.Obsolete]
    public void OnEndDrag(PointerEventData eventData)
    {
        var tempItemReference = itemBeingDragged;
 
        itemBeingDragged = null;

        // Dragged item outside of inventory
        if (tempItemReference.transform.parent == tempItemReference.transform.root)
        {
            // Hide the icon of the item at this point (Drop into the world)
            tempItemReference.SetActive(false);

            AlertDialogManager dialogManager = FindObjectOfType<AlertDialogManager>();

            dialogManager.ShowDialog("Do you want to drop this item?", (response)=>  
            {
                if (response)
                {
                    DropItemIntoTheWorld(tempItemReference);
                }
                else
                {
                   CancelDragging(tempItemReference);
                }
            
            }); 
        }
        // dropped in the same slot
        if (tempItemReference.transform.parent == startParent)
        {
            CancelDragging(tempItemReference);
        }
        // dropped in another slot
        if (tempItemReference.transform.parent != tempItemReference.transform.root
            && tempItemReference.transform.parent != startParent)
        {
            // another slot did not accepted item (pribably different item or limit exceded fot stack)
            if (tempItemReference.transform.parent.childCount > 2)
            {
                CancelDragging(tempItemReference);
                Debug.Log("Was not accepted into this slot");
            }
            else // ıtem vwas mowed another slot  
            {
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    DivideStack(tempItemReference);
                }
                Debug.Log("Should be moved to another slot");
            }
        }

        Debug.Log("OnEndDrag");
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
    }

    private void DivideStack(GameObject tempItemReference)
    {
        InventoryItem item = tempItemReference.GetComponent<InventoryItem>();
        // check if item/ stack has move than 1 item
        if (item.amountInInventory > 1)
        {
            // seperate 1 item into a different
            item.amountInInventory -= 1;
            InventorySystem.Instance.AddToInventory(item.thisName, false); // false because we dont want to stack item
        }
    }

    void CancelDragging(GameObject tempItemReference)
    {
        transform.position = startPosition;
        transform.SetParent(startParent);

        tempItemReference.SetActive(true);
    }

    [Obsolete]
    private void DropItemIntoTheWorld(GameObject tempItemReference)
    {
        // Get clean name
        string cleanName = tempItemReference.name.Split(new string[]{"(Clone)"}, StringSplitOptions.None)[0];

        // Instantiate item
        GameObject item = Instantiate(Resources.Load<GameObject>(cleanName + "_Model"));

        item.transform.position = Vector3.zero;
        var dropSpawnPosition = PlayerState.Instance.playerBody.transform.Find("DropSpawn").transform.position;
        item.transform.localPosition = new Vector3(dropSpawnPosition.x, dropSpawnPosition.y, dropSpawnPosition.z);

        // Set instantiated item to be the child of [Item] object
        var itemsObject = FindObjectOfType<EnviromentManager>().gameObject.transform.Find("[items]");
        item.transform.SetParent(itemsObject.transform);

        // Delete item from inventory
        DestroyImmediate(tempItemReference.gameObject);
        InventorySystem.Instance.ReCalculateList();
        CraftingSystem.Instance.RefleshNeededItems();
    }
}