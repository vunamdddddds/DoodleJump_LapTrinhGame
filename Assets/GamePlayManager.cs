using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;



public class GamePlayManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public static GamePlayManager instance;

    void Awake()
    {
        instance = this;
    }

    public GameObject player;
  
    public static int curScore=0;
    public GameObject gameOverPanel;

    public GameObject gamePausePanel;

    public GameObject springShoePlayer;

    // khai báo camera ==> game win vs game over ==> disable : camera tranh loi khi player bi destroy
    public GameObject cameraFollow;

    public GameObject gameWinPanel;

//haloween 
public  GameObject halloweenTheme;

    public TextMeshProUGUI curScoreText;
    //game over panel
    public TextMeshProUGUI ScoreGameOverText;
    public TextMeshProUGUI HighScoreGameOverText;

    public TextMeshProUGUI MoneyEarnedGameOverText;

// game win 
 public TextMeshProUGUI highScoreTextGameWin;
    public TextMeshProUGUI moneyTextGameWin;

    public TextMeshProUGUI ScoreTextGameWin;



// game Pause

 
// audio sfx
    public AudioSource audioSource;

    public AudioClip SpringClip;


    public AudioClip GrountGreenClip;

    public AudioClip TrampolineClip;

    public AudioClip GrountCrashClip;

public AudioClip WinClip;

public AudioClip LoseClip;

// khai báo biến coroutine
private Coroutine currentCoroutine;
// khai báo 2 biến loading
 public GameObject LoaderUI;
    public Slider progressSlider;


// Khai biến biến skin 
public  GameObject GhostSkin;
public GameObject DiverSkin;
public GameObject InuitSkin;
public GameObject AstronautSkin;



    void Start()
    {   
        SkinHandle();

    }

    


// Ham xu li skin nguoi choi 

public void SkinHandle()
    {
        int curItem = Manager.ItemID;
        switch (curItem)
        {
            case 1 :
                GhostSkin.SetActive(true);
                break;
            case 2 : 
                DiverSkin.SetActive(true);
                break;
            case 3 :
                InuitSkin.SetActive(true);
                break;
            case 4 : 
                AstronautSkin.SetActive(true);
                break;
            default:
                break;
        }
    }

public void pauseGame()
    {
        
        if (player.activeSelf)
        {

            player.SetActive(false);
            gamePausePanel.SetActive(true);
            if (halloweenTheme!=null)
            {
                          halloweenTheme.SetActive(false);

            }
        }
        else
        {player.SetActive(true);
        gamePausePanel.SetActive(false);
        if (halloweenTheme!=null)
            {
           halloweenTheme.SetActive(true);

            }

            
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
            case EnumSfxType.Trampoline:
                audioSource.PlayOneShot(TrampolineClip);
                break;
            case EnumSfxType.WinGame:
                audioSource.PlayOneShot(WinClip);
                break; 
            case EnumSfxType.GameOver:
                audioSource.PlayOneShot(LoseClip);
                break; 
            case EnumSfxType.GrountCrash:
                audioSource.PlayOneShot(GrountCrashClip);
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

         Destroy(player,5f);// hủy đối tượng player khi game over
         Platform.springShoeBootValue=0; 
         cameraFollow.SetActive(false);
         Sfx(EnumSfxType.GameOver);
        if (curScore > Manager.highScore)
        {
            saveData.playerContainer.players[0].highScore = curScore;
            Manager.highScore=curScore;
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
        HighScoreGameOverText.text =  Manager.highScore.ToString();
        MoneyEarnedGameOverText.text = moneyEarned.ToString();
        HalloweenThemeHandle();
        gameOverPanel.SetActive(true);        
    }

    private void HalloweenThemeHandle()
    {
     if (halloweenTheme!=null)
     {
        halloweenTheme.SetActive(false);
     }   
    }
    public void GameWin()
    {  
     Destroy(player,5f);// hủy đối tượng player khi game over
              Platform.springShoeBootValue=0;
               cameraFollow.SetActive(false);
       Sfx(EnumSfxType.WinGame);
        if (curScore >Manager.highScore)
        {
            saveData.playerContainer.players[0].highScore = curScore;
            Manager.highScore=curScore;
        }
        saveData.playerContainer.players[0].scoreHistory.Add(new scoreRecord
        {
            date = System.DateTime.Now.ToString("yyyy-MM-dd"),
            score = curScore
        });
        int moneyEarned = curScore / 10; // Ví dụ: mỗi 100 điểm được 1 tiền
        saveData.playerContainer.players[0].money += moneyEarned;
         saveData.playerContainer.players[0].LevelCur+=1;// tang level
        Manager.LevelCur=saveData.playerContainer.players[0].LevelCur;
        saveData.Save();
        ScoreTextGameWin.text = curScore.ToString();
        highScoreTextGameWin.text =Manager.highScore.ToString();
        moneyTextGameWin.text = moneyEarned.ToString();
        HalloweenThemeHandle();
        gameWinPanel.SetActive(true);
    }




    public void SpringShoeBoot(Collider2D other)
    {       other.gameObject.SetActive(false);
        Platform.springShoeBootValue = 20;
        springShoePlayer.SetActive(true);
        // Nếu đang có hiệu ứng cũ → dừng lại
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }
        // Bắt đầu lại thời gian mới
        currentCoroutine = StartCoroutine(SpringShoeDuration());
      
    }

    IEnumerator SpringShoeDuration()
{
    yield return new WaitForSeconds(5f);
    Platform.springShoeBootValue = 0;
    springShoePlayer.SetActive(false);

    currentCoroutine = null; // reset lại trạng thái
}
    public void LoadScene(int sceneIndex)
    {
       StartCoroutine(LoadScene_Coroutine(sceneIndex));
    }

    public IEnumerator LoadScene_Coroutine(int index)
    {
        progressSlider.value = 0;
        LoaderUI.SetActive(true);
 
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(index);
        asyncOperation.allowSceneActivation = false;
        float progress = 0;
 
        while (!asyncOperation.isDone)
        {
            progress = Mathf.MoveTowards(progress, asyncOperation.progress, Time.deltaTime);
            progressSlider.value = progress;
            if (progress >= 0.9f)
            {
                progressSlider.value = 1;
                asyncOperation.allowSceneActivation = true;
            }
            yield return null;
        }
    }
}
