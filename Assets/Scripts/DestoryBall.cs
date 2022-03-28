using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestoryBall : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("platform");
        if (collision.gameObject.name == "ball(Clone)")
        {
            //FindObjectOfType<AudioManager>().Play("hittingPlatform");
            Destroy(collision.gameObject);

        }
    }
}
