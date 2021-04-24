using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    [Header("Game settings")]
    public int startingCredits = 100;
    public float timeLimit = 600;

    [Header("Grid settings")]
    public Vector2 cellSize = Vector2.one;
    public Vector2 cellOffset = Vector2.zero;

    [Header("Buildings")]
    public BuildingSettings spacePortPref;
    public BuildingSettings creditResourceDepositPref;
    public BuildingSettings scoreResourceDepositPref;
    public BuildingSettings minerPref;
    public BuildingSettings pipePref;

    [Header("Score resource layout")]
    public int totalScoreResources = 20;
    public float scoreResourceSpacingMin = 1f;
    public float scoreResourceSpacingMax = 1f;

    [Header("Credit resource layout")]
    public int totalCreditResources = 5;
    public float creditResourceSpacingMin = 1f;
    public float creditResourceSpacingMax = 1f;

    public float resourceHorizontalVariation = 1f;

    [Header("Other")]
    public bool debugMode = true;

    //[HideInInspector]
    public HashSet<Resource> resources;


    private BuildGrid grid;
    private BuildController build;
    private EnemySpawner spawner;
    private UIController ui;
    private int credits = 0;
    private float remainingTime = 0f;
    private bool gameOver = false;

    public BuildGrid Grid { get => grid;}
    public int Credits { get => credits; set => credits = value; }
    public float RemainingTime { get => remainingTime; }
    public bool GameOver { get => gameOver;  }

    private void Awake() {
        grid = new BuildGrid();
        resources = new HashSet<Resource>();
        remainingTime = timeLimit;
        build = GetComponent<BuildController>();
        spawner = GetComponent<EnemySpawner>();
        ui = GetComponent<UIController>();
        credits = startingCredits;
    }

    // Start is called before the first frame update
    void Start()
    {
        PlaceStartingBuildings();
    }

    private void PlaceStartingBuildings() {
        //Space port
        SpawnBuilding(spacePortPref, Vector3.zero, spacePortPref.prefab.transform.rotation);

        //Starting credits
        SpawnBuilding(creditResourceDepositPref, new Vector3(2, 0, 6), creditResourceDepositPref.prefab.transform.rotation);
        SpawnBuilding(minerPref, new Vector3(2, 0, 6), minerPref.prefab.transform.rotation);

        //Resource deposits
        SpawnResources(scoreResourceDepositPref, totalScoreResources, scoreResourceSpacingMin, scoreResourceSpacingMax);
        SpawnResources(creditResourceDepositPref, totalCreditResources, creditResourceSpacingMin, creditResourceSpacingMax);

        //Pipes to first resource
        for (int i = 0; i < 4; i++) {
            SpawnBuilding(pipePref, new Vector3(1, 0, 3 + i), pipePref.prefab.transform.rotation);
        }
        
    }

    private void SpawnResources(BuildingSettings building, int count, float spacingMin, float spacingMax) {

        Vector3 curPoint = Vector3.zero;

        for (int i = 0; i < count; i++) {
            float horPos = UnityEngine.Random.Range(-resourceHorizontalVariation, resourceHorizontalVariation);
            float vertPos = UnityEngine.Random.Range(curPoint.z + spacingMin, curPoint.z + spacingMax);
            curPoint = new Vector3(horPos, 0, vertPos);
            SpawnBuilding(building, curPoint, building.prefab.transform.rotation);
        }

    }

    private void SpawnBuilding(BuildingSettings buildingSettings, Vector3 position, Quaternion rotation) {
        List<Building> buildingsAtPos = grid.GetBuildingsAtWorldPos(position);

        if (buildingsAtPos.Count > 0) {
            if (buildingSettings.prefab.GetComponent<Building>() is Miner) {
                if (buildingsAtPos.Count > 1 || !(buildingsAtPos[0] is Resource)) return;
            }
            else {
                return;
            }
        }

        

        GameObject buildingGo = Instantiate(buildingSettings.prefab, Grid.GetGridPos(position), rotation);
        Building building = buildingGo.GetComponent<Building>();
        building.Initialize(this, buildingSettings);
        building.FinishPlacing();
    }


    // Update is called once per frame
    void Update()
    {
        if (gameOver) return;

        UpdateCountdown();
    }

    private void UpdateCountdown() {
        remainingTime -= Time.deltaTime;
        if (remainingTime <= 0) OnGameOver();
    }

    public void OnGameOver() {
        gameOver = true;
        build.enabled = false;
        spawner.enabled = false;
        ui.ShowGameOver();
    }

    //TODO Switch to check from spaceport outwards
    public void RecheckMiners() {
        foreach (Resource resource in resources) {
            if (resource.miner == null) return;
            resource.miner.CheckSPConnection();
        }
    }

    public int GetScore() {
        int score = 0;
        foreach (Resource resource in resources) {
            Miner miner = resource.miner;
            if (miner != null && miner.Connected) score += resource.scoreValue ;
        }
        return score;
    }

    private void OnDrawGizmos() {
        #if UNITY_EDITOR
        if (debugMode && resources != null) {
       
            foreach (Resource resource in resources) {
                if (resource == null || resource.miner == null || resource.gameObject == null) return;

                if (resource.miner.Connected) {
                    Gizmos.color = Color.green;
                }
                else {
                    Gizmos.color = Color.red;
                }

                Gizmos.DrawSphere(resource.transform.position + new Vector3(0, 3f, 0), 0.5f);
            }
        }
        #endif
    }


}
