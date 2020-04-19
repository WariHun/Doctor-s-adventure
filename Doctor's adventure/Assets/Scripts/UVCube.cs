using UnityEngine;
 
public class UVCube : MonoBehaviour 
{
    public float titles = 0.125f;   //Változó hogy mekkora legyen egy kocka a texturának

    private MeshFilter mf;  //MeshFilterre változó
    void Start () 
    {
        //Indulásnál a material-t rászabjuk a kockára
        ApplyTexture ();
    }
    /// <summary>
    /// Megkeresi a MeshFiltert, majd koordináták alapján a materialt ráilleszti a kockára
    /// </summary>
    public void ApplyTexture()
    {
        mf = gameObject.GetComponent<MeshFilter> ();

        if(mf)
        {
            Mesh mesh = mf.sharedMesh;  //A material eltárolása
            if (mesh)
            {
                Vector2[] uvs = mesh.uv;    //Tömb a material koordináinak

                //Az oldalak meghatározása koordináták alapján

                // Eleje
                uvs[0] = new Vector2(0f, 0f); //Bal lent
                uvs[1] = new Vector2(titles, 0f); //Jobb lent
                uvs[2] = new Vector2(0f, 1f); //Bal fent
                uvs[3] = new Vector2(titles, 1f); //Jobb fent
     
                // Jobb oldala
                uvs[20] = new Vector2(titles * 1.001f, 0f);
                uvs[23] = new Vector2(titles * 2.001f, 0f);
                uvs[21] = new Vector2(titles * 1.001f, 1f);
                uvs[22] = new Vector2(titles * 2.001f, 1f);
 
 
                // Hátulja
                uvs[11] = new Vector2((titles * 2.001f), 1f);
                uvs[10] = new Vector2((titles * 3.001f), 1f);
                uvs[7] = new Vector2((titles * 2.001f), 0f);
                uvs[6] = new Vector2((titles * 3.001f), 0f);
 
                // Bal oldala
                uvs[16] = new Vector2(titles * 3.001f, 0f);
                uvs[19] = new Vector2(titles * 4.001f, 0f);
                uvs[17] = new Vector2(titles * 3.001f, 1f);
                uvs[18] = new Vector2(titles * 4.001f, 1f);
 
                // Teteje
                uvs[8] = new Vector2(titles * 4.001f, 0f);
                uvs[9] = new Vector2(titles * 5.001f, 0f);
                uvs[4] = new Vector2(titles * 4.001f, 1f);
                uvs[5] = new Vector2(titles * 5.001f, 1f);
 
                // Alja
                uvs[12] = new Vector2(titles * 5.001f, 0f);
                uvs[14] = new Vector2(titles * 6.001f, 0f);
                uvs[15] = new Vector2(titles * 5.001f, 1f);
                uvs[13] = new Vector2(titles * 6.001f, 1f);

                mesh.uv = uvs;  //A felszin frissítése amivel a material-t rászabja a kockára
            }
        }
    }
}