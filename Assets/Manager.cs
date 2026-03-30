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

        instance = this;
    }
    public GameObject musicOnButton;
    public GameObject musicOffButton;

    public GameObject optionPanel;
    public GameObject storePanel;

    public GameObject CollectionPanel;

    public GameObject scoreHistoryPanel;

    public GameObject RemoveADsPanel;

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

    // thông báo mua hàng 
    public GameObject BuySuscessText;
    public GameObject ItemAlreadyOwnText;

    public GameObject BuyFalseText;

    private Coroutine currentCoroutine;

    // thông báo đổi trang phục (item)

    public GameObject ChangeItemSusscess;



    // biến Hard Code Items;

    public GameObject GhostSkinDisplay; //id==1
    public GameObject DiverSkinDisplay;// id ==2

    public GameObject InuitSkinDisplay; // id ==3
    public GameObject AstronautSkinDisplay;//id ==4

    public GameObject GhostSkinItem;
    public GameObject DiverSkinItem;
    public GameObject InuitSkinItem;
    public GameObject AstronautSkinItem;

    // Skin At Main 
    public GameObject DiverSkinAtMain;
    public GameObject GhostSkinAtMain;
    public GameObject InuitSkinAtMain;
    public GameObject AstronautSkinAtMain;
    public GameObject DefaultSkinAtMain;


    // choose level
    public GameObject chooseLevelPanel;

    public List<GameObject> levelButtonList;

    // khai báo tên coint item trong store

    public TextMeshProUGUI GhostTitle;
    public TextMeshProUGUI CointGhost;
    // inuit
    public TextMeshProUGUI InuintTile;
    public TextMeshProUGUI CointInuint;

    // Diver
    public TextMeshProUGUI DiverTile;
    public TextMeshProUGUI DiverCoint;

    // Astronaut
    public TextMeshProUGUI AstronautTitle;
    public TextMeshProUGUI AstronautCoint;




    public static int ItemDinhMua = 0;




    // biến static
    public static string userName = "";

    public static int coint = 0;

    public static int highScore = 0;

    public static bool removeADsStatus = false;

    public static int LevelCur;


    // dùng biến tĩnh làm idItem
    public static int ItemID = 0;

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
            DisplayTileAndPriceItemsInStore();
            DisplaySettingForPlayer();
            displaySkinAtMain();
          
        }

    }

    public void chooseLevel()
    {
        chooseLevelPanel.SetActive(!chooseLevelPanel.activeSelf);
        //display 

        disPlaychooseLevelForPlayer();
    }



    public void chooseLevelHandle(int index)
    {
        LoadScene(index);
    }

    void disPlaychooseLevelForPlayer()
    {
        int cur = 0;
        foreach (GameObject level in levelButtonList)
        {
            if (cur < LevelCur)
            {
                level.SetActive(false);
                cur++;
            }

        }


    }



    // hàm xử lí xoá quảng cáo 
    public void RemoveADs()
    {
        RemoveADsPanel.SetActive(!RemoveADsPanel.activeSelf);
    }

    public void RemoveAdsHadle()
    {

        saveData.playerContainer.players[0].removeADsStatus = true;
        saveData.Save();
        Banner.instance.RemoveBanner();
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




    // hàm hiển thị setting của người chơi
    public void DisplaySettingForPlayer()
    {
        Setting setting = saveData.settingContainer.setting;

        if (setting != null)
        {
            // musicBG status 
            if (setting.musicbgStatus == true)
            {
                musicOnButton.SetActive(true);
                musicOffButton.SetActive(false);
                audioSource.Play();
            }
            else
            {
                musicOnButton.SetActive(false);
                musicOffButton.SetActive(true);
                audioSource.Pause();
            }
        }

    }
    //Store

    public void DisplayTileAndPriceItemsInStore()
    {
        List<Items> itemList = saveData.itemContainer.items;
        foreach (Items item in itemList)
        {
            switch (item.idItem)
            {
                case 1:
                    GhostTitle.text = item.nameItem;
                    CointGhost.text = item.price.ToString();
                    break;
                case 2:
                    DiverTile.text = item.nameItem;
                    DiverCoint.text = item.price.ToString();

                    break;
                case 3:
                    InuintTile.text = item.nameItem;
                    CointInuint.text = item.price.ToString();
                    break;
                case 4:
                    AstronautTitle.text = item.nameItem;
                    AstronautCoint.text = item.price.ToString();
                    break;
                default:
                    break;
            }
        }
    }
    public void DisPlayItem(int idItem)
    {
        Items item = saveData.itemContainer.items[idItem - 1];
        infomationItem.text = $"Name:{item.nameItem.ToString()}\n Price:{item.price.ToString()} coin\n Description:{item.description.ToString()} \n";
        ItemPanel.SetActive(true);
        switch (idItem)
        {
            case 1:
                GhostSkinItem.SetActive(true);
                break;
            case 2:
                DiverSkinItem.SetActive(true);
                break;
            case 3:
                InuitSkinItem.SetActive(true);
                break;
            case 4:
                AstronautSkinItem.SetActive(true);
                break;
            default:
                break;
        }
        ItemDinhMua = idItem;
    }

    public void Item()
    {
        ItemPanel.SetActive(!ItemPanel.activeSelf);
        switch (ItemDinhMua)
        {
            case 1:
                GhostSkinItem.SetActive(false);
                break;
            case 2:
                DiverSkinItem.SetActive(false);
                break;
            case 3:
                InuitSkinItem.SetActive(false);
                break;
            case 4:
                AstronautSkinItem.SetActive(false);
                break;
            default:
                break;
        }
        ItemDinhMua = 0;

    }


    // hàm hiển thị  collection



    public void Collection()
    {
        CollectionPanel.SetActive(!CollectionPanel.activeSelf);
    }

    // hàm này hiện thị những item mà người chơi đã sở hữu trong collition 
    private void DisplayItemsForPlayer()
    {
        PlayerData playerData = saveData.playerContainer.players[0];
        List<Items> itemList = saveData.itemContainer.items;
        foreach (int itemID in playerData.ownedItems)
        {
            foreach (Items item in itemList)
            {
                if (itemID == item.idItem)
                {
                    if (itemID == 1)
                    {
                        GhostSkinDisplay.SetActive(true);

                    }
                    else if (itemID == 2)
                    {
                        DiverSkinDisplay.SetActive(true);

                    }
                    else if (itemID == 3)
                    {
                        InuitSkinDisplay.SetActive(true);

                    }
                    else if (itemID == 4)
                    {
                        AstronautSkinDisplay.SetActive(true);

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
            if (item.idItem == idItem)
            {
                ItemID = idItem; // đổi biến tĩnh thành item hiện tại 
                saveData.playerContainer.players[0].currentItems = idItem;
                saveData.Save();
                DisplayNotify(ChangeItemSusscess);
                displaySkinAtMain();
                Debug.Log("Da doi Item co id la:" + idItem);
            }

        }
    }


//hàm xử lí hiển thị skin ở main (hard data)
    private void displaySkinAtMain()
    {
        switch (ItemID)
        {
            case 1:
                GhostSkinAtMain.SetActive(true);
                DiverSkinAtMain.SetActive(false);
                InuitSkinAtMain.SetActive(false);
                AstronautSkinAtMain.SetActive(false);
                DefaultSkinAtMain.SetActive(false);
                break;
            case 2:
                GhostSkinAtMain.SetActive(false);
                DiverSkinAtMain.SetActive(true);
                InuitSkinAtMain.SetActive(false);
                AstronautSkinAtMain.SetActive(false);
                                DefaultSkinAtMain.SetActive(false);

                break;
            case 3:
                GhostSkinAtMain.SetActive(false);
                DiverSkinAtMain.SetActive(false);
                InuitSkinAtMain.SetActive(true);
                AstronautSkinAtMain.SetActive(false);
                                DefaultSkinAtMain.SetActive(false);

                break;
            case 4:
                GhostSkinAtMain.SetActive(false);
                DiverSkinAtMain.SetActive(false);
                InuitSkinAtMain.SetActive(false);
                AstronautSkinAtMain.SetActive(true);
                                DefaultSkinAtMain.SetActive(false);

                break;
            default:
                break;
        }

    }



    // ham mua item 

    public void PrePurchaseItem()
    {
        PurchaseItem(ItemDinhMua);
    }
    public void PurchaseItem(int idItem)
    {

        // Tìm item trong danh sách
        Items itemToPurchase = null;
        foreach (Items item in saveData.itemContainer.items)
        {
            if (item.idItem == idItem)
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
            if (item == idItem)
            {
                Debug.Log("You have alredy this Item");
                DisplayNotify(ItemAlreadyOwnText);
                return;
            }
        }
        if (player.money >= itemToPurchase.price)
        {
            player.money -= itemToPurchase.price; // Trừ tiền
            player.ownedItems.Add(idItem); // Thêm vào danh sách vật phẩm đã sở hữu
            saveData.Save(); // Lưu lại dữ liệu sau khi mua
            moneyText.text = player.money.ToString(); // Cập nhật hiển thị tiền gốc 
            Debug.Log("Item purchased: " + itemToPurchase.nameItem);
            DisplayNotify(BuySuscessText);
        }
        else
        {
            Debug.Log("Not enough money to purchase this item.");
            DisplayNotify(BuyFalseText);
        }

    }

    public void DisplayNotify(GameObject obj)
    {

        obj.SetActive(true);
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }
        currentCoroutine = StartCoroutine(DisplayNotifyCountTime(obj));
    }

    IEnumerator DisplayNotifyCountTime(GameObject obj)
    {
        yield return new WaitForSeconds(2f);
        obj.SetActive(false);
        currentCoroutine = null; // reset lại trạng thái

    }


    private void DisplayPlayerInfo()
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

        // chưa biết để làm gì 
        userName = player.playerName;
        coint = player.money;

        highScore = player.highScore;
        // cái này thì dùng làm truyền cho các scence khác 
        removeADsStatus = player.removeADsStatus;
        LevelCur = player.LevelCur;
        ItemID = player.currentItems;
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
            saveData.settingContainer.setting.musicbgStatus = false;
        }
        else
        {
            musicOnButton.SetActive(true);
            musicOffButton.SetActive(false);
            audioSource.Play();
            saveData.settingContainer.setting.musicbgStatus = true;
        }
        saveData.Save();
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
        newData.currentItems = 0; // chua co gi 
        newData.removeADsStatus = false;
        newData.LevelCur = 1;


        //tạo hard data items 
        Items ghotSkin = new Items();
        ghotSkin.idItem = 1;
        ghotSkin.nameItem = "ghostSkin";
        ghotSkin.price = 10000;
        ghotSkin.description = "trang phục sự kiện hallowin";
        Items diver = new Items();
        diver.idItem = 2;
        diver.nameItem = "diver";
        diver.price = 500;
        diver.description = "Người được đào tạo để lặn dưới nước nhằm khảo sát, nghiên cứu, hoặc sửa chữa";
        Items Inuit = new Items();
        Inuit.idItem = 3;
        Inuit.nameItem = "InuitSkin";
        Inuit.price = 100;
        Inuit.description = "Tộc người nổi tiếng sống tại các vùng cực Bắc Cực, nổi tiếng với việc xây lều Igloo và cọ mũi thay vì hôn môi";
        Items astronaut = new Items();
        astronaut.idItem = 4;
        astronaut.nameItem = "astronaut";
        astronaut.price = 3000;
        astronaut.description = "Những người được đào tạo để du hành và làm việc trong không gian vũ trụ";

        saveData.itemContainer.items.Add(ghotSkin);
        saveData.itemContainer.items.Add(diver);
        saveData.itemContainer.items.Add(Inuit);
        saveData.itemContainer.items.Add(astronaut);


        //tao du ban dau cho setting
        Setting setting = new Setting();
        setting.musicbgStatus = true;


        // Thêm vào danh sách trong saveData
        saveData.playerContainer.players.Add(newData);
        saveData.settingContainer.setting = setting;
        // Thực hiện ghi xuống file
        saveData.Save();
        // Ẩn form nhập tên sau khi lưu
        FormInputPanel.SetActive(false);
        Start();
        Debug.Log("New player created and saved: " + newData.playerName);
    }



}