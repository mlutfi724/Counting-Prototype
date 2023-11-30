using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine.UI;

public class PlayfabManager : MonoBehaviour
{
    [SerializeField] private GameObject usernameWindow;
    [SerializeField] private TMP_InputField nameInput;
    [SerializeField] private GameObject playAgainWindow;

    [SerializeField] private GameObject scoreRowPrefab;
    [SerializeField] private Transform rowsParent;
    [SerializeField] private TextMeshProUGUI notificationText;

    [SerializeField] private Button startButton;

    private void Start()
    {
        Login();
    }

    private void Login()
    {
        notificationText.gameObject.SetActive(true);
        notificationText.text = "Loading...";
        var request = new LoginWithCustomIDRequest
        {
            CustomId = SystemInfo.deviceUniqueIdentifier,
            CreateAccount = true,
            InfoRequestParameters = new GetPlayerCombinedInfoRequestParams
            {
                GetPlayerProfile = true,
            }
        };

        PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnError);
    }

    private void OnLoginSuccess(LoginResult result)
    {
        startButton.gameObject.SetActive(true);
        notificationText.gameObject.SetActive(false);

        Debug.Log("Successful login/account create!");
        string name = null;
        if (result.InfoResultPayload.PlayerProfile != null)
        {
            name = result.InfoResultPayload.PlayerProfile.DisplayName;
        }
        if (!GameManager.isGameActive && name == null)
        {
            usernameWindow.SetActive(true);
            playAgainWindow.SetActive(false);
        }
        else if (!GameManager.isGameActive)
        {
            usernameWindow.SetActive(false);
            playAgainWindow.SetActive(true);
        }
    }

    public void SubmitNameButton()
    {
        if (nameInput.text.Length > 16)
        {
            notificationText.gameObject.SetActive(true);
            notificationText.text = "Username is too long!";
        }
        else if (nameInput.text.Length < 3)
        {
            notificationText.gameObject.SetActive(true);
            notificationText.text = "The username needs at least 3 characters!";
        }
        else
        {
            var request = new UpdateUserTitleDisplayNameRequest
            {
                DisplayName = nameInput.text,
            };
            PlayFabClientAPI.UpdateUserTitleDisplayName(request, OnDisplayNameUpdate, OnError);
            notificationText.text = "Username submitted!";
            notificationText.gameObject.SetActive(true);
            usernameWindow.SetActive(false);
            playAgainWindow.SetActive(true);
        }
    }

    private void OnDisplayNameUpdate(UpdateUserTitleDisplayNameResult result)
    {
        Debug.Log("Updated display name!");
    }

    private void OnError(PlayFabError error)
    {
        //Debug.Log("Error while logging in/creating account!");
        Debug.Log(error.GenerateErrorReport());
    }

    // Leaderboard System
    public void SendLeaderboard(int score)
    {
        var request = new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate>
            {
                new StatisticUpdate
                {
                    StatisticName = "HighScore", Value = score
                }
            }
        };
        PlayFabClientAPI.UpdatePlayerStatistics(request, OnLeaderboardUpdate, OnError);
    }

    private void OnLeaderboardUpdate(UpdatePlayerStatisticsResult result)
    {
        Debug.Log("Successfull leaderboard sent!");
    }

    public void GetLeaderboard()
    {
        var request = new GetLeaderboardRequest
        {
            StatisticName = "HighScore",
            StartPosition = 0,
            MaxResultsCount = 5,
        };
        PlayFabClientAPI.GetLeaderboard(request, OnLeaderboardGet, OnError);
    }

    private void OnLeaderboardGet(GetLeaderboardResult result)
    {
        foreach (var item in result.Leaderboard)
        {
            GameObject newScoreRow = Instantiate(scoreRowPrefab, rowsParent);
            TextMeshProUGUI[] texts = newScoreRow.GetComponentsInChildren<TextMeshProUGUI>();
            texts[0].text = (item.Position + 1).ToString();
            texts[1].text = item.DisplayName;
            texts[2].text = item.StatValue.ToString();

            Debug.Log(item.Position + " " + item.DisplayName + " " + item.StatValue);
        }
    }
}