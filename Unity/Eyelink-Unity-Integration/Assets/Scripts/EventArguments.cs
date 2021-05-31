using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EventArguments : MonoBehaviour
{
    public static EventArguments Instance { get; private set; }

    public Action OnF1Down;
    public Action OnF2Down;
    public Action OnF3Down;
    public Action OnCDown;
    public Action OnDDown;
    public Action OnGDown;
    public Action OnQDown;
    public Action OnPDown;

    private void Awake()
    {
        Instance = this;
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1)) OnF1Down?.Invoke();
        if (Input.GetKeyDown(KeyCode.F2)) OnF2Down?.Invoke();
        if (Input.GetKeyDown(KeyCode.F3)) OnF3Down?.Invoke();
        if (Input.GetKeyDown(KeyCode.C)) OnCDown?.Invoke();
        if (Input.GetKeyDown(KeyCode.D)) OnDDown?.Invoke();
        if (Input.GetKeyDown(KeyCode.G)) OnGDown?.Invoke();
        if (Input.GetKeyDown(KeyCode.Q)) OnQDown?.Invoke();
        if (Input.GetKeyDown(KeyCode.P)) OnPDown?.Invoke();

    }
}
