using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Miner : Building
{

    public bool connected;

    public override bool CanPlace() {
        List<Building> onTopOf = gm.Grid.GetBuildingsAtWorldPos(transform.position);
        if (onTopOf == null || onTopOf.Count != 1) return false;

        return onTopOf[0].minable;
    }

    public override void FinishPlacing() {
        base.FinishPlacing();
        gm.miners.Add(this);
        CheckSPConnection();
    }

    public void CheckSPConnection() {

        HashSet<Building> visited = new HashSet<Building>();
        Stack<Building> toVisit = new Stack<Building>();

        if (PushAdjacentToStack(this, toVisit, visited)) return;

        while (toVisit.Count > 0) {

            Building building = toVisit.Pop();
            visited.Add(building);

            //Add new pipes to stack
            if (PushAdjacentToStack(building, toVisit, visited)) return;
        }

        connected = false;

    }

    private bool PushAdjacentToStack(Building sourceBuilding, Stack<Building> toVisit, HashSet<Building> visited) {
        foreach (Building building in gm.Grid.GetAdjacentBuildings(sourceBuilding.transform.position)) {
           if (visited.Contains(building)) continue;

            if (building.tag.Equals("SpacePort")) {
                connected = true;
                return true;
            }

            if (building is Pipe)
                toVisit.Push(building);
        }

        return false;
    }


}
