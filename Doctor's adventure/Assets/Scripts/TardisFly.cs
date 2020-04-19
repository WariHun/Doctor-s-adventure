using UnityEngine;

public class TardisFly : MonoBehaviour
{
    public float speed = 1f;  //Mozgási sebesség
    public float rotateSpeed = 5.0f;    //Forgási sebesség

    private Vector3 newPos;    //Vector hogy merre menjen következőnek

    void Start()
    {
        NextPos();
    }

    void Update()
    {
        if (Mathf.Abs(Mathf.Round(transform.position.x))==12) NextPos();  //Ha elérte a meghatározozz pontot, új cél
        transform.position = Vector3.MoveTowards(transform.position, newPos, Time.deltaTime*speed); //Mozgatás
        transform.Rotate(0, 0, 10 * (Time.deltaTime*rotateSpeed));  //Forgatás
    }

    /// <summary>
    /// Meghazározza a következő pozíciót
    /// </summary>
    void NextPos()
    {
        int x;
        if (transform.position.x <= 0) x = 12;
        else x = -12;
        
        newPos = new Vector2(x, Random.Range(-7.0f, 10.0f));
    }
}
