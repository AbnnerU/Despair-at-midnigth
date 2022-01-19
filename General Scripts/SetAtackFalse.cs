
using UnityEngine;

public class SetAtackFalse : MonoBehaviour
{
    [SerializeField] private Animator anim;

    public void AtckFalse()
    {
        anim.SetBool("Atack", false);
    }
}
