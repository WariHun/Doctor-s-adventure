using UnityEngine;

public class SaveLoadScript : MonoBehaviour
{
    /// <summary>
    /// Adatok elmentése, az adott slotba
    /// </summary>
    public static void Save(int slot)
    {
        SaveSystem.SavePlayer(slot.ToString());
    }

    /// <summary>
    /// Adatok betöltése, az adott slotból
    /// </summary>
    public static void Load(int slot)
    {
        PlayerData data = SaveSystem.LoadPlayer(slot.ToString());

        PlayerController player = GameObject.Find("Player").GetComponent<PlayerController>();

        player.transform.position = new Vector3(data.pos[0], data.pos[1], data.pos[2]);

        DialogueManager.prologue = data.prologue;

        if (data.facingDirection == "Front")
        {
            RotationManager.facingDirection = FacingDirection.Front;
            RotationManager.degree = 0;
        }
        else if (data.facingDirection == "Right")
        {
            RotationManager.facingDirection = FacingDirection.Right;
            RotationManager.degree = -90;
        }
        else if (data.facingDirection == "Back")
        {
            RotationManager.facingDirection = FacingDirection.Back;
            RotationManager.degree = 180;
        }
        else if (data.facingDirection == "Left")
        {
            RotationManager.facingDirection = FacingDirection.Left;
            RotationManager.degree = 90;
        }
    }
}
