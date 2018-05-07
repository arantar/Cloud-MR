using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;

public class AccessPointConfig
{
    public string StatusNetwork { get; set; }
    public AccessPointConfig()
    {
        NetworkLaunch();
    }
    private string NetworkLaunch()
    {
        StatusNetwork = "";
        Process firstProcess = new Process();
        firstProcess.StartInfo = startInfo;
        StatusNetwork = СhangeEncoding(firstProcess);
        if (StatusNetwork.IndexOf("Не запущено") > 0) {
            startInfo.Arguments = @"/C netsh wlan start hostednetwork";
            Process repeatedProcess = new Process();
            repeatedProcess.StartInfo = startInfo;
            StatusNetwork = СhangeEncoding(repeatedProcess);
        }
        else {
            StatusNetwork = "Сеть уже запущена";
        }
        return StatusNetwork;
    }
    private string СhangeEncoding(Process process)
    {
        process.Start();
        Encoding cp866 = Encoding.GetEncoding("cp866");
        Encoding win1251 = Encoding.GetEncoding("Windows-1251");
        byte[] cp866Bytes = win1251.GetBytes(process.StandardOutput.ReadToEnd().ToCharArray());
        byte[] win1251Bytes = Encoding.Convert(cp866, win1251, cp866Bytes);
        string outputStr = win1251.GetString(win1251Bytes);
        process.WaitForExit();
        return outputStr;
    }

    private readonly ProcessStartInfo startInfo = new ProcessStartInfo {
        FileName = "cmd.exe",
        Arguments = @"/C netsh wlan show hostednetwork",
        WindowStyle = ProcessWindowStyle.Hidden,
        UseShellExecute = false,
        CreateNoWindow = true,
        RedirectStandardOutput = true
    };
}