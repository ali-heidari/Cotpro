using System;
using System.Collections.Generic;
using System.Text;

namespace Cotpro.Thread
{
    public class ThreadBase
    {
        public void CheckAllIPs(int CountOfThreads, System.Threading.ParameterizedThreadStart Func, object parameter)
        {

            for (int i = 0; i < CountOfThreads; i++)
            {
                System.Threading.Thread t = new System.Threading.Thread(Func);
                t.IsBackground = true;
                t.Start(parameter);
            }

        }



        public string CreateRandomIP()
        {
            Random r = new Random();
            return r.Next(0, 257) + "." + r.Next(0, 257) + "." + r.Next(0, 257) + "." + r.Next(0, 257);
        }
    }
}
