using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentSpawner : MonoBehaviour
{
    float minCircleRadius;
    float maxCircleRadius;
    [SerializeField] public GameObject[] Agents;
    [SerializeField] public GameObject[] Targets;
    float xMax, yMax, playerTargetDistance, radius;    
    public List<Vector2> agentSpawnedPos = new List<Vector2>();

    public static GameObject[] _agentInstance;

    float distanceFromTheWall = 0.5f;


    // Start is called before the first frame update
    void Start()
    {
        minCircleRadius = PlayerPrefs.GetFloat("minSpawningRadius");
        maxCircleRadius = PlayerPrefs.GetFloat("maxSpawningRadius");

        xMax = GameManager._instance.GetComponent<GameManager>().xMax;
        yMax = GameManager._instance.GetComponent<GameManager>().yMax;
        playerTargetDistance = GameManager._instance.GetComponent<GameManager>().playerTargetDistance;
        radius = GameObject.Find("Player").GetComponent<CircleCollider2D>().radius;
        SpawnAgents();
        _agentInstance = GameObject.FindGameObjectsWithTag("Agent");
        GameDataSaver.instance.AgentInstancer();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnAgents()
    {
        float xPosRand, yPosRand;
        bool failed;
        for (int i = 0 ; i < 4 ; i++)
        {
            for (int j = 0; j < (Agents.Length / 4); j++)
            {
                // Making sure that no agent instantiated close to other agent and player and also all agents
                // initial position to their targets are farther than the initial distance of the player to its target

                Vector3 targetPos = GameObject.Find("Target" + (i + 1).ToString()).GetComponent<Transform>().position;
                do
                {
                    failed = false;
                    xPosRand = Random.Range(-xMax, xMax);
                    yPosRand = Random.Range(-yMax, yMax);
                    Vector2 RandPos = new Vector2(xPosRand, yPosRand);
                    if (RandPos.magnitude < radius * 2)
                    {
                        failed = true;
                    }
                    else if (Mathf.Abs(xPosRand - (-(xMax + radius))) < distanceFromTheWall || Mathf.Abs(xPosRand - ((xMax + radius))) < distanceFromTheWall || Mathf.Abs(yPosRand - (-(yMax + radius))) < distanceFromTheWall || Mathf.Abs(yPosRand - ((yMax + radius))) < distanceFromTheWall)
                    {
                        failed = true;
                    }
                    else 
                    {
                        foreach (Vector2 ap in agentSpawnedPos)
                        {
                            if (Vector2.Distance(new Vector2(ap.x, ap.y), new Vector2(xPosRand, yPosRand)) < radius * 2) { failed = true; continue; }
                        }                        
                    }
                    
                    //Checking distance with player (0,0)                
                } while (Vector2.Distance(new Vector2(targetPos.x, targetPos.y), new Vector2(xPosRand, yPosRand)) > playerTargetDistance* maxCircleRadius || Vector2.Distance(new Vector2(targetPos.x, targetPos.y), new Vector2(xPosRand, yPosRand)) < playerTargetDistance * minCircleRadius || failed);   
                
                Instantiate(Agents[i * Agents.Length / 4 + j], new Vector3(xPosRand, yPosRand, transform.position.z), transform.rotation);
                Agents[i * Agents.Length / 4 + j].GetComponent<AgentController>().target = Targets[i].GetComponent<Transform>().position;
                Agents[i * Agents.Length / 4 + j].GetComponent<AgentController>().targetNo = i+1;
                Agents[i * Agents.Length / 4 + j].GetComponent<AgentController>().agentNo = i * Agents.Length / 4 + j + 1;
                agentSpawnedPos.Add(new Vector2(xPosRand, yPosRand));
            }
        }        
    }

}
