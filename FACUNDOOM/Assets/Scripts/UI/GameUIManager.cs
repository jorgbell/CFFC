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
	[SerializeField]
	TMPro.TextMeshProUGUI countdownText;


	[SerializeField]
	GameObject PauseOverlay;
	[SerializeField]
	Slider SensXSldr;
	[SerializeField]
	Slider SensYSldr;
	[SerializeField]
	Slider VolumeSldr;
	[SerializeField]
	Slider FOVSldr;

	[SerializeField]
	Image VignetteOverlay;
	[SerializeField]
	float maxVignetteOpacity = 0.5f;

	public Camera CurCam;


	Vector2 DefaultMinAnchors = new(0.0f, 0.0f);
	Vector2 SelectedMinAnchors = new(0.05f, 0.0f);
	Vector2 DefaultMaxAnchors = new(0.9f, 1.0f);
	Vector2 SelectedMaxAnchors = new(0.95f, 1.0f);
	void Start()
	{
		_roundManager = RoundManager.instance;
		if (_roundManager)
		{
			_roundManager.getPlayer().m_weaponEvent.AddListener(WeaponChange);
			_roundManager.eWrongAnswer.AddListener(PlayBlink);
			_roundManager.ePlayerDied.AddListener(PlayDeath);
			_roundManager.eMultiplier.AddListener(PlayStreakText);
			_roundManager.eRandomizeColorsCountdown.AddListener(StartCountdown);
		}

		currentGlow = GunGlow;
		CurrentContent = GunContent;
		WeaponChange(WEAPONTYPE.GUN);
	}

	// Update is called once per frame
	void Update()
	{
		if (_roundManager)
		{
			PlayerController plr = _roundManager.getPlayer();
			GunContainer.color = ColorComponent.m_colorList[(int)plr.gunPrefab.GetComponentInChildren<ColorComponent>().GetColor()];
			KnifeContainer.color = ColorComponent.m_colorList[(int)plr.knifePrefab.GetComponentInChildren<ColorComponent>().GetColor()];
			BombContainer.color = ColorComponent.m_colorList[(int)plr.bombPrefab.GetComponentInChildren<ColorComponent>().GetColor()];

			scoreText.text = _roundManager.GetScore().ToString();

			Color curColor = VignetteOverlay.color;
			curColor.a = maxVignetteOpacity - maxVignetteOpacity * plr.GetComponent<PlayerHealth>().HealthPercentage;
			VignetteOverlay.color = curColor;
		}

		if (Input.GetKeyDown(KeyCode.Escape))
		{
			//Toggle pause menu
			TogglePause();
		}
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
		streakText.text = multiplier.text;
		anims["ShowStreakAnim"].wrapMode = WrapMode.Once;
		anims.Play("ShowStreakAnim");
	}

	void GoToGameOver()
	{
		GameManager._instance.SendCommand("EndRound");
	}

	public void GoToMainMenu()
	{
		GameManager._instance.SendCommand("Menu");
	}

	public void TogglePause()
	{
		GameManager._instance.PauseUnpause();
		PauseOverlay.SetActive(!GameManager._instance.IsGameRunning());
		Cursor.lockState = (GameManager._instance.IsGameRunning()) ? CursorLockMode.Locked : CursorLockMode.None;
		//Set values of sliders to current ones
		if (PauseOverlay.activeSelf)
		{
			VolumeSldr.value = AudioListener.volume;
			FOVSldr.value = CurCam.fieldOfView;
			if(_roundManager && _roundManager.getPlayer())
			{
				SensXSldr.value = _roundManager.getPlayer().GetComponent<PlayerRotation>().xSensitivity;
				SensYSldr.value = _roundManager.getPlayer().GetComponent<PlayerRotation>().ySensitivity;
			}
			//SensitivitySldr.value = 
		}

	}

	public void SetFOV()
	{
		CurCam.fieldOfView = FOVSldr.value;
	}

	public void SetVol()
	{
		AudioListener.volume = VolumeSldr.value;
	}

	public void SetSens()
	{
		if (_roundManager)
		{
			_roundManager.getPlayer().GetComponent<PlayerRotation>().xSensitivity = SensXSldr.value;
			_roundManager.getPlayer().GetComponent<PlayerRotation>().ySensitivity = SensYSldr.value;
		}
	}

	void StartCountdown()
	{
		anims.Play("ColorCountdown");
		StartCoroutine("SetCountdownText");
	}

	IEnumerator SetCountdownText()
	{
		countdownText.text = "3";
		yield return new WaitForSeconds(1.0f);
		countdownText.text = "2";
		yield return new WaitForSeconds(1.0f);
		countdownText.text = "1";
	}
}
