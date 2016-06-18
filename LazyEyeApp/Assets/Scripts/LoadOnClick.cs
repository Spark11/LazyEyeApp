using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LoadOnClick : MonoBehaviour {

    public string levelID;
    public string newScene;
    
    void OnMouseDown()
    {
        PlayerPrefs.SetString("levelID", levelID);
        LoadScene(newScene);
    }

    public void LoadScene(string name)
    {
        SceneManager.LoadScene(name);
    }
}
