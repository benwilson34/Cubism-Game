using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour {

    private const int SEC_PER_MIN = 60;
    private int _seconds = 0;

    // Use this for initialization
    void Start () {
        StartTimer();
	}

    public void StartTimer() {
        InvokeRepeating("IncTimer", 0, 1);
    }

    public void IncTimer() {
        _seconds++;
        var str = string.Format("{0:D2}:{1:D2}", _seconds / SEC_PER_MIN, _seconds % 60);
        MAIN.uiCont.UpdateTimer(str);
    }
}
