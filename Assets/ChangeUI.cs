using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeUI : MonoBehaviour
{
    [SerializeField] private ChangeDonw _changeDonw;
    [SerializeField] private ChangeUp _changeUp;

    public void Construct(MoneyManager moneyManager)
    {
        _changeDonw.Construct(moneyManager);
        _changeUp.Construct(moneyManager);
    }

}
