using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Currency;

public class ChangeUp : MonoBehaviour
{
    [SerializeField] private TriggerRelay _triggerRelay;

    private MoneyManager _moneyManager;

    private Dictionary<int, CurrencyMonoBehaviour> CoinsInTrigger = new();
    private Queue<Currency.value> coinsToMake = new();

    private float _timeBetweenPopInSeconds = .1f;
    private float _timeSinceLastPopInSeconds = 0f;

    public void Construct(MoneyManager moneyManager)
    {
        _moneyManager = moneyManager;
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

    public void DoChangeUp()
    {
        Dictionary<int, CurrencyMonoBehaviour> CoppyOfCoinsInTrigger = CoinsInTrigger.ToDictionary((entry => entry.Key), (entry => entry.Value));
        int sum = 0;
        foreach (var coin in CoppyOfCoinsInTrigger.Values)
        {
            sum += Currency.intvalues[(int)coin.Value];
        }
        
        foreach (var coin in DoChangeUp(sum))
        {
            coinsToMake.Enqueue(coin);
        }
        
        foreach (var coin in CoppyOfCoinsInTrigger.Values)
        {
            _moneyManager.ResycleCurrency(coin);
        }
    }
    private List<Currency.value> DoChangeUp(int sum)
    {
        List<Currency.value> CoinsToMake = new();
        Currency.value _value = Currency.value.thousand;
        int currencyValue = sum; 
        int lowerValue;
        int head = 1;
        while (currencyValue > 0)
        {
            lowerValue = Currency.intvalues[(int)_value - head];
            if (lowerValue > currencyValue)
            {
                head++;
            }
            else
            {
                CoinsToMake.Add((Currency.value)(_value - head));
                currencyValue -= lowerValue;
            }
        }

        return CoinsToMake;
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
