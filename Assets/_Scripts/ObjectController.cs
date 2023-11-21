using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class ObjectController : MonoBehaviour
{
    [SerializeField] private ParticleSystem smokeParticle;

    [SerializeField] private AudioClip[] collideSFX;
    [SerializeField] private AudioClip mergeSFX;
    [SerializeField] private AudioClip inflatingSFX;

    private bool isDropped;
    private bool isFalling;

    private Rigidbody objectRb;
    private ObjectSpawnController spawnController;

    private GameManager gameManager;

    //Scale Growth
    [SerializeField] private float maxSize;

    [SerializeField] private bool isMaxSize;

    private float timer = 0f;
    private float growTime = 1f;

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
            //smokeParticle.Play();
            SoundFXManager.instance.PlaySoundFXClip(mergeSFX, transform, 1f);
            // objectAudio.PlayOneShot(mergeSFX);
        }
    }

    private void OnEnable()
    {
        if (!isMaxSize)
        {
            StartCoroutine(GrowObject());
        }
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
            Debug.Log("Object Merged!");
        }
        else
        {
            SoundFXManager.instance.PlayRandomSoundFXClip(collideSFX, transform, 1f);
            // objectAudio.PlayOneShot(collideSFX);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isDropped && other.gameObject.CompareTag("limit"))
        {
            gameManager.GameOver();
        }
    }

    // Give a time to check game over condition
    private IEnumerator DroppingObject()
    {
        isFalling = true;
        isDropped = false;
        yield return new WaitForSeconds(spawnController.objectFallDuration);
        isDropped = true;
    }

    private IEnumerator GrowObject()
    {
        //isMaxSize = false;
        Vector3 startScale = transform.localScale;
        Vector3 maxScale = new Vector3(maxSize, maxSize, maxSize);

        do
        {
            transform.localScale = Vector3.Lerp(startScale, maxScale, timer / growTime);
            timer += Time.deltaTime;
            yield return null;
        }

        while (timer < growTime);

        isMaxSize = true;
    }

    private void MergeObjects()
    {
        int objectIndex = int.Parse(gameObject.tag);
        int scoreToAdd = objectIndex * 5 + 10;

        GameManager.newObjectPos = transform.position;
        GameManager.objectIndex = objectIndex;
        GameManager.isNewObjectSpawned = true;
        gameManager.UpdateScore(scoreToAdd);

        Destroy(gameObject);

        //GameManager.objectIndex = int.Parse(gameObject.tag);
        //add score
    }
}