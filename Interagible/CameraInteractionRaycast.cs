
using UnityEngine;

public class CameraInteractionRaycast : InteractionPopUp
{
    [SerializeField] private Camera cam;

    [Header("Raycast")]
    [SerializeField] private bool drawGizmos;
    [SerializeField] private float raycastLength;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private int maxAllocation=2;

    private InputController inputController;

    private IinteractiveTarget currentObject;

    private RaycastHit[] hitResults;

    private void Awake()
    {
        inputController = FindObjectOfType<InputController>();

        inputController.OnInteract += InputController_OnInteract;

        hitResults = new RaycastHit[maxAllocation];
    }

    private void Update()
    {
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

        int amount = Physics.SphereCastNonAlloc(ray, 0.2f, hitResults, raycastLength, layerMask);

        IinteractiveTarget interagible=null;

        if (amount > 0)
        {
            for (int i = 0; i < amount; i++)
            {
                interagible = hitResults[i].transform.gameObject.GetComponent<IinteractiveTarget>();

                if (interagible != null)
                {
                    if (currentObject == null)
                    {
                        currentObject = interagible;

                        currentObject.OnLookingAtObject();

                        //Event
                        OnNewInteractiveTarget?.Invoke(currentObject);
                    }
                    else
                    {
                        if (currentObject == interagible)
                        {
                            //currentObject.OnLookingAtObject();

                            //Event
                            OnNewInteractiveTarget?.Invoke(currentObject);

                            return;
                        }
                        else
                        {
                            currentObject.OnStopLook();

                            currentObject = null;

                            currentObject = interagible;

                            currentObject.OnLookingAtObject();

                            //Event
                            OnNewInteractiveTarget?.Invoke(currentObject);
                        }
                    }

                    return;
                }
               
            }

            if (currentObject != null)
            {
                currentObject.OnStopLook();
                currentObject = null;

                //Event
                OnNewInteractiveTarget?.Invoke(currentObject);
            }

        }
        else
        {
            if (currentObject != null)
            {
                currentObject.OnStopLook();
                currentObject = null;

                //Event
                OnNewInteractiveTarget?.Invoke(currentObject);
            }
        }
    }


    private void InputController_OnInteract()
    {
        if (currentObject != null)
        {          
            currentObject.Action();
        }
    }

    private void OnDrawGizmos()
    {
        if (drawGizmos)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)).origin, cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)).origin + (cam.transform.forward * raycastLength));
        }
    }
}
