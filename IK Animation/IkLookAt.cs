
using UnityEngine;

public class IkLookAt : MonoBehaviour
{
    [SerializeField] private Animator animator;

    [SerializeField] private Camera cam;

    [Header("Look At")]

    [SerializeField] private bool enableLookAt;

    [SerializeField] private float raycastLength=50f;

    [SerializeField] private float IkWeight=1f;

    [SerializeField] private float bodyWeight=0.1f;

    [SerializeField] private float headWeight=1f;

    private Vector3 positionLookAt;

    private RaycastHit[] lookAtHitResults;

    private void Awake()
    {
        lookAtHitResults = new RaycastHit[1];
    }

    private void FixedUpdate()
    {
        if (enableLookAt)
        {
          
            Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)); 
            
            positionLookAt = ray.GetPoint(raycastLength);
           
            Debug.DrawRay(ray.origin, ray.direction);
        }
    }

    public void OnAnimatorIK(int layerIndex)
    {
        animator.SetLookAtWeight(IkWeight,bodyWeight, headWeight);
        animator.SetLookAtPosition(positionLookAt);    
    }




}
