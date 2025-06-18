using System.IO;
using System.Text.RegularExpressions;
using Timeline;
using UnityEngine;

namespace ET.Client
{
    [Invoke]
    [FriendOf(typeof(InjectorComponent))]
    public class HandleInjectFunctionCallback : AInvokeHandler<InjectFunctionCallback>
    {
        public override void Handle(InjectFunctionCallback args)
        {
            Unit unit = Root.Instance.Get(args.instanceId) as Unit;
            unit.RemoveComponent<InjectorComponent>();
            InjectorComponent injector = unit.AddComponent<InjectorComponent>();

            //1. 读取textAsset
            TextAsset asset = unit.GetComponent<GameObjectComponent>().GameObject.GetComponent<Injector>().script;
            string text = File.ReadAllText(asset.text);
            if (string.IsNullOrEmpty(text))
            {
                Log.Error($"cannot format bbScript!!");
                return;
            }
            
            //2. 解析Script
            string[] opLines = text.Split('\n');
            int pointer = 0;
            for (int i = 0; i < opLines.Length; i++)
            {
                string op = opLines[i].Trim();
                if (string.IsNullOrEmpty(op) || op.StartsWith('#'))
                {
                    continue;
                }
                injector.OpDict[pointer++] = op;
            }

            //3. 缓存代码块头指针
            pointer = 0;
            while (pointer < injector.OpDict.Count)
            {
                string opLine = injector.OpDict[pointer];
                string pattern = @"\[(.*?)\]";
                Match match = Regex.Match(opLine, pattern);
                if (match.Success)
                {
                    injector.GroupPointerSet.Add(pointer);
                }
                pointer++;
            }

            //4. 缓存代码块中函数、marker头指针
            foreach (int index in injector.GroupPointerSet)
            {
                DataGroup group = DataGroup.Create();
                // groupName
                string opLine = injector.OpDict[index];
                string pattern = @"\[(.*?)\]";
                Match match = Regex.Match(opLine, pattern);
                group.groupName = match.Groups[1].Value;

                // groupStartIndex
                group.startIndex = index;
                // group Function Marker
                pointer = index + 1;
                while (pointer < injector.OpDict.Count)
                {
                    //执行超出代码块
                    if (injector.GroupPointerSet.Contains(pointer)) break;
                    string _opLine = injector.OpDict[pointer];
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
                injector.GroupDict.TryAdd(group.groupName, group);
            }

            //5. 调用入口函数
            injector.Invoke(injector.GetFunctionPointer("Root", "Entry"), injector.token).Coroutine();
        }
    }
}