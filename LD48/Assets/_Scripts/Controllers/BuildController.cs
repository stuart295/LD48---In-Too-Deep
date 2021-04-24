using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildController : MonoBehaviour
{

    public List<BuildingSettings> buildingOptions;

    private Building currentBuilding = null;
    private BuildingSettings currentBuildingSettings;
    private GameController gm;
    private EnemySpawner spawner;
   
    private void Awake() {
        gm = GetComponent<GameController>();
        spawner = GetComponent<EnemySpawner>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePlacement();
    }


    private void UpdatePlacement() {
        if (currentBuilding == null) return;

        Vector3 placePosition = gm.Grid.GetGridMousePos();
        currentBuilding.UpdatePlacement(placePosition);

    }

    public bool IsPlacing() {
        return currentBuilding != null;
    }

    public void StartPlacingBuilding(BuildingSettings buildingSettings) {
        //CancelPlacingBuilding();
        GameObject buildingGo = Instantiate(buildingSettings.prefab);
        currentBuilding = buildingGo.GetComponent<Building>();
        currentBuilding.Initialize(gm, buildingSettings);
        currentBuilding.StartPlacing();
        currentBuildingSettings = buildingSettings;
    }

    public void CancelPlacingBuilding() {
        if (!currentBuilding) return;
        currentBuilding.CancelPlacing();
        currentBuilding = null;
        currentBuildingSettings = null;
    }


    public void FinishPlacingBuilding() {
        if (!CanAfford(currentBuildingSettings) || !currentBuilding.CanPlace()) return;

        spawner.furtherestBuildZ = Mathf.Max(spawner.furtherestBuildZ, currentBuilding.transform.position.z);

        currentBuilding.FinishPlacing();
        gm.Credits -= currentBuildingSettings.cost;

        StartPlacingBuilding(currentBuildingSettings);
    }

    private bool CanAfford(BuildingSettings building) {
        return gm.Credits >= building.cost;
    }

}
