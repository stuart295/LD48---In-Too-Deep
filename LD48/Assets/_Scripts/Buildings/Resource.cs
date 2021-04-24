using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : Building
{

    [Header("Resource settings")]
    public int creditsPerTick = 0;
    public int scoreValue = 0;


    [HideInInspector]
    public Miner miner;

}
