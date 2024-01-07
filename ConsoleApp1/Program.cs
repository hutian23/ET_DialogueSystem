using ET;
using MongoDB.Bson;
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

        public List<int> list = new()
        {
            2,
            3,
            6,
            2433,
            343
        };

        public Vector2 position = new Vector2(1, 2);
    }

    public class Student2: Student
    {
        public string test = "21321321啊哈哈啊哈哈哈哈啊哈哈";
    }

    public class Student3: Student
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

    public class StudentSecond : Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
    }

    public static void Main()
    {
        MongoHelper.Init();

        Student stu = new Student() { Age = 10, position = new Vector2(2, 3) };
        byte[] bytes = MongoHelper.Serialize(stu);
        Student2 stu2 = MongoHelper.Deserialize<Student2>(bytes);
        Console.WriteLine(stu2.ToJson());
    }
}