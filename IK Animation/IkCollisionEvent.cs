
using UnityEngine;

public class IkCollisionEvent : MonoBehaviour
{
    private bool collision=false;
    private Vector3 point;
    

    private void OnTriggerStay(Collider other)
    {
        collision = true;
        
        point= other.ClosestPoint(transform.position);
    }

    private void OnTriggerExit(Collider other)
    {
        collision = false;
       
    }

    public bool GetCollision()
    {
        return collision;
    }

    public Vector3 GetPoint()
    {
        return point;
    }
}
