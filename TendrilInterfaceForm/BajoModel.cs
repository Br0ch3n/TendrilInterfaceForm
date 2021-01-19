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
        private Matrix4 TensonLoads;
        private float Theta;
        private float YoungsModulus;
        private float MomentInertia;


        private TendrilStateSingleton TendrilState;

        // initialization

        public BajoModel()
        {
            ContactThreshold = 1;

            TendrilState = TendrilStateSingleton.getInstance();
        }

        public void ContactDetection(float s, float k, float phi, float r, float m, Matrix3 transform)
        {
            // 
                
            Theta = s * k;

            for (int i = 0; i < 3; i++)
            {
                
            }

        }

        private void SetTensionLoads()
        {

        }



    }
}
