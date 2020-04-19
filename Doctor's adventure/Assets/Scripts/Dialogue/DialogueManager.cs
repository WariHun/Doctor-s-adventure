using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public static bool prologue = false;    //Igaz/hamis, ha igaz akkor a dialógusnak vége, ellenkező esetben hamis

    public TMP_Text DialogueText;   //Dialógus
    public GameObject DialogueBox;  //Keret a dialógusnak

    private Queue<string> sentences;    //FIFO a mondatoknak

    void Awake()
    {
        sentences = new Queue<string>();    //FIFO felvétele
    }
    void FixedUpdate()
    {
        if (Input.anyKeyDown && !prologue) DisplayNextSentence();    //Akármilyen gombnyomásra folytassa a szöveg betöltését
    }

    /// <summary>
    /// Előkészíti a dialógust, és elindítja
    /// </summary>
    public void StartDialogue(Dialogue dialogue)
    {
        sentences.Clear();

        foreach (string s in dialogue.sentences)
        {
            sentences.Enqueue(s);
        }
        prologue = false;
        DialogueBox.SetActive(true);
        DisplayNextSentence();
    }

    /// <summary>
    /// Betölti a következő mondatot a dialógusnak
    /// </summary>
    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            prologue = true;
            DialogueBox.SetActive(false);
            return;
        }
        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    /// <summary>
    /// Kiirás "effekt"
    /// </summary>
    IEnumerator TypeSentence(string sentence)
    {
        DialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            DialogueText.text += letter;
            yield return null;
        }
    }
}
