using System.Collections;
using UnityEngine;

public class Animal : MonoBehaviour
{
    public string animalName;
    public bool playerInRange;

    [SerializeField] int currentHealt;
    [SerializeField] int maxHealt;

    [Header("Sounds")]
    [SerializeField] AudioSource soundChannel;
    [SerializeField] AudioClip rabbitHitAndScream;
    [SerializeField] AudioClip rabbitHitAndDie;

    private Animator animator;
    public bool isDead;

    [SerializeField] ParticleSystem bloodSplashParticles;
    public GameObject bloodPuddle;

    enum AnimalType
    {
        Rabbit,
        Wolf,
        Skeleton,
    }

    [SerializeField] AnimalType thisAnimalType;

    void Start()
    {
        currentHealt = maxHealt;

        animator = GetComponent<Animator>();
    }

    public void TakeDamage(int damage)
    {
        if (isDead == false)
        {
            currentHealt -= damage;

            bloodSplashParticles.Play();

            if (currentHealt <= 0)
            {
                PlayDyingSound();
                animator.SetTrigger("DIE");
                GetComponent<AI_Movement>().enabled = false;

                StartCoroutine(PuddleDelay());
                isDead = true;
            }
            else
            {
                PlayHitSound();
            }
        }
    }

    IEnumerator PuddleDelay()
    {
        yield return new WaitForSeconds(1f);
        bloodPuddle.SetActive(true);
    }

    private void PlayDyingSound()
    {
        switch (thisAnimalType)
        {
            case AnimalType.Rabbit:
                soundChannel.PlayOneShot(rabbitHitAndDie);
                break;
            case AnimalType.Wolf:
               // soundChannel.PlayOneShot(); //Wolf Sound Clip
                break;
            default:
                break;
        }
    }

    private void PlayHitSound()
    {
        switch (thisAnimalType)
        {
            case AnimalType.Rabbit:
                soundChannel.PlayOneShot(rabbitHitAndScream);
                break;
            case AnimalType.Wolf:
               // soundChannel.PlayOneShot(); //Wolf Sound Clip
                break;
            default:
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
         if(other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

}
