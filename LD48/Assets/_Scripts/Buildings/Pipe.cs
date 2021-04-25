using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pipe : Building
{

    [Header("Pipe pieces")]
    public GameObject pipeStraight;
    public GameObject pipeJoin;

    public override void FinishPlacing() {
        base.FinishPlacing();
        gm.RecheckMiners();
        UpdatePieceType();
        UpdateAdjacentPipes();
    }

    protected override void OnDeath() {
        base.OnDeath();
        gm.RecheckMiners();
        UpdateAdjacentPipes();
    }

    public override void UpdatePlacement(Vector3 position) {
        base.UpdatePlacement(position);
        UpdatePieceType();
    }

   

    public void UpdatePieceType() {
        List<Building> adjacent = gm.Grid.GetAdjacentBuildings(transform.position);
        List<Building> adjConnections = new List<Building>();
        foreach (Building b in adjacent) {
            if (b is Pipe || b is Miner || b is SpacePort) adjConnections.Add(b);
        }

        transform.rotation = Quaternion.identity;

        switch (adjConnections.Count) {
            case 0:
                SetStraightPipe(true);
                break;

            case 1:
                SetStraightPipe(true);
                if (IsToSide(adjConnections[0])) {
                    transform.rotation = Quaternion.Euler(0, 90, 0);
                }
                break;

            case 2:
                bool toSide1 = IsToSide(adjConnections[0]);
                bool toSide2 = IsToSide(adjConnections[1]);
                if (toSide1 && toSide2) {
                    SetStraightPipe(true);
                    transform.rotation = Quaternion.Euler(0, 90, 0);
                }else if (!toSide1 && !toSide2) {
                    SetStraightPipe(true);
                }
                else {
                    SetStraightPipe(false);
                }

                break;

            default:
                SetStraightPipe(false);
                break;


        }
    }

    private void SetStraightPipe(bool straight=true) {
        pipeStraight.SetActive(straight);
        pipeJoin.SetActive(!straight);
    }

    private bool IsToSide(Building b) {
        return gm.Grid.GetGridPos(b.transform.position).x == gm.Grid.GetGridPos(transform.position).x;
    }


}
