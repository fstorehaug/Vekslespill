using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;


public class TouchSelectable : MonoBehaviour
{
    [SerializeField] private GameObject _selectionIndicator;

    private PlayerActions input;

    private SelectionManager _selectionManager;
    private Currency.value _value;
    private IDgenerator _id;
    private bool _state = false;


    public Currency.value Value => _value;
    public int Id => _id.Id;

    public void Construct(SelectionManager manager, Currency.value value)
    {
        _selectionManager = manager;
        _value = value;
        _id = new IDgenerator();
    }

    void Start()
    {
        input = new PlayerActions();
        input.Enable();

        input.TouchControlls.Select.performed += Select_performed;
    }

    private void Select_performed(InputAction.CallbackContext obj)
    {
        if (_state)
        {
            OnDeSelected();
        }
        else
        {
            OnSelected();
        }
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

    // Update is called once per frame
    void Update()
    {

    }
}