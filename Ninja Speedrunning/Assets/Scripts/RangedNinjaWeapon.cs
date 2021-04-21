using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;

public class RangedNinjaWeapon : MonoBehaviour
{
    public static event Action<int> OnAmmoPickUp;
    [SerializeField] private UnityEvent OnPickUpAmmo;

    [SerializeField] private Text ammoAmount;
    [SerializeField] private GameObject playerProjectile;

    //How much ammo from the beginning
    [SerializeField] private int ammo = 5;
    [SerializeField] private float throwForce = 1000;

    
    public void UseRangedWeapon()
    {
        if (ammo >0 )
        {
            var projectile = ProjectilePool.GetPlayerProjectile(transform.position, transform.rotation);//Instantiate(playerProjectile, transform.position, transform.rotation);
            projectile.GetComponent<Rigidbody>().AddForce((transform.forward + 0.07f * transform.up) * throwForce);
            ammo--;
            UpdateAmmoText();
        }
    }
   
    public void PickUpAmmo(int ammoAmount)
    {
        OnPickUpAmmo.Invoke();
        ammo += ammoAmount;
        UpdateAmmoText();
        OnAmmoPickUp?.Invoke(ammoAmount); 
    }

    public void UpdateAmmoText()
    {
        ammoAmount.text = $"{ammo}";
    }
}
