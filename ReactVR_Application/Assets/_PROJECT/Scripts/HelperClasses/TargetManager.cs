using System.Collections;
using System.Collections.Generic;
using Assets._PROJECT.Scripts.HelperClasses;
using Assets._PROJECT.Scripts.UIManagers;
using ReactVR_API.Common.Models;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TargetManager : MonoBehaviour
{
    // score and timer tracking
    private int _hitCount = 0;
    private int _missCount = 0;
    private int _score = 0;
    private float _targetUptime = 0f;
    private float _timeLeft = 10.0f;

    // fields to update the GUI so user can see time left
    private TextMeshProUGUI _timerDisplay;
    private TextMeshProUGUI _scoreDisplay;

    //private List<Target> Targets = new List<Target>();
    private List<TargetAppearance> _targetAppearances = new List<TargetAppearance>();
    private List<GameObject> _targetGameObjects = new List<GameObject>();
    private GameObject _activeTarget = null;
    
    [Header("GUI Display")]
    [Tooltip("GameObject with TextMeshProUGUI that will show the player the time left.")]
    [SerializeField] private GameObject TimerDisplay;
    [Tooltip("GameObject with TextMeshProUGUI that will show the player their score.")]
    [SerializeField] private GameObject ScoreDisplay;
    [Tooltip("Canvas that will be activated and shown to the user when the level ends.")]
    [SerializeField] private GameObject SummaryCanvas;
    [Tooltip("Particle System to emit from the targets.")]
    [SerializeField] private ParticleSystem TargetParticleSystem;
    [Tooltip("Burst to emit from the targets.")]
    [SerializeField] private ParticleSystem TargetParticleSystemBurst;

    [Header("Materials")]
    [SerializeField] private Material InactiveMaterial;
    [SerializeField] private Material ActiveMaterial;
    [SerializeField] private Material MissMaterial;

    void Start()
    {
        // Turn off renderer for play surface
        gameObject.GetComponent<MeshRenderer>().enabled = false;

        // Get timer text GameObject
        _timerDisplay = TimerDisplay.GetComponent<TextMeshProUGUI>();
        _scoreDisplay = ScoreDisplay.GetComponent<TextMeshProUGUI>();

        foreach (Transform child in transform)
        {
            if (child.tag == "Target")
            {
                _targetGameObjects.Add(child.gameObject);
                //child.gameObject.active = false;
            }

        }

        Debug.Log($"{_targetGameObjects.Count} targets found.");

        //foreach (GameObject target in _targetGameObjects)
        //{
        //    Renderer renderer = target.GetComponent<Renderer>();
        //    renderer.material = InactiveMaterial;
        //}
    }

    // Update is called once per frame
    void Update()
    {
        _timeLeft -= Time.deltaTime;
        _timerDisplay.text = $"Time: {_timeLeft.ToString("0")}";

        if (_timeLeft <= 0)
        {
            EndLevel();
        }

        // check if there is already an active target
        if (_activeTarget != null)
        {
            _targetUptime += Time.deltaTime;
            var activeTargetScript = _activeTarget.GetComponent<TargetScript>();

            bool activeTargetFinished = false;

            // if it has been hit then update the counters and display, then continue
            if (activeTargetScript.Hit)
            {
                TargetParticleSystemBurst.transform.position = _activeTarget.transform.position;
                TargetParticleSystemBurst.gameObject.SetActive(true);
                TargetParticleSystemBurst.Play();

                _hitCount++;
                _score++;
                _scoreDisplay.text = $"Score: {_score}";
                activeTargetFinished = true;
            }
            else if (!activeTargetScript.Hit)
            {
                // if targetuptime has hit 3 seconds, it has been missed
                if (_targetUptime >= 3f)
                {
                    _missCount++;
                    activeTargetFinished = true;
                }
                else
                {
                    // Still time left to hit this target
                    return;
                }
            }

            if (activeTargetFinished)
            {
                Renderer renderer = _activeTarget.GetComponent<Renderer>();
                renderer.material = InactiveMaterial;

                activeTargetScript.Hit = false;
                activeTargetScript.Activated = false;

                _targetUptime = 0;
                //_activeTarget.active = false;
                _activeTarget = null;

                // Stop & Hide the ParticleSystem at the position of the Target
                //TargetParticleSystem.gameObject.SetActive(false);
                //TargetParticleSystem.Stop();
            }
        }
        else
        {
            // no active target, pick one
            var target = _targetGameObjects.PickRandom();
            var targetScript = target.GetComponent<TargetScript>();

            // show the user that the target is active
            Renderer renderer = target.GetComponent<Renderer>();
            renderer.material = ActiveMaterial;

            targetScript.Activated = true;

            _targetUptime = 0;
            _activeTarget = target;
            //_activeTarget.active = true;

            // Play the ParticleSystem at the position of the Target
            TargetParticleSystem.transform.position = target.transform.position;
            TargetParticleSystem.gameObject.SetActive(true);
            TargetParticleSystem.Play();
        }
    }    

    private void EndLevel()
    {
        TargetParticleSystem.Stop();
        TargetParticleSystem.gameObject.SetActive(false);

        SummaryManager summaryManager = SummaryCanvas.GetComponent<SummaryManager>();
        summaryManager.UpdateValues(_hitCount, _missCount, _score);

        SummaryCanvas.SetActive(true);

        // Disable this gameObject
        gameObject.SetActive(false);
    }

    private IEnumerator RunGame()
    {
        while (true)
        {        
            // randomly pick the next target
            var target = _targetGameObjects.PickRandom();
            var targetScript = target.GetComponent<TargetScript>();

            // show the user that the target is active
            Renderer renderer = target.GetComponent<Renderer>();
            renderer.material = ActiveMaterial;

            targetScript.Activated = true;

            // give the user 3 seconds to hit the target
            yield return new WaitForSeconds(3);

            // show player they missed after time is up
            renderer.material = MissMaterial;
            targetScript.Activated = false;

            yield return new WaitForSeconds(1);

            // set the target back to it's inactive state
            renderer.material = InactiveMaterial;
        }
    }
}
