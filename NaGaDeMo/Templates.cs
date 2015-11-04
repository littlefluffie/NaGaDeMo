using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NaGaDeMo
{
    public static class Templates
    {

        public static class Maps
        {
            public static Map DefaultMap()
            {
                Map map = new Map();

                map.TextureName = "terrain.png";
                map.MapFile = "Default.txt";

                map.GenerateTiles();
                
                return map;
            }

        }

    }
}
