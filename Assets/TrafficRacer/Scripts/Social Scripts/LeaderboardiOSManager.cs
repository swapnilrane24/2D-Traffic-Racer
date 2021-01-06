/***********************************************************************************************************
 * Produced by Madfireon:               https://www.madfireongames.com/									   *
 * Facebook:                            https://www.facebook.com/madfireon/								   *
 * Contact us:                          https://www.madfireongames.com/contact							   *
 * Madfireon Unity Asset Store catalog: https://bit.ly/2JjKCtw											   *
 * Developed by Swapnil Rane:           https://in.linkedin.com/in/swapnilrane24                           *
 ***********************************************************************************************************/

using UnityEngine;
#if UNITY_IOS
using UnityEngine.SocialPlatforms;
using UnityEngine.SocialPlatforms.GameCenter;
#endif
using UnityEngine.SceneManagement;

public class LeaderboardiOSManager : MonoBehaviour
{

    public static LeaderboardiOSManager instance;

    private string leaderBoardID;

#region GAME_CENTER	

    /// <summary>
    /// Authenticates to game center.
    /// </summary>

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

    void Start()
    {
        leaderBoardID = vars.leaderBoardID;
        AuthenticateToGameCenter();
    }

    public void AuthenticateToGameCenter()
    {
#if UNITY_IOS
        Social.localUser.Authenticate(success =>
        {
            if (success)
            {
                Debug.Log("Authentication successful");
                
            }
            else
            {
                Debug.Log("Authentication failed");
            }
        });
#endif
    }

    /// <summary>
    /// Reports the score on leaderboard.
    /// </summary>
    /// <param name="score">Score.</param>
    /// <param name="leaderboardID">Leaderboard I.</param>

    public static void ReportScore(long score, string leaderboardID)
    {

#if UNITY_EDITOR

        Debug.Log("Working");

#elif UNITY_IOS
    //Debug.Log("Reporting score " + score + " on leaderboard " + leaderboardID);
    Social.ReportScore(score, leaderboardID, success =>
		{
		if (success)
		{
			Debug.Log("Reported score successfully");
		}
		else
		{
			Debug.Log("Failed to report score");
		}

		Debug.Log(success ? "Reported score successfully" : "Failed to report score"); Debug.Log("New Score:"+score);  
	});
#endif
    }

    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
#if UNITY_IOS
	if (Social.localUser.authenticated == true)
    {
        ReportScore(Mathf.RoundToInt(GameManager.instance.lastDistance), leaderBoardID);
    }
#endif
    }

    /// <summary>
    /// Shows the leaderboard UI.
    /// </summary>

    public void ShowLeaderboard()
    {
#if UNITY_IOS
        Social.ShowLeaderboardUI();
#endif
    }

#endregion
}