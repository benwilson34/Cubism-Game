using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MAIN : MonoBehaviour {

    private GameObject _subjectSpotLeft, _subjectSpotRight;
    private GameObject _subjectPF;

    private SubjectDriver _subj, _subjDrawing;

	// Use this for initialization
	void Start () {
        _subjectPF = Resources.Load("Prefabs/subject") as GameObject;

        _subjectSpotLeft = GameObject.Find("SubjectSpot_Left");
        foreach (Transform child in _subjectSpotLeft.transform)
            Destroy(child.gameObject);

        _subjectSpotRight = GameObject.Find("SubjectSpot_Right");
        foreach (Transform child in _subjectSpotRight.transform)
            Destroy(child.gameObject);

        GenerateSubject();

        GetComponent<InputController>().Init();
    }
	
	// Update is called once per frame
	//void Update () {
		
	//}

    public void GenerateSubject() {
        _subj = Instantiate(_subjectPF, _subjectSpotRight.transform).GetComponent<SubjectDriver>();

        // TODO randomize objects, rotation, more...
        // TODO somewhere I need to give input access to the left but not the right
        _subj.Generate();
        _subjDrawing = Instantiate(_subj, _subjectSpotLeft.transform).GetComponent<SubjectDriver>();
        _subj.SetNotInteractable();
    }


    public void DEBUG_TestAccuracy() {
        Debug.LogWarning("Accuracy: " + (_subj.CalculateAccuracy(_subjDrawing) * 100f) );
    }
}
