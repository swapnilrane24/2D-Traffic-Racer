using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObject : MonoBehaviour {

    public float speed;                                      //extra speed added to cars (varies with each cars)
	
	// Update is called once per frame
	void Update ()
    {
        transform.Translate(-Vector3.up * Time.deltaTime * (GuiManager.instance.CurrentSpeed + speed));
    }
}
