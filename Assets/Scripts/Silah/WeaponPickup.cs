using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    public GameObject weaponModel; // Yerdeki silah modeli
    public Transform weaponHolder; // Oyuncunun silah tutacağı yer
    public KeyCode pickupKey = KeyCode.E; // "E" tuşu ile al
    private bool isInRange = false;

    void Update()
    {
        if (isInRange && Input.GetKeyDown(pickupKey))
        {
            PickUpWeapon();
        }
         if (isInRange && Input.GetKeyDown(pickupKey))
    {
        PickUpWeapon();
    }

    if (Input.GetKeyDown(KeyCode.G)) // G tuşuna basınca bırak
    {
        DropWeapon();
    }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInRange = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInRange = false;
        }
    }

    void PickUpWeapon()
    {
        weaponModel.transform.SetParent(weaponHolder);
        weaponModel.transform.localPosition = Vector3.zero;
        weaponModel.transform.localRotation = Quaternion.identity;
        weaponModel.GetComponent<Rigidbody>().isKinematic = true;
        weaponModel.GetComponent<Collider>().enabled = false;
        Debug.Log("Silah alindi!");
    }
    void DropWeapon()
{
    if (weaponModel.transform.parent == weaponHolder) // Eğer silah tutuluyorsa
    {
        weaponModel.transform.SetParent(null); // Silahı serbest bırak
        Rigidbody rb = weaponModel.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false; // Fizik motorunu aç
            rb.AddForce(weaponHolder.forward * 2, ForceMode.Impulse); // Hafifçe ileri fırlat
        }

        Collider col = weaponModel.GetComponent<Collider>();
        if (col != null)
        {
            col.enabled = true; // Çarpışmaları aç
        }

        Debug.Log("Silah birakildi!");
    }
}
}
