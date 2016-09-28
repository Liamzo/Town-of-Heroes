using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ControlScript : MonoBehaviour {

    /*
     * Stores information used by other scripts
    */

    public enum StateDay {Morning, Day, Night };
    public StateDay stateDay = StateDay.Morning;

    public Dictionary<string, int> resources;
    JobScript[] jobs;

    int maxWorkers;
    int availWorkers;
    int heroes;
    int day;

    GameObject btnStartDay;
    GameObject btnStartMorning;

    // For Events
    GameObject eventPanel;
    List<EventScript> todoEvents;
    List<OptionScript> doOptions;
    float eventLength;
    float eventTimer = 0f;
    bool waitOnEvent = false;


    void Awake () {
        eventPanel = GameObject.Find("EventPanel");
    }

    // Use this for initialization
    void Start () {
        resources = new Dictionary<string, int>();
        resources.Add("Iron", 0);
        resources.Add("Clay", 0);
        resources.Add("Wood", 0);
        resources.Add("Food", 0);
        resources.Add("Gold", 0);

        jobs = FindObjectsOfType<JobScript>() as JobScript[];

        maxWorkers = 5;
        availWorkers = maxWorkers;

        btnStartDay = GameObject.Find("BtnStartDay");
        btnStartMorning = GameObject.Find("BtnStartMorning");
        btnStartMorning.SetActive(false);

        GameObject.Find("ResourcePanel").GetComponent<ResourcePanelScript>().updateDisplay();

        todoEvents = new List<EventScript>();
        doOptions = new List<OptionScript>();

    }
	
	// Update is called once per frame
	void Update () {
        if (stateDay == StateDay.Day) {
            eventTimer += Time.deltaTime;

            if (eventTimer >= eventLength && waitOnEvent == false && todoEvents.Count > 0) {
                // Get event to do
                waitOnEvent = true;
                int i = Random.Range(0, todoEvents.Count);
                EventScript doEvent = todoEvents[i];
                todoEvents.RemoveAt(i);

                // Get list of options whose requirements are met
                foreach (OptionScript opt in doEvent.options) {
                    //TODO: check requirements
                    doOptions.Add(opt);
                }

                eventPanel.GetComponent<EventPanelScript>().updateInfo(doEvent, doOptions);
                eventPanel.SetActive(true);
            }

            if (eventTimer >= eventLength && waitOnEvent == false && todoEvents.Count == 0) {
                startNight();
            }
        }

	}

    public int getAvailWorkers () {
        return availWorkers;
    }

    public void assignWorker () {
        availWorkers -= 1;
    }

    public void removeWorker () {
        availWorkers += 1;
    }

    public void startMorning () {
        // Reset all workers
        stateDay = StateDay.Morning;
        btnStartDay.SetActive(true);
        btnStartMorning.SetActive(false);

        foreach (JobScript js in jobs) {
            js.resetJob();
        }
    }

    public void startDay () {
        // Find all jobs with atleast one worker, and decide if an event should occur
        foreach (JobScript job in jobs) {
            if (job.getCurWorkers() > 0) {
                // TODO: First check if an event will happen, then look at all liklihoods
                int i = Random.Range(0, job.events.Length);
                todoEvents.Add(job.events[i]);
            }
        }

        eventLength = Random.Range(3, 7);

        stateDay = StateDay.Day;
        btnStartDay.SetActive(false);
    }

    void startNight () {
        // Add resources 
        stateDay = StateDay.Night;
        eventTimer = 0f;
        btnStartMorning.SetActive(true);

        foreach (JobScript js in jobs) {
            resources[js.getResource()] += js.getCurResGain();
        }

        //Update display
        GameObject.Find("ResourcePanel").GetComponent<ResourcePanelScript>().updateDisplay();
    }

    // For events
    public void doEventOption (int i) {
        // Select outcome
        //TODO: Check liklihood
        if (doOptions[i].outcomes.Length > 0) {
            int j = Random.Range(0, doOptions[i].outcomes.Length);
            OutcomeScript outcome = doOptions[i].outcomes[j];

            for (int k = 0; k < outcome.gainOut.Length; k++) {
                // Check if not code
                if (outcome.gainOut[k] > -900 && outcome.gainOut[k] < 900) {
                    //TODO: handle non resources
                    resources[outcome.resOut[k]] += outcome.gainOut[k];
                }
            }
        }

        eventTimer = 0f;
        eventLength = Random.Range(3, 7);
        waitOnEvent = false;

        doOptions.Clear();
        eventPanel.SetActive(false);
    }

}
