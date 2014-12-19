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

        private Dictionary<char, Surface> m_cells;

        private Surface m_npc;

        private Surface m_pc1;

        private Surface m_pc2;

        private Surface m_pc3;

        private Surface m_pc4;

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

        private void loadImages()
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

            m_npc = new Surface(m_path + "Images\\npc.png").Convert(m_video, true, true);
            m_npc.Transparent = true;
            m_npc.TransparentColor = Color.FromArgb(255, 0, 220);

            m_pc1 = new Surface(m_path + "Images\\pc1.png").Convert(m_video, true, true);
            m_pc1.Transparent = true;
            m_pc1.TransparentColor = Color.FromArgb(255, 0, 220);

            m_pc2 = new Surface(m_path + "Images\\pc2.png").Convert(m_video, true, true);
            m_pc2.Transparent = true;
            m_pc2.TransparentColor = Color.FromArgb(255, 0, 220);

            m_pc3 = new Surface(m_path + "Images\\pc3.png").Convert(m_video, true, true);
            m_pc3.Transparent = true;
            m_pc3.TransparentColor = Color.FromArgb(255, 0, 220);

            m_pc4 = new Surface(m_path + "Images\\pc4.png").Convert(m_video, true, true);
            m_pc4.Transparent = true;
            m_pc4.TransparentColor = Color.FromArgb(255, 0, 220);
        }

        public void Run()
        {
            m_video = Video.SetVideoMode(m_width * 8, m_height * 8, 32, false, false, false, true);

            loadImages();

            Events.Tick += new EventHandler<TickEventArgs>(ApplicationTickEventHandler);
            Events.KeyboardDown += new EventHandler<KeyboardEventArgs>(KeyboardEventHandler);
            Events.KeyboardUp += new EventHandler<KeyboardEventArgs>(KeyboardEventHandler);
            Events.Quit += new EventHandler<QuitEventArgs>(ApplicationQuitEventHandler);
            Events.Run();
        }

        private void ApplicationTickEventHandler(object sender, TickEventArgs args)
        {
            for (int y = 0; y < m_height; y++)
            {
                for (int x = 0; x < m_width; x++)
                {
                    if (m_cells.ContainsKey(m_grid[x, y]))
                    {
                        m_video.Blit(m_cells[m_grid[x, y]], new Point(x*8, y*8));
                    }
                }
            }

            m_video.Blit(m_npc, new Point(13*8, (22*8)+4));

            m_video.Blit(m_pc1, new Point(11 * 8, (12 * 8) + 4));

            m_video.Blit(m_pc2, new Point(15 * 8, (12 * 8) + 4));

            m_video.Blit(m_pc3, new Point(11 * 8, (14 * 8) + 4));

            m_video.Blit(m_pc4, new Point(15 * 8, (14 * 8) + 4));

            m_video.Update();
        }

        private void KeyboardEventHandler(object sender, KeyboardEventArgs args)
        {
            Console.WriteLine("{0} {1}", args.Key, args.Down);
            switch (args.Key)
            {
                case Key.DownArrow:
                    break;
                case Key.UpArrow:
                    break;
                case Key.LeftArrow:
                    break;
                case Key.RightArrow:
                    break;
                case Key.Tab:
                    break;
            }
        }

        private void ApplicationQuitEventHandler(object sender, QuitEventArgs args)
        {
            Events.QuitApplication();
        }
    }
}
