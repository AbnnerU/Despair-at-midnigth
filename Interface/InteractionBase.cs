using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]public class InteractAction<T> : UnityEvent<T> { }

[Serializable]public class InteractAction : UnityEvent { }

public class InteractionPopUp : MonoBehaviour
{
     public Action<IinteractiveTarget> OnNewInteractiveTarget;
}

public interface IinteractiveTarget
{
    string PopupText { get;}

    void OnLookingAtObject();

    void Action();

    void OnStopLook();
   
}

public interface IinteractiveData
{
    bool AlreadyInteracted();

    void SetData(bool value);
}

public enum InteractionConfigBool
{
    ONE_INTERACTION, UNLIMITED, UNLIMITED_INVERTBOOL_VALUE
}

public enum InteractionConfig
{
    ONE_INTERACTION, UNLIMITED
}