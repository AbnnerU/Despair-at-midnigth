using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HidingSpot : MonoBehaviour
{
    [SerializeField] private TagDetectionEvent tagDetection;

    [SerializeField] private Transform aiPositionReference;

    private bool playerHiding = false;

    private void Awake()
    {
        tagDetection.OnObjectInside += TagDetectionEvent_OnObjectInside;
    }

    private void TagDetectionEvent_OnObjectInside(bool value)
    {
        playerHiding = value;
    }

    public Transform GetAiPositionReference()
    {
        return aiPositionReference;
    }

    public void DestroySpot()
    {
        Destroy(gameObject);
    }

    public bool PlayerHidingOnSopt()
    {
        return playerHiding;
    }

    
}
