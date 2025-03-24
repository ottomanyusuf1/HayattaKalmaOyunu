using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    [SerializeField] private Transform weaponHolder; // Silahı tutacak nokta (Oyuncunun eline takılacak)
    [SerializeField] private GameObject weaponModel; // Silahın modeli
    [SerializeField] private KeyCode pickupKey = KeyCode.E; // Silah alma tuşu (E)
    [SerializeField] private KeyCode dropKey = KeyCode.G; // Silah bırakma tuşu (G)
    
    private bool isInRange = false; // Oyuncu silaha yakın mı?
    
    void Update()
    {
        if (isInRange && Input.GetKeyDown(pickupKey)) // E tuşuna basınca silahı al
        {
            PickUpWeapon();
        }

        if (Input.GetKeyDown(dropKey)) // G tuşuna basınca silahı bırak
        {
            DropWeapon();
        }
    }

    void PickUpWeapon()
    {
        if (weaponModel == null || weaponHolder == null) return; // Silah modeli yoksa çık

        weaponModel.transform.SetParent(weaponHolder); // Silahı oyuncunun eline bağla
        weaponModel.transform.localPosition = Vector3.zero;
        weaponModel.transform.localRotation = Quaternion.identity;

        Rigidbody rb = weaponModel.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true; // Fizik motorunu kapat
            rb.useGravity = false; // Yerçekimini kapat
            rb.linearVelocity = Vector3.zero; // Hızını sıfırla
            rb.angularVelocity = Vector3.zero; // Dönmesini sıfırla
        }

        Collider col = weaponModel.GetComponent<Collider>();
        if (col != null)
        {
            col.enabled = false; // Çarpışmayı kapat
        }

        Debug.Log("Silah alındı!");
    }

   void DropWeapon()
{
    if (weaponModel.transform.parent == weaponHolder) // Eğer silah eldeyse
    {
        weaponModel.transform.SetParent(null); // Silahı serbest bırak
        Rigidbody rb = weaponModel.GetComponent<Rigidbody>();

        if (rb != null) // Rigidbody bileşeni olup olmadığını kontrol et
        {
            rb.isKinematic = false; // Fizik motorunu aç
            rb.useGravity = true;   // Yerçekimini aç
            rb.linearVelocity = Vector3.zero; // Hızını sıfırla (havada uçmasını engeller)
            rb.angularVelocity = Vector3.zero; // Dönmesini engeller
            rb.AddForce(weaponHolder.forward * 2, ForceMode.Impulse); // Hafifçe ileri fırlat
        }
        else
        {
            Debug.LogWarning("Rigidbody bulunamadı! Silah modelinde Rigidbody bileşeni var mı kontrol et.");
        }

        Collider col = weaponModel.GetComponent<Collider>();
        if (col != null)
        {
            col.enabled = true; // Çarpışmaları aç
        }

        Debug.Log("Silah bırakıldı!");
    }
}


    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Eğer oyuncu yaklaştıysa
        {
            isInRange = true;
            Debug.Log("Silahın yanına geldin!");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) // Eğer oyuncu uzaklaştıysa
        {
            isInRange = false;
            Debug.Log("Silahın yanından ayrıldın!");
        }
    }
}
