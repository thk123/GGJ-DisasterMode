using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace GGJ_DisasterMode.Codebase.Dropoffs
{
    class BasicFoodDropoff : Dropoff
    {
        public BasicFoodDropoff(Rectangle storeSlot)
            : base(GetProperties(), storeSlot)
        {

        }

        private static DropoffProperties GetProperties()
        {
            DropoffProperties properties = new DropoffProperties();
            properties.delay = 1;
            properties.duration = 1;
            properties.type = DropoffType.Dropoff_Food;
            properties.useCount = 1;//?

            properties.shopTextureLoc = "Graphics//Dropoffs//Dropoff_FirstAid_Basic";
            properties.gridTextureLoc = "Graphics//Dropoffs//Dropoff_FirstAid_Basic";

            return properties;
        }
    }
}
