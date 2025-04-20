[Root]
@RootInit:
#1. PlayerInit中挂载组件(NumericComponent、InputComponent...)
PlayerInit;
SetPos: 280000, -90000;
#2. 注册数值
NumericType: Hertz, 60;
#3. 注册数值更新事件
NumericChange: Hertz
  UpdateHertz;
EndNumericChange:
#4. 添加初始Buff
EnableJump: 2;
EnableGroundDash: 2, 120;
EnableAirDash: 2;
EnableGravityCheck: 100000, 150000, 450000;             
EnableAirCheck: 0, -1850, 1250, 1000; 
#5. 注册输入缓冲
RegistInput: RunHold;
RegistInput: SquatHold;
RegistInput: 2LPPressed;
RegistInput: 5LPPressed;
RegistInput: 5LPHold;
RegistInput: 5MPPressed;
RegistInput: 5MPPressing;
RegistInput: 5HPPressed;
RegistInput: 5HPPressing;
RegistInput: DashPressed;
RegistInput: JumpPressed;
RegistInput: QuickFallPressed;
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
RegistMove: (Rg_5C)
  MoveType: Normal;
  EndMove:
RegistMove: (Rg_5D)
  MoveType: Normal;
  EndMove:
RegistMove: (Rg_5BHold)
  MoveType: Normal;
  EndMove:
RegistMove: (Rg_DustAttack)
  MoveType: Special;
  EndMove:
RegistMove: (Rg_AirDash)
  MoveType: Special;
  EndMove:
RegistMove: (Rg_GroundDash)
  MoveType: Special;
  EndMove:
RegistMove: (Rg_Super3)
  MoveType: Special;
  EndMove:
# RegistMove: (Rg_Super2)
#   MoveType: Special;
#   EndMove:
# RegistMove: (Rg_PlungingAttack)
#   MoveType: Special;
#   EndMove:
RegistMove: (Rg_Test)
  MoveType: Etc;
  EndMove:
RegistMove: (Rg_IdleAnim)
  MoveType: Etc;
  EndMove:
#7. bullet池化
PoolObject: DeadSpike, 3;
#8. 进入默认动作
GotoBehavior: Rg_Idle;
return;

@BeforeReloadCallback:
BeforeReload;
return;

@AfterReloadCallback:
return;

@LandCallback:
LandCallback;
# SetVelocityY: -20000;
# Gravity: 0;
return;

[Rg_Idle]
@Trigger:
return;

@Main:
SetVelocityX: 0;
# 设置一个待机行为，保持idle 300帧之后进入这个行为
IdleAnim: Rg_IdleAnim, 300;
EnableDefaultCancel: true;
EnableFlip: true;
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

[Rg_Land]
@Trigger:
Transition: AirToLand, true;
return;

@Main:
SetVelocityX: 0;
EnableDefaultCancel: true;
BeginIf: (LandVelocity: 400000)
  BBSprite: MiddleLand_1, 3;
  BBSprite: MiddleLand_2, 3;
EndIf:
BBSprite: MiddleLand_3, 5;
BBSprite: MiddleLand_4, 4;
BBSprite: MiddleLand_5, 4;
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
MoveX: 130000;
BBSprite: PreRun_1, 2;
BBSprite: PreRun_2, 2;
#Run
BeginLoop: (InputType: RunHold)
  BBSprite: Run_1, 4;
  BBSprite: Run_2, 4;
  BBSprite: Run_3, 4;
  BBSprite: Run_4, 4;
  BBSprite: Run_5, 4;
  BBSprite: Run_6, 4;
  EndLoop:
#RunToIdle
CancelMoveX;
SetVelocityX: 50000;
BBSprite: RunToIdle_1, 3;
BBSprite: RunToIdle_2, 3;
SetVelocityX: 0;
BBSprite: RunToIdle_3, 3;
BBSprite: RunToIdle_4, 3;
Exit;


[Rg_Squit]
@Trigger:
InAir: false;
InputType: SquatHold;
return;

@Main:
Test;
SetVelocityX: 0;
EnableFlip: true;
EnableDefaultCancel: true;
# PreSquat
BeginIf: (TransitionCached: NoPreSquat, false)
  BBSprite: PreSquit_1, 2;
  BBSprite: PreSquit_2, 2;
EndIf:
# Squatting
BeginLoop: (InputType: SquatHold)
  BBSprite: Squit_1, 4;
  BBSprite: Squit_2, 4;
  BBSprite: Squit_3, 4;
  BBSprite: Squit_4, 4;
  BBSprite: Squit_5, 4;  
  BBSprite: Squit_6, 4;
  BBSprite: Squit_7, 4;
  BBSprite: Squit_6, 4;
  BBSprite: Squit_5, 4;
  BBSprite: Squit_4, 4;
  BBSprite: Squit_3, 4;
  BBSprite: Squit_2, 4;
EndLoop:
# SquatToIdle
EnableNandemoCancel: true;
BBSprite: PreSquit_2, 2;
BBSprite: PreSquit_1, 2;
Exit;

[Rg_AirBrone]
@Trigger:
InAir: true;
return;

@Main:
EnableDefaultCancel: true;
EnableFlip: true;
Gravity: 100000;
AirMoveX: 150000;
# Airbrone
BeginLoop: (InAir: true)
  BBSprite: Fall_1, 3;
  BBSprite: Fall_2, 3;
EndLoop:
# Land
SetTransition: AirToLand;
Exit;

[Rg_Jump]
@Trigger:
CanJump: true;
InputType: JumpPressed;
return;

@Main:
SetVelocityX: 0;
# OnGround PreJump
# BeginIf: (TransitionCached: SquatToJump, true)
#   BBSprite: SquatToJump_1, 2;
#   BBSprite: SquatToJump_2, 2;
# EndIf:
# PreJump
BeginIf: (InAir: false)
  BBSprite: PreJump_1, 2;
  BBSprite: PreJump_2, 2;
EndIf:
# Jump
EnableFlip: true;
Gravity: 0;
AirMoveX: 150000;
SetVelocityY: 200000;
JumpAdd: -1;
BBSprite: Jump_1, 3;
BBSprite: Jump_2, 3;
BBSprite: Jump_1, 3;
EnableGatlingCancel: true;
GCOption: Rg_Jump;
Gravity: 100000;
BBSprite: Jump_2, 3;
BBSprite: Jump_1, 3;
# JumpToFall
BeginLoop: (InAir: true)
  BBSprite: JumpToFall_1, 3;
  BBSprite: JumpToFall_2, 3;
  BBSprite: JumpToFall_3, 3;
  BBSprite: JumpToFall_4, 3;
  BBSprite: JumpToFall_5, 3;
  Break;
EndLoop:
SetTransition: AirToLand;
Exit;

[Rg_5B]
@Trigger:
InputType: 5LPPressed;
InAir: false;
return;

@Main:
Event: (Whiff_Start)
  EnableWhiffCancel: true;
  WhiffOption: Rg_GroundDash;
EndEvent:
Event: (Hit_Start)
  # 这里开始，受击回调
  # 对于同一对象，在持续帧内仅造成一次攻击(Repeat则为持续帧内，只要发生碰撞，每帧都会回调受击回调)
  HitNotify: Once 
    EnableGatlingCancel: true;
    EnableTargetCancel: true;
    TCOption: Rg_5C;
    Shake: 500, 0, 8000, 10; # 振动
    HitStop: 0, 10; # 打击停顿
    # # 受击行为协程需要使用的变量
    # HitParam: Shake_LengthX, 1200;
    # HitParam: Shake_LengthY, 1000;
    # HitParam: Shake_Frequency, 10000;
    # HitParam: Shake_Frame, 18;
    # # 受击者帧冻结(HitStop)的总帧长
    # HitParam: HitStopFrame, 18;
    # # HitStop结束后抛出的速度(万分制)
    # HitParam: StartV_X, -3000;
    # HitParam: StartV_Y, 250000;
    # # 受击时调整转向
    # Hit_UpdateFlip;
    # # 受击者进入哪个硬直状态
    # HitStun: Hurt3;
    EndNotify:
EndEvent:
Event: (Hit_End)
  EnableGatlingCancel: false;
  EnableTargetCancel: false;
  EnableWhiffCancel: false;
EndEvent:
ApplyRootMotion: true;
PlayTimeline: 0, 30;
ApplyRootMotion: false;
Exit;

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
#ApplyRootMotion: true;
PlayTimeline: 0, 48;
#ApplyRootMotion: false;
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
Segment: (InputType: 5HPPressing)
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
EndSegment:
BBSprite: Frame_6, 3;
BBSprite: Frame_7, 3;
BBSprite: Frame_8, 3;
HitNotify: Once
  BeginIf: (Flag: Charge, false)
    Shake: 500, 0, 8000, 10; # 振动
    HitStop: 0, 10; # 打击停顿
    HitParam: Shake_LengthX, 1000;
    HitParam: Shake_LengthY, 1000;
    HitParam: Shake_Frequency, 10000;
    HitParam: Shake_Frame, 10;
    # 受击者帧冻结(HitStop)的总帧长
    HitParam: HitStopFrame, 10;
    HitParam: LastFrame, 40;
    Hit_UpdateFlip;
    HitStun: Hurt4;
  EndIf:
  BeginIf: (Flag: Charge, true)
    Shake: 800, 0, 12000, 25;
    HitStop: 0, 25;
    # 受击行为协程需要使用的变量
    HitParam: Shake_LengthX, 1200;
    HitParam: Shake_LengthY, 1000;
    HitParam: Shake_Frequency, 10000;
    HitParam: Shake_Frame, 25;
    # 受击者帧冻结(HitStop)的总帧长
    HitParam: HitStopFrame, 25;
    # HitStop结束后抛出的速度(万分制)
    HitParam: StartV_X, -400000;
    HitParam: StartV_Y, 200000;
    # 受击时调整转向
    Hit_UpdateFlip;
    # 受击者进入哪个硬直状态
    HitStun: Hurt5;
  EndIf:
EndNotify:
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
Numeric: DashCount > 0;
return;

@Main:
Event: (GC_Start)
  # CancelWindow: Gatling;
  # CancelOption: Rg_Jump;
  # GCOption: Rg_AirDashAttack;
  # GCOption: Rg_PlungingAttack;
EndEvent:
NumericAdd: DashCount, -1;
Event: (RootMotion_Start)
  ApplyRootMotion: true;
EndEvent:
Event: (RootMotion_End)
  # Inertia
  # CancelWindow: Transition;
  ApplyRootMotion: false;
  SetVelocityX: 80000;
  SetTransition: AirToLand;
EndEvent:
# StartTimeline;
PlayTimeline: 0, 24;
Exit;

[Rg_GroundDash]
@Trigger:
InAir: false;
InputType: DashPressed;
CanGroundDash: true;
return;

@Main:
SetVelocityY: 0;
SetVelocityX: 350000;
Gravity: 100000;
GroundDashAdd: -1;
BBSprite: Dash_1, 3;
BBSprite: Dash_2, 3;
EnableGatlingCancel: true;
GCOption: Rg_Jump;
BBSprite: Dash_1, 3;
BBSprite: Dash_2, 3;
SetVelocityX: 200000;
BBSprite: Dash_1, 3;
SetVelocityX: 100000;
BBSprite: DashEnd_1, 3;
SetVelocityX: 50000;
GCOption: Rg_GroundDash;
BBSprite: DashEnd_1, 6;
BBSprite: DashEnd_2, 3;
SetVelocityX: 0;
BBSprite: DashEnd_3, 1;
SetTransition: NoPreSquat;
EnableGatlingCancel: false;
EnableNandemoCancel: true;
BBSprite: DashEnd_3, 2;
BBSprite: DashEnd_4, 3;
BBSprite: DashEnd_5, 3;
BBSprite: DashEnd_6, 3;
Exit;

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
SetTransition: NoPreSquat;
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

[Rg_IdleAnim]
@Main:
EnableNandemoCancel: true;
PlayTimeline: 0, 81;
Exit;