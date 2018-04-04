using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Timers;

namespace ConsoleApp1
{
    public struct POINT
    {
        public int X;
        public int Y;
    }

    class Program
    {
        [DllImport("user32.dll")]
        static extern bool SetCursorPos(int X, int Y);
        [DllImport("user32.dll")]
        public static extern bool GetCursorPos(out POINT lpPoint);
        [DllImport("user32.dll")]
        public static extern bool GetAsyncKeyState(int nVirtKey);

        public static bool jiggleToggle = false;
        public static bool jiggleLeft = true;

        static void Main(string[] args)
        {
            System.Timers.Timer jiggle = new System.Timers.Timer();
            jiggle.Elapsed += new ElapsedEventHandler(JiggleMouse);
            jiggle.Interval = 1000;
            jiggle.Start();

            Thread toggleThread = new Thread(toggleJiggle);
            toggleThread.Start();

            Console.WriteLine("Press the C key to toggle, jiggle is off by default");
            Console.ReadLine();
        }

        public static void toggleJiggle()
        {
            while (true)
            {
                Thread.Sleep(100);
                if (GetAsyncKeyState(0x43))
                {
                    jiggleToggle = !jiggleToggle;
                    Console.WriteLine($"Jiggle: {jiggleToggle}");
                }
            }
        }

        public static void JiggleMouse(object source, ElapsedEventArgs e)
        {
            if (jiggleToggle)
            {
                POINT mousePos;
                GetCursorPos(out mousePos);

                Console.WriteLine($"Mouse Position: {mousePos.X}:{mousePos.Y}");

                if (jiggleLeft)
                {
                    Console.WriteLine("Jiggling Left");
                    for (int i=0; i<50; i++)
                    {
                        Thread.Sleep(2);
                        SetCursorPos(mousePos.X - i, mousePos.Y);
                    }
                    GetCursorPos(out mousePos);
                    Console.WriteLine($"New Mouse Position: {mousePos.X}:{mousePos.Y}");
                    jiggleLeft = false;
                }
                else
                {
                    Console.WriteLine("Jiggling Right");
                    for (int i = 0; i < 50; i++)
                    {
                        Thread.Sleep(2);
                        SetCursorPos(mousePos.X + i, mousePos.Y);
                    }
                    GetCursorPos(out mousePos);
                    Console.WriteLine($"New Mouse Position: {mousePos.X}:{mousePos.Y}");
                    jiggleLeft = true;
                }
            }
        }
    }
}
