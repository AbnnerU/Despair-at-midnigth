using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

public class BehaviorTree : MonoBehaviour
{
    private BTnode root;

    private float executioninterval;

    private bool active = true;

    public IEnumerator Begin()
    {
        while (active)
        {
            yield return StartCoroutine(root.Run(this));
            //print(root.GetStatus().ToString());
            yield return new WaitForSeconds(executioninterval);
        }
    }

    public void Stop()
    {
        active = false;
        StopCoroutine(Begin());
        StopCoroutine(root.Run(this));

        root.ForceStop();
    }

    public void SetBehaviorRoot(BTnode rootnode)
    {
        root = rootnode;
    }

    public void SetExecutionInterval(float value)
    {
        executioninterval = value;
    }

    public void SetActive(bool value)
    {
        active = value;
    }
}
