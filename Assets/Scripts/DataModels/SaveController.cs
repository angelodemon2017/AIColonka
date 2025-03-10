using System;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

public class SaveController
{
    const string nameFileSave = "data.sav";

    public static void Save<T>(T _data, string prefix = "")
    {
        var totalPath = Application.persistentDataPath + "/" + prefix + nameFileSave;

        Debug.Log(totalPath);
        var hex = DataToString(_data);
        File.WriteAllText(totalPath, hex.Replace("-", ""));
    }

    public static T Load<T>(string prefix = "")
    {
        var totalPath = Application.persistentDataPath + "/" + prefix + nameFileSave;
        if (File.Exists(totalPath))
        {
            var filer = File.ReadAllText(totalPath);
            int charsCount = filer.Length;
            byte[] bytes = new byte[charsCount / 2];
            // UnCrypt
            for (int i = 0; i < charsCount; i += 2)
            {
                bytes[i / 2] =
                (byte)(byte.MaxValue - Convert.ToByte(filer.Substring(i, 2), 16));
            }
            var result = Encoding.UTF8.GetString(bytes, 0, bytes.Length);
            return JsonUtility.FromJson<T>(result);
        }
        else
        {
            return default(T);
        }
    }

    public static T StringToData<T>(string byteStr)
    {
        int charsCount = byteStr.Length;
        byte[] bytes = new byte[(charsCount + 1) / 3];
        // UnCrypt
        for (int i = 0; i < charsCount; i += 3)
        {
            bytes[i / 3] =
            (byte)(byte.MaxValue - Convert.ToByte(byteStr.Substring(i, 2), 16));
        }
        var result = Encoding.UTF8.GetString(bytes, 0, bytes.Length);
        return JsonUtility.FromJson<T>(result);
    }

    public static string DataToString<T>(T _data)
    {
        byte[] byteData = DataToBytes(_data);
        return BitConverter.ToString(byteData);
    }

    private static byte[] DataToBytes<T>(T _data)
    {
        string jsonSave = JsonUtility.ToJson(_data);
//        string js = _data.ToJSON();//������ ��� JsonUtility.ToJson

        byte[] byteData = Encoding.UTF8.GetBytes(jsonSave);
        // Crypt
        for (int i = 0; i < byteData.Count(); i++)
        {
            byteData[i] = (byte)(byte.MaxValue - byteData[i]);
        }/**/

        return byteData;
    }
}