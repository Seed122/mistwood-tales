using System;
using System.IO;

namespace MistwoodTales.Game.Client.Legacy
{
    class World
    {
        #region Singleton

        public static World Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lockObj)
                    {
                        if (_instance == null)
                        {
                            _instance = new World();
                        }
                    }
                }
                return _instance;
            }
        }

        private static World _instance;
        private static readonly object _lockObj = new object();

        #endregion

        private World()
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "map.txt");
            Map = new Map(path);
            Player = new Player()
            {
                MaxHP = 88,
                HP = 70,
                Name = "Gatmeat"
            };
        }

        public Map Map { get; private set; }
        public Player Player { get; set; }
    }
}
