using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;   //A dialógus mondatainak felvételére

    void Start()
    {
        if (!DialogueManager.prologue) FindObjectOfType<DialogueManager>().StartDialogue(dialogue);    //Dialógus elindítása
    }
}
