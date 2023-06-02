using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ManuUIHandler : MonoBehaviour
{
    public Texture2D sourceTex;
    public Color[] pix;
    public int width;
    public int colorIndex;


    private void Awake()
    {
        pix = sourceTex.GetPixels();
        width = sourceTex.width;
    }

    public void StartGame()
    {
        MainManager.Instance.SetName();
        SceneManager.LoadScene(1);
    }

    public void EnterHallOfFame()
    {
        SceneManager.LoadScene(2);
    }

    public void EnterSettings()
    {
        SceneManager.LoadScene(3);
    }

    public void EnterMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void DeleteProgress()
    {
        MainManager.Instance.DeleteData();

        

        if (SceneManager.GetActiveScene().name == "highscore")
        {
            HallOfFameUIHandler HallOfFameUIHandler = GameObject.Find("HallOfFameUIHandler").GetComponent<HallOfFameUIHandler>();

            HallOfFameUIHandler.RetrieveHighscores();
            HallOfFameUIHandler.SetUI();
        }

        
    }

    public void ColorPick()
    {
        float sliderValue = GameObject.Find("ColorSlider").GetComponent<Slider>().value;
        colorIndex = Mathf.RoundToInt(width*sliderValue);
        if (colorIndex == width)
        {
            colorIndex -= 1;
        }
        else if (colorIndex == 0) 
        { 
            colorIndex += 1;
        }

        Color pickedColor = GameObject.Find("ColorShower").GetComponent<Image>().color;
        if (pix != null)
        {
            pickedColor = pix[colorIndex];
            GameObject.Find("ColorShower").GetComponent<Image>().color = pix[colorIndex];
        }
        

        MainManager.Instance.favColor = pickedColor;
        MainManager.Instance.favColorIndex = colorIndex;
        MainManager.Instance.SaveScore();
    }
}
