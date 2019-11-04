using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchSystem : MonoBehaviour
{

    #region Private Variables

    [SerializeField] private Rigidbody ball;
    [SerializeField] private Transform target;
    [SerializeField] private float h = 25;
    [SerializeField] private float gravity = -18;
    [SerializeField] private Transform spawnPoint;

    #endregion

    #region Variables for Draw

    [SerializeField] private LineRenderer lineVisual;
    [SerializeField] private int lineSegment;

    private int resolution = 30;
    private float simulationTime;
    private Vector3 displacement;
    private Vector3 drawPoint;
    private bool debugPath = true;

    #endregion

    private void Start()
    {
        CoroutineDrawPath();
        
    }

    public void Launch()
    {
        MakeLaunch();
        //StartCoroutine( RecalculateLaunch() );
    }

    public void CoroutineDrawPath()
    {
        StartCoroutine( DrawPath() );
    }

    private DataLaunch CalculateLaunchData()
    {
        float Sy = target.position.y - ball.position.y;
        Vector3 Sxz = new Vector3( target.position.x - ball.position.x, 0, target.position.z - ball.position.z );

        Vector3 Uy = Vector3.up * Mathf.Sqrt( -2 * gravity * h );

        float time = ( Mathf.Sqrt( ( -2 * h ) / ( gravity ) ) + Mathf.Sqrt( ( 2 * ( Sy - h ) ) / ( gravity ) ) );
        Vector3 Uxz = Sxz / time;

        return new DataLaunch( Uxz + Uy, time );

    }

    private IEnumerator RecalculateLaunch()
    {
        //while ( debugPath )
        {
            yield return new WaitForSeconds( 0 );
            MakeLaunch();
        }
    }

    private void MakeLaunch()
    {
        Rigidbody ballRb = Instantiate( ball, spawnPoint.position, Quaternion.identity );
        Physics.gravity = Vector3.up * gravity;
        ballRb.useGravity = true;
        ballRb.velocity = CalculateLaunchData().initialVelocity;
        Debug.Log( "Vector U" + CalculateLaunchData() );
    }

    private IEnumerator DrawPath()
    {
        while ( debugPath )
        {
            yield return new WaitForSeconds( 0 );
            DataLaunch launchData = CalculateLaunchData();
            Vector3 previousDrawPoint = ball.position;

            lineVisual.positionCount = resolution + 1;

            for ( int i = 1; i <= resolution; i++ )
            {
                simulationTime = i / (float)resolution * launchData.timeToTarget;
                displacement = launchData.initialVelocity * simulationTime + ( Vector3.up * gravity * simulationTime * simulationTime ) / 2f;
                drawPoint = ball.position + displacement;
                Debug.DrawLine( previousDrawPoint, drawPoint, Color.red );
                previousDrawPoint = drawPoint;

                lineVisual.SetPosition( i, previousDrawPoint );

            }
        }
    }
}

public struct DataLaunch
{
    public readonly Vector3 initialVelocity;
    public readonly float timeToTarget;

    public DataLaunch( Vector3 _initialVelocity, float _timeToTarget )
    {
        this.initialVelocity = _initialVelocity;
        this.timeToTarget = _timeToTarget;
    }

}