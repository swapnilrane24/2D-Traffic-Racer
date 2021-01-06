/***********************************************************************************************************
 * Produced by Madfireon:               https://www.madfireongames.com/									   *
 * Facebook:                            https://www.facebook.com/madfireon/								   *
 * Contact us:                          https://www.madfireongames.com/contact							   *
 * Madfireon Unity Asset Store catalog: https://bit.ly/2JjKCtw											   *
 * Developed by Swapnil Rane:           https://in.linkedin.com/in/swapnilrane24                           *
 ***********************************************************************************************************/

using UnityEngine;

#if UNITY_ADS
using UnityEngine.Advertisements;
#endif

public enum RewardType { coins, reborn }

public class UnityAds : MonoBehaviour
{

    public static UnityAds instance;

    public RewardType rewardType = RewardType.coins;
    private int i = 0;
    [SerializeField]
    private bool rewardAdReady = false;

    [HideInInspector]
    public managerVars vars;

    void OnEnable()
    {
        vars = Resources.Load<managerVars>("managerVarsContainer");
    }

    void Awake()
    {
        if (instance == null)
            instance = this;
    }

    // Use this for initialization
    void Start()
    {
        i = 0;
    }

    public bool RewardAdReady()
    {
#if UNITY_ADS
        if (Advertisement.IsReady("rewardedVideo"))
        {
            rewardAdReady = true;
        }
        else if (!Advertisement.IsReady("rewardedVideo"))
        {
            rewardAdReady = false;
        }
#endif
        return rewardAdReady;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance == null)
            return;

        if (GameManager.instance.gameOver == true)
        {
            //we want only one ad to be shown so we put condition that when i is 0 we show ad.
            if (i == 0)
            {
                i++;
                GameManager.instance.gamesPlayed++;

                if (GameManager.instance.gamesPlayed >= vars.showInterstitialAfter)
                {
                    GameManager.instance.gamesPlayed = 0;
                    //use any one of them
                    //admob ads
#if AdmobDef
                    AdsManager.instance.ShowInterstitial();

#elif UNITY_ADS     //unity ads   
                    if(!vars.admobActive)
                        ShowAd();
#endif
                }
            }
        }
    }

    public void ShowAd()
    {
#if UNITY_ADS
        if (Advertisement.IsReady())
        {
            Advertisement.Show();
        }
#endif
    }

    //use this function for showing reward ads
    public void ShowRewardedAd()
    {
#if UNITY_ADS
        if (Advertisement.IsReady("rewardedVideo"))
        {
            Time.timeScale = 0;
            var options = new ShowOptions { resultCallback = HandleShowResult };
            Advertisement.Show("rewardedVideo", options);
        }
        else
        {
            Debug.Log("Ads not ready");
        }
#endif
    }

#if UNITY_ADS
    private void HandleShowResult(ShowResult result)
    {
        switch (result)
        {
            case ShowResult.Finished:
                Debug.Log("The ad was successfully shown.");

                if (rewardType == RewardType.coins)
                {
                    GameManager.instance.coinAmount += 25;
                    GuiManager.instance.UpdateTotalCoins();
                    SoundManager.instance.PlayFX("CoinEarned");
                    GameManager.instance.Save();
                }
                else if (rewardType == RewardType.reborn)
                {
                    GuiManager.instance.Revive();
                }


                Time.timeScale = 1;
                
                break;
            case ShowResult.Skipped:
                Debug.Log("The ad was skipped before reaching the end.");
                Time.timeScale = 1;
                break;
            case ShowResult.Failed:
                Debug.LogError("The ad failed to be shown.");
                Time.timeScale = 1;
                break;
        }
    }
#endif

}
