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
        GameObject spacePortGo = Instantiate(spacePortPref, Vector3.zero, spacePortPref.transform.rotation);
        spacePort = spacePortGo.GetComponent<Building>();
        spacePort.Initialize(this);
        grid.addBuilding(spacePort);


    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
