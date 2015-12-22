using UnityEngine;
using System.Collections;

public class WaveMovement : MonoBehaviour
{

    public float MinHeight = 0.1f;
    public float MaxHeight = 2.5f;
    public float TimeSpeed = 0.2f;
    public float ScrollSpeed = 1.2f;

    private Renderer render = null;

    void Start()
    {
        render = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update() {

        Vector3 pos = transform.position;
        pos.y  = Mathf.Lerp(MinHeight, MaxHeight, (Mathf.Sin(Time.timeSinceLevelLoad * TimeSpeed) + 1.0f) * 0.5f );

        transform.position = pos;

        transform.position = pos;

        float offsetX = 0.0f;
        float offsetY = Mathf.Sin(Time.timeSinceLevelLoad * 0.1f);
        render.material.mainTextureOffset = new Vector2(offsetX, offsetY) * 0.1f;
	}

}
