using UnityEngine;
using System;
using System.Collections.Generic;


// Lớp này dùng để chứa danh sách các player, giúp việc lưu trữ và quản lý dữ liệu dễ dàng hơn
[Serializable]
public class PlayerContainer
{
    public List<PlayerData> players=new List<PlayerData>();
}