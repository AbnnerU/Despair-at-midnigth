using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTIsOnState : BTnode
{
    private ChaserAIManager aIManager;

    private AiState stateTarget;

    public BTIsOnState(ChaserAIManager chaserAi, AiState stateTarget)
    {
        aIManager = chaserAi;

        this.stateTarget = stateTarget;
    }

    public override IEnumerator Run(BehaviorTree behaviorTree)
    {
        status = BTstatus.RUNNING;

        if (aIManager.GetAiState() == stateTarget)
            status = BTstatus.SUCCESS;
        else
            status = BTstatus.FAILURE;

       

        yield break;
                
    }
}
