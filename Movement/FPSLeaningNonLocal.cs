using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSLeaningNonLocal : MonoBehaviour
{
    [SerializeField] private CinemachineRecomposer cinemachineRecomposer;

    [SerializeField] private Transform cinemachineTarget;

    [SerializeField] private Transform defaltPosition;

    [SerializeField] private Transform rigthLeaningPosition;

    [SerializeField] private Transform leftLeaningPosition;

    [SerializeField] private float rotationZ = 30f;

    [SerializeField] private float leaningTime = 0.15f;

    [SerializeField] private float backTime = 0.1f;

    [SerializeField] private float fastBackTime = 0.05f;

    private State currentState;
    /*[SerializeField] */

    //[SerializeField] private AnimationCurve leaningCurve;

    private Vector3 defaltCinemachineTargetPosition;

    private InputController inputController;

    private Coroutine leaningCoroutine;

    //Using animation curve
    //private Vector3 positionDifferenceRight;
    //private Vector3 positionDifferenceLeft;

    //private float animationTime;
    //private float currentTime;
    public enum State
    {
        NORMAL, LEANINGRIGHT, LEANINGLEFT
    }

    private void Awake()
    {
        inputController = FindObjectOfType<InputController>();


        //defaltCinemachineTargetPosition = cinemachineTarget.localPosition;

        //positionDifferenceRight = defaltCinemachineTargetPosition - rigthLeaningPosition.localPosition;
        //positionDifferenceLeft = defaltCinemachineTargetPosition - leftLeaningPosition.localPosition;



        //animationTime = leaningCurve.keys[leaningCurve.length - 1].time;

        inputController.OnLeaningRightEvent += InputController_OnLeaningRigth;
        inputController.OnLeaningLeftEvent += InputController_OnLeaningLeft;

      
    }


    private void InputController_OnLeaningRigth(float inputValue)
    {
        if (inputValue > 0)
        {
            if (leaningCoroutine != null)
                StopCoroutine(leaningCoroutine);


            if (currentState != State.NORMAL)
            {
                StopCoroutine(FastCentralize());
                currentState = State.LEANINGRIGHT;
                StartCoroutine(FastCentralize());
            }
            else
            {
                currentState = State.LEANINGRIGHT;
                leaningCoroutine = StartCoroutine(Leaning(-rotationZ, rigthLeaningPosition));
            }

        }
        else
        {
            if (leaningCoroutine != null)
                StopCoroutine(leaningCoroutine);

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



            if (currentState != State.NORMAL)
            {
                StopCoroutine(FastCentralize());
                currentState = State.LEANINGLEFT;
                StartCoroutine(FastCentralize());
            }
            else
            {
                currentState = State.LEANINGLEFT;
                leaningCoroutine = StartCoroutine(Leaning(rotationZ, leftLeaningPosition));
            }

        }
        else
        {
            if (leaningCoroutine != null)
                StopCoroutine(leaningCoroutine);

            StopAllCoroutines();

            SetDefaltPosition();
        }
    }

    private void SetDefaltPosition()
    {
        leaningCoroutine = StartCoroutine(BackToCenter(backTime, cinemachineRecomposer.m_Dutch));
    }

    IEnumerator Leaning(float rotation, Transform target)
    {        
        float currentTime = 0;
        float value = 0;
        float addValue = 1 / leaningTime;
        do
        {
            currentTime += /*(1/leaningTime)*/ Time.deltaTime;
            value += addValue * Time.deltaTime;

            cinemachineRecomposer.m_Dutch = rotation * value;

            cinemachineTarget.position = (defaltPosition.position-(defaltPosition.position*value)) - (target.position * value);

            yield return null;

        } while (currentTime < leaningTime);

        cinemachineRecomposer.m_Dutch = rotation;
        cinemachineTarget.position = target.position;
    }

    IEnumerator BackToCenter(float time, float currentRotation /*Transform currentPosition*/)
    {
        //Vector3 difference = defaltPosition.position - currentPosition;

        float currentTime = 0;
        float value = 0;
        float addValue = 1 / time;
        do
        {
            currentTime += Time.deltaTime;
            value += addValue * Time.deltaTime;

            cinemachineRecomposer.m_Dutch = currentRotation - (currentRotation * value);

            cinemachineTarget.position = (cinemachineTarget.position - (cinemachineTarget.position*value) + (defaltPosition.position * value));

            yield return null;

        } while (currentTime < time);

        cinemachineRecomposer.m_Dutch = 0;

        cinemachineTarget.position = defaltPosition.position;
    }

    IEnumerator FastCentralize()
    {

        yield return StartCoroutine(BackToCenter(fastBackTime, cinemachineRecomposer.m_Dutch));

        if (currentState == State.LEANINGRIGHT)
        {
            leaningCoroutine = StartCoroutine(Leaning(-rotationZ, rigthLeaningPosition));
        }
        else
        {
            leaningCoroutine = StartCoroutine(Leaning(rotationZ, leftLeaningPosition));
        }
    }
}
