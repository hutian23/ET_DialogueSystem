using System.Reflection;
using System.Text.RegularExpressions;
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

    public class StudentSecond: Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
    }

    public static void Main()
    {
        string input = "VN_WaitAnimPlay ch = Knight clip = Knight_Idle time = 3000;";
        string pattern = @"VN_WaitAnimPlay\s+ch\s*=\s*(\w+)\s*clip\s*=\s*(\w+)\s*time\s*=\s*(\d+);";

        Match match = Regex.Match(input, pattern);

        if (match.Success)
        {
            string ch = match.Groups[1].Value;
            string clip = match.Groups[2].Value;
            string time = match.Groups[3].Value;

            Console.WriteLine("ch: " + ch);
            Console.WriteLine("clip: " + clip);
            Console.WriteLine("time: " + time);
        }
        else
        {
            Console.WriteLine("No match found");
        }
    }
}