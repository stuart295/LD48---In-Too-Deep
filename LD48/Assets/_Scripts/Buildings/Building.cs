using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    [Header("Visuals")]
    public Renderer mainRenderer;
    public Color invalidPlacementColor = Color.red;
    public Color placingColor = Color.white;

    [Header("Grid settings")]
    public Vector2 gridSize = Vector2.one;

    [Header("Building settings")]
    public bool minable = false;
    public float health = 1f;


    protected bool placing = false;
    protected GameController gm;
    protected Color defaultColor = Color.white;
    protected BuildingSettings settings;

    public virtual void Initialize(GameController gm, BuildingSettings settings) {
        this.gm = gm;
        this.settings = settings;
    }

    private void Awake() {
        defaultColor = mainRenderer.material.color;
    }

    // Start is called before the first frame update
    public virtual void Start()
    {
        
    }

    // Update is called once per frame
    public virtual void Update()
    {
        
    }


    public virtual void StartPlacing() {
        placing = true;
        mainRenderer.material.color = placingColor;
        mainRenderer.material.renderQueue = 3050;
    }

    public virtual void FinishPlacing() {
        placing = false;
        gm.Grid.addBuilding(this);
        mainRenderer.material.color = defaultColor;
        mainRenderer.material.renderQueue = 3000;
    }

    public virtual void CancelPlacing() {
        if (!placing) return;
        placing = false;
        Destroy(gameObject);
    } 

    public void UpdatePlacement(Vector3 position) {
        transform.position = position;

        if (CanPlace()) {
            mainRenderer.material.color = placingColor;
        }
        else {
            mainRenderer.material.color = invalidPlacementColor;
        }

    }

    public virtual bool CanPlace() {
        if (gm.Credits < settings.cost) return false;

        return !gm.Grid.IsAreaOccupied(transform.position, gridSize);
    }

    public void TakeDamage(float attackDamage) {
        health -= attackDamage;
        Debug.Log(gameObject + " took " + attackDamage + " damage");
        if (health <= 0) {
            OnDeath();
            Destroy(gameObject);
        }
    }

    protected virtual void OnDeath() {
        Debug.Log(gameObject + " destroyed" );
    }
}
