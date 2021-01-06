using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IPhoneDetect : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        if ((Camera.main.aspect) <= 0.47f)
        {
            Camera.main.orthographicSize = 7.2f;
        }
    }

}
