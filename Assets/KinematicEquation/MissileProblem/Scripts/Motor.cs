using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Motor : MonoBehaviour
{
    public float initialVelocity;
    public float acceleration;
    private float currentVelocity;

    private void Start()
    {
        currentVelocity = initialVelocity;
    }

    private void FixedUpdate()
    {
        if ( Time.fixedTime < Timer.predictedTime )                                                         // Esto quiere decir que se detendrá cuando choquen! D:
        {
            currentVelocity += acceleration * Time.deltaTime;                                               //La velocidad irá aumentando linealmente respecto a la aceleración
            transform.Translate( Vector3.right * currentVelocity * Time.fixedDeltaTime );                   //Vector normal unitario a la derecha por la velocidad y en segundos normales sólo que aplicados a la física
        }
        
    }

}
