using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpacePort : Building
{
    protected override void OnDeath() {
        base.OnDeath();
        gm.OnGameOver();
    }
}
