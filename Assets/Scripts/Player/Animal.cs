using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Animal : MonoBehaviour
{
    public string animalName;
    public bool playerInRange;

    [SerializeField] float currentHealt;
    [SerializeField] float maxHealt;

    [Header("Sounds")]
    [SerializeField] AudioSource soundChannel;
    [SerializeField] AudioClip animalHitAndScream;
    [SerializeField] AudioClip animalHitAndDie;
    [SerializeField] AudioClip animalAttack;

    private Animator animator;
    public bool isDead;

    [SerializeField] ParticleSystem bloodSplashParticles;
    public GameObject bloodPuddle;

    public Slider healthBarSlider;
    public event Action OnDestroyed;

    enum AnimalType
    {
        Rabbit,
        Skeleton,
        Bear
    }

    [SerializeField] AnimalType thisAnimalType;

    void Start()
    {
        currentHealt = maxHealt;

        animator = GetComponent<Animator>();

    }

    private void OnDestroy()
    {
        OnDestroyed?.Invoke();
    }
   

    public void TakeDamage(int damage)
    {

        
        if (isDead == false)
        {
            currentHealt -= damage;
            healthBarSlider.value = currentHealt / maxHealt;

            bloodSplashParticles.Play();

            if (currentHealt <= 0)
            {
                PlayDyingSound();
                animator.SetTrigger("DIE");
                // nev mesh agent varsa sil
                 if (thisAnimalType == AnimalType.Rabbit)
                 {
                     GetComponent<AI_Movement>().enabled = true;
                 }
                

                StartCoroutine(PuddleDelay());
                isDead = true;
            }
            else
            {
                PlayHitSound();

                animator.SetTrigger("HURT");
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
        soundChannel.PlayOneShot(animalHitAndDie);
    }

    private void PlayHitSound()
    {
        soundChannel.PlayOneShot(animalHitAndScream);
        // switch (thisAnimalType)
        // {
        //     case AnimalType.Rabbit:
        //         soundChannel.PlayOneShot(animalHitAndScream);
        //         break;
        //     case AnimalType.Bear:
        //        // soundChannel.PlayOneShot(); //Wolf Sound Clip
        //         break;
        //     default:
        //         break;
        // }
    }

    public void PlayAttackSound()
    {
        soundChannel.PlayOneShot(animalAttack);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            playerInRange = true;
            healthBarSlider.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
         if(other.CompareTag("Player"))
        {
            playerInRange = false;
            healthBarSlider.gameObject.SetActive(false);
        }
    }

}
