using System.Collections;
using System.Collections.Generic;
using Assets._PROJECT.Scripts.HelperClasses;
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
    private float _timeLeft = 60.0f;

    // fields to update the GUI so user can see time left
    private TextMeshProUGUI _timerDisplay;
    private TextMeshProUGUI _scoreDisplay;

    //private List<Target> Targets = new List<Target>();
    private List<GameObject> _targetGameObjects = new List<GameObject>();
    private GameObject _activeTarget = null;
    
    [Header("User defined")]
    [Tooltip("TextMeshPro UI that will show the player the time left.")]
    public GameObject TimerDisplay;

    [Tooltip("TextMeshPro UI that will show the player their score.")]
    public GameObject ScoreDisplay;

    public Material InactiveMaterial;
    public Material ActiveMaterial;
    public Material MissMaterial;

    void Start()
    {
        // Get timer text GameObject
        _timerDisplay = TimerDisplay.GetComponent<TextMeshProUGUI>();
        _scoreDisplay = ScoreDisplay.GetComponent<TextMeshProUGUI>();

        foreach (Transform child in transform)
        {
            if (child.tag == "Target")
            {
                _targetGameObjects.Add(child.gameObject);
            }

        }

        Debug.Log($"{_targetGameObjects.Count} targets found.");

        foreach (GameObject target in _targetGameObjects)
        {
            Renderer renderer = target.GetComponent<Renderer>();
            renderer.material = InactiveMaterial;
        }

        // Start the target stuff
        //StartCoroutine(RunGame());
    }

    // Update is called once per frame
    void Update()
    {
        _timeLeft -= Time.deltaTime;
        _timerDisplay.text = $"Time: {_timeLeft.ToString("0")}";

        if (_timeLeft <= 0)
        {
            SceneManager.LoadScene("Main Menu Scene");
        }

        // check if there is already an active target
        if (_activeTarget != null)
        {
            _targetUptime += Time.deltaTime;
            var activeTargetScript = _activeTarget.GetComponent<Target>();

            bool activeTargetFinished = false;

            // if it has been hit then update the counters and display, then continue
            if (activeTargetScript.Hit)
            {
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
                _activeTarget = null;
            }
        }
        else
        {
            // no active target, pick one
            var target = _targetGameObjects.PickRandom();
            var targetScript = target.GetComponent<Target>();

            // show the user that the target is active
            Renderer renderer = target.GetComponent<Renderer>();
            renderer.material = ActiveMaterial;

            targetScript.Activated = true;

            _targetUptime = 0;
            _activeTarget = target;
        }
    }    

    private void EndLevel()
    {
        SceneManager.LoadScene("Main Menu Scene");
    }

    private IEnumerator RunGame()
    {
        while (true)
        {        
            // randomly pick the next target
            var target = _targetGameObjects.PickRandom();
            var targetScript = target.GetComponent<Target>();

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
