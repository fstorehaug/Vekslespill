using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInstaller : MonoBehaviour
{
    [SerializeField] private MoneyManager _moneyManagerPrefab;
    [SerializeField] private PengeLageKnapper _pengeLageKnapperPrefab;

    [SerializeField] private Canvas GameUICanvas;
    [SerializeField] private Transform _spawnLocation;

    private MoneyManager _moneyManagerInstance;
    private PengeLageKnapper _pengeLageKnapperInstance;

    public void Awake()
    {
        _moneyManagerInstance = Instantiate(_moneyManagerPrefab);
        _pengeLageKnapperInstance = Instantiate(_pengeLageKnapperPrefab, GameUICanvas.transform);

        _pengeLageKnapperInstance.Construct(_moneyManagerInstance);
        _moneyManagerInstance.Construct(_spawnLocation);
    }

    

    


}
