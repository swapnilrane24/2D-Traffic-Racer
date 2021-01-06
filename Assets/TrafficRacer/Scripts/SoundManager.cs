/***********************************************************************************************************
 * Produced by Madfireon:               https://www.madfireongames.com/									   *
 * Facebook:                            https://www.facebook.com/madfireon/								   *
 * Contact us:                          https://www.madfireongames.com/contact							   *
 * Madfireon Unity Asset Store catalog: https://bit.ly/2JjKCtw											   *
 * Developed by Swapnil Rane:           https://in.linkedin.com/in/swapnilrane24                           *
 ***********************************************************************************************************/

/***********************************************************************************************************
* NOTE:- This script controls sound effect and background music                                            *
***********************************************************************************************************/

using UnityEngine;

public class SoundManager : MonoBehaviour
{

    public static SoundManager instance;

    [SerializeField] private AudioSource musicSource, fxSource, narrationSource;    //ref to audiosources

    [SerializeField] private BackgroundMusic backgroundMusic;       //backgroundmusic variable
    [SerializeField] private FxEffects fxEffects;                   //fx of btn , slide , etc
    [SerializeField] private CarFx carFx;                           //car fx
    [SerializeField] private NarrationFX[] narrationFxs;            //narration fx

    int r;                                                          //for narration fx selection

    //Make it a singleton
    void Awake()
    {
        if (instance != null)                                       
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        PlayMenuMusic();
    }

    public void PlayFX(string value)                                //method which take string as input for playing UI fx
    {
        if (value == "BtnClick")
        {
            fxSource.PlayOneShot(fxEffects.buttonClickFx);
        }
        else if (value == "PanelSlide")
        {
            fxSource.PlayOneShot(fxEffects.panelSlideFx);
        }
        else if (value == "FireWork")
        {
            fxSource.PlayOneShot(fxEffects.fireWorkFx);
        }
        else if (value == "CoinEarned")
        {
            fxSource.PlayOneShot(fxEffects.coinEarnedFx);
        }
        else if (value == "CD1")
        {
            fxSource.PlayOneShot(fxEffects.countDown1Fx);
        }
        else if (value == "CD2")
        {
            fxSource.PlayOneShot(fxEffects.countDown2Fx);
        }
        else if (value == "CD3")
        {
            fxSource.PlayOneShot(fxEffects.countDown3Fx);
        }
        else if (value == "CDGO")
        {
            fxSource.PlayOneShot(fxEffects.countDownGoFx);
        }
    }

    public void PlayNarrationFX(string value)
    {
        foreach (NarrationFX item in narrationFxs)                  //loop through all the items in the Array
        {
            if (item.name == value)                                 //if any item have same name as required value
            {
                r = Random.Range(0, item.audioClips.Length);        //get random umber between zero and total audioclips in that item
                narrationSource.PlayOneShot(item.audioClips[r]);    //play the random audio
                return;                                             //return
            }
        }
    }

    public void CarFX(string value)                                 //method which take string as input for playing car fx
    {
        if (value == "Coin")
        {
            fxSource.PlayOneShot(carFx.coinPickUpFx);
        }
        else if (value == "Bolt")
        {
            fxSource.PlayOneShot(carFx.boltPickUpFx);
        }
        if (value == "Magnet")
        {
            fxSource.PlayOneShot(carFx.magnetPickUpFx);
        }
        if (value == "Shield")
        {
            fxSource.PlayOneShot(carFx.shieldPickUpFx);
        }
        if (value == "Fuel")
        {
            fxSource.PlayOneShot(carFx.fuelPickUpFx);
        }
        else if (value == "Explosion")
        {
            fxSource.PlayOneShot(carFx.explosionFx);
        }
    }

    public void PlayMenuMusic()                                     //method called to play menu music
    {
        musicSource.Stop();                                         //stop the current music playing
        int r = Random.Range(0, backgroundMusic.menuMusics.Length); //get random number
        musicSource.clip = backgroundMusic.menuMusics[r];           //select clip depending on random number
        musicSource.Play();                                         //play
    }

    public void PlayGameMusic()                                     //method called to play game music
    {
        musicSource.Stop();                                         //stop the current music playing
        int r = Random.Range(0, backgroundMusic.gameMusic.Length);  //get random number
        musicSource.clip = backgroundMusic.gameMusic[r];            //select clip depending on random number
        musicSource.Play();                                         //play
    }


    #region Structs
    [System.Serializable]
    public struct FxEffects
    {
        public AudioClip buttonClickFx, panelSlideFx, fireWorkFx, coinEarnedFx ,countDown1Fx, countDown2Fx, countDown3Fx, countDownGoFx;    //ref to audioclips
    }

    [System.Serializable]
    public struct CarFx
    {
        public AudioClip coinPickUpFx, boltPickUpFx, shieldPickUpFx, magnetPickUpFx, fuelPickUpFx, explosionFx;    //ref to audioclips
    }

    [System.Serializable]
    public struct BackgroundMusic
    {
        public AudioClip[] menuMusics, gameMusic;   
    }

    [System.Serializable]
    public struct NarrationFX
    {
        public string name;                                     //name of fx
        public AudioClip[] audioClips;                          //ref to audioclips
    }
    #endregion
}