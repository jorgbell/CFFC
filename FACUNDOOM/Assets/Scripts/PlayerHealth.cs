using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField]
    private int m_MaxHealth = 3;

    [SerializeField]
    float m_timeUntilRecover = 2.0f;

    [SerializeField]
    float m_timeUntilFullRecover = 3.0f;

    [SerializeField]
    float m_invulnerabilityTime = 1.0f;

    [SerializeField]
    AnimationCurve recoveryCurvel;

    [SerializeField] //TODO solo para poder ver desde el editor pq somos vagos somos perros
    private int m_currentHealth;

    private float m_lastTakenDamage;
    private int m_healthAtLastHit;

    float TimeSinceLastDamage { get { return Time.time - m_lastTakenDamage;} }
    public float HealthPercentage { get { return (float)m_currentHealth / (float)m_MaxHealth; } }

    enum HealingType
    {
        Constant, //Tardas lo mismo de 1 a 3 que de 2 a 3
        Variable //El tiempo en curarse por completo depende de cuanta vida te falte
    }
    [SerializeField]
    HealingType m_healingType = HealingType.Constant;
    // Start is called before the first frame update
    void Start()
    {
        m_currentHealth = m_MaxHealth;
    }

    //recibe daño y devuelve true si el jugador sigue vivo
    public bool TakeDamage(int damage)
    {
        if(m_currentHealth > 0 && TimeSinceLastDamage > m_invulnerabilityTime)
        {
            m_currentHealth -= damage;
            m_healthAtLastHit = m_currentHealth;
            //Debug.Log("Hit left you at " + m_currentHealth);
            AudioManager.instance.Play("SFX_PlayerHit");
        }
        if(m_currentHealth == 0)
        {
            gameObject.GetComponent<PlayerMovement>().moveSpeed = 0;
            gameObject.GetComponent<PlayerMovement>().jumpForce = 0;
            RoundManager.instance.ePlayerDied.Invoke();
            GameManager._instance.SetPlayerScore(RoundManager.instance.score);

        }
        m_lastTakenDamage = Time.time;
        return m_currentHealth > 0;
    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.H))
        {
            TakeDamage(1);
        }
#endif
        if(m_currentHealth < m_MaxHealth && TimeSinceLastDamage > m_timeUntilRecover)
        {
            //Cuanto tiempo del total esperado llevamos
            float recoveredPercentage = (TimeSinceLastDamage - m_timeUntilRecover) / m_timeUntilFullRecover;
            if (TimeSinceLastDamage > m_timeUntilRecover + m_timeUntilFullRecover)
                m_currentHealth = m_MaxHealth;
            else
            {
                //Porcentaje de la recuperacion de vida que llevamos
                float percentageOfRecovery = recoveryCurvel.Evaluate(recoveredPercentage);
                if (m_healingType == HealingType.Constant)
                {
                    m_currentHealth = m_healthAtLastHit +
                        Mathf.FloorToInt(percentageOfRecovery * (m_MaxHealth-m_healthAtLastHit));
                }
                else if(m_healingType == HealingType.Variable)
                {
                    //ouf math
                }

            }

            //Debug.Log("Healed at " + m_currentHealth);
        }
    }
}
