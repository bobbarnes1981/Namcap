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

        private string m_filesPath = @"..\..\..\Files\";

        private string m_imagesFolder = @"Images";

        private string m_levelsFolder = @"Levels";

        private char[,] m_grid;

        private int m_width = 28;

        private int m_height = 31;

        private int m_scale = 8;

        private Dictionary<char, Surface> m_tiles;

        private Sprite m_npc;

        private int m_selectedPlayer = 0;

        private Surface m_pcEyes;

        private Sprite[] m_pc;

        public Game()
        {
            // 4 ghosts - tab to switch, arrow keys to move
            // 1 pacman - random moves, away from ghosts

            // board 28w 31h
            m_grid = new char[m_width, m_height];
            string[] data = File.ReadAllLines(Path.Combine(m_filesPath, m_levelsFolder, "level01.map"));

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
            m_tiles = new Dictionary<char, Surface>();

            m_tiles.Add('[', new Surface(m_filesPath + m_imagesFolder + "\\vlinel.png").Convert(m_video, true, false));
            m_tiles.Add(']', new Surface(m_filesPath + m_imagesFolder + "\\vliner.png").Convert(m_video, true, false));
            m_tiles.Add('-', new Surface(m_filesPath + m_imagesFolder + "\\hlinet.png").Convert(m_video, true, false));
            m_tiles.Add('_', new Surface(m_filesPath + m_imagesFolder + "\\hlineb.png").Convert(m_video, true, false));
            m_tiles.Add(' ', new Surface(m_filesPath + m_imagesFolder + "\\empty.png").Convert(m_video, true, false));
            m_tiles.Add('@', new Surface(m_filesPath + m_imagesFolder + "\\pill.png").Convert(m_video, true, false));
            m_tiles.Add('.', new Surface(m_filesPath + m_imagesFolder + "\\pip.png").Convert(m_video, true, false));
            m_tiles.Add('/', new Surface(m_filesPath + m_imagesFolder + "\\ctl.png").Convert(m_video, true, false));
            m_tiles.Add('\\', new Surface(m_filesPath + m_imagesFolder + "\\ctr.png").Convert(m_video, true, false));
            m_tiles.Add('<', new Surface(m_filesPath + m_imagesFolder + "\\cbl.png").Convert(m_video, true, false));
            m_tiles.Add('>', new Surface(m_filesPath + m_imagesFolder + "\\cbr.png").Convert(m_video, true, false));
            m_tiles.Add('+', new Surface(m_filesPath + m_imagesFolder + "\\hdoorb.png").Convert(m_video, true, false));
        }

        private void loadSprites()
        {
            m_npc = new Sprite(
                new Surface(m_filesPath + m_imagesFolder + "\\npc.png").Convert(m_video, true, true),
                new Point(13 * m_scale, (22 * m_scale) + 4),
                40);
            m_npc.Image.Transparent = true;
            m_npc.Image.TransparentColor = Color.FromArgb(255, 0, 220);

            m_pcEyes = new Surface(m_filesPath + m_imagesFolder + "\\pceyes.png").Convert(m_video, true, true);
            m_pcEyes.Transparent = true;
            m_pcEyes.TransparentColor = Color.FromArgb(255, 0, 220);

            m_pc = new Sprite[4];

            m_pc[0] = new Sprite(
                new Surface(m_filesPath + m_imagesFolder + "\\pc1.png").Convert(m_video, true, true),
                new PointF(11 * m_scale, (12 * m_scale) + 4),
                30);
            m_pc[0].Image.Transparent = true;
            m_pc[0].Image.TransparentColor = Color.FromArgb(255, 0, 220);

            m_pc[1] = new Sprite(
                new Surface(m_filesPath + m_imagesFolder + "\\pc2.png").Convert(m_video, true, true),
                new PointF(15 * m_scale, (12 * m_scale) + 4),
                30);
            m_pc[1].Image.Transparent = true;
            m_pc[1].Image.TransparentColor = Color.FromArgb(255, 0, 220);

            m_pc[2] = new Sprite(
                new Surface(m_filesPath + m_imagesFolder + "\\pc3.png").Convert(m_video, true, true),
                new PointF(11 * m_scale, (14 * m_scale) + 4),
                30);
            m_pc[2].Image.Transparent = true;
            m_pc[2].Image.TransparentColor = Color.FromArgb(255, 0, 220);

            m_pc[3] = new Sprite(
                new Surface(m_filesPath + m_imagesFolder + "\\pc4.png").Convert(m_video, true, true),
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
                    if (m_tiles.ContainsKey(m_grid[x, y]))
                    {
                        m_video.Blit(m_tiles[m_grid[x, y]], new Point(x * m_scale, y * m_scale));
                    }
                }
            }
        }

        private void blitSprites()
        {
            blitSprite(m_npc);

            for (int i = 0; i < 4; i++)
            {
                blitSprite(m_pc[i]);
            }
        }

        private void blitSprite(Sprite sprite)
        {
            if (sprite.X > m_video.Width - sprite.Image.Width)
            {
                m_video.Blit(sprite.Image, new Point((int)sprite.X - m_video.Width, (int)sprite.Y));
            }
            if (sprite.Y > m_video.Height - sprite.Image.Height)
            {
                m_video.Blit(sprite.Image, new Point((int)sprite.X, (int)sprite.Y - m_video.Height));
            }
            m_video.Blit(sprite.Image, new Point((int)sprite.X, (int)sprite.Y));
        }

        private void movePlayer(float elapsed)
        {
            foreach (Sprite player in m_pc)
            {
                collideSprite(player, elapsed);

                wrapSprite(player);
            }
        }

        private void collideSprite(Sprite sprite, float elapsed)
        {
            char item;
            int x;
            int y;

            sprite.MoveX(elapsed);

            y = (int)(sprite.Y + 8);

            x = (int)(sprite.X + 4);
            item = m_grid[(x / 8) % m_width, (y / 8) % m_height];
            if (item != '.' && item != ' ' && item != '+' && item != '@')
            {
                sprite.X = (((x / 8) + 1) * 8) - 4;
                //sprite.ReverseX(elapsed);
            }
            x = (int)(sprite.X + 12);
            item = m_grid[(x / 8) % m_width, (y / 8) % m_height];
            if (item != '.' && item != ' ' && item != '+' && item != '@')
            {
                sprite.X = (((x / 8) - 1) * 8) - 4;
                //sprite.ReverseX(elapsed);
            }

            sprite.MoveY(elapsed);

            x = (int)(sprite.X + 8);

            y = (int)(sprite.Y + 4);
            item = m_grid[(x / 8) % m_width, (y / 8) % m_height];
            if (item != '.' && item != ' ' && item != '+' && item != '@')
            {
                sprite.Y = (((y / 8) + 1) * 8) - 4;
                //sprite.ReverseY(elapsed);
            }
            y = (int)(sprite.Y + 12);
            item = m_grid[(x / 8) % m_width, (y / 8) % m_height];
            if (item != '.' && item != ' ' && item != '+' && item != '@')
            {
                sprite.Y = (((y / 8) - 1) * 8) - 4;
                //sprite.ReverseY(elapsed);
            }
        }

        private void wrapSprite(Sprite sprite)
        {
            if (sprite.X > m_video.Width)
            {
                sprite.X = 0;
            }
            if (sprite.X < 0)
            {
                sprite.X = m_video.Width;
            }
            if (sprite.Y > m_video.Height)
            {
                sprite.Y = 0;
            }
            if (sprite.Y < 0)
            {
                sprite.Y = m_video.Height;
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
