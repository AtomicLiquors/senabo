using UnityEngine;

public class GyroControl : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject dogObject;

    private bool gyroEnabled;
    private Gyroscope gyro;
    private Quaternion rot;
    void Start()
    {
        //dogObject = new GameObject("Camera Container");
        dogObject.transform.position = transform.position;
        transform.SetParent(dogObject.transform);
        gyroEnabled = EnableGyro();
    }

    private bool EnableGyro()
    {
        if (SystemInfo.supportsGyroscope)
        {
            gyro = Input.gyro;
            gyro.enabled = true;
            rot = Quaternion.Euler(0f, 0f, 0f);
            return true;
        }

        return false;
    }

    // Update is called once per frame
    
    void Update()
    {
        /*
        if (gyroEnabled)
        {
            Quaternion qRot = gyro.attitude * rot;
            dogObject.transform.localRotation = Quaternion.Euler(0f, qRot.eulerAngles.y, 0f);
        }*/
        rotatePerFrame();
    }
    
    private void rotatePerFrame()
    {
        float rotationSpeed = 30f; // Adjust the speed of rotation as needed
        Vector3 currentRotation = dogObject.transform.localEulerAngles;
        currentRotation.y += rotationSpeed * Time.deltaTime;
        dogObject.transform.localEulerAngles = currentRotation;
    }
}
