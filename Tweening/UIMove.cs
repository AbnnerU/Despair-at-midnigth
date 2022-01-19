using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UIMove : MonoBehaviour
{
    [SerializeField] private Vector2 startPoint;
    [SerializeField] private Vector2 endPoint;
    [SerializeField] private float duration;

    [SerializeField] private bool disableObjectOnEnd=true;

    [SerializeField] private bool playOnStart = false;

    public OnCompleteTweening OnCompleteTweening;

    private RectTransform _transform;

    private Coroutine coroutine;

    private void Awake()
    {
        _transform = GetComponent<RectTransform>();
       
    }

    private void Start()
    {
        if (playOnStart)
        {
            StartMove();
        }
    }

    public void StartMove()
    {
        gameObject.SetActive(true);

        //if (coroutine != null)
        //    StopCoroutine(coroutine);

        //coroutine = StartCoroutine(Move());

        _transform.DOAnchorPosX(endPoint.x, duration);

        _transform.DOAnchorPosY(endPoint.y, duration).OnComplete(Completed);
    }

    public void Completed()
    {
        _transform.anchoredPosition = endPoint;

        if (disableObjectOnEnd)
            gameObject.SetActive(false);

        OnCompleteTweening?.Invoke();
    }

    //IEnumerator Move()
    //{
    //    Vector2 difference = endPoint - startPoint;

    //    Vector2 addValue = difference / duration;

    //    float currentTime = 0;


    //    _transform.anchoredPosition= startPoint;

    //    do
    //    {
    //        currentTime += Time.deltaTime;

    //        _transform.anchoredPosition += addValue * Time.deltaTime;

    //        yield return null;

    //    }while(currentTime < duration);

    //    _transform.anchoredPosition = endPoint;

    //    if (disableObjectOnEnd)
    //        gameObject.SetActive(false);

    //    yield break;

    //}
}
