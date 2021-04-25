using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Movement settings")]
    public float panSpeed = 1f;
    public float mouseEdgeDistanceThreshold = 0.0f;

    [Header("Boundaries")]
    public Vector3 minBoundary;
    public Vector3 maxBoundary;

    private void Awake() {
        Cursor.lockState = CursorLockMode.Confined;
    }


    // Update is called once per frame
    void Update()
    {
        UpdateCamera();
    }

    private void UpdateCamera() {

        Vector2 mouseInputDirection = GetMouseInputDir();
        Vector2 keyInputDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        //Get the largest component in each direction as the final direction input
        Vector2 moveDirection = mouseInputDirection;
        if (Mathf.Abs(keyInputDirection.x) > Mathf.Abs(mouseInputDirection.x)) moveDirection.x = keyInputDirection.x;
        if (Mathf.Abs(keyInputDirection.y) > Mathf.Abs(mouseInputDirection.y)) moveDirection.y = keyInputDirection.y;


        MoveCamera(moveDirection.normalized);
    }

    private void MoveCamera(Vector2 moveDirection) {
        Vector3 transformedDir = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0) * new Vector3(moveDirection.x, 0, moveDirection.y) ;
      

        Vector3 targPos = transform.position + transformedDir;
        float height = targPos.y;
        targPos = Vector3.Max(minBoundary, Vector3.Min(maxBoundary, targPos));
        targPos.y = height;

        transform.position = Vector3.MoveTowards(transform.position, targPos, Time.deltaTime * panSpeed);
    }

    private Vector2 GetMouseInputDir() {

        Vector2 mousePos = Camera.main.ScreenToViewportPoint(Input.mousePosition);

        Vector2 moveDir = Vector2.zero;

        //Horizontal dir
        if (mousePos.x <= mouseEdgeDistanceThreshold) moveDir.x = -1;
        else if (mousePos.x >= 1f - mouseEdgeDistanceThreshold) moveDir.x = 1;

        //Vertical dir
        if (mousePos.y <= mouseEdgeDistanceThreshold) moveDir.y = -1;
        else if (mousePos.y >= 1f - mouseEdgeDistanceThreshold) moveDir.y = 1;


        return moveDir;
    }


    private void OnDrawGizmos() {
        #if UNITY_EDITOR
        if (Selection.Contains(gameObject)) {
            Gizmos.color = Color.green;
            Vector3 center = (minBoundary + maxBoundary) / 2f;
            Vector3 size = maxBoundary - minBoundary;

            Gizmos.DrawWireCube(center, size);
        }
        #endif
    }

}
