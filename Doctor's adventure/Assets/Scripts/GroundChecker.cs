using UnityEngine;

public class GroundChecker : MonoBehaviour
{
    public static bool grounded = true; //Igaz-hamis, hogy áll-e valamin

    //Ha olyan object indította el aminek a layer száma 8 (grounded), akkor igazra állítja a grounded változót
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 8) grounded = true;
    }
}
