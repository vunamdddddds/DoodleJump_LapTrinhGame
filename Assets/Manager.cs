using UnityEngine;
using UnityEngine.SceneManagement;
using System;
// Thêm using cho InputField
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Collections;


public class Manager : MonoBehaviour
{

    public static Manager instance;

    void Awake()
    {
        
        instance=this;
    }
    public GameObject musicOnButton;
    public GameObject musicOffButton;

    public GameObject optionPanel;
    public GameObject storePanel;

    public GameObject CollectionPanel;

    public GameObject scoreHistoryPanel;

    public GameObject FormInputPanel;

    public AudioSource audioSource;

    // hiển thị tên người chơi đã lưu nếu có, nếu không thì hiển thị form nhập tên mới
    public TextMeshProUGUI playerNameText;

    public TextMeshProUGUI scoreHistoryText;


    //  public TextMeshProUGUI currentScoreText;

    public TextMeshProUGUI highScoreText;

// store
    public TextMeshProUGUI moneyText;


    public TextMeshProUGUI infomationItem;

    public GameObject ItemPanel;
   

// biến Hard Code Items;

  public GameObject GhostSkinDisplay; //id==1
  public GameObject DiverSkinDisplay;// id ==2

  public GameObject InuitSkinDisplay; // id ==3
  public GameObject  AstronautSkinDisplay;//id ==4


// biến static
    public static string userName="";

    public static int coint = 0;

    public static int highScore=0;


// dùng biến tĩnh làm idItem
    public static int ItemID=0;

// khai báo 2 biến loading
 public GameObject LoaderUI;
    public Slider progressSlider;


    void Start()
    {
        // Tải dữ liệu cũ khi game bắt đầu
        bool loaded = saveData.Load();
        // Hiển thị form nhập tên nếu chưa có dữ liệu
        if (!loaded)
        {
            FormInputPanel.SetActive(true);
        }
        else
        {
            FormInputPanel.SetActive(false);
            DisplayPlayerInfo();// Hiển thị thông tin người chơi
            DisplayItemsForPlayer();
        }

    }



    public void LoadScene(int sceneIndex)
    {
       StartCoroutine(LoadScene_Coroutine(sceneIndex));
    }

// hàm loading 
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




//Store



    public void DisPlayItem(int idItem)
    {
     Items item = saveData.itemContainer.items[idItem-1];
infomationItem.text=$"Name:{item.nameItem.ToString()}\n Price:{item.price.ToString()} coin\n Description:{item.description.ToString()} \n"; 
ItemPanel.SetActive(true);
    }

    public void Item()
    {ItemPanel.SetActive(!ItemPanel.activeSelf);
    }
   

// hàm hiển thị  collection

public void Collection()
    {
        CollectionPanel.SetActive(!CollectionPanel.activeSelf);
    }
    private void DisplayItemsForPlayer()
    {
        PlayerData playerData =saveData.playerContainer.players[0];
        List<Items> itemList=saveData.itemContainer.items;
  ItemID=playerData.currentItems;  //trích xuất biến ItemId vào màn chơi
   foreach (int itemID in playerData.ownedItems)
   {
    foreach (Items item in itemList)
    {
        if (itemID==item.idItem)
        {
           switch (itemID)
           {
            case 1 : 
            GhostSkinDisplay.SetActive(true);
            break;

            case 2 :
            DiverSkinDisplay.SetActive(true);
            break;

            case 3 :
            InuitSkinDisplay.SetActive(true);
            break;

            case 4 :
            AstronautSkinDisplay.SetActive(true);
            break;
            default:
            break;
           }
        }
    }
   }


    }

    // hàm đổi trang phục
    public void ChangeItemCollition(int idItem)
    {


        foreach (Items item in saveData.itemContainer.items)
        {
            if (item.idItem==idItem)
            {
                ItemID=idItem;
                 saveData.playerContainer.players[0].currentItems=idItem;
                 saveData.Save();
                Debug.Log("Da doi Item co id la:"+idItem);
            }
           
        }
    }


     public void PurchaseItem()
    {
        // Tìm item trong danh sách
        Items itemToPurchase = null;
        foreach (Items item in saveData.itemContainer.items)
        {
            if (item.idItem == ItemID)
            {
                itemToPurchase = item;
                break;
            }
        }
        if (itemToPurchase == null)
        {
            Debug.Log("Item not found.");
            return;
        }
        // Kiểm tra tiền của người chơi
        PlayerData player = saveData.playerContainer.players[0]; // Lấy dữ liệu người chơi đầu tiên
        //kiểm tra người chơi có vật phẩm chưa 
       foreach (int item in player.ownedItems)
       {
        if (item == ItemID)
        {
            Debug.Log("You have alredy this Item");
            return;
        }
       }
        if (player.money >= itemToPurchase.price)
        {
            player.money -= itemToPurchase.price; // Trừ tiền
            player.ownedItems.Add(ItemID); // Thêm vào danh sách vật phẩm đã sở hữu
            saveData.Save(); // Lưu lại dữ liệu sau khi mua
            moneyText.text = player.money.ToString(); // Cập nhật hiển thị tiền
            Debug.Log("Item purchased: " + itemToPurchase.nameItem);
        }
        else
        {
            Debug.Log("Not enough money to purchase this item.");
        }

    }



    private void DisplayPlayerInfo()
    {
        if (saveData.playerContainer.players.Count > 0)
        {
            PlayerData player = saveData.playerContainer.players[0]; // Lấy dữ liệu người chơi đầu tiên
            playerNameText.text = player.playerName;
            moneyText.text = player.money.ToString();
            highScoreText.text = player.highScore.ToString();
        

            // Hiển thị lịch sử điểm số
            List<String> scoreHistory = getScoreHistoryForPlayer(player);
            foreach (String record in scoreHistory)
            {
                scoreHistoryText.text += record + "\n";
            }
    
            userName=player.playerName;
            coint=player.money;
            highScore=player.highScore;


        }
    }




    private List<String> getScoreHistoryForPlayer(PlayerData player)
    {
        List<String> history = new List<String>();
        if (player.scoreHistory.Count == 0)
        {
            history.Add("No score history available.");
            return history;
        }
        foreach (scoreRecord record in player.scoreHistory)
        {
            history.Add($"{record.date}   {record.score}");
        }
        return history;
    }



    public void TurnOnOffMusic()
    {
        if (musicOnButton.activeSelf)
        {
            musicOnButton.SetActive(false);
            musicOffButton.SetActive(true);
            audioSource.Pause();
        }
        else
        {
            musicOnButton.SetActive(true);
            musicOffButton.SetActive(false);
            audioSource.Play();
        }
    }

    public void Option()
    {
        optionPanel.SetActive(!optionPanel.activeSelf);
    }
    public void Store()
    {
        storePanel.SetActive(!storePanel.activeSelf);
    }

    public void scoreHistory()
    {
        scoreHistoryPanel.SetActive(!scoreHistoryPanel.activeSelf);
    }
    
    public void SaveNewPlayerData(TMP_InputField inputField)
    {
        // Tạo dữ liệu mới
        PlayerData newData = new PlayerData();
        newData.playerName = inputField.text;
        newData.highScore = 0;
        newData.money = 0;
        newData.scoreHistory = new List<scoreRecord>();
        newData.ownedItems = new List<int>();
        newData.currentItems=0; // chua co gi 
        //tạo hard data items 
        Items  ghotSkin = new Items();
         ghotSkin.idItem =1;
         ghotSkin.nameItem="ghostSkin";
         ghotSkin.price=10000;
         ghotSkin.description="trang phục sự kiện hallowin"; 
          Items  diver = new Items();
         diver.idItem =2;
         diver.nameItem="diver";
         diver.price=500;
         diver.description="Người được đào tạo để lặn dưới nước nhằm khảo sát, nghiên cứu, hoặc sửa chữa";
          Items  Inuit = new Items();
         Inuit.idItem =3;
         Inuit.nameItem="InuitSkin";
         Inuit.price=100;
         Inuit.description="Tộc người nổi tiếng sống tại các vùng cực Bắc Cực, nổi tiếng với việc xây lều Igloo và cọ mũi thay vì hôn môi";
          Items  astronaut = new Items();
         astronaut.idItem =4;
         astronaut.nameItem="astronaut";
         astronaut.price=3000;
         astronaut.description="Những người được đào tạo để du hành và làm việc trong không gian vũ trụ";

         saveData.itemContainer.items.Add(ghotSkin);
         saveData.itemContainer.items.Add(diver);
         saveData.itemContainer.items.Add(Inuit);
         saveData.itemContainer.items.Add(astronaut);

         
        // Thêm vào danh sách trong saveData
        saveData.playerContainer.players.Add(newData);

        // Thực hiện ghi xuống file
        saveData.Save();
        // Ẩn form nhập tên sau khi lưu
        FormInputPanel.SetActive(false);
        Start();
        Debug.Log("New player created and saved: " + newData.playerName);
    }

   

}