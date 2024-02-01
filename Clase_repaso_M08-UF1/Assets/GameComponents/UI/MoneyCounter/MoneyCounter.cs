using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoneyCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;

    private const string MoneyLableTranslationKey = "Money_label";

    private void Start()
    {
        SystemManager sm = SystemManager.Instance;
        UpdateTextCurrentMoney(sm.MONEY);
        sm.OnMoneyChange += UpdateTextCurrentMoney;
    }

    private void UpdateTextCurrentMoney(int money)
    {
        //string text = TranslationMAnager.GetString(MoneyLableTranslationKey, money.ToString());
        string text = "Money: " + money.ToString();
        _text.text = text; 
    }

    private void OnDestroy()
    {
        SystemManager sm = SystemManager.Instance;
        sm.OnMoneyChange -= UpdateTextCurrentMoney;
    }

    //simpre que haagmaos una supscripcion de un evento tenemos que manejar la des supscripcion del evento
}
