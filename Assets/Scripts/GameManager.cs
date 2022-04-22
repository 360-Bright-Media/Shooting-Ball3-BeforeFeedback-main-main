using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System;

public class GameManager : MonoBehaviour
{
    private float GlobalWalletBalance = 0;
    public bool canRestart = false;
    private int score;
    public static GameManager instance;
    private int levelCounter;
    [SerializeField] Transform LevelsHolders;
    [Header("UI")]
    // [SerializeField] TextMeshProUGUI scoreText;
    public GameObject ReplayBtn;
    public GameObject NoBalPop;
    [SerializeField] Button playButton;
    [SerializeField] GameObject tutorialPanel;
    [SerializeField] Image endGameTimer;
    int endGameTime = 3;
    [SerializeField] Text scoreTextHolder;
    [SerializeField] Text OppenentScoreTextHolder;
    [SerializeField] Text WinnerNameText;
    [SerializeField] Text LosserNameText;
    [SerializeField] Text scoreOnBoard;
    [SerializeField] Text opponentScoreOnBoard;
    [SerializeField] Text thisText;
    [SerializeField] Image thisImage;
    [SerializeField] Image otherImage;
    [SerializeField] TextMeshProUGUI otherText;
    [SerializeField] Text displayScoreTextAtEnd;
    [SerializeField] Text displayWinnerOrLosserText;
    [SerializeField] Text RankNo;
    [SerializeField] Text PlayerNameText;
    [SerializeField] Text OpponentNameText;
    [SerializeField] Image inGameProfilePicPlayer;
    [SerializeField] Image inGameProfielPicOtherPlayer;
    [SerializeField] GameObject StartScreenBackButton;

    //Networking variables
    public string serverURL = "https://gamejoyproserver1v1.herokuapp.com/";
    public string sendDataURL = "http://52.66.182.199/api/gameplay";
    public string walletInfoURL = "http://52.66.182.199/api/wallet";
    public string walletUpdateURL = "http://52.66.182.199/api/walletdeduction";
    public string getTournAttemptURL = "https://admin.gamejoypro.com/api/getattempts";
    public string botListURL = "https://admin.gamejoypro.com/api/botlist";

    public NetworkingPlayer otherPlayer;
    public NetworkingPlayer thisPlayer;
    public SendData1 sendThisPlayerData;
    public WinningDetails winningDetails;
    public string sendWinningDetailsData;
    public string sendNewData1;
    public bool isDataSend;

    [Header("Server Players Details")]
    private string walletInfoData;
    private string walletUpdateData;

    public WalletInfo walletInfo;
    public WallUpdate walletUpdate;


    public bool foundOtherPlayer = false;
    public bool canStartGame;
    public bool foundWinner;
    string myRoomId;
    string gameId;
    public bool isGameOver;
    [SerializeField] GameObject findingPlayerScreen;
    [SerializeField] GameObject StartGameOverScreen;
    [SerializeField] GameObject OpponentGameName;
    [SerializeField] GameObject OpponentScoreText;
    [SerializeField] GameObject OpponentImage;

    public bool hasExitedRoom;

    [SerializeField] Material backGroundAnim;
    GameObject levelToDestroy;
    float scrollSpeed = 0.02f;
    int random;
    [SerializeField] Animator roundAnima;
    public int roundIndex = 0;
    TextMeshProUGUI randomText;
    [SerializeField] ParticleSystem spawnEffect1,spawnEffect2;
    List<GameObject> EndUIGameObj = new List<GameObject>();
    GameObject EndUIFetchingScoreObj;
   
    [SerializeField] GameObject P2;
    [SerializeField] GameObject Footer_1;
    [SerializeField] GameObject Footer;
    [SerializeField] Text ChanceLeft;
    [SerializeField] Text ReloadPrice;

    private bool isReEntryPaid;
    private bool isSingleEntry;

    public GameObject EndUI;

    private void Awake()
    {
        instance = this;
        //EndUIFetchingScoreObj = GameObject.FindWithTag("FUI");
        foreach (GameObject ui in GameObject.FindGameObjectsWithTag("EUI"))
        {

            EndUIGameObj.Add(ui);
        }

       // walletInfoData = JsonUtility.ToJson(walletInfo);
        //WebRequestHandler.Instance.Post(walletInfoURL, walletInfoData, (response, status) =>
        //{
        //    WalletInfo walletInfoResponse = JsonUtility.FromJson<WalletInfo>(response);
        //    GlobalWalletBalance = int.Parse(walletInfoResponse.data.cash_balance);
        //    Debug.Log(GlobalWalletBalance + "<= init balance");
        //});
        //if (isTutorialViewed == false)
        //{
        //    tutorialPanel.SetActive(true);
        //    Time.timeScale = 0;
        //}
    }
    //void Update()
    //{
    //    float offset = Time.time * scrollSpeed;
    //    backGroundAnim.SetTextureOffset("_MainTex", new Vector2(offset, 0));
    //}


    void Start()
    {

        Debug.Log("android data => " +
            AndroidtoUnityJSON.instance.player_id + ", " + AndroidtoUnityJSON.instance.token + ", " + AndroidtoUnityJSON.instance.user_name + ", " +
            AndroidtoUnityJSON.instance.game_id + ", " + AndroidtoUnityJSON.instance.profile_image + ", " + AndroidtoUnityJSON.instance.game_fee + ", " +
            AndroidtoUnityJSON.instance.game_mode + ", " + AndroidtoUnityJSON.instance.battle_id + ", " + AndroidtoUnityJSON.instance.tour_id + ", " +
            AndroidtoUnityJSON.instance.tour_mode + ", " + AndroidtoUnityJSON.instance.tour_name + ", " + AndroidtoUnityJSON.instance.no_of_attempts + ", " +
            AndroidtoUnityJSON.instance.mm_player + ", " + AndroidtoUnityJSON.instance.entry_type);

        thisPlayer.gameName = thisPlayer.gameName + AndroidtoUnityJSON.instance.game_fee;

        // EndUIFetchingScoreObj.SetActive(false);
        foreach (var ui in EndUIGameObj)
        {
            ui.SetActive(false);
        }
        // Debug.Log("Why");

        if (AndroidtoUnityJSON.instance.game_mode == "tour")
        {
            if (AndroidtoUnityJSON.instance.mm_player == "2")
            {
                //VS.SetActive(true);
                P2.SetActive(true);

                if (AndroidtoUnityJSON.instance.entry_type == "re entry paid")
                {
                    isReEntryPaid = true;
                }
                else if (AndroidtoUnityJSON.instance.entry_type == "re entry")
                {
                    isReEntryPaid = false;
                }
                else if (AndroidtoUnityJSON.instance.entry_type == "single entry")
                {
                    isSingleEntry = true;
                }

                StartCoroutine(StartOnlinePlay());
            }
            else if (AndroidtoUnityJSON.instance.mm_player == "1")
            {
                //VS.SetActive(false);
                P2.SetActive(false);

                if (AndroidtoUnityJSON.instance.entry_type == "re entry paid")
                {
                    isReEntryPaid = true;
                }
                else if (AndroidtoUnityJSON.instance.entry_type == "re entry")
                {
                    isReEntryPaid = false;
                }
                else if (AndroidtoUnityJSON.instance.entry_type == "single entry")
                {
                    isSingleEntry = true;
                }

                StartCoroutine(StartOfflinePlay());
            }
        }
        else if (AndroidtoUnityJSON.instance.game_mode == "battle")
        {
            StartCoroutine(StartOnlinePlay());
        }

    }

    IEnumerator StartOnlinePlay()
    {
        //thisPlayer.playerName = AndroidtoUnityJSON.instance.user_name;
        thisPlayer.imageURL = AndroidtoUnityJSON.instance.profile_image;
        if (string.IsNullOrEmpty(thisPlayer.imageURL) == false)
        {
            WebRequestHandler.Instance.DownloadSprite(thisPlayer.imageURL, (sprite) =>
            {
                thisImage.sprite = sprite;
            });
        }

        thisPlayer.playerName = AndroidtoUnityJSON.instance.user_name;
        thisPlayer.playerIdToBeSentorReceived = AndroidtoUnityJSON.instance.player_id;
        string mydata = JsonUtility.ToJson(thisPlayer);
        //Debug.Log("me"+WebRequestHandler.Instance);

        WebRequestHandler.Instance.Post(serverURL + "startRoom", mydata, (response, status) => {

            NetworkingPlayer player = JsonUtility.FromJson<NetworkingPlayer>(response);
            thisPlayer.RoomID = player.RoomID;
            thisPlayer.playerId = player.playerId;
            thisPlayer.playerName = player.playerName;          // was commented 21/03
            myRoomId = player.RoomID;
            sendThisPlayerData.room_id = myRoomId;
            //sendThisPlayerData.player_id = AndroidtoUnityJSON.instance.player_id;       // added 21/03
            Debug.Log("My Room is " + myRoomId);
        });

        thisPlayer.score = score;
        thisText.text = thisPlayer.playerName;




        while (foundOtherPlayer == false)
        {
            yield return new WaitForSecondsRealtime(0.5f);

            WebRequestHandler.Instance.Get(serverURL+ "fetchOtherPlayerData/" + myRoomId + "/" + thisPlayer.playerId, (response, status) => {

                otherPlayer = JsonUtility.FromJson<NetworkingPlayer>(response);
                Debug.Log(response);
                if (otherPlayer.playerId != null && otherPlayer.playerId != "")
                {
                    foundOtherPlayer = true;

                    if (otherPlayer.isBot)
                    {
                        WebRequestHandler.Instance.Get(botListURL, (response, status) =>
                        {
                            BotList botList = JsonUtility.FromJson<BotList>(response);
                            System.Random r = new System.Random();
                            int rand = r.Next(0, botList.data.Length);


                            otherPlayer.playerIdToBeSentorReceived = botList.data[rand].id;
                            otherPlayer.imageURL = botList.data[rand].image;
                            otherPlayer.playerName = botList.data[rand].first_name;

                            StartScreenBackButton.gameObject.SetActive(false);
                            otherText.text = otherPlayer.playerName; //matchamking opponent name
                            OpponentNameText.text = otherPlayer.playerName;       // in game opponent name 

                            WebRequestHandler.Instance.DownloadSprite(otherPlayer.imageURL, (sprite) => { otherImage.sprite = sprite; });
                            WebRequestHandler.Instance.DownloadSprite(otherPlayer.imageURL, (sprite) => { inGameProfielPicOtherPlayer.sprite = sprite; });
                        });
                    }
                    else
                    {
                        StartScreenBackButton.gameObject.SetActive(false);
                        otherText.text = otherPlayer.playerName; //matchamking opponent name
                        OpponentNameText.text = otherPlayer.playerName;

                        WebRequestHandler.Instance.DownloadSprite(otherPlayer.imageURL, (sprite) => { otherImage.sprite = sprite; });
                        WebRequestHandler.Instance.DownloadSprite(otherPlayer.imageURL, (sprite) => { inGameProfielPicOtherPlayer.sprite = sprite; });

                    }


                    //WebRequestHandler.Instance.DownloadSprite(otherPlayer.imageURL, (sprite) => { thisImage.sprite = sprite; });
                    
                }
                //otherPlayer = JsonUtility.FromJson<NetworkingPlayer>(response);

            });
        }
        yield return new WaitForSecondsRealtime(0.5f);

        foreach (var ui in GameObject.FindGameObjectsWithTag("SUI"))
            ui.SetActive(false);

        //findingPlayerScreen.SetActive(false);
        canStartGame = true;

        PlayerNameText.text = thisPlayer.playerName;
        OpponentNameText.text = otherPlayer.playerName;
        inGameProfilePicPlayer.sprite = thisImage.sprite;
        //inGameProfielPicOtherPlayer.sprite = otherImage.sprite;

        var attemptNo = 0;

        if (AndroidtoUnityJSON.instance.game_mode == "tour")
        {
            //attempt check
            WebRequestHandler.Instance.Get(getTournAttemptURL + "/" + AndroidtoUnityJSON.instance.tour_id, (response, status) =>
            {
            GetAttempt attempt = JsonUtility.FromJson<GetAttempt>(response);

            Debug.Log("Attempt(s) remaining: " + attempt.data.remainAttemp);

            attemptNo = int.Parse(attempt.data.remainAttemp);

                if (attempt.data.remainAttemp == AndroidtoUnityJSON.instance.no_of_attempts)
                {
                    AndroidtoUnityJSON.instance.isFirst = true;
                }
                else
                {
                    AndroidtoUnityJSON.instance.isFirst = false;
                }
            });
        }

        if (AndroidtoUnityJSON.instance.game_mode == "tour")
        {
            //if (AndroidtoUnityJSON.instance.entry_type == "re entry" && AndroidtoUnityJSON.instance.isFirst ||
            //    AndroidtoUnityJSON.instance.entry_type == "re entry paid" || AndroidtoUnityJSON.instance.entry_type == "single entry")
            //{
            //    DeductWallet();
            //}
        }
        else
        {
            DeductWallet();
        }

             

        levelCounter =UnityEngine. Random.Range(0, LevelsHolders.transform.childCount);
        LevelsHolders.GetChild(levelCounter).gameObject.SetActive(true);
        levelToDestroy = LevelsHolders.GetChild(levelCounter).gameObject;

        while (isGameOver == false)
        {
            thisPlayer.score = score;
            yield return new WaitForSecondsRealtime(0.5f);

            WebRequestHandler.Instance.Get(serverURL+  "fetchscore/" + myRoomId + "/" + thisPlayer.playerId + "/" +otherPlayer.playerId+"/"+ thisPlayer.score + "/" + thisPlayer.incrementFactor, (response, status) => {

                otherPlayer.score = int.Parse(response);
                OppenentScoreTextHolder.text = response;
                //winningDetails.thisplayerScore = int.Parse(response);     // commented on 21/03
                //Debug.Log(response);

                //otherPlayer = JsonUtility.FromJson<NetworkingPlayer>(response);

            });
        }
        Debug.Log("move to");
        thisPlayer.finishedPlaying = true;

        if (AndroidtoUnityJSON.instance.game_mode == "tour" && AndroidtoUnityJSON.instance.entry_type == "single entry")
        {
            ChanceLeft.gameObject.SetActive(false);
            ReplayBtn.transform.parent.gameObject.SetActive(false);
            Footer_1.SetActive(true);
            
        }
        else if (AndroidtoUnityJSON.instance.game_mode == "tour")
        {
            if (attemptNo <= 0)
            {
                ReplayBtn.transform.parent.gameObject.SetActive(false);
                Footer_1.SetActive(true);
            }

            ChanceLeft.gameObject.SetActive(true);
            ChanceLeft.text = "Chances Left: " + attemptNo;
        }
        else if (AndroidtoUnityJSON.instance.game_mode == "battle")
        {
            ChanceLeft.gameObject.SetActive(false);
        }



        if (AndroidtoUnityJSON.instance.mm_player == "1")
        {
            LosserNameText.transform.parent.gameObject.SetActive(false);
        }



        if (float.Parse(AndroidtoUnityJSON.instance.game_fee) <= 0) //|| AndroidtoUnityJSON.instance.entry_type == "re entry" && AndroidtoUnityJSON.instance.game_mode == "tour")
        {
            ReloadPrice.text = "FREE";
        }
        else
        {
            ReloadPrice.text = /* "?" +*/ AndroidtoUnityJSON.instance.game_fee;
        }

        EndUI.SetActive(true);

        foreach (var ui in EndUIGameObj)
        {
            ui.SetActive(true);

        }

        while (foundWinner == false)
        {
            yield return new WaitForSecondsRealtime(0.5f);
            thisPlayer.score = score;
            WebRequestHandler.Instance.Get(serverURL+ "getWinner/" + myRoomId + "/" +
                thisPlayer.playerId + "/" + otherPlayer.playerId + "/" +thisPlayer.score + "/" + thisPlayer.finishedPlaying + "/" +
                foundWinner,
                (response, status) => {

                    Debug.Log("Test log " + response);
                    if (response != "false" && foundWinner==false)
                    {
                        // StartGameOverScreen.transform.GetChild(1).gameObject.SetActive(false);
                        //EndUIFetchingScoreObj.SetActive(false);

                        WebRequestHandler.Instance.Get(serverURL + "fetchscore/" + myRoomId + "/" + thisPlayer.playerId + "/" + otherPlayer.playerId + "/" + thisPlayer.score + "/" + thisPlayer.incrementFactor, (response, status) => {

                            otherPlayer.score = Int32.Parse(response);

                        });

                        foundWinner = true;

                        if (response == "draw")
                        {

                            // Debug.Log(thisPlayer.playerName + " won with score " + thisPlayer.score);
                            sendThisPlayerData.winning_details.winningPlayerScore = thisPlayer.score.ToString();              //ScoringManager.instance.score.ToString();
                            sendThisPlayerData.winning_details.winningPlayerID = thisPlayer.playerIdToBeSentorReceived;
                            sendThisPlayerData.winning_details.lossingPlayerID = otherPlayer.playerIdToBeSentorReceived;           // was not string
                            sendThisPlayerData.winning_details.lossingPlayerScore = otherPlayer.score.ToString();
                            WinnerNameText.text = thisPlayer.playerName;
                            LosserNameText.text = otherPlayer.playerName;
                            displayWinnerOrLosserText.text = "MATCH DRAW";
                            RankNo.text = "1";
                            sendThisPlayerData.game_status = "DRAW";
                            displayScoreTextAtEnd.text = thisPlayer.score.ToString();
                            scoreOnBoard.text = thisPlayer.score.ToString();
                            opponentScoreOnBoard.text = otherPlayer.score.ToString();
                        }


                        else
                        {
                            NetworkingPlayer winner = JsonUtility.FromJson<NetworkingPlayer>(response);
                            if (winner.playerId == thisPlayer.playerId)
                            {
                                //canRestart = true;
                                //ReplayBtn.GetComponent<Button>().interactable = canRestart;

                                thisPlayer.iWon = true;
                                Debug.Log(thisPlayer.playerName + " won with score " + thisPlayer.score);
                                sendThisPlayerData.winning_details.winningPlayerScore = thisPlayer.score.ToString();
                                sendThisPlayerData.winning_details.winningPlayerID = thisPlayer.playerIdToBeSentorReceived;
                                sendThisPlayerData.winning_details.lossingPlayerID = otherPlayer.playerIdToBeSentorReceived;
                                sendThisPlayerData.winning_details.lossingPlayerScore = otherPlayer.score.ToString();

                                WinnerNameText.text = thisPlayer.playerName;
                                LosserNameText.text = otherPlayer.playerName;
                                displayWinnerOrLosserText.text = "YOU WON";
                                RankNo.text = "1";
                                sendThisPlayerData.game_status = "WIN";
                                displayScoreTextAtEnd.text = score.ToString();
                                scoreOnBoard.text = score.ToString();
                                opponentScoreOnBoard.text = otherPlayer.score.ToString();
                                //UpdateWallet(true);

                            }
                            else if (winner.playerId == otherPlayer.playerId)
                            {


                                //if (GlobalWalletBalance >= int.Parse(AndroidtoUnityJSON.instance.game_fee))
                                //    canRestart = true;
                                //else
                                //    canRestart = false;

                                //ReplayBtn.GetComponent<Button>().interactable = canRestart;

                                otherPlayer.iWon = true;
                                //otherPlayer = winner;
                                Debug.Log(otherPlayer.playerName + " won with score " + otherPlayer.score);
                                sendThisPlayerData.winning_details.winningPlayerScore = otherPlayer.score.ToString();
                                sendThisPlayerData.winning_details.winningPlayerID = otherPlayer.playerIdToBeSentorReceived;
                                sendThisPlayerData.winning_details.lossingPlayerID = thisPlayer.playerIdToBeSentorReceived;
                                sendThisPlayerData.winning_details.lossingPlayerScore = thisPlayer.score.ToString();
                                WinnerNameText.text = otherPlayer.playerName;
                                LosserNameText.text = thisPlayer.playerName;
                                displayWinnerOrLosserText.text = "YOU LOSE";
                                RankNo.text = "2";
                                sendThisPlayerData.game_status = "LOST";
                                displayScoreTextAtEnd.text = score.ToString();
                                scoreOnBoard.text = otherPlayer.score.ToString();
                                opponentScoreOnBoard.text = score.ToString();
                            }
                        }


                        sendThisPlayerData.player_id = otherPlayer.playerIdToBeSentorReceived;
                        sendThisPlayerData.winning_details.thisplayerScore = thisPlayer.score;
                        sendThisPlayerData.wallet_amt = AndroidtoUnityJSON.instance.game_fee;
                        sendThisPlayerData.game_mode = AndroidtoUnityJSON.instance.game_mode;
                        sendThisPlayerData.game_id = AndroidtoUnityJSON.instance.game_id;

                        if (AndroidtoUnityJSON.instance.game_mode == "tour")
                            sendThisPlayerData.battle_tournament_id = AndroidtoUnityJSON.instance.tour_id;
                        else if (AndroidtoUnityJSON.instance.game_mode == "battle")
                            sendThisPlayerData.battle_tournament_id = AndroidtoUnityJSON.instance.battle_id;

                        sendThisPlayerData.game_end_time = GetSystemTime();      
                        //sendThisPlayerData.game_status = "FINISHED";

                        sendThisPlayerData.wallet_amt = GlobalWalletBalance.ToString();         // commented on 21/03      // uncommented on 01/04

                        string sendWinningDetailsData = JsonUtility.ToJson(winningDetails);
                        string sendNewData1 = JsonUtility.ToJson(sendThisPlayerData);
                        //Debug.Log(sendNewData+"sendNewData");
                        WebRequestHandler.Instance.Post(sendDataURL, sendNewData1, (response, status) =>
                        {
                            Debug.Log(response + "HitNewApi");
                        });

                        //WebRequestHandler.Instance.Post(sendDataURL, sendWinningDetailsData, (response, status) =>
                        //{
                        //    Debug.Log(response + "For Winning Details");
                        //});

                        isDataSend = true;

                        //if (otherPlayer.isBot)
                        //    SendBotData(otherPlayer.iWon);

                    }
                    else
                    {
                        Debug.Log("Waiting for other player to finish playing");
                    }



                });
        }
    }

    void SendBotData(bool isWon)
    {
        if (!isWon)
        {
            thisPlayer.iWon = true;
            // Debug.Log(thisPlayer.playerName + " won with score " + thisPlayer.score);
            sendThisPlayerData.winning_details.winningPlayerScore = score.ToString();
            sendThisPlayerData.winning_details.winningPlayerID = thisPlayer.playerIdToBeSentorReceived;
            sendThisPlayerData.winning_details.lossingPlayerID = otherPlayer.playerIdToBeSentorReceived;
            sendThisPlayerData.winning_details.lossingPlayerScore = otherPlayer.score.ToString();
            sendThisPlayerData.game_status = "LOST";
        }
        else if (isWon)
        {
            otherPlayer.iWon = true;
            // Debug.Log(otherPlayer.playerName + " won with score " + otherPlayer.score);
            sendThisPlayerData.winning_details.winningPlayerID = otherPlayer.playerIdToBeSentorReceived;
            sendThisPlayerData.winning_details.winningPlayerScore = otherPlayer.score.ToString();
            sendThisPlayerData.winning_details.lossingPlayerScore = score.ToString();
            sendThisPlayerData.winning_details.lossingPlayerID = thisPlayer.playerIdToBeSentorReceived;
            sendThisPlayerData.game_status = "WIN";
        }

        sendThisPlayerData.player_id = thisPlayer.playerIdToBeSentorReceived;
        sendThisPlayerData.winning_details.thisplayerScore = otherPlayer.score;
        sendThisPlayerData.wallet_amt = AndroidtoUnityJSON.instance.game_fee;
        sendThisPlayerData.game_mode = AndroidtoUnityJSON.instance.game_mode;
        sendThisPlayerData.game_id = AndroidtoUnityJSON.instance.game_id;

        if (AndroidtoUnityJSON.instance.game_mode == "tour")
            sendThisPlayerData.battle_tournament_id = AndroidtoUnityJSON.instance.tour_id;
        else if (AndroidtoUnityJSON.instance.game_mode == "battle")
            sendThisPlayerData.battle_tournament_id = AndroidtoUnityJSON.instance.battle_id;

        sendThisPlayerData.game_end_time = GetSystemTime();

        string sendWinningDetailsData = JsonUtility.ToJson(winningDetails);
        string sendNewData = JsonUtility.ToJson(sendThisPlayerData);

        WebRequestHandler.Instance.Post(sendDataURL, sendNewData, (response, status) =>
        {
            Debug.Log(response + " HitNewApiBot");
        });
    }

    IEnumerator StartOfflinePlay() //SERVERLESS GAME 
    {
        //StartScreenBackButton.SetActive(false);
        thisPlayer.playerName = AndroidtoUnityJSON.instance.user_name;
        thisPlayer.imageURL = AndroidtoUnityJSON.instance.profile_image;

        OpponentGameName.SetActive(false);
        OpponentImage.SetActive(false);
        OpponentScoreText.SetActive(false);

        //thisPlayer.score = int.Parse(score.text);
        
        //PlayerNameTextHolder.text = thisPlayer.playerName;

        WebRequestHandler.Instance.DownloadSprite(thisPlayer.imageURL, (sprite) =>
        {
            //Debug.Log("hitthis");
            inGameProfilePicPlayer.sprite = sprite;
            // thisPlayerImageFront.enabled = false;
        });

        thisText.text = thisPlayer.playerName; //matchamking player name

        foreach (var ui in GameObject.FindGameObjectsWithTag("SUI"))
            ui.SetActive(false);

        //goScreen.gameObject.SetActive(true);

        canStartGame = true;
        foundOtherPlayer = true;
        //Debug.Log("android data => " + AndroidtoUnityJSON.instance.player_id + ", " + AndroidtoUnityJSON.instance.token + ", " + AndroidtoUnityJSON.instance.user_name + ", " +
        //    AndroidtoUnityJSON.instance.game_id + ", " + AndroidtoUnityJSON.instance.profile_image + ", " + AndroidtoUnityJSON.instance.game_fee + ", " +
        //    AndroidtoUnityJSON.instance.game_mode + ", " + AndroidtoUnityJSON.instance.battle_id);

        PlayerNameText.text = thisPlayer.playerName;

        var attemptNo = 0;

        //attempt check
        WebRequestHandler.Instance.Get(getTournAttemptURL + "/" + AndroidtoUnityJSON.instance.tour_id, (response, status) =>
        {
            GetAttempt attempt = JsonUtility.FromJson<GetAttempt>(response);

            Debug.Log("Attempt(s) remaining: " + attempt.data.remainAttemp);

            attemptNo = int.Parse(attempt.data.remainAttemp);

            if (attempt.data.remainAttemp == AndroidtoUnityJSON.instance.no_of_attempts)
            {
                AndroidtoUnityJSON.instance.isFirst = true;
            }
            else
            {
                AndroidtoUnityJSON.instance.isFirst = false;
            }
        });

        if (AndroidtoUnityJSON.instance.game_mode == "tour")
        {
            //if (AndroidtoUnityJSON.instance.entry_type == "re entry" && AndroidtoUnityJSON.instance.isFirst ||
            //    AndroidtoUnityJSON.instance.entry_type == "re entry paid" || AndroidtoUnityJSON.instance.entry_type == "single entry")
            //{
            //    DeductWallet();
            //}
        }
        else
        {
            DeductWallet();
        }

        levelCounter = UnityEngine.Random.Range(0, LevelsHolders.transform.childCount);
        LevelsHolders.GetChild(levelCounter).gameObject.SetActive(true);
        levelToDestroy = LevelsHolders.GetChild(levelCounter).gameObject;

        yield return new WaitUntil(() => isGameOver);

        thisPlayer.finishedPlaying = true;


        if (AndroidtoUnityJSON.instance.game_mode == "tour" && AndroidtoUnityJSON.instance.entry_type == "single entry")
        {
            ChanceLeft.gameObject.SetActive(false);
            ReplayBtn.transform.parent.gameObject.SetActive(false);
            Footer_1.SetActive(true);
        }

        else if (AndroidtoUnityJSON.instance.game_mode == "tour")
        {
            //if (attemptNo <= 0)
            //{
            //ReplayBtn.transform.parent.gameObject.SetActive(false);
            
            Footer_1.SetActive(true);
            //}

            ChanceLeft.gameObject.SetActive(true);
            ChanceLeft.text = "Chances Left: " + attemptNo;
        }
        else if (AndroidtoUnityJSON.instance.game_mode == "battle")
        {
            ChanceLeft.gameObject.SetActive(false);
        }



        if (AndroidtoUnityJSON.instance.mm_player == "1")
        {
            LosserNameText.transform.parent.gameObject.SetActive(false);
        }



        if (float.Parse(AndroidtoUnityJSON.instance.game_fee) <= 0) //|| AndroidtoUnityJSON.instance.entry_type == "re entry" && AndroidtoUnityJSON.instance.game_mode == "tour")
        {
            ReloadPrice.text = "FREE";
        }
        else
        {
            ReloadPrice.text = /* "?" +*/ AndroidtoUnityJSON.instance.game_fee;
        }



        //EndUIFetchingScoreObj.SetActive(true);
        EndUI.SetActive(true);
        

        foreach (var ui in EndUIGameObj)
        {
            Footer.SetActive(false);
            if (ui.gameObject.name != LosserNameText.transform.parent.gameObject.name && AndroidtoUnityJSON.instance.mm_player == "1")           
                ui.SetActive(true);           
        }

        while (foundWinner == false)
        {
            // Debug.Log("entered");
            yield return new WaitForSecondsRealtime(0.5f);
            //thisPlayer.score = ScoringManager.instance.score;
            thisPlayer.score = score;
            if (foundWinner == false)
            {
                foundWinner = true;
                thisPlayer.iWon = true;
                sendThisPlayerData.player_id = AndroidtoUnityJSON.instance.player_id;
                sendThisPlayerData.room_id = "0";
                sendThisPlayerData.winning_details.winningPlayerScore = thisPlayer.score.ToString();
                sendThisPlayerData.winning_details.winningPlayerID = AndroidtoUnityJSON.instance.player_id;
                sendThisPlayerData.winning_details.lossingPlayerID = "0";
                sendThisPlayerData.winning_details.lossingPlayerScore = "0";
                WinnerNameText.text = thisPlayer.playerName;
                LosserNameText.text = otherPlayer.playerName;
                displayWinnerOrLosserText.text = "YOU SCORE";
                displayScoreTextAtEnd.text = thisPlayer.score.ToString();
                scoreOnBoard.text = thisPlayer.score.ToString();
                opponentScoreOnBoard.text = otherPlayer.score.ToString();

                RankNo.text = "1";
                sendThisPlayerData.game_status = "WIN";

                //send tournament data through api

                sendThisPlayerData.player_id = "0";
                sendThisPlayerData.winning_details.thisplayerScore = thisPlayer.score;
                sendThisPlayerData.wallet_amt = AndroidtoUnityJSON.instance.game_fee;
                sendThisPlayerData.game_mode = AndroidtoUnityJSON.instance.game_mode;
                sendThisPlayerData.game_id = AndroidtoUnityJSON.instance.game_id;

                if (AndroidtoUnityJSON.instance.game_mode == "tour")
                    sendThisPlayerData.battle_tournament_id = AndroidtoUnityJSON.instance.tour_id;
                else if (AndroidtoUnityJSON.instance.game_mode == "battle")
                    sendThisPlayerData.battle_tournament_id = AndroidtoUnityJSON.instance.battle_id;
                
                sendThisPlayerData.game_end_time = GetSystemTime();

                string sendWinningDetailsData = JsonUtility.ToJson(winningDetails);
                string sendNewData = JsonUtility.ToJson(sendThisPlayerData);

                //Debug.Log(sendNewData + "sendNewData");
                WebRequestHandler.Instance.Post(sendDataURL, sendNewData, (response, status) =>
                {
                    Debug.Log(response + "HitNewApi");
                });

                isDataSend = true;
                //StartCoroutine(CallLeaveRoom());


            }
        }
    }

    public string GetSystemTime()
    {
        int hr = DateTime.Now.Hour;
        int min = DateTime.Now.Minute;
        int sec = DateTime.Now.Second;

        int year = DateTime.Now.Year;
        int month = DateTime.Now.Month;
        int day = DateTime.Now.Day;

        string format = string.Format("{0}:{1}:{2} {3}:{4}:{5}", year, month, day, hr, min, sec);

        return format;
    }


    #region Gameplay code
    public void changeRound()
    {
        if (roundIndex > 2)
        {
            return;
        }
        this.transform.GetChild(roundIndex).gameObject.SetActive(false);
        roundIndex++;
        this.transform.GetChild(roundIndex).gameObject.SetActive(true);
        LevelsHolders = this.transform.GetChild(roundIndex);
        Debug.Log(LevelsHolders.gameObject.name);
        levelCounter = UnityEngine.Random.Range(0, LevelsHolders.transform.childCount);
        LevelsHolders.GetChild(levelCounter).gameObject.SetActive(true);
        levelToDestroy = LevelsHolders.GetChild(levelCounter).gameObject;


    }
    public void ChangeLevels()
    {
        StartCoroutine(StarwithDelay());

    }

    IEnumerator StarwithDelay()
    {
        yield return new WaitForSeconds(0.4f);
        Destroy(levelToDestroy);
        yield return new WaitForSeconds(0.1f);
      //  Debug.Log("levelCounter " + levelCounter);

        levelCounter=UnityEngine. Random.Range(0, LevelsHolders.transform.childCount);
        //if (levelCounter > LevelsHolders.transform.childCount - 1)
        //{
        //    levelCounter = 0;
        //}
       // Debug.Log("levelCounter " + levelCounter);
        LevelsHolders.GetChild(levelCounter).gameObject.SetActive(true);
        levelToDestroy = LevelsHolders.GetChild(levelCounter).gameObject;
      //  Debug.Log("level GameObject name " + LevelsHolders.GetChild(levelCounter).gameObject.name);
        //change level

    }

    #endregion

    #region Ui Related Codes
    public void UpdateScore(int incomingScore)
    {
        score = score + incomingScore;
        scoreTextHolder.text = score.ToString();
        ChangeLevels();
    }

    bool isButtonShown, isTutorialViewed;
    public bool isRoundUiIsDisplayed=false;

    //public void OnTutorialViewed(Vector2 value)
    //{
    //    Debug.Log(value);
    //    if (value.x >= 1)
    //    {
    //        isButtonShown = true;
    //        isTutorialViewed = true;
    //        playButton.gameObject.SetActive(true);
    //    }

    //}

    //public void closeTutorial()
    //{
    //    tutorialPanel.SetActive(false);
    //    Time.timeScale = 1;
    //}
    #endregion

    public void RoundUIChange()
    {
        if (roundIndex < 2 )
        {
            //Debug.Log(SlingShot.instance.GetBall());
            SlingShot.instance.GetBall().GetComponent<Collider2D>().enabled = false;
            //SlingShot.instance.GetBall().GetComponent<SpriteRenderer>().enabled = false;
            //SlingShot.instance.GetBall().GetComponent<TrailRenderer>().enabled = false;
            //SlingShot.instance.GetBall().transform.GetChild(0).GetComponent<TrailRenderer>().enabled = false;
            //SlingShot.instance.GetBall().transform.GetChild(1).GetComponent<ParticleSystem>().Stop();
            isRoundUiIsDisplayed = true;
            SlingShot.instance.shoot();
            roundAnima.Play("round");
            roundAnima.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = $"ROUND {roundIndex + 1} is completed";
            SlingShot.instance.transform.GetComponent<BoxCollider2D>().enabled = false;
            StartCoroutine(RoundUiDisplay());

        }

        if (roundIndex == 2)
        {
            isGameOver = true;
            //startReloadGameTime();
        }
    }

    IEnumerator RoundUiDisplay(){
        yield return new WaitForSecondsRealtime(2f);
        isRoundUiIsDisplayed=false;
        SlingShot.instance.transform.GetComponent<BoxCollider2D>().enabled = true;

    }



    public void Spawn(Vector2 spawnPosition, string name)
    {
        if (name == "target40")
        {
            spawnEffect1.transform.position = spawnPosition;
            spawnEffect1.Play();
        }
        else if(name == "target80"){
            spawnEffect2.transform.position = spawnPosition;
            spawnEffect2.Play(); }
    }

    public void startReloadGameTime()
    {
        StartCoroutine(EndGameTimeFill());
    }

    IEnumerator EndGameTimeFill()
    {
        if (canRestart = true)
        {

            while (true)
            {

                endGameTimer.fillAmount += 0.01f;
                yield return new WaitForSecondsRealtime(0.2f);
                if (endGameTimer.fillAmount == 1)
                {
                    SceneManager.LoadScene(0);
                }
            }
        }
        
        
       
    }

    public void ReloadScene()
    {
        //LeaveRoom();
        //wallet check
        float balance = 0f;

        walletInfoData = JsonUtility.ToJson(walletInfo);
        WebRequestHandler.Instance.Post(walletInfoURL, walletInfoData, (response, status) =>
        {
            WalletInfo walletInfoResponse = JsonUtility.FromJson<WalletInfo>(response);
            balance = float.Parse(walletInfoResponse.data.cash_balance);
            Debug.Log(balance + " <= replay check balance");

            if (balance >= float.Parse(AndroidtoUnityJSON.instance.game_fee))
                canRestart = true;
            else
                canRestart = false;

            if (canRestart)
                SceneManager.LoadScene(0, LoadSceneMode.Single);
            else
                NoBalPop.SetActive(true);

            //Debug.Log("CANT START, NOT ENOUGH BALANCE!"); //show no bal popup
        });
    }

    public void CancelApplication()
    {
       // hasExitedRoom = true;
       // LeaveRoom();
        Application.Quit();
    }


    private void OnApplicationPause(bool pause)
    {
        if(!pause)
        {
            if (canStartGame && AndroidtoUnityJSON.instance.game_mode == "battle")
            {
                WebRequestHandler.Instance.Get(serverURL + "checkIfLeftRoom/" + myRoomId, (response, status) =>
                {
                    Debug.Log(response);
                    if (response == "canQuit->" + myRoomId)
                    {
                        //Application.Quit();
                        StartCoroutine(QuitApplication());
                    }

                });
            }
        }
    }

    void Update()
    {
        float offset = Time.time * scrollSpeed;
        backGroundAnim.SetTextureOffset("_MainTex", new Vector2(offset, 0));


    }


    IEnumerator QuitApplication()
    {

        if (!isDataSend)
        {
            sendThisPlayerData.player_id = otherPlayer.playerIdToBeSentorReceived;
            sendThisPlayerData.winning_details.thisplayerScore = score;
            sendThisPlayerData.wallet_amt = AndroidtoUnityJSON.instance.game_fee;
            sendThisPlayerData.game_mode = AndroidtoUnityJSON.instance.game_mode;
            sendThisPlayerData.game_id = AndroidtoUnityJSON.instance.game_id;

            if (AndroidtoUnityJSON.instance.game_mode == "tour")
                sendThisPlayerData.battle_tournament_id = AndroidtoUnityJSON.instance.tour_id;
            else if (AndroidtoUnityJSON.instance.game_mode == "battle")
                sendThisPlayerData.battle_tournament_id = AndroidtoUnityJSON.instance.battle_id;

            sendThisPlayerData.game_end_time = GetSystemTime();
            sendThisPlayerData.game_status = "LEFT";

            sendWinningDetailsData = JsonUtility.ToJson(winningDetails);
            sendNewData1 = JsonUtility.ToJson(sendThisPlayerData);
            WebRequestHandler.Instance.Post(sendDataURL, sendNewData1, (response, status) =>
            {
                Debug.Log(response + "HitNewApi");
            });

            isDataSend = true;

            Debug.Log("bella ciao");
            Application.Quit();

            yield return new WaitForSeconds(0.2f);

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif


        }

    }


    public void DeductWallet( )
    {
        walletUpdate.amount = AndroidtoUnityJSON.instance.game_fee;
        walletUpdate.type = AndroidtoUnityJSON.instance.game_mode;

        if (AndroidtoUnityJSON.instance.game_mode == "tour")
            walletUpdate.game_id = AndroidtoUnityJSON.instance.tour_id;
        else if (AndroidtoUnityJSON.instance.game_mode == "battle")
            walletUpdate.game_id = AndroidtoUnityJSON.instance.battle_id;

        string mydata = JsonUtility.ToJson(walletUpdate);
        WebRequestHandler.Instance.Post(walletUpdateURL, mydata, (response, status) =>
        {
            Debug.Log(response + " sent wallet update");
        });
    }

    public void Reload()
    {
        //wallet check
        float balance = 0f;

        walletInfoData = JsonUtility.ToJson(walletInfo);
        WebRequestHandler.Instance.Post(walletInfoURL, walletInfoData, (response, status) =>
        {
            WalletInfo walletInfoResponse = JsonUtility.FromJson<WalletInfo>(response);
            balance = float.Parse(walletInfoResponse.data.cash_balance);
            Debug.Log(balance + " <= replay check balance");

            if (balance >= float.Parse(AndroidtoUnityJSON.instance.game_fee))
                canRestart = true;
            else
                canRestart = false;

            if (canRestart)
                SceneManager.LoadScene(0, LoadSceneMode.Single);
            else
                NoBalPop.SetActive(true);

            //Debug.Log("CANT START, NOT ENOUGH BALANCE!"); //show no bal popup
        });
    }

}
