using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveDataAgent
{
    public int agentNo;
    public int targetNo;
    public List<float> time = new List<float>();
    public List<float> xPos = new List<float>();
    public List<float> yPos = new List<float>();
    public List<float> xVel = new List<float>();
    public List<float> yVel = new List<float>();
    public List<float> xAcc = new List<float>();
    public List<float> yAcc = new List<float>();

    public List<float> uDesSeekX = new List<float>();
    public List<float> uDesSeekY= new List<float>();
    public List<float> uDampX = new List<float>();
    public List<float> uDampY = new List<float>();
    public List<float> uRepX = new List<float>();
    public List<float> uRepY = new List<float>();
    public List<float> uWallRepX = new List<float>();
    public List<float> uWallRepY = new List<float>();


    public int agentAgentColNo;
    public int agentSubjectColNo;
    public List<int> agentNoInCol = new List<int>();
    public List<float> agentAgentColTime = new List<float>();
    public List<float> agentSubjectColTime = new List<float>();

    public bool completed = false;
    public float completeTime;

}

