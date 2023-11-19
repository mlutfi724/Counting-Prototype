using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class ObjectController : MonoBehaviour
{
    private bool isDropped;
    private bool isFalling;

    private Rigidbody objectRb;
    private ObjectSpawnController spawnController;

    private GameManager gameManager;

    // Start is called before the first frame update
    private void Start()
    {
        objectRb = GetComponent<Rigidbody>();
        spawnController = FindObjectOfType<ObjectSpawnController>();
        gameManager = FindObjectOfType<GameManager>();

        isDropped = false;
        if (transform.position.y < 40.6f)
        {
            objectRb.useGravity = true;
            isFalling = true;
            isDropped = true;
        }
    }

    private void OnEnable()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        if (!isFalling)
        {
            transform.position = spawnController.transform.position;
        }

        if (Input.GetKeyDown(KeyCode.Space) && gameManager.isGameActive)
        {
            StartCoroutine(DroppingObject());
            objectRb.useGravity = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == gameObject.tag) // checking if this object collides with the same tag
        {
            MergeObjects();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isDropped && other.gameObject.CompareTag("limit"))
        {
            gameManager.GameOver();
        }
    }

    private IEnumerator DroppingObject()
    {
        isFalling = true;
        isDropped = false;
        yield return new WaitForSeconds(spawnController.objectFallDuration);
        isDropped = true;
    }

    private void MergeObjects()
    {
        int objectIndex = int.Parse(gameObject.tag);
        int scoreToAdd = objectIndex * 5 + 10;

        GameManager.newObjectPos = transform.position;
        GameManager.isNewObjectSpawned = true;
        GameManager.objectIndex = objectIndex;

        gameManager.UpdateScore(scoreToAdd);
        //GameManager.objectIndex = int.Parse(gameObject.tag);
        Destroy(gameObject);

        //add score
    }
}