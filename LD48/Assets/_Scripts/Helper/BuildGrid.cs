
using System.Collections.Generic;
using UnityEngine;

public class BuildGrid
{

    private Vector2 cellSize = Vector2.one;
    private Vector2 offset = Vector2.zero;

    private Dictionary<Vector3, GameObject> gridOccupancies;

    public BuildGrid(Vector2 cellSize, Vector2 offset) {
        this.cellSize = cellSize;
        this.offset = offset;
        gridOccupancies = new Dictionary<Vector3, GameObject>();
    }


    public Vector3 GetSnappedPos(Vector3 position, bool worldPos = true) {
        Vector3 snapped = position;
        snapped.x = Mathf.Round(position.x / cellSize.x);
        snapped.z = Mathf.Round(position.z / cellSize.y);

        if (worldPos) {
            snapped.x = (snapped.x * cellSize.x) + offset.x;
            snapped.z = (snapped.z * cellSize.y) + offset.y;
        }


        return snapped;
    }

}
