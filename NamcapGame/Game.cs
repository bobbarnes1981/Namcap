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

        private Surface m_npc;

        private Point m_npcLocation;

        private int m_selectedPlayer = 0;

        private Surface[] m_pc;

        private Point[] m_pcLocation;

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

            m_npcLocation = new Point(13 * m_scale, (22 * m_scale) + 4);

            m_pcLocation = new Point[4];
            m_pcLocation[0] = new Point(11 * m_scale, (12 * m_scale) + 4);
            m_pcLocation[1] = new Point(15 * m_scale, (12 * m_scale) + 4);
            m_pcLocation[2] = new Point(11 * m_scale, (14 * m_scale) + 4);
            m_pcLocation[3] = new Point(15 * m_scale, (14 * m_scale) + 4);
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

            m_pc = new Surface[4];

            m_pc[0] = new Surface(m_path + "Images\\pc1.png").Convert(m_video, true, true);
            m_pc[0].Transparent = true;
            m_pc[0].TransparentColor = Color.FromArgb(255, 0, 220);

            m_pc[1] = new Surface(m_path + "Images\\pc2.png").Convert(m_video, true, true);
            m_pc[1].Transparent = true;
            m_pc[1].TransparentColor = Color.FromArgb(255, 0, 220);

            m_pc[2] = new Surface(m_path + "Images\\pc3.png").Convert(m_video, true, true);
            m_pc[2].Transparent = true;
            m_pc[2].TransparentColor = Color.FromArgb(255, 0, 220);

            m_pc[3] = new Surface(m_path + "Images\\pc4.png").Convert(m_video, true, true);
            m_pc[3].Transparent = true;
            m_pc[3].TransparentColor = Color.FromArgb(255, 0, 220);
        }

        private void movePlayer(int playerId, int x, int y)
        {
            
        }

        public void Run()
        {
            m_video = Video.SetVideoMode(m_width * m_scale, m_height * m_scale, 32, false, false, false, true);

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
                        m_video.Blit(m_cells[m_grid[x, y]], new Point(x * m_scale, y * m_scale));
                    }
                }
            }

            m_video.Blit(m_npc, m_npcLocation);

            for (int i = 0; i < 4; i++)
            {
                m_video.Blit(m_pc[i], m_pcLocation[i]);
            }

            m_video.Update();
        }

        private void KeyboardEventHandler(object sender, KeyboardEventArgs args)
        {
            Console.WriteLine("{0} {1}", args.Key, args.Down);
            switch (args.Key)
            {
                case Key.DownArrow:
                    movePlayer(m_selectedPlayer, 0, 1);
                    break;
                case Key.UpArrow:
                    movePlayer(m_selectedPlayer, 0, -1);
                    break;
                case Key.LeftArrow:
                    movePlayer(m_selectedPlayer, -1, 0);
                    break;
                case Key.RightArrow:
                    movePlayer(m_selectedPlayer, 1, 0);
                    break;
                case Key.Tab:
                    if (m_selectedPlayer == 4)
                    {
                        m_selectedPlayer = 0;
                    }
                    else
                    {
                        m_selectedPlayer++;
                    }
                    break;
            }
        }

        private void ApplicationQuitEventHandler(object sender, QuitEventArgs args)
        {
            Events.QuitApplication();
        }
    }
}
