using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SocialPlatforms.Impl;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject[] allObjectPrefabs;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private GameObject titleScreen;
    [SerializeField] private GameObject pauseScreen;
    [SerializeField] private GameObject gameOverScreen;

    private int score;
    private bool isPaused;
    private ObjectSpawnController objectSpawnController;

    public static bool isNewObjectSpawned;
    public static Vector3 newObjectPos;
    public static int objectIndex;
    public bool isGameActive;

    // Start is called before the first frame update
    private void Start()
    {
        objectSpawnController = FindObjectOfType<ObjectSpawnController>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && isGameActive)
        {
            PauseGame();
        }

        ReplaceObject();
    }

    private void ReplaceObject()
    {
        if (isNewObjectSpawned)
        {
            Instantiate(allObjectPrefabs[objectIndex + 1], newObjectPos, allObjectPrefabs[objectIndex + 1].transform.rotation);
            isNewObjectSpawned = false;
        }
    }

    public void StartGame()
    {
        isGameActive = true;
        objectSpawnController.SpawnObject();
        scoreText.gameObject.SetActive(true);
        titleScreen.SetActive(false);

        score = 0;
        UpdateScore(0);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GameOver()
    {
        Cursor.visible = true;

        isGameActive = false;
        gameOverScreen.SetActive(true);
        scoreText.transform.position = new Vector3(563, 318, 0);
    }

    private void PauseGame()
    {
        if (!isPaused)
        {
            pauseScreen.gameObject.SetActive(true);
            isPaused = true;
            Time.timeScale = 0f;
        }
        else
        {
            pauseScreen.gameObject.SetActive(false);
            isPaused = false;
            Time.timeScale = 1f;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Confined;
        }
    }

    public void UpdateScore(int scoreToAdd)
    {
        score += scoreToAdd;
        scoreText.text = "Score: " + score;
    }
}