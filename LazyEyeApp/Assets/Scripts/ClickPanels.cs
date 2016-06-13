using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ClickPanels : MonoBehaviour {

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
}
