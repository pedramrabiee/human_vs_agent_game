using UnityEngine;
using System.Collections;
using System.Collections.Generic;  //lets us use lists
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    #region Attributes

    int targetNo;
    float xMax, yMax;
    float elapsedTime = 0f;
    float startTime;

    [HideInInspector] public SaveDataSubject save = new SaveDataSubject();
    [SerializeField] GameObject loadingCanvas;
    int colNo = 0;

    float xPos, yPos, xVel, yVel, xAcc, yAcc;
    [HideInInspector] public float xVelPre, yVelPre;

    //public List<float> xPosList = new List<float>();

    private bool enterContactX = false;
    private bool enterContactY = false;
    private bool isLeavingX = false;
    private bool isLeavingY = false;

    float PlayerGain, cd;
    bool bouncingFromTheWall;

    #endregion

    #region MonoBehaviour API


    private void Awake()
    {

    }
    // Start is called before the first frame update
    void Start()
    {
        PlayerGain = PlayerPrefs.GetFloat("playerGain");
        cd = PlayerPrefs.GetFloat("cdPlayer");
        bouncingFromTheWall = ToBool(PlayerPrefs.GetString("bouncingFromTheWallBool"));

        xMax = GameManager._instance.GetComponent<GameManager>().xMax;
        yMax = GameManager._instance.GetComponent<GameManager>().yMax;
        targetNo = GameManager._instance.GetComponent<GameManager>().playerTargetNo;
        startTime = Time.time;
    }


    // Update is called once per frame
    private void FixedUpdate()
    {
        Debug.Log("Player");
        elapsedTime = Time.time - startTime;
        Move();
        Score();
        ListData();
        //Debug.Log(Time.time);
    }
    #endregion

    #region My Functions
    public void Move()
    {
        float UinputX = Input.GetAxis("Horizontal");
        float UinputY = Input.GetAxis("Vertical");
        xAcc = UinputX * PlayerGain;
        yAcc = UinputY * PlayerGain;
        //xPos += 0.5f * (xAcc - cd * xVel) * Mathf.Pow(Time.deltaTime, 2f) + xVel * Time.deltaTime;
        //yPos += 0.5f * (yAcc - cd * yVel) * Mathf.Pow(Time.deltaTime, 2f) + yVel * Time.deltaTime;
        float tau = Mathf.Exp(-cd * Time.deltaTime);
        xPos += (1 - tau) * xVel /cd + (Time.deltaTime / cd - (1 - tau) / Mathf.Pow(cd, 2)) * xAcc;
        yPos += (1 - tau) * yVel /cd + (Time.deltaTime / cd - (1 - tau) / Mathf.Pow(cd, 2)) * yAcc;

        //xVel += (xAcc - cd * xVel) * Time.deltaTime;
        //yVel += (yAcc - cd * yVel) * Time.deltaTime;
        xVel = tau * xVel + (1 - tau) * xAcc / cd;
        yVel = tau * yVel + (1 - tau) * yAcc / cd;

        transform.position = new Vector3(xPos, yPos, transform.position.z);
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -xMax, xMax), Mathf.Clamp(transform.position.y, -yMax, yMax), transform.position.z);

        if (bouncingFromTheWall)
        {
            if (transform.position.x >= xMax || transform.position.x <= -xMax){ xVel = -xVel; }
            if (transform.position.y >= yMax || transform.position.y <= -yMax) { yVel = -yVel; }
        }
        else
        {
            if ((transform.position.x >= xMax || transform.position.x <= -xMax) && (!isLeavingX)) { enterContactX = true; }
            if ((transform.position.y >= yMax || transform.position.y <= -yMax) && (!isLeavingY)) { enterContactY = true; }


            if (enterContactX)
            {
                if (transform.position.x >= xMax)
                {
                    xVel = 0f;
                    if (xAcc < 0) { enterContactX = false; isLeavingX = true; }
                }
                else if (transform.position.x <= -xMax)
                {
                    xVel = 0f;
                    if (xAcc > 0) { enterContactX = false; isLeavingX = true; }
                }
            }

            if (enterContactY)
            {
                if (transform.position.y >= yMax)
                {
                    yVel = 0f;
                    if (yAcc < 0) { enterContactY = false; isLeavingY = true; }
                }
                else if (transform.position.y <= -yMax)
                {
                    yVel = 0f;
                    if (yAcc > 0) { enterContactY = false; isLeavingY = true; }
                }
            }


            if (!(transform.position.x >= xMax || transform.position.x <= -xMax)) { isLeavingX = false; }
            if (!(transform.position.y >= yMax || transform.position.y <= -yMax)) { isLeavingY = false; }
        }
        

        xVelPre = xVel;
        yVelPre = yVel;


    }

    public void Score()
    {
        GameManager._instance.timeScore = 10000f - 50f * Mathf.Pow(elapsedTime, 2);
        GameManager._instance.collisionPenalty = -colNo * 500f;
        GameManager._instance.score = GameManager._instance.timeScore + GameManager._instance.collisionPenalty;
        GameManager._instance.colNo = colNo;
    }
    #endregion

    #region SaveData
    public void ListData()
    {
        save.time.Add(elapsedTime);
        save.xPos.Add(transform.position.x);
        save.yPos.Add(transform.position.y);
        save.xVel.Add(xVel);
        save.yVel.Add(yVel);
        save.xAcc.Add(xAcc);
        save.yAcc.Add(yAcc);
    }

    #endregion

    #region Collision and End Condition
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Target" + targetNo.ToString())
        {

            save.completeTime = elapsedTime;
            loadingCanvas.SetActive(true);

            this.gameObject.SetActive(false);
            Time.timeScale = 0;
            GameDataSaver.instance.Save();


            if (GameManager._instance.currentPlayer.gameMode == 0 || GameManager._instance.currentPlayer.gameMode == 2)
            {
                GameManager._instance.TrialEndCondition();
            } else if (GameManager._instance.currentPlayer.gameMode == 1){
               GameManager._instance.HighScoreCheck();
                GameManager._instance.DayEndCondition();
                SceneManager.LoadScene("MainMenu");
            }

            // Add agent saving condition

        }
        if (other.gameObject.tag == "Agent")
        {
            colNo++;
            save.colNo = colNo;
            save.colTime.Add(elapsedTime);
            save.agentNoInCol.Add(other.gameObject.GetComponent<AgentController>().agentNo);       

            float xVelOther = other.gameObject.GetComponent<AgentController>().xVelPre;
            float yVelOther = other.gameObject.GetComponent<AgentController>().yVelPre;
            Vector3 otherPos = other.gameObject.GetComponent<Transform>().position;


            Vector2 newVel = CollisionSolver.instance.VelocityElement(transform.position, otherPos, xVel, yVel, xVelOther, yVelOther);

            xVel = newVel.x;
            yVel = newVel.y;            
        }

    }
    #endregion

    public static bool ToBool(string aText)
    {
        return aText == "true";
    }
}
