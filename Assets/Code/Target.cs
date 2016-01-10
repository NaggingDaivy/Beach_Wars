using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Target : MonoBehaviour
{
    public int ScoreValue = 10;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(0, 2, 0);
	
	}

    void OnDestroy()
    {
        GameObject.FindGameObjectWithTag("Score").GetComponent<Score>().UpdateScore(ScoreValue);
    }
}
