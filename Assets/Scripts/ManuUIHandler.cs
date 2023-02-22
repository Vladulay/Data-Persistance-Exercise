using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ManuUIHandler : MonoBehaviour
{

    public void StartGame()
    {
        MainManager.Instance.SetName();
        SceneManager.LoadScene(1);
    }

    public void DeleteProgress()
    {
        MainManager.Instance.DeleteData();
    }
}
