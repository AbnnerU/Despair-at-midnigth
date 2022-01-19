
using System.Collections.Generic;
using UnityEngine;

public class SetTextScreen : MonoBehaviour
{
    [SerializeField] private TextPage[] page;
    [SerializeField] private TextScreen textScreen;

    private void Awake()
    {
        if (textScreen == null)
            textScreen = FindObjectOfType<TextScreen>();
    }

    public void SetPage()
    {
        textScreen.StartShowPages(page);
    }

   
}
