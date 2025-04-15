using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class EquipableItem : MonoBehaviour
{

    public Animator animator;
    public bool swingWait = false;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && // left mosue button
        InventorySystem.Instance.isOpen == false && 
        CraftingSystem.Instance.isOpen == false &&
        swingWait == false &&
        SelectionManager.Instance.handIsVisible == false)
        {
            swingWait = true;  
            StartCoroutine(SwingSoundDelay());
            animator.SetTrigger("hit");
            StartCoroutine(NewSwingDelay());
        }
    }

    IEnumerator SwingSoundDelay()
    {
        yield return new WaitForSeconds(0.2f);
        SoundManager.Instance.PlaySound(SoundManager.Instance.toolSwingSound);
    }

    IEnumerator NewSwingDelay()
    {
        yield return new WaitForSeconds(1f);
        swingWait = false;
    }

    void GetHit()
    {
        GameObject selectedTree = SelectionManager.Instance.selectedTree;

            if (selectedTree != null)
            {
                selectedTree.GetComponent<ChoppableTree>().GetHit();
            }
    }
}
