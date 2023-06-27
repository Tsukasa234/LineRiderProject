using UnityEngine;

//La clase sera statica para llamarla de cualquier lugar
public static class Utils
{
    //creamos un metodo que se encargara de obtener el gameobject golpeado por un rayo
    //Recibe 2 parametros que son la camara principal, la posicion del mouse en pantalla y un layer
    public static GameObject Raycast(Camera mainCamera, Vector2 screenPosition, int layer)
    {
        //Creamos un rayo usando el metodo de la camara "ScreenPointToRay()"
        Ray ray  = mainCamera.ScreenPointToRay(screenPosition);
        //creamos el hit, que sera de la interseccion(Por que es un entorno 2D), recibiendo el rayo, el infinito y el layer
        RaycastHit2D hit2D = Physics2D.GetRayIntersection(ray, Mathf.Infinity, layer);
        //Vemos que no sea null la collision del hit
        if (hit2D.collider != null)
        {
            //Retornamos el objeto golpeado
            return hit2D.collider.gameObject;
        }
        //De no golpear nada mandamos null
        return null;
    }
}
