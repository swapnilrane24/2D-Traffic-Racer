/***********************************************************************************************************
 * Produced by Madfireon:               https://www.madfireongames.com/									   *
 * Facebook:                            https://www.facebook.com/madfireon/								   *
 * Contact us:                          https://www.madfireongames.com/contact							   *
 * Madfireon Unity Asset Store catalog: https://bit.ly/2JjKCtw											   *
 * Developed by Swapnil Rane:           https://in.linkedin.com/in/swapnilrane24                           *
 ***********************************************************************************************************/

/***********************************************************************************************************
* NOTE:- This script spawns traffic car, fuel car, pickups                                                 *
***********************************************************************************************************/

using UnityEngine;

public class Spawner : MonoBehaviour {

    public static Spawner instance;

    [SerializeField] protected DropPickUp[] dropPickUps;                            //ref to coins are there types
    [SerializeField] private float[] spawnXPos;                                     //positions at which cars will be spawned

    private GameObject car, pickUp;                                                 //varibles to store car and pickup
    private int lastSpawnIndex, randomPos;                                          //index reference of last spawn position
    private ObjectPooling objectPooling;                                            //ref to objectpooling

    // Use this for initialization
    void Start ()
    {
        if (instance == null) instance = this;
        objectPooling = GetComponent<ObjectPooling>();                              //get the objectPooling script attached to this gameobject	
    }

    public void SpawnObjects()
    {                                                                               //if player car fuel is less than 3 and fuel is not spawned
        if (GuiManager.instance.CurrentFuel < 3 && GuiManager.instance.FuelSpawned == false)
        {
            SpawnFuel();                                                            //spawn fuel
            GuiManager.instance.FuelSpawned = true;                                 //set fuel spawned to true
        }
        else
        {
            int r = Random.Range(0, 6);                                             //get random number between 0 to 5
            if (r == 3)                                                             //if r is 3
            {   
                string pickUpName = "";                                             //string to store value

                int k = Random.Range(0, 6);                                         //get random number between 0 to 5
                if (k == 0)                                                         //if k is 0
                {
                    //we check if magnet powerup is not active and power pickup is not spawned
                    if (GuiManager.instance.MagnetActive == false && GuiManager.instance.MagnetSpawned == false)      
                        pickUpName = "MagnetPickUp";                                //if not set the string to MagnetPickUp
                    else pickUpName = "Coin";                                       //else Coin
                }
                else if (k == 1)
                {
                    if (GuiManager.instance.TurboActive == false && GuiManager.instance.TurboSpawned == false)
                        pickUpName = "TurboPickUp";
                    else pickUpName = "Coin";
                }
                else if (k == 2)
                {
                    if (GuiManager.instance.ShieldActive == false && GuiManager.instance.ShieldSpawned == false)
                        pickUpName = "ShieldPickUp";
                    else pickUpName = "Coin";
                }
                else if (k == 3)
                {
                    if (GuiManager.instance.DoubleCoinActive == false && GuiManager.instance.DoubleCoinSpawned == false)
                        pickUpName = "DoubleCoinPickUp";
                    else pickUpName = "Coin";
                }
                else pickUpName = "Coin";

                if (pickUpName == "Coin")                                           //we check if pickUpName string is Coin
                    SpawnCoin();                                                    //we spawn coin
                else
                    SpawnPowerUp(pickUpName);                                       //else we spawn PowerUp
            }
            else
            {
                SpawnCar();                        //else spawn vehicle
            }
        }
    }

    void SpawnCar()
    {
        int r = Random.Range(0, objectPooling.carsToPool.Length);                           //select random number between zero and carsToPool array length
        car = objectPooling.GetCarPooledObject(objectPooling.carsToPool[r].name);           //get the car from objectPooling

        RandomPos();
        car.transform.position = new Vector3(spawnXPos[randomPos], transform.position.y, 0);    //set its transform
        car.transform.rotation = Quaternion.identity;                                       //set its rotation to car default rotation
        car.SetActive(true);                                                                //activate it in scene
        car.GetComponent<TrafficCar>().DefaultSettings(true);                               //direction of vehicle is same as player direction

        lastSpawnIndex = randomPos;                                                         //set lastSpawnIndex to randomPos
    }

    void SpawnFuel()
    {
        RandomPos();
        GameObject fuelCar = objectPooling.GetPickUpPooledObject("CarFuel");
        fuelCar.transform.position = new Vector3(spawnXPos[randomPos], transform.position.y, 0);    //set its transform
        fuelCar.transform.rotation = Quaternion.identity;                                       //set its rotation to car default rotation
        fuelCar.SetActive(true);                                                                //activate it in scene
        fuelCar.GetComponent<TrafficCar>().DefaultSettings(true);                               //direction of vehicle is same as player direction

        lastSpawnIndex = randomPos;
    }

    void RandomPos()
    {
        randomPos = Random.Range(0, spawnXPos.Length);                                      //random position

        while (lastSpawnIndex == randomPos)                                                 //if lastSpawnIndex is same as randomPos
            randomPos = Random.Range(0, spawnXPos.Length);                                  //again get random position
    }

    void SpawnCoin()
    {
        int itemChance = 0;                                                                 //in to store chance

        for (int j = 0; j < dropPickUps.Length; j++)                                        //loop through all the dropPickUps
        {
            itemChance += dropPickUps[j].spawnChance;                                       //and add the spawnChance
        }

        int randomValue = Random.Range(0, itemChance);                                      //get a random value between 0 and itemChance

        for (int i = 0; i < dropPickUps.Length; i++)
        {
            if (randomValue <= dropPickUps[i].spawnChance)      //we check if any object have a change less or equal to random
            {
                RandomPos();                                                                                //get random position
                GameObject pickUp = objectPooling.GetPickUpPooledObject(dropPickUps[i].pickUpName);
                pickUp.transform.position = new Vector3(spawnXPos[randomPos], transform.position.y, 0);     //set its transform
                pickUp.transform.rotation = Quaternion.identity;                                            //set its rotation to pickup default rotation
                pickUp.SetActive(true);                                                                     //activate it in scene
                pickUp.GetComponent<PickUps>().DefaultSettings();                                           //set default settings

                lastSpawnIndex = randomPos;                                                                 //set lastSpawnIndex
                return;                                                                                     //and return so we dont spawn other objects
            }
            //in case randomValue is more than any items spawn chance 
            randomValue -= dropPickUps[i].spawnChance;                //we reduce the randomValue by the spawnChance of i index item                                          
        }

    }

    void SpawnPowerUp(string value)                                                                 //spawn powerup
    {
        RandomPos();                                                                                //get random pos
        GameObject powerUp = objectPooling.GetPickUpPooledObject(value);                            //get the powerup
        powerUp.transform.position = new Vector3(spawnXPos[randomPos], transform.position.y, 0);    //set its transform
        powerUp.transform.rotation = Quaternion.identity;                                           //set its rotation to car default rotation
        powerUp.SetActive(true);                                                                    //activate it in scene
        powerUp.GetComponent<PickUps>().DefaultSettings();                                          //set default settings

        if (value == "MagnetPickUp")
            GuiManager.instance.MagnetSpawned = true;
        else if (value == "TurboPickUp")
            GuiManager.instance.TurboSpawned = true;
        else if (value == "DoubleCoinPickUp")
            GuiManager.instance.DoubleCoinSpawned = true;

        lastSpawnIndex = randomPos;
    }















    [System.Serializable]
    protected class DropPickUp
    {
        public string pickUpName;
        [Range(0, 100)]
        public int spawnChance;
    }
}
