using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveDataSubject
{
    public List<float> time = new List<float>();
    public List<float> xPos = new List<float>();
    public List<float> yPos = new List<float>();
    public List<float> xVel = new List<float>();
    public List<float> yVel = new List<float>();
    public List<float> xAcc = new List<float>();
    public List<float> yAcc = new List<float>();
    
    public int colNo;
    public List<int> agentNoInCol = new List<int>();
    public List<float> colTime = new List<float>();    

    public float completeTime;
}

