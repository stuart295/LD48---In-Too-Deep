using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{

    public GameObject enemy1Pref;

    public float beginDelay = 2f;

    public AnimationCurve spawnDelay;
    public AnimationCurve spawnAmount;
    public List<Rect> spawnAreas;
    public float spawnRadius = 5;
    public float minBuildZ = 15f;

    [HideInInspector]
    public float furtherestBuildZ = 0;

    private float lastSpawnTime = 0f;
    private float nextSpawnTime = 0f;




    // Start is called before the first frame update
    void Awake() {
        lastSpawnTime = Time.time;
    }

    // Update is called once per frame
    void Update() {
        if (Time.time < beginDelay || furtherestBuildZ < minBuildZ) return;

        if (nextSpawnTime == 0f) nextSpawnTime = Time.time + spawnDelay.Evaluate(Time.time);

        if (nextSpawnTime <= Time.time) {
            nextSpawnTime = Time.time + spawnDelay.Evaluate(Time.time);
            SpawnEnemies();
        }

    }

    private void SpawnEnemies() {
        Debug.Log("Spawning enemies");
        Vector3 spawnPoint = GetSpawnPoint();
        Vector3 destination = new Vector3(Random.Range(-5, 5), 0, spawnPoint.z + Random.Range(-5,5));

        int spawnCount = (int)spawnAmount.Evaluate(Time.time);

        for (int i = 0; i < spawnCount; i++) {
            Vector2 offset = Random.insideUnitCircle * spawnRadius;


            GameObject enemyGo = Instantiate(enemy1Pref, spawnPoint + new Vector3(offset.x,0, offset.y), Quaternion.identity);
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
