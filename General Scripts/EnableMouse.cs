
using UnityEngine;

public class EnableMouse : MonoBehaviour
{
    private void Awake()
    {
        Cursor.lockState = CursorLockMode.None;
    }
}
