using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BTWaitForSeconds :BTnode
{
    private NavMeshAgent agent;

    private ChaserAIManager aIManager;

    private AiState newState;

    private float waitTime;

    private bool changeState=false;

    private string print;

    public BTWaitForSeconds(float waitTime, NavMeshAgent agent, string print)
    {
        this.waitTime = waitTime;
        this.print = print;
        this.agent = agent;
    }

    public BTWaitForSeconds(float waitTime, NavMeshAgent agent,ChaserAIManager chaserAi,AiState newState, string print)
    {
        this.waitTime = waitTime;

        aIManager = chaserAi;     
               
        this.newState = newState;

        this.print = print;

        this.agent = agent;

        changeState = true;
    }

    public override IEnumerator Run(BehaviorTree behaviorTree)
    {
        status = BTstatus.RUNNING;

        float currentTime = 0;

        //Debug.Log("Inicio Tempo: "+print.ToString());

        if (changeState)
            aIManager.SetNewState(newState);

        agent.isStopped = true;

        do
        {
            if (agent.enabled == false)
            {
                Debug.Log("Cancelado: " + print.ToString());

                status = BTstatus.FAILURE;

                yield break;
            }

            currentTime += Time.deltaTime;

            yield return null;
        } while (currentTime < waitTime);

        
        

        Debug.Log("Fim Tempo: " +print.ToString());

        status = BTstatus.SUCCESS;
       
        yield break;

    }
}
