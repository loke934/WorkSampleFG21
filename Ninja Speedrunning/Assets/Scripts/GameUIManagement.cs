using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;

/*The UI
 - Pause menu
- Enter name for leaderboard
-Game over
- Update and pick up ammo text
- Health indication on player
- Timer
*/
public class GameUIManagement : MonoBehaviour
{
    //Pause
    [SerializeField] private Canvas pausecanvas;
    [SerializeField] private Image aim;
    [SerializeField] private Text ammotext;

    //Health
    [SerializeField] private Canvas mediumHealthCanvas;
    [SerializeField] private Canvas lowHealthCanvas;
    [SerializeField] private Canvas soonDeadHealthCanvas;
    [SerializeField] private GameObject player; 
    private int health;

    //Timer
    private float timer;
    [SerializeField] private Text timertext;
    [SerializeField] private AudioSource timerSound;

    //Pick up ammo text
    [SerializeField] private Text pickupammoText;

    //End game
    [SerializeField] private UnityEvent OnEndGame;
    [SerializeField] private Canvas endgameCanvas;
    [SerializeField] private Text gameOverText;
    [SerializeField] private Text winText;

    //Enter name
    [SerializeField] private Text playername;

    //public HighscoreManager highscoremanager;
    private HighscoreManager highscoremanager;
    [SerializeField] private Canvas enterNameCanvas;
    [SerializeField] private Text playerScoreDisplay;

    //Show text how much ammo is picked up
    private Coroutine PickUpAmmoCoroutine;
    [SerializeField] private RectTransform pickupammotextTextbox;
    [SerializeField] private Transform pickupammotextTextboxParent;
    private int copies = 0;
    private float latestPosition;

    private void OnEnable()
    {
       RangedNinjaWeapon.OnAmmoPickUp += RangedNinjaWeapon_OnAmmoPickUp;
    }

    private void OnDisable()
    {
        RangedNinjaWeapon.OnAmmoPickUp -= RangedNinjaWeapon_OnAmmoPickUp;
    }
   
    void Update()
    {
        health = player.GetComponent<PlayerStats>().GetHealth();
        HealthChange();

        if (!GlobalVariables.endgame)
        {
            GameTimer();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseMenu();
        }
    }

    private void FixedUpdate()
    {
        if (GlobalVariables.endgame)
        {
            EndGame();
        }
    }

    private void RangedNinjaWeapon_OnAmmoPickUp(int ammoAmount)
    {
        PickUpAmmoCoroutine = StartCoroutine(PickUpAmmoText(ammoAmount));
    }

    IEnumerator PickUpAmmoText(int ammoAmount)
    {

        float heightPosition = 50f;
        RectTransform pickupammotext = Instantiate(pickupammotextTextbox, pickupammotextTextboxParent);
        pickupammotext.anchoredPosition = pickupammotextTextbox.anchoredPosition + new Vector2(0,latestPosition -heightPosition );
        copies++;
        latestPosition = latestPosition - heightPosition;
        pickupammotext.gameObject.SetActive(true);
        pickupammotext.GetComponent<Text>().text = ($"+ {ammoAmount} shuriken");
        yield return new WaitForSeconds(3);
        Destroy(pickupammotext.gameObject);
        copies--;
        if (copies <= 0)
        {
            latestPosition = 0;
        }
    }

    private void HealthChange()
    {
        if (health <= 150 && health >= 100)
        {
            mediumHealthCanvas.gameObject.SetActive(true);
        }
        else if (health <= 99 && health >= 50)
        {
            mediumHealthCanvas.gameObject.SetActive(false);
            lowHealthCanvas.gameObject.SetActive(true);
        }
        else if(health <=49)
        {
            lowHealthCanvas.gameObject.SetActive(false);
            soonDeadHealthCanvas.gameObject.SetActive(true);
        }
    }

    public void TogglePauseMenu()
    {
        pausecanvas.gameObject.SetActive(!pausecanvas.gameObject.activeSelf);
        GlobalVariables.gamepaused = pausecanvas.gameObject.activeSelf;
        if (GlobalVariables.gamepaused)
        {
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.Confined;
            aim.gameObject.SetActive(false);
            ammotext.gameObject.SetActive(false);
        }
        else
        {
            Time.timeScale = 1;
            Cursor.lockState = CursorLockMode.Locked;
            aim.gameObject.SetActive(true);
            if (GlobalVariables.ranged)
            {
                ammotext.gameObject.SetActive(true);
            }
        }
    }

    public void EnterName()
    {
        PlayerData p = new PlayerData(playername.text, timer);
        highscoremanager.HighscoreAddSortSave(p);
    }

    public void GameTimer()
    {
        if (!GlobalVariables.gamepaused)
        {
           timer += Time.deltaTime;
           float display_timer = timer;
           Math.Round(display_timer,3);
           timertext.text = "Time: " + (display_timer) + " s.";
            if (!timerSound.isPlaying)
            {
                timerSound.Play();
            }
        }
    }

    public void EndGame()
    {
        if (GlobalVariables.victory)
        {
            winText.text = "The orb is retrived, you win!";
            enterNameCanvas.gameObject.SetActive(true);
            highscoremanager = GetComponent<HighscoreManager>();
            highscoremanager.DisplayHighscoresMini();
            Math.Round(timer, 3);
            playerScoreDisplay.text = $"Time: {timer} seconds.";
        }
        else
        {
            gameOverText.text = "Game over!";
            endgameCanvas.gameObject.SetActive(true);
        }

        Cursor.lockState = CursorLockMode.Confined;
        timertext.gameObject.SetActive(false);
        aim.gameObject.SetActive(false);
        OnEndGame.Invoke();
    }

    //Orb is taken event
    public void OrbPickedUpEventUi()
    {
        ammotext.gameObject.SetActive(false);
        aim.gameObject.SetActive(false);
    }

}
