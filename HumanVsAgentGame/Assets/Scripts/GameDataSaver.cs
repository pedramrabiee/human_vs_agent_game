using UnityEngine;
using System.Collections;
using System.Collections.Generic;  //lets us use lists
using System.Xml.Serialization; //access xmlserializer
using System.IO; //file management

public class GameDataSaver : MonoBehaviour
{
    public SaveDataSubject subjectData = new SaveDataSubject();
    public SaveDataAgent agentData = new SaveDataAgent();
    public static GameDataSaver instance;
    public static GameObject _subjectInstance;
    public static GameObject[] _agentsDataInstance;
    public static GameObject _agentTest;
    // ---------------------------------------------------------------------------------------------------------------------------------
    int subNo;
    int trialNo;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        _subjectInstance = GameObject.Find("Player");

    }

    public void AgentInstancer()
    {
        _agentsDataInstance = GameObject.FindGameObjectsWithTag("Agent");
        //Debug.Log(_agentsDataInstance.Length);
    }


    public void Save()
    {
        string dir = GameManager._instance.directory + "sub"+ GameManager._instance.currentPlayer.playerNo  + "/trial" + GameManager._instance.currentPlayer.lastTrial;
        Directory.CreateDirectory(dir);

        subjectData = _subjectInstance.GetComponent<PlayerController>().save;
        
        //open a new XML fileXmlSerializer serializer = new XmlSerializer(typeof(ItemDatabase));
        XmlSerializer serializer = new XmlSerializer(typeof(SaveDataSubject));
        string file = dir + "/Subject_data_Sub_" + GameManager._instance.currentPlayer.playerNo + "_Trial_" + GameManager._instance.currentPlayer.lastTrial + ".xml";
        FileStream stream = new FileStream(file, FileMode.Create);
        // when you create a file you are overwriting the existing file
        serializer.Serialize(stream, subjectData);
        stream.Close();

        //Debug.Log(_agentsDataInstance.Length + "    in function");
        int counter = 1;
        foreach (GameObject gb in _agentsDataInstance)
        {
            //Debug.Log("counter = "+ counter);
            agentData = gb.GetComponent<AgentController>().save;

            if (agentData.completed == false)
            {
                agentData.completeTime = subjectData.completeTime;
            }
            XmlSerializer serializerAgent = new XmlSerializer(typeof(SaveDataAgent));
            file = dir + "/Agent_data_Sub_" + GameManager._instance.currentPlayer.playerNo + "_Agt_" + agentData.agentNo + "_Trial_" + GameManager._instance.currentPlayer.lastTrial + ".xml";
            FileStream streamAgent = new FileStream(file, FileMode.Create);          
            serializerAgent.Serialize(streamAgent, agentData);
            streamAgent.Close();
            counter++;

        }

    }

}
