
using UnityEngine;

public class SavebleObject : MonoBehaviour
{
    private Transform _transform;
    private Collider objCollider=null;
    private string objName;

    private void Awake()
    {
        _transform = GetComponent<Transform>();

        objName = gameObject.name;

        gameObject.TryGetComponent<Collider>(out objCollider);
       
    }

    public bool ColliderIsActive()
    {
        if(objCollider != null)
        {
            return objCollider.enabled;
        }
        else
        {
            return false;
        }
    }

    public bool ColliderIsTrigger()
    {
        if (objCollider != null)
        {
            return objCollider.isTrigger;
        }
        else
        {
            return false;
        }
    }

    public Collider GetCollider()
    {
        return objCollider;
    }

    public bool IsEnabled()
    {
        return gameObject.activeSelf;
    }

    public string GetName()
    {
        return objName;
    }

    public Vector3 GetPosition()
    {
        if (_transform)
            return _transform.position;
        else
            return transform.position;
    }

    public Vector3 GetRotation()
    {
        if (_transform)
            return _transform.localEulerAngles;
        else
            return transform.localEulerAngles;
    }

}


