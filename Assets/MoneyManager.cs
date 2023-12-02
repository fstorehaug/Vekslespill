using System;
using System.Collections;
using System.Collections.Generic;
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
            _objectPools[i] = SetupPool(i);
        }
    }

    private ObjectPool<GameObject> SetupPool(int index)
    {
        return new ObjectPool<GameObject>(() => Instantiate(_coinPrefabs[index]), (ob) => ob.SetActive(true), (ob) => ob.SetActive(false), (ob) => Destroy(ob)); 
    }

    public void CreateCurency(Currency.value currency)
    {
        GameObject curency = _objectPools[(int)currency].Get();
        curency.AddComponent<ConstrainToCamera>();
        curency.transform.position = _spawnPoint.position;
        curency.GetComponent<Rigidbody2D>().AddForce(new Vector2(1, UnityEngine.Random.value - .5f).normalized * (UnityEngine.Random.value +.1f)* _coinEjectForce);

    }
}

public class SelectionManager
{
    private Dictionary<Currency.value, Dictionary<int, TouchSelectable>> _dictonaryOfSelectedObjects = new();

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

    public void OnSelected(TouchSelectable selectable)
    {
        if (_dictonaryOfSelectedObjects[selectable.Value] == null)
        {
            _dictonaryOfSelectedObjects[selectable.Value] = new();
        }

        _dictonaryOfSelectedObjects[selectable.Value][selectable.Id] = selectable;
    }

    public void OnDeSelect(TouchSelectable selectable)
    {
        if (!_dictonaryOfSelectedObjects.ContainsKey(selectable.Value))
            return;

        if (!_dictonaryOfSelectedObjects[selectable.Value].ContainsKey(selectable.Id))
            return;

        _dictonaryOfSelectedObjects[selectable.Value].Remove(selectable.Id);
    }

}