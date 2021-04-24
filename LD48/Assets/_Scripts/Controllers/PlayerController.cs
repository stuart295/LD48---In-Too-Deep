using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private GameController gm;
    private BuildController build;

    private void Awake() {
        gm = GetComponent<GameController>();
        build = GetComponent<BuildController>();
    }


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (build.IsPlacing()) {
            UpdateBuildInput();
        }

    }

    private void UpdateBuildInput() {
        if (Input.GetMouseButtonDown(0)) {
            build.FinishPlacingBuilding();
        }
        else if (Input.GetMouseButtonDown(1)) {
            build.CancelPlacingBuilding();
        }
    }
}
