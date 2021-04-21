using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    [SerializeField] private AudioSource sneak;
    [SerializeField] private AudioSource walk;
    [SerializeField] private AudioSource run;
    [SerializeField] private AudioSource swordSwosh;
    [SerializeField] private AudioSource shurikenSwosh;
    [SerializeField] private AudioSource pickUpAmmo;

    //Hard coded values set between the min/max in inspector
    public void Sneak()
    {
        if (!sneak.isPlaying)
        {
            sneak.volume = Random.Range(0.5f, 0.1f);
            sneak.pitch = Random.Range(0.5f, 1f);
            sneak.Play();
        }
    }

    public void Walk()
    {
        if (!walk.isPlaying)
        {
            walk.volume = Random.Range(0.15f, 0.25f);
            walk.pitch = Random.Range(0.5f, 0.8f);
            walk.Play();
        }
    }

    public void Run()
    {
        if (!run.isPlaying)
        {
            run.volume = Random.Range(0.25f, 0.5f);
            run.pitch = Random.Range(0.8f, 1f);
            run.Play();
        }
    }

    public void SwordSwosh()
    {
        swordSwosh.pitch = Random.Range(0.8f, 1f);
        swordSwosh.Play();
    }

    public void ShurikenSwosh()
    {
        shurikenSwosh.Play();
    }

    public void PickUpAmmoSound()
    {
        pickUpAmmo.Play();
    }

}
