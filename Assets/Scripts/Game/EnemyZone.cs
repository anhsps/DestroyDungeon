using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyZone : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private float spawnTime = 3f;

    [SerializeField] private bool playInZone;
    private float timer;
    private List<Enemy> enemies = new List<Enemy>();
    Collider col;

    // Start is called before the first frame update
    void Start()
    {
        col = GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playInZone)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                SpawnEnemy();
                timer = spawnTime;
            }
        }
    }

    private void SpawnEnemy()
    {
        if (enemies.Count > 10) return;
        GameObject eObj = Instantiate(enemyPrefab, GetRandomPos(), Quaternion.identity, transform);
        Enemy enemy = eObj.GetComponent<Enemy>();
        enemy.canMove = true;
        enemy.SetZone(col);
        enemies.Add(enemy);
    }

    private Vector3 GetRandomPos()
    {
        Bounds bounds = col.bounds;
        return new Vector3(Random.Range(bounds.min.x, bounds.max.x),
                            bounds.min.y,
                            Random.Range(bounds.min.z, bounds.max.z));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playInZone = true;
            foreach (Enemy enemy in enemies)
                if (enemy) enemy.canMove = true;
            timer = 0;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playInZone = false;
            foreach (Enemy enemy in enemies)
                if (enemy) enemy.StopMove();
        }
    }

    public void RemoveEnemy(Enemy enemy)
    {
        if (enemies.Contains(enemy))
            enemies.Remove(enemy);
    }
}
