using UnityEngine;
using System.Collections;
using System.Collections.Generic;  //lets us use lists



public class AgentController : MonoBehaviour
{
    #region Attributes

    float xMax, yMax;

    [HideInInspector] public int targetNo;
    [HideInInspector] public Vector3 target;
    [HideInInspector] public int agentNo;
    int agentAgentColNo;
    int agentSubjectColNo;

    [HideInInspector] public SaveDataAgent save = new SaveDataAgent();

    float xPos, yPos, xVel, yVel, xAcc, yAcc;
    [HideInInspector] public float xVelPre, yVelPre;

    //[SerializeField] public bool destinationSeeking;



    float elapsedTime = 0f;
    float startTime;

    Vector3 uDesSeek, uDamp, uRepulsion, uRepulsionWall;

    GameObject Player;

    // Flocking Variables
    float playerGain;

    bool destinationSeeking, repulsion, damping, wallRepulsion, bouncingFromTheWall;
    float agentGain, cd, r1, r2, a, b, omega_n, gamma, epsilon, d, rc, h, zeta, p_f, ra, rb, ra_wall, rb_wall, c1, c2;
    float destinationSeekingGain, dampingGain, repulsionGain, wallRepulsionGain;

    float radius;
    bool simplified = true;

    #endregion

    #region MonoBehaviour API


    private void Awake()
    {

    }
    // Start is called before the first frame update
    void Start()
    {

        playerGain = PlayerPrefs.GetFloat("playerGain");

        agentGain = PlayerPrefs.GetFloat("agentGain");
        cd = PlayerPrefs.GetFloat("cdAgent");

        destinationSeeking = ToBool(PlayerPrefs.GetString("destinationSeekingBool"));
        repulsion = ToBool(PlayerPrefs.GetString("repulsionBool"));
        damping = ToBool(PlayerPrefs.GetString("dampingBool"));
        wallRepulsion = ToBool(PlayerPrefs.GetString("wallRepulsionBool"));

        bouncingFromTheWall = ToBool(PlayerPrefs.GetString("bouncingFromTheWallBool"));

        r1 = PlayerPrefs.GetFloat("r1");        
        r2 = PlayerPrefs.GetFloat("r2");
        a = PlayerPrefs.GetFloat("a");
        b = PlayerPrefs.GetFloat("b");
        omega_n = PlayerPrefs.GetFloat("omega_n");
        gamma = PlayerPrefs.GetFloat("gamma");
        epsilon = PlayerPrefs.GetFloat("epsilon");
        d = PlayerPrefs.GetFloat("d");
        rc = PlayerPrefs.GetFloat("rc");
        h = PlayerPrefs.GetFloat("h");
        zeta = PlayerPrefs.GetFloat("zeta");
        p_f = PlayerPrefs.GetFloat("p_f");
        ra = PlayerPrefs.GetFloat("ra");
        rb = PlayerPrefs.GetFloat("rb");
        ra_wall = PlayerPrefs.GetFloat("ra_wall");
        rb_wall = PlayerPrefs.GetFloat("rb_wall");
        c1 = PlayerPrefs.GetFloat("c1");
        c2 = PlayerPrefs.GetFloat("c2");


        destinationSeekingGain = PlayerPrefs.GetFloat("destinationSeekingGain");
        dampingGain = PlayerPrefs.GetFloat("dampingGain");
        repulsionGain = PlayerPrefs.GetFloat("repulsionGain");
        wallRepulsionGain = PlayerPrefs.GetFloat("wallRepulsionGain");


        xMax = GameManager._instance.GetComponent<GameManager>().xMax;
        yMax = GameManager._instance.GetComponent<GameManager>().yMax;

        save.agentNo = agentNo;
        save.targetNo = targetNo;
        xPos = transform.position.x;
        yPos = transform.position.y;
        radius = gameObject.GetComponent<CircleCollider2D>().radius;
        Player = GameObject.Find("Player");
        startTime = Time.time;

    }


    // Update is called once per frame
    void FixedUpdate()
    {
        Debug.Log(agentNo);
        elapsedTime = Time.time - startTime;

        Move();
        ListData();

    }

    #endregion

    #region Flocking Functions

    float Nu(float disToTarget)
    {
        float nu;

        if (disToTarget <= r1)
        {
            nu = Mathf.Pow(omega_n, 2);
        }
        else if (disToTarget > r1 && disToTarget < r2)
        {
            nu = 0.5f * ((Mathf.Pow(omega_n, 2) + gamma / disToTarget) - (Mathf.Pow(omega_n, 2) - gamma / disToTarget) * Mathf.Cos(Mathf.PI * (Mathf.Pow(r2, 2) - Mathf.Pow(disToTarget, 2)) / (Mathf.Pow(r2, 2) - Mathf.Pow(r1, 2))));
        }
        else
        {
            nu = gamma / disToTarget;
        }
        return nu;
    }

    float Kappa(float disToTarget)
    {
        float kappa;
        int dampingSwitch = 2;

        if (dampingSwitch == 1)
        {
            if (disToTarget <= r1)
            {
                kappa = -2 * zeta * omega_n;
            }
            else if (disToTarget > r1 && disToTarget < r2)
            {
                kappa = -0.5f * ((2 * zeta * omega_n + gamma / p_f) - (2 * zeta * omega_n - gamma / p_f) * Mathf.Cos(Mathf.PI * (Mathf.Pow(r2, 2) - Mathf.Pow(disToTarget, 2)) / (Mathf.Pow(r2, 2) - Mathf.Pow(r1, 2))));
            }
            else
            {
                kappa = -gamma / p_f;
            }
        }else
        {
            kappa = -cd;
        }
        
        return kappa;
    }

    float Mu(float dis)
    {
        float mu;

        if (dis <= ra)
        {
            mu = 0f;
        }
        else if (dis > ra && dis < rb)
        {
            mu = 0.5f + 0.5f * (Mathf.Cos(Mathf.PI * (Mathf.Pow(rb, 2) - Mathf.Pow(dis, 2)) / (Mathf.Pow(rb, 2) - Mathf.Pow(ra, 2))));

        }
        else
        {
            mu = 1f;
        }
        return mu;
    }

    float Mu_Wall(float dis)
    {
        float mu;

        if (dis <= ra_wall)
        {
            mu = 1f;
        }
        else if (dis > ra_wall && dis < rb_wall)
        {
            mu = 0.5f + 0.5f * (Mathf.Cos(Mathf.PI * (Mathf.Pow(ra_wall, 2) - Mathf.Pow(dis, 2)) / (Mathf.Pow(ra_wall, 2) - Mathf.Pow(rb_wall, 2))));

        }
        else
        {
            mu = 0f;
        }
        return mu;
    }


    float Rho(float eta)
    {
        float rho;

        if (eta >= 0 && eta < h)
        {
            rho = 1f;
        }
        else if (eta >= h && eta <= 1)
        {
            rho = 0.5f + 0.5f * Mathf.Cos(Mathf.PI * ((eta - h) / (1 - h)));
        }
        else
        {
            rho = 0f;
        }

        return rho;
    }

    float EpsilonNorm(Vector3 x)
    {
        float norm_e;
        norm_e = (Mathf.Sqrt(1 + epsilon * x.sqrMagnitude) - 1) / epsilon;
        return norm_e;
    }

    float EpsilonNormScalar(float x)
    {
        float norm_e;
        norm_e = (Mathf.Sqrt(1 + epsilon * Mathf.Pow(x, 2)) - 1) / epsilon;
        return norm_e;
    }


    Vector3 Sigma_Epsilon(Vector3 x)
    {
        Vector3 sigma_epsilon;
        sigma_epsilon = x / (1 + epsilon * EpsilonNorm(x));
        return sigma_epsilon;
    }

    float Phi(float eta)
    {
        float phi;
        if (simplified)
        {
            phi = b * eta / Mathf.Sqrt(1 + Mathf.Pow(eta, 2));
        }
        else
        {
            float c = (b - a) / (Mathf.Sqrt(4 * a * b));
            phi = 0.5f * (((a + b) * (eta + c) / Mathf.Sqrt(1 + Mathf.Pow(eta + c, 2))) + (a - b));
        }

        return phi;
    }

    Vector3 PHI(Vector3 x)
    {
        Vector3 phi;
        if (simplified)
        {
            phi = Phi(x.magnitude - d)*x/x.magnitude;
        }
        else
        {
            phi = Phi(EpsilonNorm(x) - EpsilonNormScalar(d)) * Sigma_Epsilon(x);
        }
        return phi;
    }
    #endregion

    #region Destination Seeking    
    Vector3 U_DesSeek()
    {
        Vector3 uDes;

        if (destinationSeeking)
        {         
            if (simplified)
            {
                Vector3 toTarget = target - transform.position;
                float disToTarget = toTarget.magnitude;
                uDes = gamma * toTarget/disToTarget;
            }
            else
            {
                Vector3 toTarget = target - transform.position;
                float disToTarget = toTarget.magnitude;
                uDes = Nu(disToTarget) * toTarget;
            }
        }
        else
        {
            uDes = Vector3.zero;
        }
        
        return uDes;
    }


    #endregion

    #region Repulsion

    Vector3 U_Repulsion()
    {
        Vector3 uRep;
        uRep = Vector3.zero;
        if (repulsion)
        {
            if (simplified)
            {

                //float disToTarget = Vector3.Distance(target, transform.position);
                foreach (GameObject go in AgentSpawner._agentInstance)
                {
                    if (go != this.gameObject && go.activeSelf)
                    {
                        if (Vector3.Distance(go.transform.position, transform.position) < d)
                        {                            
                            Vector3 toAgent = go.transform.position - transform.position;                           
                            uRep += PHI(toAgent);
                        }
                    }

                }

                if (Vector3.Distance(Player.transform.position, transform.position) < d)
                {
                    Vector3 toPlayer = Player.transform.position - transform.position;
                    uRep += PHI(toPlayer);
                }

            }
            else
            {
                float disToTarget = Vector3.Distance(target, transform.position);
                foreach (GameObject go in AgentSpawner._agentInstance)
                {
                    if (go != this.gameObject && go.activeSelf)
                    {
                        if (Vector3.Distance(go.transform.position, transform.position) < d)
                        {
                            //Debug.Log("here");
                            Vector3 toAgent = go.transform.position - transform.position;
                            //float dist = toAgent.magnitude;
                            uRep += Rho(EpsilonNorm(toAgent) / EpsilonNormScalar(rc)) * PHI(toAgent);
                        }
                    }

                }

                if (Vector3.Distance(Player.transform.position, transform.position) < d)
                {
                    Vector3 toPlayer = Player.transform.position - transform.position;
                    uRep += Rho(EpsilonNorm(toPlayer) / EpsilonNormScalar(rc)) * PHI(toPlayer);
                }

                uRep = uRep * Mu(disToTarget);

            }
            
        }
        else
        {
            uRep = Vector3.zero;
        }
        return uRep;
    }
    #endregion


    #region Wall Repulsion

    Vector3 U_WallRepulsion()
    {
        Vector3 uRep;
        uRep = Vector3.zero;
        if (wallRepulsion)
        {            
            uRep.x = Mu_Wall(Mathf.Abs(transform.position.x - (-(xMax+ radius)))) * c1 / (Mathf.Abs(transform.position.x - (-(xMax + radius))) + c2);
            uRep.x += -Mu_Wall(Mathf.Abs(transform.position.x - (xMax + radius))) * c1 / (Mathf.Abs(transform.position.x - (xMax + radius)) + c2);
            uRep.y = Mu_Wall(Mathf.Abs(transform.position.y - (-(yMax + radius)))) * c1 / (Mathf.Abs(transform.position.y - (-(yMax + radius))) + c2);
            uRep.y += -Mu_Wall(Mathf.Abs(transform.position.y - (yMax + radius))) * c1 / (Mathf.Abs(transform.position.y - (yMax + radius)) + c2);
        }
        else
        {
            uRep = Vector3.zero;
        }
        return uRep;
    }

        #endregion

        #region Damping
        Vector3 U_Damping()
    {
        Vector3 uDam;
        if (damping)
        {
            float disToTarget = Vector3.Distance(target,transform.position);
            uDam = Kappa(disToTarget)*(new Vector3(xVel,yVel,0f));
        }
        else
        {
            uDam = Vector3.zero;
        }
        return uDam;
    }

    #endregion



    #region Flocking
    public void Move()
    {
        uDesSeek = U_DesSeek();
        uDesSeek.x = Mathf.Clamp(uDesSeek.x, -1f, 1f);
        uDesSeek.y = Mathf.Clamp(uDesSeek.y, -1f, 1f);
        //uDamp = U_Damping();
        uRepulsion = U_Repulsion();
        uRepulsionWall = U_WallRepulsion();

        Vector3 U = destinationSeekingGain * uDesSeek  + repulsionGain * uRepulsion + wallRepulsionGain * uRepulsionWall;

        xAcc = Mathf.Clamp(U.x * agentGain, -playerGain, playerGain);
        yAcc = Mathf.Clamp(U.y * agentGain, -playerGain, playerGain);

        float tau = Mathf.Exp(-cd * Time.deltaTime);
        xPos += (1 - tau) * xVel / cd + (Time.deltaTime / cd - (1 - tau) / Mathf.Pow(cd, 2)) * xAcc;
        yPos += (1 - tau) * yVel / cd + (Time.deltaTime / cd - (1 - tau) / Mathf.Pow(cd, 2)) * yAcc;

        //xVel += (xAcc - cd * xVel) * Time.deltaTime;
        //yVel += (yAcc - cd * yVel) * Time.deltaTime;
        xVel = tau * xVel + (1 - tau) * xAcc / cd;
        yVel = tau * yVel + (1 - tau) * yAcc / cd;

        //xPos += 0.5f * (xAcc + dampingGain * uDamp.x) * Mathf.Pow(Time.deltaTime, 2f) + xVel * Time.deltaTime;
        //yPos += 0.5f * (yAcc + dampingGain * uDamp.y) * Mathf.Pow(Time.deltaTime, 2f) + yVel * Time.deltaTime;
        //x/Vel += (xAcc - cd * xVel) * Time.deltaTime;
        //yVel += (yAcc - cd * yVel) * Time.deltaTime;

        transform.position = new Vector3(xPos, yPos, transform.position.z);
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -xMax, xMax), Mathf.Clamp(transform.position.y, -yMax, yMax), transform.position.z);
        if (bouncingFromTheWall)
        {
            if (transform.position.x >= xMax || transform.position.x <= -xMax) { xVel = -xVel; }
            if (transform.position.y >= yMax || transform.position.y <= -yMax) { yVel = -yVel; }
        }

        xVelPre = xVel;
        yVelPre = yVel;
    }
    
    #endregion

    #region Save Data
    public void ListData()
    {
        save.time.Add(elapsedTime);
        save.xPos.Add(transform.position.x);
        save.yPos.Add(transform.position.y);
        save.xVel.Add(xVel);
        save.yVel.Add(yVel);
        save.xAcc.Add(xAcc);
        save.yAcc.Add(yAcc);
        save.uDesSeekX.Add(uDesSeek.x);
        save.uDesSeekY.Add(uDesSeek.y);
        save.uDampX.Add(uDamp.x);
        save.uDampY.Add(uDamp.y);
        save.uRepX.Add(uRepulsion.x);
        save.uRepY.Add(uRepulsion.y);
        save.uWallRepX.Add(uRepulsionWall.x);
        save.uWallRepY.Add(uRepulsionWall.y);
    }
    #endregion

    #region Collision and End Condition
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Target" + targetNo.ToString())
        {
            save.completed = true;
            save.completeTime = elapsedTime;
            this.gameObject.SetActive(false);

        }

        if (other.gameObject.tag == "Player")
        {
            agentSubjectColNo++;
            save.agentSubjectColNo = agentSubjectColNo;
            save.agentSubjectColTime.Add(elapsedTime);


            float xVelOther = other.gameObject.GetComponent<PlayerController>().xVelPre;
            float yVelOther = other.gameObject.GetComponent<PlayerController>().yVelPre;
            Vector3 otherPos = other.gameObject.GetComponent<Transform>().position;

            Vector2 newVel = CollisionSolver.instance.VelocityElement(transform.position, otherPos, xVel, yVel, xVelOther, yVelOther);
            xVel = newVel.x;
            yVel = newVel.y;

        }

        if (other.gameObject.tag == "Agent")
        {
            agentAgentColNo++;
            save.agentAgentColNo = agentAgentColNo;
            save.agentAgentColTime.Add(elapsedTime);
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
