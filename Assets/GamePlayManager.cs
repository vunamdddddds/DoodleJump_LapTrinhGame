using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;


public class GamePlayManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public static GamePlayManager instance;

    void Awake()
    {
        instance = this;
    }

    public GameObject player;
    public GameObject topBar1;
    public GameObject topBar2;
    public GameObject topBar3;
    public static int curScore;
    public GameObject gameOverPanel;

    public GameObject gamePausePanel;

    public GameObject gameWinPanel;
    public TextMeshProUGUI curScoreText;
    public TextMeshProUGUI highScoreText;
    //game over panel
    public TextMeshProUGUI ScoreGameOverText;
    public TextMeshProUGUI HighScoreGameOverText;

    public TextMeshProUGUI MoneyEarnedGameOverText;

// game win 
 public TextMeshProUGUI highScoreTextGameWin;
    public TextMeshProUGUI moneyTextGameWin;

    public TextMeshProUGUI ScoreTextGameWin;


// game Pause

 

    public AudioSource audioSource;

    public AudioClip SpringClip;


    public AudioClip GrountGreenClip;




    void Start()
    {
        bool loaded = saveData.Load();
        curScore = 0;
        highScoreText.text = saveData.playerContainer.players[0].highScore.ToString();
    }


public void pauseGame()
    {
        if (player.activeSelf)
        {

            player.SetActive(false);
            gamePausePanel.SetActive(true);
        }
        else
        {player.SetActive(true);
        gamePausePanel.SetActive(false);
            
        }
    }

    public void Sfx(EnumSfxType enumSfxType)
    {

        switch (enumSfxType)
        {
            case EnumSfxType.GroundGreen:
                audioSource.PlayOneShot(GrountGreenClip);
                break;

            case EnumSfxType.GroundBlue:
                audioSource.PlayOneShot(GrountGreenClip);
                break;

            case EnumSfxType.Spring:
                audioSource.PlayOneShot(SpringClip);
                break;
            default:
                break;
        }


    }


    // Update is called once per frame
    void Update()
    {
        curScoreText.text = curScore.ToString();
    }


    // bắt đầu xây hàm game over , tình trạng : đang làm,chưa hoàn thiện
    public void GameOver()
    {
        Destroy(player);// hủy đối tượng player khi game over
        if (curScore > saveData.playerContainer.players[0].highScore)
        {
            saveData.playerContainer.players[0].highScore = curScore;
        }
        saveData.playerContainer.players[0].scoreHistory.Add(new scoreRecord
        {
            date = System.DateTime.Now.ToString("yyyy-MM-dd"),
            score = curScore
        });
        int moneyEarned = curScore / 10; // Ví dụ: mỗi 100 điểm được 1 tiền
        saveData.playerContainer.players[0].money += moneyEarned;
        saveData.Save();
        ScoreGameOverText.text = curScore.ToString();
        HighScoreGameOverText.text = saveData.playerContainer.players[0].highScore.ToString();
        MoneyEarnedGameOverText.text = moneyEarned.ToString();
        gameOverPanel.SetActive(true);
    }
    public void GameWin()
    {  
         Destroy(player);// hủy đối tượng player khi game over
        if (curScore > saveData.playerContainer.players[0].highScore)
        {
            saveData.playerContainer.players[0].highScore = curScore;
        }
        saveData.playerContainer.players[0].scoreHistory.Add(new scoreRecord
        {
            date = System.DateTime.Now.ToString("yyyy-MM-dd"),
            score = curScore
        });
        int moneyEarned = curScore / 10; // Ví dụ: mỗi 100 điểm được 1 tiền
        saveData.playerContainer.players[0].money += moneyEarned;
        saveData.Save();
        ScoreTextGameWin.text = curScore.ToString();
        highScoreTextGameWin.text = saveData.playerContainer.players[0].highScore.ToString();
        moneyTextGameWin.text = moneyEarned.ToString();
        gameWinPanel.SetActive(true);
    }

    public void ChangeTopTheme(Collider2D other)
    {
        if (other.gameObject.CompareTag("DefaultMap"))
        {
            topBar1.SetActive(true);
            topBar2.SetActive(false);
            topBar3.SetActive(false);
            Debug.Log("Collided with DefaultMap");

        }
        else if (other.gameObject.CompareTag("SnowMap"))
        {
            topBar1.SetActive(false);
            topBar2.SetActive(true);
            topBar3.SetActive(false);
            Debug.Log("Collided with SnowMap");
        }
    }
    public void LoadScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }
}
