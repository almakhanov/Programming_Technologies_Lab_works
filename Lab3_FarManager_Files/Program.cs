﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace Lab3_FarManager_Files
{
    class Program
    {
        public static bool IsFileOpened = false;

        public static void ShowInfo(State state)
        {
            Console.Clear();
            FileSystemInfo[] infos = state.Dir.GetFileSystemInfos();
            for (int i = 0; i < infos.Length; i++)
            {                
                if (i == state.Index)
                {
                    if (infos[i].GetType() == typeof(DirectoryInfo))
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                    }
                    if (infos[i].GetType() == typeof(FileInfo))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                    }
                }
                else Console.ForegroundColor = ConsoleColor.White;

                Console.WriteLine(infos[i].Name);
            }
        }

        static void Main(string[] args)
        {
            State state = new State
            {
                Dir = new DirectoryInfo(@"D:\"),
                Index = 0
            };


            Stack<State> layers = new Stack<State>();
            bool alive = true;
            layers.Push(state);

            #region keyBoard
            while (alive)
            {
                if(IsFileOpened == false)
                ShowInfo(layers.Peek());

                ConsoleKeyInfo pressedKey = Console.ReadKey();
                switch (pressedKey.Key)
                {
                    case ConsoleKey.UpArrow:
                        layers.Peek().Index--;
                        break;
                    case ConsoleKey.DownArrow:
                        layers.Peek().Index++;
                        break;
                    case ConsoleKey.Backspace:
                        if (IsFileOpened)
                        {
                            Console.Clear();
                            IsFileOpened = false;
                        }
                        else
                        layers.Pop();
                        break;
                    case ConsoleKey.Escape:
                        alive = false;
                        break;
                    case ConsoleKey.Enter:

                        FileSystemInfo fs = layers.Peek().Dir.GetFileSystemInfos()[layers.Peek().Index];
                        if (fs.GetType() == typeof(DirectoryInfo))
                        {
                            State substate = new State
                            {
                                Dir = new DirectoryInfo(fs.FullName),
                                Index = 0
                            };
                            layers.Push(substate);
                        }
                        else
                        {
                            IsFileOpened = true;

                            string file = layers.Peek().Dir.GetFileSystemInfos()[layers.Peek().Index].FullName;
                            FileStream fls = new FileStream(file, FileMode.Open, FileAccess.Read);
                            {
                                string line;
                                StreamReader sr = new StreamReader(fls);
                                Console.Clear();
                                while ((line = sr.ReadLine()) != null)
                                {
                                    Console.WriteLine(line);
                                }

                                sr.Close();
                            }
                            
                            fls.Close();
                            //Process.Start(fs.FullName);
                        }

                        break;
                    
                    default:
                        break;
                }
            }//end while
            #endregion

        }
    }
}
