using UnityEngine;
using System.Collections;
using System.Linq;
using UnityEngine.UI;
using System.Collections.Generic;

public class EventPanelScript : MonoBehaviour {

    ControlScript control;

    GameObject eventName;
    GameObject eventText;
    GameObject[] options;

    EventScript es;

    // Use this for initialization
    void Start () {
        control = GameObject.FindGameObjectWithTag("Control").GetComponent<ControlScript>();

        eventName = GameObject.Find("EventName");
        eventText = GameObject.Find("EventText");
        options = GameObject.FindGameObjectsWithTag("BtnOption").OrderBy(go => go.name).ToArray();


        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update () {
        if (control.stateDay != ControlScript.StateDay.Day) {
            gameObject.SetActive(false);
        }
    }

    public void updateInfo (EventScript es, List<OptionScript> opts) {
        this.es = es;

        eventName.GetComponent<Text>().text = es.eventName;
        eventText.GetComponent<Text>().text = es.eventText;

        // Display and hide the option buttons
        for (int i = 0; i < options.Length; i++) {
            // Show button and update text
            if (i < opts.Count) {
                options[i].GetComponentInChildren<Text>().text = opts[i].text;
                options[i].SetActive(true);
            } else {
                options[i].SetActive(false);
            }
        }
    }
}
