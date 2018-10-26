using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueDriver : MonoBehaviour {

    private const float WAIT_DUR_MIN = 2, WAIT_DUR_MAX = 10;

    private float _waitDuration;
    private float _currentWait;
    private bool _waitingForResponse = false;

	// Use this for initialization
	void Start () {
		// TODO load dialogue resources
	}

    // Update is called once per frame
    void Update() {
        if (_waitingForResponse) {
            _currentWait += Time.deltaTime * _waitDuration;
        }
    }

    public void StartNewSubject() {
        // TODO
    }

    public void ChooseDialogueOption(int opt) {
        // TODO switch btwn three options
    }

    public void SendDialogue(string str) {
        _waitingForResponse = false;
        MAIN.uiCont.ShowDialogue(str);
    }

    public void StartWaiting() {
        _currentWait = 0;
        _waitingForResponse = true;
    }

    public void SetWaitDuration(float patience) {
        float point = (float) MAIN.Subject.Patience / SubjectDriver.PATIENCE_MAX;
        _waitDuration = Mathf.Lerp(WAIT_DUR_MIN, WAIT_DUR_MAX, point);
    }
}
