using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Castle_project
{

    internal static class Program
    {
        /// <summary>
        /// Główny punkt wejścia dla aplikacji.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            /*RoomGenerator gen = new RoomGenerator();
            gen.GenerateRooms(7);
            gen.PrintSpaceArray();*/
            Application.Run(new Form1());
        }
    }

    internal class RoomGenerator
    {
        static public int[,] spaceArray = new int[31, 31];
        public List<Room> roomList;

        private void GenerateSpace()
        {
            for (int j = 7; j < 24; j++)
            {
                for (int i = 0; i < 3; i++)
                {
                    spaceArray[i, j] = 1;
                }

                for (int i = 28; i < 31; i++)
                {
                    spaceArray[i, j] = 1;
                }
            }

            for (int i = 7; i < 24; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    spaceArray[i, j] = 1;
                }

                for (int j = 28; j < 31; j++)
                {
                    spaceArray[i, j] = 1;
                }
            }

        }

        private Boolean RoomsHasSpaceToGrow()
        {
            int sizex;
            int sizey;
            int coordx;
            int coordy;
            int counter = 0;

            foreach (Room room in roomList)
            {
                sizex = room.size[0];
                sizey = room.size[1];
                coordx = room.coord[0];
                coordy = room.coord[1];

                int j1 = coordy - 1;
                int j2 = coordy + sizex;

                for (int i = coordx; i < coordx + sizey; i++)
                {
                    if (j1 >= 3 && spaceArray[i, j1] == 0)
                    {
                        counter++;
                    }
                }

                if (counter == sizey)
                {
                    return true;
                }

                counter = 0;
                for (int i = coordx; i < coordx + sizey; i++)
                {
                    if (j2 < 29 && spaceArray[i, j2] == 0)
                    {
                        counter++;
                    }
                }

                if (counter == sizey)
                {
                    return true;
                }

                int i1 = coordx - 1;
                int i2 = coordx + sizey;
                counter = 0;
                for (int j = coordy; j < coordy + sizex; j++)
                {
                    if (i1 >= 3 && spaceArray[i1, j] == 0)
                    {
                        counter++;
                    }
                }

                if (counter == sizex)
                {
                    return true;
                }

                counter = 0;
                for (int j = coordy; j < coordy + sizex; j++)
                {
                    if (i2 < 29 && spaceArray[i2, j] == 0)
                    {
                        counter++;
                    }
                }
                if (counter == sizex)
                {
                    return true;
                }
            }

            return false;
        }

        public void PrintSpaceArray()
        {
            for (int i = 0; i < 31; i++)
            {
                for (int j = 0; j < 31; j++)
                {
                    if (spaceArray[i, j] == 0)
                    {
                        Console.Write("[]");
                    }
                    else if (spaceArray[i, j] == 1)
                    {
                        Console.Write("__");
                    }
                    else if (spaceArray[i, j] == 3)
                    {
                        Console.Write("**");
                    }
                    Console.Write(" ");
                }
                Console.WriteLine();
            }
        }

        Random rand;

        public void GenerateRooms(int numberOfRooms)
        {
            GenerateSpace();
            rand = new Random();
            GetRandomRoomsList(numberOfRooms);
            Boolean ListInorrect = true;

            while (ListInorrect)
            {
                if (CheckRoomList())
                {
                    GetRandomRoomsList(numberOfRooms);
                    continue;
                }

                for (int i = 0; i < roomList.Count; i++)
                {
                    if (CheckForCoord(i))
                    {
                        GetRandomRoomsList(numberOfRooms);
                        break;
                    }
                    else
                    {
                        ListInorrect = false;
                    }
                }
            }

            AddRoomsToSpaceArray();
            int whilecounter = 0;

            while (RoomsHasSpaceToGrow())
            {
                foreach (Room room in roomList)
                {
                    int sizex = room.size[0];
                    int sizey = room.size[1];
                    int coordx = room.coord[0];
                    int coordy = room.coord[1];

                    int j1 = coordy - 1;
                    int counter = 0;

                    for (int i = coordx; i < coordx + sizey; i++)
                    {
                        if (j1 >= 3 && spaceArray[i, j1] == 0)
                        {
                            counter++;
                        }
                    }

                    if (counter == sizey)
                    {
                        GrowLeft(room);
                    }

                    sizex = room.size[0];
                    sizey = room.size[1];
                    coordx = room.coord[0];
                    coordy = room.coord[1];

                    int j2 = coordy + sizex;
                    counter = 0;
                    for (int i = coordx; i < coordx + sizey; i++)
                    {
                        if (j2 < 29 && spaceArray[i, j2] == 0)
                        {
                            counter++;
                        }
                    }

                    if (counter == sizey)
                    {
                        GrowRight(room);
                    }

                    sizex = room.size[0];
                    sizey = room.size[1];
                    coordx = room.coord[0];
                    coordy = room.coord[1];

                    int i1 = coordx - 1;
                    counter = 0;
                    for (int j = coordy; j < coordy + sizex; j++)
                    {
                        if (i1 >= 3 && spaceArray[i1, j] == 0)
                        {
                            counter++;
                        }
                    }

                    if (counter == sizex)
                    {
                        GrowTop(room);
                    }

                    sizex = room.size[0];
                    sizey = room.size[1];
                    coordx = room.coord[0];
                    coordy = room.coord[1];

                    int i2 = coordx + sizey;
                    counter = 0;
                    for (int j = coordy; j < coordy + sizex; j++)
                    {
                        if (i2 < 29 && spaceArray[i2, j] == 0)
                        {
                            counter++;
                        }
                    }

                    if (counter == sizex)
                    {
                        GrowBot(room);
                    }
                }
                whilecounter++;
                if(whilecounter > 100)
                {
                    break;
                }
            }
        }

        private int GetRandomRoomType()
        {
            return rand.Next(0, 7);
        }

        private int GetRandomCoord()
        {
            return rand.Next(3, 28);
        }

        private void GetRandomRoomsList(int numberOfRooms)
        {
            roomList = new List<Room>();
            for (int i = 0; i < numberOfRooms; i++)
            {
                Room room = new Room(GetRandomRoomType(), GetRandomCoord(), GetRandomCoord());
                roomList.Add(room);
            }
        }

        private Boolean CheckRoomList()
        {
            int[] checkRooms = new int[8];
            for (int i = 0; i < roomList.Count; i++)
            {
                int roomType = roomList.ElementAt(i).type;
                checkRooms[roomType]++;
            }

            if (checkRooms[0] != 1)
            {
                return true;
            }
            else if (checkRooms[1] > 2)
            {
                return true;
            }
            else if (checkRooms[2] > 1)
            {
                return true;
            }
            else if (checkRooms[3] > 1)
            {
                return true;
            }
            else if (checkRooms[2] == 1 && checkRooms[3] == 1)
            {
                return true;
            }
            else if (checkRooms[4] > 4)
            {
                return true;
            }
            else if (checkRooms[5] > 2)
            {
                return true;
            }
            else if (checkRooms[6] > 2)
            {
                return true;
            }
            /*else if (checkRooms[7] > 1)
            {
                return true;
            }*/

            return false;
        }

        private Boolean CheckForCoord(int i)
        {
            if(spaceArray[roomList.ElementAt(i).coord[0], roomList.ElementAt(i).coord[1]] != 1)
            {
                for (int j = i - 1; j >= 0; j--)
                {
                    if (roomList.ElementAt(i).coord[0].Equals(roomList.ElementAt(j).coord[0]))
                    {
                        if (roomList.ElementAt(i).coord[1].Equals(roomList.ElementAt(j).coord[1]))
                        {
                            return true;
                        }
                    }
                }

                for (int j = i + 1; j < roomList.Count; j++)
                {
                    if (roomList.ElementAt(i).coord[0].Equals(roomList.ElementAt(j).coord[0]))
                    {
                        if (roomList.ElementAt(i).coord[1].Equals(roomList.ElementAt(j).coord[1]))
                        {
                            return true;
                        }
                    }
                }
            }
            

            return false;
        }

        private void GrowLeft(Room room)
        {
            int sizey = room.size[1];
            int coordx = room.coord[0];
            int coordy = room.coord[1];

            int j1 = coordy - 1;

            for (int i = coordx; i < coordx + sizey; i++)
            {
                spaceArray[i, j1] = 1;
            }

            room.size[0] += 1;
            room.coord[1] -= 1;
        }

        private void GrowRight(Room room)
        {
            int sizex = room.size[0];
            int sizey = room.size[1];
            int coordx = room.coord[0];
            int coordy = room.coord[1];

            int j2 = coordy + sizex;

            for (int i = coordx; i < coordx + sizey; i++)
            {
                spaceArray[i, j2] = 1;
            }

            room.size[0] += 1;
        }

        private void GrowTop(Room room)
        {
            int sizex = room.size[0];
            int sizey = room.size[1];
            int coordx = room.coord[0];
            int coordy = room.coord[1];

            int i1 = coordx - 1;
            for (int j = coordy; j < coordy + sizex; j++)
            {
                spaceArray[i1, j] = 1;
            }

            room.size[1] += 1;
            room.coord[0] -= 1;
        }

        private void GrowBot(Room room)
        {
            int sizex = room.size[0];
            int sizey = room.size[1];
            int coordx = room.coord[0];
            int coordy = room.coord[1];

            int i2 = coordx + sizey;
            for (int j = coordy; j < coordy + sizex; j++)
            {
                spaceArray[i2, j] = 1;
            }

            room.size[1] += 1;
        }

        private void AddRoomsToSpaceArray()
        {
            for (int i = 0; i < roomList.Count; i++)
            {
                int coordx = roomList.ElementAt(i).coord[0];
                int coordy = roomList.ElementAt(i).coord[1];
                spaceArray[coordx, coordy] = 1;
            }
        }
    }

    public class Room
    {
        public int type;
        public int[] size = new int[2];
        public int[] coord = new int[2];
        //public int ratio

        public Dictionary<int, string> roomNames = new Dictionary<int, string>()
        {
            {0, "Sala tronowa"},
            {1, "Biblioteka"},
            {2, "Sala główna"},
            {3, "Dziedziniec"},
            {4, "Kwatery mieszkalne"},
            {5, "Jadalnia"},
            {6, "Kuchnia"},
            {7, "Sala wejściowa"},
        };

        public Room(int roomType, int coordx, int coordy)
        {
            type = roomType;
            size[0] = 1;
            size[1] = 1;
            coord[0] = coordx;
            coord[1] = coordy;

            if (roomType == 2 || roomType == 3)
            {
                coord[0] = 16;
                coord[1] = 16;
            }
        }

        public String GetRoomType(int roomType)
        {
            String type = roomNames[roomType];
            return type;
        }
    }
}