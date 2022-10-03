using System;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

// A debug server that is hosted along the server, this will send statistical information
// If management required, then we would need a standalone system to launch, restart, etc
namespace Game.Server.DebugServer
{
    public class DebugWebserver
    {
        private const string url = "http://localhost:8888";
        
        //public MasterSettings _settings;

        private static HttpListener httpListener;

        private static SemaphoreSlim semaphore;

        private static string[] data;
        private static string webpageData;

        public static async Task HandleHTTPIncoming()
        {
            bool runServer = true;
            while (runServer)
            {
                var ctx = await httpListener.GetContextAsync();

                var rtx = ctx.Request;
                var resp = ctx.Response;

                byte[] returnData = new byte[] {0};

                await semaphore.WaitAsync();
                string webpageFinalized = "FAILED";
                
                try
                {
                    webpageFinalized = webpageData;
                 
                    /* Disabled incase format is better than replace   
                    for (int i = 0; i < data.Length; i++)
                    {
                        webpageFinalized = webpageFinalized.Replace("{" + i + "}", data[i]);
                    }*/
                    
                    returnData = Encoding.UTF8.GetBytes(String.Format(webpageFinalized, webpageData));
                }
                finally
                {
                    semaphore.Release();
                }

                
                await resp.OutputStream.WriteAsync(returnData, 0, returnData.Length);
                resp.Close();
            }
        }
        
        public static void InitializeServer()
        {
            if (httpListener != null) return;
            
            httpListener = new HttpListener();
            httpListener.Prefixes.Add(url);
            httpListener.Start();
            Debug.Log("HTTP Debug server started!");


            
            // Launch this on a different thread, otherwise this will clog mainthread
            //Task httpListenTask = HandleHTTPIncoming();
            //httpListenTask.GetAwaiter().GetResult();
            //httpListener.Close();
            
        }

        public static async Task InsertData(string[] pageData)
        {
            
            await semaphore.WaitAsync();
            try
            {
                data = pageData;
            }
            finally
            {
                semaphore.Release();
            }
            
        }

        public static async Task InsertData(string pageData)
        {
            
            await semaphore.WaitAsync();
            try
            {
                webpageData = pageData;
            }
            finally
            {
                semaphore.Release();
            }
            
        }

        public static void SetWebpageString(string data)
        {
            // Insert data without blocking main thread, coroutine? dont know
            Task.Run(() => InsertData(data));
        }
        
        public static void SetData(string[] data)
        {
            // Insert data without blocking main thread, coroutine? dont know
            
            // Got it, create thread store data in thread, and have it wait and set the data
            // Queue of threads
            Task.Run(() => InsertData(data));
        }
    }
}