using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    [Header("Grid settings")]
    public Vector2 cellSize = Vector2.one;
    public Vector2 cellOffset = Vector2.zero;

    [Header("Prefabs")]
    public GameObject spacePortPref;
    public GameObject resourceDepositPref;

    [Header("Other")]
    public bool debugMode = true;

    //[HideInInspector]
    public HashSet<Miner> miners;

    private BuildGrid grid;

    private Building spacePort;



    public BuildGrid Grid { get => grid;}


    private void Awake() {
        grid = new BuildGrid();
        miners = new HashSet<Miner>();
    }

    // Start is called before the first frame update
    void Start()
    {
        PlaceStartingBuildings();
    }

    private void PlaceStartingBuildings() {
        //Space port
        spacePort = SpawnBuilding(spacePortPref, Vector3.zero, spacePortPref.transform.rotation);

        //Resource deposits
        SpawnBuilding(resourceDepositPref, new Vector3(5, 0, 3), resourceDepositPref.transform.rotation);
        //TODO

    }

    private Building SpawnBuilding(GameObject prefab, Vector3 position, Quaternion rotation) {
        GameObject buildingGo = Instantiate(prefab, position, rotation);
        Building building = buildingGo.GetComponent<Building>();
        building.Initialize(this);
        grid.addBuilding(building);
        return building;
    }


    // Update is called once per frame
    void Update()
    {
        
    }


    //TODO Switch to check from spaceport outwards
    public void RecheckMiners() {
        foreach (Miner miner in miners) {
            miner.CheckSPConnection();
        }
    }

    private void OnDrawGizmos() {
        #if UNITY_EDITOR
        if (debugMode && miners != null) {
       
            foreach (Miner miner in miners) {
                if (miner.connected) {
                    Gizmos.color = Color.green;
                }
                else {
                    Gizmos.color = Color.red;
                }

                Gizmos.DrawSphere(miner.transform.position + new Vector3(0, 3f, 0), 0.5f);
            }
        }
        #endif
    }

}
