using UnityEngine;
using System.Collections;
using System.Collections.Generic;  //lets us use lists
using System.Xml; // basic xml attributes
using System.Xml.Serialization; //access xmlserializer
using System.IO; //file management
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ResumeGameManager : MonoBehaviour
{
    [HideInInspector] public string directory;
    //public ExistingPlayerList existingPlayers;
    //public List<int> existingPlayers = new List<int>();
    [SerializeField] public Text errorBar;
    [SerializeField] public Text playerNoInput;
    [SerializeField] public Text playerNameText;
    [SerializeField] public Text singleStartNoInput;
    [SerializeField] GameObject singleStartNoBox;

    [HideInInspector] public CurrentPlayer currentPlayerData = new CurrentPlayer();
    bool isError = true;
    //[HideInInspector] public SavePlayerData playerData = new SavePlayerData();

    [SerializeField] public Dropdown gameModeDropDown;
    // Start is called before the first frame update


    private void Awake()
    {
        directory = PlayerPrefs.GetString("directory");
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private List<int> LoadExistingPlayer()
    {
        List<int> existingPlayers = new List<int>();
        XmlSerializer serializer = new XmlSerializer(typeof(List<int>));
        FileStream stream = new FileStream(directory + "ExistingPlayers.xml", FileMode.Open);
        existingPlayers = serializer.Deserialize(stream) as List<int>;
        stream.Close();
        return existingPlayers;
    }


    private SavePlayerData LoadExistingPlayerData(int playerNo)
    {
        string dir = directory + "sub" + playerNo;
        string file = dir + "/sub" + playerNo + "_playerData.xml";
        SavePlayerData existingPlayersData = new SavePlayerData();
        XmlSerializer serializer = new XmlSerializer(typeof(SavePlayerData));
        FileStream stream = new FileStream(file, FileMode.Open);

        existingPlayersData = serializer.Deserialize(stream) as SavePlayerData;

        stream.Close();
        return existingPlayersData;
    }

    public void CheckExistingPlayer()
    {
        string playerNoString = playerNoInput.text;
        int playerNo = int.Parse(playerNoString);

        List<int> existingPlayers = new List<int>();
        string file = directory + "ExistingPlayers.xml";
        existingPlayers = LoadExistingPlayer();

        if (!CheckPlayerExistence(existingPlayers, playerNo))
        {
            errorBar.text = "Error: Player Doesn't Exist";
            isError = true;
            playerNameText.text = "";
        }
        else
        {
            errorBar.text = "";
            playerNameText.text = LoadExistingPlayerData(playerNo).playerName;
            isError = false;
        }
    }

    public void CheckStartNoValidity()
    {
        if (int.Parse(singleStartNoInput.text) > 40 || int.Parse(singleStartNoInput.text) <= 0)
        {
            errorBar.text = "Error: Invalid Starting Number";
            isError = true;
        }
        else
        {
            errorBar.text = "";
            isError = false;
        }
    }

    public bool CheckPlayerExistence(List<int> exstply, int playerNo)
    {
        bool res = exstply.Contains(playerNo);
        return res;
    }

    public void getNewPlayerInfo()
    {
        int gameMode = gameModeDropDown.value;

        if (playerNoInput.text == "")
        {
            errorBar.text = "Error: Enter Player Number";
        }
        else if (singleStartNoInput.text == "" && !(gameMode == 0))
        {
            errorBar.text = "Error: Enter Starting Number";
        }
        else if (!isError)
        {
            int playerNo = int.Parse(playerNoInput.text);

            if (gameMode == 0)
            {
                currentPlayerData.singleStartNo = 0;

            }
            if (gameMode == 1 || gameMode == 2)
            {
                int singleStartNo = int.Parse(singleStartNoInput.text);
                currentPlayerData.singleStartNo = singleStartNo;
            }

            string dir = directory + "sub" + playerNo;
            string file = dir + "/sub" + playerNo + "_playerData.xml";
            SavePlayerData existingPlayersData = new SavePlayerData();

            XmlSerializer serializer = new XmlSerializer(typeof(SavePlayerData));
            FileStream stream = new FileStream(file, FileMode.Open);
            existingPlayersData = serializer.Deserialize(stream) as SavePlayerData;
            stream.Close();
            

            string playerName = existingPlayersData.playerName;



            currentPlayerData.playerNo = playerNo;
            currentPlayerData.playerName = playerName;
            currentPlayerData.gameMode = gameMode;
            currentPlayerData.highScore = existingPlayersData.highScore;
            currentPlayerData.lastDay = existingPlayersData.lastDay;
            currentPlayerData.lastTrial = existingPlayersData.lastTrial;
            
            // Create current player file
            XmlSerializer serializerCurrentPlayer = new XmlSerializer(typeof(CurrentPlayer));
            FileStream streamCurrentPlayer = new FileStream(directory + "CurrentPlayer.xml", FileMode.Create);
            serializerCurrentPlayer.Serialize(streamCurrentPlayer, currentPlayerData);
            streamCurrentPlayer.Close();

            SceneManager.LoadScene("SceneManager");

        }

    }

    public void DropDownUpdater()
    {
        int gameMode = gameModeDropDown.value;

        if (gameMode == 0)
        {
            singleStartNoBox.SetActive(false);

        }
        if (gameMode == 1 || gameMode == 2)
        {
            singleStartNoBox.SetActive(true);           
        }
    }
}
