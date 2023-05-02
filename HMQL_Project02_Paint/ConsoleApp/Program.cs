// See https://aka.ms/new-console-template for more information

//internal interface IShapeEntity
//{
//    string Name { get; }
//}

using IContract;
using System;
using System.Reflection;

string exeFolder = AppDomain.CurrentDomain.BaseDirectory;
var folderInfo = new DirectoryInfo(exeFolder);
var dllFiles = folderInfo.GetFiles("*.dll");

//Quét dll để tìm khả năng mới
var list = new List<IShapeEntity>();

foreach (var dll in dllFiles)
{
    //var domain = AppDomain.CurrentDomain;
    //Assembly assembly = domain.Load(
    //    AssemblyName.GetAssemblyName(dll.FullName));

    Assembly assembly = Assembly.LoadFrom(dll.FullName);

    //Get all of the types in the dll
    //Console.Write($"Getting types of file: {dll.FullName}");
    Type[] types = assembly.GetTypes();

    foreach (Type type in types)
    {
        //Console.WriteLine(type.FullName);
        if (type.IsClass)
        {
            if (typeof(IShapeEntity).IsAssignableFrom(type))
            {
                list.Add((Activator.CreateInstance(type) as IShapeEntity)!);
            }
        }
    }
}

//Hiển thị entity đã tìm ra
foreach (IShapeEntity entity in list)
{
    Console.WriteLine(entity.Name);
}