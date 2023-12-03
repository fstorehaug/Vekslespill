using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

public class ChangeDonw : MonoBehaviour
{
    [SerializeField] private TriggerRelay _triggerRelay;

    private MoneyManager _moneyManager;
    
    private Dictionary<int,CurrencyMonoBehaviour> CoinsInTrigger= new();
    private Queue<Currency.value> coinsToMake= new();

    private float _timeBetweenPopInSeconds = .1f;
    private float _timeSinceLastPopInSeconds = 0f;

    public void Construct(MoneyManager moneyManager)
    {
        _moneyManager= moneyManager;
        _triggerRelay.enter += TriggerEnter;
        _triggerRelay.exit += TriggerExit;
    }

    private void OnDestroy()
    {
        _triggerRelay.enter -= TriggerEnter;
        _triggerRelay.exit -= TriggerExit;
    }

    private void TriggerEnter(Collider2D other)
    {
        CurrencyMonoBehaviour currency = other.GetComponent<CurrencyMonoBehaviour>();
        if (currency == null)
            return;

        currency.OnDisableUnityAction += RemoveCurrencyFromTrigger;
        CoinsInTrigger.Add(currency.Id, currency);
    }

    private void TriggerExit(Collider2D other)
    {
        CurrencyMonoBehaviour currency = other.GetComponent<CurrencyMonoBehaviour>();
        if (currency == null)
            return;

        RemoveCurrencyFromTrigger(currency);
    }

    public void RemoveCurrencyFromTrigger(CurrencyMonoBehaviour currency)
    {
        if (CoinsInTrigger.ContainsKey(currency.Id))
        {
            CoinsInTrigger.Remove(currency.Id);
        }
    }

    public void ChangeDown()
    {
        Dictionary<int, CurrencyMonoBehaviour> CoppyOfCoinsInTrigger = CoinsInTrigger.ToDictionary((entry => entry.Key), (entry => entry.Value));
        foreach (var coin in CoppyOfCoinsInTrigger.Values)
        {
            foreach(var smalcoin in coin.ChangeDown())
            {
                coinsToMake.Enqueue(smalcoin);
            }
            _moneyManager.ResycleCurrency(coin);
        }
    }

    private void Update()
    {
        AttemptPopCoin();
  
    }

    private void AttemptPopCoin()
    {
        if (coinsToMake.Count <= 0)
            return;

        if (_timeSinceLastPopInSeconds < _timeBetweenPopInSeconds)
        {
            _timeSinceLastPopInSeconds += Time.deltaTime;
            return;
        }

        _moneyManager.CreateCurency(coinsToMake.Dequeue());
        _timeSinceLastPopInSeconds = 0f;
    }

}
