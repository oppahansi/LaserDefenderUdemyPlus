using UnityEngine;
using System.Collections;
using System;

public class EnemySpawner : MonoBehaviour {

    public static int enemyWave;

    public GameObject enemyPrefab;
    public float width = 10f;
    public float heigth = 5f;
    public float speed = 5f;
    public float spawnDelay = 0.5f;

    private bool movingRight = false;
    private float xMin;
    private float xMax;
    private PlayerController player;

	// Use this for initialization
	void Start () {
        float distanceToCamera = transform.position.z - Camera.main.transform.position.z;
        Vector3 leftBoundary = Camera.main.ViewportToWorldPoint(new Vector3(0f, 0f, distanceToCamera));
        Vector3 rightBoundary = Camera.main.ViewportToWorldPoint(new Vector3(1f, 0f, distanceToCamera));
        player = GameObject.Find("Player").GetComponent<PlayerController>();

        xMin = leftBoundary.x + 0.5f * width;
        xMax = rightBoundary.x - 0.5f * width;

        enemyWave = 1;
        SpawnUntilFull();
    }

    void SpawnUntilFull()
    {
        Transform freePosition = NextFreePosition();
        if (freePosition)
        {
            GameObject enemy = Instantiate(enemyPrefab, freePosition.position, Quaternion.identity) as GameObject;
            enemy.transform.parent = freePosition;
        }

        if (NextFreePosition())
        {
            Invoke("SpawnUntilFull", spawnDelay);
        }
    }
 
    public void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(width, heigth, 0f));
    }
	
	// Update is called once per frame
	void Update () {
        Vector3 updatedPosition = transform.position + (Vector3.right * speed * Time.deltaTime);
        if (updatedPosition.x > xMax || updatedPosition.x < xMin)
        {
            speed = -speed;
            float newX = Mathf.Clamp(updatedPosition.x, xMin, xMax);
            updatedPosition.x = newX;
        }
        transform.position = updatedPosition;

        if (AllMemebersDead())
        {
            SpawnUntilFull();
        }
    }

    Transform NextFreePosition()
    {
        foreach (Transform childPositionGameObject in transform)
        {
            if (childPositionGameObject.childCount == 0)
            {
                return childPositionGameObject;
            }
        }

        return null;
    }

    private bool AllMemebersDead()
    {

        foreach (Transform childPositionGameObject in transform)
        {
            if (childPositionGameObject.childCount > 0)
            {
                return false;
            }
        }

        enemyWave++;
        if (player.health < player.maxHealth)
        {
            player.AddHealth(50);
        }

        return true;
    }
}
