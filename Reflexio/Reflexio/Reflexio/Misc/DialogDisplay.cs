using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Xml;

namespace Reflexio
{
    public class DialogDisplay
    {
        Queue<String> text_queue;
        Queue<Int32> time_queue;
        int time_elapsed;
        String current_text;
        int current_end_time;
        bool finished;
        SpriteFont Font1 = GameEngine.Font;
        Vector2 FontPos = new Vector2(GameEngine.Instance.GraphicsDevice.Viewport.Width / 2,
                   GameEngine.Instance.GraphicsDevice.Viewport.Height * 0.8f);

        private void Dequeue()
        {
            try
            {
                current_text = text_queue.Dequeue();
                current_end_time = time_elapsed + time_queue.Dequeue();
            }
            catch
            {
                finished = true;
            }
        }
        public void Initialize()
        {
            time_elapsed = 0;
            finished = false;
            Dequeue();
        }

        public DialogDisplay()
        {
            text_queue = new Queue<String>();
            time_queue = new Queue<Int32>();
        }

        public void AddDialog(String text, int frames)
        {
            text_queue.Enqueue(text);
            time_queue.Enqueue(frames);
        }

        public void Update()
        {
            if (!finished)
            {
                time_elapsed++;
                if (time_elapsed > current_end_time)
                    Dequeue();
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (!finished)
            {
                spriteBatch.Begin();
                // Find the center of the string
                Vector2 FontOrigin = Font1.MeasureString(current_text) / 2;
                // Draw the string
                spriteBatch.DrawString(Font1, current_text, FontPos, Color.Black,
                    0, FontOrigin, 1.0f, SpriteEffects.None, 0f);
                spriteBatch.End();
            }
        }

        public void Serialize(XmlWriter writer)
        {
            writer.WriteStartElement("DialogDisplay");
            while (text_queue.Count != 0)
            {
                writer.WriteElementString("Dialog", text_queue.Dequeue() + ", " + time_queue.Dequeue().ToString());
            }
            writer.WriteElementString("Initialize", "");
            writer.WriteEndElement();
        }
    }
}
