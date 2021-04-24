
using System;
using System.Collections.Generic;
using UnityEngine;

public class BuildGrid
{

    private Dictionary<Vector3Int, List<Building>> gridOccupancies;

    public BuildGrid() {
        gridOccupancies = new Dictionary<Vector3Int, List<Building>>();
    }


    public Vector3Int GetGridPos(Vector3 position) {
        return Vector3Int.RoundToInt(position);
    }

    public Vector3Int GetGridMousePos() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        LayerMask mask = LayerMask.GetMask(new string[] { "Ground" });

        if (Physics.Raycast(ray, out hit, 1000f, mask)) {
            Vector3 hitPoint = hit.point;
            hitPoint.y = 0f;

            return GetGridPos(hitPoint);
        }

        return Vector3Int.zero;
    }

    public List<Building> GetBuildingsAtWorldPos(Vector3 worldPos) {
        Vector3Int cellPos = GetGridPos(worldPos);
        return GetBuildingsAtGridPos(cellPos);
    }

    public List<Building> GetBuildingsAtGridPos(Vector3Int gridPos) {
        if (gridOccupancies.ContainsKey(gridPos)) {
            return gridOccupancies[gridPos];
        }

        return new List<Building>();
    }

    public bool IsCellOccupied(Vector3Int cellPos) {
        return gridOccupancies.ContainsKey(cellPos);
    }

    public bool IsPositionOccupied(Vector3 worldPosition) {
        Vector3Int cellPos = GetGridPos(worldPosition);
        return gridOccupancies.ContainsKey(cellPos);
    }

    public bool IsAreaOccupied(Vector3 worldPosition, Vector2 areaSize) {
        Vector3Int cellPosOrig = GetGridPos(worldPosition);
        for (int i = 0; i < areaSize.x; i++) {
            for (int j = 0; j < areaSize.y; j++) {
                Vector3Int cellPos = cellPosOrig + new Vector3Int(i, 0, j);
                if (IsCellOccupied(cellPos)) return true;
            }

        }

        return false;
    }

    public void addBuilding(Building building) {
        Vector3Int buildPos = GetGridPos(building.transform.position);
        for (int i = 0; i < building.gridSize.x; i++) {
            for (int j = 0; j < building.gridSize.y; j++) {
                Vector3Int cellPos = buildPos + new Vector3Int(i, 0, j);

                if (IsCellOccupied(cellPos)) {
                    gridOccupancies[cellPos].Add(building);
                    return;
                }
                else {
                    gridOccupancies.Add(cellPos, new List<Building>() { building });
                }
            }
        }
    }

    internal void RemoveBuilding(Building building) {
        Vector3Int cellPos = GetGridPos(building.transform.position);

        if (!gridOccupancies.ContainsKey(cellPos)) return;

        List<Building> buildings = gridOccupancies[cellPos];
        if (buildings.Count > 1) {
            buildings.Remove(building);
            gridOccupancies[cellPos] = buildings;
        }
        else {
            gridOccupancies.Remove(cellPos);
        }

    }

    public List<Building> GetAdjacentBuildings(Vector3 worldPos) {
        List<Building> outList = new List<Building>();

        Vector3Int origCellPos = GetGridPos(worldPos);

        for (int i = 0; i < 4; i++) {
            Vector3Int offset = new Vector3Int((int)Mathf.Sin(i * Mathf.PI/2f), 0, (int)Mathf.Cos(i * Mathf.PI/2f));
            List<Building> buildings = GetBuildingsAtGridPos(origCellPos + offset);
            outList.AddRange(buildings);

        }


        return outList;
    } 

}
