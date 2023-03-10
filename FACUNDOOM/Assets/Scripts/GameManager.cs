using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager _instance { get; private set; }
    private void Awake()
    {
        if (_instance != null && _instance != this) Destroy(this.gameObject);
        else
        {
            _instance = this;
            DontDestroyOnLoad(this);
        }
    }
    private void Start()
    {
        
    }

    private void Update()
    {
        
    }

    /// <summary>
    /// llamar cuando se duplique un enemigo y meter efectos en pantalla y ¿restar puntuacion?
    /// </summary>
    public void WrongAnswer()
    {

    }
    
    /// <summary>
    /// quitar vida al jugador (tiene 2 slots discretos) y activa efectos visuales
    /// </summary>
    public void Hit()
    {

    }

    /// <summary>
    /// cambiar el arma activa y ¿cambiar el color del marco del crt segun el arma?
    /// </summary>
    public void ChangeWeapon()
    {

    }

    /// <summary>
    /// activar la ruleta (por definir=
    /// </summary>
    public void Roulette()
    {

    }

    /// <summary>
    /// cambiar escena
    /// </summary>
    public void ChangeScene()
    {

    }

    //--------------------METODOS QUE HAY QUE DISEÑAR-------
    /* public void camaraLenta() -> activar camara lenta mediante input
     * public void increaseVelocity()-> aumentar la velocidad del gameplay, habria que modificar la velocidad de todos los objetos??
     */

}
