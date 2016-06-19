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

    static string[] celebrationQuotes = { "YEAAAH!", "YOU GOT THIS!", "YOU ARE ON FIRE!", "KEEP IT COMING!", "BRILLIANT!", "THAT'S SUPERB PLAYING!", "JUST A LITTLE BIT MORE!", "FANTASTIC VISION!", "NOW THAT IS PERFECTION!", "WHAT A HIT!" };
    static string[] sadQuotes = { "IT WASN'T THERE.", "NOPE!", "TRY AGAIN!", "ALMOST GOT IT...", "IT WAS CLOSE.", "WE NEED TO TRY HARDER!", "MISTAKES HAPPEN." };

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

        //  add text reaction
        Destroy(CreateText(celebrationQuotes[Random.Range(0, celebrationQuotes.Length)]), ANIMATION_TIME);

        //  run coin animation
        GameObject coin = Instantiate(Resources.Load("coin"), new Vector3(Random.Range(-10.0f, 10.0f), -2.0f, 1), Quaternion.identity) as GameObject;
        coin.transform.localScale = new Vector3(1, 1, 1);
        StartCoroutine(MoveAndDie(coin, coin.transform.up, ANIMATION_TIME * 0.75f, ANIMATION_TIME));
    }


    void ClickedWrongPanel()
    {
        //  minus 1 life
        lifes--;
        StartCoroutine(GrowFadeOutAndDie(hearts.transform.GetChild(lifes).gameObject, ANIMATION_TIME));  // start disappearing animation for heart

        //  check if that was the last life 
        if (lifes == 0)
            GameLost();

        //  run animation
        Destroy(Instantiate(Resources.Load(levelID + "_idle"), new Vector3(Random.Range(-10.0f, 10.0f), Random.Range(-1.0f, 4.0f), 1), Quaternion.identity), ANIMATION_TIME);

        //  add text reaction
        Destroy(CreateText(sadQuotes[Random.Range(0, sadQuotes.Length)]), ANIMATION_TIME);
    }


    void GameWon()
    {
        Debug.Log("MUCH WIN. VERY VICTORY. WOW.");
    }

    
    void GameLost()
    {
        Debug.Log("NOOOOOOOOOOOOOOOOOOOOOOES");
    }


    IEnumerator MoveAndDie(GameObject obj, Vector3 direction, float speed, float duration)
    {
        float currentTime = 0.0f ;
        while (currentTime <= duration)
        {
            obj.transform.Translate(direction * speed * Time.deltaTime);
            currentTime += Time.deltaTime;
            yield return null;
        }

        Destroy(obj);
    }


    IEnumerator GrowFadeOutAndDie(GameObject obj, float duration)
    {
        Renderer renderer = obj.GetComponent<Renderer>();
        Color color = renderer.material.color;
        Vector3 scaleUp = new Vector3(0.0025f, 0.0025f, 0f);

        while (color.a > 0)
        {   
            obj.transform.localScale += scaleUp;
            color.a -= Time.deltaTime / duration;
            renderer.material.color = color;            
            yield return null;
        }

        Destroy(obj);
    }


    GameObject CreateText(string textString, bool onRandomLocation = true, Vector2 position = default(Vector2), int fontSize = 45)
    {
        GameObject textGO = new GameObject("text");
        textGO.transform.SetParent(GameObject.Find("Canvas").transform);

        Text text = textGO.AddComponent<Text>();
        text.text = textString;        
        text.font = Resources.Load<Font>("JazzCreateBubble");
        text.fontSize = fontSize;
        text.color = Color.black;
        text.rectTransform.sizeDelta = new Vector2(300, 100);
        text.alignment = TextAnchor.MiddleCenter;
        //text.resizeTextForBestFit = true;
        //text.horizontalOverflow = HorizontalWrapMode.Overflow;

        if (onRandomLocation)
        {
            Vector2 min = new Vector2(-Screen.width * 0.5f + text.rectTransform.rect.width * 0.5f, -Screen.height * 0.5f + text.rectTransform.rect.height * 0.5f);
            Vector2 max = new Vector2(Screen.width * 0.5f - text.rectTransform.rect.width * 0.5f, Screen.height * 0.5f - text.rectTransform.rect.height * 0.5f);
            text.rectTransform.anchoredPosition = new Vector2(Random.Range(min.x, max.x), Random.Range(min.y, max.y));
        }
        else
        {
            text.rectTransform.anchoredPosition = position;
        }

        return textGO;
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
