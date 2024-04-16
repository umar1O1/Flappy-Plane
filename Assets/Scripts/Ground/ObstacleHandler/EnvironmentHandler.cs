
using System;
using System.Collections.Generic;
using UnityEngine;


public enum EnvironmentType
{
    Dirt,
    Grass,
    Snow,
    Ice,

}

public class EnvironmentHandler : MonoBehaviour
{
    [field: SerializeField]
    public EnvironmentType currentEnvironment { get; private set; } = EnvironmentType.Dirt;

    [Space]

    public Transform spawnPosition;
    [Space]
    public Vector2 moveDirection;
    [Space(7f)]
    [SerializeField] ParallaxEffect bgParallax;
    [SerializeField] float parallaxFactor = 0.015f;
    [SerializeField] float minParallax = 0.015f;
    [SerializeField] float maxParallax = 0.5f;
    [Space(7f)]

    //Spawn Setting
    public float spawnInterval = 3f;
    public float spawnFactor = 1;

    [SerializeField] float minSpawnInterval = 0.7f;
    [SerializeField] float maxSpawnInterval = 3f;

    [Space(7f)]

    // Speed Setting
    public float moveSpeed;
    public float speedFactor = 0.7f;

    [SerializeField] float minSpeed = 0.7f;
    [SerializeField] float maxSpeed = 3f;




    [Header("Obstacle Data")]
    [Space]

    [SerializeField] List<GameObject> obstaclePrefabList = new();
    [Space(7)]
    [SerializeField] List<GameObject> _obstaclePool = new();


    [field: SerializeField] public float CurrentGameTime { get; private set; }
    [SerializeField] float changeWorldTime = 30f;
    [SerializeField] int count;
    [SerializeField] int world = 0;

    float localObstacleTime = 0f;
    float localWorldTime = 0f;

    [SerializeField] List<Transform> transformList = new();

    public static EnvironmentHandler Instance;

    public bool CanSpawnObstacle { get; set; }

    // Restart Data
    float orignalEnvMoveSpeed;
    float orignalBgMoveSpeed;
    float orignalSpawnTime;
    int orignalWorldCount;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        UnityEngine.Random.InitState((int)DateTime.Now.Ticks);



        MakeObstaclePool();
        CanSpawnObstacle = false;

        //For Resatr
        orignalEnvMoveSpeed = moveSpeed;
        orignalBgMoveSpeed = bgParallax.speedMultiplier;
        orignalSpawnTime = spawnInterval;
        orignalWorldCount = world;

    }



    private void Update()
    {
        if (!CanSpawnObstacle)
            return;

        CurrentGameTime += Time.deltaTime;

        localObstacleTime += Time.deltaTime;
        localWorldTime += Time.deltaTime;

        if (localObstacleTime > spawnInterval)
        {
            localObstacleTime = 0f;
            SpawnObstacle();

        }

        if (localWorldTime > changeWorldTime)
        {
            localWorldTime = 0;
            ChangeWorldSetting();
        }

    }
    void MakeObstaclePool()
    {
        GameObject obstaclePool = new("ObstaclePoolParent");
        GameObject tempObj;
        foreach (GameObject _prefabObj in obstaclePrefabList)
        {
            tempObj = Instantiate(_prefabObj, Vector3.zero, Quaternion.identity, obstaclePool.transform);
            _obstaclePool.Add(tempObj);
            tempObj.SetActive(false);
        }
    }


    void ChangeWorldSetting()
    {
        world++;

        if (world > 3)
        {
            world = 0;

        }

        currentEnvironment = (EnvironmentType)world;


        spawnInterval = Mathf.Clamp((spawnInterval - spawnFactor), minSpawnInterval, maxSpawnInterval);


        float newSpeed = moveSpeed + speedFactor;
        //moveSpeed = Mathf.SmoothStep(moveSpeed, newSpeed, 5f * Time.deltaTime);
        moveSpeed = Mathf.Clamp(newSpeed, minSpeed, maxSpeed);

        parallaxFactor = Mathf.Clamp(bgParallax.speedMultiplier + parallaxFactor, minParallax, maxParallax);
        bgParallax.speedMultiplier = parallaxFactor;

    }

    public void SpawnObstacle()
    {

        int _randNumber;



        _randNumber = RandomNumber(0, obstaclePrefabList.Count, 5);
        _obstaclePool[_randNumber].transform.position = spawnPosition.position;
        _obstaclePool[_randNumber].SetActive(true);


        count++;
    }
    int randomNumber = 0;
    int lastNumber = 0;
    int RandomNumber(int minNumber, int maxNumber, int maxAttempts)
    {
        for (int i = 0; randomNumber == lastNumber && i < maxAttempts; i++)
        {
            randomNumber = UnityEngine.Random.Range(minNumber, maxNumber);

        }

        lastNumber = randomNumber;
        return randomNumber;
    }


    public void SetEnvironment(bool status)
    {
        if (!status)
        {

            moveSpeed = 0f;
            bgParallax.speedMultiplier = 0;

            CanSpawnObstacle = false;


        }
        else
        {
            CurrentGameTime = 0f;
            moveSpeed = orignalEnvMoveSpeed;
            bgParallax.speedMultiplier = orignalBgMoveSpeed;
            world = orignalWorldCount;

            currentEnvironment = (EnvironmentType)world;

            spawnInterval = orignalSpawnTime;

            GroundMover[] gm = FindObjectsByType<GroundMover>((FindObjectsSortMode.None));
            foreach (var item in gm)
            {
                item.ResetRender();
            }

            CanSpawnObstacle = false;
        }




    }


    public void DisableObstacles()
    {
        foreach (var item in _obstaclePool)
        {
            item.SetActive(false);
        }
    }

}
