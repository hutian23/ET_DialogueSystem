[Root]
@RootInit:
DummyInit;
SetPos: 300000, -90000;
# Cinemachine
Camera_TargetGroupMember: TG_Camera, 110, 250;
# 添加初始buff
EnableGravityCheck: 100000, 150000, 450000;             
EnableAirCheck: 0, -1850, 1250, 1000; 
# Move
RegistMove: (Dummy_Idle)
  MoveType: None;
  EndMove:
RegistMove: (Dummy_Hurt2)
  MoveType: HitStun;
  MoveFlag: GroundHurt;
  EndMove:
RegistMove: (Dummy_Hurt5)
  MoveType: HitStun;
  MoveFlag: BounceHurt;
  EndMove:
GotoBehavior: Dummy_Idle;
return;

@LandCallback:
return;

[Dummy_Idle]
@Trigger:
return;

@Main:
SetVelocityX: 0;
SetVelocityY: -1000;
SetMarker: Loop;
BBSprite: Idle_1, 4;
BBSprite: Idle_2, 4;
BBSprite: Idle_3, 4;
BBSprite: Idle_4, 4;
BBSprite: Idle_5, 5;
BBSprite: Idle_6, 5;
BBSprite: Idle_7, 6;
BBSprite: Idle_8, 4;
BBSprite: Idle_9, 4;
BBSprite: Idle_10, 4;
BBSprite: Idle_11, 4;
BBSprite: Idle_12, 4;
GotoMarker: Loop;
Exit;

# 地面受击行为
[Dummy_Hurt2]
@Main:
BBSprite: hurt_1, 2;
BBSprite: hurt_3, 2;
BBSprite: hurt_5, 6;
BBSprite: hurt_4, 3;
BBSprite: hurt_3, 3;
BBSprite: hurt_2, 3;
BBSprite: hurt_1, 3;
Exit;

[Dummy_Hurt3]
@Main:
Shake: {Self.Shake_LengthX}, {Self.Shake_LengthY}, {Self.Shake_Frequency}, {Self.Shake_Frame};
SetVelocityX: 0;
SetVelocityY: 0;
Gravity: 0;
# 帧冻结
# HitStop: 0, {Self.HitStopFrame};
HitStop: 0, 20;
BBSprite: Frame_1, 1;
# 击飞效果
SetVelocityX: {Self.StartV_X};
SetVelocityY: {Self.StartV_Y};
BBSprite: Frame_2, 3;
BBSprite: Frame_3, 3;
Gravity: 60000;
BBSprite: Frame_2, 3;
BBSprite: Frame_3, 3;
BBSprite: Frame_2, 3;
BBSprite: Frame_3, 3;
Gravity: 120000;
# 上升
BeginLoop: (Velocity: Y > 150000)
  BBSprite: Frame_2, 3;
  BBSprite: Frame_3, 3;
  EndLoop:
# 开始下落
BBSprite: Frame_4, 3;
BBSprite: Frame_5, 3;
BBSprite: Frame_6, 3;
BBSprite: Frame_7, 3;
# 下落
BeginLoop: (InAir: true)
  BBSprite: Frame_8, 3;
  BBSprite: Frame_9, 3;
  EndLoop:
SetVelocityX: 0;
BBSprite: Frame_10, 2;
ScreenShake: 750, 750, 18000, 10; # 落地之后模拟弹地效果
BBSprite: Frame_10, 1;
BBSprite: Frame_11, 3;
BBSprite: Frame_12, 3;
BBSprite: Frame_13, 3;
BBSprite: Frame_14, 3;
BBSprite: Frame_15, 3;
BBSprite: Frame_16, 3;
BBSprite: Frame_17, 25;
BBSprite: Frame_18, 3;
BBSprite: Frame_19, 3;
BBSprite: Frame_20, 3;
BBSprite: Frame_21, 3;
BBSprite: Frame_22, 3;
BBSprite: Frame_23, 3;
BBSprite: Frame_24, 3;
BBSprite: Frame_25, 3;
BBSprite: Frame_26, 3;
BBSprite: Frame_27, 3;
Exit;

[Dummy_Hurt4]
@Main:
SetVelocityX: 0;
Gravity: 100000;
Shake: {Self.Shake_LengthX}, 0, {Self.Shake_Frequency}, {Self.Shake_Frame};
HitStop: 0, {Self.HitStopFrame};
BBSprite: Hurt_1, 1;
# PushBack: {Self.Push_V}, {Self.Push_F};
BBSprite: Hurt_2, 2;
BBSprite: Hurt_3, 2;
BBSprite: Hurt_4, 2;
BBSprite: Hurt_5, 2;
BBSprite: Hurt_6, 2;
BBSprite: Hurt_7, {Self.LastFrame};
BBSprite: Recover_1, 3;
BBSprite: Recover_2, 3;
BBSprite: Recover_3, 3;
Exit;

[Dummy_Hurt5]
@Main:
Gravity: 0;
EnableBounceCheck: 6000, -6000, 6500, 30000, true;
BBSprite: HitStop_1, 3;
BBSprite: WaitBounce_2, 4;
Gravity: 120000;
BeginLoop: (InAir: true), (Flag: Bounce, false)
  BBSprite: WaitBounce_1, 4;
  BBSprite: WaitBounce_2, 4;
EndLoop:
EnableBounceCheck: 0, 0, 0, 0, false;
# 上墙
SetVelocityX: 0;
SetVelocityY: 0;
Gravity: 0;
ScreenShake: 800, 800, 20000, 10; # 弹墙屏幕振动
BBSprite: Bounce_1, 5;
BBSprite: Bounce_2, 5;
# 弹墙
SetVelocityX: 200000;
SetVelocityY: 70000;
BBSprite: Fall_1, 3;
Gravity: 80000;
BBSprite: Fall_2, 3;
BeginLoop: (InAir: true)
  BBSprite: Fall_3, 3;
  BBSprite: Fall_2, 3;
EndLoop:
ScreenShake: 750, 750, 18000, 10; # 落地之后模拟弹地效果
# 弹地
SetVelocityX: 50000;
SetVelocityY: 60000;
Gravity: 0;
BBSprite: Land_2, 3;
Gravity: 100000;
BBSprite: Land_3, 3;
BBSprite: Land_4, 3;
BBSprite: Land_5, 3;
SetVelocityX: 30000;
BBSprite: Land_6, 3;
BBSprite: Land_7, 3;
SetVelocityX: 0;
BBSprite: Land_8, 50;
BBSprite: Land_9, 3;
BBSprite: Land_10, 3;
BBSprite: Land_11, 3;
BBSprite: Land_12, 3;
BBSprite: Land_13, 3;
BBSprite: Land_14, 3;
BBSprite: Land_15, 3;
BBSprite: Land_16, 3;
BBSprite: Land_17, 3;
BBSprite: Land_18, 3;
BBSprite: Land_19, 3;
BBSprite: Land_20, 3;
Exit;

[Dummy_ThrowHurt]
@Main:
SetVelocityX: 0;
SetVelocityY: 0;
Gravity: 0;
Shake: {Self.Shake_LengthX}, {Self.Shake_LengthY}, {Self.Shake_Frequency}, {Self.Shake_Frame};
BBSprite: Throw_1, 10000;
Exit;