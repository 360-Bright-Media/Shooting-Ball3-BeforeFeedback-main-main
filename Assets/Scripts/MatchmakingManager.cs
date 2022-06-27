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

    const string glyphs = "abcdefghijklmnopqrstuvwxyz0123456789";

    private int once = 0;

    void Start()
    {
        instance = this;

        LoadingCircle.color = Color.HSVToRGB(.34f, .84f, .67f);

        StartCoroutine(SearchAnim());
    }

    // Update is called once per frame
    IEnumerator SearchAnim()
    {
        time += Time.deltaTime;

        while (!GameManager.instance.foundOtherPlayer)
        {
            char[] temp = new char[10];

            for (int i = 0; i < 10; i++)
                temp[i] += glyphs[UnityEngine.Random.Range(0, glyphs.Length)];

            OppNo.text = new string(temp).ToUpper();
            yield return new WaitForSeconds(.05f);


            // Assign HSV values to float h, s & v. (Since material.color is stored in RGB)
            float h, s, v;
            Color.RGBToHSV(LoadingCircle.color, out h, out s, out v);

            // Use HSV values to increase H in HSVToRGB. It looks like putting a value greater than 1 will round % 1 it
            LoadingCircle.color = Color.HSVToRGB(h + Time.deltaTime * .25f, s, v);
        }
    }
}
