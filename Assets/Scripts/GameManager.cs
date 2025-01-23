using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameManager: MonoBehaviour
{
    private int fallenPins = 0;
    private static int totalFallenPins = 0;
    private bool ballThrown = false;
    private static int currentThrows = 0;
    private static int stageFallenPins = 0;
    private bool isPanelActive = false;
    
    public TMP_Text bestScoreText;
    public int totalPins = 10;

    public string pinTag = "Pin";
    public string ballTag = "Ball";
    public TMP_Text pinCountText;
    public TMP_Text totalPinCountText;

    public GameObject panel;
    public Button PlayAgainButton;
    public Button NextLevelButton;
    public Button MainMenuButton;
    public Button ExitButton;

    private HashSet<Collider> countedPins = new HashSet<Collider>();

    void Start()
    {
        AddButtonListeners();
        EnableButtons();
        UpdatePinCountText();
        Time.timeScale = 1;
    }
     void Update()
    {
        if (ballThrown && fallenPins == 0)
        {
            ShowPanel();
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(pinTag) && !countedPins.Contains(other))
        {
            fallenPins++;
            countedPins.Add(other);
            UpdatePinCountText();
        }

        if (other.CompareTag(ballTag))
        {
            ballThrown = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(ballTag))
        {
            if (fallenPins > 0)
            {
                stageFallenPins += fallenPins;
                totalFallenPins += fallenPins;
                fallenPins = 0;
                countedPins.Clear();
                UpdatePinCountText();
            }
        
             currentThrows++;
            if (currentThrows < 2)
            {
                Time.timeScale = 1;
                SceneManager.sceneLoaded += OnSceneLoaded;
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
            else
            {
                    Time.timeScale = 1;
                    ShowPanel();
            }
            ballThrown = false;
        }
    }
    

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        Time.timeScale = 1;
        EnableButtons();
        UpdatePinCountText();
        ResetFallenPins();
        Debug.Log("Current Throws: " + currentThrows);
    }
    void EnableButtons()
    {
        
        if (PlayAgainButton != null)
            PlayAgainButton.interactable = true;

        if (MainMenuButton != null)
            MainMenuButton.interactable = true;

        if (NextLevelButton != null)
            NextLevelButton.interactable = true;

        if (ExitButton != null)
            ExitButton.interactable = true;
    }

    void UpdatePinCountText()
    {
        if (pinCountText != null)
            pinCountText.text = "Pins fallen: " + fallenPins + " / " + totalPins;

        if (totalPinCountText != null)
            totalPinCountText.text = "Total Pins fallen: " + totalFallenPins;
    }

    void ShowPanel()
    {
        if (panel != null && !isPanelActive)
        {
        panel.SetActive(true); 
        isPanelActive = true;
        UpdatePinCountText();
        bestScoreText.text = "Best Score: " + totalFallenPins;
        }
        
    }   

    void ResetFallenPins()
    {
        fallenPins = 0;
        countedPins.Clear();
    }
    
    void ExitGame()
    {
        Debug.Log("Exit Game Button Clicked");
        Application.Quit();
    }

    void PlayAgain()
    {
        Debug.Log("Play Again Button Clicked");
        ResetGame();
        totalFallenPins = 0;
        SceneManager.LoadScene("Level1");
    }
    void GoToNextLevel()
    {
        Debug.Log("Next Level Button Clicked");
        SceneManager.LoadScene("Level2"); 
        ResetGame();
        currentThrows = 0;
        fallenPins = 0;
        string currentSceneName = SceneManager.GetActiveScene().name;
        Debug.Log("Current Scene: " + currentSceneName);

        if (currentSceneName == "Level1")
        {
            SceneManager.LoadScene("Level2");
        }
        else if (currentSceneName == "Level2")
        {
            Debug.Log("Last level reached");
        }
    }

    void GoToMenu()
    {
        Debug.Log("Menu Button Clicked");
        SceneManager.LoadScene("MainMenu"); 
    }
    
 
   
    void AddButtonListeners()
    {
        
        if (PlayAgainButton != null)
        {
            PlayAgainButton.onClick.AddListener(PlayAgain);
            Debug.Log("Play Again Button Listener Added");
        }
        else
        {
            Debug.LogError("Play Again Button is NULL!");
        }
        if (MainMenuButton != null)
        {
            MainMenuButton.onClick.AddListener(GoToMenu);
            Debug.Log("MainMenu Button Listener Added");
        }
        else
        {
            Debug.LogError("MainMenu Button is NULL!");
        }


        if (NextLevelButton != null)
        {
            NextLevelButton.onClick.AddListener(GoToNextLevel);
            Debug.Log("Next Level Button Listener Added");
        }
        else
        {
            Debug.LogError("Next Level Button is NULL!");
        }

        if (ExitButton != null)
        {
            ExitButton.onClick.AddListener(ExitGame);
            Debug.Log("Exit Button Listener Added");
        }
        else
        {
            Debug.LogError("Exit Button is NULL!");
        }
    }

    void ResetGame()
    {
        currentThrows = 0;
        countedPins.Clear();
        ballThrown = false;
        fallenPins = 0;
        stageFallenPins = 0;
    }
 }  
