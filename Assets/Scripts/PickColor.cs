using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PickColor : MonoBehaviour
{
    private void Awake()
    {

    }


    private void Start()
    {
        if(gameObject.GetComponent<Image>() != null)
        {
            gameObject.GetComponent<Image>().color = MainManager.Instance.favColor;
        }

        if (gameObject.GetComponent<TextMeshProUGUI>() != null)
        {
            gameObject.GetComponent<TextMeshProUGUI>().color = MainManager.Instance.favColor;
            gameObject.GetComponent<TextMeshProUGUI>().faceColor = MainManager.Instance.favColor;
        }

        if (gameObject.GetComponent<Slider>() != null)
        {
            int arrayLenght = GameObject.Find("MenuUIHandler").GetComponent<ManuUIHandler>().pix.Length;

            for (int i = 1; i < arrayLenght ; i++) 
            {
                if(GameObject.Find("MenuUIHandler").GetComponent<ManuUIHandler>().pix[i] == MainManager.Instance.favColor)
                {
                    gameObject.GetComponent<Slider>().value = i/arrayLenght;
                }
            }
        }
    }
}
