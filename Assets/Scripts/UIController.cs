using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIController : MonoBehaviour {

    private static Transform _transform;
    private static Image _bossMeterFill;
    private static GameObject _dragHelperPF;
    private static GameObject _dragHelper;
    private static Text _t_timer, _t_queueLength;

    private static GameObject _dialogueCharPF;
    private static Transform _dialogueLayout;
    private static Text[] _dialogCharTexts;

    private static SpriteRenderer _cameraFlash, _loadingIcon;

    // Use this for initialization
    void Start () {
        _transform = transform;
        //_bossMeterFill = GameObject.Find("i_bossMeterFill").GetComponent<Image>();
        _dragHelperPF = Resources.Load("UI/DragHelper") as GameObject;
        _t_timer = GameObject.Find("t_timer").GetComponent<Text>();
        //_t_queueLength = GameObject.Find("t_queueLength").GetComponent<Text>();

        _dialogueCharPF = Resources.Load("UI/T_DialogueChar") as GameObject;
        _dialogueLayout = GameObject.Find("layout").transform;

        _cameraFlash = GameObject.Find("s_flash").GetComponent<SpriteRenderer>();
        _loadingIcon = GameObject.Find("s_loading").GetComponent<SpriteRenderer>();
    }

    public void ShowDragHelper(Vector3 mousePos) {
        _dragHelper = Instantiate(_dragHelperPF, _transform);
        _dragHelper.transform.position = mousePos;
    }

    public void HideDragHelper() {
        if (_dragHelper != null) {
            Destroy(_dragHelper);
            _dragHelper = null;
        }
    }

    public void UpdateTimer(string timerStr) {
        _t_timer.text = timerStr;
    }

    //public void UpdateBossMeter(float fillAmt) {
    //    _bossMeterFill.fillAmount = fillAmt;
    //}

    //public void UpdateQueueLength(int length) {
    //    _t_queueLength.text = "Queue length\n" + length;
    //}

    public void ShowDialogue(string str) {
        StartCoroutine(_ShowDialogue(str));
    }

    private const float DIALOGUE_REVEAL_RATE = 20; // per sec

    public IEnumerator _ShowDialogue(string str) {
        foreach (Transform elem in _dialogueLayout) {
            Destroy(elem.gameObject);
        }

        var chars = str.ToCharArray();
        _dialogCharTexts = new Text[chars.Length];
        for(int i = 0; i < chars.Length; i++) {
            var ch = chars[i];
            var chObj = Instantiate(_dialogueCharPF, _dialogueLayout);
            var text = chObj.GetComponent<Text>();
            text.text = ch + "";
            text.color = Color.clear;
            _dialogCharTexts[i] = text;
        }
        foreach (var text in _dialogCharTexts) {
            text.color = Color.black;
            yield return new WaitForSeconds(1 / DIALOGUE_REVEAL_RATE);
        }
        yield return null;
    }

    public void FlashCamera() {
        // TODO sfx
        StartCoroutine(_FlashCamera());
    }
    public IEnumerator _FlashCamera() {
        const float flashUp = .05f;
        yield return _cameraFlash.DOFade(.7f, flashUp)
            .WaitForCompletion();
        const float flashDown = .5f;
        yield return _cameraFlash.DOFade(0f, flashDown);
    }

    public void StartLoading() {
        StartCoroutine(_StartLoading());
    }
    public IEnumerator _StartLoading() {
        const float timePerSpin = 1f;
        const float minSpins = 2f, maxSpins = 10f;
        float numSpins = UnityEngine.Random.Range(minSpins, maxSpins);

        _loadingIcon.color = Color.white;
        var rot = new Vector3(0, 0, numSpins * 360);
        yield return _loadingIcon.transform.DORotate(rot, numSpins * timePerSpin, RotateMode.LocalAxisAdd).SetEase(Ease.Linear).WaitForCompletion();
        _loadingIcon.color = Color.clear;

        yield return null;
    }

}
