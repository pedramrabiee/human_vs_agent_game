using UnityEngine;
using System.Collections;
using System.Collections.Generic;  //lets us use lists
using System.Xml.Serialization; //access xmlserializer
using System.IO; //file management
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [HideInInspector] public string directory;

    [SerializeField] public float xMax;
    [SerializeField] public float yMax;
    [HideInInspector] public int playerTargetNo;
    [HideInInspector] public float playerTargetDistance;

    [SerializeField] public GameObject popUpCanvas;

    [SerializeField] public Text targetText;
    [SerializeField] public Text targetTextOnCountDown;

    [SerializeField] public Text timerText;
    [SerializeField] public Text scoreText;
    [SerializeField] public Text highScoreText;
    [SerializeField] public Text totalHighScoreText;
    [SerializeField] public Text timeScoreText;
    [SerializeField] public Text collisionPenaltyText;
    [SerializeField] public Text colNoText;

    [SerializeField] public Text playerNameText;
    [SerializeField] public Text playerNoText;
    [SerializeField] public Text playerDayNoText;
    [SerializeField] public Text playerTrialNoText;



    [HideInInspector] public float score = 10000f;
    [HideInInspector] public float timeScore = 10000f;
    [HideInInspector] public float collisionPenalty = 0f;
    [HideInInspector] public float colNo = 0f;


    HighScore totalHighScore = new HighScore();


    public static GameManager _instance;

    [HideInInspector] public CurrentPlayer currentPlayer = new CurrentPlayer();

    float elapsedTime = 0f;
    float startTime;


    private void Awake()
    {
        Time.timeScale = 1;

        directory = PlayerPrefs.GetString("directory");
        // Creating Instance
        if (_instance == null)
        {
            _instance = this;
        }

        // Game Loading Options
        GameStarter();

        playerTargetNo = Random.Range(1,5);
        playerTargetDistance = GameObject.Find("Target1").GetComponent<Transform>().position.magnitude;

    }
    // Start is called before the first frame update
    void Start()
    {
        targetText.text = "Go to " + playerTargetNo.ToString();
        targetTextOnCountDown.text = "Go to " + playerTargetNo.ToString();
        highScoreText.text = Mathf.Round(currentPlayer.highScore).ToString();
        playerNameText.text = currentPlayer.playerName;
        playerNoText.text = currentPlayer.playerNo.ToString();
        playerDayNoText.text = (currentPlayer.lastDay+1).ToString();
        playerTrialNoText.text = currentPlayer.lastTrial.ToString();
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("GameManager");
        timerText.text = (Mathf.Round(elapsedTime * 10f) / 10f).ToString();

        timeScoreText.text = Mathf.Round(timeScore).ToString();
        collisionPenaltyText.text = Mathf.Round(collisionPenalty).ToString();
        colNoText.text = "("+ Mathf.Round(colNo).ToString() + ")";
        scoreText.text = Mathf.Round(score).ToString();


        if (Input.GetKeyDown("escape"))
        {
            if (!popUpCanvas.activeInHierarchy)
            {
                popUpCanvas.SetActive(true);
                Time.timeScale = 0;
            }
            else
            {
                popUpCanvas.SetActive(false);
                Time.timeScale = 1;
            }
        }
        elapsedTime = Time.time - startTime;

    }


    void HighScorePrinter()
    {
        string file = directory + "HighScore.xml";
        if (System.IO.File.Exists(file))
        {
            XmlSerializer serializerHighScore = new XmlSerializer(typeof(HighScore));
            FileStream streamHighScore = new FileStream(file, FileMode.Open);
            totalHighScore = (serializerHighScore.Deserialize(streamHighScore) as HighScore);
            streamHighScore.Close();
        }
        else
        {
            totalHighScore.highScore = 0f;

            XmlSerializer serializerHighScore = new XmlSerializer(typeof(HighScore));
            FileStream streamHighScore = new FileStream(file, FileMode.Create);
            serializerHighScore.Serialize(streamHighScore, totalHighScore);
            streamHighScore.Close();
        }
        totalHighScoreText.text = Mathf.Round(totalHighScore.highScore).ToString();
    }

    void GameStarter()
    {
        // Load CurrentPlayer Data

        string file = directory + "CurrentPlayer.xml";

        XmlSerializer serializerCurrentPlayer = new XmlSerializer(typeof(CurrentPlayer));
        FileStream streamCurrentPlayer = new FileStream(file, FileMode.Open);
        currentPlayer = serializerCurrentPlayer.Deserialize(streamCurrentPlayer) as CurrentPlayer;
        streamCurrentPlayer.Close();
        if (currentPlayer.gameMode == 2)
        {
            currentPlayer.lastTrial = currentPlayer.singleStartNo-1;
            if (currentPlayer.singleStartNo % 10 == 0)
            {
                currentPlayer.lastDay = Mathf.FloorToInt(currentPlayer.singleStartNo / 10) - 1;
            }
            else
            {
                currentPlayer.lastDay = Mathf.FloorToInt(currentPlayer.singleStartNo / 10);
            }
            currentPlayer.gameMode = 0;
        }

        if (currentPlayer.gameMode == 0)
        {
            if (currentPlayer.lastDay < 4)

            {
                if (currentPlayer.lastTrial < (currentPlayer.lastDay+1) * 10)
                {
                    currentPlayer.lastTrial++;
                    HighScorePrinter();
                }
                else
                {
                    currentPlayer.lastDay++;
                    DayEndCondition();
                }
            }
            else
            {
                SceneManager.LoadScene("MainMenu");
            }

        }else if (currentPlayer.gameMode == 1)
        {
            currentPlayer.lastTrial = currentPlayer.singleStartNo;
            HighScorePrinter();
        }
    }

    public void TrialEndCondition()
    {
        HighScoreCheck();

        // Create current player file
        XmlSerializer serializerCurrentPlayer = new XmlSerializer(typeof(CurrentPlayer));
        FileStream streamCurrentPlayer = new FileStream(directory + "CurrentPlayer.xml", FileMode.Create);
        serializerCurrentPlayer.Serialize(streamCurrentPlayer, currentPlayer);
        streamCurrentPlayer.Close();


        // Save Player Data
        SavePlayerData savePlayerData = new SavePlayerData();

        savePlayerData.playerNo = currentPlayer.playerNo;
        savePlayerData.playerName = currentPlayer.playerName;
        savePlayerData.lastDay = currentPlayer.lastDay;
        savePlayerData.lastTrial = currentPlayer.lastTrial;
        savePlayerData.highScore = currentPlayer.highScore;


        // Create current player file
        string file = directory + "sub" + savePlayerData.playerNo + "/sub" + savePlayerData.playerNo + "_playerData.xml";
        XmlSerializer serializerSavePlayerData = new XmlSerializer(typeof(SavePlayerData));
        FileStream streamSavePlayerData = new FileStream(file, FileMode.Create);
        serializerSavePlayerData.Serialize(streamSavePlayerData, savePlayerData);
        streamSavePlayerData.Close();
        //SceneManager.LoadScene("MainMenu");

        SceneManager.LoadScene("SceneManager");
    }



    public void DayEndCondition()
    {
        SavePlayerData savePlayerData = new SavePlayerData();

        savePlayerData.playerNo = currentPlayer.playerNo;
        savePlayerData.playerName = currentPlayer.playerName;
        savePlayerData.lastDay = currentPlayer.lastDay;
        savePlayerData.lastTrial = currentPlayer.lastTrial;
        savePlayerData.highScore = currentPlayer.highScore;

        // Create current player file
        string file = directory + "sub" + savePlayerData.playerNo + "/sub" + savePlayerData.playerNo + "_playerData.xml";
        XmlSerializer serializerSavePlayerData = new XmlSerializer(typeof(SavePlayerData));
        FileStream streamSavePlayerData = new FileStream(file, FileMode.Create);
        serializerSavePlayerData.Serialize(streamSavePlayerData, savePlayerData);
        streamSavePlayerData.Close();
        SceneManager.LoadScene("MainMenu");
    }

    public void HighScoreCheck()
    {
        if (score > currentPlayer.highScore)
        {
            currentPlayer.highScore = score;
        }
        if (score > totalHighScore.highScore)
        {
            totalHighScore.highScore = score;

            string file = directory + "HighScore.xml";
            XmlSerializer serializerHighScore = new XmlSerializer(typeof(HighScore));
            FileStream streamHighScore = new FileStream(file, FileMode.Create);
            serializerHighScore.Serialize(streamHighScore, totalHighScore);
            streamHighScore.Close();
        }
    }

}
