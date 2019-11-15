#region License
/*
* Copyright (c) Lightstreamer Srl
*
* Licensed under the Apache License, Version 2.0 (the "License");
* you may not use this file except in compliance with the License.
* You may obtain a copy of the License at
*
* http://www.apache.org/licenses/LICENSE-2.0
*
* Unless required by applicable law or agreed to in writing, software
* distributed under the License is distributed on an "AS IS" BASIS,
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
* See the License for the specific language governing permissions and
* limitations under the License.
*/
#endregion License

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;

using Lightstreamer.Interfaces.Data;

public class HelloWorldAdapter : IDataProvider
{
    private IItemEventListener _listener;
    private volatile bool go;

    public void Init(IDictionary parameters, string configFile)
    {
    }

    public bool IsSnapshotAvailable(string itemName)
    {
        return false;
    }

    public void SetListener(IItemEventListener eventListener)
    {
        _listener = eventListener;
    }

    public void Subscribe(string itemName)
    {
        if (itemName.Equals("greetings"))
        {
            Thread t = new Thread(new ThreadStart(Run));
            t.Start();
        }
    }

    public void Unsubscribe(string itemName)
    {
        if (itemName.Equals("greetings"))
        {
            go = false;
        }
    }

    public void Run()
    {
        go = true;
        Random rand = new Random();

        while (go)
        {
            LoadTempratureData(out List<string> lstVibX, out List<string> lstVibY,
                out List<string> lstChasi);
            for (int i = 0; i < lstVibX.Count; i++)
            {
                IDictionary eventData = new Hashtable();
                eventData["vibxaccg"] = lstVibX[i];
                eventData["vibyaccg"] = lstVibY[i];
                eventData["chasiaccg"] = lstChasi[i];
                _listener.Update("greetings", eventData, false);
                Thread.Sleep(1000 + rand.Next(2000));
            }
            
        }
       
    }

    private void LoadTempratureData(out List<string> lstVibX, out List<string> lstVibY, out List<string> lstChasi)
    {
        string path =
            System.IO.Path.GetFullPath(Directory.GetCurrentDirectory() + @"\data.csv");
        using (var reader = new StreamReader(path))
        {
          lstVibX=new List<string>();
          lstVibY = new List<string>();
          lstChasi = new List<string>();
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var values = line.Split(',');


                lstVibX.Add(values[0]);
                lstVibY.Add(values[1]);
                lstChasi.Add(values[2]);
            }
        }
    }
}