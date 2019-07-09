using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour { 
    
    private float partyMemberInfoVerticalSpacing = 100.0f;
    private float partyMemberPortraitVerticalSpacing = 0.0f;
    private float partyMemberPortraitHorizontalSpacing = 0.0f;
    private float partyMemberStatusHorizontalSpacing = 0.0f;
    private float partyMemberPortraitWidth = 50.0f;
    private float partyMemberPortraitHeight = 50.0f;
    private float partyMemberStatusVerticalSpacing = 0.0f;
    private Vector2 partyMemberInfoAnchor;

    // Start is called before the first frame update
    void Start() {
        partyMemberInfoAnchor = new Vector2(partyMemberPortraitWidth, Screen.height - partyMemberPortraitHeight * 2);
        gameObject.layer = LayerMask.NameToLayer("UI");
        buildInterface();
    }

    // Update is called once per frame
    void Update() { }

    void buildInterface() {
        gameObject.AddComponent<Canvas>();
        gameObject.AddComponent<CanvasScaler>();
        gameObject.AddComponent<GraphicRaycaster>();
        RectTransform parentRectTransform = gameObject.GetComponent<RectTransform>();
        Canvas canvas = gameObject.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.pixelPerfect = true;
        int counter = 0;

        GameObject partyObject = GameObject.Find("Party");
        foreach (Transform child in partyObject.transform) {
            UnitController unit = child.GetComponent<UnitController>();
            if (unit != null) {
                Sprite portrait = unit.portrait;
                GameObject portraitWrapper = new GameObject();
                portraitWrapper.name = "portrait_" + unit.name;
                portraitWrapper.layer = LayerMask.NameToLayer("UI");
                portraitWrapper.transform.parent = gameObject.transform;
                portraitWrapper.AddComponent<Image>();
                portraitWrapper.GetComponent<Image>().sprite = portrait;
                RectTransform portraitTransform = portraitWrapper.GetComponent<RectTransform>();
                portraitTransform.anchoredPosition = new Vector2(0.0f, 0.0f);
                portraitTransform.sizeDelta = new Vector2(partyMemberPortraitWidth, partyMemberPortraitHeight);
                portraitTransform.anchorMin = new Vector2(0.5f, 0.5f);
                portraitTransform.anchorMax = new Vector2(0.5f, 0.5f);
                portraitTransform.pivot = new Vector2(0.0f, 0.0f);
                portraitWrapper.transform.position = new Vector3(partyMemberInfoAnchor[0],
                                                                 partyMemberInfoAnchor[1] - (counter * partyMemberInfoVerticalSpacing),
                                                                 0.0f);

                Texture2D statusBarsTexture = Resources.Load("Textures/health_mana_bar") as Texture2D;
                Sprite statusBars = Sprite.Create(statusBarsTexture, 
                                                  new Rect(0.0f, 0.0f, statusBarsTexture.width, statusBarsTexture.height), 
                                                  new Vector2(0.5f, 0.5f));
                GameObject statusBarsWrapper = new GameObject();
                statusBarsWrapper.name = "status_bars_" + unit.name;
                statusBarsWrapper.layer = LayerMask.NameToLayer("UI");
                statusBarsWrapper.transform.parent = gameObject.transform;
                statusBarsWrapper.AddComponent<Image>();
                statusBarsWrapper.GetComponent<Image>().sprite = statusBars;
                RectTransform statusBarsTransform = statusBarsWrapper.GetComponent<RectTransform>();
                statusBarsTransform.anchoredPosition = new Vector2(0.0f, 0.0f);
                statusBarsTransform.sizeDelta = new Vector2(statusBarsTexture.width, statusBarsTexture.height);
                statusBarsTransform.anchorMin = new Vector2(0.5f, 0.5f);
                statusBarsTransform.anchorMax = new Vector2(0.5f, 0.5f);
                statusBarsTransform.localScale = new Vector3(3.0f, 3.0f, 1.0f);
                statusBarsTransform.pivot = new Vector2(0.0f, 0.0f);
                statusBarsWrapper.transform.position = new Vector3(partyMemberInfoAnchor[0] + partyMemberPortraitWidth,
                                                                   partyMemberInfoAnchor[1] - partyMemberStatusVerticalSpacing -
                                                                   (counter * partyMemberInfoVerticalSpacing),
                                                                   0.0f);

                counter++;
            }
        }
    }
}
