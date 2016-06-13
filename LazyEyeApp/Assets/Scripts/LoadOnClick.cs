using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LoadOnClick : MonoBehaviour {

    public string newScene;
    
    void OnMouseDown()
    {
        LoadScene(newScene);
    }

    public void LoadScene(string name)
    {
        SceneManager.LoadScene(name);
    }
}
