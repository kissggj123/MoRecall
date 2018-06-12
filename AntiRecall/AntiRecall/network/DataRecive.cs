﻿using System;
using System.Collections.Generic;
using System.Linq;
using socks5;
using socks5.TCP;
using System.Windows;
using System.Windows.Controls;
using System.Threading;
using System.Net;
using System.Windows.Forms;

namespace AntiRecall.network
{
    class DataRecive : socks5.Plugin.DataHandler
    {
        public override bool OnStart()
        {
            return true;
        }

        private bool enabled = true;
        public override bool Enabled
        {
            get
            {
                return enabled;
            }
            set
            {
                enabled = value;
            }
        }


        public override void OnClientDataReceived(object sender, DataEventArgs e)
        {

            return;
        }

        public override void OnServerDataReceived(object sender, DataEventArgs e)
        {
            
            if (e.Buffer[6] == 0x17)
                if (e.Count == 137 || e.Count == 121)
                {
                    e.Buffer[6] = 0x00;

                    MainWindow.count++;
                /*
                    if ((MainWindow.count + 7) % 8 ==0)
                    {
                        App.Current.Dispatcher.Invoke(
                            (Action)delegate {
                                ((MainWindow)System.Windows.Application.Current.MainWindow).ModifyRecallCount();
                            }
                            );
                        
                    }
                    */
                
#if DEBUG
                    Console.WriteLine("capture recall");
#endif
                }
       
            return;
            
        }

    }
}
