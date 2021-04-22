using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

//Script over all the inputs for movement, sneak/sprint, change weapons, sight and shoot etc.
public class PlayerInputs : MonoBehaviour
{
    //Events for the sounds
    [SerializeField] private UnityEvent sneak;
    [SerializeField] private UnityEvent walk;
    [SerializeField] private UnityEvent run;
    [SerializeField] private UnityEvent swordSwosh;
    [SerializeField] private UnityEvent shurikenSwosh;

    [SerializeField] private Camera cam;
    [SerializeField] private Transform feet;
    private PlayerStats player;

    [SerializeField] private Text ammotext;

    [SerializeField] private RangedNinjaWeapon ranged;
    [SerializeField] private MeleeNinjaWeapon melee;

    [SerializeField] private float moveForce = 1000;
    private float moveForceNormal;
    [SerializeField] private float jumpForce = 500;
    [SerializeField] private float turnSpeed = 200;
    [SerializeField] private float adjustSpeed = 2.5f;
    private bool jump;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        player = GetComponent<PlayerStats>();
    }

    void Update()
    {
        if (player.IsAlive() && !GlobalVariables.gamepaused && !GlobalVariables.endgame)
        {
            KeyboardInputs();
            MouseInputs();
        }
    }

    void KeyboardInputs()
    {
        //Movement 
        if (Input.GetKey(KeyCode.W))
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                run.Invoke();
            }
            else if (Input.GetKey(KeyCode.LeftControl))
            {
                sneak.Invoke();
            }
            else
            {
                walk.Invoke();
            }
            GetComponent<Rigidbody>().AddForce(transform.forward * moveForce * Time.deltaTime);
        }

        else if (Input.GetKey(KeyCode.S))
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                run.Invoke();
            }
            else if (Input.GetKey(KeyCode.LeftControl))
            {
                sneak.Invoke();
            }
            else
            {
                walk.Invoke();
            }
            GetComponent<Rigidbody>().AddForce(-transform.forward * moveForce * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.A))
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                run.Invoke();
            }
            else if (Input.GetKey(KeyCode.LeftControl))
            {
                sneak.Invoke();
            }
            else
            {
                walk.Invoke();
            }
            GetComponent<Rigidbody>().AddForce(-transform.right * moveForce * Time.deltaTime);
        }

        else if (Input.GetKey(KeyCode.D))
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                run.Invoke();
            }
            else if (Input.GetKey(KeyCode.LeftControl))
            {
                sneak.Invoke();
            }
            else
            {
                walk.Invoke();
            }
            GetComponent<Rigidbody>().AddForce(transform.right * moveForce * Time.deltaTime);
        }

        //Jump
        if (Input.GetKeyDown(KeyCode.Space))
        {
            RaycastHit hit;
            if (Physics.Raycast(feet.position, -transform.up, out hit, 0.1f))
            {
              GetComponent<Rigidbody>().AddForce(transform.up * jumpForce);
            }
        }

        //Weapons
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!GlobalVariables.orbTaken)
            {
                ranged.gameObject.SetActive(!ranged.gameObject.activeSelf);
                ranged.UpdateAmmoText(); //NOT SURE WHY THIS WORKS?
                GlobalVariables.ranged = ranged.gameObject.activeSelf;
                if (GlobalVariables.ranged)
                {
                    melee.gameObject.SetActive(false);
                    ammotext.gameObject.SetActive(true);
                }
                else
                {
                    melee.gameObject.SetActive(true);
                    ammotext.gameObject.SetActive(false);
                }
            }
        }

        //Sneak mode
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            moveForce /= adjustSpeed;
            cam.gameObject.transform.localPosition = new Vector3(0, -0.5f, 0);
            GlobalVariables.sneakMode = true;
        }
        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            moveForce *= adjustSpeed;
            cam.gameObject.transform.localPosition = new Vector3(0, 0.8f, 0);
            GlobalVariables.sneakMode = false;
        }

        //Sprint mode
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            moveForceNormal = moveForce;
            moveForce *= 2f;
            GlobalVariables.sprintMode = true;
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            moveForce = moveForceNormal;
            GlobalVariables.sprintMode = false;
        }
    }

    void MouseInputs()
    {
        if (Input.GetAxis("Mouse X") != 0)
        {
            transform.Rotate(new Vector3(0, Input.GetAxis("Mouse X") * turnSpeed * Time.deltaTime, 0));
        }
        if (Input.GetAxis("Mouse Y") != 0)
        {
            cam.transform.Rotate(new Vector3(-Input.GetAxis("Mouse Y") * turnSpeed * Time.deltaTime, 0, 0));
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (!GlobalVariables.orbTaken)
            {
                if (GlobalVariables.ranged)
                {
                    shurikenSwosh.Invoke();
                    ranged.UseRangedWeapon();
                }
                else
                {
                    swordSwosh.Invoke();
                    melee.UseMeleeWeapon();
                }
            }
        }
    }
    //On Orb is taken event
    public void DisableWeapons()
    {
        ranged.gameObject.SetActive(false);
        melee.gameObject.SetActive(false);
    }



}


