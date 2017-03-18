using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using System.IO.Ports;

namespace MatLabClient
{
    class Program
    {
        static void Main(string[] args)
        {
            //Matrix<double> A = DenseMatrix.OfArray(new double[,] { { 1, 1, 1, 1 }, { 2, 3, 1, 5 }, { 4, 5, 2, 1 } });
            string[] portNames = SerialPort.GetPortNames();
            SerialPort port = new SerialPort("COM3");
            port.BaudRate = 9600;
            port.Parity = Parity.None;
            port.StopBits = StopBits.One;
            port.Handshake = Handshake.None;
            port.DataBits = 8;
            port.RtsEnable = true;

            port.DtrEnable = true;

            port.Open();
            port.Write("Some text");
            Console.WriteLine("Waiting for data...");
            Console.WriteLine();
            Console.ReadKey();
            port.Close();
        }
    }
}
