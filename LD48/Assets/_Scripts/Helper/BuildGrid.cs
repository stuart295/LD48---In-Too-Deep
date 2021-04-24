
using System;
using System.Collections.Generic;
using UnityEngine;

public class BuildGrid
{

    private Vector2 cellSize = Vector2.one;
    private Vector2 offset = Vector2.zero;

    private Dictionary<Vector3, Building> gridOccupancies;

    public BuildGrid(Vector2 cellSize, Vector2 offset) {
        this.cellSize = cellSize;
        this.offset = offset;
        gridOccupancies = new Dictionary<Vector3, Building>();
    }


    public Vector3 GetGridPos(Vector3 position, bool worldPos = true) {
        Vector3 snapped = position;
        snapped.x = Mathf.Round(position.x / cellSize.x);
        snapped.z = Mathf.Round(position.z / cellSize.y);

        if (worldPos) {
            snapped.x = (snapped.x * cellSize.x) + offset.x;
            snapped.z = (snapped.z * cellSize.y) + offset.y;
        }


        return snapped;
    }

    public Vector3 GetGridMousePos() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        LayerMask mask = LayerMask.GetMask(new string[] { "Ground" });

        if (Physics.Raycast(ray, out hit, 1000f, mask)) {
            Vector3 hitPoint = hit.point;
            hitPoint.y = 0f;

            return GetGridPos(hitPoint);
        }

        return Vector3.zero;
    }

    public bool IsCellOccupied(Vector3 cellPos) {
        return gridOccupancies.ContainsKey(cellPos);
    }

    public bool IsPositionOccupied(Vector3 worldPosition) {
        Vector3 cellPos = GetGridPos(worldPosition);
        return gridOccupancies.ContainsKey(cellPos);
    }

    public bool IsAreaOccupied(Vector3 worldPosition, Vector2 areaSize) {
        Vector3 cellPosOrig = GetGridPos(worldPosition);
        for (int i = 0; i < areaSize.x; i++) {
            for (int j = 0; j < areaSize.y; j++) {
                Vector3 cellPos = cellPosOrig + new Vector3(i * cellSize.x, 0f, j * cellSize.y);
                if (IsCellOccupied(cellPos)) return true;
            }

        }

        return false;
    }

    public void addBuilding(Building building) {
        Vector3 buildPos = building.transform.position;
        for (int i = 0; i < building.gridSize.x; i++) {
            for (int j = 0; j < building.gridSize.y; j++) {
                Vector3 cellPos = buildPos + new Vector3(i * cellSize.x, 0f, j * cellSize.y);

                if (IsCellOccupied(cellPos)) {
                    Debug.LogError("Trying to place building in occupied cell!");
                    return;
                }
                else {
                    gridOccupancies.Add(cellPos, building);
                }
            }
        }
    }
}
