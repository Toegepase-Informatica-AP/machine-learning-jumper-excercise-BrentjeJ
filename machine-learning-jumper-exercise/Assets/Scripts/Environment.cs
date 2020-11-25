using Assets.Scripts;
using TMPro;
using UnityEngine;

public class Environment : MonoBehaviour
{
    public Obstacle obstacle;
    public Player player;
    public float maxTime = 2f;
    public float minTime = 1f;

    private SpawnLine spawnLine;
    private SpawnLineCross spawnLineCross;

    private Player p;
    private TextMeshPro scoreBoard;

    private GameObject obstacles;
    private GameObject obstaclesCross;

    private Vector3 spawnLineLocation;
    private Vector3 spawnLineLocationCross;

    private GameObject floor;
    private GameObject floorCross;

    private Quaternion spawnLineRotation;
    private Quaternion spawnLineRotationCross;

    //current time
    private float currentTimer;

    //The time to spawn the object
    private float randomTimed;

    // Start is called before the first frame update
    private void Start()
    {
        currentTimer = 0f;
        SetRandomTimed();
    }

    private void SetRandomTimed()
    {
        randomTimed = Random.Range(minTime, maxTime);
    }

    public void OnEnable()
    {
        p = transform.GetComponentInChildren<Player>();
        scoreBoard = transform.GetComponentInChildren<TextMeshPro>();

        //for default
        obstacles = transform.Find("Obstacles").gameObject;             
        spawnLine = transform.GetComponentInChildren<SpawnLine>();        
        floor = transform.Find("Floor").gameObject;      

        spawnLineLocation = spawnLine.transform.position;
        spawnLineRotation = spawnLine.transform.rotation;

        //for cross
        obstaclesCross = transform.Find("ObstaclesCross").gameObject;
        spawnLineCross = transform.GetComponentInChildren<SpawnLineCross>();
        floorCross = transform.Find("FloorCross").gameObject;

        spawnLineLocationCross = spawnLineCross.transform.position;
        spawnLineRotationCross = spawnLineCross.transform.rotation;
    }

    public void ClearEnvironment()
    {
        foreach (Transform obstacle in obstacles.transform)
        {
            Destroy(obstacle.gameObject);
        }
    }

    public void SpawnPlayer()
    {
        Player newPlayer = Instantiate(player, new Vector3(0, 1.5f, 42.2f), new Quaternion(0f, 180f, 0f, 0f));
        newPlayer.transform.SetParent(gameObject.transform);
    }

    private void SpawnObstacle()
    {
        ResetTimer();
        //for default
        Obstacle newObstacle = Instantiate(obstacle, new Vector3(spawnLineLocation.x, floor.transform.position.y + 0.5f, spawnLineLocation.z), spawnLineRotation);
        newObstacle.transform.SetParent(obstacles.transform);
        
    }

    private void SpawnObstacleCross()
    {
        ResetTimer();
        //for cross
        Obstacle newObstacleCross = Instantiate(obstacle, new Vector3(spawnLineLocationCross.x, floorCross.transform.position.y + 0.5f, spawnLineLocationCross.z), spawnLineRotationCross);
        newObstacleCross.transform.SetParent(obstaclesCross.transform);
    }

    private void ResetTimer()
    {
        currentTimer = 0;
    }

    private void FixedUpdate()
    {
        //Counts up
        currentTimer += Time.deltaTime;
        int randomValue = Random.Range(1, 3);

        //Check if its the right time to spawn the object
        if (CanSpawn())
        {
            switch (randomValue)
            {
                case 1:
                    SpawnObstacle();
                    break;
                case 2:
                    SpawnObstacleCross();
                    break;
                default:
                    break;
            }

            RestartCurrentTimer();
        }

        scoreBoard.text = p.GetCumulativeReward().ToString("f3");
    }

    private bool CanSpawn()
    {
        return randomTimed <= currentTimer;
    }

    private void RestartCurrentTimer()
    {
        currentTimer = 0f;
    }
}
