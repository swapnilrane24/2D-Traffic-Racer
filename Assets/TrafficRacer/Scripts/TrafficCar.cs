/***********************************************************************************************************
 * Produced by Madfireon:               https://www.madfireongames.com/									   *
 * Facebook:                            https://www.facebook.com/madfireon/								   *
 * Contact us:                          https://www.madfireongames.com/contact							   *
 * Madfireon Unity Asset Store catalog: https://bit.ly/2JjKCtw											   *
 * Developed by Swapnil Rane:           https://in.linkedin.com/in/swapnilrane24                           *
 ***********************************************************************************************************/

/***********************************************************************************************************
* NOTE:- This script controls movement of traffic cars                                                     *
***********************************************************************************************************/

using UnityEngine;

public class TrafficCar : MonoBehaviour {

    [SerializeField] private LayerMask layerMask;
    [SerializeField] private float objectLength;                    //distance when travelled by object spawns the new object
    [SerializeField] private float deactivationDistance = 35;       //distance in -ve y axis travvelled to deactive gameobject
    [SerializeField] private float slowDownDistance;                //distance from the front car when this car slow downs
    [SerializeField] private Transform rayPos;

    private float randomSpeed;                                      //extra speed added to cars (varies with each cars)
    private float originYPos;                                       //pos at which vehicle object got active
    private bool nextObjectSpawned = false, vehicleGot = false;

    public float RandomSpeed { get { return randomSpeed; } }        //getter and setter

    // Use this for initialization
    void Start ()
    {
        randomSpeed = Random.Range(2, 5);                         //get values between 10 and 15
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (GameManager.instance.gameOver == true) return;

        if (transform.position.y <= -deactivationDistance)
        {
            gameObject.SetActive(false);
        }

        if (Mathf.Abs(transform.position.y - originYPos) > (objectLength + Random.Range(0, GuiManager.instance.MaxEnemyCarGap)) && nextObjectSpawned == false)
        {
            nextObjectSpawned = true;                           //set nextObjectSpawned to true
            Spawner.instance.SpawnObjects();
        }

        VehicleMovement();

    }

    void VehicleMovement()                                      //method which decide vehicle movement
    {
        DetectVehicle();                                        //to detect any other vehicle in same lane

        transform.Translate(-Vector3.up * Time.deltaTime * (GuiManager.instance.CurrentSpeed + randomSpeed));
    }

    void DetectVehicle()                                        //to detect any other vehicle in same lane
    {
        if (vehicleGot == true) return;                         //if other vehicle in same lane is already detected

        RaycastHit2D hit = Physics2D.Raycast(rayPos.position, -Vector2.up, slowDownDistance, layerMask);   //create RaycastHit variable
        if (hit.collider != null)       //if collider is found and its gameobject tag is Enemy
        {
            randomSpeed = hit.collider.gameObject.GetComponent<TrafficCar>().RandomSpeed;   //set the speed to other gameobject speed
            vehicleGot = true;
        }
    }

    public void DefaultSettings(bool value)                                 //reset settings at spawning
    {
        vehicleGot = false;
        originYPos = transform.position.y;
        nextObjectSpawned = false;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        UnityEditor.Handles.color = Color.red;
        if (rayPos != null)
            UnityEditor.Handles.DrawLine(rayPos.position, rayPos.position + new Vector3(0, -1 * slowDownDistance, 0));
    }
#endif
}
