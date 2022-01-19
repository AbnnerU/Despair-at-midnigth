using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTIsCloseToTheTarget : BTnode
{
    private Transform target;

    private Transform iaTransform;

    private float referenceDistance;

    public BTIsCloseToTheTarget(Transform iaTransform,Transform target, float referenceDistance)
    {
        this.iaTransform = iaTransform;

        this.target = target;

        this.referenceDistance = referenceDistance;
    }

    public override IEnumerator Run(BehaviorTree behaviorTree)
    {
        status = BTstatus.RUNNING;

        if (target != null)
        {
            if((target.position - iaTransform.position).magnitude < referenceDistance)
            {
                status = BTstatus.SUCCESS;
            }
            else
            {
                status = BTstatus.FAILURE;
            }

        }
        else
        {
            status = BTstatus.FAILURE;
        }

        yield break;
    }
}
