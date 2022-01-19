
using UnityEngine;

public class EnableCollider : MonoBehaviour
{
    [SerializeField] private Collider col;

    public void DisableThisCollider(bool disable)
    {
        print(disable);
        col.enabled = !disable;
    }

    public void EnableThisCollider(bool enable)
    {
        col.enabled = enable;
    }
}
