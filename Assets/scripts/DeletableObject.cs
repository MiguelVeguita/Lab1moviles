using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DeletableObject : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "imagen4")
        {
            // Eliminar el objeto
            Destroy(gameObject);
            Debug.Log("Objeto eliminado por colisión.");
        }
    }
    private void OnTriggerEnter(Collider collision)
    {
       
    }
}
