using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
   public static Action ShootInput;
   public bool isHeld = false;

    private void Update()
    {
        if (isHeld && Input.GetMouseButton(0))
        {
            ShootInput?.Invoke();
        }
    }
    public void PickupGun()
    {
        isHeld = true; // Silah alındığında çalıştır
    }

    public void DropGun()
    {
        isHeld = false; // Silah bırakıldığında çalıştır
    }


}
