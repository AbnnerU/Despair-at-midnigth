using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

[System.Serializable] public class OnOpenTextScreen : UnityEvent { }
[System.Serializable] public class OnCloseTextScreen : UnityEvent { }

public class TextScreen : MonoBehaviour
{
    [SerializeField] private InputController inputController;

    [SerializeField] private AnchorTypeConfig anchorType;

    [SerializeField] private GameObject background;

    [SerializeField] private GameObject[] allTextsSlot;

    [SerializeField] private GameObject[] allImagesSlot;

    [Space(3)]
    public OnOpenTextScreen onOpenTextScreen;

    public OnCloseTextScreen onCloseTextScreen;

    private List<TMP_Text> textComponents;
    private List<Image> imageComponents;

    private RectTransform backgroundTransform;
    private Image backgroundImageComponent;

    private List<RectTransform> textTransform;

    private List<RectTransform> imagesTransform;

    private TextPage[] allPages;

    private TextPage currentPage;

    private int currentNumber;

    private void Awake()
    {
        background.SetActive(false);

        SetComponents();

        if (inputController == null)
            inputController = FindObjectOfType<InputController>();

        inputController.OnExit += Close;
        inputController.OnNextPage += NextPage;
        inputController.OnBackPage += BackPage;

    }

    public void StartShowPages(TextPage[] pages)
    {
        onOpenTextScreen?.Invoke();

        background.SetActive(true);

        inputController.EnableInventoryInputs();      

        allPages = pages;

        ModifyCanvas(0);
    }

    public void Close()
    {
        onCloseTextScreen?.Invoke();

        inputController.EnableGameplayInputs();

        background.SetActive(false);
       
        allPages = null;

        currentNumber = 0;

    }

    public void NextPage()
    {
        if (currentNumber < allPages.Length-1)
        {
            //print("Next");
            //print(currentNumber+"/"+allPages.Count);
            currentNumber++;
            ModifyCanvas(currentNumber);
        }
    }

    public void BackPage()
    {
        if (currentNumber > 0)
        {
            currentNumber--;
            ModifyCanvas(currentNumber);
        }
    }
  

    private void ModifyCanvas(int pageNumber)
    {

        DisableAllSlots();

        currentNumber = pageNumber;

        currentPage = allPages[pageNumber];

        //background
        ChangeBackground();
        //text
        SetTexts();
        //image
        SetImage();


    }

    public void ChangeBackground()
    {
        SetLeft(backgroundTransform, currentPage.BGLeft);
        SetRight(backgroundTransform, currentPage.BGRight);
        SetTop(backgroundTransform, currentPage.BGTop);
        SetBottom(backgroundTransform, currentPage.BGBottom);


        if (currentPage.BgImage != null)
        {
            backgroundImageComponent.sprite = currentPage.BgImage;

            if (currentPage.setNativeSize)
                backgroundImageComponent.SetNativeSize();
        }

        backgroundImageComponent.color = currentPage.BGColor;
    }

    public void SetTexts()
    {
        int textAmount = 0;
        if(currentPage.texts.Length > allTextsSlot.Length)
        {
            Debug.LogWarning("text slots is less than the amount of texts");
            textAmount = allTextsSlot.Length;
        }
        else
        {
            textAmount = currentPage.texts.Length;
        }

        if (textAmount == 0)
            return;

        Vector2 min = Vector2.zero;
        Vector2 max = Vector2.zero;

        for (int i = 0; i < textAmount; i++)
        {
            allTextsSlot[i].SetActive(true);

            RectTransform rt = textTransform[i];
            TextConfig textConfig = currentPage.texts[i];
            
            //Anchor
            ChooseAnchorStretch(textConfig.anchortype, out min, out max);

            rt.anchorMax = max;
            rt.anchorMin = min;

            //PosY
            rt.anchoredPosition = new Vector2(rt.anchoredPosition.x, textConfig.posY);

            //Left-Right
            SetRight(rt, textConfig.right);
            SetLeft(rt, textConfig.left);

            //Height
            SetSize(rt, rt.sizeDelta.x,textConfig.height);

            //Font
            textComponents[i].font = textConfig.font;

            //Text
            textComponents[i].text = textConfig.text;

            //FontSize
            textComponents[i].fontSize = textConfig.fontSize;

            //Alignment
            textComponents[i].alignment = textConfig.alignmentOptions;

            //Font Style
            textComponents[i].fontStyle = textConfig.fontStyle;

            //Font Color
            textComponents[i].color = textConfig.color;

            //Spacing
            textComponents[i].lineSpacing = textConfig.lineSpacing;
            textComponents[i].paragraphSpacing = textConfig.paragraphSpacing;
            textComponents[i].characterSpacing = textConfig.characterSpacing;
            textComponents[i].wordSpacing = textConfig.wordSpacing;

        }

        print("Texto feito");
    }

    private void SetImage()
    {
        int textAmount = 0;
        if (currentPage.images.Length > allImagesSlot.Length)
        {
            Debug.LogWarning("text slots is less than the amount of texts");
            textAmount = allImagesSlot.Length;
        }
        else
        {
            textAmount = currentPage.images.Length;
        }

        if (textAmount == 0)
            return;

        Vector2 min = Vector2.zero;
        Vector2 max = Vector2.zero;

        for (int i = 0; i < textAmount; i++)
        {
            allImagesSlot[i].SetActive(true);

            RectTransform rt = imagesTransform[i];
            ImageConfig imageConfig = currentPage.images[i];
            Image currentImage = imageComponents[i];

            //Anchor
            ChooseAnchor(imageConfig.anchortype, out min, out max);

            rt.anchorMax = max;
            rt.anchorMin = min;

            //Position
            rt.anchoredPosition = imageConfig.position;

            //Sprite
            currentImage.sprite = imageConfig.sprite;

            //Size
            if (imageConfig.setNativeSize)
            {
                currentImage.SetNativeSize();
            }
            else
            {
                SetSize(rt, imageConfig.width, imageConfig.height);
            }

           
         
        }
        print("Imagem feito");
    }

    public void ChooseAnchorStretch(AnchorTypeStretch type,out Vector2 minValue,out Vector2 maxValue)
    {       
        int index =anchorType.anchorStretchConfigs.FindIndex(x => x.name == type);
        minValue = anchorType.anchorStretchConfigs[index].anchorMin;
        maxValue = anchorType.anchorStretchConfigs[index].anchorMax;
    }

    public void ChooseAnchor(Anchortype type, out Vector2 minValue, out Vector2 maxValue)
    {
        int index = anchorType.anchorConfigs.FindIndex(x => x.name == type);
        minValue = anchorType.anchorConfigs[index].anchorMin;
        maxValue = anchorType.anchorConfigs[index].anchorMax;
    }

    public void SetLeft( RectTransform rt, float left)
    {
        rt.offsetMin = new Vector2(left, rt.offsetMin.y);
    }

    public void SetRight( RectTransform rt, float right)
    {
        rt.offsetMax = new Vector2(-right, rt.offsetMax.y);
    }

    public void SetTop( RectTransform rt, float top)
    {
        rt.offsetMax = new Vector2(rt.offsetMax.x, -top);
    }

    public void SetBottom( RectTransform rt, float bottom)
    {
        rt.offsetMin = new Vector2(rt.offsetMin.x, bottom);
    }

    public void SetSize(RectTransform rt, float width, float height)
    {
        rt.sizeDelta = new Vector2(width ,height);
    }

    private void DisableAllSlots()
    {
        foreach(GameObject obj in allImagesSlot)
        {
            obj.SetActive(false);
        }

        foreach(GameObject obj in allTextsSlot)
        {
            obj.SetActive(false);
        }
    }

    private void SetComponents()
    {
        textComponents = new List<TMP_Text>();
        imageComponents = new List<Image>();

        textTransform = new List<RectTransform>();
        imagesTransform = new List<RectTransform>();  

        foreach (GameObject obj in allTextsSlot)
        {
            textComponents.Add(obj.GetComponent<TMP_Text>());

            textTransform.Add(obj.GetComponent<RectTransform>());

        }

   

        foreach (GameObject obj in allImagesSlot)
        {
            imageComponents.Add(obj.GetComponent<Image>());

            imagesTransform.Add(obj.GetComponent<RectTransform>());
        }


        backgroundTransform = background.GetComponent<RectTransform>();
        backgroundImageComponent = background.GetComponent<Image>();
        
    }



}
