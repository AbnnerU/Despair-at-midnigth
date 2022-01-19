using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTMarckSound : BTnode
{
    private SoundIntensityReceptor soundReceptor;

    private ChaserAIManager aIManager;

    public BTMarckSound(SoundIntensityReceptor soundReceptor, ChaserAIManager chaserManager)
    {
        this.soundReceptor = soundReceptor;
        aIManager = chaserManager;
    }

    public override IEnumerator Run(BehaviorTree behaviorTree)
    {
        status = BTstatus.RUNNING;

        aIManager.MarkSoundOrigin(soundReceptor.GetSoundOrigin());

        status = BTstatus.SUCCESS;

        //Debug.Log("MARCOU");
        yield break;
    }
}
