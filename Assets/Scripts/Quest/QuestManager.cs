using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance { get; set; }
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

    public List<Quest> allActiveQuest;
    public List<Quest> allCompletedQuest;

    [Header("QuestMenu")]
    public GameObject questMenu;
    public bool isQuestMenuOpen;

    public GameObject activeQuestPrefab;
    public GameObject complatedQuestPrefab;

    public GameObject questMenuContent;

    [Header("QuestTracker")]
    public GameObject questTrackerContent;
    public GameObject trackerRowPrefab;
    
    public List<Quest> allTrackedQuest;

    public void TrackQuest(Quest quest)
    {
        allTrackedQuest.Add(quest);
        RefleshTrackerList();
    }

    public void unTrackQuest(Quest quest)
    {
        allTrackedQuest.Remove(quest);
        RefleshTrackerList();
    }


    private void RefleshTrackerList()
    {
        
        // Destroying the previous list
        foreach (Transform child in questTrackerContent.transform)
        {
            Destroy(child.gameObject);
        }
 
        foreach (Quest trackedQuest in allTrackedQuest)
        {
            GameObject trackerPrefab = Instantiate(trackerRowPrefab, Vector3.zero, Quaternion.identity);
            trackerPrefab.transform.SetParent(questTrackerContent.transform, false);
 
            TrackerRow tRow = trackerPrefab.GetComponent<TrackerRow>();
 
            tRow.questName.text = trackedQuest.questName;
            tRow.description.text = trackedQuest.questDescription;

            var req1 = trackedQuest.info.firstRequirmentItem;
            var req1Amount =trackedQuest.info.firstRequirementAmount;
            var req2 = trackedQuest.info.secondRequirmentItem;
            var req2Amount = trackedQuest.info.secondRequirementAmount;
 
            if (req2 != "") // if we have 2 requirements
            {
                tRow.requirements.text = $"{req1} " + InventorySystem.Instance.CheckItemAmount(req1) +"/" + $"{req1Amount}\n" +
               $"{req2} " + InventorySystem.Instance.CheckItemAmount(req2) +"/" + $"{req2Amount}";
            }
            else // if we have only one
            {
                tRow.requirements.text = $"{req1} " +  InventorySystem.Instance.CheckItemAmount(req1) +"/" + $"{req1Amount}";
            }

            if (trackedQuest.info.hasCheckpoints)
            {
                var existingText = tRow.requirements.text;
                tRow.requirements.text = PrintCheckPoints(trackedQuest, existingText);
            }
 
 
        }
        
    }

    private string PrintCheckPoints(Quest trackedQuest, string existingText)
    {
        var finalText = existingText;

        foreach (Checkpoint cp in trackedQuest.info.checkpoints)
        {
            if (cp.isComplated)
            {
                finalText = finalText + "\n" + cp.name + "[Complated]";
            }
            else
            {
                finalText = finalText + "\n" + cp.name;
            }
        }
        return finalText;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (!isQuestMenuOpen)
            {
                questMenu.SetActive(true);
                questMenu.GetComponentInParent<Canvas>().sortingOrder = MenuManager.Instance.SetAsFront();

                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;

                SelectionManager.Instance.DisableSelection();
                SelectionManager.Instance.GetComponent<SelectionManager>().enabled = false;

                isQuestMenuOpen = true;
            }
            else
            {
                questMenu.SetActive(false);

                if (!CraftingSystem.Instance.isOpen && !InventorySystem.Instance.isOpen)
                {
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;

                    SelectionManager.Instance.EnableSelection();
                    SelectionManager.Instance.GetComponent<SelectionManager>().enabled = true;
                }

                isQuestMenuOpen = false;
            }
        }
    }


    public void AddAvtiveQuest(Quest quest)
    {
        allActiveQuest.Add(quest);
        TrackQuest(quest);
        RefleshQuestList();
    }

    public void MarkQuestComplated(Quest quest)
    {
        // Remove quest from active list
        allActiveQuest.Remove(quest);
        // Add it into the complated list
        allCompletedQuest.Add(quest);
        unTrackQuest(quest);
        RefleshQuestList();
    }

    
    public void RefleshQuestList()
    {
        foreach (Transform child in questMenuContent.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (Quest activeQuest in allActiveQuest)
        {
            GameObject questPrefab = Instantiate(activeQuestPrefab, Vector3.zero, Quaternion.identity);
            questPrefab.transform.SetParent(questMenuContent.transform, false);

            QuestRow qRow = questPrefab.GetComponent<QuestRow>();

            qRow.thisQuest = activeQuest;

            qRow.questName.text = activeQuest.questName;
            qRow.questGiver.text = activeQuest.questGiver;

            qRow.isActive = true;
            qRow.isTracking = true;

            qRow.coinAmount.text = $"{activeQuest.info.coinReward}";

           if (activeQuest.info.rewardItem1 != "")
            {
                qRow.firstReward.sprite = GetSpriteForitem(activeQuest.info.rewardItem1); ; 
                qRow.firstRewardAmount.text = "";
            }
            else
            {
                qRow.firstReward.gameObject.SetActive(false);
                qRow.firstRewardAmount.text = "";
            }
            if (activeQuest.info.rewardItem2 != "")
            {
                qRow.secondReward.sprite = GetSpriteForitem(activeQuest.info.rewardItem2); ;
                qRow.secondRewardAmount.text = "";
            }
            else
            {
                qRow.secondReward.gameObject.SetActive(false);
                qRow.secondRewardAmount.text = "";
            }
        }

        foreach (Quest complatedQuest in allCompletedQuest)
        {
            GameObject questPrefab = Instantiate(complatedQuestPrefab, Vector3.zero, Quaternion.identity);
            questPrefab.transform.SetParent(questMenuContent.transform, false);

            QuestRow qRow = questPrefab.GetComponent<QuestRow>();

            qRow.questName.text = complatedQuest.questName;
            qRow.questGiver.text = complatedQuest.questGiver;

            qRow.isActive = false;
            qRow.isTracking = false;

            qRow.coinAmount.text = $"{complatedQuest.info.coinReward}";

            if (complatedQuest.info.rewardItem1 != "")
            {
                qRow.firstReward.sprite = GetSpriteForitem(complatedQuest.info.rewardItem1); ; 
                qRow.firstRewardAmount.text = "";
            }
            else
            {
                qRow.firstReward.gameObject.SetActive(false);
                qRow.firstRewardAmount.text = "";
            }
            if (complatedQuest.info.rewardItem2 != "")
            {
                qRow.secondReward.sprite = GetSpriteForitem(complatedQuest.info.rewardItem2); ;
                qRow.secondRewardAmount.text = "";
            }
            else
            {
                qRow.secondReward.gameObject.SetActive(false);
                qRow.secondRewardAmount.text = "";
            }
        }
    }

    private Sprite GetSpriteForitem(string item)
    {
        var itemToGet = Resources.Load<GameObject>(item);
        return itemToGet.GetComponent<Image>().sprite;
    }
}
