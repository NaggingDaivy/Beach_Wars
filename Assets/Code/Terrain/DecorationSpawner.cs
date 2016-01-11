using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class DecorationSpawner : MonoBehaviour {

    [System.Serializable]
    public class decoInfo
    {
        public List<Decoration> decorationReferences;
        public int decorationCount = 10;
        public float minimalDistanceBetweenDecoration = 10.0f;
    }

    public float minX = 0.0f;
    public float maxX = 180.0f;
    public float minZ = 0.0f;
    public float maxZ = 180.0f;

    public List<decoInfo> decorations = new List<decoInfo>();

    //private float Scale = 0.5f;
    private GameObject Deco;
    private TerrainGenerator Terrain;

    public void RemoveDecoration()
    {
        foreach (Decoration item in Object.FindObjectsOfType<Decoration>())
        {
            DestroyImmediate(item.gameObject);
        }
        DestroyImmediate(Deco.gameObject);
    }

    public void SpawnDecoration()
    {
        Terrain = GetComponent<TerrainGenerator>();

        if (Deco == null)
        {
            Deco = new GameObject("Decorations");
        }

        foreach (decoInfo item in decorations)
        {
            for (int i = 0; i < item.decorationCount; i++)
            {
                int iObject = Random.Range(0, item.decorationReferences.Count);

                bool bPlaced = false;
                int counter = 0;
                while ((bPlaced == false) && (counter < 10))
                {
                    item.decorationReferences[iObject].transform.position = new Vector3(Random.Range(minX, maxX), 250, Random.Range(minZ, maxZ));

                    RaycastHit hit;
                    Physics.Raycast(item.decorationReferences[iObject].transform.position, Vector3.down, out hit);

                    if (hit.transform.gameObject == this.transform.gameObject)
                    {
                        if (item.decorationReferences[iObject].isAllowed(hit.point.y) && hit.point.y != Terrain.PlatformHeight)
                        {
                            item.decorationReferences[iObject].transform.position = new Vector3(item.decorationReferences[iObject].transform.position.x,
                                                                                                item.decorationReferences[iObject].getRandomAltitude(hit.point.y),
                                                                                                item.decorationReferences[iObject].transform.position.z);

                            GameObject tObject = Instantiate(item.decorationReferences[iObject].gameObject, item.decorationReferences[iObject].transform.position, item.decorationReferences[iObject].getRandomOrientation(hit.normal)) as GameObject;
                            tObject.transform.localScale = item.decorationReferences[iObject].getRandomScale();
                            tObject.transform.parent = Deco.transform;
                            bPlaced = true;
                        }
                        else counter++;
                    }
                    else counter++;
                }
            }
        }
    }
}
