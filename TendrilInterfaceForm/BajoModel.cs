using System;
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


        private TendrilStateSingleton TendrilState;

        // initialization

        public BajoModel()
        {
            ContactThreshold = 1;

            TendrilState = TendrilStateSingleton.getInstance();
        }

        public void ContactDetection()
        {
            // 
        }



    }
}
