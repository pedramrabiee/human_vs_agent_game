using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsDataClass
{

    // General Settings

    public string directory;
    public string bouncingFromTheWallBool;
    public float playerGain;
    public float cdPlayer;
    public float cdAgent;

    // Agents behavior

    public string destinationSeekingBool;
    public string dampingBool;
    public string repulsionBool;
    public string wallRepulsionBool;

    public float destinationSeekingGain;
    public float dampingGain;
    public float repulsionGain;
    public float wallRepulsionGain;

    public float r1;
    public float r2;
    public float rc;
    public float ra;
    public float rb;
    public float ra_wall;
    public float rb_wall;

    public float a;
    public float b;
    public float d;
    public float h;

    public float omega_n;
    public float gamma;
    public float epsilon;
    public float zeta;

    public float p_f;
    public float c1;
    public float c2;
}
