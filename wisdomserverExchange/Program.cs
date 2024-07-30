﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace wisdomserverExchange
{
    internal static class Program
    {
        static void Main()
        {
            ////#if DEBUG
            ////Service1 myService = new Service1();
            ////myService.OnDebug();
            ////System.Threading.Thread.Sleep(System.Threading.Timeout.Infinite);
            ////#else
            //ServiceBase[] ServicesToRun;
            //ServicesToRun = new ServiceBase[]
            //{
            //    new Service1()
            //};
            //ServiceBase.Run(ServicesToRun);
            ////#endif



#if DEBUG
            Service1 myService = new Service1();
            myService.OnDebug();
            System.Threading.Thread.Sleep(System.Threading.Timeout.Infinite);
#else
                        ServiceBase[] ServicesToRun;
                        ServicesToRun = new ServiceBase[]
                        {
                        new Service1()
                        };
                        ServiceBase.Run(ServicesToRun);
#endif
        }

    }
}
