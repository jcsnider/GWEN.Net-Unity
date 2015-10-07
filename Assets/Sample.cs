using UnityEngine;
using System.Collections;
using Gwen;
using Gwen.Control;
using Gwen.Renderer;
using Gwen.UnitTest;
using Button = Gwen.Control.Button;

public class Sample : MonoBehaviour
{
    private static Gwen.Input.UnityGwenInput m_Input;
    private static Gwen.Control.Canvas m_Canvas;
    private static UnitTest m_UnitTest;
    private Texture2D SkinTexture;

    //Custom Mouse Movement Event Handling
    private float mousex;
    private float mousey;

    private int width = Screen.width;
    private int height = Screen.height;
    Gwen.Control.WindowControl win;

	// Use this for initialization
	void Start () {
        // create GWEN renderer
        UnityGwenRenderer gwenRenderer = new UnityGwenRenderer();

        // Create GWEN skin
        //Skin.Simple skin = new Skin.Simple(GwenRenderer);
        Gwen.Skin.TexturedBase skin = new Gwen.Skin.TexturedBase(gwenRenderer, "DefaultSkin");

        // set default font
        skin.DefaultFont = new Gwen.Font(gwenRenderer, "Arvo-Regular", 10);

        // Create a Canvas (it's root, on which all other GWEN controls are created)
        m_Canvas = new Gwen.Control.Canvas(skin);
        m_Canvas.SetSize(width, height);
        m_Canvas.ShouldDrawBackground = true;
        m_Canvas.BackgroundColor = new System.Drawing.Color(255, 150, 170, 170);
        m_Canvas.KeyboardInputEnabled = true;

        //win = new Gwen.Control.WindowControl(m_Canvas);
        //win.SetBounds(50, 50, 300, 300);
        //win.RenderColor = System.Drawing.Color.White;
        

        // Create GWEN input processor
        m_Input = new Gwen.Input.UnityGwenInput();
        m_Input.Initialize(m_Canvas);
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnGUI() {
		//GUI.DrawTexture (new Rect (0, 0, Screen.width, Screen.height), guiTex);
	    //GUI.DrawTextureWithTexCoords(new Rect(0, 0, 400, 400), skinTex, new Rect(0, 1f - (126f/512f), .2f,126f/512f));
        if (m_Canvas.Width != Screen.width || m_Canvas.Height != Screen.height) m_Canvas.SetSize(Screen.width,Screen.height);
	    if (m_UnitTest == null)
	    {
            //win.Title = "This is a window!";
            // create the unit test control
            m_UnitTest = new UnitTest(m_Canvas);
	    }
        if (Event.current.mousePosition.x != mousex || Event.current.mousePosition.y != mousey)
        {
            mousex = Event.current.mousePosition.x;
            mousey = Event.current.mousePosition.y;
            Event tmpevent = new Event();
            tmpevent.type = EventType.MouseMove;
            tmpevent.mousePosition = Event.current.mousePosition;
            m_Input.ProcessMessage(tmpevent);
        }
	    if (Event.current.type == EventType.Repaint)
	    {
            m_Canvas.RenderCanvas();
            
	    }
        else if (Event.current.type == EventType.MouseDown || Event.current.type == EventType.ScrollWheel || Event.current.type == EventType.MouseUp || Event.current.type == EventType.KeyDown || Event.current.type == EventType.KeyUp)
        {
            m_Input.ProcessMessage(Event.current);
        }
        
	}
}
