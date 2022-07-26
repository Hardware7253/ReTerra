using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class MainBalancing : MonoBehaviour
{
    int turn;

    int tileGrowChancePerTurn = 50;
    int seedSpreadChancePerAdjacentTile = 50;

    public event Action<int, int> onSendBalanceInfo;
    public event Action onNewTurn;

    public event Action<int, int, int> setStatDisplay;
    public void IncrementTurn()
    {
        turn++;
        onNewTurn?.Invoke();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }


    // Update is called once per frame
    void Update()
    {
        onSendBalanceInfo?.Invoke(tileGrowChancePerTurn, seedSpreadChancePerAdjacentTile);
    }
}
