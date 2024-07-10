using System.Text.RegularExpressions;
using UnityEngine;

// // 定义语法树节点的类型
// public enum NodeType
// {
//     If,
//     Condition,
//     Statement
// }
//
// // 定义语法树节点
// public class Node
// {
//     public NodeType Type { get; set; }
//     public string Value { get; set; }
//     public Node Left { get; set; }
//     public Node Right { get; set; }
//
//     public Node(NodeType type, string value)
//     {
//         Type = type;
//         Value = value;
//         Left = null;
//         Right = null;
//     }
// }
//
// // 用于构建语法树的类
// public class SyntaxTreeBuilder
// {
//     public Node BuildSyntaxTree(string condition, string statement)
//     {
//         Node rootNode = new Node(NodeType.If, "if");
//         Node conditionNode = new Node(NodeType.Condition, condition);
//         Node statementNode = new Node(NodeType.Statement, statement);
//
//         rootNode.Left = conditionNode;
//         rootNode.Right = statementNode;
//
//         return rootNode;
//     }
// }

public class TestInfo
{
    public int frame;
}

class Program
{
    static void Main(string[] args)
    {
        //Linq测试
        //count >=1
        // List<TestInfo> infos = new(){new TestInfo(){frame = 3},new TestInfo(){frame = 4},new TestInfo(){frame = 5}};
        List<TestInfo> infos = new();
        Console.WriteLine(infos.Max(info => info.frame));
        // List<int> list = new List<int>()
        // {
        //     1,
        //     2,
        //     3,
        //     6,
        //     656
        // };
        // Console.WriteLine(list.Max());
        // string input = "LogWarning: <Param name = Hello World/> <Param name = 2222/>";
        // string pattern = @"<Param name = [^/]+/>";
        //
        // MatchCollection matches = Regex.Matches(input, pattern);
        // foreach (Match match in matches)
        // {
        //     string matchLine = match.Value;
        //     Match match2 = Regex.Match(matchLine, "<Param name = (?<param>.*?)/>");
        //     Console.WriteLine(match2.Groups["param"].Value);
        //     Console.WriteLine("匹配到的内容: " + match.Value);
        //     input = input.Replace(match2.Value, match2.Groups["param"].Value);
        // }

        // Console.WriteLine(input);
        // Match match2 = Regex.Match("BeginIf: HP > 10", "BeginIf: (.+)");
        // string op = match2.Groups[1].Value;
        // var ops = op.Split(' ');
        // Console.WriteLine(ops[0]);

        // var data = new List<string> { "just a test", "Just a test", "Test", "example", "another test" };
        // string searchTerm = "test";
        //
        // var results = FuzzySearchMethod(data, searchTerm);
        // foreach (var result in results)
        // {
        //     Console.WriteLine(result);
        // }
        // Console.WriteLine((int)Mathf.Clamp01(0.2f));
        // SortedSet<int> set = new();
        // set.Add(2);
        // set.Add(1);
        // set.Add(1);
        // set.Add(0);
        // foreach (var  t in set)
        // {
        //     Console.WriteLine(t);
        // }
        // string input = "HP < 10";
        // string pattern = @"^\w+"; // 匹配以字母开头的字符序列
        //
        // Match match = Regex.Match(input, pattern);
        //
        // if (match.Success)
        // {
        // Console.WriteLine("第一个单词是: " + match.Value);
        // }
        // else
        // {
        //     Console.WriteLine("没有找到匹配的单词");
        // }
        // Match match = Regex.Match("HP > 10", @"^\w+");
        // Console.WriteLine(match.Value);
        // Match match = Regex.Match("RegistInput: '236P',5;", @"RegistInput: '(?<Checker>\w+)',(?<LastedFrame>\w+);");
        // Console.WriteLine(match.Groups["LastedFrame"].Value);
        // List<string> str2 = new();
        // str.ForEach(s => { str2.Add(s); });
        // string input = "Sprite: 'rg000_1',3;";
        // string pattern = @"Sprite:\s*'([^']+)',(\d+);";
        //
        // Match match = Regex.Match(input, pattern);
        // if (match.Success)
        // {
        //     string spriteName = match.Groups[1].Value;
        //     int param1 = int.Parse(match.Groups[2].Value);
        //
        //     Console.WriteLine("Sprite Name: " + spriteName);
        //     Console.WriteLine("Parameter 1: " + param1);
        // }
        // else
        // {
        //     Console.WriteLine("No match found.");
        // }
        // string input = "SkillTrigger: HP < 10;";
        // string pattern = @":\s*(.+);"; // 匹配冒号后面的内容，直到分号为止
        //
        // Match match = Regex.Match(input, pattern);
        //
        // if (match.Success)
        // {
        //     string content = match.Groups[1].Value;
        //     Console.WriteLine("匹配到的内容是:" + content);
        // }
        // else
        // {
        //     Console.WriteLine("没有找到匹配的内容。");
        // }
        // int i = 0;
        // Console.WriteLine(i++);
        // Dictionary<int, string> dic = new();
        // dic.Add(1,"222");
        // var op = dic[1];
        // op = "Hello world";
        // Console.WriteLine(dic[1]);
    }

    // static void PrintSyntaxTree(Node node, int indent)
    // {
    //     if (node == null)
    //         return;
    //
    //     // 打印节点值
    //     Console.WriteLine($"{new string(' ', indent)}{node.Type}: {node.Value}");
    //
    //     // 打印左子树
    //     PrintSyntaxTree(node.Left, indent + 2);
    //
    //     // 打印右子树
    //     PrintSyntaxTree(node.Right, indent + 2);
    // }

    /// <summary>
    ///     Calculate the difference between 2 strings using the Levenshtein distance algorithm
    /// </summary>
    /// <param name="source1">First string</param>
    /// <param name="source2">Second string</param>
    /// <returns></returns>
    public static int Calculate(string source1, string source2) //O(n*m)
    {
        var source1Length = source1.Length;
        var source2Length = source2.Length;

        var matrix = new int[source1Length + 1, source2Length + 1];

        // First calculation, if one entry is empty return full length
        if (source1Length == 0)
            return source2Length;

        if (source2Length == 0)
            return source1Length;

        // Initialization of matrix with row size source1Length and columns size source2Length
        for (var i = 0; i <= source1Length; matrix[i, 0] = i++)
        {
        }

        for (var j = 0; j <= source2Length; matrix[0, j] = j++)
        {
        }

        // Calculate rows and collumns distances
        for (var i = 1; i <= source1Length; i++)
        {
            for (var j = 1; j <= source2Length; j++)
            {
                var cost = (source2[j - 1] == source1[i - 1])? 0 : 1;

                matrix[i, j] = Math.Min(Math.Min(matrix[i - 1, j] + 1, matrix[i, j - 1] + 1),
                    matrix[i - 1, j - 1] + cost);
            }
        }

        // return result
        return matrix[source1Length, source2Length];
    }

    public static List<string> FuzzySearchMethod(List<string> data, string searchTerm)
    {
        // 转换输入的searchTerm为小写
        searchTerm = searchTerm.ToLower();

        var results = data
                .Where(item => item.ToLower().Contains(searchTerm)) // 进行包含匹配并忽略大小写
                .OrderBy(item => item.Length) // 可以按字符串长度排序，确保更精确的结果在前
                .ToList();

        return results;
    }

    public static double JaroWinklerDistance(string s1, string s2)
    {
        int s1Len = s1.Length;
        int s2Len = s2.Length;

        if (s1Len == 0)
            return s2Len == 0? 1.0 : 0.0;

        int matchDistance = Math.Max(s1Len, s2Len) / 2 - 1;

        bool[] s1Matches = new bool[s1Len];
        bool[] s2Matches = new bool[s2Len];

        int matches = 0;
        int transpositions = 0;

        for (int i = 0; i < s1Len; i++)
        {
            int start = Math.Max(0, i - matchDistance);
            int end = Math.Min(i + matchDistance + 1, s2Len);

            for (int j = start; j < end; j++)
            {
                if (s2Matches[j])
                    continue;
                if (s1[i] != s2[j])
                    continue;
                s1Matches[i] = true;
                s2Matches[j] = true;
                matches++;
                break;
            }
        }

        if (matches == 0)
            return 0.0;

        int k = 0;
        for (int i = 0; i < s1Len; i++)
        {
            if (!s1Matches[i])
                continue;
            while (!s2Matches[k])
                k++;
            if (s1[i] != s2[k])
                transpositions++;
            k++;
        }

        double jaro = ((double)matches / s1Len +
            (double)matches / s2Len +
            (double)(matches - transpositions / 2.0) / matches) / 3.0;

        double p = 0.1; // scaling factor
        int l = 0; // length of common prefix
        int prefixLimit = Math.Min(4, Math.Min(s1Len, s2Len));
        for (int i = 0; i < prefixLimit; i++)
        {
            if (s1[i] == s2[i])
                l++;
            else
                break;
        }

        return jaro + l * p * (1.0 - jaro);
    }
}