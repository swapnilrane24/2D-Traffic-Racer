/***********************************************************************************************************
 * Produced by Madfireon:               https://www.madfireongames.com/									   *
 * Facebook:                            https://www.facebook.com/madfireon/								   *
 * Contact us:                          https://www.madfireongames.com/contact							   *
 * Madfireon Unity Asset Store catalog: https://bit.ly/2JjKCtw											   *
 * Developed by Swapnil Rane:           https://in.linkedin.com/in/swapnilrane24                           *
 ***********************************************************************************************************/

/***********************************************************************************************************
* NOTE:- This script controls power up shop menu                                                           *
***********************************************************************************************************/

using UnityEngine;
using UnityEngine.UI;

public class PowerUpShop : MonoBehaviour {

    [SerializeField]
    private PowerUpUI powerUpUI;                        //ref to powerup shop UI elements

	// Use this for initialization
	void Start ()
    {
        SetData();                                      //set the powerups data
    }

    void SetData()
    {
        powerUpUI.turboLevelText.text = "Level " + (GameManager.instance.turboUpgrade + 1);                     //set the turbo level
        if (GameManager.instance.turboUpgrade == 4) powerUpUI.turboCostText.text = "Max";                       //if level is equal to 4, set Cost text to MAX
        else powerUpUI.turboCostText.text = "" + 200 * (GameManager.instance.turboUpgrade + 1);                 //else set Cost text to coins of upgrade
        powerUpUI.turboBar.fillAmount = (GameManager.instance.turboUpgrade + 1 )/ 5f;                           //set the turboBar fill amount
        GameManager.instance.turboTime = 8f + (GameManager.instance.turboUpgrade * powerUpUI.turboTimeIncr);    //set the turbo time

        powerUpUI.doubleLevelText.text = "Level " + (GameManager.instance.doubleCoinUpgrade + 1);               //set the double level
        if (GameManager.instance.doubleCoinUpgrade == 4) powerUpUI.doubleCostText.text = "Max";                 //if level is equal to 4, set Cost text to MAX
        else powerUpUI.doubleCostText.text = "" + 200 * (GameManager.instance.doubleCoinUpgrade + 1);           //else set Cost text to coins of upgrade
        powerUpUI.doubleBar.fillAmount = (GameManager.instance.doubleCoinUpgrade + 1) / 5f;                     //set the doubleBar fill amount
        GameManager.instance.doubleCoinTime = 10f + (GameManager.instance.doubleCoinUpgrade * powerUpUI.doubleTimeIncr);    //set the turbo time

        powerUpUI.magnetLevelText.text = "Level " + (GameManager.instance.magnetUpgrade + 1);                   //set the magnet level
        if (GameManager.instance.magnetUpgrade == 4) powerUpUI.magnetCostText.text = "Max";                     //if level is equal to 4, set Cost text to MAX
        else powerUpUI.magnetCostText.text = "" + 200 * (GameManager.instance.magnetUpgrade + 1);               //else set Cost text to coins of upgrade
        powerUpUI.magnetBar.fillAmount = (GameManager.instance.magnetUpgrade + 1 )/ 5f;                         //set the magnetBar fill amount
        GameManager.instance.magnetTime = 10f + (GameManager.instance.magnetUpgrade * powerUpUI.magnetTimeIncr);//set the magnet time
    }

    public void UpgradeTurbo()                                                                                  //method for turbo upgrade buttons
    {
        if (GameManager.instance.turboUpgrade < 4)                                                              //if upgrade is less than 4
        {
            if (GameManager.instance.coinAmount >= 200 * (GameManager.instance.turboUpgrade + 1))               //we check if we have enough coins to upgrade
            {
                GameManager.instance.coinAmount -= 200 * (GameManager.instance.turboUpgrade + 1);               //reduce the coins by upgrade cost
                GameManager.instance.turboUpgrade++;                                                            //increase turbo level
                GameManager.instance.Save();                                                                    //save it

                SetData();                                                                                      //set the data
                GuiManager.instance.UpdateTotalCoins();                                                         //update total coins text
            }
        }
    }

    public void UpgradeDoubleCoin()                                                                             //method for DoubleCoin upgrade buttons
    {
        if (GameManager.instance.doubleCoinUpgrade < 4)
        {
            if (GameManager.instance.coinAmount >= 200 * (GameManager.instance.doubleCoinUpgrade + 1))
            {
                GameManager.instance.coinAmount -= 200 * (GameManager.instance.doubleCoinUpgrade + 1);
                GameManager.instance.doubleCoinUpgrade++;
                GameManager.instance.Save();

                SetData();
                GuiManager.instance.UpdateTotalCoins();
            }
        }
    }

    public void UpgradeMagnet()                                                                                 //method for Magnet upgrade buttons
    {
        if (GameManager.instance.magnetUpgrade < 4)
        {
            if (GameManager.instance.coinAmount >= 200 * (GameManager.instance.magnetUpgrade + 1))
            {
                GameManager.instance.coinAmount -= 200 * (GameManager.instance.magnetUpgrade + 1);
                GameManager.instance.magnetUpgrade++;
                GameManager.instance.Save();

                SetData();
                GuiManager.instance.UpdateTotalCoins();
            }
        }
    }



















    [System.Serializable]
    protected struct PowerUpUI
    {
        public Text turboLevelText, doubleLevelText, magnetLevelText, turboCostText, doubleCostText, magnetCostText;
        public Image turboBar, doubleBar, magnetBar;
        public float turboTimeIncr, doubleTimeIncr, magnetTimeIncr;
    }
}
