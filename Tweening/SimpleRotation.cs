using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SimpleRotation : MonoBehaviour
{   
    [SerializeField] private Vector3 targetRotation;

    [SerializeField] private float duration;

    [SerializeField] private bool local = false;

    public OnComplete onComplete;

    private Transform _transform;

    private void Awake()
    {
        _transform = GetComponent<Transform>();
    }

    public void Rotation()
    {
        if (local)
        {
            _transform.DOLocalRotate(targetRotation,duration).OnComplete(Completed);
        }
        else
        {
            _transform.DORotate(targetRotation, duration).OnComplete(Completed);
        }
        
    }

    public void Completed()
    {
        onComplete?.Invoke();
    }
}
