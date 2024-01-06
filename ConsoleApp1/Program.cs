using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;
using UnityEngine;

static class Program
{
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        
        public List<int> list = new(){2,3,6,2433,343};
        public List<string> data = new() { "hello world", "test " };
        public List<test> Test223423423s = new() { new test(){a=2},new test(){a=3} };
        public List<test223423423> struct_list = new() { new test223423423() };

        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfDocuments)]
        public Dictionary<test223423423, int> dict = new();
        public myEnum _myEnum;
        public test ins;
        public test223423423 ins2;
        public Vector2Int position;
        public test223423423 ins3;
    }
    
    public class Student2 : Student
    {
        public string test = "21321321啊哈哈啊哈哈哈哈啊哈哈";
    }
    
    public class Student3 : Student
    {
        public string test2 = "213213324423423423421啊哈哈啊哈哈哈哈啊哈哈";
    }
    
    public enum myEnum
    {
        None = 0
    }
    
    public struct test223423423
    {
        public int b;
    }

    public class test
    {
        public int a = 10;
    }

    public class StudentSecond
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
    }

    public static void Main()
    {
        // EditorSerializeHelper.Init();
        // Student stu = new Student()
        // {
        //     Age = 10,
        //     Name = "123",
        //     Id = 111,
        //     list = new() { 1, 23, 4432, 24 },
        //     ins = new test() { a = 20 },
        //     ins2 = new test223423423() { b = 123 },
        //     ins3 = new test223423423(){ b = 233},
        //     position = new Vector2Int(1,2)
        // };
        // stu.dict.Add(new test223423423(){b = 2},100);
        // stu.dict.Add(new test223423423(){b = 3},120);
        // test223423423 ins2 = new() { b = 232 };
        // EditorSerializeHelper.RegisterStruct<test223423423>();
        // byte[] byte2 = EditorSerializeHelper.Serialize(ins2);
        // test223423423 test223423423 = EditorSerializeHelper.Deserialize<test223423423>(byte2);
        // Console.WriteLine(EditorSerializeHelper.ToJson(test223423423));
        //
        // byte[] bytes = EditorSerializeHelper.Serialize(stu);
        // Student2 stu2 = EditorSerializeHelper.Deserialize<Student2>(bytes);
        // stu2.ins.a = 30;
        // Console.WriteLine(EditorSerializeHelper.ToJson(stu2));
        //
        // byte[] byte3 = EditorSerializeHelper.Serialize(stu2);
        // Student3 stu3 = EditorSerializeHelper.Deserialize<Student3>(byte3);
        // Console.WriteLine(EditorSerializeHelper.ToJson(stu3));
        //
        // foreach (var kv in stu2.dict)
        // {
        //     Console.WriteLine(kv);
        // }
    }
}