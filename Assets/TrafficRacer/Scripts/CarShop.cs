/***********************************************************************************************************
 * Produced by Madfireon:               https://www.madfireongames.com/									   *
 * Facebook:                            https://www.facebook.com/madfireon/								   *
 * Contact us:                          https://www.madfireongames.com/contact							   *
 * Madfireon Unity Asset Store catalog: https://bit.ly/2JjKCtw											   *
 * Developed by Swapnil Rane:           https://in.linkedin.com/in/swapnilrane24                           *
 ***********************************************************************************************************/

/***********************************************************************************************************
* NOTE:- This script controls car shop menu                                                                *
***********************************************************************************************************/

using UnityEngine;
using UnityEngine.UI;

public class CarShop : MonoBehaviour {

    public static CarShop instance;

    [SerializeField] [Header("Car Shop UI Elements")]
    private CarShopUI carShopUI;
    [SerializeField] [Header("-----------------------")] [Header("Car Data")]
    private CarData[] carDatas;

    private int currentIndex = 0;

    // Use this for initialization
    void Awake()
    {
        if (instance == null)
            instance = this;
    }

    private void Start()
    {
        SetGameData();
    }

    public void InitializeCarShop()
    {
        SetCarDetails();
    }

    void SetGameData()
    {
        //here we set the gamemanagers values as per our selected car
        GameManager.instance.normalSpeed        = carDatas[GameManager.instance.selectedCar].carSpeed;
        GameManager.instance.turboSpeed         = carDatas[GameManager.instance.selectedCar].carTurboSpeed;
        GameManager.instance.fuel               = carDatas[GameManager.instance.selectedCar].carFuel;
        GuiManager.instance.DistanceMultiplier  = GameManager.instance.normalSpeed;
    }

    void SetCarDetails()                                                                        //method which set the car details in the car shop
    {
        SoundManager.instance.PlayFX("BtnClick");                                                       //play sound
        carShopUI.carNameText.text      = carDatas[currentIndex].carName;                       //set car name text
        carDatas[currentIndex].carLevel = GameManager.instance.carDatas[currentIndex].carLevel; //set car level
        if (GameManager.instance.carDatas[currentIndex].unlocked == true)                       //if selected car is unlocked
        {           
            if (GameManager.instance.selectedCar == currentIndex)                               //if its selected car
                carShopUI.selectText.text = "Selected";                                         //set Select Btn text to Selected
            else if (GameManager.instance.selectedCar != currentIndex)                          //if iits not the selected car
                carShopUI.selectText.text = "Select";                                           //set Select Btn text to Select

            if (GameManager.instance.carDatas[currentIndex].carLevel >= 3)                      //if car level is more than 3
            {
                carShopUI.upgradeInfoText.text = "Upgrade maxed";                               //set the text
                carShopUI.upgradeCostText.text = "Max";
                carShopUI.upgradeButton.interactable = false;                                   //make upgrade button interactable false
            }
            else if (GameManager.instance.carDatas[currentIndex].carLevel < 3)                  //if car level is less than 3
            {   
                carShopUI.upgradeInfoText.text = "Upgrade to lvl" + (carDatas[currentIndex].carLevel + 1);  //set the upgrade info text
                carShopUI.upgradeCostText.text = "" + 200 * (carDatas[currentIndex].carLevel + 1);  //set the cost text
                carShopUI.upgradeButton.interactable = true;                                    //make upgrade button interactable true
            }
        }
        else if (GameManager.instance.carDatas[currentIndex].unlocked == false)                 //if car is not unlocked
        {
            carShopUI.selectText.text = carDatas[currentIndex].carPrice + " Coins";             //set the select button to price of car
            carShopUI.upgradeButton.interactable = false;                                       //make upgrade button interactable false

            carShopUI.upgradeInfoText.text = "Upgrade to lvl" + (carDatas[currentIndex].carLevel + 1);  //set the upgrade info text
            carShopUI.upgradeCostText.text = "" + 200 * (carDatas[currentIndex].carLevel + 1);
        }

        carShopUI.carImage.sprite = carDatas[currentIndex].carSprite;                           //set the car image
                                                                                                //set speed bar value
        carShopUI.speedBar.fillAmount = (carDatas[currentIndex].carSpeed + carDatas[currentIndex].carLevel * carDatas[currentIndex].carSpeedIncreaser) / 15;
                                                                                                //set fuel bar value
        carShopUI.fuelBar.fillAmount = (carDatas[currentIndex].carFuel + carDatas[currentIndex].carLevel * carDatas[currentIndex].carFuelIncreaser) / 15;
                                                                                                //set turbo speed bar value
        carShopUI.turboSpeedBar.fillAmount = (carDatas[currentIndex].carTurboSpeed + carDatas[currentIndex].carLevel * carDatas[currentIndex].carTurboSpeedIncreaser) / 8;
    }

    public void NextCar()                                                                       //next button method
    {
        if (currentIndex < carDatas.Length - 1)                                                 //if current Index is less than total cars
        {
            currentIndex++;                                                                     //increase it by 1
            SetCarDetails();                                                                    //set car details
        }
    }

    public void PreviousCar()                                                                   //previous button method
    {
        if (currentIndex > 0)                                                                   //if current Index is more than 0
        {
            currentIndex--;                                                                     //decrease it by 1
            SetCarDetails();                                                                    //set car details
        }
    }

    public void UpgradeCarBtn()                                                                 //upgrade button method
    {
        if (carDatas[currentIndex].carLevel < 3)                                                //if car level is less than 3
        {
            if (GameManager.instance.coinAmount >= (200 * (carDatas[currentIndex].carLevel + 1)))   //if we have enough coins to upgrade
            {
                GameManager.instance.coinAmount -= (200 * (carDatas[currentIndex].carLevel + 1));   //reduce the coins
                GameManager.instance.carDatas[currentIndex].carLevel++;                             //upgrade the level
                GameManager.instance.Save();                                                        //save it
                GuiManager.instance.UpdateTotalCoins();                                             //update the total coins text
                SetCarDetails();                                                                    //set the car details
            }
        }
    }

    public void SelectCarBtn()                                                                  //car select button
    {
        if (currentIndex != GameManager.instance.selectedCar)                                   //if currentIndex is not equal to selected car
        {
            if (GameManager.instance.carDatas[currentIndex].unlocked == true)                   //if the car is unlocked
            {
                GameManager.instance.selectedCar = currentIndex;                                //set the GameManager.instance.selectedCar to currentIndex    
                GameManager.instance.Save();                                                    //Save it
                SetCarDetails();                                                                //set car details
                PlayerController.instance.SetCarSprite();                                       //change player car sprite
            }
            else if (GameManager.instance.carDatas[currentIndex].unlocked == false)             //if the car is not unlocked
            {
                if (GameManager.instance.coinAmount >= carDatas[currentIndex].carPrice)         //we check if we have enough coins
                {
                    GameManager.instance.coinAmount -= carDatas[currentIndex].carPrice;         //we reduce the coins
                    GameManager.instance.carDatas[currentIndex].unlocked = true;                //we unlock it
                    GameManager.instance.selectedCar = currentIndex;                            //set it as selected car
                    GameManager.instance.Save();                                                //save it
                    SetCarDetails();                                                            //set car details
                    GuiManager.instance.UpdateTotalCoins();                                     //update the total coins text
                    PlayerController.instance.SetCarSprite();                                   //change player car sprite
                }
            }
            SetGameData();                                                                      //set game data
        }
    }


























    [System.Serializable]
    protected struct CarShopUI
    {
        public Text carNameText, upgradeInfoText, upgradeCostText, selectText;
        public Image carImage;
        public Image speedBar, fuelBar, turboSpeedBar;
        public Button upgradeButton;
    }

    [System.Serializable]
    protected struct CarData
    {
        public string carName;
        public Sprite carSprite;
        public int carPrice;
        public float carSpeed, carFuel, carTurboSpeed;
        public float carSpeedIncreaser, carFuelIncreaser, carTurboSpeedIncreaser;
        [HideInInspector] public int carLevel;
        [HideInInspector] public bool unlocked;
    }

}
