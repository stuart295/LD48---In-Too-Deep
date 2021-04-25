using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{

    private GameController gm;
    private BuildController build;
    private UIController ui;

    private void Awake() {
        gm = GetComponent<GameController>();
        build = GetComponent<BuildController>();
        ui = GetComponent<UIController>();
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

        //Exit
        if (Input.GetKeyDown(KeyCode.Escape)) {
            SceneManager.LoadScene(0);
        }

    }

    private void UpdateBuildInput() {
        if (Input.GetMouseButton(0)) {
            if (ui.Focussed) {
                build.CancelPlacingBuilding();
            }
            else {
                build.FinishPlacingBuilding();
            }
            
        }
        else if (Input.GetMouseButtonDown(1)) {
            build.CancelPlacingBuilding();
        }

        

    }
}
