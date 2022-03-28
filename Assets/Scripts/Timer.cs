using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    float currentTime = 0f;
    float startTime = 30f;
    public Text countDownText;
    Image countDownImage;
    Color color = Color.red;
    public bool loadingUi;
    public static Timer instance;

    void Start()
    {
        instance = this;
        currentTime = startTime;
        countDownImage = this.gameObject.GetComponent<Image>();
        //Debug.Log(countDownImage.sprite);

    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.foundOtherPlayer == true && GameManager.instance.canStartGame == true)
        {
            currentTime -= Time.deltaTime;
            // Debug.Log(currentTime);
            countDownText.text = currentTime.ToString("00");
            this.GetComponent<Image>().fillAmount = currentTime / startTime;
            if (currentTime <= 10)
            {
                countDownText.color = color;

            }
            if (currentTime <= 0)
            {
                HideTimer();
                //loadingUi = true;
                //Respawnball.instance.isReady = false;
                
                GameManager.instance.RoundUIChange();
                currentTime = startTime + 2;
                countDownText.color = Color.black;
                Invoke("ShowTimer", 2f);
                //GameManager.instance.Invoke("changeRound", 2f);
                if (GameManager.instance.roundIndex < 2)
                {
                    GameManager.instance.Invoke("changeRound", 2f);

                }
            }
        }
       

    }

    void ShowTimer()
    {
        countDownImage.enabled = true;
        countDownText.enabled = true;
    }

    void HideTimer()
    {
        countDownImage.enabled = false;
        countDownText.enabled = false;
    }


}
