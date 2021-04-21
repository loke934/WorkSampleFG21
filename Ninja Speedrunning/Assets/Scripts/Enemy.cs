using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

/*Script controls the characteristics of the ground-enemy:
 - Random movement
- Search/dectection method of player
- Chasing player when player found
- Attacking player when player found and in a certain distance from player
-Enemy takes damage
-The lantern lights range and color
    */
public class Enemy : MonoBehaviour
{
    //Move speed
    private float moveSpeed;
    [SerializeField] private float moveSpeedDefault = 2f;
    [SerializeField] private float moveSpeedOrbIsStolen = 5f;
    [SerializeField] private float moveSpeedPlayerDetected = 6.5f;

    //Health.
    [SerializeField] private int health = 100;

    //Movement.
    [SerializeField] private float distanceToNextLocation = 1f;  //If distance to nextLocation is less than this,find a new location to move towards
    private Vector3 nextLocation;
    private Vector3 lastPosition;
    private float turnSpeed = 200f;
    private float timer = 0;
    [SerializeField] private float randomRangeMinInSetNewLocation = -15f;
    [SerializeField] private float randomRangeMaxInSetNewLocation = 15f;

    //Search/Detection of player. 
    //private int viewStage = 0;
    private float FOV; //FOV=field of view
    [SerializeField] private float fovDefault = 60f;
    [SerializeField] private float fovInSprint = 90f;
    [SerializeField] private float fovMax = 110f;
    public bool playerFound = false;
    private float viewDistance;
    [SerializeField] private float viewDistanceDefault = 15f;
    [SerializeField] private float viewDistanceInSneak = 7f;
    [SerializeField] private float viewDistanceInSprint = 20f;

    //ModifyLanternLight, the range and color of the lantern light.
    [SerializeField] private GameObject lantern;
    [SerializeField] private Light lanternlight;

    //EnemyChasePlayer, when player is found.
    private Transform player;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private AudioSource gunshot;

    //EnemyAttack, start shooting at player within a certain distance.
    [SerializeField] private GameObject projectilePrefab;
    private float throwForce = 500f;
    private float shootTimer;
    [SerializeField] private float distanceToStartAttack = 20f;
    [SerializeField] private Transform enemyWeapon;
    [SerializeField] private Transform projectileSpawnpoint;

    //"Hearing" the player when in walk or sprint mode, disabled when player is in sneak.
    [SerializeField] private GameObject hearing;
    private Vector3 normalScale;
    [SerializeField] private int timesBetterHearing = 3;
    private Vector3 betterHearingScale;
    private bool betterHearing = false;

    //Die
    [SerializeField] private AudioSource dieSound;
    [SerializeField] private Animator animator;

    //Damage to player
    [SerializeField] private int dealsDamage = 25;

    void Start()
    {
        moveSpeed = moveSpeedDefault;
        FOV = fovDefault;
        nextLocation = transform.position;
        viewDistance = viewDistanceDefault;
        ModifyLanternLight(FOV, viewDistance, Color.yellow);
    }

    void Update()
    {
        if (health > 0 && !GlobalVariables.gamepaused && !GlobalVariables.endgame)
        {
             SneakOrSprintMode();
            

            if (!playerFound)
            {
                Patrolling();
                SightDetection();
            }
           else if (playerFound)
           {
                if (player == null)
                {
                    player = GlobalVariables.player_reference.transform;
                }
                PlayerFound();
           }

            if (GlobalVariables.allAttack)
            {
                player = GlobalVariables.player_reference.transform;
                playerFound = true;
            }
        }
        //To make enemy stop chasing player when endgame and when they die
        if (GlobalVariables.endgame || health <= 0)
        {
            agent.enabled = false;
        }
    }

    private void SneakOrSprintMode()
    {
        //Sneak mode
        if (GlobalVariables.sneakMode && !GlobalVariables.sprintMode)
        {
            viewDistance = viewDistanceInSneak;
            ModifyLanternLight(FOV, viewDistance, Color.yellow);
            hearing.gameObject.SetActive(false);
        }
        else if (!GlobalVariables.sneakMode && !GlobalVariables.sprintMode)
        {
            viewDistance = viewDistanceDefault;
            ModifyLanternLight(FOV, viewDistance, Color.yellow);
            hearing.gameObject.SetActive(true);
        }
        //Sprint mode
        if (GlobalVariables.sprintMode && !GlobalVariables.sneakMode)
        {
            viewDistance = viewDistanceInSprint;
            FOV = fovInSprint;
            ModifyLanternLight(FOV, viewDistance, Color.yellow);
            if (!betterHearing)
            {
                normalScale = hearing.gameObject.transform.localScale;
                betterHearingScale = normalScale * timesBetterHearing;
                hearing.gameObject.transform.localScale = betterHearingScale;
                betterHearing = true;
            }
        }
        else if (!GlobalVariables.sprintMode && !GlobalVariables.sneakMode)
        {
            viewDistance = viewDistanceDefault;
            FOV = fovDefault;
            ModifyLanternLight(FOV, viewDistance, Color.yellow);
            if (betterHearing)
            {
                hearing.gameObject.transform.localScale = normalScale;
                betterHearing = false;
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (GlobalVariables.sprintMode)
        {
            Gizmos.DrawWireSphere(transform.position,1.5f);
        }
    }

    private void Patrolling()
    {
        animator.SetFloat("Speed", 1f);
        //If distance to nextLocation is less than 1 find a new location to move towards
        if (Vector3.Distance(transform.position, nextLocation) <= distanceToNextLocation)
        {
            SetNewLocation();
        }
        else
        {

            Vector3 direction = (nextLocation - transform.position).normalized;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(direction), turnSpeed * Time.deltaTime);
            transform.Translate(transform.forward * moveSpeed * Time.deltaTime, Space.World);

            //If position been almoast the same 1 sec (enemy is stuck)
            timer += Time.deltaTime;
            if (timer > 1)
            {
                if (Vector3.Distance(lastPosition, transform.position) < 0.5f)
                {
                    SetNewLocation();
                }
                timer = 0;
                lastPosition = transform.position;
            }
        }
    }

    private void SetNewLocation()
    {
        nextLocation = transform.position + new Vector3(Random.Range(randomRangeMinInSetNewLocation, randomRangeMaxInSetNewLocation), 0, Random.Range(randomRangeMinInSetNewLocation, randomRangeMaxInSetNewLocation));
    }

    private void SightDetection()
    {
        RaycastHit hit;
        int rays = 15;
        float angle_between_rays = FOV / rays;
        float starting_angle = -FOV / 2;
        float current_angle;

        for (int i = 0; i < rays; i++)
        {
            current_angle = starting_angle + i * angle_between_rays;
            Vector3 ray_direction = Quaternion.AngleAxis(current_angle, lanternlight.transform.right) * lanternlight.transform.forward;

            if (Physics.Raycast(lanternlight.transform.position, ray_direction, out hit, viewDistance))
            {
                if (hit.transform.GetComponent<PlayerInputs>() != null)
                {
                    player = hit.transform;
                    playerFound = true;
                    return;
                }
            }
            Debug.DrawLine(lanternlight.transform.position, lanternlight.transform.position + viewDistance * ray_direction, Color.red);
        }
        /*
        int rays = 10;
        float view_stage_increase = 0.25f * viewStage;
        float angle = FOV / 2 + view_stage_increase * FOV / rays;
        RaycastHit hit;

        if (viewStage > 0)
        {
            rays -= 1;
        }

        for (int i = 0; i < rays; i++)
        {
            if (viewStage != 0)
            {
                angle = -FOV / 2 + i * FOV / (rays + 1) + view_stage_increase * FOV / (rays + 1);
            }
            else
            {
                angle = -FOV / 2 + i * FOV / rays + view_stage_increase * FOV / rays;
            }

            Vector3 ray_direction = Quaternion.AngleAxis(angle, lantern.transform.up) * lantern.transform.forward;

            if (Physics.Raycast(lantern.transform.position, ray_direction, out hit, viewDistance))
            {
                if (hit.transform.GetComponent<PlayerInputs>() != null)
                {
                    player = hit.transform;
                    playerFound = true;
                    return;
                }
            }

            if (viewStage == 0)
            {
                Debug.DrawLine(lantern.transform.position, lantern.transform.position + viewDistance * ray_direction, Color.red);
            }
            else if (viewStage == 1)
            {
                Debug.DrawLine(lantern.transform.position, lantern.transform.position + viewDistance * ray_direction, Color.blue);
            }
            else if (viewStage == 2)
            {
                Debug.DrawLine(lantern.transform.position, lantern.transform.position + viewDistance * ray_direction, Color.green);
            }
            else if (viewStage == 3)
            {
                Debug.DrawLine(lantern.transform.position, lantern.transform.position + viewDistance * ray_direction, Color.yellow);
            }
        }
        viewStage++;
        if (viewStage >= 4)
        {
            viewStage = 0;
        }*/
    }
    private void PlayerFound()
    {
        animator.SetFloat("Speed",2f);
        GetComponent<Rigidbody>().isKinematic = true;
        moveSpeed = moveSpeedPlayerDetected;
        ModifyLanternLight(fovMax, viewDistance, Color.red);
        EnemyChasePlayer();
        EnemyAttack();
    }

    /*private void EnemyChasePlayer()
    {
        Vector3 direction = (player.transform.position - transform.position).normalized;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(direction), turnSpeed * Time.deltaTime);
        transform.Translate((player.transform.position - transform.position).normalized * moveSpeed * Time.deltaTime, Space.World);
    }*/

    private void EnemyChasePlayer()
    {
        agent.speed = moveSpeedPlayerDetected;
        agent.enabled = true;
        agent.destination = player.position;
    }

    public void EnemyAttack()
    {
        bool shoot = false;
        shootTimer -= Time.deltaTime;
        if (shootTimer <= 0 && Vector3.Distance(transform.position, player.transform.position) <= distanceToStartAttack)
        {
            if (shoot == false)
            {
                //GameObject projectile_prefab = Instantiate(projectilePrefab, projectileSpawnpoint.position, enemyWeapon.rotation);
                var projectilePrefab =  ProjectilePool.GetEnemyProjectile(projectileSpawnpoint.position, enemyWeapon.rotation);
                projectilePrefab.GetComponent<Rigidbody>().AddForce((transform.forward + 0.1f * transform.up) * throwForce);
                gunshot.Play();
                shoot = true;
                shootTimer = 1f;
            }
            else
            {
                shoot = false;
            }
        }
    }
    
    public void EnemyTakeDamage(int dmg)
    {
        health -= dmg;
        if (health <= 0)
        {
            health = 0;
            Die();
        }
    }

    private void Die()
    {
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<CapsuleCollider>().enabled = false;
        animator.SetTrigger("IsDead");
        dieSound.Play();
        lanternlight.enabled = false;
        /*GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationY;*/
    }
    private void ModifyLanternLight(float fov, float range, Color color)
    {
        FOV = fov;
        viewDistance = range;
        lanternlight.color = color;
        lanternlight.spotAngle = fov;
        lanternlight.range = range;
    }

    //When Orb Pick up event
    public void OrbPickedUpEnemy()
    {
       ModifyLanternLight(fovMax, viewDistance, Color.white);
       moveSpeed = moveSpeedOrbIsStolen;
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<PlayerStats>() != null)
        {
            collision.gameObject.GetComponent<PlayerStats>().PlayerTakeDamage(dealsDamage);
            dealsDamage = 0;
        }
    }

}
