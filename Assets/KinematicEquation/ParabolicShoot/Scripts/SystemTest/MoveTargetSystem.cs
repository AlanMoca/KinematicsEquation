using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof( TargetInputData ) )]
public class MoveTargetSystem : MonoBehaviour
{
    private TargetInputData playerInput;
    [SerializeField] private LaunchSystem launch;

    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private bool moveTarget = true;
    [SerializeField] private float turnSpeed = 22f;

    void Awake() {
        playerInput = GetComponent<TargetInputData>();
    }

    private void Start()
    {
        CoroutineMoveTarget();
    }

    public void CoroutineMoveTarget()
    {
        StartCoroutine( MoveTarget() );
    }

    private IEnumerator MoveTarget()
    {
        while ( moveTarget )
        {
            yield return new WaitForSeconds( 0 );
            TargetTranslate( playerInput.Horizontal, playerInput.Vertical );
        }
    }

    private void TargetTranslate( float horizontal, float vertical )
    {
        transform.Translate( new Vector3( horizontal * moveSpeed * Time.deltaTime, 0, vertical * moveSpeed * Time.deltaTime ) );
        //transform.position += new Vector3( playerInput.Horizontal * moveSpeed * Time.deltaTime, 0, playerInput.Vertical * moveSpeed * Time.deltaTime);
        if ( Input.GetKeyDown( KeyCode.Space ) )
        {
            launch.Launch();
        }

    }

}
