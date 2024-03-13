using System.Text.RegularExpressions;

// 定义语法树节点的类型
public enum NodeType
{
    If,
    Condition,
    Statement
}

// 定义语法树节点
public class Node
{
    public NodeType Type { get; set; }
    public string Value { get; set; }
    public Node Left { get; set; }
    public Node Right { get; set; }

    public Node(NodeType type, string value)
    {
        Type = type;
        Value = value;
        Left = null;
        Right = null;
    }
}

// 用于构建语法树的类
public class SyntaxTreeBuilder
{
    public Node BuildSyntaxTree(string condition, string statement)
    {
        Node rootNode = new Node(NodeType.If, "if");
        Node conditionNode = new Node(NodeType.Condition, condition);
        Node statementNode = new Node(NodeType.Statement, statement);

        rootNode.Left = conditionNode;
        rootNode.Right = statementNode;

        return rootNode;
    }
}

class Program
{
    static void Main(string[] args)
    {
        Match match = Regex.Match("If: HP > 10", @":\s*(\w+)");
        Console.WriteLine(match.Groups[1].Value);
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

    static void PrintSyntaxTree(Node node, int indent)
    {
        if (node == null)
            return;

        // 打印节点值
        Console.WriteLine($"{new string(' ', indent)}{node.Type}: {node.Value}");

        // 打印左子树
        PrintSyntaxTree(node.Left, indent + 2);

        // 打印右子树
        PrintSyntaxTree(node.Right, indent + 2);
    }
}