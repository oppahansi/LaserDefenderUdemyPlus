using UnityEngine;
using System.Collections;

public class Formations : MonoBehaviour {

    public GameObject[] formations;
    private bool newFormationSet = false;
	
    void Start()
    {
        formations[0] = Instantiate(formations[0], formations[0].transform.position, Quaternion.identity) as GameObject;
        formations[0].transform.parent = this.transform;
    }

	// Update is called once per frame
	void Update () {

	    if (EnemySpawner.enemyWave == 2)
        {
            Debug.Log(EnemySpawner.enemyWave);
            if (!newFormationSet)
            {
                formations[0].SetActive(false);
                formations[1] = Instantiate(formations[1], formations[1].transform.position, Quaternion.identity) as GameObject;
                formations[1].transform.parent = this.transform;
                Debug.Log("Formation set : " + newFormationSet);
                newFormationSet = true;
            }
        }
	}

    void EnableFormation(int formationPos)
    {
        Debug.Log(formationPos + " Name: " + formations[formationPos].name);
        formations[formationPos].SetActive(true);
    }
}
