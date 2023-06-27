using UnityEngine;

public class InputManager : MonoBehaviour
{

    //Region para los eventos
    #region Events

    //Delegado y evento cuando se empieza a dibujar
    public delegate void StartDraw();
    public event StartDraw OnStartDraw;
    //Delegado y evento cuando se termina de dibujar
    public delegate void EndDraw();
    public event EndDraw OnEndDraw;
    //Delegado y eevento de cuando se empieza a borrar lo dibujado
    public delegate void StartErase();
    public event StartErase OnStartErase;
    //Delegado y evento de cuando se termina de borrar lo dibujado
    public delegate void EndErase();
    public event EndErase OnEndErase;

    //Referencia a nuestro InputActionMap
    private MouseControls mouseControls;

    #endregion


    
    void Awake()
    {   
        //Instanciamos nuestro inputaction
        mouseControls = new MouseControls();
    }

    void OnEnable()
    {
        //Activamos nuestro Inputaction
        mouseControls.Enable();
    }

    void OnDisable()
    {
        //Desactivamos nuestro inputaction
        mouseControls.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {
        //Suscribmos el eventos de inicio de click a un lambda sin pasar parametros
        mouseControls.Mouse.Click.started += _ =>
        {
            //Comprobamos si el evento tiene suscriptores
            if (OnStartDraw != null)
            {
                //Mandamos ha llamar al evento
                OnStartDraw();
            }
        };
        //Nos suscribimos al evento de cuando se termina de dar click por lamda sin pasar parametros
        mouseControls.Mouse.Click.canceled += _ =>
        {
            //Comprobamos si el evento tiene suscriptores
            if (OnEndDraw != null)
            {
                //Mandamos ha llamar al evento correspondiente
                OnEndDraw();
            }
        };
        //Nos suscribimos al evento de borrado por medio de un lamda sin pasar parametros
        mouseControls.Mouse.Erase.started += _ =>
        {
            //Comprobamos si el evento tiene suscriptores
            if (OnStartErase != null)
            {
                //Mandamos ha llamar al evento correspondiente
                OnStartErase();
            }
        };
        //Nos suscribikmos al evento de cancel del borrado por medio del lamda sin pasar parametros
        mouseControls.Mouse.Erase.canceled += _ =>
        {
            //Comprobamos si el evento tiene suscriptores
            if (OnEndErase != null)
            {
                //Mandamos ha llamar al evento correpondiente
                OnEndErase();
            }
        };

        // Cursor.lockState = CursorLockMode.Confined;
    }

    //Metodo public para obtener Zoom atravez de este manaager
    public float GetZoom()
    {
        return mouseControls.Mouse.Zoom.ReadValue<float>();
    }
    
    //Metodo para obtener la posicion actual del mouse en pantalla
    public Vector2 GetMousePosition()
    {
        return mouseControls.Mouse.ClickPosition.ReadValue<Vector2>();
    }
}
