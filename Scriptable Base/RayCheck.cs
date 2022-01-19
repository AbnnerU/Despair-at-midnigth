
using UnityEngine;

[CreateAssetMenu(fileName = "RayCheck", menuName = "RayCastCheck")]
public class RayCheck : ScriptableObject
{
    public float defaltSphereRadius;
    public float defaltRaycastLength;
    private RaycastHit[] hitResults;

    public bool RayCastCheck(Vector3 position, Vector3 direction, float length, LayerMask layer)
    {
        if (length == 0)
            length = defaltRaycastLength;

        hitResults = new RaycastHit[1];

        if (Physics.RaycastNonAlloc(position,direction,hitResults,length,layer)>0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool RayCastCheck(out RaycastHit hitInfo,Vector3 position, Vector3 direction, float length, LayerMask layer)
    {
        if (length == 0)
            length = defaltRaycastLength;

        hitResults = new RaycastHit[1];

        if (Physics.RaycastNonAlloc(position, direction, hitResults, length, layer) > 0)
        {
            hitInfo = hitResults[0];
            return true;
        }
        else
        {
            hitInfo = hitResults[0];
            return false;
        }
    }

    public bool SphereCheck(Vector3 position, float radius, LayerMask layer)
    {
        if (radius == 0)
            radius = defaltSphereRadius;

        if (Physics.CheckSphere(position,radius,layer))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

}