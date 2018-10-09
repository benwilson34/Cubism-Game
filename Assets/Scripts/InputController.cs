using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputController : MonoBehaviour {

    public RectTransform scrollVis;

    private List<ScrollElement> _scrollElements;
    private float _uiWidth;

	// Use this for initialization
	void Start () {
        // TODO go through all objects in scene and get ScrollElements to update 
        _scrollElements = new List<ScrollElement>(FindObjectsOfType<ScrollElement>());
        Debug.Log("Found " + _scrollElements.Count + " scroll elements");

        //_uiWidth = GameObject.Find("ui").GetComponent<RectTransform>().rect.width;
        _uiWidth = Screen.width;
	}
	
	// Update is called once per frame
	void Update () {
        var mouseX = Input.mousePosition.x / Screen.width;
        mouseX = Mathf.Clamp(mouseX, 0, 1);
        //Debug.Log(mouseX);

        var scrollVisPos = scrollVis.position;
        scrollVisPos.x = mouseX * _uiWidth;
        scrollVis.position = scrollVisPos;

        UpdateScrollables(mouseX);
	}

    void UpdateScrollables(float rot) {
        foreach (var scr in _scrollElements) {
            scr.SetNewRotation(rot);
        }
    }
}
