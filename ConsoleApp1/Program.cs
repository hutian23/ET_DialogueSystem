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
        int i = 0;
        Console.WriteLine(i++);
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