using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using SdlDotNet.Core;
using SdlDotNet.Graphics;
using SdlDotNet.Input;

namespace NamcapGame
{
    public class Sprite
    {
        private PointF m_location;
        private Point m_direction;
        public float Speed { get; private set; }
        public Surface Image { get; private set; }
        public PointF Location { get { return m_location; } }
        
        public Sprite(Surface surface, PointF location, float speed)
        {
            Image = surface;
            m_location = location;
            Speed = speed;
            m_direction = new Point(0, 0);
        }

        public void Direct(Key key, bool down)
        {
            if (down)
            {
                switch (key)
                {
                    case Key.DownArrow:
                        m_direction.Y = 1;
                        break;
                    case Key.UpArrow:
                        m_direction.Y = -1;
                        break;
                    case Key.LeftArrow:
                        m_direction.X = -1;
                        break;
                    case Key.RightArrow:
                        m_direction.X = 1;
                        break;
                }
            }
            else
            {
                switch (key)
                {
                    case Key.DownArrow:
                        m_direction.Y = 0;
                        break;
                    case Key.UpArrow:
                        m_direction.Y = 0;
                        break;
                    case Key.LeftArrow:
                        m_direction.X = 0;
                        break;
                    case Key.RightArrow:
                        m_direction.X = 0;
                        break;
                }
            }

        }

        public float X
        {
            get
            {
                return m_location.X;
            }

            set
            {
                m_location.X = value;
            }
        }

        public float Y
        {
            get
            {
                return m_location.Y;
            }

            set
            {
                m_location.Y = value;
            }
        }

        public void Move(float elapsed)
        {
            m_location.X += m_direction.X * Speed * elapsed;
            m_location.Y += m_direction.Y * Speed * elapsed;
        }
    }
}
