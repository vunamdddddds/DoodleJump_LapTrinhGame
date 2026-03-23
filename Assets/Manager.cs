using UnityEngine;
using UnityEngine.SceneManagement;
using System;

// Thêm using cho InputField
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class Manager : MonoBehaviour
{
    public GameObject musicOnButton;
    public GameObject musicOffButton;

    public GameObject buyButton; 
    public GameObject optionPanel;
    public GameObject storePanel;

    public GameObject scoreHistoryPanel;

    public GameObject FormInputPanel;

    public AudioSource audioSource;

    // hiển thị tên người chơi đã lưu nếu có, nếu không thì hiển thị form nhập tên mới
    public TextMeshProUGUI playerNameText;

    public TextMeshProUGUI scoreHistoryText;


    //  public TextMeshProUGUI currentScoreText;

    public TextMeshProUGUI highScoreText;

    public TextMeshProUGUI moneyText;

    public TextMeshProUGUI itemListText;


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
            DisPlayItems();// Hiển thị danh sách vật phẩm trong cửa hàng
        }

    }


    private void DisPlayItems()
    {
        List<string> itemStrings = new List<string>();
        // Hiển thị danh sách vật phẩm trong cửa hàng
        if (saveData.itemContainer.items != null)
        {
            foreach (Items item in saveData.itemContainer.items)
            {
                
                itemStrings.Add($"{item.nameItem}    {item.price}    {item.description}\n");
                
            }
            itemListText.text = string.Join("\n", itemStrings);
        }
        else
        {
            itemListText.text = "No items available in the store.";
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



    public void LoadScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
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

        // Thêm vào danh sách trong saveData
        saveData.playerContainer.players.Add(newData);

        // Thực hiện ghi xuống file
        saveData.Save();
        // Ẩn form nhập tên sau khi lưu
        FormInputPanel.SetActive(false);
        Start();
        Debug.Log("New player created and saved: " + newData.playerName);
    }

    public void PurchaseItem(int itemId)
    {
        // Tìm item trong danh sách
        Items itemToPurchase = null;
        foreach (Items item in saveData.itemContainer.items)
        {
            if (item.idItem == itemId)
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
        if (player.money >= itemToPurchase.price)
        {
            player.money -= itemToPurchase.price; // Trừ tiền
            player.ownedItems.Add(itemId); // Thêm vào danh sách vật phẩm đã sở hữu
            saveData.Save(); // Lưu lại dữ liệu sau khi mua
            moneyText.text = player.money.ToString(); // Cập nhật hiển thị tiền
            Debug.Log("Item purchased: " + itemToPurchase.nameItem);
        }
        else
        {
            Debug.Log("Not enough money to purchase this item.");
        }

    }

}