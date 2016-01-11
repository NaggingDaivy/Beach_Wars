using UnityEngine;
using System.Collections;

public class Decoration : MonoBehaviour {

    public float minimalAltitude = 0.0f;
    public float maximalAltitude = 20.0f;

    public bool randomizeXOrientation = false;
    public bool randomizeYOrientation = true;
    public bool randomizeZOrientation = false;

    public bool useTerrainOrientation = true;

    public float maximalDepth = 0.5f;

    public float scaleRandomizationIntensity = 0.2f;
    public bool scaleRandomizatioinUniform = true;

    public bool isAllowed(float altitude)
    {
        return ((altitude >= minimalAltitude) && (altitude <= maximalAltitude));
    }

    public float getRandomAltitude(float altitude)
    {
        return altitude - Random.Range(0.0f, maximalDepth);
    }

    public Vector3 getRandomScale()
    {
        if (scaleRandomizatioinUniform)
        {
            float scaleRnd = 1.0f + Random.Range(0.0f, scaleRandomizationIntensity);
            return new Vector3(scaleRnd, scaleRnd, scaleRnd);
        }
        else
        {
            Vector3 scaleRnd = Random.insideUnitSphere; //Donne un vector 3 alléatoire de max de 1 (Attention aussi valleurs négative)
            scaleRnd.x = Mathf.Abs(scaleRnd.x);
            scaleRnd.y = Mathf.Abs(scaleRnd.y);
            scaleRnd.z = Mathf.Abs(scaleRnd.z);
            return Vector3.one + scaleRnd * scaleRandomizationIntensity; // Vector3.one = (1,1,1)
        }
    }

    public Quaternion getRandomOrientation(Vector3 normal)
    {
        Quaternion originalRot;

        if (useTerrainOrientation)
        {
            originalRot = Quaternion.LookRotation(normal, Vector3.forward) * Quaternion.Euler(new Vector3(90.0f, 0.0f, 0.0f)); // Forward de l'objet qui regarde dans l'orientatiopn de la normal du terrain
            // * 90.0f car c'est le Z qui oriente l'objet -> on rederesse l'objet
        }
        else
        {
            originalRot = Quaternion.identity;
        }

        Vector3 xyzRot = Vector3.zero;
        if (randomizeXOrientation)
            xyzRot.x = 1.0f;
        if (randomizeYOrientation)
            xyzRot.y = 1.0f;
        if (randomizeZOrientation)
            xyzRot.z = 1.0f;

        Quaternion rndRot = Quaternion.Euler(xyzRot.x * Random.Range(0.0f, 360.0f), xyzRot.y * Random.Range(0.0f, 360.0f), xyzRot.z * Random.Range(0.0f, 360.0f));

        return originalRot * rndRot;
    }
	
}
