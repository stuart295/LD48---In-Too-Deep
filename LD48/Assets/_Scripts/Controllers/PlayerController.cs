using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private GameController gm;

    private void Awake() {
        gm = GetComponent<GameController>();
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //RaycastHit hit;

        //if (Physics.Raycast(ray, out hit)) {
        //    Vector3 hitPoint = hit.point;
        //    hitPoint.y = 0f;

        //    Vector3 gridPos = grid.GetSnappedPos(hitPoint);

        //    test.transform.position = gridPos;
        //}

    }
}
