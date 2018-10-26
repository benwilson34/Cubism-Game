using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubjectDriver : MonoBehaviour {

    public ScrollElement[] scrollElements;
    public Material sketchMaterial;

    public const int PATIENCE_MIN = 0, PATIENCE_MAX = 100;
    private const int PATIENCE_START_MIN = 20, PATIENCE_START_MAX = 80;

    public int Patience { get { return _patience; } }
    private int _patience;

	// Use this for initialization
	//void Start () {
        
 //   }

    public void SetNotInteractable() {
        foreach (var elem in scrollElements)
            elem.Interactable = false;
    }

    public void ReplaceMaterials() {
        var meshes = GetComponentsInChildren<MeshRenderer>();
        Debug.Log("Replacing " + meshes.Length + " materials...");
        foreach (var mesh in meshes) {
            mesh.materials = new Material[1] { sketchMaterial };
        }
    }

    public void Generate() {
        _patience = Random.Range(PATIENCE_START_MIN, PATIENCE_START_MAX + 1);

        for(int i = 0; i < scrollElements.Length; i++) {
            var elem = scrollElements[i];
            var elemRot = Random.Range(-90f, 90f);
            elem.RotationY = elemRot;

            var elemScale = Random.Range(.2f, 2f);
            elem.Scale = elemScale;
            // TODO random model choice
        }
    }

    public float CalculateAccuracy(SubjectDriver otherDriver) {
        float sum = 0;
        var otherScrollElems = otherDriver.scrollElements;
        for (int i = 0; i < scrollElements.Length; i++) {
            var error = scrollElements[i].CalcError(otherScrollElems[i]);
            Debug.LogWarning("--- error for "+ scrollElements[i].gameObject.name+": " + error);
            sum += 1f - error;
        }
        return sum / scrollElements.Length; // average
    }

    public void ChangePatience(int amt) {
        _patience += amt;
        _patience = Mathf.Clamp(_patience, PATIENCE_MIN, PATIENCE_MAX);
    }

    public bool MakeChoiceAboutPrint(SubjectDriver otherDriver) {
        // TODO relationship between accuracy and acceptance
        // for now it'll just be random
        return Random.Range(0, 1f) > 0.5f;
    }
}
