using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable] public class AIInteractEvent<T> : UnityEvent<T> { }

[Serializable] public class AIInteractEvent : UnityEvent { }

public interface IAIInteractAction 
{
    void Interact();
}

