using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hit : MonoBehaviour
{
   
    private void OnCollisionEnter(Collision collision)
    {


        if (collision.gameObject.TryGetComponent(out Hurtable hurtable) )
        {
            Debug.Log("Hitto" + collision.gameObject.name);
            hurtable.Hurt();
        }
    }
}
