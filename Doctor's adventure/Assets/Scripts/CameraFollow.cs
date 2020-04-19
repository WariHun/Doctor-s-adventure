using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;    //Mit kövessen a kamera
    public float speed = 0.1f;  //A követés sebessége

    private void Update()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, RotationManager.degree, 0),8 * Time.deltaTime);  //A tengely forgatása

    }
    private void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, new Vector3(target.position.x, Mathf.Round(target.position.y * 10) / 10, target.position.z), speed);   //A tengely mozgatása, hogy felvegye a játékos a pozícióját
    }
}
