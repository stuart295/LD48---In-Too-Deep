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


    private bool placing = false;
    private GameController gm;

    public void Initialize(GameController gm) {
        this.gm = gm;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void StartPlacing() {
        placing = true;
        mainRenderer.material.color = placingColor;
        mainRenderer.material.renderQueue = 3050;
    }

    public void FinishPlacing() {
        placing = false;
        gm.Grid.addBuilding(this);
        mainRenderer.material.color = Color.white;
        mainRenderer.material.renderQueue = 3000;
    }

    public void CancelPlacing() {
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
        return !gm.Grid.IsAreaOccupied(transform.position, gridSize);
    }

}
