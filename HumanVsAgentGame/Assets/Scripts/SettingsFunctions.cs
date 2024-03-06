using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsFunctions : MonoBehaviour
{

    float destinationSeekingGain;
    float dampingGain;
    float repulsionGain;
    float wallRepulsionGain;

    float playerGain;
    float agentGain;
    float cdPlayer;
    float cdAgent;

    [SerializeField] GameObject nextButton;
    [SerializeField] GameObject previousButton;
    [SerializeField] GameObject agentSettingsPanel1;
    [SerializeField] GameObject agentSettingsPanel2;

    [SerializeField] GameObject generalSettingsPanel;
    [SerializeField] GameObject agentSettingsPanels;

    public SettingsDataClass settings = new SettingsDataClass();

    // PlaceHolders:

    [SerializeField] public Text directoryInputText;

    [SerializeField] public Text playerGainInputText, cdPlayerInputText, agentGainInputText, cdAgentInputText;
    [SerializeField] public Text destinationSeekingGainInputText, dampingGainInputText, repulsionGainInputText, wallRepulsionInputText;

    [SerializeField] public Text r1InputText, r2InputText, aInputText, bInputText, omega_nInputText, gammaInputText;
    [SerializeField] public Text epsilonInputText, dInputText, rcInputText, hInputText, zetaInputText, p_fInputText;
    [SerializeField] public Text raInputText, rbInputText, raWallInputText, rbWallInputText, c1InputText, c2InputText;
    //[SerializeField] public Text noOfAgentsInSetsInputText;
    [SerializeField] public Text minSpawningRadiusInputText, maxSpawningRadiusInputText;


    // Input Field
    [SerializeField] public Text directoryInput;

    [SerializeField] public Text playerGainInput, cdPlayerInput, agentGainInput, cdAgentInput;
    [SerializeField] public Text destinationSeekingGainInput, dampingGainInput, repulsionGainInput, wallRepulsionInput;

    [SerializeField] public Text r1Input, r2Input, aInput, bInput, omega_nInput, gammaInput;
    [SerializeField] public Text epsilonInput, dInput, rcInput, hInput, zetaInput, p_fInput;
    [SerializeField] public Text raInput, rbInput, raWallInput, rbWallInput, c1Input, c2Input;
    //[SerializeField] public Text noOfAgentsInSetsInput;
    [SerializeField] public Text minSpawningRadiusInput, maxSpawningRadiusInput;



    [SerializeField] public Toggle bouncingToggle;
    [SerializeField] public Toggle destinationSeekingToggle;
    [SerializeField] public Toggle dampingToggle;
    [SerializeField] public Toggle repulsionToggle;
    [SerializeField] public Toggle wallRepulsionToggle;

    void Start()
    {
        LoadSettings();
    }

    public void FactoryReset()
    {
        float targetRadius = 0.45f;
        float playerRadius = 0.09f;
        agentGain = 1;
        playerGain = 0.8f;

        destinationSeekingGain = playerGain/agentGain;
        dampingGain = playerGain;
        repulsionGain = playerGain * 5;
        wallRepulsionGain = playerGain*5;

        cdPlayer = 0.2f;
        cdAgent = 0.2f;
        float h = 0.5f;

        float d = 1f;

        float rc = d/h + 0.001f;

        
        float gamma = Mathf.Sqrt(2f);

        float b = Mathf.Sqrt(1+Mathf.Pow(d,2))/d;

        float c1 = 1;
        float c2 = c1 - playerRadius;
        
        PlayerPrefs.SetString("directory", "C:/Users/Pedram/Desktop/UnityTestData/");

        PlayerPrefs.SetFloat("playerGain", playerGain);
        PlayerPrefs.SetFloat("agentGain", agentGain);
        PlayerPrefs.SetFloat("cdPlayer", cdPlayer);
        PlayerPrefs.SetFloat("cdAgent", cdAgent);

        PlayerPrefs.SetString("bouncingFromTheWallBool", "true");
        PlayerPrefs.SetFloat("r1", targetRadius/2);
        PlayerPrefs.SetFloat("r2", targetRadius);        
        PlayerPrefs.SetFloat("a", b);               
        PlayerPrefs.SetFloat("b", b);                      
        PlayerPrefs.SetFloat("omega_n", 1f);
        PlayerPrefs.SetFloat("gamma", gamma);                  
        PlayerPrefs.SetFloat("epsilon", 0.001f);
        PlayerPrefs.SetFloat("d", d);
        PlayerPrefs.SetFloat("rc", rc);
        PlayerPrefs.SetFloat("h", h);
        PlayerPrefs.SetFloat("zeta", 0.5f);
        PlayerPrefs.SetFloat("p_f", 10f);
        PlayerPrefs.SetFloat("ra", targetRadius/2);
        PlayerPrefs.SetFloat("rb", targetRadius);
        PlayerPrefs.SetFloat("ra_wall",0.3f);
        PlayerPrefs.SetFloat("rb_wall",0.5f);
        PlayerPrefs.SetFloat("c1",c1);
        PlayerPrefs.SetFloat("c2", c2);

        PlayerPrefs.SetString("destinationSeekingBool", "true");
        PlayerPrefs.SetString("dampingBool", "true");
        PlayerPrefs.SetString("repulsionBool", "true");
        PlayerPrefs.SetString("wallRepulsionBool", "true");

        PlayerPrefs.SetFloat("destinationSeekingGain", destinationSeekingGain);
        PlayerPrefs.SetFloat("dampingGain", dampingGain);
        PlayerPrefs.SetFloat("repulsionGain", repulsionGain);
        PlayerPrefs.SetFloat("wallRepulsionGain", wallRepulsionGain);

       // PlayerPrefs.SetInt("noOfAgentInSets", 20);


        PlayerPrefs.SetFloat("minSpawningRadius", 0.8f);
        PlayerPrefs.SetFloat("maxSpawningRadius", 1.2f);



        LoadSettings();

}

    public void BackToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void LoadSettings()
    {
        directoryInputText.text = PlayerPrefs.GetString("directory");

        playerGainInputText.text = PlayerPrefs.GetFloat("playerGain").ToString();
        agentGainInputText.text = PlayerPrefs.GetFloat("agentGain").ToString();
        cdPlayerInputText.text = PlayerPrefs.GetFloat("cdPlayer").ToString();
        cdAgentInputText.text = PlayerPrefs.GetFloat("cdAgent").ToString();

        bouncingToggle.isOn = ToBool(PlayerPrefs.GetString("bouncingFromTheWallBool"));

        r1InputText.text = PlayerPrefs.GetFloat("r1").ToString();
        r2InputText.text = PlayerPrefs.GetFloat("r2").ToString();
        aInputText.text = PlayerPrefs.GetFloat("a").ToString();
        bInputText.text = PlayerPrefs.GetFloat("b").ToString();
        omega_nInputText.text = PlayerPrefs.GetFloat("omega_n").ToString();
        gammaInputText.text = PlayerPrefs.GetFloat("gamma").ToString();
        epsilonInputText.text = PlayerPrefs.GetFloat("epsilon").ToString();
        dInputText.text = PlayerPrefs.GetFloat("d").ToString();
        rcInputText.text = PlayerPrefs.GetFloat("rc").ToString();
        hInputText.text = PlayerPrefs.GetFloat("h").ToString();
        zetaInputText.text = PlayerPrefs.GetFloat("zeta").ToString();
        p_fInputText.text = PlayerPrefs.GetFloat("p_f").ToString();
        raInputText.text = PlayerPrefs.GetFloat("ra").ToString();
        rbInputText.text = PlayerPrefs.GetFloat("rb").ToString();
        raWallInputText.text = PlayerPrefs.GetFloat("ra_wall").ToString();
        rbWallInputText.text = PlayerPrefs.GetFloat("rb_wall").ToString();
        c1InputText.text = PlayerPrefs.GetFloat("c1").ToString();
        c2InputText.text = PlayerPrefs.GetFloat("c2").ToString();

        //PlayerPrefs.GetString("destinationSeekingBool");
        //PlayerPrefs.GetString("dampingBool");
        //PlayerPrefs.GetString("repulsionBool");
        //PlayerPrefs.GetString("wallRepulsionBool");

        destinationSeekingToggle.isOn = ToBool(PlayerPrefs.GetString("destinationSeekingBool"));
        dampingToggle.isOn = ToBool(PlayerPrefs.GetString("dampingBool"));
        repulsionToggle.isOn = ToBool(PlayerPrefs.GetString("repulsionBool"));
        wallRepulsionToggle.isOn = ToBool(PlayerPrefs.GetString("wallRepulsionBool"));

        destinationSeekingGainInputText.text = PlayerPrefs.GetFloat("destinationSeekingGain").ToString();
        dampingGainInputText.text = PlayerPrefs.GetFloat("dampingGain").ToString();
        repulsionGainInputText.text = PlayerPrefs.GetFloat("repulsionGain").ToString();
        wallRepulsionInputText.text = PlayerPrefs.GetFloat("wallRepulsionGain").ToString();

        minSpawningRadiusInputText.text = PlayerPrefs.GetFloat("minSpawningRadius").ToString();
        maxSpawningRadiusInputText.text =  PlayerPrefs.GetFloat("maxSpawningRadius").ToString();
        //noOfAgentsInSetsInputText.text =  PlayerPrefs.GetInt("noOfAgentInSets").ToString();
    }


    public void NextButton()
    {
        nextButton.SetActive(false);
        previousButton.SetActive(true);
        agentSettingsPanel1.SetActive(false);
        agentSettingsPanel2.SetActive(true);
    }

    public void PreviousButton()
    {
        previousButton.SetActive(false);
        nextButton.SetActive(true);
        agentSettingsPanel2.SetActive(false);
        agentSettingsPanel1.SetActive(true);
    }

    public void OpenGeneralSettings()
    {
        agentSettingsPanels.SetActive(false);
        generalSettingsPanel.SetActive(true);
    }

    public void OpenAgentsSettings()
    {
        generalSettingsPanel.SetActive(false);
        agentSettingsPanels.SetActive(true);
    }

    public void BouncingOnOff(bool changed)
    {        
        if (changed){
            if (bouncingToggle.isOn == true)
            {
                PlayerPrefs.SetString("bouncingFromTheWallBool", "true");
            }
            else if (bouncingToggle.isOn == false)
            {
                PlayerPrefs.SetString("bouncingFromTheWallBool", "false");

            }
        }
        
    }

    public void DestinationSeekingOnOff(bool changed)
    {
        if (changed)
        {
            if (destinationSeekingToggle.isOn == true)
            {
                PlayerPrefs.SetString("destinationSeekingBool", "true");
            }
            else if (destinationSeekingToggle.isOn == false)
            {
                PlayerPrefs.SetString("destinationSeekingBool", "false");

            }
        }

    }

    public void DampingOnOff(bool changed)
    {
        if (changed)
        {
            if (dampingToggle.isOn == true)
            {
                PlayerPrefs.SetString("dampingBool", "true");
            }
            else if (dampingToggle.isOn == false)
            {
                PlayerPrefs.SetString("dampingBool", "false");

            }
        }

    }

    public void RepulsionOnOff(bool changed)
    {
        if (changed)
        {
            if (repulsionToggle.isOn == true)
            {
                PlayerPrefs.SetString("repulsionBool", "true");
            }
            else if (repulsionToggle.isOn == false)
            {
                PlayerPrefs.SetString("repulsionBool", "false");

            }
        }

    }

    public void WallRepulsionOnOff(bool changed)
    {
        if (changed)
        {
            if (wallRepulsionToggle.isOn == true)
            {
                PlayerPrefs.SetString("wallRepulsionBool", "true");
            }
            else if (wallRepulsionToggle.isOn == false)
            {
                PlayerPrefs.SetString("wallRepulsionBool", "false");

            }
        }

    }


    public void ApplyChanges()
    {

        if (!(directoryInput.text == ""))
        {
            PlayerPrefs.SetString("directory", directoryInput.text);
        }
        if (!(playerGainInput.text == ""))
        {
            PlayerPrefs.SetFloat("playerGain", float.Parse(playerGainInput.text));
        }
        if (!(cdPlayerInput.text == ""))
        {
            PlayerPrefs.SetFloat("cdPlayer", float.Parse(cdPlayerInput.text));
        }
        if (!(agentGainInput.text == ""))
        {
            PlayerPrefs.SetFloat("agentGain", float.Parse(agentGainInput.text));
        }
        if (!(cdAgentInput.text == ""))
        {
            PlayerPrefs.SetFloat("cdAgent", float.Parse(cdAgentInput.text));
        }
        if (!(destinationSeekingGainInput.text == ""))
        {
            PlayerPrefs.SetFloat("destinationSeekingGain", float.Parse(destinationSeekingGainInput.text));
        }
        if (!(dampingGainInput.text == ""))
        {
            PlayerPrefs.SetFloat("dampingGain", float.Parse(dampingGainInput.text));
        }
        if (!(repulsionGainInput.text == ""))
        {
            PlayerPrefs.SetFloat("repulsionGain", float.Parse(repulsionGainInput.text));
        }
        if (!(wallRepulsionInput.text == ""))
        {
            PlayerPrefs.SetFloat("wallRepulsionGain", float.Parse(wallRepulsionInput.text));
        }


        if (!(r1Input.text == ""))
        {
            PlayerPrefs.SetFloat("r1", float.Parse(r1Input.text));
        }

        if (!(r2Input.text == ""))
        {
            PlayerPrefs.SetFloat("r2", float.Parse(r2Input.text));
        }

        if (!(aInput.text == ""))
        {
            PlayerPrefs.SetFloat("a", float.Parse(aInput.text));
        }


        if (!(bInput.text == ""))
        {
            PlayerPrefs.SetFloat("b", float.Parse(bInput.text));
        }

        if (!(omega_nInput.text == ""))
        {
            PlayerPrefs.SetFloat("omega_n", float.Parse(omega_nInput.text));
        }

        if (!(gammaInput.text == ""))
        {
            PlayerPrefs.SetFloat("gamma", float.Parse(gammaInput.text));
        }

        if (!(epsilonInput.text == ""))
        {
            PlayerPrefs.SetFloat("epsilon", float.Parse(epsilonInput.text));
        }

        if (!(dInput.text == ""))
        {
            PlayerPrefs.SetFloat("d", float.Parse(dInput.text));
        }


        if (!(rcInput.text == ""))
        {
            PlayerPrefs.SetFloat("rc", float.Parse(rcInput.text));
        }

        if (!(hInput.text == ""))
        {
            PlayerPrefs.SetFloat("h", float.Parse(hInput.text));
        }

        if (!(zetaInput.text == ""))
        {
            PlayerPrefs.SetFloat("zeta", float.Parse(zetaInput.text));
        }

        if (!(p_fInput.text == ""))
        {
            PlayerPrefs.SetFloat("p_f", float.Parse(p_fInput.text));
        }

        if (!(raInput.text == ""))
        {
            PlayerPrefs.SetFloat("ra", float.Parse(raInput.text));
        }

        if (!(rbInput.text == ""))
        {
            PlayerPrefs.SetFloat("rb", float.Parse(rbInput.text));
        }

        if (!(raWallInput.text == ""))
        {
            PlayerPrefs.SetFloat("ra_wall", float.Parse(raWallInput.text));
        }

        if (!(rbWallInput.text == ""))
        {
            PlayerPrefs.SetFloat("rb_wall", float.Parse(rbWallInput.text));
        }

        if (!(c1Input.text == ""))
        {
            PlayerPrefs.SetFloat("c1", float.Parse(c1Input.text));
        }

        if (!(c2Input.text == ""))
        {
            PlayerPrefs.SetFloat("c2", float.Parse(c2Input.text));
        }

        if (!(minSpawningRadiusInput.text == ""))
        {
            PlayerPrefs.SetFloat("minSpawningRadius", float.Parse(minSpawningRadiusInput.text));
        }

        if (!(maxSpawningRadiusInput.text == ""))
        {
            PlayerPrefs.SetFloat("maxSpawningRadius", float.Parse(maxSpawningRadiusInput.text));
        }

    }




    public static bool ToBool(string aText)
    {
        return aText == "true";
    }


}
