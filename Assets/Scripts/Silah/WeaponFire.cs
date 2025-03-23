using UnityEngine;

public class WeaponFire : MonoBehaviour
{
    public GameObject bulletPrefab; // Mermi prefabı
    public Transform firePoint; // Ateşleme noktası
    public float bulletSpeed = 20f; // Mermi hızı

    public ParticleSystem muzzleFlash;
    public AudioSource fireSound;

    [System.Obsolete]
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Fire();
        }
    }

    [System.Obsolete]
    void Fire()
    {
        // Ateş efekti ve sesi çal
        if (muzzleFlash != null)
            muzzleFlash.Play();
        if (fireSound != null)
            fireSound.Play();

        // Mermi oluştur ve ileriye fırlat
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = firePoint.forward * bulletSpeed;
        }

        // 2 saniye sonra mermiyi yok et (Performans için önemli)
        Destroy(bullet, 2f);
    }
}
