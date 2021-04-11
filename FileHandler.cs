#region Using statements

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;

#endregion Using statements

namespace StartupOrganizer
{
    internal static class FileHandler
    {
        #region Internal enums

        [Flags]
        internal enum CompilationMode
        {
            Invalid = 0,
            Native = 0x1,
            CLR = Native << 1,
            Bit32 = CLR << 1,
            Bit64 = Bit32 << 1
        }

        #endregion Internal enums

        #region Internal static functions

        internal static bool PotentiallyA32bitApplication(string fileName)
        {
            FileInfo fileInfo = new(fileName);
            if (fileInfo is null) return false;
            var mode = GetCompilationMode(fileInfo);
            return mode.HasFlag(CompilationMode.Bit32) || fileInfo.DirectoryName.Contains(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86));

        }

        internal static CompilationMode GetCompilationMode(FileInfo fileInfo)
        {
            if (!fileInfo.Exists) throw new ArgumentException($"{fileInfo.FullName} does not exist");

            var intPtr = IntPtr.Zero;
            try
            {
                uint unmanagedBufferSize = 4096;
                intPtr = Marshal.AllocHGlobal((int)unmanagedBufferSize);

                using (var stream = File.Open(fileInfo.FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    var bytes = new byte[unmanagedBufferSize];
                    stream.Read(bytes, 0, bytes.Length);
                    Marshal.Copy(bytes, 0, intPtr, bytes.Length);
                }

                //Check DOS header magic number
                if (Marshal.ReadInt16(intPtr) != 0x5a4d) return CompilationMode.Invalid;

                // This will get the address for the WinNT header  
                var ntHeaderAddressOffset = Marshal.ReadInt32(intPtr + 60);

                // Check WinNT header signature
                var signature = Marshal.ReadInt32(intPtr + ntHeaderAddressOffset);
                if (signature != 0x4550) return CompilationMode.Invalid;

                //Determine file bitness by reading magic from IMAGE_OPTIONAL_HEADER
                var magic = Marshal.ReadInt16(intPtr + ntHeaderAddressOffset + 24);

                var result = CompilationMode.Invalid;
                uint clrHeaderSize;
                if (magic == 0x10b)
                {
                    clrHeaderSize = (uint)Marshal.ReadInt32(intPtr + ntHeaderAddressOffset + 24 + 208 + 4);
                    result |= CompilationMode.Bit32;
                }
                else if (magic == 0x20b)
                {
                    clrHeaderSize = (uint)Marshal.ReadInt32(intPtr + ntHeaderAddressOffset + 24 + 224 + 4);
                    result |= CompilationMode.Bit64;
                }
                else return CompilationMode.Invalid;

                result |= clrHeaderSize != 0
                    ? CompilationMode.CLR
                    : CompilationMode.Native;

                return result;
            }
            finally
            {
                if (intPtr != IntPtr.Zero) Marshal.FreeHGlobal(intPtr);
            }
        }

        internal static Constants.FILE_TYPE GetFileType(string fileName)
        {
            FileInfo fi = new(fileName);
            if (fi is null) return Constants.FILE_TYPE.OTHER;
            return fi.Extension.ToLower() switch
            {
                ".exe" => Constants.FILE_TYPE.EXECUTABLE,
                ".lnk" => Constants.FILE_TYPE.SHORTCUT,
                ".dll" => Constants.FILE_TYPE.DLL,
                _ => Constants.FILE_TYPE.OTHER,
            };
        }

        internal static List<string> RunPostScript(string scriptFile)
        {
            List<string> output = new();
            ProcessStartInfo startInfo = new("powershell", $"-ExecutionPolicy Bypass -File {scriptFile}")
            {
                CreateNoWindow = true,
                Verb = "runas",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false
            };
            using (Process process = new()
            {
                EnableRaisingEvents = true,
                StartInfo = startInfo
            })
            {
                process.OutputDataReceived += (object sender, DataReceivedEventArgs e) =>
                {
                    if (e.Data != null)
                    {
                        output.Add(e.Data);
                    }
                };
                process.ErrorDataReceived += (object sender, DataReceivedEventArgs e) =>
                {
                    if (e.Data != null)
                    {
                        output.Add(e.Data);
                    }
                };
                process.Start();
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();
                process.WaitForExit(2000);
            }
            return output;
        }

        #endregion Internal static functions
    }
}
