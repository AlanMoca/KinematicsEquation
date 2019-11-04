using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class NewInputSystem : MonoBehaviour
{
    public LaunchSystem launch;

    //WithActions
    [SerializeField] private InputAction miMovimiento;
    [SerializeField] private InputAction miDisparo;

    //MapaDeAcciones
    [SerializeField] private InputActionAsset AssetEntrada;

    private InputActionMap mapa;
    private InputAction miMovimientoMapa;
    private InputAction miDisparoMapa;

    private void Awake()
    {
        //**** WithInputAction ****
        WithMap();

    }

    private void OnEnable()
    {
        //**** WithInputAction ****
        //EnableInputsActions();

        //**** WithInputMap ****
        EnableInputsMap();

    }

    private void OnDisable()
    {
        //**** WithInputAction ****
        //DisableInputsActions();

        //**** WithInputMap ****
        DisableInputsMap();

    }

    // Update is called once per frame
    void Update()
    {
        //WithInputKey()
    }

    #region InputSystem with Keys
    private void WithKey()
    {
        if ( Keyboard.current.wKey.isPressed )
        {
            transform.Translate( Vector3.forward );
        }
        if ( Keyboard.current.sKey.isPressed )
        {
            transform.Translate( -Vector3.forward );
        }
        if ( Keyboard.current.dKey.isPressed )
        {
            transform.Translate( Vector3.right );
        }
        if ( Keyboard.current.aKey.isPressed )
        {
            transform.Translate( -Vector3.right );
        }
        if ( Keyboard.current.spaceKey.wasPressedThisFrame )
        {
            launch.Launch();
        }
    }

    #endregion

    #region InputSystem With Actions

    private void Movimiento( InputAction.CallbackContext context )
    {
        Vector2 direccion = context.ReadValue<Vector2>();
        Debug.Log( direccion );
        Vector3 vectorDir = new Vector3( direccion.x * 10f * Time.deltaTime, 0, direccion.y * 10f * Time.deltaTime );

        transform.Translate( vectorDir );
    }

    private void MakeLaunch( InputAction.CallbackContext context )
    {
        launch.Launch();
    }

    private void EnableInputsActions()
    {
        //miMovimiento.continuos = true;                        //Lo quitaron pero con esto funcionaría mejor los actions para que fuese continuo y no apretando.
        miMovimiento.performed += Movimiento;
        miMovimiento.Enable();

        miDisparo.performed += MakeLaunch;
        miDisparo.Enable();
    }

    private void DisableInputsActions()
    {
        miMovimiento.performed -= Movimiento;
        miMovimiento.Disable();

        miDisparo.performed -= MakeLaunch;
        miDisparo.Disable();
    }

    #endregion

    #region InputSystem With Map

    private void WithMap()
    {
        //Obtenemos el mapa del asset
        mapa = AssetEntrada.GetActionMap( "Canon" );

        miMovimientoMapa = mapa.GetAction( "Movimiento" );
        miDisparoMapa = mapa.GetAction( "Launch" );

    }

    private void EnableInputsMap()
    {
        //miMovimientoMapa.continuos = true;                        //Lo quitaron pero con esto funcionaría mejor los actions para que fuese continuo y no apretando.
        miMovimiento.performed += Movimiento;
        miMovimiento.Enable();

        miDisparo.performed += MakeLaunch;
        miDisparo.Enable();
    }

    private void DisableInputsMap()
    {
        miMovimiento.performed -= Movimiento;
        miMovimiento.Disable();

        miDisparo.performed -= MakeLaunch;
        miDisparo.Disable();
    }

    #endregion

}
