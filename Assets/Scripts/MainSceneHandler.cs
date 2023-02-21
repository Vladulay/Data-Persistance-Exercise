using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSceneHandler : MonoBehaviour
{
    private void Awake()
    {
        MainManager.Instance.SetupMainScene();
    }
}
