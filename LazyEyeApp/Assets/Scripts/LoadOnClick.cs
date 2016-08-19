using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadOnClick : MonoBehaviour {

    public string levelID;
    public string newScene;

    private int speed;

    void Start()
    {
        //  rotate each planet with a random speed and direction /left or right/
        speed = Random.Range(10, 40);
        speed *= (Random.value > 0.5f) ? 1 : -1;

        //  hide "Loading..." text
        ShowLoadingText(false);
    }
    
    void OnMouseDown()
    {
        //  show "Loading..." text
        ShowLoadingText(true);

        //  transition to selected level
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

    void ShowLoadingText(bool showing)
    {
        GameObject loading = GameObject.Find("loading");
        loading.GetComponent<CanvasRenderer>().SetAlpha(showing ? 1 : 0);
        loading.transform.GetChild(0).GetComponent<Text>().enabled = showing;
    }
}
