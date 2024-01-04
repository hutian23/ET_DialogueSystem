using System;
using System.Reflection;
using System.Text.RegularExpressions;
using ET;

class Program
{
    static void Main()
    {
        // string input = "这是一个示例 <model type=Numeric name=HP/> <model/> 这是另一个示例 <model type=UnitConfig name=Name/> 这是最后一个示例";
        //
        // // 匹配以<model开头以/>结尾的字符串
        // string pattern = @"<model\b[^>]*\/>";
        // Regex regex = new Regex(pattern);
        //
        // MatchCollection matches = regex.Matches(input);
        //
        // foreach (Match match in matches)
        // {
        //     Console.WriteLine("匹配到的字符串：" + match.Value);
        // }

        // string input = "<model type=hp name=HP/>";
        //
        // // 匹配 <model type= 后的内容
        // // string pattern = @"<model\s+type=(\w+)";
        // string pattern = @"<model\s+name=(\w+)";
        // Regex regex = new Regex(pattern);
        //
        // Match match = regex.Match(input);
        //
        // if (match.Success)
        // {
        //     string matchedText = match.Groups[1].Value;
        //     Console.WriteLine("匹配到的内容：" + matchedText);
        // }

        // string input = "<model type=Numeric name=HP/>";
        //
        // // 匹配 <model name= 后的内容
        // // string pattern = @"name=([A-Za-z]+)";
        // string pattern = @"type=([A-Za-z]+)";
        // Regex regex = new Regex(pattern);
        //
        // Match match = regex.Match(input);
        //
        // if (match.Success)
        // {
        //     string matchedText = match.Groups[1].Value;
        //     Console.WriteLine("匹配到的内容：" + matchedText);
        // }
        // Type type = typeof(NumericType);
        //
        // // 获取所有公共静态字段
        // FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.Static);
        //
        // // 遍历所有字段并输出它们的名称和值
        // foreach (FieldInfo field in fields)
        // {
        //     Console.WriteLine((int)(field.GetValue(null) ?? throw new InvalidOperationException()));
        // }

        // string input = "<model type=UnitConfig id=1233 name=Name />";
        // string pattern = @"id=(\d+)";

        // Regex regex = new(pattern);
        // Regex regex2 = new(pattern2);
        //
        // Match match = regex.Match(input);
        // Match match2 = regex2.Match(input);
        // Console.WriteLine(match.Groups[1].Value);
        // Console.WriteLine(match2.Groups[1].Value);
        // int intvalue = 2;
        // object enumValue = Enum.ToObject(typeof (test), intvalue);
        // Console.WriteLine(enumValue);

        string input = "这是一个{{HP}}字符串，{{MP}}双花括号内容HP。";
        // string replaced = ReplaceBracketsContent(input, "Hello world", "MP");
        //
        // Console.WriteLine("替换前: " + input);
        // Console.WriteLine("替换后: " + replaced);
        string replaced = Replace2(input, "HP", "你好啊");
        Console.WriteLine("替换前: " + input); 
        Console.WriteLine("替换后: " + replaced);
    }

    static string ReplaceBracketsContent(string input, string replacementHP, string replacementMP)
    {
        string patternHP = @"\{\{HP\}\}";
        string patternMP = @"\{\{MP\}\}";

        input = Regex.Replace(input, patternHP, replacementHP);
        input = Regex.Replace(input, patternMP, replacementMP);

        return input;
    }

    static string Replace2(string input, string beReplace, string replace)
    {
        string pattern = @"\{\{" + beReplace + @"\}\}";
        input = Regex.Replace(input, pattern, replace);
        return input;
    }

    public enum test
    {
        test1 = 1,
        test2 = 2
    }
}