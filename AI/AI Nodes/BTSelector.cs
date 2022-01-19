using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTSelector : BTnode
{
    private List<BTnode> nodes;

    private Coroutine nodeRunning;

    private BehaviorTree behaviorTreeRef;
    
    public BTSelector(List<BTnode> nodes)
    {
        this.nodes = nodes;
    }

    public override IEnumerator Run(BehaviorTree behaviorTree)
    {
        status = BTstatus.RUNNING;

        behaviorTreeRef = behaviorTree;

        for(int i = 0 ; i < nodes.Count; i++)
        {
            BTnode currentNode = nodes[i];
            yield return nodeRunning = behaviorTree.StartCoroutine(currentNode.Run(behaviorTree));
            if (currentNode.GetStatus() == BTstatus.SUCCESS)
            {
                status = BTstatus.SUCCESS;
                break;
            }
        }

        if (status == BTstatus.RUNNING)
            status = BTstatus.FAILURE;
        
    }

    public override void ForceStop()
    {
        if (behaviorTreeRef)
        {
            behaviorTreeRef.StopCoroutine(nodeRunning);
        }
    }
}
