using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemManager
{
    private static SystemManager instance;
    public static SystemManager Instance
    { 
        get 
        {
        if(instance == null)
            {
                instance = new SystemManager();
            }
        return instance;
        }

    }


    public delegate void MoneyChange(int currnetMoney);
    public event MoneyChange OnMoneyChange;

    private const string MoneyKey = "Money";
    private int _money = 0;

  
    public int MONEY
    {
        get => _money;

        
    }


    public bool ModifMoney(int valeu)
    {

        if(MONEY + valeu < 0)
        {
            return false;
        }

        _money = valeu;
        OnMoneyChange?.Invoke(_money);
        return true;
    }
    private SystemManager() 
    {
        LoadData();
    }

    private void LoadData()
    {
        LoadMoney();
        //other loads
    }

    private void LoadMoney()
    {
        //load form file or others DB

        _money = PlayerPrefs.GetInt(MoneyKey, 0);
    }

}
