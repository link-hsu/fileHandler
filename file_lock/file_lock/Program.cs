using FileSystem;
using System.IO;
using System.Collections.Generic;
using System.Formats.Tar;

string type = "";
if (args != null)
  {
    int argsLength = args.Length;
        for (int i = 0; i < argsLength; i++)
    {
      type = args[i];
    }
  }

Console.WriteLine("Hello, World!");


string sPath = @"D:\Web\FileUpload"; //指定目标文件夹
// string sPath = @"D:\web\FileUpload\Temp\StockSubbrokerageOpen\20241024\180b44a9-39fa-4464-b200-1e1e945806d4"; //指定目标文件夹
List<string> listFiles = HandleFiles.FindFile(sPath);
foreach (string sFile in listFiles)
{
  if (File.Exists(sFile))
    {
    var a = HandleFiles.CheckTextFile(sFile);
    if (type == "D" && a == HandleFiles.FileExtension.Encrypt)
      {    
          HandleFiles.DecryptFile(sFile, sFile.TrimEnd("_E".ToArray()));
      } else if(type == "E") {
      // Console.WriteLine(sFile);
        HandleFiles.EncryptFile(sFile, sFile+"_E");
      }
  }
}
