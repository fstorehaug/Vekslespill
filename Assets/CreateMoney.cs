using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using UnityEngine;

public class CreateMoney : MonoBehaviour
{
    private MoneyManager _moneyManager;
    [SerializeField] private Currency.value _currency;

    public void Construct(MoneyManager moneyManager)
    { _moneyManager = moneyManager; }

    public void OnClick()
    {
        _moneyManager.CreateCurency(_currency);
    }

}
