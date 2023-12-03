using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashAllCoins : MonoBehaviour
{
    private MoneyManager _moneyManager;
    public void Construct(MoneyManager moneyManager)
    {
        _moneyManager= moneyManager;
    }

    public void OnClick ()
    {
        _moneyManager.ResycleAllCurrency();
    }

}
