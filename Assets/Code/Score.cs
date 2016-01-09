using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Score : MonoBehaviour {

    private int _score  = 0;
	// Use this for initialization
	void Start () {
        UpdateScore(_score);
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void UpdateScore(int score)
    {
        _score += score;

        GetComponent<Text>().text = "Score: " + _score;
    }
}
