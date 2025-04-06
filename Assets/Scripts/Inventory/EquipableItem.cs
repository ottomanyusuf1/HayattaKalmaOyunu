using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class EquipableItem : MonoBehaviour
{

public Animator animator;
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
        SelectionManager.Instance.handIsVisible == false)
        {
            StartCoroutine(SwingSoundDelay());
            animator.SetTrigger("hit");
        }
    }

    IEnumerator SwingSoundDelay()
    {
        yield return new WaitForSeconds(0.2f);
        SoundManager.Instance.PlaySound(SoundManager.Instance.toolSwingSound);
    }
}
