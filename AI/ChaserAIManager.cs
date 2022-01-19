using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

[System.Serializable]
public class OnAiAction : UnityEvent { }

public class ChaserAIManager : MonoBehaviour
{
    [SerializeField] private AiState aiState = AiState.INVESTIGATING;

    [SerializeField] private NavMeshAgent agent;

    [SerializeField] private BehaviorTree behaviorTree;

    [SerializeField] private float executionInterval;

    [SerializeField] private bool drawGizmos;

    [Header("Move to target")]

    [SerializeField] private Transform targetReference;

    [SerializeField] private float minDistance;

    [SerializeField] private float updateDestinationInterval=0.05f;

    [SerializeField] private float persitentFollowTime;

    [SerializeField] private Vector3 positionVariation;

    [SerializeField] private float timeToStartVariation;

    [SerializeField] private float variationMinDistance;

    [SerializeField] private int variationAmount = 2;

    [SerializeField] private int findPositionMaxTentatives=4;

    [Header("Sound detection")]

    [SerializeField] private SoundIntensityReceptor soundReceptor;

    [SerializeField] private float goToGenericSoundDelay = 1f;

    [SerializeField] private float soundMinDistance=1f;

    [SerializeField] private float doActionDelay;

    //[SerializeField] private float goToSoundDelay;

    [Header("Path")]

    [SerializeField] private AiPathPoints path;

    [SerializeField] private float goToNextPointDelay;

    [Header("Invetigating Local")]

    [SerializeField] private float maxMoveDistance;

    [SerializeField] private float minMoveAngle;

    [SerializeField] private float maxMoveAngle;

    [SerializeField] private LayerMask groundLayer;

    [Header("Looking around")]

    [SerializeField] private float lookingTime;

    [Header("Atack")]

    [SerializeField] private string targetTag;

    [SerializeField] private Transform atackStartPoint;

    [SerializeField] private LayerMask atackLayer;

    [SerializeField] private int maxAllocation;

    [SerializeField] private float atackRadius;

    [SerializeField] private float atackDelay;

    public OnAiAction OnHitPlayer;

    public OnAiAction OnHeardPlayer;

    public OnAiAction OnStopHeardPlayer;

    [Header("Data")]

    [SerializeField] private Transform marquedSoundOrigin;

    public Action<bool> OnStartAtack;

    private bool behaviorTreeCompleted = false;

    private bool lastHeardPlayerValue=false;

    private void Awake()
    {
        //StartBehaviorTree();
    }

    public void StopBehaviorTree()
    {
        OnStartAtack?.Invoke(false);

        soundReceptor.SetHeardSomething(false);

        marquedSoundOrigin = null;

        agent.enabled = false;

        behaviorTree.Stop();
    }

    public void OnEnable()
    {
        StartBehaviorTree();
    }

    public void StartBehaviorTree()
    {
        aiState = AiState.INVESTIGATING;

        gameObject.SetActive(true);

        agent.enabled = true;

        behaviorTree.SetActive(true);

        if (behaviorTreeCompleted)
        {
            behaviorTree.StartCoroutine(behaviorTree.Begin());
        }
        else
        {
            ConstructBehaviorTree();
        }
    }

    private void ConstructBehaviorTree()
    {
        #region----HeardPlayer----
        //----HeardPlayer----
        BTHeardSomething btHeardPlayer = new BTHeardSomething(soundReceptor, SoundType.PLAYER, onHeardSucess:()=>HeardPlayer(true),onHeardFail: ()=>HeardPlayer(false));
        BTMarckSound btMarckSound = new BTMarckSound(soundReceptor, this);
        BTHeardDiferrentSound btHeardDiferrentSound = new BTHeardDiferrentSound(soundReceptor, this);
        BTInverter btHeardDiferrentSoundInverter = new BTInverter(btHeardDiferrentSound);
        BTMoveToTarget btMoveToPlayer = new BTMoveToTarget(onFindTarget:()=>HeardPlayer(true),onLostTarget:()=>HeardPlayer(false),soundReceptor, agent, targetReference, this, positionVariation, groundLayer, timeToStartVariation, variationMinDistance,variationAmount, findPositionMaxTentatives, minDistance,persitentFollowTime ,updateDestinationInterval);    
        BTConditionalSequence btHPConditionalSequence = new BTConditionalSequence(new List<BTnode> { btHeardDiferrentSoundInverter}, btMoveToPlayer);
        BTTryGetPlayer btTryGetPlayer = new BTTryGetPlayer(onStartAtackCallBack:()=>OnStartAtack?.Invoke(true),this,atackStartPoint, atackLayer, atackRadius, maxAllocation, targetTag, atackDelay);
        BTSequence heardPlayerSequence = new BTSequence(new List<BTnode>
        {
            btHeardPlayer,
            btMarckSound,
            btHPConditionalSequence,
            btTryGetPlayer
        });
        #endregion


        #region----Investigating Local----
        //----Investigating Local----
        BTIsOnState btIsChansing = new BTIsOnState(this, AiState.CHASINGPLAYER);
        BTAgentHasPath btAgentHasPath = new BTAgentHasPath(agent);
        BTInverter btAgentHasPathInverter = new BTInverter(btAgentHasPath);
        BTHeardSomethingNonFilter btHeardSomethingNonFilter = new BTHeardSomethingNonFilter(soundReceptor);
        BTInverter btHeardSomethingInverter = new BTInverter(btHeardSomethingNonFilter);
        BTWaitForSeconds btINWaitForSeconds = new BTWaitForSeconds(lookingTime,agent, this, AiState.LOOKINGAROUND,"Investigating");
        BTConditionalSequence btINConditionalSequence01 = new BTConditionalSequence(new List<BTnode> { btHeardSomethingInverter }, btINWaitForSeconds);
        BTRandomNearbyPoint btRandomNearbyPoint = new BTRandomNearbyPoint(agent,maxMoveDistance,minMoveAngle,maxMoveAngle,groundLayer);
        BTConditionalSequence btINConditionalSequence02 = new BTConditionalSequence(new List<BTnode> { btHeardSomethingInverter }, btRandomNearbyPoint);
        BTSequence investigatingLocalSequence = new BTSequence(new List<BTnode>
        {
            btIsChansing,
            btAgentHasPathInverter,
            btINConditionalSequence01,
            btINConditionalSequence02,
            btINConditionalSequence01
        });
        #endregion


        #region----Heard Generic Sound----
        //----Heard Generic Sound----
        BTHeardSomething btHeardGenericSound = new BTHeardSomething(soundReceptor, SoundType.GENERIC);
        //BTInverter btHeardPlayerInverter = new BTInverter(btHeardPlayer);
        //BTHeardDiferrentSound btHeardDiferrentSound = new BTHeardDiferrentSound(soundReceptor, this);
        //BTInverter btHeardDiferrentSoundInverter = new BTInverter(btHeardDiferrentSound);
        BTWaitForSeconds btHGSWaitForSeconds01 = new BTWaitForSeconds(goToGenericSoundDelay,agent, "Generic sound");
        BTConditionalSequence bTHGSConditionalSequence01 = new BTConditionalSequence(new List<BTnode> {  btHeardDiferrentSoundInverter }, btHGSWaitForSeconds01);
        BTMoveToSoundOrigin btMoveToSoundOrigin = new BTMoveToSoundOrigin(agent, this, soundMinDistance);
        BTConditionalSequence bTHGSConditionalSequence02 = new BTConditionalSequence(new List<BTnode> {  btHeardDiferrentSoundInverter }, btMoveToSoundOrigin);
        BTWaitForSeconds btHGSWaitForSeconds02 = new BTWaitForSeconds(doActionDelay, agent, "Generic sound action delay");
        BTConditionalSequence bTHGSConditionalSequence03 = new BTConditionalSequence(new List<BTnode> { btHeardDiferrentSoundInverter }, btHGSWaitForSeconds02);
        BTInteractWhithObject btInteractWhithObject = new BTInteractWhithObject(this);
        BTSequence heardGenericSoundSequence = new BTSequence(new List<BTnode>
        {
            btHeardGenericSound,
            btMarckSound,
            bTHGSConditionalSequence01,
            bTHGSConditionalSequence02,
            bTHGSConditionalSequence03,
            btInteractWhithObject
        });
        #endregion


        #region ----Go To Path Point----
        //----Go To Path Point----
        BTMoveToPathPoint btMoveToPathPoint = new BTMoveToPathPoint(path, agent, this);
        BTConditionalSequence btPathCondicionalSequence01 = new BTConditionalSequence(new List<BTnode> { btHeardSomethingInverter }, btMoveToPathPoint);
        BTWaitForSeconds btPathWaitForSeconds = new BTWaitForSeconds(goToNextPointDelay,agent, "Path");
        BTConditionalSequence bTPathConditionalSequence02 = new BTConditionalSequence(new List<BTnode> { btHeardSomethingInverter }, btPathWaitForSeconds);
        BTSequence pathSequence = new BTSequence(new List<BTnode>
        {
            btHeardSomethingInverter,
            btAgentHasPathInverter,
            btPathCondicionalSequence01,
            bTPathConditionalSequence02
        });
        #endregion


        //----Root----
        BTSelector rootSelector = new BTSelector(new List<BTnode>
        {
            heardPlayerSequence,
            investigatingLocalSequence,
            heardGenericSoundSequence,
            pathSequence

        });


        behaviorTree.SetBehaviorRoot(rootSelector);

        behaviorTree.SetExecutionInterval(executionInterval);

        behaviorTree.StartCoroutine(behaviorTree.Begin());

        behaviorTreeCompleted = true;

    }

    public void MarkSoundOrigin(Transform value)
    {
        marquedSoundOrigin = value;
    }

    public Transform GetMarquedSound()
    {
        return marquedSoundOrigin;
    }

    public void SetNewState(AiState newState)
    {
        aiState = newState;
    }

    public AiState GetAiState()
    {
        return aiState;
    }

    public void HeardPlayer(bool heardPlayer)
    {
        if(heardPlayer==true && heardPlayer != lastHeardPlayerValue)
        {
            print("Escutou Jogador");

            lastHeardPlayerValue = heardPlayer;

            OnHeardPlayer?.Invoke();
        }
        else if(heardPlayer==false && heardPlayer!=lastHeardPlayerValue)
        {
            print("Não escutou player");

            lastHeardPlayerValue = heardPlayer;

            OnStopHeardPlayer?.Invoke();
        }
       
    }

    private void OnDrawGizmos()
    {
        if (drawGizmos)
        {
            if (atackStartPoint)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(atackStartPoint.position, atackRadius);
            }
        }

    }

}

[System.Serializable]
public struct FOV
{
    public string targetTag;

    public float visionFielValue;

    public Transform aimAtPosition;

    public bool drawGizmos;

    [Header("SphereCast")]

    public bool drawSphere;

    public Transform sphereStartPoint;

    public int maxAllocation;

    public float radius;

    public LayerMask validLayers;

    [Header("Raycast")]

    public Transform rayStartPoint;

    public float raycastLength;

    public LayerMask layersToInteract;

    
}


[System.Serializable]
public struct SearchHidingSpotsConfig
{
    public AiHidingSpotsData spotsData;

    public string targetTag;

    public Transform verificationStartPoint;

    public LayerMask validLayes;

    public float verificationRadius;

    public int maxAllocation;
}


public enum AiState
{
    WALKING,
    LOOKINGAROUND,
    INVESTIGATING,
    CHASINGPLAYER
}