using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using static UnityEngine.GraphicsBuffer;

public class ObjectController : MonoBehaviour
{
    // [SerializeField] private ParticleSystem smokeParticle;

    [SerializeField] private AudioClip collideGlassSFX;
    [SerializeField] private AudioClip collideObjectSFX;
    [SerializeField] private AudioClip[] mergeSFX;

    private bool isDropped;
    private bool isFalling;
    private new Transform camera;

    private Rigidbody objectRb;

    //script references
    private ObjectSpawnController spawnController;

    private GameManager gameManager;
    private SlimeController slimeController;

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
        slimeController = GetComponent<SlimeController>();
        camera = Camera.main.transform;

        isDropped = false;
        if (transform.position.y < 3f)
        {
            objectRb.useGravity = true;
            isFalling = true;
            isDropped = true;
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
            LookCamera();
            slimeController.SetSlimeFaceState(NewSlimeFaceState.NoMouthFace);
        }

        if (Input.GetKeyDown(KeyCode.Space) && GameManager.isGameActive && !isDropped)
        {
            StartCoroutine(DroppingObject());
            objectRb.useGravity = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        isFalling = true; // to prevent merged object stuck in the spawnPoint

        // calculate the volume based on Impact
        float sfxVolume = 0.1f * collision.relativeVelocity.magnitude;
        // Debug.Log("Impact: " + collision.relativeVelocity.magnitude);
        if (collision.gameObject.tag == gameObject.tag) // checking if this object collides with the same tag
        {
            SoundFXManager.instance.PlayRandomSoundFXClip(mergeSFX, transform, 1f);
            MergeObjects();
        }
        else if (collision.gameObject.CompareTag("glass"))
        {
            SoundFXManager.instance.PlaySoundFXClip(collideGlassSFX, transform, sfxVolume);
        }
        else
        {
            SoundFXManager.instance.PlaySoundFXClip(collideObjectSFX, transform, sfxVolume);
        }

        if (collision.relativeVelocity.magnitude > 1f)
        {
            StartCoroutine(SlimeCollideFaceStateChange());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isDropped && other.gameObject.CompareTag("limit"))
        {
            gameManager.GameOver();
        }
    }

    private IEnumerator SlimeCollideFaceStateChange()
    {
        slimeController.SetSlimeFaceState(NewSlimeFaceState.CrossedEyesFace);
        yield return new WaitForSeconds(1f);
        slimeController.SetSlimeFaceState(NewSlimeFaceState.CuteFace);
    }

    // Give a time to check game over condition
    private IEnumerator DroppingObject()
    {
        isFalling = true;
        isDropped = false;
        slimeController.SetSlimeFaceState(NewSlimeFaceState.AngryFace);
        yield return new WaitForSeconds(spawnController.objectFallDuration);
        slimeController.SetSlimeFaceState(NewSlimeFaceState.CuteFace);
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

    private void LookCamera()
    {
        //Vector3 camPos = ;
        //camPos.x = 0f;
        //camPos.z = 0f;
        //transform.LookAt(camera);

        var newRotation = Quaternion.LookRotation(camera.position - transform.position);
        newRotation.z = 0.0f;
        newRotation.x = 0.0f;
        transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * 8);
    }

    private void MergeObjects()
    {
        int objectIndex = int.Parse(gameObject.tag);
        GameManager.newObjectPos = transform.position;
        GameManager.objectIndex = objectIndex;
        GameManager.isNewObjectSpawned = true;
        Destroy(gameObject);
    }
}