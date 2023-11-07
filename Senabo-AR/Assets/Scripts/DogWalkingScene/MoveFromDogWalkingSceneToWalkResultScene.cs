using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveFromDogWalkingSceneToWalkResultScene : MonoBehaviour
{
    public void OnGoHomeButton()
    {
        SceneManager.LoadScene("WalkResultScene");
    }
}
