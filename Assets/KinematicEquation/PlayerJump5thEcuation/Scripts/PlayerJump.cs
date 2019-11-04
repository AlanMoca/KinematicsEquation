using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    // s - Displacement
    // u - initial velocity
    // v - final velocity
    // a - acceleration                                                 //Para simular la gravedad usaremos a = (v - u)/t (que querremos que caiga el jugador)   =>   g = -u/3
    // t - time

    //Un jugador quiere tener un desplazamiento de 3m en un mundo que tiene una gravedad que se toma como la pura aceleración de 20 m/s^2. Cuál sería su velocidad inicial?
    //Podemos asumir que la aceleración será negativa porque armando el diagrama de fuerzas la gravedad irá en sentido contrario al salto del jugador y que su velocidad final será cero porque es el punto límite. Sabiendo eso:
    // s = 3[m]
    // u = ?
    // v = 0[m/s]
    // a = -20[m/s^2]
    // t = ?

    //Ya que sólo nos piden la velocidad inicial y no el tiempo, podemos hacer uso sólo de la 5th ecuation of kinematic and that is: 
    //                          v^2 = u^2 + 2as;

    private float s = 3.0f;
    private float ss = 0.75f;
    private float u = 0.0f;     //Sólo para definirla.
    private float v = 0.0f;                                            //Tener cuidado con dejar variables publicas o privadas serializadas porque salen otros valores!
    private float a = -9.81f;
    private float aa = -9.81f * 2f;
    private float uu = 0.0f;

    private void fivethEcuationKinematic()
    {
        //u = Mathf.Sqrt( ( Mathf.Pow( v, 2 ) - 2 * ( a * s ) ) );                   //Más costosa
        u = Mathf.Sqrt( v * v - 2 * a * s );                                     //Menos costosa
        Debug.Log( u );
        uu = Mathf.Sqrt( v * v - 2 * aa * ss );
        Debug.Log( uu );
    }

    private void Start()
    {
        fivethEcuationKinematic();
    }

    private void Update()
    {
        if ( Input.GetKeyDown( KeyCode.Space ) )
        {
            gameObject.GetComponent<Rigidbody>().velocity = new Vector3( 0, u, 0 );
        }
    }

}
