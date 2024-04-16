using System.Collections.Generic;
using UnityEngine;

public class CoinsSpawner : MonoBehaviour
{
    public bool canSpawnCoin;
    [SerializeField] GameObject coinPrefab;
    [SerializeField] float spawnInterval;
    [SerializeField] BoxCollider2D boxCollider;

    float localTime;
    Bounds bounds;

    [SerializeField] Vector3 spawnVector;
    [Header("Pool Setting")]
    [SerializeField] float poolSize;
    [SerializeField] List<GameObject> poolList = new();

    //Local Variable
    [SerializeField] int poolCount;
    GameObject _coin;
    //
    float offsetX;
    float offsetY;
    private void Start()
    {
        poolCount = 0;
        localTime = spawnInterval;
        bounds = boxCollider.bounds;
        GameObject coinsParent = new("Coins");
        coinsParent.transform.parent = transform;
        for (int i = 0; i < poolSize; ++i)
        {
            GameObject temp = Instantiate(coinPrefab, coinsParent.transform);
            temp.SetActive(false);
            poolList.Add(temp);
        }

    }
    private void Update()
    {
        localTime += Time.deltaTime;
        if (localTime > spawnInterval && canSpawnCoin)
        {
            if (poolCount >= poolList.Count)
            {
                poolCount = 0;
            }
            _coin = poolList[poolCount];
            poolCount++;

            _coin.SetActive(true);
            offsetX = Random.Range(bounds.min.x, bounds.max.x);
            offsetY = Random.Range(bounds.min.y, bounds.max.y);

            spawnVector.x = offsetX;
            spawnVector.y = offsetY;
            spawnVector.z = 0;

            localTime = 0;
            _coin.transform.localPosition = spawnVector;
        }
    }

}
