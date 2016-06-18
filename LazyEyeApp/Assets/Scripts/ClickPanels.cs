using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ClickPanels : MonoBehaviour {

    private const int ANIMATION_TIME = 4;
    private const int MAX_LIFES = 3;
    private const int MAX_ROUNDS = 10;

    private string levelID = "knight";
        
    public Text uiText;
    private GameObject hearts;

    private int correctPanel;
    private int score;
    private int lifes;
    

	// Use this for initialization
	void Start () {
        //  set score
        score = 0;
        uiText.text = score.ToString();

        //  set lifes and rounds
        lifes = MAX_LIFES;        
        hearts = GameObject.Find("hearts");

        //  set background image
        levelID = PlayerPrefs.GetString("levelID");
        SetBackground();

        //  choose a correct panel
        correctPanel = Random.Range(0, 4);
        transform.GetChild(correctPanel).GetComponent<PanelSelected>().ToggleIsCorrect();
    }


    void SetBackground()
    {
        GameObject background = GameObject.Find("Background");
        SpriteRenderer backSR = background.GetComponent<SpriteRenderer>();

        backSR.sprite = Resources.Load<Sprite>(levelID + "_back");

        float worldScreenHeight = Camera.main.orthographicSize * 2;
        float worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;

        background.transform.localScale = new Vector3(worldScreenWidth / backSR.sprite.bounds.size.x, worldScreenHeight / backSR.sprite.bounds.size.y, 1);
    }


    public void PanelClicked(int id) {
        //  check if the clicked panel is the correct one
        if (id == correctPanel)
            ClickedCorrectPanel();
        else
            ClickedWrongPanel();

        //  hide panels
        foreach (Transform child in transform)
            child.gameObject.SetActive(false);

        //  check for victory
        if (score == MAX_ROUNDS)
            GameWon();
        else        
            StartCoroutine(ResetPanelsAfterTime(ANIMATION_TIME));   //  reset panels back after animations
    }


    IEnumerator ResetPanelsAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        ResetPanels();
    }


    void ResetPanels()
    {
        //  show panels
        foreach (Transform child in transform)
            child.gameObject.SetActive(true);

        //  choose a new correct panel
        transform.GetChild(correctPanel).GetComponent<PanelSelected>().ToggleIsCorrect();       //  reset currently correct panel        
        correctPanel = Random.Range(0, 4);              //  choose new random correct panel        
        transform.GetChild(correctPanel).GetComponent<PanelSelected>().ToggleIsCorrect();       //  set the new chosen panel as correct      
    }
    

    void ClickedCorrectPanel()
    {
        //  update score
        score++;
        uiText.text = score.ToString();

        if (score == MAX_ROUNDS)
            return;

        //  run character animation
         Destroy(Instantiate(Resources.Load(levelID + "_happy"), new Vector3(Random.Range(-10.0f, 10.0f), Random.Range(-1.0f, 4.0f), 1), Quaternion.identity), ANIMATION_TIME);

        //  run coin animation
        GameObject coin = Instantiate(Resources.Load("coin"), new Vector3(Random.Range(-10.0f, 10.0f), -2.0f, 1), Quaternion.identity) as GameObject;
        coin.transform.localScale = new Vector3(1, 1, 1);
        StartCoroutine(MoveAndDie(coin, coin.transform.up, ANIMATION_TIME * 0.75f, ANIMATION_TIME));
    }


    void ClickedWrongPanel()
    {
        //  minus 1 life
        lifes--;
        hearts.transform.GetChild(lifes).gameObject.SetActive(false);
        if (lifes == 0)
            GameLost();

        //  run animation
        Destroy(Instantiate(Resources.Load(levelID + "_idle"), new Vector3(Random.Range(-10.0f, 10.0f), Random.Range(-1.0f, 4.0f), 1), Quaternion.identity), ANIMATION_TIME);
    }


    void GameWon()
    {
        Debug.Log("MUCH WIN. VERY VICTORY. WOW.");
    }

    
    void GameLost()
    {
        Debug.Log("NOOOOOOOOOOOOOOOOOOOOOOES");
    }


    IEnumerator MoveAndDie(GameObject obj, Vector3 direction, float speed, float moveTime)
    {
        float currentTime = 0.0f ;
        while (currentTime <= moveTime)
        {
            obj.transform.Translate(direction * speed * Time.deltaTime);
            currentTime += Time.deltaTime;
            yield return null;
        }

        Destroy(obj);
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            levelID = "knight";
            SetBackground();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2)) { 
            levelID = "dragon";
            SetBackground();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3)) { 
            levelID = "princess";
            SetBackground();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4)) { 
            levelID = "alien";
            SetBackground();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5)) { 
            levelID = "fox";
            SetBackground();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6)) { 
            levelID = "penguin";
            SetBackground();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha7)) { 
            levelID = "girl";
            SetBackground();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha8)) { 
            levelID = "droid";
            SetBackground();
        }
    }
}
