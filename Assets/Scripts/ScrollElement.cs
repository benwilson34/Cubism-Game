using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollElement : MonoBehaviour {

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

    private float _origYrotation;

    //public void SetRandomRotation(float dy) {
    //    RotationY = _origYrotation + dy;
    //}

    public void StartScroll() {
        _origYrotation = RotationY;

    }

    public void DraggingRotation(float dy) {
        //Debug.Log("dy=" + dy);
        //var rot = transform.rotation.eulerAngles;
        //rot.y = _origYrotation + dy;
        //transform.rotation = Quaternion.Euler(rot);
        RotationY = _origYrotation + dy;
    }
}
