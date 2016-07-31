using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LoadOnClick : MonoBehaviour {

    public string levelID;
    public string newScene;

    private int speed;

    void Start()
    {
        //  rotate each planet with a random speed and direction /left or right/
        speed = Random.Range(5, 40);
        speed *= (Random.value > 0.5f) ? 1 : -1;
    }
    
    void OnMouseDown()
    {
        PlayerPrefs.SetString("levelID", levelID);
        SoundManager.instance.PlayEffect(SoundManager.SFX.CLICK);
        LoadScene(newScene);
    }

    public void LoadScene(string name)
    {
        SceneManager.LoadScene(name);
    }

    void Update()
    {
        transform.Rotate(Vector3.up * Time.deltaTime * speed);
    }
}
