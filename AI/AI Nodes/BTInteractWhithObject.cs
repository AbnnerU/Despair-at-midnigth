using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTInteractWhithObject : BTnode
{
    private ChaserAIManager aIManager;
    private SoundIntensityReceptor soundReceptor;

    public BTInteractWhithObject(ChaserAIManager chaserManager)
    {
        aIManager = chaserManager;
    }

    public override IEnumerator Run(BehaviorTree behaviorTree)
    {
        status = BTstatus.RUNNING;
        GameObject interactObj = aIManager.GetMarquedSound().gameObject;

        IAIInteractAction aIInteractAction = interactObj.GetComponent<IAIInteractAction>();

        if (aIInteractAction != null)
        {
            aIInteractAction.Interact();

            status = BTstatus.SUCCESS;
        }
        else
        {          
            status = BTstatus.FAILURE;
        }

        yield break;
    }
}
