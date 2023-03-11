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
    Animation blinkAnim;
    void Start()
    {
        _roundManager = RoundManager.instance;
        _roundManager.getPlayer().m_weaponEvent.AddListener(WeaponChange);
        _roundManager.eWrongAnswer.AddListener(PlayBlink);
        currentGlow = GunGlow;
    }

    // Update is called once per frame
    void Update()
    {
        PlayerController plr = _roundManager.getPlayer();
        GunContainer.color = ColorComponent.m_colorList[(int)plr.gunPrefab.GetComponentInChildren<ColorComponent>().GetColor()];
        KnifeContainer.color = ColorComponent.m_colorList[(int)plr.knifePrefab.GetComponentInChildren<ColorComponent>().GetColor()];
        BombContainer.color = ColorComponent.m_colorList[(int)plr.bombPrefab.GetComponentInChildren<ColorComponent>().GetColor()];
    }

    void WeaponChange(WEAPONTYPE type)
	{
        currentGlow.gameObject.SetActive(false);
		switch (type)
		{
            case WEAPONTYPE.GUN:
                currentGlow = GunGlow;
                break;
            case WEAPONTYPE.KNIFE:
                currentGlow = KnifeGlow;
                break;
            case WEAPONTYPE.FIRE:
                currentGlow = BombGlow;
                break;
		}
        currentGlow.gameObject.SetActive(true);
	}

    void PlayBlink(Enemy enemy)
	{
        Debug.Log("Playing anim");
        blinkAnim.Play();
	}
}
