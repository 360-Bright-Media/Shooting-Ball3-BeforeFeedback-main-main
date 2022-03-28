using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MatchmakingManager : MonoBehaviour
{
    public static MatchmakingManager instance;

    public float ColorSpeed = 1f;
    public float time = 0f;

    public Image LoadingCircle;
    public TextMeshProUGUI OppNo;

    private int once = 0;

    void Start()
    {
        instance = this;

        LoadingCircle.color = Color.HSVToRGB(.34f, .84f, .67f);        
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;

        if(GameManager.instance.foundOtherPlayer==false)
            OppNo.text = string.Format("{0}*****{1}", UnityEngine.Random.Range(80, 99), UnityEngine.Random.Range(800, 999));

        // Assign HSV values to float h, s & v. (Since material.color is stored in RGB)
        float h, s, v;
        Color.RGBToHSV(LoadingCircle.color, out h, out s, out v);

        // Use HSV values to increase H in HSVToRGB. It looks like putting a value greater than 1 will round % 1 it
        LoadingCircle.color = Color.HSVToRGB(h + Time.deltaTime * .25f, s, v);        
        
    }
}
