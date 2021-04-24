using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pipe : Building
{

    public override void FinishPlacing() {
        base.FinishPlacing();
        gm.RecheckMiners();
    }

    protected override void OnDeath() {
        base.OnDeath();
        gm.RecheckMiners();
    }
}
