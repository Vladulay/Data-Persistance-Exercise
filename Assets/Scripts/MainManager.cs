using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    public static MainManager Instance;
    public string playerName;
    string path;

    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public GameObject GameOverText;
    public Text HighscoreText;
    
    private bool m_Started = false;
    private int m_Points;
    
    private bool m_GameOver = false;

    public int highscore;
    public string bestPlayerName;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        path = Application.persistentDataPath + "/savefile.json";

        LoadData();
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

    [System.Serializable]
    class SaveData
    {
        public string playerName;
        public int highscore;
    }

    public void SetupMainScene()
    {
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        m_GameOver = false;
        m_Started = false;
        m_Points = 0;

        ScoreText = GameObject.Find("ScoreText").GetComponent<Text>();
        GameOverText = GameObject.Find("GameoverText");
        GameOverText.SetActive(false);
        HighscoreText = GameObject.Find("HighscoreText").GetComponent<Text>();

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

        ScoreText.text = $"Score of {playerName}: {m_Points}";

        UpdateHighscore();
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score of {playerName}: {m_Points}";
    }

    public void GameOver()
    {
        m_GameOver = true;
        GameOverText.SetActive(true);

        if (m_Points > highscore)
        {
            SaveScore();
        }
    }

    public void SaveScore()
    {
        SaveData data = new SaveData();
        data.playerName = playerName;
        data.highscore = m_Points;
        highscore = m_Points;
        bestPlayerName = playerName;

        string json = JsonUtility.ToJson(data);

        File.WriteAllText(path, json);

        UpdateHighscore();
    }
    
    public void LoadData()
    {
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            highscore = data.highscore;
            bestPlayerName = data.playerName;
        }
        else
        {
            bestPlayerName = "Nobody";
            highscore = 0;
            SaveScore();
        }
    }

    public void DeleteData()
    {
        playerName = "Nobody";
        m_Points = 0;
        SaveScore();
    }

    public void SetName()
    {
        string typedName;
        typedName = GameObject.Find("Typed Name").GetComponent<TextMeshProUGUI>().text;
        
        if(typedName != "")
        {
            playerName = typedName;
        }
        else
        {
            playerName = "Nobody";
        }
        
    }

    public void UpdateHighscore()
    {
        if(HighscoreText != null)
        {
            HighscoreText.text = bestPlayerName + " holds the Highscore with " + highscore + " points";
        }
    }
}
