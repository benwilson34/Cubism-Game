using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMeter : MonoBehaviour {

    private const float METER_MAX = 1f;

    private float _meter;
    private float _increasePerSec; // how much the meter increases every second

	// Use this for initialization
	void Start () {
        _meter = 0;
        _increasePerSec = .005f;
	}
	
	// Update is called once per frame
	//void Update () {
 //       IncreaseMeter(Time.deltaTime * _increasePerSec);
	//}

 //   public void IncreaseMeter(float amt) {
 //       _meter += amt;
 //       MAIN.uiCont.UpdateBossMeter(_meter);
 //       if (_meter > METER_MAX) { // game over
 //           MAIN.EndTheGame();
 //       }
 //   }

 //   public void DecreaseMeter(float amt) {
 //       _meter -= amt;
 //       if (_meter < 0)
 //           _meter = 0;
 //       MAIN.uiCont.UpdateBossMeter(_meter);
 //   }
}
