using System.IO;
using System.Text.RegularExpressions;
using Timeline;

namespace ET.Client
{
    [Invoke]
    [FriendOf(typeof(BBParser))]
    public class HandleProcessBBScriptCallback : AInvokeHandler<ProcessBBScriptCallback>
    {
        public override void Handle(ProcessBBScriptCallback args)
        {
            //1. 查询组件
            if (Root.Instance.Get(args.instanceId) is not BBParser bbParser) return;
            Unit unit = bbParser.GetParent<Unit>();
            BBScript bbScript = unit.GetComponent<GameObjectComponent>().GameObject.GetComponent<BBScript>();

            // 编辑器阶段方便测试
            Injector injector = bbScript.GetComponent<Injector>() ?? bbScript.gameObject.AddComponent<Injector>();
            injector.hertz = 60;
            
            //2. 初始化
            bbParser.Init();

            //3. 解析bbScript  TODO 这里还没想好打包后如何解析BBScript
            // string script = Define.IsEditor ? File.ReadAllText(bbScript.Script.GetPath()) : string.Empty;
            string script = bbScript.Script.text;
            
            if (string.IsNullOrEmpty(script))
            {
                Log.Error($"cannot format bbScript!!");
                return;
            }
            
            string[] opLines = script.Split('\n');
            int pointer = 0;
            for (int i = 0; i < opLines.Length; i++)
            {
                string op = opLines[i].Trim();
                if (string.IsNullOrEmpty(op) || op.StartsWith('#'))
                {
                    continue;
                }
                bbParser.OpDict[pointer++] = op;
            }
            
            //4. 缓存代码块头指针
            pointer = 0;
            while (pointer < bbParser.OpDict.Count)
            {
                string opLine = bbParser.OpDict[pointer];
                string pattern = @"\[(.*?)\]";
                Match match = Regex.Match(opLine, pattern);
                if (match.Success)
                {
                    bbParser.GroupPointerSet.Add(pointer);
                }
                pointer++;
            }
            
            //5. 缓存代码块中函数、marker头指针
            foreach (int index in bbParser.GroupPointerSet)
            {
                DataGroup group = DataGroup.Create();
                // groupName
                string opLine = bbParser.OpDict[index];
                string pattern = @"\[(.*?)\]";
                Match match = Regex.Match(opLine, pattern);
                group.groupName = match.Groups[1].Value;

                // groupStartIndex
                group.startIndex = index;
                // group Function Marker
                pointer = index + 1;
                while (pointer < bbParser.OpDict.Count)
                {
                    //执行超出代码块
                    if (bbParser.GroupPointerSet.Contains(pointer)) break;
                    string _opLine = bbParser.OpDict[pointer];
                    //匹配函数指针
                    string _pattern = "@([^:]+)";
                    Match _match = Regex.Match(_opLine, _pattern);
                    if (_match.Success)
                    {
                        group.funcPointers.TryAdd(_match.Groups[1].Value, pointer);
                    }

                    //匹配Marker指针
                    string _pattern2 = @"SetMarker:\s+(\w+)";
                    Match _match2 = Regex.Match(_opLine, _pattern2);
                    if (_match2.Success)
                    {
                        group.markerPointers.TryAdd(_match2.Groups[1].Value, pointer);
                    }
                    pointer++;
                }
                // group endIndex
                group.endIndex = pointer;
                bbParser.GroupDict.TryAdd(group.groupName, group);
            }
            
            //fin. 执行入口函数
            bbParser.Invoke(bbParser.GetFunctionPointer("Root","RootInit"), bbParser.CancellationToken).Coroutine();
        }
    }
}