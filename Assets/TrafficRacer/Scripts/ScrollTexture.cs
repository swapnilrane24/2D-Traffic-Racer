/***********************************************************************************************************
 * Produced by Madfireon:               https://www.madfireongames.com/									   *
 * Facebook:                            https://www.facebook.com/madfireon/								   *
 * Contact us:                          https://www.madfireongames.com/contact							   *
 * Madfireon Unity Asset Store catalog: https://bit.ly/2JjKCtw											   *
 * Developed by Swapnil Rane:           https://in.linkedin.com/in/swapnilrane24                           *
 ***********************************************************************************************************/

/***********************************************************************************************************
* NOTE:- This script controls scrolling of texture                                                         *
***********************************************************************************************************/

using UnityEngine;

public class ScrollTexture : MonoBehaviour
{

    public static ScrollTexture instance;

    [SerializeField] private float scrollSpeed;                 //speed of scrolling texture
    [SerializeField] private Renderer[] renderers;              //ref to all the textures

    public float ScrollSpeed { get { return scrollSpeed; } set { scrollSpeed = value; } }

    // Use this for initialization
    void Awake()
    {
        if (instance == null) instance = this;
    }
    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.gameOver == true) return;
        foreach (Renderer renderer in renderers)            //loop through all the textures in the renderers array
        {
            renderer.material.mainTextureOffset += new Vector2(0, Time.deltaTime * scrollSpeed);    //apply the speed
        }
    }
}