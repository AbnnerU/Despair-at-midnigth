using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BTMoveToSoundOrigin : BTnode
{
    //private SoundIntensityReceptor soundReceptor;
    private NavMeshAgent agent;
    private ChaserAIManager aIManager;
    private float minDistance;
    private Color color = Color.red;


    public BTMoveToSoundOrigin(/*SoundIntensityReceptor soundReceptor,*/ NavMeshAgent agent, ChaserAIManager chaserAI, float minDistance)
    {
        //this.soundReceptor = soundReceptor;
        this.agent = agent;
        aIManager = chaserAI;
        this.minDistance = minDistance;
        
    }

    public override IEnumerator Run(BehaviorTree behaviorTree)
    {
        status = BTstatus.RUNNING;

        agent.SetDestination(aIManager.GetMarquedSound().position);

        aIManager.SetNewState(AiState.INVESTIGATING);
        agent.isStopped = false;

        while (agent.enabled == true && agent != null)
        {
            if(agent.pathPending == false && agent.remainingDistance < minDistance)
            {
                status = BTstatus.SUCCESS;

                yield break;
            }

            yield return null;
        }

        status = BTstatus.FAILURE;

        yield break;

    }
}
