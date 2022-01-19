using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "TextPage", menuName = "Assets/New Text Page")]
public class TextPage : ScriptableObject
{
    [Header("BackGround")]

    public float BGLeft;
    public float BGTop;
    public float BGRight;
    public float BGBottom;

    public Sprite BgImage;
    public bool setNativeSize;
    public Color BGColor;

    public TextConfig[] texts;

    public ImageConfig[] images;  
   
}

[Serializable]
public struct TextConfig
{
    [TextArea]
    public string text;

    public TMP_FontAsset font;

    public TextAlignmentOptions alignmentOptions;

    public  FontStyles fontStyle;

    public Color color;

    public float fontSize;

    public float lineSpacing;

    public float characterSpacing;

    public float paragraphSpacing;

    public float wordSpacing;

    [Header("Position")]
    public AnchorTypeStretch anchortype;

    public float left;

    public float right;

    public float posY;

    public float height;

}

[Serializable]
public struct ImageConfig
{
    public Sprite sprite;

    public Anchortype anchortype;

    public Vector3 position;

    public float width;

    public float height;

    public bool setNativeSize;
}