using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SimpleMovementY : MonoBehaviour
{
    [SerializeField] private float endValue;

    [SerializeField] private float duration;

    [SerializeField] private Transform _transform;

    public OnComplete onStartTweening;

    public OnComplete onComplete;

   

    private void Awake()
    {
        if(_transform==null)
        _transform = GetComponent<Transform>();
    }

    public void MoveY()
    {
        onStartTweening?.Invoke();
        _transform.DOLocalMoveY(endValue,duration).OnComplete(Completed); 
    }

    public void MoveYWhitDelay(float delay)
    {
        StartCoroutine(Delay(delay));
    }

    IEnumerator Delay(float delayValue)
    {
        yield return new WaitForSeconds(delayValue);

        MoveY();

        yield break;
    }

    public void Completed()
    {
        onComplete?.Invoke();
    }

}
