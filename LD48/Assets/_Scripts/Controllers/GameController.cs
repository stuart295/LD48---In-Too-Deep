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

    private BuildGrid grid;

    private Building spacePort;

    public BuildGrid Grid { get => grid;}


    private void Awake() {
        grid = new BuildGrid(cellSize, cellOffset);
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
        SpawnBuilding(resourceDepositPref, new Vector3(9, 0, 3), resourceDepositPref.transform.rotation);
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
}
