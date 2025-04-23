using System.Collections.Generic;
using System.Text.RegularExpressions;
using Cinemachine;
using UnityEngine;

namespace ET.Client
{
    public class CM_TargetGroupCamera_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "CM_TargetGroupCamera";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"CM_TargetGroupCamera: (?<Name>\w+);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            Unit unit = parser.GetParent<Unit>();
            GameObject _camera = unit.GetComponent<GameObjectComponent>().GameObject;
            
            //1. TargetGroup初始化
            GameObject _targetGroup = new($"TargetGroup_{match.Groups["Name"].Value}");
            GameObject cameraTarget = parser.GetParam<GameObject>("CM_CameraTarget");
            _targetGroup.transform.SetParent(_camera.transform);
            
            CinemachineTargetGroup targetGroup = _targetGroup.AddComponent<CinemachineTargetGroup>();
            targetGroup.AddMember(cameraTarget.transform, 1, 3);
            targetGroup.m_PositionMode = CinemachineTargetGroup.PositionMode.GroupAverage;
            
            //2. 添加相机
            GameObject _virtualCamera = new(match.Groups["Name"].Value);
            _virtualCamera.transform.SetParent(_camera.transform);
            CinemachineVirtualCamera vc = _virtualCamera.AddComponent<CinemachineVirtualCamera>();

            Dictionary<string, GameObject> cameraDict = parser.GetParam<Dictionary<string, GameObject>>("CM_CameraDict");
            if (!cameraDict.TryAdd(match.Groups["Name"].Value, _virtualCamera))
            {
                Log.Error($"already exist virtualCamera: {match.Groups["Name"].Value}");
                return Status.Failed;
            }

            //3. 相机设置
            vc.AddCinemachineComponent<CinemachineFramingTransposer>();
            
            // Extension
            _virtualCamera.AddComponent<CinemachineCameraOffset>();
            _virtualCamera.AddComponent<CinemachineConfiner2D>();
            
            vc.Follow = _targetGroup.transform;
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}