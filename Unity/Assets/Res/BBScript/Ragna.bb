[Root]
@RootInit:
#1. PlayerInit中挂载组件(NumericComponent、InputComponent...)
PlayerInit;
#2. 相机跟随
Camera_FollowPlayer;
SetPos: 0, -70000;
#3. 初始化对象池
PoolObject: CircleWave, 1;
#4. 添加初始Buff
HP: 10000;
SP: 200;
JustEvade: 600;
EnableJump: 2;
EnableAirCheck;
EnableGroundDash: 2, 70;
EnableAirDash: 2;
EnableGravityCheck: 100000, 150000, 450000;        
EnableHardLandCheck: 10, 450000;   
#5. 注册输入缓冲
RegistInput: RunHold;
RegistInput: SquatHold;
RegistInput: 2LPPressed;
RegistInput: 5LPPressed;
RegistInput: 5LPHold;
RegistInput: 5MPPressed;
RegistInput: 5MPPressing;
RegistInput: QuickFallPressed;
RegistInput: 5HPPressed;
RegistInput: 5HPPressing;
RegistInput: DashPressed;
RegistInput: JumpPressed;
#6. 注册动作
RegistMove: (Rg_Idle)
  MoveType: None;
EndMove:
RegistMove: (Rg_Land)
  MoveType: Move;
EndMove:
RegistMove: (Rg_Run)
  MoveType: Move;
EndMove:
RegistMove: (Rg_Squit)
  MoveType: Move;
EndMove:
RegistMove: (Rg_AirBrone)
  MoveType: Move;
EndMove:
RegistMove: (Rg_Jump)
  MoveType: Move;
EndMove:
RegistMove: (Rg_5B)
  MoveType: Normal;
  EndMove:
# RegistMove: (Rg_5C)
#   MoveType: Normal;
#   EndMove:
# RegistMove: (Rg_5D)
#   MoveType: Normal;
#   EndMove:
# RegistMove: (Rg_5BHold)
#   MoveType: Normal;
#   EndMove:
# RegistMove: (Rg_DustAttack)
#   MoveType: Special;
#   EndMove:
# RegistMove: (Rg_AirDash)
#   MoveType: Special;
#   EndMove:
RegistMove: (Rg_GroundDash)
  MoveType: Special;
  EndMove:
# RegistMove: (Rg_Super3)
#   MoveType: Special;
#   EndMove:
# RegistMove: (Rg_Test)
#   MoveType: Etc;
#   EndMove:
# RegistMove: (Rg_Turn)
#   MoveType: Etc;
# EndMove:
# RegistMove: (Rg_SquatTurn)
#   MoveType: Etc;
# EndMove:
RegistMove: (Rg_HardLand)
  MoveType: Etc;
EndMove:
RegistMove: (Rg_IdleAnim)
  MoveType: Etc;
EndMove:
RegistMove: (Rg_Hurt)
  MoveType: HitStun;
EndMove:
#8. 进入默认动作
GotoBehavior: Rg_Idle;
return;

@HPWatcher:
return;

@BeforeReloadCallback:
BeforeReload;
return;

@AfterReloadCallback:
return;

@LandCallback:
LandCallback;
return;

[Rg_Idle]
@Trigger:
return;

@Main:
SetVelocity: 0, 0;
# 设置一个待机行为，保持idle 300帧之后进入这个行为
EnableWaitFrameCallback: true, 300, Rg_Idle, IdleAnim;
# EnableRepeatedTimerCallback: true, 1, Rg_Idle, TurnCheck;
EnableDefaultCancel: true;
SetMarker: Loop;
BBSprite: Idle_1, 4;
BBSprite: Idle_2, 4;
BBSprite: Idle_3, 4;
BBSprite: Idle_4, 4;
BBSprite: Idle_5, 5;
BBSprite: Idle_6, 6;
BBSprite: Idle_7, 5;
BBSprite: Idle_8, 4;
BBSprite: Idle_9, 4;
BBSprite: Idle_10, 4;
BBSprite: Idle_11, 4;
BBSprite: Idle_12, 4;
BBSprite: Idle_13, 4;
GotoMarker: Loop;
Exit;

@IdleAnim:
GotoBehavior: Rg_IdleAnim;
return;

# @TurnCheck:
# BeginIf: (FlipChange: true)
#   GotoBehavior: Rg_Turn;
# EndIf:

[Rg_Land]
@Trigger:
Transition: AirToLand, true;
return;

@Main:
SetVelocityX: 0;
EnableDefaultCancel: true;
BeginIf: (LandVel: 400000)
  BBSprite: Land_1, 3;
  BBSprite: Land_2, 3;
  BBSprite: Land_3, 3;
EndIf:
BBSprite: Land_4, 5;
BBSprite: Land_5, 4;
Exit;


[Rg_Run]
@Trigger:
InAir: false;
InputType: RunHold;
return;

@Main:
#PreRun
EnableFlip: true;
EnableDefaultCancel: true;
BBSprite: PreRun_1, 2;
EnableMoveX: 130000, true;
BBSprite: PreRun_2, 2;
#Run
BeginLoopAnim: (InputType: RunHold)
  LoopSprite: Run_1, 4;
  LoopSprite: Run_2, 4;
  LoopSprite: Run_3, 4;
  LoopSprite: Run_4, 4;
  LoopSprite: Run_5, 4;
  LoopSprite: Run_6, 4;
EndLoopAnim:
#RunToIdle
EnableMoveX: 0, false;
SetVelocityX: 50000;
BBSprite: RunToIdle_1, 3;
BBSprite: RunToIdle_2, 3;
SetVelocityX: 0;
EnableNandemoCancel: true;
BBSprite: RunToIdle_3, 3;
BBSprite: RunToIdle_4, 3;
Exit;

[Rg_Squit]
@Trigger:
InAir: false;
InputType: SquatHold;
return;

@Main:
SetVelocityX: 0;
EnableFlip: true;
EnableDefaultCancel: true;
# PreSquat
BeginIf: (TransitionCached: NoPreSquat, false)
  BBSprite: Start_1, 2;
  BBSprite: Start_2, 2;
EndIf:
# Squatting
# EnableRepeatedTimerCallback: true, 1, Rg_Squit, SquatTurnCheck;
BeginLoopAnim: (InputType: SquatHold)
  LoopSprite: Squit_1, 4;
  LoopSprite: Squit_2, 4;
  LoopSprite: Squit_3, 4;
  LoopSprite: Squit_4, 4;
  LoopSprite: Squit_5, 4;  
  LoopSprite: Squit_6, 4;
  LoopSprite: Squit_7, 4;
  LoopSprite: Squit_6, 4;
  LoopSprite: Squit_5, 4;
  LoopSprite: Squit_4, 4;
  LoopSprite: Squit_3, 4;
  LoopSprite: Squit_2, 4;
EndLoopAnim:
# EnableRepeatedTimerCallback: false, 0, 0, 0;
# SquatToIdle
EnableNandemoCancel: true;
BBSprite: End_1, 4;
BBSprite: End_2, 4;
Exit;

# @SquatTurnCheck:
# BeginIf: (FlipChange: true)
#   GotoBehavior: Rg_SquatTurn;
# EndIf:

[Rg_AirBrone]
@Trigger:
InAir: true;
return;

@Main:
EnableHardLandCheck: 10, 350000;
EnableDefaultCancel: true;
EnableFlip: true;
Gravity: 100000;
EnableAirMoveX: 150000, true;
BBSprite: JumpToFall_5, 4;
# AirBone
BeginLoopAnim: (InAir: true)
  LoopSprite: Fall_1, 3;
  LoopSprite: Fall_2, 3;
EndLoopAnim:
# Land
BeginIf: (HardLand: true)
  GotoBehavior: Rg_HardLand;
EndIf:
BeginIf: (HardLand: false)
  SetTransition: AirToLand, true;
EndIf:
Exit;

[Rg_Jump]
@Trigger:
CanJump: true;
InputType: JumpPressed;
return;

@Main:
# LandToJump
SetVelocity: 0, 0;
BeginIf: (InAir: false)
  BBSprite: PreJump_1, 3;
  BBSprite: PreJump_2, 3;
EndIf:
# Jump
EnableFlip: true;
EnableHardLandCheck: 10, 350000;
Gravity: 0;
EnableAirMoveX: 150000, true;
SetVelocityY: 200000;
BBSprite: Jump_1, 3;
BBSprite: Jump_2, 3;
BBSprite: Jump_1, 3;
JumpAdd: -1;
EnableGatlingCancel: true;
GCOption: Rg_Jump;
Gravity: 100000;
BBSprite: Jump_2, 3;
BBSprite: Jump_1, 3;
# JumpToFall
RegistCounter: 18;
BeginLoopAnim: (InAir: true), (Counter: Value > 0)
  LoopSprite: JumpToFall_1, 3;
  LoopSprite: JumpToFall_2, 3;
  LoopSprite: JumpToFall_3, 4;
  LoopSprite: JumpToFall_4, 4;
  LoopSprite: JumpToFall_5, 4;
EndLoopAnim:
# Fall
BeginLoopAnim: (InAir: true)
  LoopSprite: Fall_1, 3;
  LoopSprite: Fall_2, 3;
EndLoopAnim:
BeginIf: (HardLand: true)
  GotoBehavior: Rg_HardLand; # 强制切换进硬直中
EndIf:
BeginIf: (HardLand: false)
  SetTransition: AirToLand, true;
EndIf:
Exit;

[Rg_5B]
@Trigger:
InputType: 5LPPressed;
InAir: false;
return;

@Main:
# Event: (Whiff_Start)
#   EnableWhiffCancel: true;
#   WhiffOption: Rg_GroundDash;
# EndEvent:
# Event: (Hit_Start)
#   # 这里开始，受击回调
#   # 对于同一对象，在持续帧内仅造成一次攻击(Repeat则为持续帧内，只要发生碰撞，每帧都会回调受击回调)
#   HitNotify: Once 
#     EnableGatlingCancel: true;
#     EnableTargetCancel: true;
#     TCOption: Rg_5C;
#     # 受击方切换到受击动作
#     HitStun: BounceHurt; 
#     HitStop: 0, 10; 
#     Shake: 800, 0, 8000, 18; 
#     HitShake: 1200, 1000, 10000, 18; 
#     HitVel: -300000, 250000;
#     EndNotify:
# EndEvent:
# Event: (Hit_End)
#   EnableGatlingCancel: false;
#   EnableTargetCancel: false;
#   EnableWhiffCancel: false;
# EndEvent:
# 注册帧事件
RegistMarkerEvent: Whiff_Start, Rg_5B, Whiff_Start;
ApplyRootMotion: true;
PlayTimeline: 0, 30;
ApplyRootMotion: false;
Exit;

@Whiff_Start:
EnableWhiffCancel: true;
WhiffOption: Rg_GroundDash;
return;

[Rg_5C]
@Trigger: 
TCOption: Rg_5C;
InputType: 5LPPressed;
InAir: false;
return;

@Main:
SetVelocityX: 0;
# 攻击持续第一帧
Event: (Hit_Start)
  # 攻击检测
  HitNotify: Once
    EnableGatlingCancel: true;
    EnableTargetCancel: true;
    TCOption: Rg_5D;
    Shake: 500, 0, 8000, 14; # 振动
    HitStop: 1, 14; # 打击停顿
  EndNotify:
EndEvent:
# 攻击持续最后一帧
Event: (Hit_End)
  EnableGatlingCancel: false;
  EnableTargetCancel: false;
EndEvent:
ApplyRootMotion: true;
PlayTimeline: 0, 48;
ApplyRootMotion: false;
Exit;

[Rg_5D]
@Trigger:
TCOption: Rg_5D;
InputType: 5LPPressed;
InAir: false;
return;

@Main:
SetVelocityX: 0;
Event: (Hit_Start)
  HitNotify: Once
    # 挂载GatlingCancel组件
    EnableGatlingCancel: true;
    Shake: 800, 0, 10000, 18;
    HitStop: 2, 18;
  EndNotify:
EndEvent:
Event: (Hit_End)
  # 销毁GatlingCancel组件
  EnableGatlingCancel: false;
EndEvent:
PlayTimeline: 0, 39;
Exit;

[Rg_5BHold]
@Trigger:
InputType: 5LPHold;
InAir: false;
return;

@Main:
SetVelocityX: 0;
BBSprite: Frame_1, 3;
BBSprite: Frame_2, 3;
BBSprite: Frame_3, 3;
BBSprite: Frame_4, 3;
BBSprite: Frame_5, 3;
BBSprite: Frame_6, 3;
BBSprite: Frame_7, 3;
BBSprite: Frame_8, 3;
BBSprite: Frame_9, 3;
BBSprite: Frame_10, 2;
# 创建Bullet
CreateBullet: DeadSpike
  BulletPos: -28000, -5000;
EndCreateBullet:
BBSprite: Frame_10, 2;
BBSprite: Frame_11, 3;
BBSprite: Frame_12, 3;
BBSprite: Frame_13, 4;
BBSprite: Frame_14, 4;
BBSprite: Frame_15, 4;
BBSprite: Frame_16, 4;
BBSprite: Frame_17, 4;
BBSprite: Frame_18, 4;
BBSprite: Frame_19, 4;
BBSprite: Frame_20, 4;
Exit;

[Rg_TC_End]
@Trigger:
TCOption: Rg_TC_End;
InputType: 5LPPressed;
InAir: false;
return;

@Main:
StartTimeline;
Exit;

[Rg_AirDashAttack]
@Trigger:
InAir: true;
InputType: 5LPPressed;
GCOption: Rg_AirDashAttack;
return;

@Main:
SetVelocityX: 10000;
SetVelocityY: 0;
Gravity: 0;
BBSprite: Attack_1, 3;
BBSprite: Attack_2, 3;
BBSprite: Attack_3, 3;
InputBuffer: true;
BBSprite: Attack_4, 3;
BBSprite: Attack_5, 3;
SetVelocityX: 70000;
BBSprite: Attack_6, 3;
BBSprite: Attack_7, 3;
SetVelocityX: 10000;
GCWindow;
GCOption: Rg_AirDashAttack;
BBSprite: Attack_8, 3;
BBSprite: Attack_9, 3;
BBSprite: Attack_10, 3;
BBSprite: Attack_11, 3;
BBSprite: Attack_12, 3;
Exit;


[Rg_6P]
# 进入行为的判定条件
@Trigger:
InAir: false;
InputType: 5LPPressed;
return;

@Main:
# 打开输入缓冲
InputBuffer: true;
SetVelocityX: 0;
BBSprite: Start_1, 3;
BBSprite: Start_2, 3;
BBSprite: Start_3, 3;
BBSprite: Start_4, 3;
BBSprite: Start_5, 3;
# 这里开始，受击回调
HitNotify: Once # 对于同一对象，在持续帧内仅造成一次攻击(Repeat则为持续帧内，只要发生碰撞，每帧都会回调受击回调)
  CancelWindow: Gatling;
  CancelOption: Rg_JumpCancel;
  Shake: 500, 0, 8000, 18; # 振动
  HitStop: 0, 18; # 打击停顿
  # 受击行为协程需要使用的变量
  HitParam: Shake_LengthX, 1200;
  HitParam: Shake_LengthY, 1000;
  HitParam: Shake_Frequency, 10000;
  HitParam: Shake_Frame, 18;
  # 受击者帧冻结(HitStop)的总帧长
  HitParam: HitStopFrame, 18;
  # HitStop结束后抛出的速度(万分制)
  HitParam: StartV_X, -3000;
  HitParam: StartV_Y, 250000;
  # 受击时调整转向
  Hit_UpdateFlip;
  # 受击者进入哪个硬直状态
  HitStun: Hurt3;
  EndNotify:
# 攻击判定的持续帧
BBSprite: Active_1, 4;
DisposeWindow;
BBSprite: Recovery_1, 3;
BBSprite: Recovery_2, 3;
BBSprite: Recovery_3, 3;
BBSprite: Recovery_4, 3;
BBSprite: Recovery_5, 3;
BBSprite: Recovery_6, 3;
BBSprite: Recovery_7, 3;
# 退出行为
Exit;

[Rg_DustAttack]
@Trigger:
InAir: false;
InputType: 5HPPressed;
return;

@Main:
SetVelocityX: 0;
BBSprite: Frame_1, 4;
BBSprite: Frame_2, 4;
# 蓄力阶段
BeginLoop: (InputType: 5HPPressing)
  BBSprite: Frame_3, 4;
  BBSprite: Frame_4, 4;
  BBSprite: Frame_5, 4;
  BBSprite: Frame_3, 4;
  BBSprite: Frame_4, 4;
  AddFlag: Charge;
  BBSprite: Frame_5, 4;
  BBSprite: Frame_3, 4;
  BBSprite: Frame_4, 4;
  BBSprite: Frame_5, 4;
  Break;
EndLoop:
BBSprite: Frame_6, 3;
BBSprite: Frame_7, 3;
BBSprite: Frame_8, 3;
# HitNotify: Once
#   BeginIf: (Flag: Charge, false)
#     # HitStun: GroundHurt;
#     HitStop: 0, 15; # 打击停顿
#     Shake: 500, 0, 8000, 15; # 振动
#     HitShake: 10000, 10000, 10000, 15;
#     # Damage: 10000;
#   EndIf:
#   BeginIf: (Flag: Charge, true)
#     # HitStun: BounceHurt;
#     HitStop: 0, 25;
#     Shake: 800, 0, 12000, 25;
#     HitShake: 1200, 1000, 10000, 25;
#     HitVel: -400000, 250000;
#   EndIf:
# EndNotify:
BBSprite: Frame_9, 4;
BBSprite: Frame_10, 4;
BBSprite: Frame_11, 4;
BBSprite: Frame_12, 4;
BBSprite: Frame_13, 4;
BBSprite: Frame_14, 4;
BBSprite: Frame_15, 4;
BBSprite: Frame_16, 4;
Exit;

[Rg_AirDash]
@Trigger:
InAir: true;
InputType: DashPressed;
CanAirDash: true;
return;

@Main:
AirDashAdd: -1;
Event: (FallEvent)
  EnableGatlingCancel: true;
  # 空中冲刺衔接冲刺
  EnableFlip: true;
  GCOption: Rg_AirDash;
  GCOption: Rg_Jump;
  # 设置冲刺惯性
  ApplyRootMotion: false;
  SetVelocityX: 80000;
EndEvent:
ApplyRootMotion: true;
PlayTimeline: 0, 24;
SetTransition: AirToLand, true;
Exit;

[Rg_GroundDash]
@Trigger:
InAir: false;
InputType: DashPressed;
CanGroundDash: true;
return;

@Main:
# In GroundDash
SetVelocity: 350000, 0;
GroundDashAdd: -1;
# 启动精准闪避窗口
EnableJustEvadeCheck: 8, 0, -5000, 45000, 47000;
RegistJustEvadeCallback: Rg_GroundDash, JustEvadeCallback;
BBSprite: Active_1, 3;
BBSprite: Active_2, 3;
EnableGatlingCancel: true;
GCOption: Rg_Jump;
BBSprite: Active_3, 3;
BBSprite: Active_1, 3;
SetVelocityX: 200000;
BBSprite: Active_2, 3;
SetVelocityX: 50000;
# End
BBSprite: End_1, 6;
BBSprite: End_2, 3;
SetVelocityX: 0;
BBSprite: End_3, 3;
SetTransition: NoPreSquat, true;
EnableNandemoCancel: true;
BBSprite: End_3, 2;
BBSprite: End_4, 3;
BBSprite: End_5, 3;
Exit;

@JustEvadeCallback:
TimeFroze: 20, 5;
CreateEffect: CircleWave
  CreateEffect_Scale: 70000, 70000;
  CreateEffect_LocalPosition: -6000, -10000;
EndCreateEffect:
TacticalTime: 300, 10;
Invincible: 300;
return;

[Rg_PlungingAttack]
@Trigger:
InAir: true;
InputType: 2LPPressed;
return;

@Main:
Gravity: 0;
# PreAttack
ApplyRootMotion: true;
PlayTimeline: 0, 16;
ApplyRootMotion: false;
#Attack
SetVelocityX: 0;
SetVelocityY: -600000;
Test;
BBSprite: Attack_1, 2;
BeginLoop: (InAir: true)
  BBSprite: Attack_2, 3;
  BBSprite: Attack_3, 3;
EndLoop:
#Recovery
BBSprite: Recovery_1, 4;
BBSprite: Recovery_2, 4;
BBSprite: Recovery_3, 4;
BBSprite: Recovery_4, 4;
BBSprite: Recovery_5, 4;
BBSprite: Recovery_6, 4;
BBSprite: Recovery_7, 4;
Exit;

[Rg_QuickFall]
@Trigger:
InAir: true;
InputType: QuickFallPressed;
return;

@Main:
Gravity: 0;
SetVelocityX: 0;
SetVelocityY: -3000000;
BeginLoop: (InAir: true)
  BBSprite: Fall_1, 3;
EndLoop:
BBSprite: Land_1, 10;
BBSprite: Land_2, 4;
BBSprite: Land_3, 4;
#ToSquat
SetTransition: NoPreSquat, true;
InputBuffer: true;
CancelWindow: Transition;
BBSprite: Land_3, 2;
BBSprite: Land_4, 4;
BBSprite: Land_5, 4;
BBSprite: Land_6, 4;
Exit;

[Rg_24D]
@Trigger:
InputType: 5MPPressed; 
return;

@Main:
InputBuffer: true;
# Start
BeginIf: (InAir: false)
  SetVelocityX: 0;
  BBSprite: Start_1, 2;
  BBSprite: Start_2, 2;
  BBSprite: Start_3, 2;
  EndIf:
SetVelocityX: 180000;
SetVelocityY: 170000;
Gravity: 0;
BBSprite: Start_5, 4;
Gravity: 100000;
BBSprite: Start_5, 4;
# Active
CancelWindow: Gatling;
CancelOption: Rg_AirDash;
CancelOption: Rg_Jump;
HitNotify: Once
  HitStop: 0, 8; # 打击停顿
  Shake: 500, 0, 8000, 8; # 振动
  # 受击行为协程需要使用的变量
  HitParam: Shake_LengthX, 1200;
  HitParam: Shake_LengthY, 1000;
  HitParam: Shake_Frequency, 10000;
  HitParam: Shake_Frame, 15;
  # 受击者帧冻结(HitStop)的总帧长
  HitParam: HitStopFrame, 15;
  # HitStop结束后抛出的速度
  HitParam: Push_V, -200000;
  HitParam: Push_F, 800000;
  # 受击时调整转向
  Hit_UpdateFlip;
  # 受击者进入哪个硬直状态
  HitStun: Hurt4;
  EndNotify:
BBSprite: Active_1, 3;
BBSprite: Active_2, 3;
BBSprite: Active_3, 3;
BBSprite: Recover_1, 4;
# Recover
BBSprite: Recover_2, 3;
CancelOption: Rg_26C;
CancelOption: Rg_24D_Derive;
BBSprite: Recover_3, 3;
BBSprite: Recover_4, 3;
BeginLoop: (InAir: true)
  BBSprite: Recover_5, 1;
EndLoop:
DisposeWindow;
SetVelocityX: 0;
BBSprite: Recover_6, 4;
BBSprite: Recover_7, 4;
BBSprite: Recover_8, 4;
BBSprite: Recover_9, 4;
Exit;

[Rg_24D_Derive]
@Trigger:
CancelOption: Rg_24D_Derive;
InputType: 5MPPressed;
return;

@Main:
# Derive_Start
SetVelocityX: 80000;
SetVelocityY: 120000;
Gravity: 0;
BBSprite: Start2_1, 2;
# Derive_Active
HitNotify: Once
  HitStop: 5, 15; # 打击停顿
  Shake: 500, 0, 8000, 15; # 振动
  # 受击行为协程需要使用的变量
  HitParam: Shake_LengthX, 1200;
  HitParam: Shake_LengthY, 1000;
  HitParam: Shake_Frequency, 10000;
  HitParam: Shake_Frame, 15;
  # 受击者帧冻结(HitStop)的总帧长
  HitParam: HitStopFrame, 15;
  # HitStop结束后抛出的速度
  HitParam: StartV_X, -18000;
  HitParam: StartV_Y, 300000;
  # 受击时调整转向
  Hit_UpdateFlip;
  # 受击者进入哪个硬直状态
  HitStun: Hurt3;
  EndNotify:
BBSprite: Active2_1, 2;
Gravity: 100000;
BBSprite: Active2_1, 3;
BBSprite: Active2_2, 3;
# Derive_Recover
BBSprite: Recover2_1, 3;
BBSprite: Recover_4, 3;
BeginLoop: (InAir: true)
  BBSprite: Recover_5, 1;
EndLoop:
SetVelocityX: 0;
BBSprite: Recover_6, 4;
BBSprite: Recover_7, 4;
BBSprite: Recover_8, 4;
BBSprite: Recover_9, 4;
Exit;

[Rg_24A]
@Trigger:
InputType: 5MPPressed;
InAir: false;
return;

@Main:
InputBuffer: true;
BBSprite: Start_3, 4;
SetVelocityX: 200000;
BBSprite: Start_4, 3;
BBSprite: Start_5, 3;
SetVelocityX: 100000;
HitNotify: Once
  HitStop: 0, 30; # 打击停顿
  Shake: 500, 0, 8000, 23; # 振动
  # 受击行为协程需要使用的变量
  HitParam: Shake_LengthX, 1100;
  HitParam: Shake_LengthY, 1100;
  HitParam: Shake_Frequency, 12000;
  HitParam: Shake_Frame, 23;
  # 受击者帧冻结(HitStop)的总帧长
  HitParam: HitStopFrame, 28;
  HitParam: StartV_X, -500000;
  HitParam: StartV_Y, 140000;
  # 受击时调整转向
  Hit_UpdateFlip;
  # 受击者进入哪个硬直状态
  HitStun: Hurt5;
  EndNotify:
BBSprite: Active_1, 5;
SetVelocityX: 50000;
BBSprite: Active_2, 3;
BBSprite: Active_2, 2;
SetVelocityX: 0;
BBSprite: Active_1, 5;
BBSprite: Recover_1, 4;
BBSprite: Recover_2, 4;
BBSprite: Recover_3, 3;
BBSprite: Recover_4, 3;
BBSprite: Recover_5, 3;
Exit;

[Rg_26C]
@Trigger:
CancelOption: Rg_26C;
InputType: 5LPPressed;
InAir: true;
return;

@Main:
SetVelocityX: 0;
SetVelocityY: 0;
Gravity: 0;
BBSprite: Start_1, 2;
BBSprite: Start_2, 2;
BBSprite: Start_3, 2;
BBSprite: Start_4, 2;
BBSprite: Start_5, 2;
BBSprite: Active_1, 4;
BBSprite: Active_2, 4;
BBSprite: Recover_1, 3;
BBSprite: Recover_2, 3;
Gravity: 100000;
BBSprite: Recover_3, 3;
BBSprite: Recover_4, 3;
Exit;

[Rg_Super]
@Trigger:
InputType: 2LPPressed;
return;

@Main:
SetVelocityX: 0;
TimeFrozeCheckBox: 0, 20000, 250000, 100000, 1, 55;
BBSprite: Frame_1, 4;
RegistCounter: Cnt_1, 40;
BeginLoop: (Counter: Cnt_1 > 0)
  BBSprite: Frame_2, 4;
  BBSprite: Frame_3, 4;
  BBSprite: Frame_4, 4;
EndLoop:
# Hit_0
HitNotify: Once
  AddFlag: Hit; 
EndNotify:
BBSprite: Frame_5, 3;
SetVelocityX: 240000;
BBSprite: Frame_6, 3;
SetVelocityX: 150000;
BBSprite: Frame_7, 4;
SetVelocityX: 70000;
BBSprite: Frame_7, 4;
SetVelocityX: 0;
BBSprite: Frame_7, 5;
BBSprite: Frame_8, 8;
# 挥空
BeginIf: (Flag: Hit, false)
  BBSprite: Frame_54, 4;
  BBSprite: Frame_55, 4;
  BBSprite: Frame_52, 4;
  BBSprite: Frame_53, 4;
  Exit;
EndIf:
# 命中
BBSprite: Frame_9, 4;
BBSprite: Frame_10, 4;
BBSprite: Frame_11, 4;
BBSprite: Frame_12, 4;
BBSprite: Frame_13, 4;
BBSprite: Frame_14, 4;
BBSprite: Frame_15, 4;
BBSprite: Frame_16, 4;
BBSprite: Frame_17, 4;
BBSprite: Frame_18, 4;
BBSprite: Frame_19, 4;
BBSprite: Frame_20, 4;
BBSprite: Frame_21, 4;
BBSprite: Frame_22, 4;
# Hit1
HitNotify: Once
  HitStop: 0, 6;
EndNotify:
BBSprite: Frame_23, 4;
BBSprite: Frame_24, 4;
BBSprite: Frame_25, 4;
# Hit2
HitNotify: Once
  HitStop: 0, 6;
EndNotify:
BBSprite: Frame_26, 4;
BBSprite: Frame_27, 4;
BBSprite: Frame_28, 4;
BBSprite: Frame_29, 4;
# Hit_3
HitNotify: Once
  HitStop: 0, 6;
EndNotify:
BBSprite: Frame_30, 4;
BBSprite: Frame_31, 4;
BBSprite: Frame_32, 4;
BBSprite: Frame_33, 4;
BBSprite: Frame_34, 4;
# Hit_4
HitNotify: Once
  HitStop: 0, 6;
EndNotify:
BBSprite: Frame_35, 4;
BBSprite: Frame_36, 4;
BBSprite: Frame_37, 4;
BBSprite: Frame_38, 4;
BBSprite: Frame_39, 4;
BBSprite: Frame_40, 4;
BBSprite: Frame_41, 4;
BBSprite: Frame_42, 4;
BBSprite: Frame_43, 4;
BBSprite: Frame_44, 4;
BBSprite: Frame_45, 4;
BBSprite: Frame_46, 4;
BBSprite: Frame_47, 4;
# Hit_5
HitNotify: Once
  HitStop: 0, 6;
EndNotify:
BBSprite: Frame_48, 4;
BBSprite: Frame_49, 8;
BBSprite: Frame_50, 5;
BBSprite: Frame_51, 4;
BBSprite: Frame_52, 4;
BBSprite: Frame_53, 4;
Exit;

[Rg_Super2]
@Trigger:
InputType: 5MPPressed;
return;

@Main:
SetVelocityX: 0;
BBSprite: Frame_1, 4;
BBSprite: Frame_2, 4;
TimeFrozeCheckBox: 0, 20000, 250000, 100000, 0, 70;
BBSprite: Frame_3, 4;
BBSprite: Frame_4, 5;
ScreenShake: 0, 650, 10000, 60;
BBSprite: Frame_4, 60;
BBSprite: Frame_5, 4;
HitNotify: Once
  HitStop: 0, 5; # 打击停顿
  Shake: 500, 0, 8000, 8; # 振动
  # 受击行为协程需要使用的变量
  HitParam: Shake_LengthX, 1200;
  HitParam: Shake_LengthY, 1000;
  HitParam: Shake_Frequency, 10000;
  HitParam: Shake_Frame, 5;
  # 受击者帧冻结(HitStop)的总帧长
  HitParam: HitStopFrame, 5;
  HitParam: LastFrame, 60;
  # HitStop结束后抛出的速度
  HitParam: Push_V, 0;
  HitParam: Push_F, 0;
  # 受击时调整转向
  Hit_UpdateFlip;
  # 受击者进入哪个硬直状态
  HitStun: Hurt4;
  AddFlag: Hit;
  TargetBind;
  TargetPoint: 29000, 0;
EndNotify:
BBSprite: Frame_6, 6;
# 挥空
BeginIf: (Flag: Hit, false)
  BBSprite: Frame_33, 5;
  BBSprite: Frame_34, 4;
  BBSprite: Frame_35, 4;
  BBSprite: Frame_36, 3;
  BBSprite: Frame_37, 3;
  BBSprite: Frame_38, 3;
  BBSprite: Frame_39, 3;
  BBSprite: Frame_40, 3;
  BBSprite: Frame_41, 3;
  BBSprite: Frame_42, 3;
  Exit;
EndIf:
BBSprite: Frame_7, 4;
BBSprite: Frame_8, 4;
BBSprite: Frame_9, 4;
BBSprite: Frame_10, 4;
BBSprite: Frame_12, 4;
HitNotify: Once
  HitParam: Shake_LengthX, 0;
  HitParam: Shake_LengthY, 0;
  HitParam: Shake_Frequency, 1;
  HitParam: Shake_Frame, 1;
  HitStun: ThrowHurt;
  TargetBind;
  TargetPoint: 27000, 2000;
EndNotify:
BBSprite: Frame_13, 5;
TargetPoint: 16000, 11000;
BBSprite: Frame_14, 3;
BBSprite: Frame_15, 3;
BBSprite: Frame_16, 3;
BBSprite: Frame_17, 3;
BBSprite: Frame_18, 3;
BBSprite: Frame_19, 3;
BBSprite: Frame_20, 3;
HitNotify: Once
  Shake: 500, 0, 8000, 18; # 振动
  HitStop: 0, 18; # 打击停顿
  # 受击行为协程需要使用的变量
  HitParam: Shake_LengthX, 2500;
  HitParam: Shake_LengthY, 2500;
  HitParam: Shake_Frequency, 10000;
  HitParam: Shake_Frame, 18;
  # 受击者帧冻结(HitStop)的总帧长
  HitParam: HitStopFrame, 18;
  # HitStop结束后抛出的速度
  HitParam: StartV_X, -80000;
  HitParam: StartV_Y, 250000;
  # 受击时调整转向
  Hit_UpdateFlip;
  # 受击者进入哪个硬直状态
  HitStun: Hurt3;
EndNotify:
BBSprite: Frame_21, 4;
BBSprite: Frame_22, 4;
BBSprite: Frame_23, 4;
BBSprite: Frame_24, 4;
BBSprite: Frame_25, 4;
BBSprite: Frame_26, 4;
EnableNandemoCancel: true;
BBSprite: Frame_27, 4;
BBSprite: Frame_28, 4;
BBSprite: Frame_29, 4;
BBSprite: Frame_30, 4;
BBSprite: Frame_31, 4;
Exit;

[Rg_Super3]
@Trigger:
InAir: false;
InputType: 5MPPressed;
return;

@Main:
SetVelocityX: 0;
BBSprite: Frame_1, 4;
BBSprite: Frame_2, 4;
BBSprite: Frame_3, 4;
RegistCounter:  30;
BeginLoop: (InputType: 5MPPressing), (Counter: Value > 0)
  BBSprite: Frame_4, 4;
EndLoop:
BBSprite: Frame_5, 4;
BBSprite: Frame_6, 4;
BBSprite: Frame_33, 4;
BBSprite: Frame_34, 4;
BBSprite: Frame_35, 4;
BBSprite: Frame_36, 4;
BBSprite: Frame_37, 4;
BBSprite: Frame_38, 4;
BBSprite: Frame_39, 4;
EnableNandemoCancel: true;
BBSprite: Frame_40, 4;
BBSprite: Frame_41, 4;
BBSprite: Frame_42, 2;
Exit;

[Rg_HardLand]
@Main:
SetVelocityX: 0;
ScreenShake: 1200, 1200, 11000, 20, 0;
BBSprite: Land_1, 2;
BBSprite: Land_2, 3;
BBSprite: Land_3, 20;
BBSprite: Land_4, 4;
BBSprite: Land_5, 4;
Exit;

[Rg_IdleAnim]
@Main:
EnableNandemoCancel: true;
PlayTimeline: 0, 81;
Exit;

[Rg_Hurt]
@Main:
SetVelocity: 0, 0;
Gravity: 0;
Shake: 600, 600, 12000, 20, 0;
BBSprite: Air_1, 20;
SetVelocity: -30000, 180000;
BBSprite: Air_2, 3;
BBSprite: Air_3, 3;
BBSprite: Air_4, 2;
Gravity: 150000;
BBSprite: Air_4, 2;
BBSprite: Air_5, 3;
BeginLoopAnim: (InAir: true)
  LoopSprite: Air_6, 5;
  LoopSprite: Air_7, 5;
EndLoopAnim:
ScreenShake: 800, 800, 10000, 20, 0;
SetVelocity: -30000, 200000;
Shake: 500, 500, 12000, 20, 0;
BBSprite: Land_1, 5;
BBSprite: Land_2, 5;
BBSprite: Land_3, 5;
SetVelocityX: 0;
BBSprite: Land_4, 5;
BBSprite: Land_5, 5;
BBSprite: Land_6, 5;
BBSprite: Land_7, 5;
BBSprite: Land_8, 100;
Exit;