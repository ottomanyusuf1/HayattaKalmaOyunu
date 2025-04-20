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
    



}
