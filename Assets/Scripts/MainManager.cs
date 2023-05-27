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
    public GameObject MenuButton;
    
    private bool m_Started = false;
    private int m_Points;
    
    private bool m_GameOver = false;

    public int[] highscore;
    public string[] bestPlayerName;

    public Color favColor;

    private void Awake()
    {
        bestPlayerName = new string[3];
        highscore= new int[3];

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

                MenuButton.SetActive(false);
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
        public string firstPlayerName;
        public string secondPlayerName;
        public string thirdPlayerName;
        public int firstHighscore;
        public int secondHighscore;
        public int thirdHighscore;
        public string favColor;
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
        MenuButton = GameObject.Find("Menu Button");

        GameObject BallObject;
        BallObject = GameObject.Find("Ball");
        if (BallObject != null)
        {
            Ball = BallObject.GetComponent<Rigidbody>();
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
        MenuButton.SetActive(true);

        if (m_Points > highscore[2])
        {
            SaveScore();
        }
    }

    public void SaveScore()
    {
        SaveData data = new SaveData();

        if (m_Points > highscore[0])
        {
            highscore[2] = highscore[1];
            highscore[1] = highscore[0];
            highscore[0] = m_Points;

            bestPlayerName[2] = bestPlayerName[1];
            bestPlayerName[1] = bestPlayerName[0];
            bestPlayerName[0] = playerName;
        }
        else if (m_Points > highscore[1])
        {
            highscore[2] = highscore[1];
            highscore[1] = m_Points;

            bestPlayerName[2] = bestPlayerName[1];
            bestPlayerName[1] = playerName;
        }
        else if (m_Points > highscore[2])
        {
            highscore[2] = m_Points;

            bestPlayerName[2] = playerName;
        }
                
        data.firstPlayerName = bestPlayerName[0];
        data.secondPlayerName = bestPlayerName[1];
        data.thirdPlayerName = bestPlayerName[2];

        data.firstHighscore = highscore[0];
        data.secondHighscore = highscore[1];
        data.thirdHighscore = highscore[2];

        data.favColor = ColorUtility.ToHtmlStringRGBA(favColor);
        
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

            highscore[0] = data.firstHighscore;
            highscore[1] = data.secondHighscore;
            highscore[2] = data.thirdHighscore;

            bestPlayerName[0] = data.firstPlayerName;
            bestPlayerName[1] = data.secondPlayerName;
            bestPlayerName[2] = data.thirdPlayerName;

            Color newCol;

            if (ColorUtility.TryParseHtmlString("#"+data.favColor, out newCol))
            {
                favColor = newCol;
            }

            
        }
        else
        {
            bestPlayerName[0] = "Nobody";
            bestPlayerName[1] = "Nobody";
            bestPlayerName[2] = "Nobody";
            highscore[0] = 0;
            highscore[1] = 0;
            highscore[2] = 0;

            m_Points = 0;
            playerName = "Nobody";

            Color newCol;
            if (ColorUtility.TryParseHtmlString("#00FFFF", out newCol))
            {
                favColor = newCol;
            }

            SaveScore();
        }

        
    }

    public void DeleteData()
    {
        bestPlayerName[0] = "Nobody";
        bestPlayerName[1] = "Nobody";
        bestPlayerName[2] = "Nobody";
        highscore[0] = 0;
        highscore[1] = 0;
        highscore[2] = 0;
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
            HighscoreText.text = bestPlayerName[0] + " holds the Highscore with " + highscore[0] + " points";
        }
    }
}
