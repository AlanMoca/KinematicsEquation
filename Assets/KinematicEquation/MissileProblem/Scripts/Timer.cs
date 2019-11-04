using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{

    /*
    Calcular el momento (tiempo) exacto donde un misil alcanza a un jugador a 40m de distancia. Todo es en línea recta a través del eje de las "x". Se sabe que la velocidad inicial y la aceleración de cada uno:
          MisilB:    u = 1[m/s]      a = 17[m/s]                                           h = 40[m]
          PlayerA:   u = 19[m/s]     a = 3[m/s]                                 B _________________________ A

        Podemos deducir que el desplazamiento de A cuando esté en A sería equivalente a:                                                                 Sb = Sa + h;

        Se puede remplazar el desplalzamiento en ambos puntos de la ecuación de arriba al hacer uso de la ecuación 3 :                                   S = ut + (at^2)/2
        Nota: El tiempo en A y el tiempo en B, será el mismo porque estarán en el mismo punto.

        aa - aceleración en el punto a.
        ab - aceleración en el punto b.
        ua - velocidad inicial en el punto a.
        ub - velocidad inicial en el punto b.

        Ub * tb + (ab * tb^2)/2  =  Ua * ta + (aa * ta^2)/2 + 2h
        2Ub * t + (ab * t) = 2Ua * t + (aa * t) + 2h
        abt^2 - aat^2 + 2Ubt - 2Uat - 2h = 0;
        (ab - aa)t^2 + 2(Ub - Ua)t - 2h = 0
        (17 - 3)t^2 + 2(1 - 19)t - 2(40) = 0
        14t^2 + 36t - 80 = 0       -->                          Podemos ver que es una ecuación de 2do grado (ax^2 + bx + c = 0).               

        Entonces aplicaremos la formula general, descartando el valor "negativo". Como resultado nos daría 4 segundos que sería el tiempo que tardaría en alcanzar al jugador.


    */

    public static float predictedTime;
    public Motor playerA;
    public Motor misileB;

    public float timeStep = 0.001f;

    private void Start()
    {
        Time.fixedDeltaTime = timeStep;
        
        float h = playerA.transform.position.x - misileB.transform.position.x;

        float a = misileB.acceleration - playerA.acceleration;
        float b = 2 * ( misileB.initialVelocity - playerA.initialVelocity );
        float c = -2 * h;

        //Formula general pasando los valores correspondientes ya habiendo obtenido la ecuación cuadrática. 
        predictedTime = ( -b + ( Mathf.Sqrt( b * b - ( 4 * a * c ) ) ) ) / ( 2 * a );
        Debug.Log( predictedTime );

    }

}
