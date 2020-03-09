using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Assets._PROJECT.Scripts.HelperClasses;
using Newtonsoft.Json;
using ReactVR_API.Common.Models;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets._PROJECT.Scripts.UIManagers
{
    class SummaryManager : MonoBehaviour
    {
        private int _score;

        private TextMeshProUGUI _hitDisplay;
        private TextMeshProUGUI _missedDisplay;
        private TextMeshProUGUI _scoreDisplay;

        [Header("User defined")]
        [Tooltip("TextMeshPro UI that will show the number of targets hit.")]
        [SerializeField] private GameObject HitDisplay;

        [Tooltip("TextMeshPro UI that will show the number of targets missed.")]
        [SerializeField] private GameObject MissedDisplay;

        [Tooltip("TextMeshPro UI that will show the total score.")]
        [SerializeField] private GameObject ScoreDisplay;
        
        [Tooltip("Scene to change to when player hits 'Continue'.")]
        [SerializeField] private SceneAsset MenuScene;

        public void UpdateValues(int hitCount, int missCount, int score)
        {
            _score = score;

            _hitDisplay = HitDisplay.GetComponent<TextMeshProUGUI>();
            _missedDisplay = MissedDisplay.GetComponent<TextMeshProUGUI>();
            _scoreDisplay = ScoreDisplay.GetComponent<TextMeshProUGUI>();

            _hitDisplay.text = hitCount.ToString();
            _missedDisplay.text = missCount.ToString();
            _scoreDisplay.text = score.ToString();
        }

        public void PostScoreToAPI()
        {
            try
            {
                // Get access token to be sent with the request
                TokenManager tokenManager = new TokenManager();
                var accessToken = tokenManager.RetrieveToken();

                // Build the ScoreboardCreateModel
                ScoreboardCreateModel createModel = new ScoreboardCreateModel();
                //createModel.LevelConfigurationId = SessionData.LevelConfigurationViewModel.LevelConfigurationId;
                createModel.LevelConfigurationId = new Guid("5B6FA5B4-66BE-4137-8558-54C792AAF354");
                createModel.Score = _score;

                var jsonString = JsonConvert.SerializeObject(createModel);
                var content = new StringContent(jsonString, Encoding.UTF8, "application/json");

                // POST the model to the API
                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
                    var jsonResponse = httpClient.PostAsync(new Uri("http://localhost:7071/api/Scoreboard/CreateScoreboardEntry"), content).Result;

                    // if score successfully logged, 
                    if (jsonResponse.IsSuccessStatusCode)
                    {
                        SceneManager.LoadSceneAsync("Main Menu Scene", LoadSceneMode.Single);
                    }
                }
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                throw ex;
            }

        }
    }
}
