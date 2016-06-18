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
        SetBackground();
        correctPanel = Random.Range(0, 4);
        transform.GetChild(correctPanel).GetComponent<PanelSelected>().ToggleIsCorrect();
    }


    void ResetPanels()
    {
        transform.GetChild(correctPanel).GetComponent<PanelSelected>().ToggleIsCorrect();       //  reset currently correct panel        
        correctPanel = Random.Range(0, 4);              //  choose new random correct panel        
        transform.GetChild(correctPanel).GetComponent<PanelSelected>().ToggleIsCorrect();       //  set the new chosen panel as correct      
    }


    void SetBackground()
    {
        GameObject background = GameObject.Find("Background");
        SpriteRenderer backSR = background.GetComponent<SpriteRenderer>();

        backSR.sprite = Resources.Load<Sprite>(spriteName + "_back");
        
        float worldScreenHeight = Camera.main.orthographicSize * 2;
        float worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;

        background.transform.localScale = new Vector3(worldScreenWidth / backSR.sprite.bounds.size.x, worldScreenHeight / backSR.sprite.bounds.size.y, 1);
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            spriteName = "knight";
            SetBackground();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2)) { 
            spriteName = "dragon";
            SetBackground();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3)) { 
            spriteName = "princess";
            SetBackground();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4)) { 
            spriteName = "alien";
            SetBackground();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5)) { 
            spriteName = "fox";
            SetBackground();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6)) { 
            spriteName = "penguin";
            SetBackground();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha7)) { 
            spriteName = "girl";
            SetBackground();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha8)) { 
            spriteName = "droid";
            SetBackground();
        }



        if (Input.GetKeyDown(KeyCode.A))
        {
            Destroy(Instantiate(Resources.Load(spriteName + "_idle"), new Vector3(Random.Range(-10.0f, 10.0f), Random.Range(-1.0f, 4.0f), 1), Quaternion.identity), 4);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            Destroy(Instantiate(Resources.Load(spriteName + "_happy"), new Vector3(Random.Range(-10.0f, 10.0f), Random.Range(-1.0f, 4.0f), 1), Quaternion.identity), 4);
        }
    }
}
