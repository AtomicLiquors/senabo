using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FromDictDetailSceneToOwnerDictScene : MonoBehaviour
{
    public void SceneChange()
    {
        SceneManager.LoadScene("OwnerDictScene");
    }
}
