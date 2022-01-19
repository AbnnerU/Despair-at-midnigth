using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTHeardDiferrentSound : BTnode
{
    private SoundIntensityReceptor soundReceptor;

    private ChaserAIManager aIManager;

    public BTHeardDiferrentSound(SoundIntensityReceptor soundReceptor, ChaserAIManager chaserManager)
    {
        this.soundReceptor = soundReceptor;
        aIManager = chaserManager;
    }

    public override IEnumerator Run(BehaviorTree behaviorTree)
    {
        status = BTstatus.RUNNING;

        if (aIManager.GetMarquedSound() != soundReceptor.GetSoundOrigin())
            status = BTstatus.SUCCESS;
        else
            status = BTstatus.FAILURE;
        //Debug.Log(status.ToString());
        yield break;
    }
}
