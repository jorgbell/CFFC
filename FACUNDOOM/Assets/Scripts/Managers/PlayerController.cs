using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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
    WEAPONTYPE actualWeapon = WEAPONTYPE.GUN;
    [SerializeField]
    GameObject gunPrefab;
    [SerializeField]
    GameObject knifePrefab;
    [SerializeField]
    GameObject bombPrefab;


    void Start()
    {
        if (m_weaponEvent == null)
            m_weaponEvent = new ChangeWeaponEvent();
        m_weaponEvent.AddListener(ChangeWeapon);

        weapons[0] = gunPrefab.GetComponentInChildren<Shoot>();
        weapons[1] = knifePrefab.GetComponentInChildren<Knife>();
        weapons[2] = bombPrefab.GetComponentInChildren<Bomb>();

        m_weaponEvent.Invoke(actualWeapon);

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && m_weaponEvent != null)
            m_weaponEvent.Invoke(WEAPONTYPE.GUN);
        if (Input.GetKeyDown(KeyCode.Alpha2) && m_weaponEvent != null)
            m_weaponEvent.Invoke(WEAPONTYPE.KNIFE);
        if (Input.GetKeyDown(KeyCode.Alpha3) && m_weaponEvent != null)
            m_weaponEvent.Invoke(WEAPONTYPE.FIRE);

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

    public ColorComponent getActualColorComponent() { return weapons[(int)actualWeapon].GetComponent<ColorComponent>(); }

}