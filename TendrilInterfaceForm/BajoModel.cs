﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenGL;

namespace TendrilInterfaceForm
{
    class BajoModel
    {
        // List of variables

        private float ContactThreshold;
        private Matrix4 Jacobian;

        // initialization

        public BajoModel()
        {
            ContactThreshold = 10;
            //Jacobian = TendrilUtils
        }

        public void ContactDetection()
        {

        }



    }
}
