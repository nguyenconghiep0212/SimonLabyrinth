using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{

    // Property to access the instance
    public static EnemyManager Instance { get; set; }

    private void Awake()
    {
        if (Instance != null & Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            Instance = this;
        }
    }

    [SerializeField] bool hasInitSpawn;
    [SerializeField] List<SpawnClass> spawnList = new List<SpawnClass>();
    [SerializeField] float spawnInterval;
    [SerializeField] int maxSpawn = 10;

    public List<GameObject> _listEnemy = new List<GameObject>();

    int currentEnemyCount
    {
        get
        {
            return _listEnemy.Count;
        }
    }


    float timer;
    // Start is called before the first frame update
    void Start()
    {
        hasInitSpawn = true;
        timer = spawnInterval;
    }

    // Update is called once per frame
    void Update()
    {
        if (hasInitSpawn)
        {
            timer -= Time.deltaTime;
            if (timer < 0f)
            {
                SpawnEnemyByChance();
                timer = spawnInterval;
            }
        }
    }

    private Vector3 GetRandomSpawnPosition()
    {
        float distanceFromPOV = 3f;

        Vector3 bottomLeft = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, Camera.main.nearClipPlane));
        Vector3 topRight = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, Camera.main.nearClipPlane));

        float minX = bottomLeft.x;
        float maxX = topRight.x;
        float minY = bottomLeft.y;
        float maxY = topRight.y;

        Vector3 spawnPosition;
        if (Random.value < 0.5f)
        {
            // Spawn Horizonal
            float leftOrRight = Random.value < 0.5f ? maxX + distanceFromPOV : minX - distanceFromPOV;
            spawnPosition = new Vector3(leftOrRight, Random.Range(minY - distanceFromPOV, maxY + distanceFromPOV), 0);
        }
        else
        {
            // Spawn Vertical
            float topOrBottom = Random.value < 0.5f ? maxY + distanceFromPOV : minY - distanceFromPOV;
            spawnPosition = new Vector3(Random.Range(minX - distanceFromPOV, maxX + distanceFromPOV), topOrBottom, 0);
        }
        return spawnPosition;
    }

    private void SpawnEnemyByChance()
    {
        _listEnemy.RemoveAll(e => e == null);
        if (currentEnemyCount < maxSpawn)
        {
            float randomValue = Random.value;
            float cumulativeChance = 0f;

            foreach (SpawnClass enemy in spawnList)
            {
                cumulativeChance += enemy.spawnChance;
                if (randomValue < cumulativeChance)
                {
                    Vector3 spawnPosition = GetRandomSpawnPosition();
                    GameObject newEnemy = Instantiate(enemy.enemyPrefab, spawnPosition, Quaternion.identity);
                    newEnemy.transform.parent = transform;
                    _listEnemy.Add(newEnemy);
                    break;
                }
            }
        }
    }
}
