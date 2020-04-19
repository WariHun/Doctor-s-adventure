using UnityEngine;

public class PlayerReturn : MonoBehaviour
{
    public static Transform returnPoint;  //A visszatérési pont
    void Start()
    {
        returnPoint = GameObject.Find("ReturnPoint").transform; //Visszatérési pont megkeresése
    }
    //Ha leesik valami (jelen esetben csak a játékos eshet le), akkor visszarakja a megadott pontra, esési sebesség nélkül
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag=="Player")
        {
            other.gameObject.GetComponent<Rigidbody>().velocity =new Vector3(other.gameObject.GetComponent<Rigidbody>().velocity.x, 0, other.gameObject.GetComponent<Rigidbody>().velocity.z);
            other.transform.position = returnPoint.transform.position;
        }
    }
}
