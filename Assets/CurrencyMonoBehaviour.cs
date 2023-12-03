using System;
using System.Collections.Generic;
using System.Xml.Schema;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;


public class CurrencyMonoBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject _selectionIndicator;
    public UnityAction<CurrencyMonoBehaviour> OnDisableUnityAction;

    private PlayerActions input;

    private SelectionManager _selectionManager;
    private Currency.value _value;
    private IDgenerator _id;
    private bool _state = false;
    private bool _wasDragged = false;
    private bool _isTarget = false;

    private int _dragbuffer = 2;
    private int _dragcount = 0;

    public Currency.value Value => _value;
    public int Id => _id.Id;

    public void Construct(SelectionManager manager, Currency.value value)
    {
        _selectionManager = manager;
        _value = value;
        _id = new IDgenerator();
    }

    private void OnEnable()
    {
        input = new PlayerActions();
        input.Enable();
        input.TouchControlls.Drag.performed += Drag_performed;
        input.TouchControlls.Select.canceled += Select_cansled;
        input.TouchControlls.Select.started += Select_started;

        _state = false;
        _wasDragged = false;
        _isTarget = false;
    }


    //I think this calls for creating a state object that can be disposed. 
    private void OnDisable()
    {
        input.TouchControlls.Drag.performed -= Drag_performed;
        input.TouchControlls.Select.canceled -= Select_cansled;
        input.TouchControlls.Select.started -= Select_started;
        input.Disable();

        _state = false;
        _wasDragged = false;
        _isTarget = false;

        OnDeSelected();
        OnDisableUnityAction?.Invoke(this);
    }

    private void Drag_performed(InputAction.CallbackContext obj)
    {
        if (!_isTarget)
        {
            return;
        }

        if (!_wasDragged && _dragbuffer > _dragcount)
        {
            _dragcount++;
            return;
        }

        transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Pointer.current.position.ReadValue());

        _dragcount = 0;
        _wasDragged = true;

    }

    private void Select_started(InputAction.CallbackContext obj)
    {
        bool inBounds = false;
        Vector2 pointerPosition = Camera.main.ScreenToWorldPoint(Pointer.current.position.ReadValue());
        if ((int)_value < 4)
        {
            inBounds = Vector2.Distance(pointerPosition, transform.position) < .50f;
        }
        else
        {
            inBounds = (math.abs(pointerPosition.x - transform.position.x) < 1.2f) && (math.abs(pointerPosition.y - transform.position.y) < .65f);
        }

        if (!inBounds)
        {
            return;
        }

        _wasDragged = false;
        _isTarget = true;
    }

    private void Select_cansled(InputAction.CallbackContext obj)
    {
        if (!_isTarget)
        {
            return;
        }
        _isTarget = false;

        if (_wasDragged == true)
        {
            _wasDragged = false;
            return;
        }

        if (Vector2.Distance(Camera.main.ScreenToWorldPoint(Pointer.current.position.ReadValue()), transform.position) > .6f)
            return;

        if (_state)
        {
            OnDeSelected();
        }
        else
        {
            OnSelected();
        }
        _state = !_state;
    }

    private void OnSelected()
    {
        _selectionManager.OnSelected(this);
        _selectionIndicator.SetActive(true);
    }
    private void OnDeSelected()
    {
        _selectionManager.OnDeSelect(this);
        _selectionIndicator.SetActive(false);
    }

    public List<Currency.value> ChangeDown()
    {
        List<Currency.value> CoinsToMake = new();

        if (_value == Currency.value.one)
        {
            CoinsToMake.Add(Currency.value.one);
            return CoinsToMake;
        }

        int currencyValue = Currency.intvalues[(int)_value];
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
}