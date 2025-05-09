[Root]
@RootInit:
CameraInit;
# DefaultCamera
Camera: DefaultCamera;
Camera_Priority: DefaultCamera, 100;
Camera_OrthoSize: DefaultCamera, 105000;
Camera_XDamping: DefaultCamera, 25000;
Camera_YDamping: DefaultCamera, 8000;
Camera_DeadZone: DefaultCamera, 60, 20;
Camera_SoftZone: DefaultCamera, 80, 40;
Camera_Bias: DefaultCamera, 0, 50;
# TargetGroupCamera
TargetGroupCamera: TG_Camera;
Camera_Priority: TG_Camera, 11;
Camera_OrthoSize: TG_Camera, 65000;
Camera_XDamping: TG_Camera, 75000;
Camera_YDamping: TG_Camera, 8000;
Camera_DeadZone: TG_Camera, 8, 10;
Camera_SoftZone: TG_Camera, 15, 20;
Camera_Bias: TG_Camera, 0, 50;
Camera_FOV: TG_Camera, 50000, 150000;
# Follow
Camera_FollowPlayer;
return;

# UI测试
[UIBinder]
@ButtonClick:
# LogWarning: 'HelloWorld';
WaitFrame: 20;
# LogWarning: 'World';
return;

@ButtonStay:
ButtonHighLight: true;
return;

@ButtonExit:
ButtonHighLight: false;
Invoke: Test;
return;

@Test:
# LogWarning: 'HelloWorld';
return;