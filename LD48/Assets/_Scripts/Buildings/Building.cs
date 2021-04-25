using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    [Header("Visuals")]
    //public Renderer mainRenderer;
    public List<Renderer> renderers;
    public Color invalidPlacementColor = Color.red;
    public Color placingColor = Color.white;

    [Header("Grid settings")]
    public Vector2 gridSize = Vector2.one;

    [Header("Building settings")]
    public float health = 1f;

    protected bool placing = false;
    protected GameController gm;
    protected List<Color> defaultColors;
    protected BuildingSettings settings;

    public bool Placing { get => placing;  }

    public virtual void Initialize(GameController gm, BuildingSettings settings) {
        this.gm = gm;
        this.settings = settings;
    }

    private void Awake() {
        foreach (Renderer r in renderers) {
            defaultColors.Add(r.material.color);
        }
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

        foreach (Renderer r in renderers) {
            r.material.color = placingColor;
            r.material.renderQueue = 3050;
        }
    }

    public virtual void FinishPlacing() {
        placing = false;
        gm.Grid.addBuilding(this);
        for (int i = 0; i < renderers.Count; i++) {
            Renderer r = renderers[i];
            r.material.color = defaultColors[i];
            r.material.renderQueue = 3000;
        }
    }

    public virtual void CancelPlacing() {
        if (!placing) return;
        placing = false;
        Destroy(gameObject);
    } 

    public virtual void UpdatePlacement(Vector3 position) {
        transform.position = position;

        if (CanPlace()) {
            foreach (Renderer r in renderers) {
                r.material.color = placingColor;
            }
        }
        else {
            foreach (Renderer r in renderers) {
                r.material.color = invalidPlacementColor;
            }
        }

    }

    protected virtual void UpdateAdjacentPipes() {
        List<Building> adjacent = gm.Grid.GetAdjacentBuildings(transform.position);
        foreach (Building b in adjacent) {
            if (b is Pipe) ((Pipe)b).UpdatePieceType();
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
        gm.Grid.RemoveBuilding(this);
    }
}
