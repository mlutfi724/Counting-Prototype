using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawnController : MonoBehaviour
{
    [SerializeField] private Transform mainCamera;
    [SerializeField] private GameObject[] spawnableObjectPrefabs;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float xRange = 1.6f;
    [SerializeField] private float zRange = 1.75f;

    private GameManager gameManager;

    public float objectFallDuration = 1f;

    private float horizontalInput;
    private float verticalInput;

    private bool canSpawn;

    // Start is called before the first frame update
    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    private void Update()
    {
        // Using input to spawn the object
        if (Input.GetKey(KeyCode.Space) && gameManager.isGameActive)
        {
            SpawnObjectValidate();
        }

        // The object can only move if it already spawned
        if (canSpawn && gameManager.isGameActive)
        {
            ObjectSpawnerMovement();
        }

        // Checking if the objectSpawner still in move area
        CheckingMoveBounds();
    }

    private void ObjectSpawnerMovement()
    {
        // Inputs
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        // Camera Directions
        Vector3 mainCameraRight = mainCamera.transform.right;
        Vector3 mainCameraForward = mainCamera.transform.forward;

        mainCameraRight.y = 0f;
        mainCameraForward.y = 0f;

        // Creating relative camera movement
        transform.Translate(mainCameraRight.normalized * moveSpeed * horizontalInput * Time.deltaTime);
        transform.Translate(mainCameraForward.normalized * moveSpeed * verticalInput * Time.deltaTime);
    }

    private void SpawnObjectValidate()
    {
        if (canSpawn)
        {
            StartCoroutine(SpawnDelay());
        }
    }

    private IEnumerator SpawnDelay()
    {
        canSpawn = false;
        yield return new WaitForSeconds(objectFallDuration);
        SpawnObject();
    }

    public void SpawnObject()
    {
        canSpawn = true;
        int randomizeObject = Random.Range(0, spawnableObjectPrefabs.Length);
        Instantiate(spawnableObjectPrefabs[randomizeObject], transform.position, spawnableObjectPrefabs[randomizeObject].transform.rotation);
    }

    private void CheckingMoveBounds()
    {
        // Check for position.x bounds
        if (transform.position.x < -xRange)
        {
            transform.position = new Vector3(-xRange, transform.position.y, transform.position.z);
        }

        if (transform.position.x > xRange)
        {
            transform.position = new Vector3(xRange, transform.position.y, transform.position.z);
        }

        // Check for position.z bounds
        if (transform.position.z < -zRange)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, -zRange);
        }

        if (transform.position.z > zRange)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, zRange);
        }
    }
}