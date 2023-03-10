using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    public GameObject camera;
    //public GameObject decalPrefab;
    //public ParticleSystem particleSystem;
    public float coolDown;
    public float shotForce;

    public Recoil recoil;
    public float recoilMagnitude;
    public float recoilDuration;

    Transform rotationAxis;
    //AudioSource audioSource;

    private ColorType colorType = ColorType.blue;

    [SerializeField]
    private bool onCooldown = false;

    //public int maxDecalAmount;

    //[SerializeField]
    //List<GameObject> decals;

    //private ScreenShake screenShake;
    [SerializeField]
    Quaternion startingRotation;


    // Start is called before the first frame update
    void Start()
    {
        rotationAxis = GetComponentInParent<Transform>();
        //audioSource = GetComponent<AudioSource>();
        //screenShake = camera.GetComponent<ScreenShake>();

        startingRotation = rotationAxis.localRotation;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0) && !onCooldown)
        {
            //audioSource.Play();
            //screenShake.Shake(0.0f, 0.1f);
            recoil.PushUpwards(recoilMagnitude, recoilDuration);
            //particleSystem.Play();

            castShot();

            StopAllCoroutines();
            StartCoroutine(CooldownCoroutine());
            onCooldown = true;
        }
    }

    private IEnumerator CooldownCoroutine()
    {
        float elapsed = 0.0f;

        while (elapsed < coolDown)
        {
            elapsed += Time.deltaTime;

            if (elapsed / coolDown <= 0.5) rotationAxis.Rotate(-100f * Time.deltaTime, 0, 0, Space.Self);
            else rotationAxis.Rotate(100f * Time.deltaTime, 0, 0, Space.Self);

            yield return null;
        }

        rotationAxis.localRotation = startingRotation;
        onCooldown = false;
    }

    public void castShot() 
    {
        Debug.DrawRay(camera.transform.position, camera.transform.forward * 100, Color.red, 10);

        if (Physics.Raycast(camera.transform.position, camera.transform.forward, out RaycastHit hit, 100))
        {
            //if (hit.collider.TryGetComponent<Rigidbody>(out Rigidbody targetBody)) targetBody.AddForce(camera.transform.forward * shotForce, ForceMode.Impulse);

            if (hit.collider.TryGetComponent<Enemy>(out Enemy targetEnemy)) targetEnemy.Hit(colorType);


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
}
