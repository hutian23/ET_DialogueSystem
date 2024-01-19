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
        string input = "<Numeric id=100/> <UnitConfig type=helloworld id=10/> <replace dsa123/>";

        string pattern = @"<\w+\s+[^>]*\/>";

        Regex regex = new Regex(pattern);

        MatchCollection matches = regex.Matches(input);

        foreach (Match match in matches)
        {
            Console.WriteLine(match.Value);
        }
        // string input = "VN_Position ch = Celika2 position = (-4,-4);";
        //
        // // 匹配 ch、type 和 position 后的参数，要求 type 和/或 position 至少有一个存在
        // string pattern = @"VN_Position ch = (?<ch>\w+)(?: type = (?<type>\w+))?(?: position = \((?<x>-?\d+),(?<y>-?\d+)\))?;";
        //
        // Match match = Regex.Match(input, pattern);
        //
        // if (match.Success)
        // {
        //     string chValue = match.Groups["ch"].Value;
        //     string typeValue = match.Groups["type"].Success ? match.Groups["type"].Value : "N/A";
        //     string xValue = match.Groups["x"].Success ? match.Groups["x"].Value : "N/A";
        //     string yValue = match.Groups["y"].Success ? match.Groups["y"].Value : "N/A";
        //
        //     Console.WriteLine($"匹配成功:\nch = {chValue}\ntype = {typeValue}\nposition = ({xValue},{yValue})");
        // }
        // else
        // {
        //     Console.WriteLine("匹配失败");
        // }
    }
}