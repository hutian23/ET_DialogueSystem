using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

static class Program
{
    public class User
    {
        public int id = 100;
        public int TreeID = 10;
        public int targetID = 10;
    }

    public class User2: User
    {
        public int test = 10;
    }

    public class UserSerializer: SerializerBase<User>
    {
        public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, User value)
        {
            context.Writer.WriteStartDocument();
            context.Writer.WriteName("Id");
            context.Writer.WriteInt32(300);
            context.Writer.WriteName("treeId");
            context.Writer.WriteInt32(100);
            

            context.Writer.WriteEndDocument();
        }
    }

    public static void Main()
    {
        BsonClassMap.LookupClassMap(typeof (User));
        BsonSerializer.RegisterSerializer(typeof (User), new UserSerializer());
        
        User user = new();
        Console.WriteLine(user.ToJson());
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