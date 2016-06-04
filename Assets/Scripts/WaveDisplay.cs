using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WaveDisplay : MonoBehaviour {

    private Text enemyWave;

    // Use this for initialization
    void Start () {
        enemyWave = GetComponent<Text>();
        enemyWave.text = "Wave: " + EnemySpawner.enemyWave.ToString();
    }
	
	// Update is called once per frame
	void Update () {
        enemyWave.text = "Wave: " + EnemySpawner.enemyWave.ToString();
    }
}
