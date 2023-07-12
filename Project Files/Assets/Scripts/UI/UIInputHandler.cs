using System.Collections;
using UnityEngine;

public class UIInputHandler : MonoBehaviour
{
    private InputHandler _inputHandler;

    private Vector2 _input = Vector2.zero;
    
    private void Awake()
    {
        InputHandler[] inputHandlers = FindObjectsOfType<InputHandler>();
    
        foreach (var handler in inputHandlers)
        {
            if (handler.UIInput)
            {
                _inputHandler = handler;
                break;
            }
        }
    }

    public void OnForwardPress()
    {
        _input.y = 1;
        _inputHandler.SetInput(_input);
    }

    public void OnBrakePress()
    {
        _input.y = -1;
        _inputHandler.SetInput(_input);
    }

    public void OnForwardBreakRelease()
    {
        _input.y = 0;
        _inputHandler.SetInput(_input);
    }

    public void OnLeftPress()
    {
        _input.x = -1;
        _inputHandler.SetInput(_input);
    }

    public void OnRightPress()
    {
        _input.x = 1;
        _inputHandler.SetInput(_input);
    }

    public void OnSteerRelease()
    {
        _input.x = 0;
        _inputHandler.SetInput(_input);
    }

    public void ControlSlowMo()
    {
        if (GameManager.Instance.TimeSlowed == true) GameManager.Instance.CanSlow = false;
        else GameManager.Instance.CanSlow = true;
    }
}
