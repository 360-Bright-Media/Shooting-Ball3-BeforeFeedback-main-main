using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundSound : MonoBehaviour
{
    public static GroundSound instance;
    private void Awake()
    {
        instance = this;
        this.GetComponent<BoxCollider2D>().enabled = false;
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("sound");
        if (collision.gameObject.name == "ball(Clone)")
        {
            collision.transform.GetChild(1).GetComponent<ParticleSystem>().Stop();
            FindObjectOfType<AudioManager>().Play("hittingPlatform");


        }
    }
}


