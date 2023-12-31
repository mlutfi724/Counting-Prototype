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
    [SerializeField] private TextMeshProUGUI gameplayHighScoreText;
    [SerializeField] private TextMeshProUGUI titleHighScoreText;
    [SerializeField] private GameObject titleScreen;
    [SerializeField] private GameObject pauseScreen;
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private GameObject gameplayScreen;

    private int score;
    private bool isPaused;

    private ObjectSpawnController objectSpawnController;
    private PlayfabManager playfabManager;

    public static bool isNewObjectSpawned;
    public static Vector3 newObjectPos;
    public static int objectIndex;
    public static bool isGameActive;

    // Start is called before the first frame update
    private void Start()
    {
        objectSpawnController = FindObjectOfType<ObjectSpawnController>();
        playfabManager = FindObjectOfType<PlayfabManager>();
        isGameActive = false;
        UpdateHighScore();
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
            int scoreToAdd = (objectIndex * 5 + 10) * 2;
            UpdateScore(scoreToAdd);
            isNewObjectSpawned = false;
        }
    }

    public void StartGame()
    {
        isGameActive = true;
        objectSpawnController.SpawnObject();
        gameplayScreen.SetActive(true);
        titleScreen.SetActive(false);

        score = 0;
        UpdateScore(0);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        playfabManager.GetLeaderboard();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1f;
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void GameOver()
    {
        playfabManager.SendLeaderboard(score);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;

        isGameActive = false;
        gameOverScreen.SetActive(true);
    }

    public void PauseGame()
    {
        if (!isPaused)
        {
            pauseScreen.gameObject.SetActive(true);
            isPaused = true;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
            Time.timeScale = 0f;
        }
        else
        {
            pauseScreen.gameObject.SetActive(false);
            isPaused = false;
            Time.timeScale = 1f;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    private void UpdateScore(int scoreToAdd)
    {
        score += scoreToAdd;
        scoreText.text = "Score: " + score;
        CheckHighScore();
    }

    private void CheckHighScore()
    {
        if (score > PlayerPrefs.GetInt("HighScore", 0))
        {
            PlayerPrefs.SetInt("HighScore", score);
            UpdateHighScore();
        }
    }

    private void UpdateHighScore()
    {
        gameplayHighScoreText.text = "High Score: " + PlayerPrefs.GetInt("HighScore");
        titleHighScoreText.text = "High Score: " + PlayerPrefs.GetInt("HighScore");
    }

    public void ResetGameData()
    {
        PlayerPrefs.DeleteAll();
    }
}