using UnityEngine;
using UnityEngine.InputSystem;


public class TouchSelectable : MonoBehaviour 
{
    [SerializeField] private GameObject _selectionIndicator;

    private PlayerActions input;

    private SelectionManager _selectionManager;
    private Currency.value _value;
    private IDgenerator _id;
    private bool _state = false;
    private bool _wasDragged = false;
    private bool _isTarget = false;

    private int _dragbuffer = 5;
    private int _dragcount = 0;

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

        input.TouchControlls.Drag.performed += Drag_performed;
        input.TouchControlls.Select.canceled += Select_cansled;
        input.TouchControlls.Select.started += Select_started;
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

        _dragcount= 0;
       _wasDragged= true;

    }

    private void Select_started(InputAction.CallbackContext obj)
    {
        if (Vector2.Distance(Camera.main.ScreenToWorldPoint(Pointer.current.position.ReadValue()), transform.position) > .6f)
        { 
        return;
        } 
        
        _wasDragged= false;
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

    // Update is called once per frame
    void Update()
    {

    }
}