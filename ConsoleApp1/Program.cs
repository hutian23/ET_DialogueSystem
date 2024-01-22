﻿
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.Options;
using MongoDB.Bson.Serialization.Serializers;

static class Program
{
    public class User
    {
        public int id = 100;
    }

    public class test
    {
        [BsonIgnore]
        public int a = 10;

        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfDocuments)]
        public Dictionary<int, string> dic = new() { { 1, "2323" } };
    }

    public class User2: User
    {
    }

    static void AddRangeToBsonDocument(BsonDocument bsonDocument, Dictionary<int, User> dic)
    {
        foreach (var kvp in dic)
        {
            bsonDocument.Add(new BsonElement(kvp.Key.ToString(), ToBsonValue(kvp.Value)));
        }
    }

    static BsonValue ToBsonValue(User user)
    {
        BsonDocument subDoc = user.ToBsonDocument();
        subDoc.Remove("a");
        subDoc.Add("test", 123);
        return subDoc;
    }
    
    public enum Test
    {
        test1,
        test2
    }
    
    
    public static void Main()
    {
        BsonDocument doc = new BsonDocument();
        Dictionary<Test, string> dic = new();
        dic.Add(Test.test1,"2323");
        dic.Add(Test.test2,"22323");
        foreach ((Test key, string value) in dic)
        {
            doc.Add(new BsonElement(key.ToString(), value));
        }
        Console.WriteLine(doc);
        Console.WriteLine(new test().ToJson());
        // string input = "# 角色1\nVN_RegisterCharacter ch = Celika unitId = 1002; #Hello worldtewtew # 1231312313131123\nVN_Position ch = Celika type = Left;\n\n# 角色2\nVN_RegisterCharacter ch = Celika2 unitId = 1002;\nVN_Position ch = Celika2 type = Right;\nVN_Flip ch = Celika2 type = Left;";
        //
        // // 按行分割字符串
        // string[] lines = input.Split('\n');
        //
        // // 过滤掉以"#"开头且以"\n"结尾的行，保留注释部分
        // string result = string.Join("\n", lines
        //         .Select(line =>
        //         {
        //             int commentIndex = line.IndexOf('#');
        //             return commentIndex >= 0 ? line.Substring(0, commentIndex).Trim() : line.Trim();
        //         })
        //         .Where(filteredLine => !string.IsNullOrWhiteSpace(filteredLine)));
        //
        // // 打印结果
        // Console.WriteLine(input);
        // Console.WriteLine("\n");
        // Console.WriteLine(result);

        // ConventionPack conventionPack = new(){ new IgnoreExtraElementsConvention(true) };
        // ConventionRegistry.Register("IgnoreExtraElements", conventionPack, _=> true);
        // BsonClassMap.LookupClassMap(typeof (User));
        // BsonClassMap.LookupClassMap(typeof (User2));
        // var bsonDocument = new BsonDocument();
        //
        // Dictionary<int, User> dic = new();
        // dic.Add(1,new User());
        // dic.Add(2,new User());
        // dic.Add(3,new User2());
        // AddRangeToBsonDocument(bsonDocument,dic);
        // foreach (BsonValue bsonDocumentValue in bsonDocument.Values)
        // {
        //     Console.WriteLine(BsonSerializer.Deserialize<User>(bsonDocumentValue.ToBsonDocument()));
        // }
        // Console.WriteLine(bsonDocument);

        // var newRestaurant = new BsonDocument
        // {
        //     { "address", new BsonDocument { { "street", "pizza st" }, { "zipCode", "1003" } } }, { "coord", new BsonArray { -33, 444 } }
        // };
        // newRestaurant.Add(new BsonElement("restaurant_id", "123"));
        // newRestaurant.Remove("coord");
        // newRestaurant.Set("coord", "Hello world");
        // Console.WriteLine(newRestaurant);
        //
        // string outputFileName = "./myFile.bson";
        // using (var stream = File.OpenWrite(outputFileName))
        // using (var writer = new BsonBinaryWriter(stream))
        // {
        //     writer.WriteStartDocument();
        //     writer.WriteName("id");
        //     writer.WriteInt32(123);
        //     
        //     writer.WriteName("coord");
        //     writer.WriteStartArray();
        //     writer.WriteDouble(3.3);
        //     writer.WriteDouble(3.5);
        //     writer.WriteEndArray();
        //     
        //     writer.WriteName("address");
        //     writer.WriteStartDocument();
        //     writer.WriteName("hello wolrd");
        //     writer.WriteInt32(123213);
        //     writer.WriteEndDocument();
        //     writer.WriteEndDocument();
        // }
        //
        // using (var stream = File.OpenRead(outputFileName))
        // using (var reader = new BsonBinaryReader(stream))
        // {
        //     reader.ReadStartDocument();
        //     string addressFiledName = reader.ReadName();
        //     int addressno = reader.ReadInt32();
        //     Console.WriteLine(addressno);
        // }
        // ConventionPack conventionPack = new(){ new IgnoreExtraElementsConvention(true) };
        // ConventionRegistry.Register("IgnoreExtraElements", conventionPack, _=> true);
        // BsonClassMap.LookupClassMap(typeof (User));
        // BsonClassMap.LookupClassMap(typeof (User2));
        // User user = new User();
        // BsonDocument doc = user.ToBsonDocument();
        // doc.Remove("id");
        // doc.Add("Jel", 12);
        // Console.WriteLine(doc);
        // byte[] bytes = doc.ToBson();
        // User user2 = BsonSerializer.Deserialize<User>(bytes);
        // Console.WriteLine(user2.ToJson());
        // User user = new();
        // Console.WriteLine(user.ToJson());
        // string input = "这是一些文本，包含 <Numeric id=10/> <Numeric id=1230/> 这样的标记。";
        //
        // // 定义正则表达式模式
        // string pattern = @"<Numeric id=(\d+)/>";
        //
        // // 使用正则表达式获取所有匹配结果
        // MatchCollection matches = Regex.Matches(input, pattern);
        //
        // // 遍历每个匹配
        // foreach (Match match in matches)
        // {
        //     // 提取id的值
        //     string idValue = match.Groups[1].Value;
        //
        //     // 替换每个匹配的字符串为id的值
        //     input = input.Replace(match.Value, idValue);
        // }

        // Console.WriteLine("原始字符串: " + input);
        // string input = "<Numeric type=Hp test/> <Numeric <UnitConfig type=helloworld id=10/> <replace dsa123/> <1 />";
        // string replaceText = input;
        //
        // string pattern = @"<\w+\s+[^>]*\/>";
        //
        // Regex regex = new(pattern);
        //
        // MatchCollection matches = regex.Matches(input);
        //
        // foreach (Match match in matches)
        // {
        //     string replaceName = match.Value.Split(' ')[0];
        //     replaceName = replaceName.Substring(1, replaceName.Length - 1);
        //
        //     string pattern2 = @"<Numeric type=(?<type>\w+)/>";
        //     Match match2 = Regex.Match(match.Value, pattern2);
        //
        //     replaceText = input.Replace(match.Value, match2.Groups["type"].Value);
        // }
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