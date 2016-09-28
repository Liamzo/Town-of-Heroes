using UnityEngine;
using System.Collections;
using System.Linq;
using UnityEngine.UI;

public class JobDayPanelScript : MonoBehaviour {

    ControlScript control;

    GameObject jobName;
    GameObject jobGain;
    GameObject[] workers;
    GameObject[] guards;

    JobScript job;
    Color availColor = Color.white;
    Color blockColor = Color.red;

	// Use this for initialization
	void Start () {
        control = GameObject.FindGameObjectWithTag("Control").GetComponent<ControlScript>();

        jobName = GameObject.Find("JobName");
        jobGain = GameObject.Find("Gain");

        workers = GameObject.FindGameObjectsWithTag("BtnWorker").OrderBy(go => go.name).ToArray();

        guards = GameObject.FindGameObjectsWithTag("BtnGuard").OrderBy(go => go.name).ToArray();

        gameObject.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
	    if (control.stateDay != ControlScript.StateDay.Morning) {
            gameObject.SetActive(false);
        }
	}

    public void updateInfo (JobScript job) {
        jobName.GetComponent<Text>().text = job.getJobName() + " - " + job.getLevel();
        jobGain.GetComponent<Text>().text = "Gain: " + job.getCurResGain();

        this.job = job;

        for (int i = 0; i < workers.Length; i++) {
            if (i < job.getCurWorkers()) {
                // Set buttons to filled with worker
                workers[i].GetComponent<Image>().color = job.color;
                workers[i].GetComponent<Button>().interactable = true;
            } else if (i == job.getCurWorkers() && i < job.getMaxWorkers()) {
                workers[i].GetComponent<Image>().color = availColor;
                workers[i].GetComponent<Button>().interactable = true;
            } else if (i < job.getMaxWorkers()) {
                workers[i].GetComponent<Image>().color = blockColor;
                workers[i].GetComponent<Button>().interactable = true;
            } else {
                workers[i].GetComponent<Image>().color = Color.black;
                workers[i].GetComponent<Button>().interactable = false;
            }
        }
    }

    public void workerBtn (int i) {
        // Check color of button, and react accordingly

        // If button is white, then we can add a worker to the job
        if (i == job.getCurWorkers()) {
            // Update color and set next button to avail
            workers[i].GetComponent<Image>().color = job.color;
            job.addWorker();
            jobGain.GetComponent<Text>().text = "Gain: " + job.getCurResGain();
            if (workers[i+1].GetComponent<Image>().color == blockColor) {
                workers[i+1].GetComponent<Image>().color = availColor;
            }
        // Remove worker
        } else if (i == (job.getCurWorkers() - 1)) {
            workers[i].GetComponent<Image>().color = availColor;
            job.removeWorker();
            jobGain.GetComponent<Text>().text = "Gain: " + job.getCurResGain();
            workers[i+1].GetComponent<Image>().color = blockColor;
        }
    }

    public void exitPanel () {
        gameObject.SetActive(false);
    }
}
