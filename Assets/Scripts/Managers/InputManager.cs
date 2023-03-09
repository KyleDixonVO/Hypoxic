using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager inputManager;

    private bool _escapePressed;
    public bool escapePressed;

    private bool _spacedPressed;
    public bool spacePressed;

    private bool _wPressed;
    public bool wPressed;

    private bool _sPressed;
    public bool sPressed;

    private bool _aPressed;
    public bool aPressed;

    private bool _dPressed;
    public bool dPressed;

    private bool _ePressed;
    public bool ePressed;
    public bool eCycled;

    private bool _rPressed;
    public bool rPressed;

    private bool _capsPressed;
    public bool capsPressed;

    private bool _tabPressed;
    public bool tabPressed;
    private bool tabCycled;

    private int _lastNumKeyPressed;
    public int lastNumKeyPressed;

    private void Awake()
    {
        if (inputManager == null)
        {
            inputManager = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (inputManager != null && inputManager != this)
        {
            Destroy(this.gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _escapePressed = false;
        eCycled = true;
        _tabPressed = false;
        tabCycled = true;
        _lastNumKeyPressed = 0;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateInputs();
        UpdateLastNumPressed();
        //Debug.Log(eCycled);
    }

    public void ResetEscape()
    {
        _escapePressed = false;
    }
    public void ResetTab()
    {
        _tabPressed = false;
    }

    void UpdateLastNumPressed()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) _lastNumKeyPressed = 0;
        else if (Input.GetKeyDown(KeyCode.Alpha2)) _lastNumKeyPressed = 1;
        else if (Input.GetKeyDown(KeyCode.Alpha3)) _lastNumKeyPressed = 2;

        lastNumKeyPressed = _lastNumKeyPressed;
    }

    void UpdateInputs()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _escapePressed = !_escapePressed;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            _spacedPressed = true;
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            _spacedPressed = false;
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            _wPressed = true;
        }
        else if (Input.GetKeyUp(KeyCode.W))
        {
            _wPressed = false;
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            _sPressed = true;
        }
        else if (Input.GetKeyUp(KeyCode.S))
        {
            _sPressed = false;
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            _aPressed = true;
        }
        else if (Input.GetKeyUp(KeyCode.A))
        {
            _aPressed = false;
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            _dPressed = true;
        }
        else if (Input.GetKeyUp(KeyCode.D))
        {
            _dPressed = false;
        }

        if (Input.GetKeyDown(KeyCode.CapsLock))
        {
            _capsPressed = true;
        }
        else if (Input.GetKeyUp(KeyCode.CapsLock))
        {
            _capsPressed = false;
        }

        if (Input.GetKey(KeyCode.E))
        {
            _ePressed = true;
        }
        else if (Input.GetKeyUp(KeyCode.E))
        {
            _ePressed = false;
            eCycled = true;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            _rPressed = true;
        }
        else if (Input.GetKeyUp(KeyCode.R))
        {
            _rPressed = false;
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            //Debug.Log(_tabPressed);
            if (!tabCycled) return;
            _tabPressed = !_tabPressed;
            tabCycled = false;
        }
        else if (Input.GetKeyUp(KeyCode.Tab))
        {
            tabCycled = true;
        }

        tabPressed = _tabPressed;
        escapePressed = _escapePressed;
        spacePressed = _spacedPressed;
        wPressed = _wPressed;
        sPressed = _sPressed;
        dPressed = _dPressed;
        aPressed = _aPressed;
        capsPressed = _capsPressed;
        ePressed = _ePressed;
        rPressed = _rPressed;
    }

    public void SetECycledfalse()
    {
        eCycled = false;
    }


}
