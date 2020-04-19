using UnityEngine;
using UnityEngine.SceneManagement;

public class EndTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        SceneChange(2);  //A pálya vége
    }
    public void SceneChange(int scene)
    {
        SceneManager.LoadScene(scene);
    }
}
