using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BTMoveToPathPoint : BTnode
{
    private NavMeshAgent agent;
    private AiPathPoints pathPoints;
    private ChaserAIManager aIManager;
    private Color color = Color.red;

    public BTMoveToPathPoint(AiPathPoints pathPoints, NavMeshAgent agent, ChaserAIManager chaserAI)
    {
        this.pathPoints = pathPoints;
        this.agent = agent;
        aIManager = chaserAI;
    }

    public override IEnumerator Run(BehaviorTree behaviorTree)
    {
        status = BTstatus.RUNNING;

        float distance = agent.stoppingDistance;

        aIManager.SetNewState(AiState.WALKING);

        agent.SetDestination(pathPoints.GetValidPoint());
        agent.isStopped = false;

        while (agent != null)
        {
            if(agent.pathPending == false && agent.remainingDistance < distance)
            {
                Debug.Log("Chegou");
                status = BTstatus.SUCCESS;

                yield break;
            }

            yield return null;
        }

        status = BTstatus.FAILURE;

        yield break;
        
    }
}
