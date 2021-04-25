using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{

    [Header("References")]
    public GameObject buildingBar;
    public TMP_Text scoreText;
    public TMP_Text creditsText;
    public TMP_Text timeText;
    public TMP_Text gameOverText;
    public GameObject gameOverPanel;

    [Header("Prefabs")]
    public GameObject buildIconPref;

    private BuildController build;
    private GameController gm;

    private Dictionary<BuildingSettings, Button> buildingButtonMap;

    private void Awake() {
        build = GetComponent<BuildController>();
        gm = GetComponent<GameController>();
        PopulateBuildBar();
    }

    private void PopulateBuildBar() {
        buildingButtonMap = new Dictionary<BuildingSettings, Button>();
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
            buildingButtonMap.Add(buildingSettings, button);

        }
    }

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        UpdateScore();
        UpdateBuildAvailability();
    }

    private void UpdateScore() {
        scoreText.text = gm.Score.ToString();
        creditsText.text = gm.Credits.ToString();

        TimeSpan t = TimeSpan.FromSeconds(gm.RemainingTime);
        timeText.text = string.Format("Storm inbound: {0:D2}:{1:D2}", t.Minutes, t.Seconds);
    }

    private void UpdateBuildAvailability() {
        foreach (BuildingSettings buildingSettings in build.buildingOptions) {
            Button button = buildingButtonMap[buildingSettings];
            button.interactable = gm.Credits >= buildingSettings.cost;

        }
 
    }

    public void ShowGameOver() {
        gameOverPanel.SetActive(true);
        gameOverText.text = "Score\n" + gm.Score.ToString();
    }

    public void OnExitClick() {
        Application.Quit();
    }

    public void OnRetryClick() {
        SceneManager.LoadScene(1);
    }

}
