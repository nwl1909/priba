using System;
using System.IO;
using System.Runtime.InteropServices;
using CTFAK.FileReaders;
using CTFAK.Utils;
using Joveler.Compression.ZLib;

namespace CTFAK
{
    public class CTFAKCore
    {
        public delegate void SaveHandler(int index, int all);

        public static IFileReader currentReader;
        public static string parameters;
        public static string path;

        public static void Init()
        {
            AppDomain.CurrentDomain.UnhandledException += (o, e) =>
            {
                Console.WriteLine(e.ExceptionObject.GetType());
                if (e.ExceptionObject is Exception ex)
                    Console.WriteLine(ex);
            };

            string baseDir = AppContext.BaseDirectory;
            // 64-bit native libs live in "x64"; 32-bit in "x86" (falls back to x64 folder)
            string arch = RuntimeInformation.ProcessArchitecture == Architecture.X86 ? "x86" : "x64";
            string nativeDir = Path.Combine(baseDir, arch);
            if (!Directory.Exists(nativeDir))
                nativeDir = Path.Combine(baseDir, "x64");

            string zlibPath = Path.Combine(nativeDir, "zlibwapi.dll");
            string nativePath = Path.Combine(nativeDir, "CTFAK-Native.dll");

            if (!File.Exists(zlibPath))
                throw new FileNotFoundException(
                    "zlibwapi.dll not found. Extract ctfakrequirements.zip next to CTFAK.Cli.exe.", zlibPath);
            if (!File.Exists(nativePath))
                throw new FileNotFoundException(
                    "CTFAK-Native.dll not found. Extract ctfakrequirements.zip next to CTFAK.Cli.exe.", nativePath);

            ZLibInit.GlobalInit(zlibPath);
            // Preload so the [DllImport("CTFAK-Native.dll")] calls resolve from the x64/x86 folder
            NativeLibrary.Load(nativePath);
        }
    }
}
