using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    Weapon currentWeapon;
    [SerializeField]
    Shoot gunWeapon;
    //[SerializeField]
    //Knife gunWeapon;
    //[SerializeField]
    //Bomb gunWeapon;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		if (Input.GetMouseButtonDown(0))
		{
            currentWeapon.Attack();
		}
    }
}
