using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerState : MonoBehaviour
{
    public static PlayerState Instance { get; set;}

    // ------- Player Health -------- //
    public float currentHealth;
    public float maxHealth;
    public bool isPlayerDead;
    public RespawnLocation registeredRespawnLocation;
    public event Action OnRespawnRegistered;


    // ----- Player Calories ------ //
    public float currentCalories;
    public float maxCalories;

    float distanceTravelled = 0;
    Vector3 lastPosition;

    public GameObject playerBody;


    // ------- Player Hydration ----- //
    public float currentHydrationPersent;
    public float maxHydrationPersent;
    public bool isHdrationActive;

    // ------- Player Oxygen ----- //
    public float currentOxygenPercent;
    public float maxOxygenPercent = 100;
    public float oxygenDecreasedPerSecond = 5f;
    private float oxygenTimer = 0f;
    private float decreaseInterval = 1f;
    public float outOfAirDamagePerSecond = 5f;

    public AudioSource playerAudioSource;
    public AudioClip playerPainSound;
    public AudioClip playerDeathSound;

    private float hurtSoundDelay = 2f;
    private float nextHurtTime = 0f;
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        currentHealth = maxHealth;
        currentCalories = maxCalories;
        currentHydrationPersent = maxHydrationPersent;

        currentOxygenPercent = maxOxygenPercent;

        StartCoroutine(decreaseHydration());
    }

        IEnumerator decreaseHydration()
        {
           while(true)
           {
                currentHydrationPersent -=1;
                yield return new WaitForSeconds(10);
           }
    
        }
    // Update is called once per frame
    void Update()
    {

        if (playerBody.GetComponent<FPSController>().isUnderwater)
        {
            oxygenTimer += Time.deltaTime;

            if (oxygenTimer >= decreaseInterval)
            {
                DecreaseOxygen();
                oxygenTimer = 0;
            }
        }

        distanceTravelled += Vector3.Distance(playerBody.transform.position, lastPosition);
        lastPosition = playerBody.transform.position;

        if(distanceTravelled >=5)
        {
            distanceTravelled = 0;
            currentCalories -= 1;
        }





        if(Input.GetKeyDown(KeyCode.N))
        {
            currentHealth -= 10;
        }
    }

    private void DecreaseOxygen()
    {
        currentOxygenPercent -= oxygenDecreasedPerSecond * decreaseInterval;

        if (currentOxygenPercent < 0)
        {
            currentOxygenPercent =0;
            setHealth(currentHealth - outOfAirDamagePerSecond);
        }
    }

    public void setHealth(float newHealth)
    {

        currentHealth = newHealth;
    }

    public void setCalories(float newCalories)
    {
        currentCalories = newCalories;
    }

    public void setHydration(float newHydration)
    {
        currentHydrationPersent = newHydration;
    }
    
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0 && isPlayerDead == false)
        {
            Debug.Log("Player dead");
            PlayerDead();
        }
        else
        {
            if (currentHealth > 0 && Time.time >= nextHurtTime)
            {
                playerAudioSource.PlayOneShot(playerPainSound);
                Debug.Log("Player is hunr");

                nextHurtTime = Time.time + hurtSoundDelay;             
            }
            
        }
    }

    public void PlayerDead()
    {
        isPlayerDead = true;
        playerAudioSource.PlayOneShot(playerDeathSound);

        RespawnPlayer();

    }

    public void RespawnPlayer()
    {
        StartCoroutine(RespawnCoroutine());
    }

    IEnumerator RespawnCoroutine()
    {
        playerBody.GetComponent<FPSController>().enabled = false;

        if (registeredRespawnLocation != null)
        {
            Vector3 position = registeredRespawnLocation.transform.position;

            position.y += 7f;
            position.z += 7f;

            playerBody.transform.position = position;

            currentHealth = maxHealth;
        }

        yield return new WaitForSeconds(0.2f);

        isPlayerDead = false;

        playerBody.GetComponent<FPSController>().enabled = true;
    }

    internal void SetRegisteredLocation(RespawnLocation respawnLocation)
    {
        registeredRespawnLocation = respawnLocation;
        OnRespawnRegistered?.Invoke();
    }



}
