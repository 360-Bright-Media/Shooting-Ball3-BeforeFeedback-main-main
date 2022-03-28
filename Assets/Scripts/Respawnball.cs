using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawnball : MonoBehaviour
{
    public bool isReady;
    public static Respawnball instance;


    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        isReady = true;
    }
    public void ReloadBalls()
    {
       
        if (FindObjectOfType<Ball>()) Destroy(FindObjectOfType<Ball>().gameObject);
        isReady = true;
        SlingShot.instance.CreateBall();
        GroundSound.instance.GetComponent<BoxCollider2D>().enabled = false;

        //if (Timer.instance.loadingUi==false)
        //{

        //}
    }
}
