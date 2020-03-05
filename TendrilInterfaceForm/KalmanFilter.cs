using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics;

namespace TendrilInterfaceForm
{
    class KalmanFilter
    {
        private float stateError, measError;
        Matrix<float> Phi, Stt, Stt1, Q, Kt, Xtt1, Xtt, M;
        float R, T, Yt;

        public KalmanFilter(float sError, float mError)
        {
            float[] x = new float[] { 1, T, 0, 1 };
            stateError = sError;
            measError = mError;

            T = 1;

            Xtt1 = Matrix<float>.Build.Dense(2, 1);
            Xtt = Matrix<float>.Build.Dense(2, 1);
            Phi = Matrix<float>.Build.DenseOfRowMajor(2, 2, x);
            Stt = Matrix<float>.Build.DenseIdentity(2);
            Stt1 = Matrix<float>.Build.DenseIdentity(2);
            x = new float[] { 0, 0, 0, (float)Math.Pow((double)stateError, 2) };
            Q = Matrix<float>.Build.DenseOfRowMajor(2, 2, x);
            R = (float)Math.Pow((double)measError, 2);
            x = new float[] { 1, 0 };
            M = Matrix<float>.Build.DenseOfRowMajor(2, 1, x);
            Kt = Matrix<float>.Build.Dense(2, 2);
            
        }

        public void Predict()
        {
            Xtt1 = Phi.Multiply(Xtt);
        }

        public void Update(float meas)
        {
            Stt1 = Phi.Multiply(Stt.Multiply(Phi.Transpose() + Q));

            Yt = meas;

            Kt = Stt1.Multiply(M.Transpose()).Multiply(M.Multiply(Stt1.Multiply(M.Transpose() + R)).Inverse());

            Xtt = Xtt1 + Kt.Multiply(Yt - M.Multiply(Xtt1));

            Stt = (Matrix<float>.Build.DenseIdentity(2) - Kt.Multiply(M)).Multiply(Stt1);
        }

        public float GetOutput()
        {
            return Xtt[0, 0];
        }

    }
}
