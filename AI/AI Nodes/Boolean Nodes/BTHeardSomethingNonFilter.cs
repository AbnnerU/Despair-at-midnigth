using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTHeardSomethingNonFilter : BTnode
{
    private SoundIntensityReceptor soundReceptor;

    public BTHeardSomethingNonFilter(SoundIntensityReceptor receptor)
    {
        soundReceptor = receptor;
    }

    public override IEnumerator Run(BehaviorTree behaviorTree)
    {
        status = BTstatus.RUNNING;

        if (soundReceptor.GetHeardSomething() == true)
        {
            status = BTstatus.SUCCESS;
        }
        else
        {
            status = BTstatus.FAILURE;
        }



        yield break;
    }
}