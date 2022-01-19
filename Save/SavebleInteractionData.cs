
using UnityEngine;

public class SavebleInteractionData : MonoBehaviour
{
    private IinteractiveData interactive=null;

    private string objName;

    private void Awake()
    {
        gameObject.TryGetComponent(out interactive);

        objName = gameObject.name;
    }

    public bool GetInteractiveState()
    {
        if (interactive != null)
            return interactive.AlreadyInteracted();
        else
            return false;
    }

    public string GetName()
    {
        return objName;
    }
}
