using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PengeLageKnapper : MonoBehaviour
{
    [SerializeField] private CreateMoney[] buttons;

    private MoneyManager _moneyManager;

    public void Construct(MoneyManager moneyManager)
    {
        _moneyManager= moneyManager;
        foreach(CreateMoney moneyButton in buttons)
        {
            moneyButton.Construct(moneyManager);
        }
    }   

}
