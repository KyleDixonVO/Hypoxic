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

    private void Awake()
    {
        if (inputManager != null && inputManager != this)
        {
            Destroy(this.gameObject);
        }        
        else if (inputManager == null)
        {
            inputManager = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateInputs();
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

        escapePressed = _escapePressed;
        spacePressed = _spacedPressed;
        wPressed = _wPressed;
        sPressed = _sPressed;
        dPressed = _dPressed;
        aPressed = _aPressed;
    }


}
