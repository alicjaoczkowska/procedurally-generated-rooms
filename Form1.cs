using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Castle_project
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            RoomGenerator gen = new RoomGenerator();
            gen.GenerateRooms(10);
            gen.PrintSpaceArray();
            List<Room> roomList = gen.roomList;

            Brush towerBrush = new SolidBrush(Color.DimGray);
            Brush roomBrush = new SolidBrush(Color.LightBlue);
            Brush mainBrush = new SolidBrush(Color.Gray);
            Pen mainPen = new Pen(Color.Black);
            FontFamily family = new FontFamily(GenericFontFamilies.Serif);
            Font mainFont = new Font(family, 10f);
            int tileSize = 12;
            int xOffset = 3 * tileSize;
            int yOffset = 3 * tileSize;
            g.FillRectangle(towerBrush, xOffset, yOffset, 7 * tileSize, 7 * tileSize);
            g.FillRectangle(towerBrush, xOffset + 24 * tileSize, yOffset, 7 * tileSize, 7 * tileSize);
            g.FillRectangle(towerBrush, xOffset, yOffset + 24 * tileSize, 7 * tileSize, 7 * tileSize);
            g.FillRectangle(towerBrush, xOffset + 24 * tileSize, yOffset + 24 * tileSize, 7 * tileSize, 7 * tileSize);
            g.FillRectangle(mainBrush, xOffset + 3 * tileSize, yOffset + 3 * tileSize, 25 * tileSize, 25 * tileSize);

            foreach (Room room in roomList)
            {
                g.FillRectangle(roomBrush, xOffset + (room.coord[1] * tileSize), yOffset + (room.coord[0] * tileSize), (room.size[0]) * tileSize, (room.size[1]) * tileSize);
                g.DrawRectangle(mainPen, xOffset + (room.coord[1] * tileSize), yOffset + (room.coord[0] * tileSize), (room.size[0]) * tileSize, (room.size[1]) * tileSize);
                g.DrawString(room.GetRoomType(room.type), mainFont, towerBrush, xOffset + (room.coord[1] * tileSize) + ((room.size[0]) * tileSize / 5), yOffset + (room.coord[0] * tileSize) + ((room.size[1]) * tileSize / 2));
            }
        }
    }
}
