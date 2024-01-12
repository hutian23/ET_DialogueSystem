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
        // string input = "Numeric [HP] + 100;";
        //
        // // 提取方括号内的字符串
        // Match match = Regex.Match(input, @"\[([^]]*)\]");
        // if (match.Success)
        // {
        //     string bracketContent = match.Groups[1].Value;
        //     Console.WriteLine("方括号内的字符串: " + bracketContent);
        // }
        //
        // // 匹配操作码"+"和数字100
        // match = Regex.Match(input, @"(\+|\-|\*|\/)\s*(\d+)");
        // if (match.Success)
        // {
        //     string opcode = match.Groups[1].Value;
        //     string number = match.Groups[2].Value;
        //     Console.WriteLine("操作码: " + opcode);
        //     Console.WriteLine("数字: " + number);
        // }
        // string input = "ShowEmoji          \"Angry\"";
        //
        // // 定义正则表达式
        // string pattern = @"ShowEmoji\s+""([^""]*)""";
        //
        // // 创建 Regex 对象
        // Regex regex = new Regex(pattern);
        //
        // // 进行匹配
        // Match match = regex.Match(input);
        //
        // // 输出匹配的结果
        // if (match.Success)
        // {
        //     string emojiName = match.Groups[1].Value;
        //     Console.WriteLine("匹配到的表情名称：" + emojiName);
        // }
        // else
        // {
        //     Console.WriteLine("未匹配到任何内容。");
        // }
        
        string input = "WaitTime 323";

        // 使用模式 (\d+) 匹配一个或多个数字
        string pattern = @"WaitTime\s+(\d+)";

        Regex regex = new Regex(pattern);
        Match match = regex.Match(input);

        if (match.Success)
        {
            string numberValue = match.Groups[1].Value;
            Console.WriteLine("匹配到的数字: " + numberValue);
        }
        else
        {
            Console.WriteLine("匹配失败");
        }
    }

    static string GetFirstWord(string input)
    {
        // 定义一个正则表达式模式，匹配第一个单词
        string pattern = @"^\w+";

        // 创建正则表达式对象
        Regex regex = new Regex(pattern);

        // 进行匹配
        Match match = regex.Match(input);

        // 如果匹配成功，返回匹配到的值，否则返回空字符串
        return match.Success? match.Value : string.Empty;
    }
}