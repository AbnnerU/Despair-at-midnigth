using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IkHandOnWall : MonoBehaviour
{
    [SerializeField] private IkCollisionEvent IkEvent;
    [SerializeField] private Animator animator;

    [SerializeField] private AvatarIKGoal IKGoal;
    [SerializeField] private AvatarIKHint IKHint;

    [Header("Config")]
    [SerializeField] private float timeToHandOnWall;
    [SerializeField] private float timeToNormalPosition;
    [SerializeField] private Transform raycastStartPoint;
    [SerializeField] private float raycastLegth;
    [SerializeField] private LayerMask layerMask;

    [SerializeField] private float ajustDistance = 0.1f;

    [SerializeField] private float rotationX;
    [SerializeField] private float rotationZ;

    [Header("Normal: Positive Z")]

    [SerializeField] private RotationRelativeToNormal targetRotationZ;

    [Header("Normal: Negative Z")]

    [SerializeField] private RotationRelativeToNormal targetRotationNZ;

    [Header("Normal: Positive X")]

    [SerializeField] private RotationRelativeToNormal targetRotationX;

    [Header("Normal: Negative X")]

    [SerializeField] private RotationRelativeToNormal targetRotationNX;

    [SerializeField]private Vector3 handRotation;
    [SerializeField] private Vector3 elbowAjust;


 
    private RaycastHit[] hitResults;

    private Vector3 handPosition;
 
    private Vector3 elbowPosition;

    private Coroutine changeWeigthCoroutine;

    private float weigthValue;

    private bool handOnWall=false;

    private bool active = false;

    private bool ikEnabled = true;

    private void Awake()
    {
        hitResults = new RaycastHit[1];

    }

    private void FixedUpdate()
    {
        #region testes
        //Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

        //if (Physics.RaycastNonAlloc(ray, hitResults, raycastLength, layerMask) > 0)
        //{
        //    active = true;
        //    position = hitResults[0].point+(hitResults[0].normal * ajustDistance);
        //    position2 = hitResults[0].point + elbowAjust;
        //    //rotation = hitResults[0].normal*90;
        //}
        //else
        //{
        //    active = false;
        //}
        //print(active);

        //if (Physics.RaycastNonAlloc(teste.position,teste.forward,hitResults,distance,layerMask)>0)
        //{
        //    active = true;
        //    realPosition = hitResults[0].point;
        //    position = hitResults[0].point + (hitResults[0].normal * ajustDistance);
        //    position2 = hitResults[0].point + elbowAjust;
        //}
        //else
        //{
        //    active = false;
        //}

        //if (Physics.SphereCastNonAlloc(teste.position,radius,teste.forward,hitResults,distance,layerMask)>0)
        //{
        //    active = true;
        //    realPosition = hitResults[0].point;
        //    position = hitResults[0].point + (hitResults[0].normal * ajustDistance);

        //    verificar.position = position;

        //    position2 = hitResults[0].point + elbowAjust;
        //}
        //else
        //{
        //    active = false;
        //}

        #endregion
        if (ikEnabled == false)
            return;

        if (IkEvent.GetCollision())
        {
            active = true;
            Vector3 point = IkEvent.GetPoint();

            Vector3 start = raycastStartPoint.position;

            if (Physics.RaycastNonAlloc(start, point - start, hitResults, raycastLegth, layerMask) > 0)
            {

                if (hitResults[0].normal == Vector3.right || hitResults[0].normal == -Vector3.right || hitResults[0].normal == Vector3.forward || hitResults[0].normal == -Vector3.forward)
                {
                    active = true;

                    if (handOnWall == false)
                    {
                        handOnWall = true;

                        if (changeWeigthCoroutine != null)
                        {
                            StopCoroutine(changeWeigthCoroutine);
                        }

                        changeWeigthCoroutine = StartCoroutine(ChangeHandWeight(timeToHandOnWall, 1, true));

                    }

                   
                    handPosition = hitResults[0].point + (hitResults[0].normal * ajustDistance);

                    ChooseRotation(hitResults[0].normal);

                    elbowPosition = hitResults[0].point + elbowAjust;

                }
            }
            else
            {
                Debug.LogError("Ray can't touch on point");
            }

           
        }
        else
        {
            if (handOnWall==true)
            {
                handOnWall = false;

                if (changeWeigthCoroutine != null)
                {
                    StopCoroutine(changeWeigthCoroutine);
                }

                changeWeigthCoroutine = StartCoroutine(SetWeightToZero(timeToNormalPosition,false));

            }

          
        }
    }

    private void ChooseRotation(Vector3 normal)
    {
        Vector3 rotationOne=Vector3.zero;
        Vector3 rotationTwo=Vector3.zero;

        Vector3 ajustValueOne=Vector3.zero;
        Vector3 ajustValueTwo = Vector3.zero;

        if (normal.x != 0)
        {
            if (normal.x > 0)
            {
                rotationOne = targetRotationX.handRotation ;
                rotationOne.y *= Mathf.Abs(normal.x);

                ajustValueOne = targetRotationX.elbowPositionAjust * Mathf.Abs(normal.x);
            }
            else
            {
                rotationOne = targetRotationNX.handRotation;
                rotationOne.y *= Mathf.Abs(normal.x);

                ajustValueOne = targetRotationNX.elbowPositionAjust * Mathf.Abs(normal.x);
            }
        }

        if (normal.z != 0)
        {
            if (normal.z > 0)
            {
                rotationTwo = targetRotationZ.handRotation;
                rotationTwo.y *= Mathf.Abs(normal.z);

                ajustValueTwo = targetRotationZ.elbowPositionAjust * Mathf.Abs(normal.z);
            }
            else
            {
                rotationTwo = targetRotationNZ.handRotation;
                rotationTwo.y *= Mathf.Abs(normal.z);

                ajustValueTwo = targetRotationNZ.elbowPositionAjust * Mathf.Abs(normal.z);
            }
        }

        

        handRotation=  new Vector3(rotationX, rotationTwo.y + rotationOne.y,rotationZ);

        elbowAjust = ajustValueTwo + ajustValueOne;
    }

    public void DisableIk(bool disable)
    {
        ikEnabled = !disable;

        if (disable == true)
        {
            if (changeWeigthCoroutine != null)
            {
                StopCoroutine(changeWeigthCoroutine);
            }

            active = false;
        }
    }

    IEnumerator ChangeHandWeight(float time,float target,bool setActive)
    {
        float currentTime = 0;
        float value = 0;
        float addValue = 1 / time;

        do
        {
            currentTime += Time.deltaTime;
            value += addValue * Time.deltaTime;

            weigthValue = target * value;

            yield return null;

        } while (currentTime<time);

        weigthValue = target;
        active = setActive;
    }

    IEnumerator SetWeightToZero(float time, bool setActive)
    {
        float currentTime = 0;
        float value = 0;
        float addValue = 1 / time;

        do
        {
            currentTime += Time.deltaTime;
            value += addValue * Time.deltaTime;

            weigthValue -= weigthValue * value;

            yield return null;

        } while (currentTime < time);

        weigthValue = 0;
        active = setActive;
    }


    public void OnAnimatorIK(int layerIndex)
    {
        if (active)
        {
            animator.SetIKPositionWeight(IKGoal, weigthValue);
            animator.SetIKPosition(IKGoal ,handPosition);

            animator.SetIKRotationWeight(IKGoal, weigthValue);
            animator.SetIKRotation(IKGoal, Quaternion.Euler(handRotation));

            animator.SetIKHintPositionWeight(IKHint, weigthValue);
            animator.SetIKHintPosition(IKHint, elbowPosition);
        }
    }

    private void OnDrawGizmos()
    {
        if (active)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(handPosition, 0.1f);
            
        }

        if (raycastStartPoint != null)
        {      
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(raycastStartPoint.position,raycastStartPoint.position + (raycastStartPoint.forward * raycastLegth));
        }
    }
}

[System.Serializable]
public struct RotationRelativeToNormal
{
    public Vector3 handRotation;
    public Vector3 elbowPositionAjust;
}
