using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private GameObject dogObject;
    [SerializeField]
    private GameObject snackObject;
    [SerializeField]
    private GameObject poopObject;
    [SerializeField]
    private GameObject snackLocation;
    [SerializeField]
    private GameObject poopLocation;
    [SerializeField]
    private GameObject pingPaw;

    private void Start()
    {
        
    }

    private void Update()
    {
        SpawnItem(snackObject, snackLocation);
    }

    public void SpawnItem(GameObject item, GameObject location)
    {
        Debug.Log("called");
        if (dogObject.activeInHierarchy)
        {
            Debug.Log("activate");
            Vector3 position = location.GetComponent<Transform>().position;
            item.SetActive(true);
            item.transform.position = position;
        }
    }

    public void RemoveItem(GameObject item)
    {
        item.SetActive(false);
    }
}
