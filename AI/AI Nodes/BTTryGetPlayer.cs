using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTTryGetPlayer : BTnode
{
    private Transform atackStartPoint;

    private ChaserAIManager chaserAIManager;

    private LayerMask layerMask;

    private Collider[] results;

    public Action OnStartAtackCallBack;

    private string targetTag;

    private int maxAllocation;

    private float atackRadius;

    private float atackDelay;

    public BTTryGetPlayer(ChaserAIManager chaserAIManager,Transform atackStartPoint, LayerMask layerMask, float atackRadius, int maxAllocation, string targetTag,float atackDelay)
    {
        this.atackStartPoint = atackStartPoint;
        this.layerMask = layerMask;
        this.atackRadius = atackRadius;
        this.maxAllocation = maxAllocation;
        this.targetTag = targetTag;
        this.atackDelay = atackDelay;
        this.chaserAIManager = chaserAIManager;

        results = new Collider[maxAllocation];

    }

    public BTTryGetPlayer(Action onStartAtackCallBack,ChaserAIManager chaserAIManager, Transform atackStartPoint, LayerMask layerMask, float atackRadius, int maxAllocation, string targetTag, float atackDelay)
    {
        this.atackStartPoint = atackStartPoint;
        this.layerMask = layerMask;
        this.atackRadius = atackRadius;
        this.maxAllocation = maxAllocation;
        this.targetTag = targetTag;
        this.atackDelay = atackDelay;
        this.chaserAIManager = chaserAIManager;

        results = new Collider[maxAllocation];

        OnStartAtackCallBack = onStartAtackCallBack;
    }

    public override IEnumerator Run(BehaviorTree behaviorTree)
    {
        OnStartAtackCallBack?.Invoke();

        yield return new WaitForSeconds(atackDelay);

        int hits = Physics.OverlapSphereNonAlloc(atackStartPoint.position, atackRadius, results, layerMask);
        if (hits > 0)
        {
            for(int i = 0; i < hits; i++)
            {
                if (results[i].CompareTag(targetTag))
                {
                    chaserAIManager.OnHitPlayer?.Invoke();
                    status = BTstatus.SUCCESS;

                    //Debug.Log("Pegou player: " + status);
                    yield break;
                }
            }

            status = BTstatus.FAILURE;
        }
        else
        {
            status = BTstatus.FAILURE;
        }

        //Debug.Log("Pegou player: " + status);
        yield break;

    }
}
