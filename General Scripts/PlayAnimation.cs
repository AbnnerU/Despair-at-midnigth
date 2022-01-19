using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class AnimationEvent: UnityEvent { }

public class PlayAnimation : MonoBehaviour
{
    [SerializeField] private Animator anim;

    public AnimationEvent animationEvent;

    private void Awake()
    {
        if (anim == null)
            anim = GetComponent<Animator>();
    }

    public void Play(string animationName)
    {
        anim.Play(animationName, 0, 0);
    }


    public void InvokeEvent()
    {
        animationEvent?.Invoke();
    }
}
