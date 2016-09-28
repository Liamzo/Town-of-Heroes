using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class JobScript : MonoBehaviour {

    /*
     * Information about the job
    */

    GameObject control;
    GameObject dayPanel;
    GameObject nightPanel;

    public Color color;

    protected string jobName;
    protected string resource;

    protected int buildingLevel;
    protected int buildingMaxLevel;

    public Dictionary<string, int>[] upgradeCosts;

    public int[] resPerWorker;
    protected int curResGain;

    public int[] maxWorkers;
    protected int curWorkers;

    public int[] maxGuards;
    protected int curGuards;

    // For events
    public EventScript[] events;

	void Awake () {
        dayPanel = GameObject.Find("JobDayPanel");
        nightPanel = GameObject.Find("JobNightPanel");
    }


	protected virtual void Start () {
        control = GameObject.FindGameObjectWithTag("Control");

        buildingLevel = 1;
	}

    void OnMouseUp () {
        if (control.GetComponent<ControlScript>().stateDay == ControlScript.StateDay.Morning) {
            dayPanel.GetComponent<JobDayPanelScript>().updateInfo(this);
            dayPanel.SetActive(true);
        } else if (control.GetComponent<ControlScript>().stateDay == ControlScript.StateDay.Night) {
            nightPanel.GetComponent<JobNightPanelScript>().updateInfo(this);
            nightPanel.SetActive(true);
        }
    }

    public void addWorker () {
        curWorkers += 1;
        curResGain += resPerWorker[buildingLevel - 1];
        control.GetComponent<ControlScript>().assignWorker();
    }
    public void removeWorker () {
        curWorkers -= 1;
        curResGain -= resPerWorker[buildingLevel - 1];
        control.GetComponent<ControlScript>().removeWorker();
    }

    public void addGuard () {
        curGuards += 1;

        control.GetComponent<ControlScript>().assignWorker();
    }
    public void removeGuard () {
        curGuards -= 1;

        control.GetComponent<ControlScript>().removeWorker();
    }

    public void resetJob () {
        curWorkers = 0;
        curGuards = 0;
        curResGain = 0;
    }

    public void upgrade () {
        buildingLevel += 1;
    }

    // Getters
    public string getJobName () {
        return jobName;
    }
    public string getResource () {
        return resource;
    }
    public int getLevel () {
        return buildingLevel;
    }
    public int getMaxLevel () {
        return buildingMaxLevel;
    }
    public int getResPerWorker () {
        return resPerWorker[buildingLevel - 1];
    }
    public int getCurResGain () {
        return curResGain;
    }
    public int getMaxWorkers () {
        return maxWorkers[buildingLevel - 1];
    }
    public int getCurWorkers () {
        return curWorkers;
    }
    public int getMaxGuards () {
        return maxGuards[buildingLevel - 1];
    }
    public int getCurGuards () {
        return curGuards;
    }



}
