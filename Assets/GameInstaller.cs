using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using UnityEngine;

public class GameInstaller : MonoBehaviour
{
    [SerializeField] private MoneyManager _moneyManagerPrefab;
    [SerializeField] private PengeLageKnapper _pengeLageKnapperPrefab;
    [SerializeField] private ChangeUI _changeUIPrefab;

    [SerializeField] private Canvas GameUICanvas;
    [SerializeField] private Transform _spawnLocation;

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

        _pengeLageKnapperInstance.Construct(_moneyManagerInstance);
        _moneyManagerInstance.Construct(_spawnLocation);
        _changeUIInstance.Construct(_moneyManagerInstance);
    }

    public static void InstallTouchSelectable(CurrencyMonoBehaviour item, Currency.value value)
    {
        item.Construct(_selectionManager, value);
    }

    


}
