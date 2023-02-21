using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    public static MainManager Instance;
    public string playerName;

    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public GameObject GameOverText;
    
    private bool m_Started = false;
    private int m_Points;
    
    private bool m_GameOver = false;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        LoadHighscore();
    }

    public void SetupMainScene()
    {
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);

        ScoreText = GameObject.Find("ScoreText").GetComponent<Text>();
        GameOverText = GameObject.Find("GameoverText");
        GameOverText.SetActive(false);

        GameObject BallObject;
        BallObject = GameObject.Find("Ball");
        if (BallObject != null)
        {
            Ball = BallObject.GetComponent<Rigidbody>();
            Debug.Log("Ball Rb Found");
        }

        int[] pointCountArray = new [] {1,1,2,2,5,5};
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }

    }

    private void Update()
    {

        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                if (Ball != null)
                {
                    Ball.transform.SetParent(null);
                    Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
                }
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score : {m_Points}";
    }

    public void GameOver()
    {
        m_GameOver = true;
        GameOverText.SetActive(true);
    }

    public void LoadHighscore()
    {
        Debug.Log("Add code to load highscore");
    }

    public void SetName()
    {
        string typedName = "Test";
        typedName = GameObject.Find("Typed Name").GetComponent<TextMeshProUGUI>().text;
        Debug.Log(typedName);

        playerName = typedName;
    }
}
