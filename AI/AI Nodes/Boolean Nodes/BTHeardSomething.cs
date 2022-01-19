using System;
using System.Collections;
using UnityEngine;

public class BTHeardSomething : BTnode
{
    private SoundIntensityReceptor soundReceptor;

    private SoundType soundType;

    public Action OnHeardSoundCallBack;

    public Action OnHeardSoundFailCallBack;

    public BTHeardSomething(SoundIntensityReceptor receptor, SoundType type)
    {
        soundReceptor = receptor;

        soundType = type;
    }

    public BTHeardSomething(SoundIntensityReceptor receptor, SoundType type, Action onHeardSucess)
    {
        soundReceptor = receptor;

        soundType = type;

        OnHeardSoundCallBack = onHeardSucess;
    }

    public BTHeardSomething(SoundIntensityReceptor receptor, SoundType type, Action onHeardSucess, Action onHeardFail)
    {
        soundReceptor = receptor;

        soundType = type;

        OnHeardSoundCallBack = onHeardSucess;

        OnHeardSoundFailCallBack = onHeardFail;
    }

    public override IEnumerator Run(BehaviorTree behaviorTree)
    {
        status = BTstatus.RUNNING;

        if (soundReceptor.GetHeardSomething()==true && soundReceptor.GetCurrentSoundType()==soundType)
        {
            status = BTstatus.SUCCESS;

            OnHeardSoundCallBack?.Invoke();
        }
        else
        {       
            status = BTstatus.FAILURE;

            OnHeardSoundFailCallBack?.Invoke();
        }

     

        yield break;
    }
}
