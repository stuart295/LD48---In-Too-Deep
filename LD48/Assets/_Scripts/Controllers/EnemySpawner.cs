using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{

    public GameObject normAlienPref;
    public GameObject hardAlienPref;

    public float beginDelay = 2f;

    public AnimationCurve spawnDelay;

    public AnimationCurve normalAlienSpawnAmount;
    public AnimationCurve hardAlienSpawnAmount;


    public List<Rect> spawnAreas;
    public float spawnRadius = 5;
    public float minBuildZ = 15f;

    [HideInInspector]
    public float furtherestBuildZ = 0;

    private float lastSpawnTime = 0f;
    private float nextSpawnTime = 0f;

    private GameController gm;




    // Start is called before the first frame update
    void Awake() {
        lastSpawnTime = Time.time;
        gm = GetComponent<GameController>();
    }

    // Update is called once per frame
    void Update() {
        if (Time.time < beginDelay || furtherestBuildZ < minBuildZ) return;

        if (nextSpawnTime == 0f) nextSpawnTime = Time.time + spawnDelay.Evaluate(Time.time/gm.timeLimit);

        if (nextSpawnTime <= Time.time) {
            nextSpawnTime = Time.time + spawnDelay.Evaluate(Time.time / gm.timeLimit);
            SpawnWave();
        }

    }

    private void SpawnWave() {
        Debug.Log("Spawning enemies");
        Vector3 spawnPoint = GetSpawnPoint();
        Vector3 destination = new Vector3(Random.Range(-5, 5), 0, spawnPoint.z + Random.Range(-5,5));

        //Spawn
        SpawnEnemies(normAlienPref, normalAlienSpawnAmount, spawnPoint, destination);
        SpawnEnemies(hardAlienPref, hardAlienSpawnAmount, spawnPoint, destination);
        
    }

    private void SpawnEnemies(GameObject prefab, AnimationCurve spawnCountCurve, Vector3 spawnPoint, Vector3 destination) {
        int spawnCount = Mathf.RoundToInt(spawnCountCurve.Evaluate(Time.time / gm.timeLimit));

        for (int i = 0; i < spawnCount; i++) {
            Vector2 offset = Random.insideUnitCircle * spawnRadius;
            GameObject enemyGo = Instantiate(prefab, spawnPoint + new Vector3(offset.x, 0, offset.y), Quaternion.identity);
            Alien alien = enemyGo.GetComponent<Alien>();
            alien.Destination = destination;
        }
    }

    private Vector3 GetSpawnPoint() {
        int areaIdx = Random.Range(0, spawnAreas.Count);
        Rect area = new Rect(spawnAreas[areaIdx]);

        area.yMax = Mathf.Min(area.yMax, furtherestBuildZ);

        return new Vector3(Random.Range(area.xMin, area.xMax), 0f, Random.Range(area.yMin, area.yMax));

    }

    private void OnDrawGizmos() {
#if UNITY_EDITOR
        if (Selection.Contains(gameObject)) {
            Gizmos.color = Color.red;
            foreach (Rect rect in spawnAreas) {
                Vector3 center = new Vector3(rect.center.x, 0f, rect.center.y);
                Vector3 size = new Vector3(rect.size.x, 0f, rect.size.y);

                Gizmos.DrawWireCube(center, size);
            }
        }

#endif
    }

}
