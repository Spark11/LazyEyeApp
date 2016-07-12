﻿using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

public class ClickPanels : MonoBehaviour {

    private const int OPENING_STORY_TIME = 10;  // TODO:: set to 10-11
    private const int LOSS_CLOSING_STORY_TIME = 6;
    private const int WIN_CLOSING_STORY_TIME = 10;
    private const int ANIMATION_TIME = 4;
    private const int MAX_LIFES = 3;
    private const int MAX_ROUNDS = 6;          // TODO:: set to 10-15

    private static readonly Vector2 SCREEN = new Vector2(9, 6);

    private string levelID = "knight";
        
    private Text scoreText;
    private GameObject pauseButton;
    private GameObject pauseMenu;
    private GameObject hearts;

    private Color fontColor;

    private Coroutine coroutine;

    private int correctPanel;
    private int score;
    private int lifes;

    private Sprite[] items;

    static string[] celebrationQuotes = { "YEAAAH!", "YOU GOT THIS!", "YOU ARE ON FIRE!", "KEEP IT COMING!", "BRILLIANT!", "THAT'S SUPERB PLAYING!", "JUST A LITTLE BIT MORE!", "FANTASTIC VISION!", "NOW THAT IS PERFECTION!", "WHAT A HIT!" };
    static string[] sadQuotes = { "IT WASN'T THERE.", "NOPE!", "TRY AGAIN!", "ALMOST GOT IT...", "IT WAS CLOSE.", "WE NEED TO TRY HARDER!", "MISTAKES HAPPEN." };

    // Use this for initialization
    void Start ()
    {
        //  initialize GUI objects
        pauseMenu = GameObject.Find("pauseMenu");
        pauseButton = GameObject.Find("pauseButton");
        scoreText = GameObject.Find("score").GetComponent<Text>();

        //  set lifes and rounds
        lifes = MAX_LIFES;        
        hearts = GameObject.Find("hearts");

        //  set score
        score = 0;
        scoreText.text = score.ToString();

        //  set background image
        levelID = PlayerPrefs.GetString("levelID");
        SetBackground();

        //  load a sprite array of character's items
        items = Resources.LoadAll<Sprite>(levelID + "_items");

        //  set font color
        if (levelID == "knight")
            fontColor = Color.black;
        else if (levelID == "dragon")
            fontColor = Color.red;
        else if (levelID == "princess")
            fontColor = new Color(0.5f, 0, 0.5f, 1);
        else if (levelID == "alien")
            fontColor = Color.cyan;
        else if (levelID == "fox")
            fontColor = new Color(0.9f, 0, 0, 1);
        else if (levelID == "penguin")
            fontColor = Color.yellow;
        else if (levelID == "girl")
            fontColor = new Color(0.5f, 0, 0, 1);
        else if (levelID == "droid")
            fontColor = Color.green;
        
        //  change font color of text fields
        scoreText.color = fontColor;                                                //  score label
        pauseMenu.transform.GetChild(0).GetComponent<Text>().color = fontColor;     //  pause label

        //  hide panels
        ShowPanels(false);

        //  hide pause button and menu
        pauseButton.SetActive(false);
        pauseMenu.SetActive(false);

        //  show opening story
        StartCoroutine(OpeningStory(OPENING_STORY_TIME));
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
    

    IEnumerator OpeningStory(float duration)
    {
        //  run animation
        Destroy(Instantiate(Resources.Load(levelID + "_idle"), new Vector3(0, 1, 1), Quaternion.identity), duration);

        //  add text with plea, part 1
        string plea = "";
        
        if(levelID == "knight")
            plea = "GOOD DAY. MY NAME IS SIR ARTHUR DAYNE AND I REQUIRE YOUR ASSISTANCE.";
        else if (levelID == "dragon")
            plea = "ROAR! THEY CALL ME BALERION AND I HEARD YOU ARE GOOD AT FINDING STUFF.";
        else if (levelID == "princess")
            plea = "PLEASED TO MEET YOU, MY NAME IS LADY ELENA AND I'D LIKE TO ASK YOU A FAVOUR.";
        else if (levelID == "alien")
            plea = "GREETINGS, EARTHLING. I AM REFERED TO AS SHA'TRA AND I COULD USE YOUR ASSISTANCE.";
        else if (levelID == "fox")
            plea = "HIYA! KIT THE FOX HERE, NICE TO MEET YOU. CAN YOU GIVE ME A HAND WITH SOMETHING?";
        else if (levelID == "penguin")
            plea = "HEY THERE! I AM TUX AND I REALLY NEED YOUR HELP FOR A FEW MINUTES.";
        else if (levelID == "girl")
            plea = "HI! I AM SARA AND I WONDER IF YOU COULD HELP ME WITH SOMETHING.";
        else if (levelID == "droid")
            plea = "HELLO, HUMAN. I AM CALLED R3D6 AND I COULD DO WITH SOME HELP, PLEASE.";

        Destroy(CreateText(plea, fontColor, false, new Vector2(0, Screen.height * 0.25f), 30), duration * 0.5f);

        //  wait for the user to read it
        yield return new WaitForSeconds(duration * 0.5f);

        //  add text with plea, part 2
        string plea2 = "";

        if (levelID == "knight")
            plea2 = "I LOST MY " + MAX_ROUNDS + " BEST WEAPONS. COULD YOU HELP ME FIND THEM?";
        else if (levelID == "dragon")
            plea2 = "I LOST MY " + MAX_ROUNDS + " BEST WEAPONS. COULD YOU HELP ME FIND THEM?";
        else if (levelID == "princess")
            plea2 = "I SEEM TO HAVE LOST MY " + MAX_ROUNDS + " FAVOURITE ACCESSORIES. WOULD YOU MIND FINDING THEM FOR ME?";
        else if (levelID == "alien")
            plea2 = "I LOST MY " + MAX_ROUNDS + " BEST WEAPONS. COULD YOU HELP ME FIND THEM?";
        else if (levelID == "fox")
            plea2 = "I LOST MY " + MAX_ROUNDS + " BEST WEAPONS. COULD YOU HELP ME FIND THEM?";
        else if (levelID == "penguin")
            plea2 = "I LOST MY " + MAX_ROUNDS + " BEST WEAPONS. COULD YOU HELP ME FIND THEM?";
        else if (levelID == "girl")
            plea2 = "I LOST MY " + MAX_ROUNDS + " BEST WEAPONS. COULD YOU HELP ME FIND THEM?";
        else if (levelID == "droid")
            plea2 = "I LOST MY " + MAX_ROUNDS + " BEST WEAPONS. COULD YOU HELP ME FIND THEM?";

        Destroy(CreateText(plea2, fontColor, false, new Vector2(0, Screen.height * 0.25f), 30), duration * 0.5f);

        //  wait for opening story to finish playing
        yield return new WaitForSeconds(duration * 0.5f);

        //  start the game
        StartGame();
    }


    void StartGame()
    {
        //  choose a correct panel
        correctPanel = Random.Range(0, 4);
        transform.GetChild(correctPanel).GetComponent<PanelSelected>().ToggleIsCorrect();

        //  show panels
        ShowPanels(true);

        //  show pause button
        pauseButton.SetActive(true);
    }


    public void PanelClicked(int id)
    {
        //  hide panels
        ShowPanels(false);

        //  check if the clicked panel is the correct one
        if (id == correctPanel)
            ClickedCorrectPanel();
        else
            ClickedWrongPanel();
    }


    IEnumerator ResetPanelsAfterTime(float delay)
    {
        //  wait for the delay
        yield return new WaitForSeconds(delay);

        //  show panels
        ShowPanels(true);

        //  choose a new correct panel
        transform.GetChild(correctPanel).GetComponent<PanelSelected>().ToggleIsCorrect();       //  reset currently correct panel        
        correctPanel = Random.Range(0, 4);              //  choose new random correct panel        
        transform.GetChild(correctPanel).GetComponent<PanelSelected>().ToggleIsCorrect();       //  set the new chosen panel as correct     

        yield return null;
    }



    void ClickedCorrectPanel()
    {
        //  update score
        score++;
        scoreText.text = score.ToString();

        //  check for victory
        if (score == MAX_ROUNDS)
        {
            StartCoroutine(GameWon());
            return;
        }

        //  run character animation
        Destroy(Instantiate(Resources.Load(levelID + "_happy"), new Vector3(Random.Range(-SCREEN.x, SCREEN.x), Random.Range(-1.0f, 4.0f), 1), Quaternion.identity), ANIMATION_TIME);

        //  add text reaction
        Destroy(CreateText(celebrationQuotes[Random.Range(0, celebrationQuotes.Length)], fontColor), ANIMATION_TIME);

        //  run coin animation
        GameObject coin = Instantiate(Resources.Load("coin"), new Vector3(Random.Range(-SCREEN.x, SCREEN.x), -2.0f, 1), Quaternion.identity) as GameObject;
        coin.transform.localScale = new Vector3(1, 1, 1);
        StartCoroutine(MoveAndDie(coin, coin.transform.up, ANIMATION_TIME * 0.75f, ANIMATION_TIME));

        //  reveal the item
        RevealItem();

        //  reset panels back after animations
        coroutine = StartCoroutine(ResetPanelsAfterTime(ANIMATION_TIME));
    }


    void ClickedWrongPanel()
    {
        //  minus 1 life
        lifes--;
        StartCoroutine(GrowFadeOutAndDie(hearts.transform.GetChild(lifes).gameObject, ANIMATION_TIME * 0.5f));  // start disappearing animation for heart

        //  check if that was the last life 
        if (lifes == 0)
        {
            GameLost();
            return;
        }

        //  run animation
        Destroy(Instantiate(Resources.Load(levelID + "_idle"), new Vector3(Random.Range(-SCREEN.x, SCREEN.x), Random.Range(-1.0f, 4.0f), 1), Quaternion.identity), ANIMATION_TIME);

        //  add text reaction
        Destroy(CreateText(sadQuotes[Random.Range(0, sadQuotes.Length)], fontColor), ANIMATION_TIME);

        //  reset panels back after animations
        coroutine = StartCoroutine(ResetPanelsAfterTime(ANIMATION_TIME));
    }


    IEnumerator GameWon()
    {
        //  go back to main screen after the closing animation is finished
        Invoke("OnHome", WIN_CLOSING_STORY_TIME);

        //  hide pause button
        pauseButton.SetActive(false);

        //  run animation
        Destroy(Instantiate(Resources.Load(levelID + "_happy"), new Vector3(0, 1, 1), Quaternion.identity), WIN_CLOSING_STORY_TIME);

        //  add text with quote
        string quote = "";

        if (levelID == "knight")
            quote = "YES, YOU DID IT! YOU ARE A TRUE NOBLE HERO!";
        else if (levelID == "dragon")
            quote = "ROAR! THEY CALL ME BALERION AND I HEARD YOU ARE GOOD AT FINDING STUFF.";
        else if (levelID == "princess")
            quote = "YES, YOU MADE IT! I AM MUCH OBLIGED TO YOU FOR YOUR KINDNESS.";
        else if (levelID == "alien")
            quote = "GREETINGS, EARTHLING. I AM REFERED TO AS SHA'TRA AND I COULD USE YOUR ASSISTANCE.";
        else if (levelID == "fox")
            quote = "HIYA! KIT THE FOX HERE, NICE TO MEET YOU. CAN YOU GIVE ME A HAND WITH SOMETHING?";
        else if (levelID == "penguin")
            quote = "HEY THERE! I AM TUX AND I REALLY NEED YOUR HELP FOR A FEW MINUTES.";
        else if (levelID == "girl")
            quote = "HI! I AM SARA AND I WONDER IF YOU COULD HELP ME WITH SOMETHING.";
        else if (levelID == "droid")
            quote = "HELLO, HUMAN. I AM CALLED R3D6 AND I COULD DO WITH SOME HELP, PLEASE.";

        Destroy(CreateText(quote, fontColor, false, new Vector2(0, Screen.height * 0.25f), 30), WIN_CLOSING_STORY_TIME);

        //  make coins fall from the sky
        for(int i = 0; i < MAX_ROUNDS; i++)
        {
            GameObject coin = Instantiate(Resources.Load("coin"), new Vector3(Random.Range(-SCREEN.x, SCREEN.x), 5.0f, 1), Quaternion.identity) as GameObject;
            coin.transform.localScale = new Vector3(0.75f, 0.75f, 1);
            StartCoroutine(MoveAndDie(coin, -coin.transform.up, ANIMATION_TIME, ANIMATION_TIME));
            yield return new WaitForSeconds(0.05f);
        }

        //  make items appear around character
        for (int i = 0; i < items.Length; i++)
        {
            //  create a game object to hold the random item from the items sprite array
            GameObject itemGO = new GameObject("item");

            SpriteRenderer itemSR = itemGO.AddComponent<SpriteRenderer>();
            itemSR.sprite = items[i];

            //  position it on the left or right of the character with declining height (3 items on the left and 3 on the right)
            float x = (i==1 || i==4) ? 4.5f : 7.0f;         //  TODO:: 6.0f : 9.0f
            x *= (i < items.Length / 2) ? -1 : 1;
            float y = 4.0f - 3 * (i % (items.Length / 2));
            Vector3 position = new Vector3(x, y, 1);
            itemGO.transform.position = position;

            //  create a particle effect behind it
            position.z += 1;
            Instantiate(Resources.Load("vortex_particle"), position, Quaternion.identity);

            yield return new WaitForSeconds(1);
        }
    }

    
    void GameLost()
    {
        //  hide pause button
        pauseButton.SetActive(false);

        //  run animation
        Destroy(Instantiate(Resources.Load(levelID + "_idle"), new Vector3(0, 1, 1), Quaternion.identity), LOSS_CLOSING_STORY_TIME);

        //  add text with quote
        string quote = "";

        if (levelID == "knight")
            quote = "GOOD GAME, CHAMP. BETTER LUCK NEXT TIME!";
        else if (levelID == "dragon")
            quote = "ROAR! THEY CALL ME BALERION AND I HEARD YOU ARE GOOD AT FINDING STUFF.";
        else if (levelID == "princess")
            quote = "DO NOT LOSE FAITH - YOU WILL GET THEM NEXT TIME.";
        else if (levelID == "alien")
            quote = "GREETINGS, EARTHLING. I AM REFERED TO AS SHA'TRA AND I COULD USE YOUR ASSISTANCE.";
        else if (levelID == "fox")
            quote = "HIYA! KIT THE FOX HERE, NICE TO MEET YOU. CAN YOU GIVE ME A HAND WITH SOMETHING?";
        else if (levelID == "penguin")
            quote = "HEY THERE! I AM TUX AND I REALLY NEED YOUR HELP FOR A FEW MINUTES.";
        else if (levelID == "girl")
            quote = "HI! I AM SARA AND I WONDER IF YOU COULD HELP ME WITH SOMETHING.";
        else if (levelID == "droid")
            quote = "HELLO, HUMAN. I AM CALLED R3D6 AND I COULD DO WITH SOME HELP, PLEASE.";

        Destroy(CreateText(quote, fontColor, false, new Vector2(0, Screen.height * 0.25f), 30), LOSS_CLOSING_STORY_TIME);

        //  go back to main screen after the closing animation is finished
        Invoke("OnHome", LOSS_CLOSING_STORY_TIME);
    }


    void ShowPanels(bool shouldShow)
    {
        foreach (Transform child in transform)
            child.gameObject.SetActive(shouldShow);
    }


    IEnumerator MoveAndDie(GameObject obj, Vector3 direction, float speed, float duration)
    {
        float currentTime = 0.0f ;
        while (currentTime <= duration)
        {
            if (!pauseMenu.activeInHierarchy)
                yield return null;

            obj.transform.Translate(direction * speed * Time.deltaTime);
            currentTime += Time.deltaTime;

        }

        Destroy(obj);
    }


    IEnumerator GrowFadeOutAndDie(GameObject obj, float duration)
    {
        obj.GetComponent<Image>().CrossFadeAlpha(0.0f, ANIMATION_TIME, false);
        Vector3 scaleUp = new Vector3(0.0025f, 0.0025f, 0f);
        Destroy(obj, ANIMATION_TIME);

        while (true)
        {
            obj.transform.localScale += scaleUp;

            if (!pauseMenu.activeInHierarchy)
                yield return null;
        }
    }


    void RevealItem()
    {
        //  create a game object to hold the random item from the items sprite array
        GameObject itemGO = new GameObject("item");

        SpriteRenderer itemSR = itemGO.AddComponent<SpriteRenderer>();
        itemSR.sprite = items[Random.Range(0, items.Length)];

        //  position it where the correct panel is
        Vector3 position = new Vector3();
        switch (correctPanel)       
        {
            // TODO:: fix this
            /*case 0: position = new Vector3(-6.5f, 2.5f, 0); break;
            case 1: position = new Vector3(6.5f, 2.5f, 0); break;
            case 2: position = new Vector3(-6.5f, -1.5f, 0); break;
            case 3: position = new Vector3(6.5f, -1.5f, 0); break;*/
            case 0: position = new Vector3(-SCREEN.x/2, 3f, 0); break;
            case 1: position = new Vector3(SCREEN.x/2, 3f, 0); break;
            case 2: position = new Vector3(-SCREEN.x/2, -1f, 0); break;
            case 3: position = new Vector3(SCREEN.x/2, -1f, 0); break;
        }
        itemGO.transform.position = position;

        Destroy(itemGO, ANIMATION_TIME);

        //  create a particle effect behind it
        position.z += 1;
        Destroy(Instantiate(Resources.Load("vortex_particle"), position, Quaternion.identity), ANIMATION_TIME);
    }


    public void OnPause()
    {   
        //  pause game objects     
        Time.timeScale = 0.0f;
        
        //  hide panels
        ShowPanels(false);

        //  hide pause button
        pauseButton.SetActive(false);

        //  stop the coroutine that is about to reset the panels
        StopCoroutine(coroutine);
                
        //  show pause menu
        pauseMenu.SetActive(true);

        //  remove any texts with quotes, character sprites, coins, items or particles (if there are such on the screen)
        try {
            Destroy(GameObject.Find("text"));
            Destroy(GameObject.Find(levelID + "_happy(Clone)"));
            Destroy(GameObject.Find(levelID + "_idle(Clone)"));
            Destroy(GameObject.Find("coin(Clone)"));
            Destroy(GameObject.Find("item"));
            Destroy(GameObject.Find("vortex_particle(Clone)"));
        }
        catch{}
    }


    public void OnResume()
    {
        //  unpause game objects     
        Time.timeScale = 1.0f;

        //  show panels
        //ShowPanels(true);

        //  show pause button
        pauseButton.SetActive(true);

        //  hide pause menu
        pauseMenu.SetActive(false);

        //  start coroutine to reset the panels without any delay
        coroutine = StartCoroutine(ResetPanelsAfterTime(0));
    }


    public void OnHome()
    {
        //  unpause game objects     
        Time.timeScale = 1.0f;

        SceneManager.LoadScene("mainMenu");
    }


    public void OnRestart()
    {
        //  unpause game objects     
        Time.timeScale = 1.0f;

        SceneManager.LoadScene("level");
    }


    GameObject CreateText(string textString, Color textColor, bool onRandomLocation = true, Vector2 position = default(Vector2), int fontSize = 60/*45*/)
    {
        GameObject textGO = new GameObject("text");
        textGO.transform.SetParent(GameObject.Find("Canvas").transform);

        Text text = textGO.AddComponent<Text>();
        text.text = textString;        
        text.font = Resources.Load<Font>("JazzCreateBubble");
        text.fontSize = fontSize;
        text.color = textColor;
        text.rectTransform.sizeDelta = new Vector2(350, 100);
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
    
}
