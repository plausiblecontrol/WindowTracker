using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace WindowTracker {
  class Program {
    [DllImport("user32.dll")]
    static extern IntPtr GetForegroundWindow();

    [DllImport("user32.dll")]
    public static extern IntPtr GetWindowThreadProcessId(IntPtr hWnd, out uint ProcessId);

    [DllImport("user32.dll")]
    static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

    static void Main(string[] args) {
      string titleA = "";
      while (true) {
        string titleB = GetActiveWindowTitle();
        string proc = GetActiveProcessFileName();
        if (titleA != titleB) {
          int l = proc.LastIndexOf("\\");
          Console.WriteLine(";" + DateTime.Now.ToLocalTime());
          Console.Write(DateTime.Now.ToLocalTime()+";"+proc.Substring(l+1,proc.Length-l-1)+";"+titleB);
          titleA = titleB;
        }
        Thread.Sleep(1000);
      }
    }
      private static string GetActiveWindowTitle() {
      const int nChars = 256;
      StringBuilder Buff = new StringBuilder(nChars);
      IntPtr handle = GetForegroundWindow();

      if (GetWindowText(handle, Buff, nChars) > 0) {
        return Buff.ToString();
      }
      return null;
    }
     private static string GetActiveProcessFileName() {
       try {
         IntPtr hwnd = GetForegroundWindow();
         uint pid;
         GetWindowThreadProcessId(hwnd, out pid);
         Process p = Process.GetProcessById((int)pid);
         return p.MainModule.FileName.ToString();
       } catch {
         return "n/a";
       }
      }
  }
}
