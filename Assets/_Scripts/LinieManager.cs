using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Hacemos que para tener este componente tambien exista el manager en conjunto
[RequireComponent(typeof(InputManager))]
public class LinieManager : MonoBehaviour
{

    //Region para variables privadas
    #region Private

    //referencia al player
    [SerializeField] private Player player;
    //Esta lista guardara todas las lineas que se hayan creado
    private List<GameObject> lines;
    //Esta lista guardara las posiciones de todas las lineas
    private List<Vector2> currentLine;
    //Esta variable es para dibujar la linea
    private LineRenderer lineRenderer;
    //Esta variable es para agregar collision a las lineas
    private EdgeCollider2D currentLineEdgeCollider;

    //Referencia al "Physics Material 2D" que ira a la linea
    [SerializeField] private PhysicsMaterial2D physicsMaterial2D;


    //Variables que determinaran el estado del juego si esta dibujado o borrando
    private bool drawing = false;
    private bool erasing = false;

    //Obtener una referencia a la camara principal
    private Camera mainCamera;

    //Referencia al Componente de "Pan"
    private Pan panning;

    //Referencia al objeto de Zoom
    private Zoom zooming;

    //Referencia al inputmanager
    private InputManager inputManager;

    //Esta variable sera la linea que se creara
    private GameObject currentLineObject;

    //Settings
    //Variable que define la distancia que deben de tener cada una de otra
    [SerializeField] private float lineSeparationDistance = 0.3f;
    //Variable que define el grosor de la linea
    [SerializeField] private float lineWidth = .1f;
    //Variable que define el color predeterminado de la linea
    [SerializeField] private Color lineColor = Color.black;
    //Esta variable define la cantidad de vertices al final de cada linea
    [SerializeField] private int lineVertices = 5;

    //NT- estas variables son para configurar los parametros del linerenderer

    //Esta variable controla la velocidad del "SurfaceEffector"
    [SerializeField] private float effectorSpeed = 3f;

    #endregion


    void Awake()
    {
        //Intancia del inputmanager
        inputManager = GetComponent<InputManager>();
        //Asiganacion de la camara principal a nuestra variable
        mainCamera = Camera.main;
        //Obtenemos el componente de Pan y Zoom
        panning = GetComponent<Pan>();
        zooming = GetComponent<Zoom>();
    }

    void OnEnable()
    {
        //Nos suscribimos a todos los eventos anteriores con nuestros propios metodos dentro de este script
        inputManager.OnStartDraw += OnStartDraw;
        inputManager.OnEndDraw += OnEndDraw;
        inputManager.OnStartErase += OnStartErase;
        inputManager.OnEndErase += OnEndErase;
    }

    void OnDisable()
    {
        //Nos desuscribimos a todos los eventos anteriores con nuestros propios metodos dentro de este script

        inputManager.OnStartDraw -= OnStartDraw;
        inputManager.OnEndDraw -= OnEndDraw;
        inputManager.OnStartErase -= OnStartErase;
        inputManager.OnEndErase -= OnEndErase;
    }

    //En el update vemos desde el player si se esta jugnado o no
    void Update()
    {
        if (!player.playing)
        {
            //Si no se esta jugando ejecutamos el pan y el zoom
            panning.PanScreen(GetCurrentScreenPoint());
            zooming.ZoomScreen(GetZoomValue());
        }
    }

    //Con este metodo obtenemos el valor del scroll del mouse
    private float GetZoomValue()
    {
        return inputManager.GetZoom();
    }

    //Metodo para iniciar a dibujar
    public void OnStartDraw()
    {
        if (!erasing)
        {
            //Arranque de una coroutine
            StartCoroutine("Drawing");
        }
    }
    //Metodo para cuando se termine de dibujar
    public void OnEndDraw()
    {
        //Damos por finalizada el dibujo
        drawing = false;
    }
    //Metodo para cuando se inicie ha borrar
    public void OnStartErase()
    {
        //Vemos que no se este dibujando
        if (!drawing)
        {
            //Iniciamos la coroutina
            StartCoroutine("Erasing");
        }
    }
    //Metodo para cuando se termine de borrar
    public void OnEndErase()
    {
        //Al finalizar borrar seteamos la variable "erasing" en false
        erasing = false;
    }

    #region Erasing

    IEnumerator Erasing()
    {
        //Seteamos la variable "erasing" ha true
        erasing = true;
        //Mientras sea true entramos en un while
        while (erasing)
        {
            //obtenemos la posicion del mouse en Pantalla
            Vector2 screenMousePosition = GetCurrentScreenPoint();
            //creamos un GameObject que se recibira desde el script "De utils" que recibe la camara, la posicion del mouse y del layer
            GameObject g = Utils.Raycast(mainCamera, screenMousePosition, 1<<8);
            //hacemos comprobaciones
            if (g != null)
            {
                //Destruimos la linea que se ha recibido
                DestroyLine(g);
            }
            //Retornamos el yield return null
            yield return null;
        }
    }

    private void DestroyLine(GameObject g)
    {
        Destroy(g);
    }

    public Vector2 GetCurrentScreenPoint()
    {
        return inputManager.GetMousePosition();
    }

    #endregion

    #region Drawing

    //Coroutine para el evento de dibujado
    IEnumerator Drawing()
    {
        //Seteamos la variable de dibujo en true
        drawing = true;
        //Mandamos ha llamar ha un metodo de ayuda que indica el inicio de la linea
        StartLine();
        //Hacemos un loopp que se llamara mientras la variable "drawing" este en true
        while (drawing)
        {
            //Hacemos un "Return null" de parte de la coroutine
            yield return null;
            //Mandamos ha llamar a otro metodo de ayuda el cual este definira el inicio de la linea, recibe un Vector2 como parametro
            //El parametro que se le pasara sera el de la posicion del mouse
            AddPoint(GetCurrentWorldPoint());
        }
        //Metodo de ayuda para determina el fin de la linea
        EndLine();
    }

    //Inicio de la linea determinado
    private void StartLine()
    {
        //Creacion de la nueva linea
        //Creamos la instancia para la lista de las posiciones de nuestra lineas
        currentLine = new List<Vector2>();
        //Creamos un nuevo Gameobject instanciado
        currentLineObject = new GameObject();
        //Al objeto creado le asignamos un nombre
        currentLineObject.name = "Line";
        //Hacemos que este nuevo gameobject sea hijo del actual gameobject que tenga este script
        currentLineObject.transform.parent = transform;
        //Asignamos a la variable del "LineRenderer" el objeto que creamos añadiendole el componente de "LineRenderer"
        lineRenderer = currentLineObject.AddComponent<LineRenderer>();
        //Asignamos a la variable del "Edgecollider" el objeto creado añadiendole el componente de "EdgeCollider2D"
        currentLineEdgeCollider = currentLineObject.AddComponent<EdgeCollider2D>();
        //Aqui asigamos un "PhysicMaterial" a la linea
        currentLineEdgeCollider.sharedMaterial = physicsMaterial2D;
        //Aqui decimos que se usara un effector
        currentLineEdgeCollider.usedByEffector = true;
        //Aqui añadimos el componente de "SurfaceEffector2D"
        SurfaceEffector2D currentEffector = currentLineObject.AddComponent<SurfaceEffector2D>();
        //Seteamos la velocidad del Effector
        currentEffector.speed = effectorSpeed;

        //Start Set Settings

        //Estas son las cantidad de vertices que tiene la linea al inicio
        lineRenderer.positionCount = 0;
        //Esta propiedad determina el ancho de la linea al inicio
        lineRenderer.startWidth = lineWidth;
        //Esta propiedad determina el ancho de la linea al final
        lineRenderer.endWidth = lineWidth;
        //Seteamos la cantidad de vertices que tendra una linea
        lineRenderer.numCapVertices = lineVertices;
        //Necesitamos que estas tengan un color por lo tanto debemos asiganr un color
        lineRenderer.material = new Material(Shader.Find("Particles/Standard Unlit"));
        //habiendo asigando el material ahora podemos cambiar el color
        lineRenderer.startColor = lineColor;
        lineRenderer.endColor = lineColor;
        //Ahora setearemos cuanto grosor tendra nuestro collider
        currentLineEdgeCollider.edgeRadius = .25f;
        //Aqui le decimos a la linea la posicion del layer en la que estara asiganada
        currentLineObject.layer = 1<<3;
    }

    //Inicio de la linea
    private void AddPoint(Vector2 point)
    {
        //Obtenemos un bool de un metodo de ayuda pasandole el parametro que ya recibe este parametro
        if (PlacePoint(point))
        {
            //de ser asi se añade una nueva linea
            currentLine.Add(point);
            //Se suma una posicion a la linea
            lineRenderer.positionCount++;
            //y se coloca la posicion
            lineRenderer.SetPosition(lineRenderer.positionCount - 1, point);
        }
    }

    //Final de la linea
    private void EndLine()
    {
        //Vemos que si la linea es la 1
        if (currentLine.Count == 1)
        {
            //Destruimos la linea actual
            DestroyLine(currentLineObject);
        }
        else
        {
            //Al final al collider le pasamos la linea dibujada, para que setee los puntos del collider
            currentLineEdgeCollider.SetPoints(currentLine);
        }
    }


    private bool PlacePoint(Vector2 point)
    {
        //Vemos que si la cantidad de linea es 0 envie true
        if (currentLine.Count == 0) return true;
        //Si la distancia de estos es menor a la de separacion seteada
        if (Vector2.Distance(point, currentLine[currentLine.Count - 1]) < lineSeparationDistance)
        {
            //Enviamos False
            return false;
        }
        //De no ser asi todo esto igualmente enviamos true
        return true;
    }

    #endregion

    //Obtenemos la posicion del mouse
    private Vector2 GetCurrentWorldPoint()
    {
        //Retornamos el metoodo de la camara "ScrrenToWorldPoint()" que recibe un Vector2
        //Como parametro le pasamos la posicion del mouse
        return mainCamera.ScreenToWorldPoint(inputManager.GetMousePosition());
    }
}
