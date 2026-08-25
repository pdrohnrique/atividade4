using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SaveData
{
    public string sceneName;
    public Vector3 playerPosition;
    public int currentCoins;
    public List<string> collectedCoinIDs = new List<string>();
    public bool hasCheckpoint;
}