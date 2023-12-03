using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using UnityEngine;

public class GameInstaller : MonoBehaviour
{
    [SerializeField] private MoneyManager _moneyManagerPrefab;
    [SerializeField] private PengeLageKnapper _pengeLageKnapperPrefab;
    [SerializeField] private ChangeUI _changeUIPrefab;
    [SerializeField] private TrashAllCoins _trashAllCoinsPrefab;

    [SerializeField] private Canvas GameUICanvas;
    [SerializeField] private Transform _spawnLocation;

    private static TrashAllCoins _trashAllCoinsInstance;
    private static MoneyManager _moneyManagerInstance;
    private static PengeLageKnapper _pengeLageKnapperInstance;
    private static ChangeUI _changeUIInstance;

    private static SelectionManager _selectionManager;

    public void Awake()
    {
        _selectionManager = new SelectionManager();

        _moneyManagerInstance = Instantiate(_moneyManagerPrefab);
        _pengeLageKnapperInstance = Instantiate(_pengeLageKnapperPrefab, GameUICanvas.transform);
        _changeUIInstance = Instantiate(_changeUIPrefab, GameUICanvas.transform);
        _trashAllCoinsInstance = Instantiate(_trashAllCoinsPrefab, GameUICanvas.transform);

        _trashAllCoinsInstance.Construct(_moneyManagerInstance);
        _pengeLageKnapperInstance.Construct(_moneyManagerInstance);
        _changeUIInstance.Construct(_moneyManagerInstance);
        _moneyManagerInstance.Construct(_spawnLocation);
    }

    public static void InstallTouchSelectable(CurrencyMonoBehaviour item, Currency.value value)
    {
        item.Construct(_selectionManager, value);
    }

    


}
