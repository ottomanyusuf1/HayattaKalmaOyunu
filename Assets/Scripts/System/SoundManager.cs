using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance {get; set;}

    //SOUND FX
    //public AudioSource dropItemSound;

    public AudioSource craftingSound;
    public AudioSource toolSwingSound;
    public AudioSource pickupItemSound;
    public AudioSource grassWalkSound;

    //MUSİC
   // public AudioSource startingZoneBGMusic;

    
    private void Awake()

    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public void PlaySound(AudioSource soundToPlay)
    {
        if(!soundToPlay.isPlaying)
        {
            soundToPlay.Play();
        }
    }
}
