using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollElement : MonoBehaviour {

    float _origYrotation;

	void Start () {
        _origYrotation = transform.rotation.y;
	}

    public void SetNewRotation(float y) {
        var rot = transform.rotation;
        rot.y = _origYrotation + y;
        transform.rotation = rot;
    }
}
