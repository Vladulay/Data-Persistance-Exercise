using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuInitilizer : MonoBehaviour
{
    public GameObject nameField;
    private string playerName;

    void Start()
    {
        playerName = MainManager.Instance.playerName;
        nameField.GetComponent<TMP_InputField>().text = playerName;
    }
}