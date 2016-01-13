using UnityEngine;
using System.Collections;

public class TerrainGenerator : MonoBehaviour
{

    public int SizeX = 20;
    public int SizeZ = 20;
    public float Lacunarity = 2.0f;
    public float Gain = 0.5f;
    public int Octaves = 8;
    public float BaseFrequency = 0.5f;
    public float FieldScale = 10.0f;

    //Beach
    public float BeachRadius = 30.0f;
    public float BeachHeight = 10.0f;

    //Platform
    public float PlatformRadius = 30.0f;
    public float PlatformHeight = 0.0f;
    public float PlatformTransitionWidth = 20.0f;
    public int PlatformPosX = 0;
    public int PlatformPosZ = 0;
    public int SquareScale = 25;
    public Color DamierColor1 = Color.black;
    public Color DamierColor2 = Color.white;

    //Texture
    public Texture2D AlbedoTexture = null;
    public int TextureWidth = 512;
    public int TextureHeight = 512;

    //Color
    public Color BeachColor = Color.yellow;

    public Color TerrainHighColor = Color.white;
    public Color TerrainLowColor = Color.green;
    public float ColorScale = 0.5f;

    private Mesh _mesh;

    float GetFBRHeight(float px, float pz)
    {
        float noise = 0.0f;
        float frequency = BaseFrequency;
        float amplitude = Gain;

        for (int i = 0; i < Octaves; ++i)
        {
            noise += Mathf.PerlinNoise((float)px * frequency, (float)pz * frequency) * amplitude;
            frequency *= Lacunarity;
            amplitude *= Gain;
        }



        return noise;

    }


    float GetHeight(float px, float pz, out float platformWeight, out float beachWeight)
    {
        float height = GetFBRHeight(px, pz) * FieldScale;

        //height += terrainOffset
        //BEACH

        beachWeight = 1.0f;
        beachWeight *= Mathf.Clamp01(px / BeachRadius);          //if (px <= BeachRadius) {weight *= px / BeachRadius;}
        beachWeight *= Mathf.Clamp01(pz / BeachRadius);          //if (pz <= BeachRadius) {weight *= px / BeachRadius;}
        beachWeight *= Mathf.Clamp01((SizeX - px) / BeachRadius);          //if (SizeX - px <= BeachRadius) {weight *= px / BeachRadius;}
        beachWeight *= Mathf.Clamp01((SizeZ - pz) / BeachRadius);          //if (SizeZ - pz <= BeachRadius) {weight *= px / BeachRadius;}

        beachWeight *= Mathf.SmoothStep(0.0f, 1.0f, beachWeight);

        height *= beachWeight;

        beachWeight = 1.0f - beachWeight; // poids de la plage par rapport à la montange, 1 = montagne

        // Mathf.Lerp(BeachHeight, height, beachWeight);


        //END BEACH

        //PLATFORM

        platformWeight = 1.0f;

        platformWeight *= Mathf.Clamp01((px - (PlatformPosX - PlatformRadius - PlatformTransitionWidth)) / PlatformTransitionWidth);
        platformWeight *= Mathf.Clamp01((pz - (PlatformPosZ - PlatformRadius - PlatformTransitionWidth)) / PlatformTransitionWidth);
        platformWeight *= Mathf.Clamp01(((PlatformPosX + PlatformRadius + PlatformTransitionWidth) - px) / PlatformTransitionWidth);
        platformWeight *= Mathf.Clamp01(((PlatformPosZ + PlatformRadius + PlatformTransitionWidth) - pz) / PlatformTransitionWidth);
        //END PLATFORM

        platformWeight = Mathf.SmoothStep(0.0f, 1.0f, platformWeight);

        return Mathf.Lerp(height, PlatformHeight, platformWeight);



    }

    public void RegenerateTexture()
    {
        Color[] pix;

        if (AlbedoTexture)
            DestroyImmediate(AlbedoTexture);
        AlbedoTexture = new Texture2D(TextureWidth, TextureHeight);

        MeshRenderer mRenderer = GetComponent<MeshRenderer>();
        mRenderer.material.SetTexture("_MainTex", AlbedoTexture);

        pix = new Color[TextureWidth * TextureHeight];

        Color PlatformColor = DamierColor1;

        int squareScale = TextureWidth / SquareScale;

        int counterSquareWidth = 1, counterSquareHeight = 1;

        for (int z = 0; z < TextureWidth; z++)
        {
            if (counterSquareWidth > squareScale)
            {
                counterSquareWidth = 0;

                if (PlatformColor == DamierColor2)
                    PlatformColor = DamierColor1;
                else
                    PlatformColor = DamierColor2;
            }

            counterSquareHeight = 0;

            for (int x = 0; x < TextureHeight; x++)
            {
                if (counterSquareHeight > squareScale)
                {
                    counterSquareHeight = 0;

                    if (PlatformColor == DamierColor2)
                        PlatformColor = DamierColor1;
                    else
                        PlatformColor = DamierColor2;
                }

               
                int index = z * TextureWidth + x;
                float xUV = x / (float)(TextureWidth - 1.0f);
                float zUV = z / (float)(TextureHeight - 1.0f);

                float terrainCoordX = xUV * (float)SizeX;
                float terrainCoordZ = zUV * (float)SizeZ;

                float flatWeigth, beachWeight;
                float height = GetHeight(terrainCoordX, terrainCoordZ, out flatWeigth, out beachWeight) / (FieldScale * ColorScale);

                Color final;

                if (flatWeigth < .99f)
                    flatWeigth = 0.0f;

                final = Color.Lerp(TerrainLowColor, TerrainHighColor, height);
                final = Color.Lerp(final, BeachColor, beachWeight);
                final = Color.Lerp(final, PlatformColor, flatWeigth);


                pix[index] = final;

                ++counterSquareHeight;
            }

            ++counterSquareWidth;
        }

        AlbedoTexture.SetPixels(pix);
        AlbedoTexture.Apply();

    }

    public void RegenerateMesh()
    {

        if (_mesh != null)
            DestroyImmediate(_mesh); // ne jamais utiliser en mode gameplay, uniquement dans editor

        _mesh = new Mesh();

        _mesh.vertices = new Vector3[SizeX + 1 * SizeZ + 1];

        int numVertices = (SizeX + 1) * (SizeZ + 1);
        int numTriangles = SizeX * SizeZ * 2;

        Vector3[] vertices = new Vector3[numVertices];
        Vector2[] uvs = new Vector2[numVertices];

        int[] triangles = new int[numTriangles * 3]; // 3 index par tiriangle

        int i = 0;

        for (int z = 0; z < SizeZ + 1; ++z) // vertices
            for (int x = 0; x < SizeX + 1; ++x)
            {
                float px, py, pz;
                float u, v;

                //Calcul de px,py,pz et u,v (to do)

                px = (float)x;
                pz = (float)z;

                float platformWeight, beachWeight;

                py = GetHeight(px, pz, out platformWeight, out beachWeight);

                u = (float)x / (float)SizeX; // a changer quand x vaut 0, quand x vaut sizeX ça vaut 1
                v = (float)z / (float)SizeZ; // a changer idem v

                vertices[i] = new Vector3(px, py, pz);
                uvs[i] = new Vector2(u, v);
                ++i;
            }

        i = 0;
        /* Pourquoi un tableau une dimension? Car le GPU attends un buffer (void*) content des donnéees. avec un tableau 2D, on a un indirection en plus (tableau de tableau qui sont eux même
        des pointeurs. Donc le tableau principal contient une liste de pointeurs. Si on copie sur le GPU, tout ce qu'il lira c'est des adresse qui ne veuelent rien dire)
        ==> Choix d'un tableau à une dimension.*/

        for (int z = 0; z < SizeZ; ++z) //triangles
            for (int x = 0; x < SizeX; ++x)
            {
                //Premier triangle sens anti-horlogieque
                //x est le nième element de la z ième ligne (ligne = nombre de vertice sur la longueur)
                //elem = numCol + numLigne * (tailleLigne + 1)
                triangles[i] = x + z * (SizeX + 1); // coin inférieur gauche
                triangles[i + 1] = x + (z + 1) * (SizeX + 1); // coin supérieur gauche
                triangles[i + 2] = x + 1 + z * (SizeX + 1); // coin inférieur droit

                //second triangle sens anti-horlogique
                triangles[i + 3] = x + (z + 1) * (SizeX + 1); // coin supérieur gauche
                triangles[i + 4] = x + 1 + (z + 1) * (SizeX + 1); //  coin supérieur droit
                triangles[i + 5] = x + 1 + z * (SizeX + 1);  // coin inférieur droit

                i += 6;
            }

        _mesh.vertices = vertices;
        _mesh.uv = uvs;
        _mesh.triangles = triangles;

        _mesh.RecalculateNormals();

        //Mesh FIlter
        MeshFilter meshFilter = gameObject.GetComponent<MeshFilter>();

        if (meshFilter == null)
        {
            meshFilter = gameObject.AddComponent<MeshFilter>();
        }

        meshFilter.mesh = _mesh;

        // MEsh COllider
        MeshCollider meshCollider = gameObject.GetComponent<MeshCollider>();

        if (meshCollider == null)
        {
            meshCollider = gameObject.AddComponent<MeshCollider>();
        }

        meshCollider.sharedMesh = _mesh;

        //Mesh Renderer
        MeshRenderer meshRenderer = gameObject.GetComponent<MeshRenderer>();

        if (meshRenderer == null)
        {
            meshRenderer = gameObject.AddComponent<MeshRenderer>();
        }

    }

    // Use this for initialization
    void Start()
    {
        //RegenerateMesh();
    }


}
