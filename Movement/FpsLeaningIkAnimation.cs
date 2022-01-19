using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FpsLeaningIkAnimation : MonoBehaviour
{
 
    [SerializeField] private Animator anim;

    [SerializeField] private float disableAfterTime=1f;

    [Header("Verification")]

    [SerializeField] private RayCheck rayCheck;
    [SerializeField] private bool drawGizmos;
    [SerializeField] private RaycastType raycastType;
    [SerializeField] private Transform startPointRigth;
    [SerializeField] private Transform startPointLeft;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float length;

    [Header("Parallel Verification")]
    [SerializeField] private Transform pointRigth;
    [SerializeField] private Transform pointLeft;
    [SerializeField] private float verificationRadius;

    [Header("Leaning Right")]

    [SerializeField] private Vector3 spineRight;
    [SerializeField] private Vector3 chestRight;
    [SerializeField] private Vector3 upperChestRight;
    [SerializeField] private Vector3 neckRight;
    [SerializeField] private Vector3 headRight;

    [Header("Leaning Left")]
   
    [SerializeField] private Vector3 spineLeft;
    [SerializeField] private Vector3 chestLeft;
    [SerializeField] private Vector3 upperChestLeft;
    [SerializeField] private Vector3 neckLeft;
    [SerializeField] private Vector3 headLeft;

    //[SerializeField] private float rotationZ = 30f;

    [SerializeField] private float leaningTime = 0.15f;

    [SerializeField] private float backTime = 0.1f;

    [SerializeField] private float fastBackTime = 0.05f;


     private Vector3 spineRotation;
     private Vector3 chestRotation;
     private Vector3 upperChestRotation;
     private Vector3 neckRotation;
     private Vector3 headRotation;

    private State currentState;

    private InputController inputController;

    private Coroutine leaningCoroutine;
    private Coroutine disableCoroutine;

    private NewLeaning leaningValuesRight;
    private NewLeaning leaningValuesLeft;

    private RaycastHit[] hitResults;

    private bool executeLeaning = false;

    public enum State
    {
        NORMAL, LEANINGRIGHT, LEANINGLEFT
    }

    public enum RaycastType
    {
        RAYCAST,SPHERECAST
    }

    private void Awake()
    {

        hitResults = new RaycastHit[1];

        leaningValuesRight = new NewLeaning
        {

            spine = spineRight,
            chest = chestRight,
            upperChest = upperChestRight,
            neck = neckRight,
            head = headRight,
        };

        leaningValuesLeft = new NewLeaning
        {

            spine = spineLeft,
            chest = chestLeft,
            upperChest = upperChestLeft,
            neck = neckLeft,
            head = headLeft,
        };


        inputController = FindObjectOfType<InputController>();

        inputController.OnLeaningRightEvent += InputController_OnLeaningRigth;
        inputController.OnLeaningLeftEvent += InputController_OnLeaningLeft;


    }

    private void OnDisable()
    {
        spineRotation = Vector3.zero;
        chestRotation = Vector3.zero;
        upperChestRotation = Vector3.zero;
        neckRotation = Vector3.zero;
        headRotation = Vector3.zero;
    }

    private void Update()
    {
        if (currentState == State.NORMAL)
            return;

        if(currentState == State.LEANINGRIGHT)
        {
            if(Verify(pointRigth.position, pointRigth.right) == true)
            {
                print("PINTO DIREITA");
                if (leaningCoroutine != null)
                    StopCoroutine(leaningCoroutine);

                if (disableCoroutine != null)
                    StopCoroutine(disableCoroutine);

                StopAllCoroutines();

                SetDefaltPosition();
            }
        }
        else if(currentState == State.LEANINGLEFT)
        {
            if (Verify(pointLeft.position, -pointLeft.right) == true)
            {
                print("PINTO ESQUERDA");
                if (leaningCoroutine != null)
                    StopCoroutine(leaningCoroutine);

                if (disableCoroutine != null)
                    StopCoroutine(disableCoroutine);

                StopAllCoroutines();

                SetDefaltPosition();
            }
        }
    }

    private void InputController_OnLeaningRigth(float inputValue)
    {
        if (inputValue > 0)
        {
           
            if (leaningCoroutine != null)
                StopCoroutine(leaningCoroutine);

            if (disableCoroutine != null)
                StopCoroutine(disableCoroutine);

            if (Verify(startPointRigth.position,startPointRigth.right)==false)
            {
                executeLeaning = true;

                if (currentState != State.NORMAL)
                {

                    currentState = State.LEANINGRIGHT;
                    StartCoroutine(FastCentralize(leaningValuesRight));
                }
                else
                {
                    currentState = State.LEANINGRIGHT;
                    leaningCoroutine = StartCoroutine(Leaning(leaningValuesRight));
                }
            }

        }
        else
        {
            if (leaningCoroutine != null)
                StopCoroutine(leaningCoroutine);

            if (disableCoroutine != null)
                StopCoroutine(disableCoroutine);

            StopAllCoroutines();

            SetDefaltPosition();
        }
    }

    private void InputController_OnLeaningLeft(float inputValue)
    {
        if (inputValue > 0)
        {
            if (leaningCoroutine != null)
                StopCoroutine(leaningCoroutine);

            if (disableCoroutine != null)
                StopCoroutine(disableCoroutine);

          

            if (Verify(startPointLeft.position,-startPointLeft.right)==false)
            {
                executeLeaning = true;

                if (currentState != State.NORMAL)
                {
                    currentState = State.LEANINGLEFT;
                    StartCoroutine(FastCentralize(leaningValuesLeft));
                }
                else
                {

                    currentState = State.LEANINGLEFT;
                    leaningCoroutine = StartCoroutine(Leaning(leaningValuesLeft));
                }
            }

        }
        else
        {
            if (leaningCoroutine != null)
                StopCoroutine(leaningCoroutine);

            if (disableCoroutine != null)
                StopCoroutine(disableCoroutine);

            StopAllCoroutines();

            SetDefaltPosition();
        }
    }

    private void SetDefaltPosition()
    {
        leaningCoroutine = StartCoroutine(BackToCenter(backTime,true));
 
        disableCoroutine = StartCoroutine(SetDisabled());
    }

    private bool Verify(Vector3 startPoint, Vector3 direction)
    {
        if (raycastType == RaycastType.RAYCAST)
        {
            return rayCheck.RayCastCheck(startPoint, direction, length, groundLayer);
        }
        else
        {
            return rayCheck.SphereCheck(startPoint, length, groundLayer);
        }
    }

    
    IEnumerator Leaning(NewLeaning leaningValues)
    {
        float currentTime = 0;
        float value = 0;
        float addValue = 1 / leaningTime;

        do
        {
            currentTime += Time.deltaTime;
            value += addValue * Time.deltaTime;

            spineRotation = leaningValues.spine * value;
            chestRotation = leaningValues.chest * value;
            upperChestRotation = leaningValues.upperChest * value;
            neckRotation = leaningValues.neck * value;
            headRotation = leaningValues.head * value;

            yield return null;

        } while (currentTime < leaningTime);

        spineRotation = leaningValues.spine;
        chestRotation = leaningValues.chest;
        upperChestRotation = leaningValues.upperChest;
        neckRotation = leaningValues.neck;
        headRotation = leaningValues.head;

        
    }

    IEnumerator BackToCenter(float time, bool keepExecutingLeaning)
    {
        currentState = State.NORMAL;

        float currentTime = 0;
        float value = 0;
        float addValue = 1 / time;

        do
        {
            currentTime += Time.deltaTime;
            value += addValue * Time.deltaTime;

            spineRotation = spineRotation -(spineRotation * value);
            chestRotation = chestRotation-(chestRotation * value);
            upperChestRotation = upperChestRotation-(upperChestRotation * value);
            neckRotation = neckRotation- (neckRotation * value);
            headRotation =  headRotation-(headRotation* value);

            yield return null;

        } while (currentTime < leaningTime);

        spineRotation = Vector3.zero;
        chestRotation = Vector3.zero;
        upperChestRotation = Vector3.zero;
        neckRotation = Vector3.zero;
        headRotation = Vector3.zero;

        //executeLeaning = keepExecutingLeaning;
    }

    IEnumerator FastCentralize(NewLeaning leaningValues)
    {
        yield return StartCoroutine(BackToCenter(fastBackTime,true));

        leaningCoroutine = StartCoroutine(Leaning(leaningValues));
        
    }

    IEnumerator SetDisabled()
    {
        yield return new WaitForSeconds(disableAfterTime);
       
        executeLeaning = false;
    }

    private void OnAnimatorIK(int layerIndex)
    {
        if (executeLeaning)
        {           
            anim.SetBoneLocalRotation(HumanBodyBones.Spine, Quaternion.Euler(spineRotation));
            anim.SetBoneLocalRotation(HumanBodyBones.Chest, Quaternion.Euler(chestRotation));
            anim.SetBoneLocalRotation(HumanBodyBones.UpperChest, Quaternion.Euler(upperChestRotation));
            anim.SetBoneLocalRotation(HumanBodyBones.Neck, Quaternion.Euler(neckRotation));
            anim.SetBoneLocalRotation(HumanBodyBones.Head, Quaternion.Euler(headRotation));
            
        }
    }

    private void OnDrawGizmos()
    {
        if (drawGizmos)
        {
            if (raycastType == RaycastType.RAYCAST)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawLine(startPointRigth.position, startPointRigth.position + (startPointRigth.right * length));
                Gizmos.color = Color.red;
                Gizmos.DrawLine(startPointLeft.position, startPointLeft.position + (-startPointLeft.right * length));

                Gizmos.color = Color.yellow;
                Gizmos.DrawLine(pointRigth.position, pointRigth.position + (pointRigth.right * length));
                Gizmos.color = Color.yellow;
                Gizmos.DrawLine(pointLeft.position, pointLeft.position + (-pointLeft.right * length));
            }
            else
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawSphere(startPointRigth.position, length);
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(startPointLeft.position, length);

                Gizmos.color = Color.yellow;
                Gizmos.DrawSphere(pointRigth.position, verificationRadius);
                Gizmos.color = Color.yellow;
                Gizmos.DrawSphere(pointLeft.position, verificationRadius);
            }
        }
        
    }

}

public struct NewLeaning
{

     public Vector3 spine;
     public Vector3 chest;
     public Vector3 upperChest;
     public Vector3 neck;
     public Vector3 head;
}
