[Root]
@RootInit:
CM_Init;
# DefaultCamera
CM_2DCamera: DefaultCamera;
CM_Priority: DefaultCamera, 100;
CM_OrthoSize: DefaultCamera, 65000;
CM_XDamping: DefaultCamera, 25000;
CM_YDamping: DefaultCamera, 8000;
CM_DeadZone: DefaultCamera, 60, 20;
CM_SoftZone: DefaultCamera, 80, 40;
CM_Bias: DefaultCamera, 0, 50;
# TargetGroupCamera
CM_TargetGroupCamera: TG_Camera;
CM_Priority: TG_Camera, 11;
CM_OrthoSize: TG_Camera, 65000;
CM_XDamping: TG_Camera, 75000;
CM_YDamping: TG_Camera, 8000;
CM_DeadZone: TG_Camera, 8, 10;
CM_SoftZone: TG_Camera, 15, 20;
CM_Bias: TG_Camera, 0, 50;
CM_TargetGroup_FOV: TG_Camera, 50000, 150000;
# Follow
CM_FollowPlayer;
return;


[DefaultCamera]
@RootInit:
CM_Priority: DefaultCamera, 100;
return;