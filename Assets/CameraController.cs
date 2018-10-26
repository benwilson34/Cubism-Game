using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraController : MonoBehaviour {

    public float leftPos, centerPos, rightPos;
    public enum CameraPos { Left, Center, Right };

	//// Use this for initialization
	//void Start () {
		
	//}
	
	//// Update is called once per frame
	//void Update () {
		
	//}

    public IEnumerator GoToPosition(CameraPos pos) {
        const float moveDur = 2f;
        Tween t = null;
        switch (pos) {
            case CameraPos.Left:
                t = transform.DOMoveX(leftPos, moveDur);
                break;
            case CameraPos.Center:
                t = transform.DOMoveX(centerPos, moveDur);
                break;
            case CameraPos.Right:
                t = transform.DOMoveX(rightPos, moveDur);
                break;
        }

        yield return t.WaitForCompletion();
    }
}
