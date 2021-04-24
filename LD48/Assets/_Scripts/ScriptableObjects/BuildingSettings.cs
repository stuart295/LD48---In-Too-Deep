using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BuildingSettings", menuName = "Buildings/Building definition")]
public class BuildingSettings : ScriptableObject
{
    public string buildingName;
    public GameObject prefab;
    //TODO icon
}
