using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Miner : Building
{
    public float tickDelay = 1f;
    public GameObject effect;

    private bool connected;
    private float lastTick;
    private Resource resource;

    public bool Connected { get => connected;  }

    public override bool CanPlace() {
        if (gm.Credits < settings.cost) return false;

        List<Building> onTopOf = gm.Grid.GetBuildingsAtWorldPos(transform.position);
        if (onTopOf == null || onTopOf.Count != 1) return false;

        return onTopOf[0]is Resource;
    }

    public override void FinishPlacing() {
        base.FinishPlacing();

        List<Building> onTopOf = gm.Grid.GetBuildingsAtWorldPos(transform.position);
        foreach (Building building in onTopOf) {
            if (building is Resource) {
                resource = (Resource)building;
                break;
            }
        }

        if (resource == null) {
            Debug.LogError("No resource found for miner!", gameObject);
            return;
        }

        resource.miner = this;
        gm.resources.Add(resource);
        CheckSPConnection();
        lastTick = Time.time;

        UpdateAdjacentPipes();
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
        effect.SetActive(false);

    }

    private bool PushAdjacentToStack(Building sourceBuilding, Stack<Building> toVisit, HashSet<Building> visited) {
        foreach (Building building in gm.Grid.GetAdjacentBuildings(sourceBuilding.transform.position)) {
           if (visited.Contains(building) || building == null || building.gameObject == null) continue;

            if (building.tag.Equals("SpacePort")) {
                connected = true;
                effect.SetActive(true);
                return true;
            }

            if (building is Pipe || building is Miner)
                toVisit.Push(building);
        }

        return false;
    }

    public override void Update() {
        if (!gm || gm.GameOver || placing) return;
        base.Update();
        UpdateTick();
    }

    private void UpdateTick() {
        if (!connected || Time.time - lastTick < tickDelay) return;

        lastTick = Time.time;

        gm.Credits += resource.creditsPerTick;
        gm.Score += resource.scoreValue;
        //TODO add effect
    }

    protected override void OnDeath() {
        base.OnDeath();
        gm.resources.Remove(resource);
        UpdateAdjacentPipes();
    }
}
