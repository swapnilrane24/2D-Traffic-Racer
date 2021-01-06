using UnityEngine;

public class managerVars : ScriptableObject {

    // Standart Vars
    [SerializeField]
    public string adMobInterstitialID, adMobBannerID, admobAppID, rateButtonUrl, leaderBoardID, facebookUrl, moregamesUrl;
	[SerializeField]
	public int showInterstitialAfter, bannerAdPoisiton;
    [SerializeField]
    public bool admobActive, googlePlayActive, unityIAP, facebookActive;
}
