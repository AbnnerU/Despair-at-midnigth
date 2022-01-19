
using UnityEngine;

public class TargetSeachPoint : MonoBehaviour
{
    [SerializeField] private Transform point;

    public Transform GetPoint()
    {
        return point;
    }
}
