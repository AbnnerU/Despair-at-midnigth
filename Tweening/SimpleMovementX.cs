using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SimpleMovementX : MonoBehaviour
{
    [SerializeField] private float endValue;

    [SerializeField] private float duration;

    public OnComplete onComplete;

    private Transform _transform;

    private void Awake()
    {
        _transform = GetComponent<Transform>();
    }

    public void MoveX()
    {
        _transform.DOLocalMoveX(endValue,duration).OnComplete(Completed);       
    }

    public void Completed()
    {
        onComplete?.Invoke();
    }


}
