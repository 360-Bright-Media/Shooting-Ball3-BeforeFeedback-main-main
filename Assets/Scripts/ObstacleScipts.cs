using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleScipts : MonoBehaviour
{
  

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("platform");
        if (collision.gameObject.name == "ball(Clone)")
        {
            collision.transform.GetChild(1).GetComponent<ParticleSystem>().Stop();
            FindObjectOfType<AudioManager>().Play("hittingPlatform");
            Destroy(collision.gameObject, 2f);

        }
    }
}


