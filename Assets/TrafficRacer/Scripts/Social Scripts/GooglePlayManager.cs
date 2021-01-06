/***********************************************************************************************************
 * Produced by Madfireon:               https://www.madfireongames.com/									   *
 * Facebook:                            https://www.facebook.com/madfireon/								   *
 * Contact us:                          https://www.madfireongames.com/contact							   *
 * Madfireon Unity Asset Store catalog: https://bit.ly/2JjKCtw											   *
 * Developed by Swapnil Rane:           https://in.linkedin.com/in/swapnilrane24                           *
 ***********************************************************************************************************/

using UnityEngine;
using UnityEngine.SceneManagement;

#if GooglePlayDef
using GooglePlayGames;        
#endif       

using UnityEngine.SocialPlatforms;


public class GooglePlayManager : MonoBehaviour
{
    public static GooglePlayManager singleton;

    private AudioSource sound;

    [HideInInspector]
    public managerVars vars;

    void OnEnable()
    {
        vars = Resources.Load<managerVars>("managerVarsContainer");
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    void Awake()
    {
        MakeInstance();
    }

    void MakeInstance()
    {
        if (singleton != null)
        {
            Destroy(gameObject);
        }
        else
        {
            singleton = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    // Use this for initialization
    void Start()
    {
        sound = GetComponent<AudioSource>();

        #if GooglePlayDef
        PlayGamesPlatform.Activate();
        Social.localUser.Authenticate((bool success) =>
        {
            if (success)
            {
                
            }
        });
        #endif
    }

    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        ReportScore(GameManager.instance.lastDistance);
    }

    public void OpenLeaderboardsScore()
    {
        #if GooglePlayDef
        if (Social.localUser.authenticated)
        {
            PlayGamesPlatform.Instance.ShowLeaderboardUI(vars.leaderBoardID);
        }
        #endif
    }

    void ReportScore(int score)
    {
#if GooglePlayDef
        if (Social.localUser.authenticated)
        {
            Social.ReportScore(score, vars.leaderBoardID, (bool success) => { });
        }
#endif
    }
}
