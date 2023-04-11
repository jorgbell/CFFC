using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : Weapon
{
    public float bombVelocity;

    public GameObject player;

    public GameObject bombPrefab;
    public GameObject explosionPrefab;

    private ParticleSystem particleSystem;

    private void Start()
    {
        m_colorComponent.OnColorChanged.AddListener(OnColorChanged);
        ResetCooldown();
        RoundManager.instance.eBombExploded.AddListener(ResetCooldown);
    }

    private void Update()
    {
        Debug.DrawLine(transform.position, transform.position + transform.forward * 10, Color.magenta);
    }

    private void OnEnable()
    {
        var main = GetComponentInChildren<ParticleSystem>().main;

        main.prewarm = true;
    }

    public override void Attack()
    {
        Debug.Log("chiquita bomba");

        if (!onCooldown) 
        {
            RoundManager.instance.eBombShot.Invoke();
            AudioManager.instance.Play("SFX_throwFire");
            //var main = GetComponentInChildren<ParticleSystem>().main;
            GetComponentInChildren<ParticleSystem>().Stop();
            GetComponentInChildren<ParticleSystem>().Clear();

            explosionPrefab.GetComponentInChildren<ExplosionCollision>().setColor(m_colorComponent);


            changeBombParticleColor(explosionPrefab.GetComponentInChildren<ParticleSystem>().colorOverLifetime);

            GameObject bomb = Instantiate(bombPrefab, transform.position + transform.lossyScale.z * transform.forward, Quaternion.identity);
            bomb.GetComponent<BombProjectileBehaviour>().setExplosionSystem(explosionPrefab, explosionPrefab.GetComponentInChildren<ParticleSystem>());
            changeBombParticleColor(bomb.GetComponentInChildren<ParticleSystem>().colorOverLifetime);

            Rigidbody bombRB = bomb.GetComponent<Rigidbody>();

            bombRB.velocity = player.GetComponent<Rigidbody>().velocity / 2 + bombVelocity * transform.forward;

            onCooldown = true;

            StartCoroutine("CooldownCoroutine");
        }
    }

    void changeHandParticleColor(ParticleSystem.ColorOverLifetimeModule colorModule)
    {
        Gradient gradient = new Gradient();
        {
            GradientColorKey[] colorKeys = new GradientColorKey[2];
            GradientAlphaKey[] alphaKeys = new GradientAlphaKey[1];

            colorKeys[0].color = Color.black;
            colorKeys[0].time = 0f;

            colorKeys[1].color = ColorComponent.m_colorList[(int)GetComponent<ColorComponent>().GetColor()];
            colorKeys[1].time = 1f;

            alphaKeys[0].alpha = 1f;
            alphaKeys[0].time = 0f;

            gradient.SetKeys(colorKeys, alphaKeys);
        }


        colorModule.color = gradient;
    }

    void changeBombParticleColor(ParticleSystem.ColorOverLifetimeModule colorModule)
    {
        Gradient gradient = new Gradient();
        {
            GradientColorKey[] colorKeys = new GradientColorKey[3];
            GradientAlphaKey[] alphaKeys = new GradientAlphaKey[1];

            colorKeys[0].color = Color.white;
            colorKeys[0].time = 0f;

            colorKeys[1].color = ColorComponent.m_colorList[(int)GetComponent<ColorComponent>().GetColor()];
            colorKeys[1].time = 0.224f;

            colorKeys[0].color = Color.black;
            colorKeys[0].time = 1f;

            alphaKeys[0].alpha = 1f;
            alphaKeys[0].time = 0f;

            gradient.SetKeys(colorKeys, alphaKeys);
        }


        colorModule.color = gradient;
    }

    protected override void AttackAnim()
    {
        //throw new System.NotImplementedException();
    }

    protected override void OnColorChanged(ColorType colorType)
    {
        changeHandParticleColor(GetComponentInChildren<ParticleSystem>().colorOverLifetime);
    }

    protected override void ResetCooldown() 
    { 
        onCooldown = false;
        //var main = GetComponentInChildren<ParticleSystem>().main;
        GetComponentInChildren<ParticleSystem>().Play();
    }
}
