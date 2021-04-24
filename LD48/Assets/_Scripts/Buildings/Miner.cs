using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Miner : Building
{
    public override bool CanPlace() {
        List<Building> onTopOf = gm.Grid.GetBuildingsAtWorldPos(transform.position);
        if (onTopOf == null || onTopOf.Count != 1) return false;

        return onTopOf[0].minable;
    }



}
