﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System;
using UnityEngine.UI;

public class Debug_ConnectAndSetGazeSimple : MonoBehaviour
{
    // Gaze Position Text
    [SerializeField] Text _screenText;
    private bool _showGaze;
    private bool ShowGaze
    {
        get { return _showGaze; }
        set
        {
            _showGaze = value;
            ToggleGazeComponents(_showGaze);
        }
    }

    // Object to display gaze
    [SerializeField] ScreenToPosition _displayCube;

    IEnumerator CurrentCoroutine = null;

    // EyeLink
    private SREYELINKLib.EL_EYE eye;
    private SREYELINKLib.EyeLinkUtil elutil;
    private SREYELINKLib.EyeLink el;
    Process _myProcess = new Process();


    private void Start()
    {
        eye = SREYELINKLib.EL_EYE.EL_EYE_NONE;
        elutil = new SREYELINKLib.EyeLinkUtil();
        el = new SREYELINKLib.EyeLink();



        EventArguments.Instance.OnCDown += ELTrackerSetup;
        EventArguments.Instance.OnDDown += PerformDriftCorrect;
        EventArguments.Instance.OnQDown += StopGaze;
        EventArguments.Instance.OnPDown += StartGaze;

        _myProcess.EnableRaisingEvents = true;
        _myProcess.Exited += new EventHandler(Ps_Exited);
        _myProcess.StartInfo.UseShellExecute = true;
        _myProcess.StartInfo.RedirectStandardOutput = false;
    }
    private void ELTrackerSetup()
    {
        StopGaze();

        _myProcess.StartInfo.FileName = Environment.CurrentDirectory + "/HelperPrograms/EyeLinkTrackerSetup/EyeLinkTrackerSetup.exe";
        _myProcess.StartInfo.Arguments = "setup";

        _myProcess.Start();
        _myProcess.WaitForExit();
    }
    private void PerformDriftCorrect()
    {
        StopGaze();

        _myProcess.StartInfo.FileName = Environment.CurrentDirectory + "/HelperPrograms/EyeLinkTrackerSetup/EyeLinkTrackerSetup.exe";
        _myProcess.StartInfo.Arguments = "driftCorrect";

        _myProcess.Start();
        _myProcess.WaitForExit();
    }
    private void Ps_Exited(object sender, EventArgs e)
    {
        if (_myProcess.ExitCode == 0)
        {
            StartGaze();
        }
        else UnityEngine.Debug.LogError("Error: Tracker Setup Not Successful. Code: " + _myProcess.ExitCode);    
    }

    private void StopGaze()
    {
        if (CurrentCoroutine == null) return;

        ShowGaze = false;
        StopCoroutine(CurrentCoroutine);
        CurrentCoroutine = null;

        el.stopRecording();
		el.close();

    }
    private void StartGaze()
    {
        if (CurrentCoroutine != null) return;
		
		el.open("100.1.1.1", 0);
        el.sendCommand("link_sample_data  = LEFT,RIGHT,GAZE");
        el.sendCommand("screen_pixel_coords=0,0," + Screen.currentResolution.width.ToString() + "," + Screen.currentResolution.height.ToString());
        el.setOfflineMode();
        elutil.pumpDelay(50);

        ShowGaze = true;
        CurrentCoroutine = DrawGaze();
        StartCoroutine(CurrentCoroutine);
    }
    IEnumerator DrawGaze()
    {
        ShowGaze = true;
        el.startRecording(false, false, true, false);

        double lastSampleTime = 0.0;

        SREYELINKLib.Sample s;
        while (true)
        {
            s = el.getNewestSample();
            if (s != null && s.time != lastSampleTime)
            {
                if (eye != SREYELINKLib.EL_EYE.EL_EYE_NONE)
                {
                    if (eye == SREYELINKLib.EL_EYE.EL_BINOCULAR)
                        eye = SREYELINKLib.EL_EYE.EL_LEFT;

                    int x = (int)s.get_gx(eye);
                    int y = (int)s.get_gy(eye);

                    if ((int)x != (int)SREYELINKLib.EL_CONSTANT.EL_MISSING_DATA && (int)y != (int)SREYELINKLib.EL_CONSTANT.EL_MISSING_DATA)
                    {
                        // Update gaze position text
                        _screenText.text = "X: " + x.ToString() + ", Y: " + y.ToString();
                        // Update gaze position cube
                        _displayCube.PupilPosition = new Vector2(x, y);
                    }

                    lastSampleTime = s.time;
                }
                else
                {
                    eye = (SREYELINKLib.EL_EYE)el.eyeAvailable();
                }
            }
            yield return null;
        }
    }
    private void ToggleGazeComponents(bool state)
    {
        _displayCube.gameObject.SetActive(state);
        _screenText.gameObject.SetActive(state);
    }
}