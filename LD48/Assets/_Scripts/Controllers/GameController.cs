using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    [Header("Grid settings")]
    public Vector2 cellSize = Vector2.one;
    public Vector2 cellOffset = Vector2.zero;

    private BuildGrid grid;

    public BuildGrid Grid { get => grid;}


    private void Awake() {
        grid = new BuildGrid(cellSize, cellOffset);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
