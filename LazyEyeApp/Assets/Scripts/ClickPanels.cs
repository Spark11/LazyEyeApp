using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ClickPanels : MonoBehaviour {

    private string spriteName = "knight";

    public Text uiText;

    private int correctPanel;
    private int score;

    public void PanelClicked(int id) {
        if (id == correctPanel)
        {
            score++;
            uiText.text = score.ToString();
            ResetPanels();
        }
        else
            Debug.Log("nope!");
    }

	// Use this for initialization
	void Start () {
        score = 0;
        uiText.text = score.ToString();        
        correctPanel = Random.Range(0, 4);
        transform.GetChild(correctPanel).GetComponent<PanelSelected>().ToggleIsCorrect();
    }


    void ResetPanels()
    {
        transform.GetChild(correctPanel).GetComponent<PanelSelected>().ToggleIsCorrect();       //  reset currently correct panel        
        correctPanel = Random.Range(0, 4);              //  choose new random correct panel        
        transform.GetChild(correctPanel).GetComponent<PanelSelected>().ToggleIsCorrect();       //  set the new chosen panel as correct      
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            spriteName = "knight";
        else if (Input.GetKeyDown(KeyCode.Alpha2))
            spriteName = "dragon";
        else if (Input.GetKeyDown(KeyCode.Alpha3))
            spriteName = "princess";
        else if (Input.GetKeyDown(KeyCode.Alpha4))
            spriteName = "alien";
        else if (Input.GetKeyDown(KeyCode.Alpha5))
            spriteName = "fox";
        else if (Input.GetKeyDown(KeyCode.Alpha6))
            spriteName = "penguin";
        else if (Input.GetKeyDown(KeyCode.Alpha7))
            spriteName = "girl";
        else if (Input.GetKeyDown(KeyCode.Alpha8))
            spriteName = "droid";

        if (Input.GetKeyDown(KeyCode.A))
        {
            Destroy(Instantiate(Resources.Load(spriteName + "_idle"), new Vector3(0, 0, 1), Quaternion.identity), 4);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            Destroy(Instantiate(Resources.Load(spriteName + "_happy"), new Vector3(0, 0, 1), Quaternion.identity), 4);
        }
    }
}
