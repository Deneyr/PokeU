﻿using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeU.Model
{
    public interface ILandObject: IObject
    {
        Vector2i Position
        {
            get;
        }

        int Altitude
        {
            get;
            set;
        }

        LandTransition LandTransition
        {
            get;
        }

        ILandObject Clone(LandTransition wallLandTransition);

        ILandObject Clone();
    }
}
