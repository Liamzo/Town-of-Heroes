using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class JobNightPanelScript : MonoBehaviour {

    ControlScript control;

    GameObject jobName;
    GameObject curLevels;
    GameObject nextLevels;
    GameObject costLevels;

    JobScript job;

    // Use this for initialization
    void Start () {
        control = GameObject.FindGameObjectWithTag("Control").GetComponent<ControlScript>();

        jobName = GameObject.Find("JobName");
        curLevels = GameObject.Find("CurLevels");
        nextLevels = GameObject.Find("NextLevels");
        costLevels = GameObject.Find("CostLevels");

        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update () {
        if (control.stateDay != ControlScript.StateDay.Night) {
            gameObject.SetActive(false);
        }
    }

    public void updateInfo (JobScript job) {
        this.job = job;

        // Name
        jobName.GetComponent<Text>().text = job.getJobName() + " - " + job.getLevel();
        // Current levels
        curLevels.GetComponent<Text>().text = "Workers: " + job.getMaxWorkers() + " Guards:" + job.getMaxGuards() + " Gain: " + job.getResPerWorker();
        // Next levels
        if (job.getLevel() < job.getMaxLevel()) {
            nextLevels.GetComponent<Text>().text = "Workers: " + job.maxWorkers[job.getLevel()] + " Guards:" + job.maxGuards[job.getLevel()] + " Gain: " + job.resPerWorker[job.getLevel()];
        } else {
            nextLevels.GetComponent<Text>().text = "-";
        }
        // Cost
        if (job.getLevel() < job.getMaxLevel()) {
            costLevels.GetComponent<Text>().text = "";
            Dictionary<string, int> costs = job.upgradeCosts[job.getLevel() - 1];
            foreach (KeyValuePair<string, int> cost in costs) {
                costLevels.GetComponent<Text>().text += cost.Key + " " + control.resources[cost.Key] + "/" + cost.Value;
            }
        } else {
            costLevels.GetComponent<Text>().text = "-";
        }

        this.job = job;

    }

    public void upgradeBuilding () {
        bool canUpgrade = true;

        // Check if we can affoard to upgrade
        if (job.getLevel() < job.getMaxLevel()) {
            Dictionary<string, int> costs = job.upgradeCosts[job.getLevel() - 1];
            foreach (KeyValuePair<string, int> cost in costs) {
                if (control.resources[cost.Key] < cost.Value) {
                    canUpgrade = false;
                    break;
                }
            }

            // If we can upgrade, remove the resources and level up
            if (canUpgrade) {
                foreach (KeyValuePair<string, int> cost in costs) {
                    control.resources[cost.Key] -= cost.Value;
                }

                job.upgrade();
                //Update display
                this.updateInfo(job);              
                GameObject.Find("ResourcePanel").GetComponent<ResourcePanelScript>().updateDisplay();
            }
        }
    }

    public void exitPanel () {
        gameObject.SetActive(false);
    }
}
