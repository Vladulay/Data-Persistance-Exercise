using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsUIInitilizer : MonoBehaviour
{
    private Slider slider;
    private int colorIndex;
    private float value;
    private ManuUIHandler uiHandler;

    void Start()
    {
        slider = GameObject.Find("ColorSlider").GetComponent<Slider>();
        uiHandler = GameObject.Find("MenuUIHandler").GetComponent<ManuUIHandler>();
        colorIndex = MainManager.Instance.favColorIndex;

        value = (float)colorIndex / uiHandler.width;

        slider.value = value;
    }

}
