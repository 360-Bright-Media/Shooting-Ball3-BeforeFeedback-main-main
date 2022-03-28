using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
   

    private void FixedUpdate()
    {
        
    }



    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        if (collision.gameObject.name == "ball(Clone)" && (this.name == "target80" || this.name=="target40"))
        {
            FindObjectOfType<AudioManager>().Play("BottleShot");
            collision.transform.GetChild(1).GetComponent<ParticleSystem>().Stop();
            HittheBall();
            GameManager.instance.Spawn(this.transform.position,this.name);

            this.name = "TargetUsed";
            FindObjectOfType<AudioManager>().Play("Shatter");
            collision.gameObject.GetComponent<Collider2D>().enabled = false;
            Destroy(collision.gameObject, 0.25f);
            SlingShot.instance.ballRespawn.GetComponent<Animator>().Play("Reload");
            //var rb = collision.gameObject.GetComponent<Rigidbody2D>();

            //Vector2 originalVelocity = rb.velocity;
            //rb.velocity = originalVelocity / -4;
           // rb.gravityScale = 1;


        }
    }
    

    private void HittheBall()
    {
        if (this.name == "target40")
        {
            GameManager.instance.UpdateScore(40);
        }
        else if (this.name == "target80")
        {
            GameManager.instance.UpdateScore(80);

        }
        Debug.Log("you Hit");
        // Shatter();
        Invoke("Shatter", 0.1f);
    }

    void Shatter()
    {
        Debug.Log("shattered");
        //  this.GetComponent<SpriteRenderer>().enabled = false;
        // this.transform.GetChild(0).gameObject.SetActive(true);
        this.GetComponent<Animator>().Play("Shatter");


    }
}
