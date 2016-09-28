using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IronMine : JobScript {

    // Use this for initialization
    new void Start () {
        base.Start();
        buildingMaxLevel = 5;

        jobName = "Iron Mine";
        resource = "Iron";

        color = Color.grey;

        resPerWorker = new int[] { 2, 3, 3, 4, 5 };

        curResGain = 0;

        maxWorkers = new int[] { 3, 3, 4, 4, 5 };
        curWorkers = 0;

        maxGuards = new int[] { 1, 1, 2, 2, 3 };
        curGuards = 0;

        // Upgrade costs
        Dictionary<string, int> upgradeTo2 = new Dictionary<string, int>() {
            {"Iron", 5 }
        };
        Dictionary<string, int> upgradeTo3 = new Dictionary<string, int>() {
            {"Clay", 5}, {"Wood",9 }
        };
        Dictionary<string, int> upgradeTo4 = new Dictionary<string, int>() {
            {"Clay", 5}, {"Wood",5 }
        };
        Dictionary<string, int> upgradeTo5 = new Dictionary<string, int>() {
            {"Clay", 5}, {"Wood",5 }
        };

        upgradeCosts = new Dictionary<string, int>[] { upgradeTo2, upgradeTo3, upgradeTo4, upgradeTo5 };

    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
