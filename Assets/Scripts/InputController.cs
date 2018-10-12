using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputController : MonoBehaviour {

    public RectTransform scrollVis;

    private const float SCROLL_ROT_DIS = 180; // in pixels
    private const float FULL_ROT = 360;

    private List<ScrollElement> _scrollElements;
    private float _uiWidth;
    private Vector3 _mouseDownPos;
    private ScrollElement _mouseScr;

    // Use this for initialization
    public void Init() {
        // TODO go through all objects in scene and get ScrollElements to update 
        //_scrollElements = new List<ScrollElement>(FindObjectsOfType<ScrollElement>());
        //Debug.Log("Found " + _scrollElements.Count + " scroll elements");

        //_uiWidth = GameObject.Find("ui").GetComponent<RectTransform>().rect.width;
        _uiWidth = Screen.width;
    }

    // Update is called once per frame
    void Update () {
        //var mouseX = Input.mousePosition.x / Screen.width;
        //mouseX = Mathf.Clamp(mouseX, 0, 1);
        ////Debug.Log(mouseX);

        //var scrollVisPos = scrollVis.position;
        //scrollVisPos.x = mouseX * _uiWidth;
        //scrollVis.position = scrollVisPos;

        //UpdateScrollables(mouseX);
        var mouseState = GetMouseState();
        //if(mouseState != MouseState.None)
        //    Debug.Log(mouseState.ToString());

        switch (mouseState) {
            case MouseState.MouseDown:
                _mouseDownPos = Input.mousePosition;
                Ray ray = Camera.main.ScreenPointToRay(_mouseDownPos);
                foreach (var hit in Physics.RaycastAll(ray)) {
                    ScrollElement scrEl = hit.transform.parent.gameObject.GetComponent<ScrollElement>();
                    if (scrEl != null && scrEl.Interactable) {
                        _mouseScr = scrEl;
                        _mouseScr.StartScroll();
                        Debug.Log("Hit ScrollElement! on " + hit.transform.name);
                        break;
                    }
                }

                break;

            case MouseState.MouseHold:
                if (_mouseScr == null)
                    break;

                var mousePos = Input.mousePosition;
                var mouseDis = mousePos.x - _mouseDownPos.x;
                //mouseDis *= mousePos.x < _mouseDownPos.x ? -1 : 1;
                //Debug.Log(mouseDis);

                var ratio = -1 * (mouseDis / SCROLL_ROT_DIS) * FULL_ROT;
                _mouseScr.DraggingRotation(ratio);

                break;

            case MouseState.MouseUp:
                if (_mouseScr != null) {
                    _mouseScr = null;
                }
                break;

            case MouseState.None:
                break;
        }
    }


    private bool _mouseThisFrame = false, _mouseLastFrame;
    private enum MouseState { None, MouseDown, MouseHold, MouseUp };

    MouseState GetMouseState() {
        _mouseLastFrame = _mouseThisFrame;
        _mouseThisFrame =  Input.GetMouseButton(0);

        if      (!_mouseLastFrame  &&  _mouseThisFrame) return MouseState.MouseDown;
        else if ( _mouseLastFrame  &&  _mouseThisFrame) return MouseState.MouseHold;
        else if ( _mouseLastFrame  && !_mouseThisFrame) return MouseState.MouseUp;
        else return MouseState.None;
    }

}
