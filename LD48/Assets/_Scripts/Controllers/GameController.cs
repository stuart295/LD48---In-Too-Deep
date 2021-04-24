using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    [Header("Game settings")]
    public int startingCredits = 100;

    [Header("Grid settings")]
    public Vector2 cellSize = Vector2.one;
    public Vector2 cellOffset = Vector2.zero;

    [Header("Buildings")]
    public BuildingSettings spacePortPref;
    public BuildingSettings resourceDepositPref;
    public BuildingSettings minerPref;
    public BuildingSettings pipePref;

    [Header("Resource layout")]
    public int totalResources = 10;
    public float resourceSpacingMin = 1f;
    public float resourceSpacingMax = 1f;
    public float resourceHorizontalVariation = 1f;

    [Header("Other")]
    public bool debugMode = true;

    //[HideInInspector]
    public HashSet<Miner> miners;

    private BuildGrid grid;

    private Building spacePort;
    private int credits = 0;
    private float lastTick = 0;

    public BuildGrid Grid { get => grid;}
    public int Credits { get => credits; set => credits = value; }

    private void Awake() {
        grid = new BuildGrid();
        miners = new HashSet<Miner>();
        lastTick = Time.time;
    }

    // Start is called before the first frame update
    void Start()
    {
        PlaceStartingBuildings();
    }

    private void PlaceStartingBuildings() {
        //Space port
        spacePort = SpawnBuilding(spacePortPref, Vector3.zero, spacePortPref.prefab.transform.rotation);

        //Resource deposits
        SpawnBuilding(resourceDepositPref, new Vector3(2, 0, 6) , resourceDepositPref.prefab.transform.rotation);
        SpawnBuilding(minerPref, new Vector3(2, 0, 6), minerPref.prefab.transform.rotation);
        SpawnResources();

        //Pipes to first resource
        for (int i = 0; i < 4; i++) {
            SpawnBuilding(pipePref, new Vector3(1, 0, 3 + i), pipePref.prefab.transform.rotation);
        }
        
    }

    private void SpawnResources() {

        Vector3 curPoint = Vector3.zero;

        for (int i = 0; i < totalResources; i++) {
            float horPos = UnityEngine.Random.Range(-resourceHorizontalVariation, resourceHorizontalVariation);
            float vertPos = UnityEngine.Random.Range(curPoint.z + resourceSpacingMin, curPoint.z + resourceSpacingMax);
            curPoint = new Vector3(horPos, 0, vertPos);
            SpawnBuilding(resourceDepositPref, curPoint, resourceDepositPref.prefab.transform.rotation);
        }

    }

    private Building SpawnBuilding(BuildingSettings buildingSettings, Vector3 position, Quaternion rotation) {
        GameObject buildingGo = Instantiate(buildingSettings.prefab, Grid.GetGridPos(position), rotation);
        Building building = buildingGo.GetComponent<Building>();
        building.Initialize(this, buildingSettings);
        building.FinishPlacing();
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

    public int GetScore() {
        int score = 0;
        foreach (Miner miner in miners) {
            if (miner.Connected) score += 1;
        }
        return score;
    }

    private void OnDrawGizmos() {
        #if UNITY_EDITOR
        if (debugMode && miners != null) {
       
            foreach (Miner miner in miners) {
                if (miner == null || miner.gameObject == null) return;

                if (miner.Connected) {
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
