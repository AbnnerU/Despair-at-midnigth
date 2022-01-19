using System;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class EventsAction: UnityEvent { }

public class TagDetectionEvent : MonoBehaviour
{
    [SerializeField] private string targetTag="NONE";

    public EventsAction OnEnter;

    public EventsAction OnExit;

    public Action<bool> OnObjectInside;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(targetTag))
        {
            OnEnter?.Invoke();

            OnObjectInside?.Invoke(true);       
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag(targetTag))
        {
            OnExit?.Invoke();

            OnObjectInside?.Invoke(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(targetTag))
        {
            OnEnter?.Invoke();

            OnObjectInside?.Invoke(true);         
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(targetTag))
        {
            OnExit?.Invoke();

            OnObjectInside?.Invoke(false);
        }
    }

}
