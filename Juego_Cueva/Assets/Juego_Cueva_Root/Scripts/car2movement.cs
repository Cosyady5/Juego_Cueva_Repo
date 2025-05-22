using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class car2movement : MonoBehaviour
{
    public bool inverse;
    public float speed;
    public float posx;
    public float pos2x;
    Vector3 posicion;

private void Start()
{
    posicion = transform.localPosition;
}
void Update()
{
    transform.Translate(Vector3.left * Time.deltaTime * speed);

    if (inverse)
        {
            if (transform.position.x <= posx)
            {
                transform.position = new Vector3(pos2x, posicion.y, posicion.z);
            }
        }

    else
        {
            if(transform.position.x >= posx)
            {
                transform.position = new Vector3(pos2x, posicion.y, posicion.z);
            }
        }


}
}
