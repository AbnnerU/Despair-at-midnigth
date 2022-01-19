using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class OnAwakeAction : UnityEvent { }

public class OnAwakeEvents : MonoBehaviour
{
    public OnAwakeAction OnAwakeAction;

    private void Awake()
    {
        OnAwakeAction?.Invoke();
    }
}
