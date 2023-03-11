using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUIManager : MonoBehaviour
{
    // Start is called before the first frame update
    RoundManager _roundManager;
    [SerializeField]
    Image GunContainer;
    [SerializeField]
    Image KnifeContainer;
    [SerializeField]
    Image BombContainer;
    [SerializeField]
    Image GunGlow;
    [SerializeField]
    Image KnifeGlow;
    [SerializeField]
    Image BombGlow;
    Image currentGlow;

    [SerializeField]
    RectTransform GunContent;
    [SerializeField]
    RectTransform KnifeContent;
    [SerializeField]
    RectTransform BombContent;
    RectTransform CurrentContent;


    [SerializeField]
    Animation blinkAnim;
    [SerializeField]
    Animation anims;
    [SerializeField]
    Animation streakAnim;

    [SerializeField]
    TMPro.TextMeshProUGUI streakText;
    [SerializeField]
    TMPro.TextMeshProUGUI scoreText;


    Vector2 DefaultMinAnchors = new(0.0f,0.0f);
    Vector2 SelectedMinAnchors = new(0.05f,0.0f);
    Vector2 DefaultMaxAnchors = new(0.9f,1.0f);
    Vector2 SelectedMaxAnchors = new(0.95f,1.0f);
    void Start()
    {
        _roundManager = RoundManager.instance;
        _roundManager.getPlayer().m_weaponEvent.AddListener(WeaponChange);
        _roundManager.eWrongAnswer.AddListener(PlayBlink);
        _roundManager.ePlayerDied.AddListener(PlayDeath);
        _roundManager.eMultiplier.AddListener(PlayStreakText);
        currentGlow = GunGlow;
        CurrentContent = GunContent;
        WeaponChange(WEAPONTYPE.GUN);
    }

    // Update is called once per frame
    void Update()
    {
        PlayerController plr = _roundManager.getPlayer();
        GunContainer.color = ColorComponent.m_colorList[(int)plr.gunPrefab.GetComponentInChildren<ColorComponent>().GetColor()];
        KnifeContainer.color = ColorComponent.m_colorList[(int)plr.knifePrefab.GetComponentInChildren<ColorComponent>().GetColor()];
        BombContainer.color = ColorComponent.m_colorList[(int)plr.bombPrefab.GetComponentInChildren<ColorComponent>().GetColor()];

        scoreText.text = _roundManager.GetScore().ToString();
    }

    void WeaponChange(WEAPONTYPE type)
	{
        currentGlow.gameObject.SetActive(false);
        CurrentContent.anchorMin = DefaultMinAnchors;
        CurrentContent.anchorMax = DefaultMaxAnchors;
        switch (type)
		{
            case WEAPONTYPE.GUN:
                currentGlow = GunGlow;
                CurrentContent = GunContent;
                break;
            case WEAPONTYPE.KNIFE:
                currentGlow = KnifeGlow;
                CurrentContent = KnifeContent;
                break;
            case WEAPONTYPE.FIRE:
                currentGlow = BombGlow;
                CurrentContent = BombContent;
                break;
        }
        currentGlow.gameObject.SetActive(true);
        CurrentContent.anchorMin = SelectedMinAnchors;
        CurrentContent.anchorMax = SelectedMaxAnchors;
    }

    void PlayBlink(Enemy enemy)
	{
        Debug.Log("Playing anim");
        blinkAnim["BlinkAnim"].wrapMode = WrapMode.Once;
        blinkAnim.Play("BlinkAnim");
	}

    void PlayDeath()
	{
        Debug.Log("Playing death anim");
        anims["LoseOverlayFadein"].wrapMode = WrapMode.Once;
        anims.Play("LoseOverlayFadein");
        Invoke("GoToGameOver", 3.5f);
    }

    void PlayStreakText(Multiplier multiplier)
	{
        Debug.Log("a");
        streakText.text = multiplier.text;
        Debug.Log("b");
        anims["ShowStreakAnim"].wrapMode = WrapMode.Once;
        Debug.Log("c");
        anims.Play("ShowStreakAnim");
	}

    void GoToGameOver()
	{
        GameManager._instance.SendCommand("EndRound");
	}
}
