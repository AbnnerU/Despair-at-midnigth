using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BTMoveToTarget : BTnode
{
    private Transform _transform;
    private SoundIntensityReceptor soundReceptor;
    private NavMeshAgent agent;
    private ChaserAIManager aIManager;
    private Transform target;
    private LayerMask groundLayer;
    private Vector3 positionVariation;
    private RaycastHit[] hitResult;

    public Action OnLostTarget;

    public Action OnFindTarget;

    private float minDistance;
    private float updateDestinationDelay;

    private float validVariationDistance;
    private float persistentFollowTime;
    private float timeToStartVariation;
    private float variationDuration;
    private int variationAmount;
    private int maxTentatives;

    private Color color = Color.red;


    public BTMoveToTarget(SoundIntensityReceptor soundReceptor,NavMeshAgent agent, Transform target, ChaserAIManager chaserAI, Vector3 positionVariation, LayerMask layerMask, float timeToStartVariation, float validVariationDistance, int variationAmount, int maxTentatives, float minDistance, float persistentFollowTime,float updateDestinationDelay)
    {
        this.soundReceptor = soundReceptor;
        this.agent = agent;
        aIManager = chaserAI;
        this.target = target;
        this.minDistance = minDistance;
        this.updateDestinationDelay = updateDestinationDelay;
        this.persistentFollowTime = persistentFollowTime;
        this.positionVariation = positionVariation;
        this.groundLayer = layerMask;
        this.variationAmount = variationAmount;
        this.maxTentatives = maxTentatives;
        this.timeToStartVariation = timeToStartVariation;
        this.validVariationDistance = validVariationDistance;

        variationDuration = (persistentFollowTime - timeToStartVariation) / variationAmount;

        _transform = agent.transform;

        hitResult = new RaycastHit[1];
    }

    public BTMoveToTarget(Action onFindTarget,Action onLostTarget,SoundIntensityReceptor soundReceptor, NavMeshAgent agent, Transform target, ChaserAIManager chaserAI, Vector3 positionVariation, LayerMask layerMask, float timeToStartVariation, float validVariationDistance, int variationAmount, int maxTentatives, float minDistance, float persistentFollowTime, float updateDestinationDelay)
    {
        this.soundReceptor = soundReceptor;
        this.agent = agent;
        aIManager = chaserAI;
        this.target = target;
        this.minDistance = minDistance;
        this.updateDestinationDelay = updateDestinationDelay;
        this.persistentFollowTime = persistentFollowTime;
        this.positionVariation = positionVariation;
        this.groundLayer = layerMask;
        this.variationAmount = variationAmount;
        this.maxTentatives = maxTentatives;
        this.timeToStartVariation = timeToStartVariation;
        this.validVariationDistance = validVariationDistance;

        variationDuration = (persistentFollowTime - timeToStartVariation) / variationAmount;

        _transform = agent.transform;

        hitResult = new RaycastHit[1];

        OnLostTarget = onLostTarget;

        OnFindTarget = onFindTarget;
    }

    public override IEnumerator Run(BehaviorTree behaviorTree)
    {
        status = BTstatus.RUNNING;

        float currentPersitentTime = 0;

        float currentTimeToVariation=0;

        float currentVariationDuration=0;

        bool firtVariation = true;

        agent.SetDestination(soundReceptor.GetSoundOrigin().position);

        aIManager.SetNewState(AiState.CHASINGPLAYER);

        agent.isStopped = false;

        while (agent.enabled==true && target != null) 
        {
            if (soundReceptor.GetHeardSomething() == true)
            {
                OnFindTarget?.Invoke();

                currentPersitentTime = 0;
                currentTimeToVariation = 0;
                currentVariationDuration = 0;
                firtVariation = true;

                if (agent.pathPending == false && agent.remainingDistance < minDistance)
                {
                    status = BTstatus.SUCCESS;

                    yield break;
                }
               
                agent.SetDestination(soundReceptor.GetSoundOrigin().position);

            }
            else
            {               

                if (agent.pathPending == false && (target.position-_transform.position).magnitude < 1f)
                {
                    status = BTstatus.SUCCESS;

                    yield break;
                }

                currentTimeToVariation += updateDestinationDelay;

                currentPersitentTime += updateDestinationDelay;

                if (currentPersitentTime < persistentFollowTime)
                {
                    if (currentTimeToVariation >= timeToStartVariation)
                    {
                        OnLostTarget?.Invoke();

                        if (currentVariationDuration >= variationDuration || firtVariation ==true)
                        {

                            currentVariationDuration = 0;
                            firtVariation = false;

                            Vector3 startPoint = target.position;
                            Vector3 finalPosition;
                            int tentative = 0;

                            do
                            {
                                Vector3 tempVariation = NewVariation();

                                Vector3 tempPosition = startPoint + tempVariation;

                                Vector3 tempDirection = tempPosition - startPoint;

                                if (Physics.RaycastNonAlloc(startPoint, tempDirection, hitResult,2.5f, groundLayer) > 0)
                                {
                                    float distance = (hitResult[0].point - startPoint).magnitude;

                                    if (distance >= validVariationDistance)
                                    {
                                        finalPosition = hitResult[0].point;
                                        break;
                                    }
                                }
                                else
                                {
                                    if (Vector3.Distance(startPoint,tempPosition) >= validVariationDistance)
                                    {
                                        
                                        finalPosition = tempPosition;
                                        break;
                                    }
                                }

                                finalPosition = tempPosition;
                                tentative++;

                            } while (tentative < maxTentatives);


                            agent.SetDestination(finalPosition);

                            //Debug.Log(finalPosition+"/"+tentative);

                        }
                       
                        currentVariationDuration += updateDestinationDelay;

                    }

                    //Debug.Log(currentPersitentTime);

                }
                else
                {
                    status = BTstatus.FAILURE;

                    agent.isStopped = true;

                    yield break;
                }

                
            }


            yield return new WaitForSeconds(updateDestinationDelay);

        }

        status = BTstatus.FAILURE;

        yield break;
    }

    private Vector3 NewVariation()
    {
        float positionX = UnityEngine.Random.Range(-positionVariation.x, positionVariation.x);
        float positionZ = UnityEngine.Random.Range(-positionVariation.z, positionVariation.z);

        return new Vector3(positionX, 0, positionZ);
    }
  
}
