using TMPro;
using UnityEngine;

public class HudHelper : MonoBehaviour
{
    public static HudHelper Instance;

    public GameObject hud;

    public TextMeshProUGUI score;
    public TextMeshProUGUI gunNameField;
    public RectTransform heatBarFiller;
    public RectTransform heatBarBackground;
    public RectTransform healthBarFiller;
    public RectTransform healthBarBackground;

    private float maxHeatBarFillerWidth = 0.0f;
    private float maxHealthBarFillerWidth = 0.0f;

    public void Start()
    {
        Instance = this;

        maxHeatBarFillerWidth = heatBarBackground.rect.width - 4.0f;
        maxHealthBarFillerWidth = healthBarBackground.rect.width - 4.0f;
    }

    public static void SetScore(int _score) => Instance.score.text = _score.ToString();

    public static void SetGunName(string _gunName) => Instance.gunNameField.text = _gunName;

    public static void SetHealthBar(float _value)
    {
        Instance.healthBarFiller.sizeDelta = new Vector2(Instance.maxHealthBarFillerWidth * _value, Instance.healthBarFiller.sizeDelta.y);
    }

    public static void SetHeatBar(float _value)
    {
        Instance.heatBarFiller.sizeDelta = new Vector2(Instance.maxHeatBarFillerWidth * _value , Instance.heatBarFiller.sizeDelta.y);
    }

    public static void ShowHud()
    {
        Instance.hud.SetActive(true);
    }

    public static void HideHud()
    {
        Instance.hud.SetActive(false);
    }
}
