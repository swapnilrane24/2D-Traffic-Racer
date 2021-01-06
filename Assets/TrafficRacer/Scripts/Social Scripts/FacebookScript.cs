using System.Collections.Generic;
using UnityEngine;
#if FbActive
using Facebook.Unity;
#endif
using System;
using UnityEngine.SceneManagement;

public class FacebookScript : MonoBehaviour {

    public static FacebookScript instance;
    // Custom Share Link
    [SerializeField] private string shareLink = "https://developers.facebook.com/";
    [SerializeField] private string shareTitle = "Link Title";
    [SerializeField] private string shareDescription = "Link Description", facebookPageLink;
    [SerializeField] private string shareImage = "http://i.imgur.com/j4M7vCO.jpg";

    private int rewardCoins;
    private string status = "Ready";
    private string lastResponse = string.Empty;
    private Texture2D profilePic;
    private bool sharingImage = false;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        GetProfilePic();
    }

    private void Awake()
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
        GetProfilePic();

        #if FbActive
        if (FB.IsInitialized == false)
        {
            FB.Init(InitCallback, OnHideUnity);
        }
        else
        {
            FB.ActivateApp();

            if (GameManager.instance.fbConnected == true && FB.IsLoggedIn == false)
            {
                var permissions = new List<string>() { "public_profile", "email"/*, "user_friends" */};
                FB.LogInWithReadPermissions(permissions, AuthCallback);
            }
        }
        #endif

        if (GameManager.instance.shareReward == true)
        {
            GuiManager.instance.facebookElements.shareBtnText.text = "Share with your friends";
        }

        if (GameManager.instance.likeReward == true)
        {
            GuiManager.instance.facebookElements.likeBtnText.text = "Like our official page";
        }
    }

    public void CloseFBPanel()
    {
        GuiManager.instance.facebookElements.fbPanel.SetActive(false);
    }

    public void FacebookLikePage()
    {
        if (GameManager.instance.likeReward == false)
        {
            SetReward(200);
            GameManager.instance.likeReward = true;
            GameManager.instance.Save();
        }
        GuiManager.instance.facebookElements.likeBtnText.text = "Like our official page";
        Application.OpenURL(facebookPageLink); 
    }

    void SetReward(int value)
    {
        GuiManager.instance.facebookElements.coinInfoText.text = value + " coins";
        rewardCoins = value;
        GuiManager.instance.facebookElements.coinRewardPanel.SetActive(true);
    }

    public void CollectCoinsBtn()
    {
        SoundManager.instance.PlayFX("BtnClick");                                                       //play sound
        GameManager.instance.coinAmount += rewardCoins;
        GameManager.instance.Save();
        GuiManager.instance.facebookElements.coinRewardPanel.SetActive(false);
        GuiManager.instance.UpdateTotalCoins();
    }

#region Login Logout
    public void FacebookLogin()
    {
        #if FbActive
        if (FB.IsInitialized == false) return;

        if (FB.IsLoggedIn == false)
        {
            var permissions = new List<string>() { "public_profile", "email"/*, "user_friends" */};
            FB.LogInWithReadPermissions(permissions, AuthCallback);
            sharingImage = false;
        }
        else if (FB.IsLoggedIn)
        {
            GetProfilePic();
            GuiManager.instance.facebookElements.fbPanel.SetActive(true);
        }
        #endif

    }

    public void FacebookLogout()
    {
        #if FbActive
        FB.LogOut();
        #endif
        GameManager.instance.fbConnected = false;
        GameManager.instance.Save();
        GuiManager.instance.facebookElements.fbPanel.SetActive(false);
    }
#endregion

    public void GetProfilePic()
    {
        #if FbActive
        if (FB.IsLoggedIn)
            FB.API("/me/picture", HttpMethod.GET, this.ProfilePhotoCallback);
        #endif
    }

    public void FacebookShareLink()
    {
        #if FbActive
        if (FB.IsInitialized == false) return;
        sharingImage = false;

        if (FB.IsLoggedIn)
            FB.ShareLink(new Uri(this.shareLink), this.shareTitle, this.shareDescription, callback: ShareLinkCallback);

        else if (FB.IsLoggedIn == false)
        {
            var permissions = new List<string>() { "public_profile", "email"/*, "user_friends"*/ };
            FB.LogInWithReadPermissions(permissions, AuthCallback);
        }
        #endif
    }

    public void FacebookShareImage()
    {
        #if FbActive
        if (FB.IsInitialized == false) return;
        sharingImage = true;

        if (FB.IsLoggedIn)
        {
            FB.ShareLink(new Uri(this.shareLink), this.shareTitle, this.shareDescription, new Uri(this.shareImage), callback: ShareImageCallback);
        }

        else if (FB.IsLoggedIn == false)
        {
            var permissions = new List<string>() { "public_profile", "email"/*, "user_friends" */};
            //var publishPerms = new List<string>() { "publish_to_groups", "publish_pages" };
            FB.LogInWithReadPermissions(permissions, AuthCallback);
            //FB.LogInWithPublishPermissions(publishPerms, AuthCallback);
        }
        #endif
    }

#region Inviting

    public void FacebookGameRequest()
    {
        #if FbActive
        FB.AppRequest("Check out this awesome 2D Traffic Racing Game, GUYS!!!", title: shareTitle, callback: GameRquestCallback);
        #endif
    }

#endregion

#region CallBack methods

    private void InitCallback()
    {
        #if FbActive
        if (FB.IsInitialized)
        {
            // Signal an app activation App Event
            FB.ActivateApp();
            // Continue with Facebook SDK
            // ...
            if (GameManager.instance.fbConnected == true && FB.IsLoggedIn == false)
            {
                var permissions = new List<string>() { "public_profile", "email"/*, "user_friends"*/ };
                FB.LogInWithReadPermissions(permissions, AuthCallback);
            }
        }
        else
        {
            Debug.Log("Failed to Initialize the Facebook SDK");
        }
        #endif
    }

    private void OnHideUnity(bool isGameShown)
    {
        if (!isGameShown)
        {
            // Pause the game - we will need to hide
            Time.timeScale = 0;
        }
        else
        {
            // Resume the game - we're getting focus again
            Time.timeScale = 1;
        }
    }

    #if FbActive
    private void AuthCallback(ILoginResult result)
    {
        if (FB.IsLoggedIn)
        {
            // AccessToken class will have session details
            var aToken = AccessToken.CurrentAccessToken;
            // Print current access token's User ID
            Debug.Log(aToken.UserId);
            // Print current access token's granted permissions
            foreach (string perm in aToken.Permissions)
            {
                Debug.Log(perm);
            }

            if (GameManager.instance.fbConnected == false)
            {

                if (sharingImage == false)
                {
                    GetProfilePic();
                    GuiManager.instance.facebookElements.fbPanel.SetActive(true);
                }

                if (GameManager.instance.loginReward == false)
                {
                    SetReward(200);
                    GameManager.instance.loginReward = true;
                }

                GameManager.instance.fbConnected = true;
                GameManager.instance.Save();
            }
            else if (GameManager.instance.fbConnected == true)
            {
                GetProfilePic();
            }


            if (sharingImage == true)
            {
                FacebookShareImage();
            }
        }
        else
        {
            Debug.Log("User cancelled login");
        }
    }

    private void ProfilePhotoCallback(IGraphResult result)
    {
        if (string.IsNullOrEmpty(result.Error) && result.Texture != null)
        {
            profilePic = result.Texture;

            Sprite sprite = Sprite.Create(profilePic, new Rect(0, 0, profilePic.width, profilePic.height), new Vector2(0.5f, 0.5f), 32);

            GuiManager.instance.facebookElements.profileImage.sprite = sprite;
            Debug.Log("Got profile Pic");
        }

        this.HandleResult(result);
    }

    private void ShareImageCallback(IShareResult result)
    {
        if (result.Cancelled || !String.IsNullOrEmpty(result.Error))
        {
            Debug.Log("ShareLink Error: " + result.Error);
        }
        else if (!String.IsNullOrEmpty(result.PostId))
        {
            // Print post identifier of the shared content
            Debug.Log(result.PostId);
        }
        else
        {
            // Share succeeded without postID
            Debug.Log("ShareImage success!");
        }
    }

    private void ShareLinkCallback(IShareResult result)
    {
        if (result.Cancelled || !String.IsNullOrEmpty(result.Error))
        {
            Debug.Log("ShareLink Error: " + result.Error);
        }
        else if (!String.IsNullOrEmpty(result.PostId))
        {
            // Print post identifier of the shared content
            Debug.Log(result.PostId);
        }
        else
        {
            // Share succeeded without postID
            Debug.Log("ShareLink success!");

            if (GameManager.instance.shareReward == false)
            {
                GuiManager.instance.facebookElements.shareBtnText.text = "Share with your friends";
                SetReward(400);
                GameManager.instance.shareReward = true;
                GameManager.instance.Save();
            }
        }
    }

    private void GameRquestCallback(IAppRequestResult result)
    {
        bool requested = false;
        int requestCount = 0;

        if (result == null)
        {
            lastResponse = "Null Response\n";
            Debug.Log(lastResponse);
            return;
        }

        // Some platforms return the empty string instead of null.
        if (!string.IsNullOrEmpty(result.Error))
        {
            status = "Error - Check log for details";
            lastResponse = "Error Response:\n" + result.Error;
        }
        else if (result.Cancelled)
        {
            status = "Cancelled - Check log for details";
            lastResponse = "Cancelled Response:\n" + result.RawResult;
        }
        else if (!string.IsNullOrEmpty(result.RawResult))
        {
            status = "Success - Check log for details";
            lastResponse = "Success Response:\n" + result.RawResult;
            object obj;

            if (result.ResultDictionary.TryGetValue("to", out obj))
            {
                char[] charSeparators = new char[] { ',' };
                string resultIds = (string)obj;
                string[] ids = resultIds.Split(charSeparators);
                Debug.Log(ids.Length);

                if (ids.Length > 0)
                {
                    requested = true;
                    requestCount = ids.Length;
                    SetReward(200 * requestCount);
                }
            }

        }
        else
        {
            lastResponse = "Empty Response\n";
        }

        Debug.Log(result.ToString());

    }

    protected void HandleResult(IResult result)
    {
        if (result == null)
        {
            lastResponse = "Null Response\n";
            Debug.Log(lastResponse);
            return;
        }

        // Some platforms return the empty string instead of null.
        if (!string.IsNullOrEmpty(result.Error))
        {
            status = "Error - Check log for details";
            lastResponse = "Error Response:\n" + result.Error;
        }
        else if (result.Cancelled)
        {
            status = "Cancelled - Check log for details";
            lastResponse = "Cancelled Response:\n" + result.RawResult;
        }
        else if (!string.IsNullOrEmpty(result.RawResult))
        {
            status = "Success - Check log for details";
            lastResponse = "Success Response:\n" + result.RawResult;
        }
        else
        {
            lastResponse = "Empty Response\n";
        }

        Debug.Log(result.ToString());
    }
    #endif
#endregion
}
