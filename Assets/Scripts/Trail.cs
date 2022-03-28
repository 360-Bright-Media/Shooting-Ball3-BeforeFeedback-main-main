using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trail : MonoBehaviour
{
    public static Trail instance;
    
    TrailRenderer trail;
    void Start()
    {
        instance = this;
        trail = this.GetComponent<TrailRenderer>();
        //trail.startColor = Color.white;
        trail.enabled = false;
    }

  public void setTrail()
    {
        trail.enabled = true;
    }
}
