using UnityEngine;
using System.Collections;
using System.Collections.Generic;  //lets us use lists
using System.Xml; // basic xml attributes
using System.Xml.Serialization; //access xmlserializer
using System.IO; //file management
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NewPlayerManager : MonoBehaviour
{
    [HideInInspector] public string directory;
    //public ExistingPlayerList existingPlayers;
    //public List<int> existingPlayers = new List<int>();
    [SerializeField] public Text errorBar;
    [SerializeField] public Text playerNoInput;
    [SerializeField] public Text playerNameInput;
    [SerializeField] public Text singleStartNoInput;
    [SerializeField] GameObject singleStartNoBox;

    [SerializeField] public InputField playerNameInputField;

    [HideInInspector] public CurrentPlayer currentPlayerData  = new CurrentPlayer();
    [HideInInspector] public SavePlayerData playerData = new SavePlayerData();

    [SerializeField] public Dropdown gameModeDropDown;
    bool isError = false;

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


    public void AddToExistingPlayer()
    {
        string playerNoString = playerNoInput.text;
        int playerNo = int.Parse(playerNoString);

        List<int> existingPlayers = new List<int>();
        string file = directory + "ExistingPlayers.xml";

        if (System.IO.File.Exists(file))
        {
            existingPlayers = LoadExistingPlayer();
            if (CheckPlayerExistence(existingPlayers, playerNo))
            {
                errorBar.text = "Error: Player Exists";
                playerNameInputField.interactable = false;
            }
            else
            {
                errorBar.text = "";
                playerNameInputField.interactable = true;
            }
        }
        else
        {
            playerNameInputField.interactable = true;
        }
    }

    public bool CheckPlayerExistence(List<int> exstply, int playerNo)
    {
        bool res = exstply.Contains(playerNo);
        return res;
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

    public void getNewPlayerInfo()
    {

        int gameMode = gameModeDropDown.value;

        if (playerNoInput.text == "")
        {
            errorBar.text = "Error: Enter Player Number";
        }else if (playerNameInput.text == "")
        {
            errorBar.text = "Error: Enter Player Name";
        }
        else if (singleStartNoInput.text == "" && !(gameMode == 0))
        {
            errorBar.text = "Error: Enter Starting Number";
        }
        else if(!isError)
        {
            int playerNo = int.Parse(playerNoInput.text);
            string playerName = playerNameInput.text;

            if (gameMode == 0)
            {
                currentPlayerData.singleStartNo = 0;
            }
            if (gameMode == 1 || gameMode == 2)
            {
                int singleStartNo = int.Parse(singleStartNoInput.text);
                currentPlayerData.singleStartNo = singleStartNo;
            }

            string file = directory + "ExistingPlayers.xml";
            List<int> existingPlayers = new List<int>();
            if (System.IO.File.Exists(file))
            {
                existingPlayers = LoadExistingPlayer();
                existingPlayers.Add(playerNo);
                XmlSerializer serializerExistingPlayer = new XmlSerializer(typeof(List<int>));
                FileStream streamExistingPlayer = new FileStream(directory + "ExistingPlayers.xml", FileMode.Create);
                serializerExistingPlayer.Serialize(streamExistingPlayer, existingPlayers);
                streamExistingPlayer.Close();
            }
            else
            {
                existingPlayers.Add(playerNo);
                XmlSerializer serializerExistingPlayer = new XmlSerializer(typeof(List<int>));
                FileStream streamExistingPlayer = new FileStream(directory + "ExistingPlayers.xml", FileMode.Create);
                serializerExistingPlayer.Serialize(streamExistingPlayer, existingPlayers);
                streamExistingPlayer.Close();
            }
            
            currentPlayerData.playerNo = playerNo;
            currentPlayerData.playerName = playerName;
            currentPlayerData.gameMode = gameMode;
            currentPlayerData.highScore = 0f;
            currentPlayerData.lastDay = 0;
            currentPlayerData.lastTrial = 0;

            // Create current player file
            XmlSerializer serializer = new XmlSerializer(typeof(CurrentPlayer));
            FileStream stream = new FileStream(directory + "CurrentPlayer.xml", FileMode.Create);
            serializer.Serialize(stream, currentPlayerData);
            stream.Close();

            // Create player folder and player data file
            string dir = directory + " /sub" + playerNo;
            Directory.CreateDirectory(dir);

            playerData.playerNo = playerNo;
            playerData.lastDay = 0;
            playerData.lastTrial = 0;
            playerData.highScore = 0f;
            playerData.playerName = playerName;

            XmlSerializer serializerPlayerData = new XmlSerializer(typeof(SavePlayerData));
            FileStream streamPlayerData = new FileStream(dir + "/sub" + playerNo + "_playerData.xml", FileMode.Create);
            serializerPlayerData.Serialize(streamPlayerData, playerData);
            streamPlayerData.Close();

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
