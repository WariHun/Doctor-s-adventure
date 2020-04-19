[System.Serializable]
public class PlayerData
{
    public float[] pos; //Tömb a pozíciónak
    public string facingDirection;  //String az iránynak
    public bool prologue;   //Bool a dialógus elvégzésének tárolására

    /// <summary>
    /// A játékos pozíciójának eltárolása egy 3 elemű float tömbbe és a karakter nézési irányánák az eltárolása egy string változóban, valamint a dialógus állapotának elmentése egy igaz/hamis változóba
    /// </summary>
    public PlayerData(PlayerController player)
    {
        pos = new float[3];
        pos[0]= player.transform.position.x;
        pos[1]= player.transform.position.y;
        pos[2]= player.transform.position.z;

        prologue = DialogueManager.prologue;

        facingDirection = RotationManager.facingDirection.ToString();
    }
}
