using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;

public class DogStroll : MonoBehaviour
{
    public GameObject dog;
    private float timer = 3.0f;
    private float interval = 3.0f;
    private float speed = 2.0f;
    private Vector3 moveDirection;

    void Start()
    {
        dog.GetComponent<Collider2D>().enabled = true;
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= interval)
        {
            GenerateRandomVector();
            timer = 0.0f;
        }
        else
        {
            MoveDog();
        }
    }

    void GenerateRandomVector()
    {
        moveDirection = new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)).normalized;
    }

    void MoveDog()
    {
        dog.transform.position += moveDirection * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        Debug.Log("Collider Entered!"); // Debug Code
        if (collider.gameObject.CompareTag("Barrier"))
        {
            Debug.Log("Barrier Collision!"); // Debug Code

            moveDirection = (new Vector3(0.0f, 0.0f) - dog.transform.position).normalized;
            timer = 0.0f;
        }
    }
}
