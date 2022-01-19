using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiInteractEvents : MonoBehaviour, IAIInteractAction
{
    public AIInteractEvent interactEvent;

    public void Interact()
    {
        interactEvent?.Invoke();
    }
}
