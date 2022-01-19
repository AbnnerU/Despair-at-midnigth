using System.Collections;


public abstract class BTnode
{
    protected BTstatus status;

    public abstract IEnumerator Run(BehaviorTree behaviorTree);  

    public virtual BTstatus GetStatus()
    {
        return status;
    }

    public virtual void ForceStop()
    {
        //
    }
}

public interface INode
{ 
  
    IEnumerator Run(BehaviorTree behaviorTree);

    BTstatus GetStatus();
}



public enum BTstatus
{
    RUNNING,
    FAILURE,
    SUCCESS
}