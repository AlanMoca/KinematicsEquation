using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetInputData : MonoBehaviour
{
    public float Vertical { get; private set; }
    public float Horizontal { get; private set; }

    private void Update() {

        Vertical = Input.GetAxis( "Vertical" );
        Horizontal = Input.GetAxis( "Horizontal" );

    }

}

/*

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent( typeof( TargetInputData ) )]
public class MoveTargetSystem : MonoBehaviour
{
    private TargetInputData playerInput;

    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private bool moveTarget = true;

    void Awake()
    {
        playerInput = GetComponent<TargetInputData>();
    }

    private void Start()
    {
        CoroutineMoveTarget( playerInput.Horizontal, playerInput.Vertical );
    }

    public void CoroutineMoveTarget( float horizontal, float vertical )
    {
        StartCoroutine( MoveTarget( horizontal, vertical ) );
    }

    public void CoroutineMoveTarget( Vector3 targetTranslate )
    {
        StartCoroutine( MoveTarget( targetTranslate ) );
    }

    private IEnumerator MoveTarget( float horizontal, float vertical )
    {
        while ( moveTarget )
        {
            yield return new WaitForSeconds( 0 );
            transform.Translate( new Vector3( horizontal * moveSpeed * Time.deltaTime, 0, vertical * moveSpeed * Time.deltaTime ) );
            //transform.position += new Vector3( playerInput.Horizontal * moveSpeed * Time.deltaTime, 0, playerInput.Vertical * moveSpeed * Time.deltaTime);
        }
    }

    private IEnumerator MoveTarget( Vector3 targetTranslate )
    {
        while ( moveTarget )
        {
            yield return new WaitForSeconds( 0 );
            transform.Translate( targetTranslate );
            //transform.position += targetTranslate;
        }
    }


    private void PlayerMovement()
    {
        //if ( movement.magnitude > 0 ) 
        //{
        //    Quaternion newDirection = Quaternion.LookRotation( movement );
        //    transform.rotation = Quaternion.Slerp( transform.rotation, newDirection, turnSpeed * Time.deltaTime );
        //}
    }


}
*/
