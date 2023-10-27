using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogLeadDrawer : MonoBehaviour
{
    // Start is called before the first frame update
    LineRenderer lineRenderer;
    Vector3 dogPos, cameraPos;

    [SerializeField]
    GameObject dogObject;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.startWidth = .05f;
        lineRenderer.endWidth = .05f;
    }

    void Update() {
        print(dogObject);
        dogPos = ModifyDogPos(dogObject.GetComponent<Transform>().position);
        cameraPos = Camera.current.ViewportToWorldPoint(new Vector3(0.5f, 0f, Camera.current.nearClipPlane));
        lineRenderer.SetPosition(0, dogPos);
        lineRenderer.SetPosition(1, cameraPos);
    }

    Vector3 ModifyDogPos(Vector3 originalDogPos)
    {
        float z = originalDogPos.z + 0.3f;
        return new Vector3(originalDogPos.x, originalDogPos.y, z);
    }
}
