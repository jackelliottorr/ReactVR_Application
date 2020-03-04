using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetBehaviour : MonoBehaviour
{
    private Collider _collider;
    private Collider _controllerCollider;

    // Start is called before the first frame update
    void Start()
    {
        _collider = this.GetComponent<MeshCollider>();
    }

    // Update is called once per frame
    void Update()
    {
       // OVRInput.SetControllerVibration(1, 1);
    }
}
