using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputController : MonoBehaviour {

    public RectTransform scrollVis;

    private const float SCROLL_ROT_DIS = 180; // in pixels
    private const float FULL_ROT = 360;

    private const float SCROLL_SCALE_DIS = 300; // in pixels

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
                        MAIN.uiCont.ShowDragHelper(_mouseDownPos);
                        Debug.Log("Hit ScrollElement! on " + hit.transform.name);
                        break;
                    }
                }

                break;

            case MouseState.MouseHold:
                if (_mouseScr == null) // if the user didn't click a scrollable
                    break;

                var mousePos = Input.mousePosition;
                if(_dragMode == DragMode.None)
                    CheckForDragMode(mousePos); //only check if a mode hasn't been determined yet

                if (_dragMode == DragMode.Horiz)
                    HandleHorizScroll(mousePos);
                else if (_dragMode == DragMode.Vert)
                    HandleVertScroll(mousePos);

                break;

            case MouseState.MouseUp:
                if (_mouseScr != null) {
                    _mouseScr = null;
                    MAIN.uiCont.HideDragHelper();
                }
                _dragMode = DragMode.None;
                break;

            case MouseState.None:
                break;
        }
    }

    void HandleHorizScroll(Vector3 mousePos) {
        var mouseDis = mousePos.x - _mouseDownPos.x;
        //mouseDis *= mousePos.x < _mouseDownPos.x ? -1 : 1;
        //Debug.Log(mouseDis);

        var ratio = -1 * (mouseDis / SCROLL_ROT_DIS) * FULL_ROT;
        _mouseScr.DraggingRotation(ratio);
    }

    void HandleVertScroll(Vector3 mousePos) {
        var mouseDis = mousePos.y - _mouseDownPos.y;

        var ratio = (mouseDis / SCROLL_SCALE_DIS);
        _mouseScr.DraggingScale(ratio);
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



    private const int DRAG_MODE_DISTANCE = 30; // in pixels
    private enum DragMode { None, Horiz, Vert };
    private DragMode _dragMode = DragMode.None;

    void CheckForDragMode(Vector3 mousePos) {
        if (Vector3.Distance(_mouseDownPos, mousePos) > DRAG_MODE_DISTANCE) {
            var diff = mousePos - _mouseDownPos;
            _dragMode = Mathf.Abs(diff.x) > Mathf.Abs(diff.y) ? DragMode.Horiz : DragMode.Vert;
            MAIN.uiCont.HideDragHelper();
        }
    }

}
