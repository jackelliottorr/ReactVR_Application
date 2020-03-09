using System.Collections;
using System.Collections.Generic;
using Assets._PROJECT.Scripts.HelperClasses;
using ReactVR_API.Common.Models;
using UnityEngine;

public class StandardGameModeManager : MonoBehaviour
{
    private bool _started;

    // score and timer tracking
    private int _hitCount = 0;
    private int _missCount = 0;
    private int _score = 0;
    private float _targetUptime = 0f;
    private float _timeLeft = 60.0f;

    private LevelConfigurationViewModel _levelConfigurationViewModel;

    [Header("Materials")]
    [SerializeField] private GameObject OVRPlayerController;

    [Header("Materials")]
    [SerializeField] private Material InactiveMaterial;
    [SerializeField] private Material ActiveMaterial;
    [SerializeField] private Material MissMaterial;

    // Start is called before the first frame update
    void Start()
    {
        // maybe put this in a countdown timer before level starts
        //_levelConfigurationViewModel = SessionData.LevelConfigurationViewModel;

        GameObject plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
        plane.transform.position = new Vector3(0, 0, 0);
        plane.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
        plane.transform.Rotate(Vector3.forward * 90);

        var targetGameObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);

        // set plane to parent, then center in the parent
        targetGameObject.transform.parent = plane.transform;
        targetGameObject.transform.localPosition = Vector3.zero;
        targetGameObject.transform.localPosition = new Vector3(1, 0, 1);
        targetGameObject.transform.localScale = new Vector3(1, 1, 1);

        Renderer renderer = targetGameObject.GetComponent<Renderer>();
        renderer.material = InactiveMaterial;
        targetGameObject.AddComponent<TargetScript>();
        targetGameObject.AddComponent<SphereCollider>();

        //foreach (Target target in _levelConfigurationViewModel.Targets)
        //{
        //    var targetGameObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);

        //    // set plane to parent, then center in the parent
        //    targetGameObject.transform.parent = plane.transform;
        //    targetGameObject.transform.localPosition = Vector3.zero;
        //    //targetGameObject.transform.localPosition = new Vector3(1, 0, 0);

        //    Renderer renderer = targetGameObject.GetComponent<Renderer>();
        //    renderer.material = InactiveMaterial;
        //    targetGameObject.AddComponent<TargetScript>();
        //    targetGameObject.AddComponent<SphereCollider>();
        //}
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
