using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollElement : MonoBehaviour {

    // if this should be different per facepart, make public and not const, set in PF
    private const float SCALE_MIN = 0.2f, SCALE_MAX = 2f;
    private const float ROT_MIN = 0f, ROT_MAX = 180f;

    public bool Interactable { get { return _interactable; } set { _interactable = value; } }
    private bool _interactable = true;

    public float RotationY {
        get {
            return transform.rotation.eulerAngles.y;
        }

        set {
            var rot = transform.rotation.eulerAngles;
            rot.y = value;
            transform.rotation = Quaternion.Euler(rot);
        }
    }

    public float Scale {
        get {
            return transform.localScale.x; // return x scale since it scales uniformly
        }

        set {
            transform.localScale = new Vector3(value, value, value);
        }
    }

    private float _origYrotation;
    private float _origScale;

    //public void SetRandomRotation(float dy) {
    //    RotationY = _origYrotation + dy;
    //}

    public void StartScroll() {
        _origYrotation = RotationY;
        _origScale = Scale;
    }

    public void DraggingRotation(float dx) {
        //Debug.Log("dy=" + dy);
        //var rot = transform.rotation.eulerAngles;
        //rot.y = _origYrotation + dy;
        //transform.rotation = Quaternion.Euler(rot);

        RotationY = _origYrotation + dx;
        // TODO clamp rotation
    }

    public void DraggingScale(float dy) {
        var newScale = _origScale + dy;
        newScale = Mathf.Clamp(newScale, SCALE_MIN, SCALE_MAX);
        Scale = newScale;
    }

    private const float ROT_ERR_FORGIVENESS = 100f; // in degrees
    private const float SCALE_ERR_FORGIVENESS = .5f; // in scale units

    public float CalcError(ScrollElement other) {
        var rotDiff = Quaternion.Angle(transform.rotation, other.transform.rotation);
        var error = Mathf.Clamp(rotDiff / ROT_ERR_FORGIVENESS, 0, 1); 

        var scaleDiff = Mathf.Abs(Scale - other.Scale);
        error += Mathf.Clamp(scaleDiff / SCALE_ERR_FORGIVENESS, 0, 1);
        return error / 2f;
    }
}
