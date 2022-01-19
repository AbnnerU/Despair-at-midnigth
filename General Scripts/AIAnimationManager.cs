using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIAnimationManager : MonoBehaviour
{
    [SerializeField] private ChaserAIManager aIManager;

    [SerializeField] private NavMeshAgent agent;

    [SerializeField] private Animator anim;

    [SerializeField] private bool useMoveAnimation;

    [SerializeField] private float minVelocity = 0.01f;

    private Transform _tranform;

    private  Vector3 lastPosition;

    private int crounchMask;

    private bool active = true;

    private void Awake()
    {
        _tranform = GetComponent<Transform>();

        crounchMask = 1 << NavMesh.GetAreaFromName("Crounch Area");

        aIManager.OnStartAtack += StartAtack;
    }

    private void StartAtack(bool value)
    {
        anim.SetBool("Atack",value);
    }

    private void OnDisable()
    {
        active = false;
    }

    private void OnEnable()
    {
        active = true;

        lastPosition = _tranform.position;
    }

    private void Update()
    {
        if (useMoveAnimation == false)
            return;

        NavMeshHit navMeshHit;

        agent.SamplePathPosition(NavMesh.AllAreas, 0f, out navMeshHit);


        if((navMeshHit.mask & crounchMask) != 0)
        {
            anim.SetBool("Crounch", true);
        }
        else
        {
            anim.SetBool("Crounch", false);
        }


        if (active)
        {           
            if((_tranform.position - lastPosition).magnitude > minVelocity)
            {
                anim.SetFloat("Walk Blend", 1);               
            }
            else
            {
               
                anim.SetFloat("Walk Blend", 0);
            }
            lastPosition = _tranform.position;
        }
    }

  
}
