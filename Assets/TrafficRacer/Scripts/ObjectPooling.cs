/***********************************************************************************************************
 * Produced by Madfireon:               https://www.madfireongames.com/									   *
 * Facebook:                            https://www.facebook.com/madfireon/								   *
 * Contact us:                          https://www.madfireongames.com/contact							   *
 * Madfireon Unity Asset Store catalog: https://bit.ly/2JjKCtw											   *
 * Developed by Swapnil Rane:           https://in.linkedin.com/in/swapnilrane24                           *
 ***********************************************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooling : MonoBehaviour {

    public static ObjectPooling instance;

    public ObjectPoolItems[] carsToPool;                                //ref to car objects
    public ObjectPoolItems[] gameFXToPool;                      //ref to game effects
    public ObjectPoolItems[] pickUpObjectsToPool;

    private List<GameObject> carPooledObjects;                          //stores all the spawned car objects
    private List<GameObject> gameFX;                              //stores all the spawned game effects objects
    private List<GameObject> pickUpPooledObjects;

    // Use this for initialization
	void Awake()
    {
        if (instance == null) instance = this;

        carPooledObjects = new List<GameObject>();                      //set it empty at start
        gameFX = new List<GameObject>();                           //set it empty at start	
        pickUpPooledObjects = new List<GameObject>();

        //car pooled object
        foreach (ObjectPoolItems item in carsToPool)                    //loop through all the elements in carsToPool array
        {
            for (int i = 0; i < item.poolAmount; i++)                   //loop poolAmount times
            {
                GameObject obj = Instantiate(item.poolObject);          //spawn the respective gameobject
                obj.name = item.name;                                   //give it a name
                obj.transform.parent = this.transform;                  //set its parent
                obj.SetActive(false);                                   //deactivate it in the scene
                carPooledObjects.Add(obj);                              //add the object to List
            }
        }

        //pickup effect pooled object
        foreach (ObjectPoolItems item in gameFXToPool)
        {
            for (int i = 0; i < item.poolAmount; i++)                   //loop poolAmount times
            {
                GameObject obj = Instantiate(item.poolObject);          //spawn the respective gameobject
                obj.name = item.name;                                   //give it a name
                obj.transform.parent = this.transform;                  //set its parent
                obj.SetActive(false);                                   //deactivate it in the scene
                gameFX.Add(obj);                                   //add the object to List
            }
        }

        //pickup pooled object
        foreach (ObjectPoolItems item in pickUpObjectsToPool)                    //loop through all the elements in carsToPool array
        {
            for (int i = 0; i < item.poolAmount; i++)                   //loop poolAmount times
            {
                GameObject obj = Instantiate(item.poolObject);          //spawn the respective gameobject
                obj.name = item.name;                                   //give it a name
                obj.transform.parent = this.transform;                  //set its parent
                obj.SetActive(false);                                   //deactivate it in the scene
                pickUpPooledObjects.Add(obj);                              //add the object to List
            }
        }
    }

    public GameObject GetCarPooledObject(string name)                   //method which provide the respective cars
    {
        for (int i = 0; i < carPooledObjects.Count; i++)                //loop total number of objects in carPooledObjects List times
        {                                                               //check if object of respective name is not active in scene
            if (carPooledObjects[i].activeInHierarchy == false && carPooledObjects[i].name == name)
                return carPooledObjects[i];                             //retuen that object
        }

        foreach (ObjectPoolItems item in carsToPool)                    //loop through all the elements in carsToPool array
        {
            if (item.poolObject.name == name)                           //we check in item in poolObject list has the respective name
            {
                GameObject obj = Instantiate(item.poolObject);          //spawn the respective gameobject
                obj.name = item.name;                                   //give it a name
                obj.transform.parent = this.transform;                  //set its parent
                obj.SetActive(false);                                   //deactivate it in the scene
                carPooledObjects.Add(obj);                              //add the object to List
                return obj;                                             //retuen that object
            }
        }
        return null;                                                    //by default we return null
    }

    public GameObject GetPickUpFXPooledObject(string name)
    {
        for (int i = 0; i < gameFX.Count; i++)
        {
            if (gameFX[i].activeInHierarchy == false && gameFX[i].name == name)
                return gameFX[i];
        }

        foreach (ObjectPoolItems item in gameFXToPool)
        {
            if (item.poolObject.name == name)
            {
                GameObject obj = Instantiate(item.poolObject);
                obj.name = item.name;
                obj.transform.parent = this.transform;
                obj.SetActive(false);
                gameFX.Add(obj);
                return obj;
            }
        }
        return null;
    }


    public GameObject GetPickUpPooledObject(string name)                   //method which provide the respective PickUp
    {
        for (int i = 0; i < pickUpPooledObjects.Count; i++)                //loop total number of objects in pickUpPooledObjects List times
        {                                                               //check if object of respective name is not active in scene
            if (pickUpPooledObjects[i].activeInHierarchy == false && pickUpPooledObjects[i].name == name)
                return pickUpPooledObjects[i];                             //retuen that object
        }

        foreach (ObjectPoolItems item in pickUpObjectsToPool)                    //loop through all the elements in pickUpObjectsToPool array
        {
            if (item.poolObject.name == name)                           //we check in item in poolObject list has the respective name
            {
                GameObject obj = Instantiate(item.poolObject);          //spawn the respective gameobject
                obj.name = item.name;                                   //give it a name
                obj.transform.parent = this.transform;                  //set its parent
                obj.SetActive(false);                                   //deactivate it in the scene
                pickUpPooledObjects.Add(obj);                              //add the object to List
                return obj;                                             //retuen that object
            }
        }
        return null;                                                    //by default we return null
    }

    public void SpawnPickUpFX(string value, Vector3 position)
    {
        GameObject pickUpFX = GetPickUpFXPooledObject(value);
        pickUpFX.transform.position = position;
        pickUpFX.transform.rotation = Quaternion.identity;
        pickUpFX.SetActive(true);
    }

    public void SpawnPickUpFX(string value, Vector3 position,float speed)
    {
        GameObject pickUpFX = GetPickUpFXPooledObject(value);
        pickUpFX.transform.position = position;
        pickUpFX.transform.rotation = Quaternion.identity;
        pickUpFX.SetActive(true);

        if (pickUpFX.GetComponent<MoveObject>() != null)
            pickUpFX.GetComponent<MoveObject>().speed = speed;
    }






















    [System.Serializable]
    public struct ObjectPoolItems                               //struct from storing pool item details
    {
        public string name;                                     //name
        public int poolAmount;                                  //how many to spawn at start of scene
        public GameObject poolObject;                           //object which we need to spawn
        public bool shouldExpland;                              //should we spawn more if required
    }
}
