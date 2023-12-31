using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.ExceptionServices;
using UnityEngine;
using UnityEngine.Pool;
public class Currency
{
    public enum value
    {
        one,
        five,
        ten,
        twenty,
        fifty,
        onehundred,
        twohundred,
        fivehundred,
        thousand
    }

    public static int[] intvalues = { 1, 5, 10, 20, 50, 100, 200, 500, 1000 };
}

public class MoneyManager : MonoBehaviour
{
    ObjectPool<GameObject> _ones;
    ObjectPool<GameObject> _fives;
    ObjectPool<GameObject> _tens;
    ObjectPool<GameObject> _twenties;
    ObjectPool<GameObject> _fiftys;
    ObjectPool<GameObject> _onehundreds;
    ObjectPool<GameObject> _twohundreds;
    ObjectPool<GameObject> _fivehundreds;
    ObjectPool<GameObject> _oneThousands;

    [SerializeField] private GameObject[] _coinPrefabs;
    [SerializeField] private float _coinEjectForce;

    private ObjectPool<GameObject>[] _objectPools;
    private Transform _spawnPoint;

    private Dictionary<int, CurrencyMonoBehaviour> _allActiveCoins = new();

    public void Construct(Transform spawnPoint)
    {
        _spawnPoint = spawnPoint; 

        _objectPools = new ObjectPool<GameObject>[_coinPrefabs.Length];
        _objectPools[0] = _ones;
        _objectPools[1] = _fives;
        _objectPools[2] = _tens;
        _objectPools[3] = _twenties;
        _objectPools[4] = _fiftys;
        _objectPools[5] = _onehundreds;
        _objectPools[6] = _twohundreds;
        _objectPools[7] = _fivehundreds;
        _objectPools[8] = _oneThousands;
        
        for (int i = 0; i< _objectPools.Length; i++)
        {
            _objectPools[i] = SetupPool((Currency.value)i);
        }
    }

    private ObjectPool<GameObject> SetupPool(Currency.value currencyValue)
    {

        return new ObjectPool<GameObject>(() =>
        {
            GameObject curencyInstance = Instantiate(_coinPrefabs[(int)currencyValue]);
            curencyInstance.AddComponent<ConstrainToCamera>();
            curencyInstance.transform.position = _spawnPoint.position;
            curencyInstance.GetComponent<Rigidbody2D>().AddForce(new Vector2(1, UnityEngine.Random.value - .5f).normalized * (UnityEngine.Random.value + .1f) * _coinEjectForce);
            GameInstaller.InstallTouchSelectable(curencyInstance.GetComponent<CurrencyMonoBehaviour>(), currencyValue);
            return curencyInstance;
        }
        , (ob) => {
            ob.SetActive(true);
            ob.transform.position = _spawnPoint.position;
            CurrencyMonoBehaviour currency = ob.GetComponent<CurrencyMonoBehaviour>();
            _allActiveCoins.Add(currency.Id, currency);
        } 
        , (ob) => {
            _allActiveCoins.Remove(ob.GetComponent<CurrencyMonoBehaviour>().Id);
            ob.SetActive(false);
        }
        , (ob) => Destroy(ob)); 
    }
    public GameObject CreateCurency(Currency.value curencyValue)
    {
        return _objectPools[(int)curencyValue].Get();
    }

    public void ResycleCurrency(CurrencyMonoBehaviour currency)
    {
        _objectPools[(int)currency.Value].Release(currency.gameObject);
    }
    public void ResycleAllCurrency()
    {
        Dictionary<int, CurrencyMonoBehaviour> allActiveCoinsCopy = _allActiveCoins.ToDictionary((entry => entry.Key), (entry => entry.Value)); ;
        foreach(var coin in allActiveCoinsCopy.Values) {
            ResycleCurrency(coin);
        }
        _allActiveCoins.Clear();
    }
}

public class SelectionManager
{
    private Dictionary<Currency.value, Dictionary<int, CurrencyMonoBehaviour>> _dictonaryOfSelectedObjects = new();

    public SelectionManager() 
    {
        _dictonaryOfSelectedObjects = new();
        for (int i = 0; i < 9; i++)
        {
            _dictonaryOfSelectedObjects[(Currency.value)i] = new(); 
        }
    }

    public int SumValue
    {
        get
        {
            int sum = 0;
            foreach (Currency.value key in _dictonaryOfSelectedObjects.Keys)
            {
                sum += Currency.intvalues[(int)key] * _dictonaryOfSelectedObjects[key].Keys.Count;
            }
            return sum;
        }
    }

    public void OnSelected(CurrencyMonoBehaviour selectable)
    {
        if (_dictonaryOfSelectedObjects[selectable.Value] == null)
        {
            _dictonaryOfSelectedObjects[selectable.Value] = new();
        }

        _dictonaryOfSelectedObjects[selectable.Value][selectable.Id] = selectable;
    }

    public void OnDeSelect(CurrencyMonoBehaviour selectable)
    {
        if (!_dictonaryOfSelectedObjects.ContainsKey(selectable.Value))
            return;

        if (!_dictonaryOfSelectedObjects[selectable.Value].ContainsKey(selectable.Id))
            return;

        _dictonaryOfSelectedObjects[selectable.Value].Remove(selectable.Id);
    }

}