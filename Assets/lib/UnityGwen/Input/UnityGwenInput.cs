using System;
using Gwen.Control;

namespace Gwen.Input
{
    /// <summary>
    /// SFML input handler.
    /// </summary>
    public class UnityGwenInput
    {
        private Canvas m_Canvas;
        private int m_MouseX;
        private int m_MouseY;

        public UnityGwenInput()
        {
            // not needed, retained for clarity
            m_MouseX = 0;
            m_MouseY = 0;
        }

        /// <summary>
        /// Sets the currently active canvas.
        /// </summary>
        /// <param name="canvas">Canvas to use.</param>
        /// <param name="target">Rander target (needed for scaling).</param>
        public void Initialize(Canvas canvas)
        {
            m_Canvas = canvas;
        }

        /// <summary>
        /// Translates control key's SFML key code to GWEN's code.
        /// </summary>
        /// <param name="sfKey">SFML key code.</param>
        /// <returns>GWEN key code.</returns>
        private static Key TranslateKeyCode(UnityEngine.KeyCode sfKey)
        {
            switch (sfKey)
            {
                case UnityEngine.KeyCode.Backspace: return Key.Backspace;
                case UnityEngine.KeyCode.Return: return Key.Return;
                case UnityEngine.KeyCode.Escape: return Key.Escape;
                case UnityEngine.KeyCode.Tab: return Key.Tab;
                case UnityEngine.KeyCode.Space: return Key.Space;
                case UnityEngine.KeyCode.UpArrow: return Key.Up;
                case UnityEngine.KeyCode.DownArrow: return Key.Down;
                case UnityEngine.KeyCode.LeftArrow: return Key.Left;
                case UnityEngine.KeyCode.RightArrow: return Key.Right;
                case UnityEngine.KeyCode.Home: return Key.Home;
                case UnityEngine.KeyCode.End: return Key.End;
                case UnityEngine.KeyCode.Delete: return Key.Delete;
                case UnityEngine.KeyCode.LeftControl: return Key.Control;
                case UnityEngine.KeyCode.LeftAlt: return Key.Alt;
                case UnityEngine.KeyCode.LeftShift: return Key.Shift;
                case UnityEngine.KeyCode.RightControl: return Key.Control;
                case UnityEngine.KeyCode.RightAlt: return Key.Alt;
                case UnityEngine.KeyCode.RightShift: return Key.Shift;
            }
            return Key.Invalid;
        }

        /// <summary>
        /// Translates alphanumeric SFML key code to character value.
        /// </summary>
        /// <param name="sfKey">SFML key code.</param>
        /// <returns>Translated character.</returns>
        private static char TranslateChar(UnityEngine.KeyCode sfKey)
        {
            if (sfKey >= UnityEngine.KeyCode.A && sfKey <= UnityEngine.KeyCode.Z)
                return (char)('A' + (int)sfKey);
            return ' ';
        }

        /// <summary>
        /// Main entrypoint for processing input events. Call from your RenderWindow's event handlers.
        /// </summary>
        /// <param name="args">SFML input event args: can be MouseMoveEventArgs, SFMLMouseButtonEventArgs, MouseWheelEventArgs, TextEventArgs, SFMLKeyEventArgs.</param>
        /// <returns>True if the event was handled.</returns>
        public bool ProcessMessage(UnityEngine.Event args)
        {
            if (null == m_Canvas) return false;

            if (args.type == UnityEngine.EventType.MouseMove)
            {
                UnityEngine.Vector2 ev = new UnityEngine.Vector2();
                UnityEngine.Vector2 coord = UnityEngine.Event.current.mousePosition;
                ev.x = (int)Math.Floor(coord.x);
                ev.y = (int)Math.Floor(coord.y);

                int dx = (int)ev.x - m_MouseX;
                int dy = (int)ev.y - m_MouseY;

                m_MouseX = (int)ev.x;
                m_MouseY = (int)ev.y;

                return m_Canvas.Input_MouseMoved(m_MouseX, m_MouseY, dx, dy);
            }

            if (args.type == UnityEngine.EventType.MouseDown)
            {
                return m_Canvas.Input_MouseButton((int)args.button, true);
            }

            if (args.type == UnityEngine.EventType.MouseUp)
            {
                return m_Canvas.Input_MouseButton((int)args.button, false);
            }


            if (args.type == UnityEngine.EventType.ScrollWheel)
            {
                return m_Canvas.Input_MouseWheel((int)args.delta.y * -60);
            }

            if (args.type == UnityEngine.EventType.KeyDown)
            {
                UnityEngine.Event ev = args;

                if (ev.control && ev.alt && ev.keyCode == UnityEngine.KeyCode.LeftControl)
                    return false; // this is AltGr

                char ch = TranslateChar(ev.keyCode);
                if (ev.button > -1 && InputHandler.DoSpecialKeys(m_Canvas, ch))
                    return false;

                Key key = TranslateKeyCode(ev.keyCode);
                if (key == Key.Invalid || key == Key.Space)
                {
                    m_Canvas.Input_Character((char)ev.keyCode);
                    return InputHandler.HandleAccelerator(m_Canvas, ch);
                }
                return m_Canvas.Input_Key(key, true);//TODO FIX THIS LAST PARAMETER
            }

            if (args.type == UnityEngine.EventType.KeyUp)
            {
                UnityEngine.Event ev = args;

                if (ev.control && ev.alt && ev.keyCode == UnityEngine.KeyCode.LeftControl)
                    return false; // this is AltGr

                Key key = TranslateKeyCode(ev.keyCode);
                return m_Canvas.Input_Key(key, false);//TODO FIX THIS LAST PARAMETER
            }

            throw new ArgumentException("Invalid event args", "args");
        }
    }
}
