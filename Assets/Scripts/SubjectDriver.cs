using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubjectDriver : MonoBehaviour {

    public ScrollElement[] scrollElements;

    private const float ACCURACY_FORGIVENESS = 100f; // in degrees

    public float[] Solutions { get { return _rotationSolutions; } }
    private float[] _rotationSolutions;

	// Use this for initialization
	//void Start () {
        
 //   }

    public void SetNotInteractable() {
        foreach (var elem in scrollElements)
            elem.Interactable = false;
    }

    public void Generate() {
        _rotationSolutions = new float[scrollElements.Length];
        for(int i = 0; i < scrollElements.Length; i++) {
            var elem = scrollElements[i];
            var elemRot = Random.Range(-90f, 90f);
            elem.RotationY = elemRot;
            _rotationSolutions[i] = elemRot;
        }
    }

    public float CalculateAccuracy(SubjectDriver otherDriver) {
        float sum = 0;
        var actualScrollElems = otherDriver.scrollElements;
        for (int i = 0; i < scrollElements.Length; i++) {
            var actual = actualScrollElems[i].transform.rotation;
            var solution = Quaternion.Euler(0, _rotationSolutions[i], 0);
            //Debug.Log("actual: " + actual + ", solution: " + solution);
            var diff = Quaternion.Angle(actual, solution);

            var error = diff / ACCURACY_FORGIVENESS;
            error = Mathf.Clamp(error, 0, 1);
            Debug.LogWarning("--- SubError: " + error + " with diff="+ diff);
            sum += 1f - error;
        }
        return sum / scrollElements.Length;
    }
}
