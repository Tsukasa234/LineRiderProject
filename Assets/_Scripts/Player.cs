using UnityEngine;

public class Player : MonoBehaviour
{

    //Referencia a InputAction para el player
    private PlayerControls playerControls;
    //Obtenemos la referencia al Rigidbody
    private Rigidbody2D _rb;
    //Creamos una variable para poder iniciar el juego
    public bool playing {get; private set;}

    //Obtenemos una referencia a nuestro script de "CameraFollow"
    private CameraFollow cameraFollow;

    //Creamos una variable para definir siempre la posicion inicial del player
    private Vector3 startPosition;
    //Creamos una variable para definir la rotacion inicial
    private Quaternion startRotation;

    //En el Awake instanciamos los controles, el Rigidbody, tambien obtenemos la posicion inicial del player y si rotation
    //y el componente de "CameraFollow" de nuestra camara principal
    void Awake()
    {
        playing = false;
        cameraFollow = Camera.main.GetComponent<CameraFollow>();
        playerControls = new PlayerControls();
        _rb = GetComponent<Rigidbody2D>();
        startPosition = transform.position;
        startRotation = transform.rotation;
    }

    //Activamos los controles
    void OnEnable()
    {
        playerControls.Enable();
    }
    //Desactivamos los controles
    void OnDisable()
    {
        playerControls.Disable();
    }

    // En el Start al evento Performed lo suscribimos a nuestro metodo
    void Start()
    {
        playerControls.Player.Space.performed += _ => StartGame();
    }

    //Este metodo hara el trabajo del iniciar el juego y de finalizarlo
    private void StartGame()
    {
        //Asignamos esta variable, cuando sea true sera false y vice-versa
        playing = !playing;
        //Entramos en un if
        if (playing)
        {
            //al "RigidBody" le cambiamos el tipo de cuerpo a dinamico para que el player empieze a moverse
            _rb.bodyType = RigidbodyType2D.Dynamic;
            //Cambiamos la interpolacion del "RegidBody"
            _rb.interpolation = RigidbodyInterpolation2D.Interpolate;
            //Activamos el componente de "CameraFollow"
            cameraFollow.enabled = true;
        }
        else
        {   
            //Cuando el juego finaliza cambiamos el tipo de cuerpo del RigidBody" a Statico nuevamente
            _rb.bodyType = RigidbodyType2D.Static;
            //y Devolvemos al player ha su posicion original
            transform.position = startPosition;
            //Devolvemos el player ha su rotacion inicial
            transform.rotation = startRotation;
            //Centramos la camara en el jugador con su posicion inicial
            cameraFollow.CenterOnTarget();
            //Desactivamos el componente de "CameraFollow"
            cameraFollow.enabled = false;

        }
    }
}
