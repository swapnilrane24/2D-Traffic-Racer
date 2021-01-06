/***********************************************************************************************************
 * Produced by Madfireon:               https://www.madfireongames.com/									   *
 * Facebook:                            https://www.facebook.com/madfireon/								   *
 * Contact us:                          https://www.madfireongames.com/contact							   *
 * Madfireon Unity Asset Store catalog: https://bit.ly/2JjKCtw											   *
 * Developed by Swapnil Rane:           https://in.linkedin.com/in/swapnilrane24                           *
 ***********************************************************************************************************/

using UnityEngine;

public class DeactivateWithTime : MonoBehaviour {

    public float deactivateTime;

    private float currentTime;

    private void OnEnable()
    {
        currentTime = deactivateTime;
    }

    // Update is called once per frame
    void Update ()
    {
        currentTime -= Time.deltaTime;
        if (currentTime <= 0)
            gameObject.SetActive(false);
	}
}
