using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMusic : MonoBehaviour
{
    [SerializeField] private AudioSource caveMusicStart;
    [SerializeField] private AudioSource musicOrbIsTaken;
    [SerializeField] private AudioSource musicAllAttack;
    void Start()
    {
        caveMusicStart.Play();
    }
    private void Update()
    {
        if (GlobalVariables.allAttack)
        {
            AllAttack();
        }
    }
    public void MusicWhenOrbIsTaken()
    {
        if (!GlobalVariables.allAttack)
        {
            musicOrbIsTaken.Play();
        }
       
    }
    public void AllAttack()
    {
        if (!GlobalVariables.endgame)
        {
            caveMusicStart.Stop();
            musicOrbIsTaken.Stop();
            if (!musicAllAttack.isPlaying)
            {
                musicAllAttack.Play();
            }
        } 
    }
    public void EndGame()
    {
        caveMusicStart.Stop();
        musicAllAttack.Stop();
        musicOrbIsTaken.Stop();
    }


}
