using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{

    [Header("References")]
    public GameObject buildingBar;
    public TMP_Text scoreText;
    public TMP_Text creditsText;

    [Header("Prefabs")]
    public GameObject buildIconPref;

    private BuildController build;
    private GameController gm;

    private void Awake() {
        build = GetComponent<BuildController>();
        gm = GetComponent<GameController>();
        PopulateBuildBar();
    }

    private void PopulateBuildBar() {
       foreach (BuildingSettings buildingSettings in build.buildingOptions) {
            GameObject iconGo = GameObject.Instantiate(buildIconPref, buildingBar.transform);

            //Set Icon
            Image iconImage = iconGo.transform.Find("Icon").GetComponent<Image>();
            iconImage.sprite = buildingSettings.icon;

            //Set cost display
            TMP_Text costText = iconGo.transform.Find("CostText").GetComponent<TMP_Text>();
            costText.text = buildingSettings.cost.ToString();

            //On click
            Button button = iconGo.GetComponent<Button>();
            button.onClick.AddListener(() => build.StartPlacingBuilding(buildingSettings));

        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateScore();
    }

    private void UpdateScore() {
        scoreText.text = gm.GetScore().ToString();
        creditsText.text = gm.Credits.ToString();
    }
}