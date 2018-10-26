using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MAIN : MonoBehaviour {

    [HideInInspector]
    public static UIController uiCont;

    private GameObject _subjectSpotLeft, _subjectSpotRight;
    private GameObject _subjectPF;

    private Timer _timer;
    private CameraController _cam;
    public GameObject _paper;

    public static SubjectDriver Subject { get { return _subj; } }
    private static SubjectDriver _subj, _subjDrawing;

    public enum Step { InBetween, TakePhoto, EditImage, HandPrintedImage };
    private Step _currentStep = Step.InBetween;

	// Use this for initialization
	void Start () {
        uiCont = GameObject.Find("ui").GetComponent<UIController>();

        _timer = GetComponent<Timer>();
        _cam = GameObject.Find("Main Camera").GetComponent<CameraController>();
        _paper = GameObject.Find("paper");

        _subjectPF = Resources.Load("Prefabs/subject") as GameObject;

        _subjectSpotLeft = GameObject.Find("SubjectSpot_Left");
        _subjectSpotRight = GameObject.Find("SubjectSpot_Right");
        ClearSubjects();

        //GenerateSubject();

        GetComponent<InputController>().Init();
    }
	
	// Update is called once per frame
	//void Update () {
		
	//}

    public void GenerateSubject() {
        _subj = Instantiate(_subjectPF, _subjectSpotRight.transform).GetComponent<SubjectDriver>();
        // TODO randomize objects, rotation, more...
        _subj.Generate();
        // TODO somewhere I need to give input access to the left but not the right
        _subj.SetNotInteractable();

        _subjDrawing = Instantiate(_subjectPF, _subjectSpotLeft.transform).GetComponent<SubjectDriver>();
        _subjDrawing.Generate();
        _subjDrawing.gameObject.SetActive(false);
        //_subjDrawing.ReplaceMaterials();
    }

    public void ClearSubjects() {
        foreach (Transform child in _subjectSpotLeft.transform)
            Destroy(child.gameObject);
        foreach (Transform child in _subjectSpotRight.transform)
            Destroy(child.gameObject);
    }

    public void GiveDrawing() {
        // TODO test accuracy, figure out how that relates to BossMeter
        // TODO animate pulling the page away and giving to the subject
        // Can I project the model onto the sprite, like it's drawn on?
        // TODO dialogue about the drawing
        // TODO remove subject from queue, dequeue the next if there is a next
    }

    public static void EndTheGame() {
        // TODO 
    }

    public void DEBUG_NextStep() {
        StartCoroutine(NextStep());
    }

    IEnumerator NextStep() {
        // Remember, _currentStep here is what JUST GOT FINISHED.
        switch (_currentStep) {
            case Step.InBetween:
                // TODO Bring subject in
                GenerateSubject();
                uiCont.ShowDialogue("Yo draw my face dawggggggggg hehehehehehehehehehehhe");
                break;

            case Step.TakePhoto:
                yield return _cam.GoToPosition(CameraController.CameraPos.Right);
                // TODO flash camera
                yield return uiCont._FlashCamera();
                // TODO loading icon on screen
                StartCoroutine(_cam.GoToPosition(CameraController.CameraPos.Center));
                yield return uiCont._StartLoading();
                // TODO show model on screen
                _subjDrawing.gameObject.SetActive(true);
                break;

            case Step.EditImage:
                StartCoroutine(_cam.GoToPosition(CameraController.CameraPos.Left));
                uiCont.ShowDialogue("Accuracy: " + (_subj.CalculateAccuracy(_subjDrawing) * 100f));
                // TODO print image out - somehow project model onto it??
                // maybe printer jams heh
                yield return Print();
                yield return _cam.GoToPosition(CameraController.CameraPos.Center);
                _paper.transform.localScale = Vector3.one;
                _paper.transform.position = new Vector3(-3.5f, -9, -17);
                const float moveDur = 1.2f;
                yield return _paper.transform.DOMoveY(-.3f, moveDur).WaitForCompletion();
                break;

            case Step.HandPrintedImage:
                // TODO hand printed photo to subject
                var front = _paper.transform.Find("front");
                var back = _paper.transform.Find("back");
                const float turnDur = .8f;
                yield return front.DOScaleX(0, turnDur / 2).SetEase(Ease.Linear).WaitForCompletion();
                yield return back.DOScaleX(1, turnDur / 2).WaitForCompletion();
                // TODO dismiss subject

                if (_subj.MakeChoiceAboutPrint(_subjDrawing)) {
                    yield return uiCont._ShowDialogue("hey that's pretty good");
                } else {
                    yield return uiCont._ShowDialogue("damn son what did you do to my face. 0/10");
                }
                // TODO close the "file" on computer?
                ClearSubjects();
                GameObject.Destroy(_paper);
                break;
        }

        _currentStep = (Step)(((int)_currentStep + 1) % Enum.GetValues(typeof(Step)).Length);

        yield return null;
    }

    private GameObject _paperPF;

    IEnumerator Print() {
        if (_paperPF == null)
            _paperPF = Resources.Load("Prefabs/paper") as GameObject;

        _paper = Instantiate(_paperPF);
        float randTime = UnityEngine.Random.Range(2, 8);
        yield return _paper.transform.DOMoveY(-4.5f, randTime).WaitForCompletion();
        // TODO falling off screen or getting picked up?
        const float fallTime = .4f;
        yield return _paper.transform.DOMoveY(-9f, fallTime).WaitForCompletion();
        yield return null;
    }
}
