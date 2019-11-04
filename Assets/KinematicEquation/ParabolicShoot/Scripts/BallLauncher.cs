using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallLauncher : MonoBehaviour
{


    /*
    
    Sy - Desplazamiento en y.
    Sxz - Desplazamiento en x y z.
    Uup - U en right que sería la componente Uxz del vector U.
    Uright - U en right que sería la componente Uxz del vector U.
    Uxz - Componente inicial XZ del vector U
    Uy - Componente inicial Y del vector U
    time - tup + tdown

    */

    public Rigidbody ball;
    public Transform target;

    public float h = 25;                    //El limite máximo de lanzamiento vertical de la pelota
    public float gravity = -18;

    public bool debugPath = false;

    public Rigidbody bulletPrefab;
    public Transform spawnPoint;
    public Transform canon;
    public LineRenderer lineVisual;
    public int lineSegment;

    //VariablesWithMouse
    public GameObject cursor;
    public LayerMask layer;
    private Camera cam; //(settearla a la main en el start)

    private void Start()
    {
        cam = Camera.main;
        
        ball.useGravity = false;
    }

    private void Update()
    {
        if ( Input.GetKeyDown( KeyCode.Space ) )
        {
            Launch();
        }

        if ( debugPath )
        {
            DrawPath();
        }

        //if ( Input.GetMouseButtonDown( 0 ) )
        {
            LaunchWithMouse();
        }

    }

    //Opción 1:
    private Vector3 CalculateLaunchVelocity()
    {
        float Sy = target.position.y - ball.position.y;                                                                 //Si lo miras como el diagrama o bien 2D, la pelota está en el origen y target sólo arriba. 
        Vector3 Sxz = new Vector3( target.position.x - ball.position.x, 0, target.position.z - ball.position.z );        // Lo calculamos en x y z. Ya tenemos los vectores de distancia...

        //Descomponemos la velocidad inicial en sus componentes Vy y Vx que serían lo equivalente a Uup y Uright. Ya habiendolas obteniendo usaremos pitagoras para sacar el vector de la velocidad inicial (u).
        Vector3 Uy = Vector3.up * Mathf.Sqrt( -2 * gravity * h ); 
        Vector3 Uxz = Sxz / ( Mathf.Sqrt( ( -2 * h ) / ( gravity ) ) + Mathf.Sqrt( ( 2 * ( Sy - h ) ) / ( gravity ) ) );

        return Uxz + Uy;
        //return Uxz + Uy * Mathf.Sign(gravity);                                                                          //Para parabolas inversas con h negativa y gravedad positiva.
       
    }

    //Opción 2:
    private LaunchData CalculateLaunchData()
    {
        float Sy = target.position.y - ball.position.y;
        Vector3 Sxz = new Vector3( target.position.x - ball.position.x, 0, target.position.z - ball.position.z );

        Vector3 Uy = Vector3.up * Mathf.Sqrt( -2 * gravity * h );

        //time es la suma de tup y tdown que es el tiempo que tarda en transcurrir. Bien es la parte divisora de Uright, lo separamos para poder tomar los distintos parametros de tiempo cuando se lanza! :D
        float time = ( Mathf.Sqrt( ( -2 * h ) / ( gravity ) ) + Mathf.Sqrt( ( 2 * ( Sy - h ) ) / ( gravity ) ) );                   //Según yo es el tiempo totar que tarda la pelota en llegar al target!
        Vector3 Uxz = Sxz / time;

        //LaunchData launch = new LaunchData( Uxz + Uy, time );
        //return launch;

        return new LaunchData( Uxz + Uy, time);

    }

    private void Launch()
    {
        Physics.gravity = Vector3.up * gravity;                 //Le decimos que aplique esta gravedad a todos los rigidbodies de la escena!!! D: D: D: (gravedad = -18)
        ball.useGravity = true;

        //Opcion 1: Sale lapelota sin dibujar trayectoria
        //ball.velocity = CalculateLaunchVelocity();              //Con esta instruccion ya simplemente pasamos al rigidbody lo que debe hacer! :O
        //Debug.Log( "Vector U" + CalculateLaunchVelocity() );

        //Opcion 2: Sale lapelota dibujando la trayectoria gracias a time!
        ball.velocity = CalculateLaunchData().initialVelocity;              //Con esta instruccion ya simplemente pasamos al rigidbody lo que debe hacer! :O
        Debug.Log( "Vector U" + CalculateLaunchData() );
    }

    private void LaunchWithMouse()
    {
        
        Ray camRay = cam.ScreenPointToRay( Input.mousePosition );
        RaycastHit hit;

        if ( Physics.Raycast( camRay, out hit, 100f, layer ) )
        {
            cursor.SetActive( true );
            cursor.transform.position = hit.point + Vector3.up * 0.1f;
            canon.rotation = Quaternion.LookRotation( cursor.transform.position);

            if ( Input.GetMouseButtonDown( 0 ) )
            {

                Rigidbody ballRb = Instantiate( bulletPrefab, spawnPoint.position, Quaternion.identity );
                Physics.gravity = Vector3.up * gravity;                             //La física aplicada a todos los rigidbodies en la escena!!! D: D: D: (gravedad = -18)
                ballRb.useGravity = true;
                ballRb.velocity = CalculateLaunchData().initialVelocity;              //Con esta instruccion ya simplemente pasamos al rigidbody lo que debe hacer! :O
                Debug.Log( "Vector U" + CalculateLaunchData() );
                //ball.useGravity = true;
                //ball.velocity = CalculateLaunchData().initialVelocity;              //Con esta instruccion ya simplemente pasamos al rigidbody lo que debe hacer! :O
                //Debug.Log( "Vector U" + CalculateLaunchData() );
            }
        }

    }

    private void DrawPath()
    {
        LaunchData launchData = CalculateLaunchData();
        Vector3 previousDrawPoint = ball.position;

        int resolution = 30;        //Cantidad de puntos en la trayectoria de la posición inicial a la final que calculará
        float simulationTime;       //Guardará la fracción de tiempo actual de la trayectoria (bien la siguiente de la inicial)
        Vector3 displacement;       //Por la 3th ecuacion obtendremos el desplazamiento en cada punto de fraccion de tiempo.
        Vector3 drawPoint;          //Recibirá la posición "de la siguiente pelota" tiene una más que el transform actual de la pelota :V

        lineVisual.positionCount = resolution + 1;

        for ( int i = 1; i <= resolution; i++ )
        {
            simulationTime = i / (float)resolution * launchData.timeToTarget;                                                              //Esto nos da una variable que va de cero al tiempo general. (Así sólo dibuja hasta el target y no al infinito). Vas 1/30 al 30/30 para limitar las veces que dibujarás y a la vez multiplicas eso por el tiempo total para que ese tiempo total se fraccione 30 veces!! respecto a cada valor en i!!! D: D:
            displacement = launchData.initialVelocity * simulationTime + (Vector3.up * gravity * simulationTime * simulationTime ) /2f;    // Usamos la 3th ecuation of kinematic s = ut + (at^2)/2 para obtener el valor del desplazamiento en cada uno de los distintos puntos gracias a simulationTime que ya está fraccionado en cada uno de los distintos puntos! D:
            drawPoint = ball.position + displacement;                                                                                      // displacement guarda la posición de cada uno de los fraccionamientos en el tiempo de simulationTime. Y a diferencia de multiplicarlo que puede fraccionar o doblar, al sumarlo con la posición de la pelota en cada fracción de tiempo, pasamos el valor que la pelota tendrá en la siguiente posición! Ya que la pelota está en i=0 pero nuestro for empieza un punto después en i=1!! D: D:
            Debug.DrawLine( previousDrawPoint, drawPoint, Color.red );                                                  //Y aquí se nota, previousDrawPoint guarda la posición i=0 de la pelota y drawPoint en realidad es el actual pero sólo del for y así dibujamos desde donde está la pelota sino iniciaríamos en el siguiente punto el dibujado!
            previousDrawPoint = drawPoint;                                      //Sólo actualizamos para el siguiente dibujado! :3

            lineVisual.SetPosition( i, previousDrawPoint );

        }

    }

}

struct LaunchData {

    public readonly Vector3 initialVelocity;
    public readonly float timeToTarget;

    public LaunchData( Vector3 _initialVelocity, float _timeToTarget )
    {
        this.initialVelocity = _initialVelocity;
        this.timeToTarget = _timeToTarget;
    }

}
