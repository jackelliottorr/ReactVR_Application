using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Assets._PROJECT.Scripts.HelperClasses;
using Newtonsoft.Json;
using ReactVR_API.Common.Models;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using VRUiKits.Utils;

public class MainMenuManager : MonoBehaviour
{
    // store found levels so that we can hit back and not have to hit the API again
    private List<Level> _levels = new List<Level>();
    private List<LevelConfigurationViewModel> _levelConfigurationViewModels = new List<LevelConfigurationViewModel>();

    private CardListManager cardListManager;

    [Header("User Defined")]
    [Tooltip("The GameObject containing the CardListManager script.")]
    [SerializeField] private GameObject LevelCardList;

    [Header("Demo Scenes")]
    [Tooltip("The Demo Scene.")]
    [SerializeField] private Scene DemoScene;
    [Tooltip("The 360 degree Demo Scene.")]
    [SerializeField] private Scene DemoScene360;

    void Start()
    {
    }

    public void PlayGame()
    {
        // loads next scene
        // this would actually be after choosing the level etc
        SceneManager.LoadScene("Game Scene");
    }

    public void PlayDemo()
    {
        if (SessionData.DemoMode)
        {
            SceneManager.LoadScene("Demo Scene");
        }
    }

    public void Play360Demo()
    {
        if (SessionData.DemoMode)
        {
            SceneManager.LoadScene("360 Demo Scene");
        }
    }

    public void QuitGame()
    {
        // need to use the oculus one instead that store requires
        Application.Quit();
    }

    public void GetAllLevels()
    {
        TokenManager tokenManager = new TokenManager();
        var accessToken = tokenManager.RetrieveToken();

        try
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
                var jsonResponse = httpClient.GetAsync(new Uri("http://localhost:7071/api/Level/GetAllLevels")).Result;

                if (jsonResponse.IsSuccessStatusCode)
                {
                    var responseContent = jsonResponse.Content.ReadAsStringAsync();
                    var jsonString = responseContent.Result;

                    List<Level> levels = JsonConvert.DeserializeObject<List<Level>>(jsonString);

                    PopulateLevels(levels);
                }
            }
        }
        catch (Exception ex)
        {
            var message = ex.Message;
            throw ex;
        }
    }

    private void PopulateLevels(List<Level> levels)
    {
        cardListManager = LevelCardList.GetComponent<CardListManager>();
        cardListManager.Reset();
        cardListManager.cardList.Clear();

        foreach (Level level in levels)
        {
            Card newCard = new Card()
            {
                Id = level.LevelId,
                title = level.Name,
                description = level.Description,
                subtitle = "Subtitle"
                //image = 
            };

            cardListManager.cardList.Add(newCard);
        }

        // PopulateList will draw the UI element in the cardlist.
        cardListManager.PopulateList();

        foreach (CardItem item in cardListManager.cardItems)
        {
            item.OnCardClicked += PopulateLevelConfigurations;
        }
    }

    /// <summary>
    /// When a Card is clicked, use it's LevelId to go to the API and retrieve the LevelConfigurations.
    /// Empty out the CardListManager now, and show the Configurations instead.
    /// Now, set the OnClicked event to load the level instead.
    /// </summary>
    /// <param name="card"></param>
    private void PopulateLevelConfigurations(Card card)
    {
        // Clear out the CardListManager's cards
        cardListManager = LevelCardList.GetComponent<CardListManager>();
        cardListManager.Reset();
        cardListManager.cardList.Clear();

        // Create Cards for LevelConfigurations retrieved from the API
        // keep in mind, we could create our own versions of the Card/CardItem classes, so that they have they the
        // level information as well. Instead of storing in this class and finding by Id (which we've added anyway).
        List<LevelConfigurationViewModel> levelConfigurations = GetLevelConfigurations(card.Id);
        foreach (LevelConfigurationViewModel levelConfiguration in levelConfigurations)
        {
            _levelConfigurationViewModels.Add(levelConfiguration);

            Card newCard = new Card()
            {
                Id = levelConfiguration.LevelConfigurationId,
                title = levelConfiguration.Name,
                description = levelConfiguration.Description,
                subtitle = "Subtitle"
                //image = 
            };

            cardListManager.cardList.Add(newCard);
        }

        // PopulateList will draw the UI element in the cardlist.
        cardListManager.PopulateList();

        foreach (CardItem item in cardListManager.cardItems)
        {
            item.OnCardClicked += LoadLevel;
        }
    }

    /// <summary>
    /// Gets LevelConfigurations from the API, must supply levelId.
    /// </summary>
    /// <param name="levelId"></param>
    /// <returns></returns>
    private List<LevelConfigurationViewModel> GetLevelConfigurations(Guid levelId)
    {
        List<LevelConfigurationViewModel> levelConfigurations = new List<LevelConfigurationViewModel>();
        
        TokenManager tokenManager = new TokenManager();
        var accessToken = tokenManager.RetrieveToken();

        try
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
                var jsonResponse = httpClient.GetAsync(new Uri($"http://localhost:7071/api/LevelConfiguration/{levelId}")).Result;

                if (jsonResponse.IsSuccessStatusCode)
                {
                    var responseContent = jsonResponse.Content.ReadAsStringAsync();
                    var jsonString = responseContent.Result;

                    levelConfigurations = JsonConvert.DeserializeObject<List<LevelConfigurationViewModel>>(jsonString);
                }

                return levelConfigurations;
            }
        }
        catch (Exception ex)
        {
            var message = ex.Message;
            throw ex;
        }
    }

    private void LoadLevel(Card card)
    {
        try
        {
            LevelConfigurationViewModel levelConfigToLoad = _levelConfigurationViewModels.FirstOrDefault(l => l.LevelConfigurationId == card.Id);
            SessionData.LevelConfigurationViewModel = levelConfigToLoad;

            // somehow pass the configuration into the next scene?
            SceneManager.LoadSceneAsync("Standard Game Mode", LoadSceneMode.Single);
        }
        catch (Exception exception)
        {
            throw exception;
        }
    }

    // move all these methods to a seperate class probably for api calls
    private List<Target> GetTargets(Guid levelConfigurationId)
    {
        List<Target> targets = new List<Target>();

        return targets;
    }

    private TargetZone GetTargetZone(Guid levelConfigurationId)
    {
        TargetZone targetZone = new TargetZone();

        return targetZone;
    }

}
