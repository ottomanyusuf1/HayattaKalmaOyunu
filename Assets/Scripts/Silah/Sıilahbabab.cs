using UnityEngine;

public class Gun : MonoBehaviour
{
    [Header("References")]
    [SerializeField] GunData gunData;

    private void Start()
    {
        PlayerShoot.ShootInput += Shoot;
    }


    public void Shoot()
    {
        Debug.Log("Silah Ateşledi!");
    }
}

