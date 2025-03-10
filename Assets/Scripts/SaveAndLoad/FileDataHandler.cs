using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class FileDataHandler
{
    private string dataDirPath = "";
    private string dataFileName = "";

    public FileDataHandler(string _dataDirPath,string _datafilename)
    {
        dataDirPath = _dataDirPath;
        dataFileName = _datafilename;
    }

    public void Save(GameData _data)
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);

        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
            //Directory.CreateDirectory()是 .NET 框架中的一个方法，用于在指定的路径上创建所有目录和子目录（如果不存在的话）。这个方法属于System.IO命名空间下的Directory类。
            //使用这个方法可以确保即使路径中包含多级不存在的目录，也会一次性全部创建出来，而不需要手动逐级创建。
            //Path.Combine方法将dataDirPath和dataFileName合并成一个完整的文件路径。


            string dataToStore =JsonUtility.ToJson(_data,true);
            //使用JsonUtility.ToJson方法将_data对象序列化为JSON格式的字符串，true参数表示格式化输出，使JSON字符串更易于阅读。

            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
                //使用FileStream创建一个新的文件流，用于写入数据。FileMode.Create参数表示如果文件已存在，则覆盖它。
            {
                using (StreamWriter writer = new StreamWriter(stream))//使用StreamWriter创建一个新的流写入器，用于将字符串写入到stream中。
                {
                    writer.Write(dataToStore);//将序列化后的JSON字符串dataToStore写入到文件中。
                }
            }
        }

        catch (Exception e)
        {
            Debug.LogError("Error on trying to save data to file: " + fullPath +"\n" +e);
        }

    }

    public GameData Load()
    {
        string fullPath=Path.Combine(dataDirPath, dataFileName);
        GameData loadData =null;

        if (File.Exists(fullPath))//检查fullPath指定的文件是否存在。
        {
            try
            {
                string dataToLoad = "";

                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                //使用FileStream创建一个新的文件流，用于读取数据。FileMode.Open参数表示如果文件不存在，则抛出异常。
                {
                    using (StreamReader reader = new StreamReader(stream))//使用StreamReader创建一个新的流读取器，用于从stream中读取字符串。
                    {
                        dataToLoad = reader.ReadToEnd();//使用StreamReader的ReadToEnd方法将文件中的所有内容读取到dataToLoad字符串中。
                    }
                }

                loadData = JsonUtility.FromJson<GameData>(dataToLoad);//使用JsonUtility.FromJson方法将dataToLoad字符串反序列化为GameData类型的实例，并赋值给loadData。
            }
            catch (Exception e)
            {
                Debug.LogError("Error on trying to load data from file:"+fullPath+"\n"+e);
            }
        }

        return loadData;
    }
    public void Delete()
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);

        if (File.Exists(fullPath))
            File.Delete(fullPath);
    }
}
