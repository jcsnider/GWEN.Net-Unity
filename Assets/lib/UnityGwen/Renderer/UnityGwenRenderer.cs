using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using Color = System.Drawing.Color;
using UnityEngine;

namespace Gwen.Renderer
{
    /// <summary>
    /// SFML renderer.
    /// </summary>
    public class UnityGwenRenderer : Renderer.Base
    {
        //No Target Needed, Rendering Directly to GUI
        private Color m_Color;
        private Vector2 m_ViewScale;
        private Texture2D whiteTex;
        private bool clipping = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnityGwenRenderer"/> class.
        /// </summary>
        /// <param name="target">Unity render target.</param>
        public UnityGwenRenderer()
        {
            whiteTex = new Texture2D(1, 1);
            whiteTex.SetPixel(0, 0, UnityEngine.Color.white);
            whiteTex.Apply();
            m_ViewScale = Vector2.one;
        }

        /// <summary>
        /// Gets or sets the current drawing color.
        /// </summary>
        public override System.Drawing.Color DrawColor
        {
            get
            {
                return System.Drawing.Color.FromArgb(m_Color.A, m_Color.R, m_Color.G, m_Color.B);
            }
            set
            {
                m_Color = new Color(value.A, value.R, value.G, value.B);
            }
        }

        public override System.Drawing.Color PixelColor(Texture texture, uint x, uint y, System.Drawing.Color defaultColor)
        {
            int x1 = (int)x;
            int y1 = (int)y;
            UnityEngine.Texture2D tex = texture.RendererData as UnityEngine.Texture2D;
            if (tex == null)
                return defaultColor;
            UnityEngine.Color clr = tex.GetPixel(x1,tex.height - y1);
            return new Color((byte)(clr.a * 255), (byte)(clr.r * 255), (byte)(clr.g * 255), (byte)(clr.b * 255));
        }

        /*
        public override void DrawLine(int x1, int y1, int x2, int y2)
        {
            Translate(ref x1, ref y1);
            Translate(ref x2, ref y2);

            Vertex[] line = {new Vertex(new Vector2f(x1, y1), m_Color), new Vertex(new Vector2f(x2, y2), m_Color)};

            m_Target.Draw(line, PrimitiveType.Lines);
        }
        */
        /// <summary>
        /// Loads the specified font.
        /// </summary>
        /// <param name="font">Font to load.</param>
        /// <returns>True if succeeded.</returns>
        public override bool LoadFont(Font font)
        {
            font.RealSize = font.Size*Scale;
            bool ret = true;

            try
            {
                font.RendererData = (UnityEngine.Font)Resources.Load(font.FaceName);
            }
            catch (Exception)
            {
                return false;
            }

            if (font.RendererData == null) return false;

            return ret;
        }

        /// <summary>
        /// Frees the specified font.
        /// </summary>
        /// <param name="font">Font to free.</param>
        public override void FreeFont(Font font)
        {
            if ( font.RendererData == null ) return;
            font.RendererData = null;
        }

        /// <summary>
        /// Returns dimensions of the text using specified font.
        /// </summary>
        /// <param name="font">Font to use.</param>
        /// <param name="text">Text to measure.</param>
        /// <returns>
        /// Width and height of the rendered text.
        /// </returns>
        public override Point MeasureText(Font font, string text)
        {
            if (font == null || font.RendererData == null)
            {
                return Point.Empty;
            }
            Vector2 size = GUI.skin.label.CalcSize(new GUIContent(text));
            return new System.Drawing.Point((int)size.x, (int)size.y);
        }

        public override void RenderText(Font font, Point pos, string text)
        {
            pos = Translate(pos);
            GUI.skin.label.normal.textColor = new UnityEngine.Color32(m_Color.R,m_Color.G,m_Color.B,m_Color.A);
            GUI.color = UnityEngine.Color.white;
            Rect clip = new Rect(ClipRegion.X, ClipRegion.Y, ClipRegion.Width, ClipRegion.Height);
            GUI.BeginGroup(clip);
            GUI.Label(new Rect(pos.X - clip.x,pos.Y - clip.y,Screen.width,Screen.height), text);
            GUI.EndGroup();
        }

        public override void DrawFilledRect(Rectangle targetRect)
        {
            Rect rect = new Rect(Translate(targetRect).X, Translate(targetRect).Y, Translate(targetRect).Width, Translate(targetRect).Height);

            //TODO
            GUIStyle box = new GUIStyle();
            box.normal.background = whiteTex;
            GUI.color = new UnityEngine.Color32(m_Color.R, m_Color.G, m_Color.B, m_Color.A);

            if (clipping)
            {
                Rect clip = new Rect(ClipRegion.X, ClipRegion.Y, ClipRegion.Width, ClipRegion.Height);
                clip.x = (int)Math.Round(clip.x * m_ViewScale.x);
                clip.y = (int)Math.Round(clip.y * m_ViewScale.y);
                clip.width = (int)Math.Round(clip.width * m_ViewScale.x);
                clip.height = (int)Math.Round(clip.height * m_ViewScale.y);

                float diff = 0;
                if (rect.x < clip.x)
                {
                    diff = clip.x - rect.x;
                    rect.x += diff;
                    rect.width -= diff;
                }

                if (rect.x + rect.width > clip.x + clip.width)
                {
                    diff = (rect.x + rect.width) - (clip.x + clip.width);
                    rect.width -= diff;
                }

                if (rect.y < clip.y)
                {
                    diff = clip.y - rect.y;
                    rect.y += diff;
                    rect.height -= diff;
                }

                if (rect.y + rect.height > clip.y + clip.height)
                {
                    diff = (rect.y + rect.height) - (clip.y + clip.height);
                    rect.height -= diff;
                }

                if (rect.width <= 0) { return; }
                if (rect.height <= 0) { return; }
            }


            GUI.Box(new Rect(rect), GUIContent.none, box);
        }

        public override void DrawTexturedRect(Gwen.Texture tex, Rectangle targetRect, Color clr, float u1 = 0, float v1 = 0, float u2 = 1, float v2 = 1)
        {
            if (tex.RendererData != null)
            {
                UnityEngine.Texture uTex = (UnityEngine.Texture)tex.RendererData;
                DrawTexturedRect(uTex, targetRect, clr, u1, v1, u2, v2);
            }
            else
            {
                DrawMissingImage(targetRect);
            }
        }

        protected void DrawTexturedRect(UnityEngine.Texture tex, Rectangle targetRect, Color clr, float u1 = 0, float v1 = 0, float u2 = 1, float v2 = 1)
        {
            Rect rect = new Rect(Translate(targetRect).X,Translate(targetRect).Y,Translate(targetRect).Width,Translate(targetRect).Height);

            if (null == tex)
            {
                DrawMissingImage(targetRect);
                return;
            }

            u1 *= tex.width;
            v1 *= tex.height;
            u2 *= tex.width;
            v2 *= tex.height;


            if (clipping)
            {
                Rect clip = new Rect(ClipRegion.X,ClipRegion.Y,ClipRegion.Width,ClipRegion.Height);
                clip.x = (int)Math.Round(clip.x * m_ViewScale.x);
                clip.y = (int)Math.Round(clip.y * m_ViewScale.y);
                clip.width = (int)Math.Round(clip.width * m_ViewScale.x);
                clip.height = (int)Math.Round(clip.height * m_ViewScale.y);

                float diff = 0;
                float vdiff = 0;
                if (rect.x < clip.x)
                {
                    diff = clip.x - rect.x;
                    vdiff = (diff / rect.width);
                    rect.x += diff;
                    rect.width -= diff;
                    u1 += vdiff;
                }

                if (rect.x + rect.width > clip.x + clip.width)
                {
                    diff = (rect.x + rect.width) - (clip.x + clip.width);
                    vdiff = (diff / rect.width);
                    rect.width -= diff;
                    u2 -= vdiff;
                }

                if (rect.y < clip.y)
                {
                    diff = clip.y - rect.y;
                    vdiff = (diff / rect.height);
                    rect.y += diff;
                    rect.height -= diff;
                    v1 += vdiff;
                }

                if (rect.y + rect.height > clip.y + clip.height)
                {
                    diff = (rect.y + rect.height) - (clip.y + clip.height);
                    vdiff = (diff / rect.height);
                    rect.height -= diff;
                    v2 -= vdiff;
                }

                if (rect.width <= 0) { return; }
                if (rect.height <= 0) { return; }
            }
            u1 /= tex.width;
            v1 /= tex.height;
            u2 /= tex.width;
            v2 /= tex.height;

            GUI.color = new UnityEngine.Color32(m_Color.R, m_Color.G, m_Color.B, m_Color.A);
            GUI.DrawTextureWithTexCoords(rect, tex, new Rect(u1, 1f - v2, u2 - u1, v2 - v1), true);
        }

        public override void LoadTexture(Texture texture)
        {
            if (null == texture) return;

            Texture2D sfTexture;

            try
            {
                sfTexture = Resources.Load<Texture2D>(texture.Name);
                
            }
            catch (Exception)
            {
                texture.Failed = true;
                return;
            }

            if (sfTexture == null)
            {
                texture.Failed = true;
                return;
            }

            texture.Width = (int)sfTexture.width;
            texture.Height = (int)sfTexture.height;
            texture.RendererData = sfTexture;
            texture.Failed = false;
        }

        public void LoadUnityTexture(Texture uitexture, Texture2D sftexture)
        {
            if (null == uitexture) return;
            if (null == sftexture) return;

            if (uitexture.RendererData != null)
                FreeTexture(uitexture);

            uitexture.Width = (int)sftexture.width;
            uitexture.Height = (int)sftexture.height;
            uitexture.RendererData = sftexture;
            uitexture.Failed = false;
        }

        public override void StartClip()
        {
            clipping = true;
        }

        public override void EndClip()
        {
            clipping = false;
        }

       #region Implementation of ICacheToTexture

        /* private Dictionary<Control.Base, RenderTexture> m_RT;
        private Stack<RenderTexture> m_Stack;
        private RenderTexture m_RealRT;

        public void Initialize()
        {
            m_RT = new Dictionary<Control.Base, RenderTexture>();
            m_Stack = new Stack<RenderTexture>();
        }

        public void ShutDown()
        {
            m_RT.Clear();
            if (m_Stack.Count > 0)
                throw new InvalidOperationException("Render stack not empty");
        }

        /// <summary>
        /// Called to set the target up for rendering.
        /// </summary>
        /// <param name="control">Control to be rendered.</param>
        public void SetupCacheTexture(Control.Base control)
        {
            m_RealRT = m_Target;
            m_Stack.Push(m_Target); // save current RT
            m_Target = m_RT[control]; // make cache current RT
        }

        /// <summary>
        /// Called when cached rendering is done.
        /// </summary>
        /// <param name="control">Control to be rendered.</param>
        public void FinishCacheTexture(Control.Base control)
        {
            m_Target = m_Stack.Pop();
        }

        /// <summary>
        /// Called when gwen wants to draw the cached version of the control. 
        /// </summary>
        /// <param name="control">Control to be rendered.</param>
        public void DrawCachedControlTexture(Control.Base control)
        {
            RenderTexture ri = m_RT[control];
            //ri.Display();
            RenderTarget rt = m_Target;
            m_Target = m_RealRT;
            DrawTexturedRect(ri.Texture, control.Bounds, Color.White);
            //DrawMissingImage(control.Bounds);
            m_Target = rt;
        }

        /// <summary>
        /// Called to actually create a cached texture. 
        /// </summary>
        /// <param name="control">Control to be rendered.</param>
        public void CreateControlCacheTexture(Control.Base control)
        {
            // initialize cache RT
            if (!m_RT.ContainsKey(control))
            {
                m_RT[control] = new RenderTexture((uint)control.Width, (uint)control.Height);
                View view = new View(new FloatRect(0, 0, control.Width, control.Height));
                //view.Viewport = new FloatRect(0, control.Height, control.Width, control.Height);
                m_RT[control].SetView(view);
            }

            RenderTexture ri = m_RT[control];
            ri.Display();
        }

        public void UpdateControlCacheTexture(Control.Base control)
        {
            throw new NotImplementedException();
        }

        public void SetRenderer(Base renderer)
        {
            throw new NotImplementedException();
        }
        */
        #endregion
    }
}
