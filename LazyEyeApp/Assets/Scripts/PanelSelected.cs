using UnityEngine;
using System.Collections;

public class PanelSelected : MonoBehaviour {
    
    private const int rectCount = 9;
    static Vector2 panelHalfDim;        // WARNING: this assumes that all 4 panels are of the same size!!
    static Vector2 panelQuarterDim;     // WARNING: this assumes that all 4 panels are of the same size!!
    static float height;

    static Vector2 rectSize = new Vector2(Screen.width * 0.01f, Screen.width * 0.01f);
    static Texture2D[] rectTexture = new Texture2D[rectCount];
    static GUIStyle[] rectStyle = new GUIStyle[rectCount];

    static Color[] colors = { Color.black, Color.blue, Color.cyan, Color.gray, Color.green, Color.magenta, Color.red, Color.white, Color.yellow };
        
    private bool isCorrectPanel = false;
    private int insideBoxColor;


    void Start()
    {
        //  resize panel to 0.4 (local scale) of the screen width and height
        GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width, Screen.height);
             
        //  initialize variables for rectangles drawing   
        height = Screen.height;
        panelHalfDim = new Vector2(GetComponent<RectTransform>().rect.width * transform.localScale.x * 0.45f, GetComponent<RectTransform>().rect.height * transform.localScale.y * 0.45f);
        panelQuarterDim = new Vector2(panelHalfDim.x * 0.5f, panelHalfDim.y * 0.5f);

        for (int i = 0; i < rectCount; i++)
        {
            rectTexture[i] = new Texture2D(1, 1);
            rectTexture[i].SetPixel(0, 0, colors[i]);
            rectTexture[i].Apply();

            rectStyle[i] = new GUIStyle();
            rectStyle[i].normal.background = rectTexture[i];
        }
    }

	public void OnClick()
    {
        //  call parent's script to inform it that this panel has been clicked
        ClickPanels other = (ClickPanels)this.GetComponentInParent(typeof(ClickPanels));
        other.PanelClicked(int.Parse(this.name));        
    }

    void OnGUI()
    {
        //  draw differently coloured rectangles on random positions inside the panel
        for (int i = 0; i < 150; i++)
            DrawRect(i % 9);

        //  if this is the correct panel with the hidden box, draw same coloured rectangles in the center of it
        if (isCorrectPanel)
        {            
            for(int i = 0; i < 75; i++)
            {
                DrawRectCentrally(insideBoxColor);
            }
        }
    }

    public void DrawRect(int index)
    {
        Vector2 rectPosition = new Vector2(Random.Range(transform.position.x - panelHalfDim.x, transform.position.x + panelHalfDim.x),
                                            Random.Range(height - transform.position.y - panelHalfDim.y, height - transform.position.y + panelHalfDim.y));

        GUI.Box(new Rect(rectPosition, rectSize), GUIContent.none, rectStyle[index]);
    }

    public void DrawRectCentrally(int index)
    {
        Vector2 rectPosition = new Vector2(Random.Range(transform.position.x - panelQuarterDim.x, transform.position.x + panelQuarterDim.x),
                                            Random.Range(height - transform.position.y - panelQuarterDim.y, height - transform.position.y + panelQuarterDim.y));
                                                  
        GUI.Box(new Rect(rectPosition, rectSize), GUIContent.none, rectStyle[index]);
    }

    public void ToggleIsCorrect()
    {
        isCorrectPanel = !isCorrectPanel;
        insideBoxColor = Random.Range(0, rectCount);
    }

}
