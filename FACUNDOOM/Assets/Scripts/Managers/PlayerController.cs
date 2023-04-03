using SnapshotShaders.URP;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

public enum WEAPONTYPE
{
    GUN, KNIFE, FIRE
}
public class ChangeWeaponEvent : UnityEvent<WEAPONTYPE>
{
}
public class PlayerController : MonoBehaviour
{
    public ChangeWeaponEvent m_weaponEvent;

    // Start is called before the first frame update
    Weapon[] weapons = new Weapon[3];
    [SerializeField]
    List<GameObject> gunModels = new List<GameObject>();
    [SerializeField]
    List<GameObject> swordModels = new List<GameObject>();
    WEAPONTYPE actualWeapon = WEAPONTYPE.GUN;
    public GameObject gunPrefab;
    public GameObject knifePrefab;
    public GameObject bombPrefab;

    RoundManager roundManager;

    private void Awake()
    {
        if (m_weaponEvent == null)
            m_weaponEvent = new ChangeWeaponEvent();
    }

    void Start()
    {
        m_weaponEvent.AddListener(ChangeWeapon);

        weapons[0] = gunPrefab.GetComponentInChildren<Shoot>();
        weapons[1] = knifePrefab.GetComponentInChildren<Knife>();
        weapons[2] = bombPrefab.GetComponentInChildren<Bomb>();

        m_weaponEvent.Invoke(actualWeapon);

        roundManager = RoundManager.instance;
        roundManager.eRandomizeColors.AddListener(RandomizeWeaponColors);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager._instance.IsGameRunning())
        {
            if (Input.GetKeyDown(KeyCode.Alpha1) && m_weaponEvent != null)
                m_weaponEvent.Invoke(WEAPONTYPE.GUN);
            if (Input.GetKeyDown(KeyCode.Alpha2) && m_weaponEvent != null)
                m_weaponEvent.Invoke(WEAPONTYPE.KNIFE);
            if (Input.GetKeyDown(KeyCode.Alpha3) && m_weaponEvent != null)
                m_weaponEvent.Invoke(WEAPONTYPE.FIRE);
            
            float mouseScroll = Input.GetAxisRaw("Mouse ScrollWheel");
            if (mouseScroll < 0f)
            {
                int i = (int)actualWeapon;
                actualWeapon = (WEAPONTYPE)((i + 1) % 3);
                m_weaponEvent.Invoke(actualWeapon);

            }
            else if (mouseScroll > 0f)
            {
                int i = (int)actualWeapon;
                actualWeapon = (WEAPONTYPE)((i + -1+3) % 3);
                m_weaponEvent.Invoke(actualWeapon);

            }
            if (Input.GetMouseButtonDown(0))
            {
                weapons[(int)actualWeapon].Attack();
            }

            //DEBUG
            if (Input.GetKeyDown(KeyCode.C))
            {
                ColorComponent weaponColor;
                if (TryGetComponent(out weaponColor))
                {
                    weaponColor.SetColor((ColorType)(((int)weaponColor.GetColor() + 1) % (int)ColorType.lastColor));
                }
            }
        }
    }
    private void ChangeWeapon(WEAPONTYPE type)
    {
        switch (type)
        {
            case WEAPONTYPE.GUN:
                actualWeapon = WEAPONTYPE.GUN;
                gunPrefab.SetActive(true);
                knifePrefab.SetActive(false);
                bombPrefab.SetActive(false);
                break;
            case WEAPONTYPE.KNIFE:
                actualWeapon = WEAPONTYPE.KNIFE;
                gunPrefab.SetActive(false);
                knifePrefab.SetActive(true);
                bombPrefab.SetActive(false);
                break;
            case WEAPONTYPE.FIRE:
                actualWeapon = WEAPONTYPE.FIRE;
                gunPrefab.SetActive(false);
                knifePrefab.SetActive(false);
                bombPrefab.SetActive(true);
                break;
        }
        Debug.Log(actualWeapon);
    }

    void RandomizeWeaponColors()
    {
        //List of possible colors
        ColorType[] colorList = new ColorType[(int)ColorType.lastColor];
        colorList[0] = gunPrefab.GetComponentInChildren<ColorComponent>().GetColor();
        colorList[1] = knifePrefab.GetComponentInChildren<ColorComponent>().GetColor();
        colorList[2] = bombPrefab.GetComponentInChildren<ColorComponent>().GetColor();

        //Shuffle 
        Utilities.ShuffleColorArray(ref colorList);

        //Assign colors to the weapons  
        gunPrefab.GetComponentInChildren<ColorComponent>().SetColor(colorList[0]);
        gunModels.ForEach(sprite => sprite.SetActive(false));
        gunModels[(int)(colorList[0])].SetActive(true);
        knifePrefab.GetComponentInChildren<ColorComponent>().SetColor(colorList[1]);
        swordModels.ForEach(sprite => sprite.SetActive(false));
        swordModels[(int)(colorList[1])].SetActive(true);
        bombPrefab.GetComponentInChildren<ColorComponent>().SetColor(colorList[2]);
    }

    public Color getActualColor()
    {
        ColorType c = ColorType.lastColor;
        switch (actualWeapon)
        {
            case WEAPONTYPE.GUN:
                c = gunPrefab.GetComponentInChildren<ColorComponent>().GetColor();
                break;
            case WEAPONTYPE.KNIFE:
                c = knifePrefab.GetComponentInChildren<ColorComponent>().GetColor();
                break;
            case WEAPONTYPE.FIRE:
                c = bombPrefab.GetComponentInChildren<ColorComponent>().GetColor();
                break;
        }
        return ColorComponent.m_colorListCRT[(int)c];
    }
}
