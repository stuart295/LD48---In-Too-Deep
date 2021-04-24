using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildController : MonoBehaviour
{

    public List<BuildingSettings> buildingOptions;

    private Building currentBuilding = null;
    private GameController gm;
   
    private void Awake() {
        gm = GetComponent<GameController>();
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
        GameObject buildingGo = Instantiate(buildingSettings.prefab);
        currentBuilding = buildingGo.GetComponent<Building>();
        currentBuilding.Initialize(gm);
        currentBuilding.StartPlacing();
    }

    public void CancelPlacingBuilding() {
        currentBuilding.CancelPlacing();
        currentBuilding = null;
    }


    public void FinishPlacingBuilding() {
        if (!currentBuilding.CanPlace()) return;

        currentBuilding.FinishPlacing();
        currentBuilding = null;
    }

}
