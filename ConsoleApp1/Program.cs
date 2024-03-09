using System.Reflection;
using System.Text.RegularExpressions;
using ET;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.Options;
using MongoDB.Bson.Serialization.Serializers;
using UnityEngine;
using System;

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
        string input = "If: Check_HelloWorld > 10";
        string pattern = @":\s*(\w+)"; // 匹配冒号后面的第一个单词

        Match match = Regex.Match(input, pattern);
        if (match.Success)
        {
            string word = match.Groups[1].Value;
            Console.WriteLine("匹配到的单词是: " + word);
        }
        else
        {
            Console.WriteLine("未找到匹配的单词。");
        }
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