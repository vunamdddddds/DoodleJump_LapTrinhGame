using System.Collections; 
using System;
using System.Collections.Generic;
using UnityEngine;



[Serializable]
public class PlayerData
{
    
    public string playerName;
    public int highScore;
    public int money;

    public List<scoreRecord> scoreHistory ;

    public List<int> ownedItems;

    public int currentItems;
  

}