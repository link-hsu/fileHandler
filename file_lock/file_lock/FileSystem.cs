using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.ExtendedProperties;
using DocumentFormat.OpenXml.Math;
using log4net.Repository.Hierarchy;
using System;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace FileSystem
{

  public enum FileType
  {
    Image = 1,
    Doc = 2,
    Txt = 3,
    Js = 4,
    Pdf = 5
  }
  static class HandleFiles
  {
    private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    public static void EncryptFile(string inputFile, string outputFile)   //加密
    {
      try
      {
        string password = @"12345678";
        UnicodeEncoding UE = new UnicodeEncoding();
        byte[] key = UE.GetBytes(password);

        string cryptFile = outputFile;
        FileStream fsCrypt = new FileStream(cryptFile, FileMode.Create);

        RijndaelManaged RMCrypto = new RijndaelManaged();

        CryptoStream cs = new CryptoStream(fsCrypt,
            RMCrypto.CreateEncryptor(key, key),
            CryptoStreamMode.Write);

        FileStream fsIn = new FileStream(inputFile, FileMode.Open);

        int data;
        while ((data = fsIn.ReadByte()) != -1)
          cs.WriteByte((byte)data);


        fsIn.Close();
        cs.Close();
        fsCrypt.Close();
        File.Delete(inputFile);

      }
      catch (Exception ex)
      {
      }
    }

    public static string DecryptFile(string inputFile, string outputFile)   //解密
    {
      try
      {
        string password = @"12345678";
        UnicodeEncoding UE = new UnicodeEncoding();
        byte[] key = UE.GetBytes(password);

        FileStream fsCrypt = new FileStream(inputFile, FileMode.Open);

        RijndaelManaged RMCrypto = new RijndaelManaged();

        CryptoStream cs = new CryptoStream(fsCrypt,
            RMCrypto.CreateDecryptor(key, key),
            CryptoStreamMode.Read);

        FileStream fsOut = new FileStream(outputFile, FileMode.Create);

        int data;
        while ((data = cs.ReadByte()) != -1)
          fsOut.WriteByte((byte)data);


        fsOut.Close();
        cs.Close();
        fsCrypt.Close();
        Console.WriteLine(outputFile);
        Byte[] bytes = File.ReadAllBytes(outputFile);
        String file = Convert.ToBase64String(bytes);
        // File.Delete(outputFile);
        return file;
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.Message);
        return ex.Message;
      }
    }

    public static List<string> FindFile(string sSourcePath)
    {
      List<string> list = new List<string>();
      DirectoryInfo theFolder = new DirectoryInfo(sSourcePath);
      FileInfo[] thefileInfo = theFolder.GetFiles("*.*", SearchOption.TopDirectoryOnly);
      foreach (FileInfo NextFile in thefileInfo)
      {
        list.Add(NextFile.FullName);
      }  //遍历文件
         //遍历子文件夹(以及文件夹里的文件)
      DirectoryInfo[] dirInfo = theFolder.GetDirectories();
      foreach (DirectoryInfo NextFolder in dirInfo)
      {
        list.Add(NextFolder.FullName);
        /* 遍历子文件夹里的文件 */
        
        FileInfo[] fileInfo = NextFolder.GetFiles("*.*", SearchOption.AllDirectories);
        foreach (FileInfo NextFile in fileInfo)  
        {
            list.Add(NextFile.FullName);
        }
        
      }

      return list;
    }


    public static FileExtension CheckTextFile(string fileName)
    {
      FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
      BinaryReader br = new BinaryReader(fs);
      string fileType = string.Empty; ;
      try
      {
        byte data = br.ReadByte();
        fileType += data.ToString();
        data = br.ReadByte();
        fileType += data.ToString();
        Console.WriteLine(fileType + "::" + fileName);
        FileExtension extension;

        try
        {
          extension = (FileExtension)Enum.Parse(typeof(FileExtension), fileType);
        }
        catch
        {
          // Console.WriteLine("Hello, World!11111");
          extension = FileExtension.VALIDFILE;
         }
        return extension;
      }
      catch (Exception ex)
      {
        FileExtension extension;
        extension = FileExtension.VALIDFILE;
        // Console.WriteLine(fileName);
        return extension;
      }
      finally
      {
        if (fs != null)
        {
          fs.Close();
          br.Close();
        }
      }
    }
  
  public enum FileExtension
  {
    JPG = 255216,
    GIF = 7173,
    PNG = 13780,
    SWF = 6787,
    RAR = 8297,
    ZIP = 8075,
    _7Z = 55122,
    Encrypt = 69194,
    VALIDFILE = 9999999
  }

}
}