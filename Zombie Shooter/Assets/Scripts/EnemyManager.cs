using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Collections;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance;

    public List<Transform> spawnPoints = new List<Transform>();
    public List<Transform> navPoints = new List<Transform>();
    public List<Transform> endPoints = new List<Transform>();

    public List<Enemy> enemies = new List<Enemy>();

    public Transform enemyContainer = null;
    public List<GameObject> enemyPrefabs = new List<GameObject>();

    public void Start()
    {
        Instance = this;
        StartCoroutine(EnemySpawner());
    }

    public IEnumerator EnemySpawner()
    {
        while (true)
        {
            if (GameManager.Started && !GameManager.Paused)
            {
                if (enemies.Count() < 100)
                    SpawnEnemy();
            }

            yield return new WaitForSeconds(1);
        }
    }

    public void SpawnEnemy()
    {
        var random = new System.Random();
        var spawnPoint = spawnPoints[random.Next(spawnPoints.Count)];
        var enemyObject = Instantiate(enemyPrefabs[random.Next(enemyPrefabs.Count)], spawnPoint.position, Quaternion.identity, enemyContainer);
        var enemy = enemyObject.GetComponent<Enemy>();
        enemy.currentNavPoint = spawnPoint;

        enemies.Add(enemy);
    }

    public static Transform GetNextNavPoint(Transform currentNavPoint)
    {
        Transform chosenPoint = null;
        var pointsWithDistance = new Dictionary<Transform, float>();

        foreach (Transform point in Instance.navPoints)
        {
            if (point != currentNavPoint)
            {
                Vector3 directionToTarget = point.position - currentNavPoint.position;
                pointsWithDistance.Add(point, directionToTarget.sqrMagnitude);
            }
        }

        var threeClosestPoints = pointsWithDistance.OrderBy(pair => pair.Value).Take(10).ToList();

        var random = new System.Random();
        var randomizedList = threeClosestPoints.OrderBy(a => random.Next()).ToList();

        foreach (var point in randomizedList)
        {
            if (chosenPoint == null)
            {
                Vector3 newDirectionToTarget = point.Key.position - Instance.endPoints[0].position;
                float newDistance = newDirectionToTarget.sqrMagnitude;

                Vector3 oldDirectionToTarget = currentNavPoint.position - Instance.endPoints[0].position;
                float oldDistance = oldDirectionToTarget.sqrMagnitude;

                if (newDistance < oldDistance)
                    chosenPoint = point.Key;
            }
        }

        if (chosenPoint != null)
            return chosenPoint;
        else
            return currentNavPoint;
    }

    public void ResetManager()
    {
        foreach (var enemy in enemies.ToList())
        {
            if (enemy != null)
                Destroy(enemy.gameObject);
        }

        enemies.Clear();
    }
}
