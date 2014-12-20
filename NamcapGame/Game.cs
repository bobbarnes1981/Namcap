using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using SdlDotNet.Core;
using SdlDotNet.Graphics;
using SdlDotNet.Input;

namespace NamcapGame
{
    class Game
    {
        private Surface m_video;

        private Game m_game;

        private string m_path = @"..\..\..\Files\";

        private char[,] m_grid;

        private int m_width = 28;

        private int m_height = 31;

        private int m_scale = 8;

        private Dictionary<char, Surface> m_cells;

        private Sprite m_npc;

        private int m_selectedPlayer = 0;

        private Sprite[] m_pc;

        public Game()
        {
            // 4 ghosts - tab to switch, arrow keys to move
            // 1 pacman - random moves, away from ghosts

            // board 28w 31h
            m_grid = new char[m_width, m_height];
            string[] data = File.ReadAllLines(m_path + "Levels\\level01.map");

            for (int y = 0; y < m_height; y++)
            {
                for (int x = 0; x < m_width; x++)
                {
                    m_grid[x, y] = data[y][x];
                }
            }
        }

        private void loadTiles()
        {
            m_cells = new Dictionary<char, Surface>();

            m_cells.Add('[', new Surface(m_path + "Images\\vlinel.png").Convert(m_video, true, false));
            m_cells.Add(']', new Surface(m_path + "Images\\vliner.png").Convert(m_video, true, false));
            m_cells.Add('-', new Surface(m_path + "Images\\hlinet.png").Convert(m_video, true, false));
            m_cells.Add('_', new Surface(m_path + "Images\\hlineb.png").Convert(m_video, true, false));
            m_cells.Add(' ', new Surface(m_path + "Images\\empty.png").Convert(m_video, true, false));
            m_cells.Add('@', new Surface(m_path + "Images\\pill.png").Convert(m_video, true, false));
            m_cells.Add('.', new Surface(m_path + "Images\\pip.png").Convert(m_video, true, false));
            m_cells.Add('/', new Surface(m_path + "Images\\ctl.png").Convert(m_video, true, false));
            m_cells.Add('\\', new Surface(m_path + "Images\\ctr.png").Convert(m_video, true, false));
            m_cells.Add('<', new Surface(m_path + "Images\\cbl.png").Convert(m_video, true, false));
            m_cells.Add('>', new Surface(m_path + "Images\\cbr.png").Convert(m_video, true, false));
            m_cells.Add('+', new Surface(m_path + "Images\\hdoorb.png").Convert(m_video, true, false));
        }

        private void loadSprites()
        {
            m_npc = new Sprite(
                new Surface(m_path + "Images\\npc.png").Convert(m_video, true, true),
                new Point(13 * m_scale, (22 * m_scale) + 4),
                40);
            m_npc.Image.Transparent = true;
            m_npc.Image.TransparentColor = Color.FromArgb(255, 0, 220);

            m_pc = new Sprite[4];

            m_pc[0] = new Sprite(
                new Surface(m_path + "Images\\pc1.png").Convert(m_video, true, true),
                new PointF(11 * m_scale, (12 * m_scale) + 4),
                30);
            m_pc[0].Image.Transparent = true;
            m_pc[0].Image.TransparentColor = Color.FromArgb(255, 0, 220);

            m_pc[1] = new Sprite(
                new Surface(m_path + "Images\\pc2.png").Convert(m_video, true, true),
                new PointF(15 * m_scale, (12 * m_scale) + 4),
                30);
            m_pc[1].Image.Transparent = true;
            m_pc[1].Image.TransparentColor = Color.FromArgb(255, 0, 220);

            m_pc[2] = new Sprite(
                new Surface(m_path + "Images\\pc3.png").Convert(m_video, true, true),
                new PointF(11 * m_scale, (14 * m_scale) + 4),
                30);
            m_pc[2].Image.Transparent = true;
            m_pc[2].Image.TransparentColor = Color.FromArgb(255, 0, 220);

            m_pc[3] = new Sprite(
                new Surface(m_path + "Images\\pc4.png").Convert(m_video, true, true),
                new PointF(15 * m_scale, (14 * m_scale) + 4),
                30);
            m_pc[3].Image.Transparent = true;
            m_pc[3].Image.TransparentColor = Color.FromArgb(255, 0, 220);
        }

        private void blitTiles()
        {
            for (int y = 0; y < m_height; y++)
            {
                for (int x = 0; x < m_width; x++)
                {
                    if (m_cells.ContainsKey(m_grid[x, y]))
                    {
                        m_video.Blit(m_cells[m_grid[x, y]], new Point(x * m_scale, y * m_scale));
                    }
                }
            }
        }

        private void blitSprites()
        {
            m_video.Blit(m_npc.Image, new Point((int)m_npc.Location.X, (int)m_npc.Location.Y));

            for (int i = 0; i < 4; i++)
            {
                m_video.Blit(m_pc[i].Image, new Point((int)m_pc[i].Location.X, (int)m_pc[i].Location.Y));
            }
        }

        private void movePlayer(float elapsed)
        {
            foreach (Sprite player in m_pc)
            {
                // collision detection with walls
                // screen wrapping
                player.Move(elapsed);
            }
        }

        private void cyclePlayer()
        {
            if (m_selectedPlayer < m_pc.Length - 1)
            {
                m_selectedPlayer++;
            }
            else
            {
                m_selectedPlayer = 0;
            }
            Console.WriteLine("Selected player {0}", m_selectedPlayer);
        }

        public void Run()
        {
            m_video = Video.SetVideoMode(m_width * m_scale, m_height * m_scale, 32, false, false, false, true);

            loadTiles();
            loadSprites();

            Events.Tick += new EventHandler<TickEventArgs>(ApplicationTickEventHandler);
            Events.KeyboardDown += new EventHandler<KeyboardEventArgs>(KeyboardEventHandler);
            Events.KeyboardUp += new EventHandler<KeyboardEventArgs>(KeyboardEventHandler);
            Events.Quit += new EventHandler<QuitEventArgs>(ApplicationQuitEventHandler);
            Events.Run();
        }

        private void ApplicationTickEventHandler(object sender, TickEventArgs args)
        {
            movePlayer(args.SecondsElapsed);

            blitTiles();

            blitSprites();

            m_video.Update();
        }

        private void KeyboardEventHandler(object sender, KeyboardEventArgs args)
        {
            Console.WriteLine("{0} {1}", args.Key, args.Down);
            m_pc[m_selectedPlayer].Direct(args.Key, args.Down);
            if (args.Key == Key.Tab && args.Down)
            {
                cyclePlayer();
            }
        }

        private void ApplicationQuitEventHandler(object sender, QuitEventArgs args)
        {
            Events.QuitApplication();
        }
    }
}
