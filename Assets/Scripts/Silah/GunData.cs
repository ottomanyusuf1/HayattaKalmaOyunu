using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName ="Gun", menuName ="Weapon/Gun")]
public class GunData : ScriptableObject
{
    [Header("Info")]
    public new string name;
    
    [Header("Shooting")]
    public float damage;
    public float maxDistance; //max mesafe

    [Header("Reloading")]
    public int currentAmmo; //güncel cephane
    public int magSize; //boyut
    public float fireRate;
    public float reloadTime;
    [HideInInspector]
    public bool reloading;
}
