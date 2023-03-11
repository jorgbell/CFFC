using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Shoot : Weapon
{
    public GameObject camera;
    //public GameObject decalPrefab;
    //public ParticleSystem particleSystem;
    public float shotForce;

    public Recoil recoil;
    public float recoilMagnitude;
    public float recoilDuration;

    Transform rotationAxis;
    //AudioSource audioSource;

    //public int maxDecalAmount;

    //[SerializeField]
    //List<GameObject> decals;

    //private ScreenShake screenShake;
    [SerializeField]
    Quaternion startingRotation;


    // Start is called before the first frame update
    void Start()
    {
        GetComponent<MeshRenderer>().material.color = Color.red;

        rotationAxis = GetComponentInParent<Transform>();
        //audioSource = GetComponent<AudioSource>();
        //screenShake = camera.GetComponent<ScreenShake>();

        startingRotation = rotationAxis.localRotation;

        if (!TryGetComponent(out m_colorComponent))
        {
            Debug.LogWarning("WEAPON HAS NO COLOR COMPONENT");
        }

        m_colorComponent.OnColorChanged.AddListener(OnColorChanged);
    }

    // Update is called once per frame

    

    public void castShot() 
    {
        Debug.DrawRay(camera.transform.position, camera.transform.forward * 100, Color.red, 10);

        if (Physics.Raycast(camera.transform.position, camera.transform.forward, out RaycastHit hit, 100))
        {
            //if (hit.collider.TryGetComponent<Rigidbody>(out Rigidbody targetBody)) targetBody.AddForce(camera.transform.forward * shotForce, ForceMode.Impulse);

            if (hit.collider.TryGetComponent<Enemy>(out Enemy targetEnemy))
                targetEnemy.Hit(m_colorComponent.GetColor());


            //GameObject decal;

            //if (decals.Count < maxDecalAmount) decal = Instantiate(decalPrefab, hit.point + (hit.normal * 0.01f), Quaternion.identity, hit.transform);

            //else 
            //{
            //    decal = decals[0];
            //    decals.RemoveAt(0);
            //    decal.transform.position = hit.point + (hit.normal * 0.01f);
            //    decal.transform.parent = hit.transform;
            //}

            //decal.transform.forward = -hit.normal;

            //decals.Add(decal);
        }
    }

    public override void Attack()
    {
        //Debug.Log("pum");
        //audioSource.Play();
        //screenShake.Shake(0.0f, 0.1f);
        recoil.PushUpwards(recoilMagnitude, recoilDuration);
        //particleSystem.Play();
        castShot();
    }

    protected override void AttackAnim()
    {
        if (elapsed / coolDown <= 0.5) rotationAxis.Rotate(-100f * Time.deltaTime, 0, 0, Space.Self);
        else rotationAxis.Rotate(100f * Time.deltaTime, 0, 0, Space.Self);
    }

    protected override void ResetAttackAnim()
    {
        rotationAxis.localRotation = startingRotation;
    }
}
