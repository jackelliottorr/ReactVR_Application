using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Assets._PROJECT.Scripts.HelperClasses;
using Newtonsoft.Json;
using ReactVR_API.Common.Models;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using VRUiKits.Utils;

public class MainMenuManager : MonoBehaviour
{
    private APIHelper _apiHelper = new APIHelper();

    // store found levels so that we can hit back and not have to hit the API again
    private List<Level> _levels = new List<Level>();
    private List<LevelConfigurationViewModel> _levelConfigurationViewModels = new List<LevelConfigurationViewModel>();

    private CardListManager cardListManager;

    [Header("User Defined")]
    [Tooltip("The GameObject containing the CardListManager script.")]
    [SerializeField] private GameObject LevelCardList;
    [Tooltip("The GameObject for the Leaderboard.")]
    [SerializeField] private GameObject VerticalCardList;
    [Tooltip("The GameObject to display when asynchronous tasks are processing.")]
    [SerializeField] private GameObject LoadingObject;

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

    private void PopulateCardList(List<Card> cards, CardListManager cardListManager)
    {
        cardListManager.Reset();
        cardListManager.cardList.Clear();

        cards.ForEach(c => cardListManager.cardList.Add(c));

        // PopulateList will draw the UI element in the cardlist.
        cardListManager.PopulateList();
    }

    public async void PopulateLevels()
    {
        List<Level> levels = await GetLevelsAsync();
        List<Card> cards = new List<Card>();

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

            cards.Add(newCard);
        }

        cardListManager = LevelCardList.GetComponent<CardListManager>();
        PopulateCardList(cards, cardListManager);

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
    private async void PopulateLevelConfigurations(Card card)
    {
        List<LevelConfigurationViewModel> levelConfigurations = await GetLevelConfigurationsAsync(card.Id);
        List<Card> cards = new List<Card>();

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

            cards.Add(newCard);
        }

        cardListManager = LevelCardList.GetComponent<CardListManager>();
        PopulateCardList(cards, cardListManager);

        foreach (CardItem item in cardListManager.cardItems)
        {
            item.OnCardClicked += LoadLevel;
        }

        //foreach (CardItem item in cardListManager.cardItems)
        //{
        //    item.OnCardClicked += PopulateScoreboard;
        //}
    }

    private async void PopulateScoreboard(Card card)
    {
        List<Scoreboard> scores = await GetScoresAsync(card.Id);
        List<Card> cards = new List<Card>();

        foreach (Scoreboard score in scores)
        {
            Card newCard = new Card()
            {
                Id = score.ScoreboardId,
                title = "Title",
                description = "Description",
                subtitle = "Subtitle"
            };

            cards.Add(newCard);
        }

        cardListManager = VerticalCardList.GetComponent<CardListManager>();
        PopulateCardList(cards, cardListManager);
    }

    private async Task<List<Level>> GetLevelsAsync()
    {
        try
        {
            TokenManager tokenManager = new TokenManager();
            var accessToken = tokenManager.RetrieveToken();

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);

                Task<HttpResponseMessage> levelsTask = httpClient.GetAsync(new Uri(_apiHelper.GetBaseUri() + "Level/GetAllLevels"));
                LoadingObject.SetActive(true);
                HttpResponseMessage jsonResponse = await levelsTask;

                List<Level> levels = new List<Level>();

                if (jsonResponse.IsSuccessStatusCode)
                {
                    Task<string> readAsStringTask = jsonResponse.Content.ReadAsStringAsync();
                    string jsonString = await readAsStringTask;

                    levels = JsonConvert.DeserializeObject<List<Level>>(jsonString);
                }

                return levels;
            }
        }
        catch (Exception ex)
        {
            var message = ex.Message;
            throw ex;
        }
    }

    
    private async void PopulateLevelCreationTab()
    {
        // get level configurations that user has created
        List<LevelConfigurationViewModel> levelConfigurations = new List<LevelConfigurationViewModel>();

        TokenManager tokenManager = new TokenManager();
        var accessToken = tokenManager.RetrieveToken();

        try
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
                
                Task<HttpResponseMessage> levelConfigurationsTask = httpClient.GetAsync(new Uri(_apiHelper.GetBaseUri() + "LevelConfiguration/GetLevelConfigurationsByCreatedById"));

                // show loading animation
                LoadingObject.SetActive(true);

                HttpResponseMessage jsonResponse = await levelConfigurationsTask;

                if (jsonResponse.IsSuccessStatusCode)
                {
                    var responseContent = await jsonResponse.Content.ReadAsStringAsync();
                    var jsonString = responseContent;

                    levelConfigurations = JsonConvert.DeserializeObject<List<LevelConfigurationViewModel>>(jsonString);
                }

                // hide loading animation
                LoadingObject.SetActive(false);

                //return levelConfigurations;
            }
        }
        catch (Exception ex)
        {
            var message = ex.Message;
            throw ex;
        }
    }


    /// <summary>
    /// Gets LevelConfigurations from the API, must supply levelId.
    /// </summary>
    /// <param name="levelId"></param>
    /// <returns></returns>
    private async Task<List<LevelConfigurationViewModel>> GetLevelConfigurationsAsync(Guid levelId)
    {
        List<LevelConfigurationViewModel> levelConfigurations = new List<LevelConfigurationViewModel>();
        
        TokenManager tokenManager = new TokenManager();
        var accessToken = tokenManager.RetrieveToken();

        try
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);

                Task<HttpResponseMessage> levelConfigurationsTask = httpClient.GetAsync(new Uri(_apiHelper.GetBaseUri() + $"LevelConfiguration/{levelId}")); 

                // show loading animation
                LoadingObject.SetActive(true);

                HttpResponseMessage jsonResponse = await levelConfigurationsTask;

                if (jsonResponse.IsSuccessStatusCode)
                {
                    var responseContent = await jsonResponse.Content.ReadAsStringAsync();
                    var jsonString = responseContent;

                    levelConfigurations = JsonConvert.DeserializeObject<List<LevelConfigurationViewModel>>(jsonString);
                }

                // hide loading animation
                LoadingObject.SetActive(false);

                return levelConfigurations;
            }
        }
        catch (Exception ex)
        {
            var message = ex.Message;
            throw ex;
        }
    }


    private async Task<List<Scoreboard>> GetScoresAsync(Guid levelConfigurationId)
    {
        List<Scoreboard> scores = new List<Scoreboard>();

        TokenManager tokenManager = new TokenManager();
        var accessToken = tokenManager.RetrieveToken();

        try
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);

                Task<HttpResponseMessage> levelConfigurationsTask = httpClient.GetAsync(new Uri(_apiHelper.GetBaseUri() + "Scoreboard/{levelConfigurationId}"));

                // show loading animation
                LoadingObject.SetActive(true);

                HttpResponseMessage jsonResponse = await levelConfigurationsTask;

                if (jsonResponse.IsSuccessStatusCode)
                {
                    var responseContent = await jsonResponse.Content.ReadAsStringAsync();
                    var jsonString = responseContent;

                    scores = JsonConvert.DeserializeObject<List<Scoreboard>>(jsonString);
                }

                // hide loading animation
                LoadingObject.SetActive(false);

                return scores;
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
            // for now, just run the default standard game mode

            if (levelConfigToLoad.Name == "Test Level")
            {
                SceneManager.LoadSceneAsync("Demo Scene", LoadSceneMode.Single);
            }
            else if (levelConfigToLoad.Name == "360 Test Level")
            {
                SceneManager.LoadSceneAsync("360 Demo Scene", LoadSceneMode.Single);
            }
        }
        catch (Exception exception)
        {
            throw exception;
        }
    }

 
    
    
    
    

    //// move all these methods to a seperate class probably for api calls
    //private List<Target> GetTargets(Guid levelConfigurationId)
    //{
    //    List<Target> targets = new List<Target>();

    //    return targets;
    //}

    //private TargetZone GetTargetZone(Guid levelConfigurationId)
    //{
    //    TargetZone targetZone = new TargetZone();

    //    return targetZone;
    //}
}

public enum MenuContext {
    Play = 0,
    Leadboard = 1
}
