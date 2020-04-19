using UnityEngine;
public class CubeManager : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        PlayerReturn.returnPoint.position = new Vector3(transform.position.x, transform.position.y + 1.33f, transform.position.z);  //Visszatérési pont frissítése
    }
}
