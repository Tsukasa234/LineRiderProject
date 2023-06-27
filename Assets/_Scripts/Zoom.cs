using UnityEngine;

public class Zoom : MonoBehaviour
{
    //Variable para controlar la velocidad en la que se hace zoom
    [SerializeField] private float zoomSpeed = 2f;
    //variable para controlar cuanto se puede hacer zoom cercano
    [SerializeField] private float zoomInMax = 1;
    //Varaible para contrlar cuanto se puede hacer zoom lejano
    [SerializeField] private float zoomOutMax = 15;
    //Referencia a la camara principal
    private Camera mainCamera;
    //Variable para regresar el zoom de la camara a su posicion inicial
    private float startingZPosition;

    void Awake()
    {
        //Obtenemos la referencia a la camara principal
        mainCamera = Camera.main;
        //Seteamos la posicion inicial de la camara en el zoom
        startingZPosition = mainCamera.transform.position.z;
    }

    //Este metodo hara el trabajo de hacer el zoom, recibe el incremente que es el valor del scroll
    public void ZoomScreen(float increment)
    {
        //Vemos que si el incremente es 0 no haga nada
        if (increment  == 0)
        {
            return;
        }

        //de no ser 0, hacemos que el "OrtographicSize" de la camara sea igual al incremente, pero limitandolo al zoominmax y zoomoutmax
        //Hacemos uso del Clamp para eso
        float target = Mathf.Clamp(mainCamera.orthographicSize + increment, zoomInMax, zoomOutMax);
        //Asignamos ese valor al "orthographicSize", usando "Lerp" de Mathf
        mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, target, Time.deltaTime * zoomSpeed);
    }
}
