using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetScript : MonoBehaviour
{
    private bool _active = false;
    private bool _hit = false;

    [Header("User Defined")]
    [Tooltip("Sound Effect to play on hit.")]
    [SerializeField] private AudioClip HitSoundEffect;

    [Tooltip("Sound Effect to play on activation.")]
    [SerializeField] private AudioClip ActivationSoundEffect;

    public bool Activated
    {
        get
        {
            return _active;
        }
        set
        {
            if (ActivationSoundEffect != null)
            {
                AudioSource.PlayClipAtPoint(ActivationSoundEffect, this.transform.position);
            }

            _active = value;
        }
    }

    public bool Hit
    {
        get => _hit;
        set => _hit = value;
    }

    public TargetScript()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_active)
        {
            var collisionObject = collision.gameObject.name;
            Debug.Log($"GameObject {collisionObject} collided with this Target.");

            OVRInput.Controller controller = OVRInput.Controller.None;

            if (collisionObject == "RightControllerAnchor")
            {
                controller = OVRInput.Controller.RTouch;
            }
            else if (collisionObject == "LeftControllerAnchor")
            {
                controller = OVRInput.Controller.LTouch;
            }

            if (controller != OVRInput.Controller.None)
            {
                StartCoroutine(ControllerCollision(controller));
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var collisionObject = collision.gameObject.name;
        Debug.Log($"GameObject {collisionObject} collided with this Target.");

        OVRInput.Controller controller = OVRInput.Controller.None;

        if (collisionObject == "RightControllerAnchor")
        {
            controller = OVRInput.Controller.RTouch;
        }
        else if (collisionObject == "LeftControllerAnchor")
        {
            controller = OVRInput.Controller.LTouch;
        }

        if (controller != OVRInput.Controller.None)
        {
            StartCoroutine(ControllerCollision(controller));
        }
    }

    private IEnumerator ControllerCollision(OVRInput.Controller controller)
    {
        if (HitSoundEffect != null)
        {
            AudioSource.PlayClipAtPoint(HitSoundEffect, this.transform.position);
        }

        _active = false;
        _hit = true;

        OVRInput.SetControllerVibration(1, 1f, controller);

        yield return new WaitForSeconds(0.2f);

        OVRInput.SetControllerVibration(0, 0, controller);
    }
}
