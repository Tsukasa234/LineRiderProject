using UnityEngine;

public class Pan : MonoBehaviour
{
    //Variable que controla la velocidad del pan
    [SerializeField] private float panSpeed = 2f;
    //Obtenemos la referencia al transform de la camara
    private Transform mainCamera;


    void Awake()
    {   
        //Asignamos la referencia de la camara
        mainCamera = Camera.main.transform;
    }

    //y creamos un metodo que hara el pan, como parametro recibe la posicion del mouse
    public void PanScreen(Vector2 mouseScreenPosition)
    {
        //obtenemos exactamente la posicion del mouse atravez de un metodo que indica hacia que direcion se hara el pan
        Vector2 direction = PanDirection(mouseScreenPosition);
        //hacemos que la posicion de la camara sea igual a un Vector3 interpolado(Lerp) con la posicion de la camara, la direcion sumada
        //La posicion de la camara, y la velocida de pan * Time.deltaTime
        mainCamera.position = Vector3.Lerp(mainCamera.position, (Vector3)direction + mainCamera.position, Time.deltaTime * panSpeed);
    }

    //Este metodo se encarga de definir la direccion del pan, recibe como parametro la posicion del mouse en pantalla
    private Vector2 PanDirection(Vector2 mouseScreenPosition)
    {
        //Creamos un vector2 para la direccion inicializada en 0
        Vector2 direction = Vector2.zero;
        //Vemos que si la posicion del mouse en Y, es mayor o igual al 95% dela altura de la pantalla
        if (mouseScreenPosition.y >= Screen.height * .95f)
        {
            //De ser asi la direccion en Y se incrementara en 1
            direction.y += 1;
        }
        //tambien vemos que si la posicion del mouse en Y es menor o igual al 5% de la altura de nuestra pantalla
        else if (mouseScreenPosition.y <= Screen.height * .05f)
        {
            //La direccion se decrementara en 1
            direction.y -= 1;
        }
        //debemos hacer lo mismo en X, si la posicion de nuestro mouse en menor o igual al 95% del ancho de nuestra pantalla
        if (mouseScreenPosition.x >= Screen.width * .95f)
        {
            //en X se incrementara en 1
            direction.x += 1;
        }
        //Si la posicion del mouse es menor o igual al ancho de nuestra pantalla
        else if (mouseScreenPosition.x <= Screen.width * .05f)
        {
            //en X se decrementara en 1
            direction.x -= 1;
        }

        //Retornamos la direccion
        return direction;
    }
}
