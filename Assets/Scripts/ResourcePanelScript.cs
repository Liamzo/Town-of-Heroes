using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ResourcePanelScript : MonoBehaviour {

    GameObject control;

	// Use this for initialization
	void Start () {
        control = GameObject.FindGameObjectWithTag("Control");
	}

    public void updateDisplay () {
        foreach (Transform child in transform) {
            child.GetComponent<Text>().text = child.name + ": " + control.GetComponent<ControlScript>().resources[child.name];
        }
    }

}
