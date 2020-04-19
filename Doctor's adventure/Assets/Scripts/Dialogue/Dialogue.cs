using UnityEngine;

[System.Serializable]
public class Dialogue
{
    [TextArea(1,3)]
    public string[] sentences;  //Dialógus mondatainak tárolására egy tömb
}
