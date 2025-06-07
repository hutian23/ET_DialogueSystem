[Root]
@RootInit:
#1. PlayerInit中挂载组件(NumericComponent、InputComponent...)
PlayerInit;
#2. 相机跟随
Camera_FollowPlayer;
SetPos: 0, -70000;
#3. 初始化对象池
PoolObject: DeadSpike, 2;
PoolObject: CircleWave, 1;
PoolObject: GDust, 1;
PoolObject: ADust, 1;
PoolObject: HellsFang, 1;
#4. 添加初始Buff
HP: 10000;
SP: 200;
EnableJustEvade: 600;
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
RegistMove: (Rg_5C)
  MoveType: Normal;
EndMove:
RegistMove: (Rg_5D)
  MoveType: Normal;
EndMove:
RegistMove: (Rg_TCEnd)
  MoveType: Normal;
EndMove:
RegistMove: (Rg_JC)
  MoveType: Normal;
EndMove:
RegistMove: (Rg_PlungingAttack)
  MoveType: Normal;
EndMove:
RegistMove: (Rg_AirDash)
  MoveType: Special;
EndMove:
RegistMove: (Rg_AirDashAttack)
  MoveType: Special;
EndMove:
RegistMove: (Rg_GroundDash)
  MoveType: Special;
EndMove:
RegistMove: (Rg_GroundDashAttack)
  MoveType: Special;
EndMove:
RegistMove: (Rg_HardLand)
  MoveType: Etc;
EndMove:
RegistMove: (Rg_IdleAnim)
  MoveType: Etc;
EndMove:
# RegistMove: (Rg_Hurt)
#   MoveType: HitStun;
# EndMove:
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
EnableTargetComboCancel: true;
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

[Rg_Land]
@Trigger:
Transition: AirToLand, true;
return;

@Main:
SetVelocityX: 0;
EnableTargetComboCancel: true;
EnableDefaultCancel: true;
# MiddleLand
BeginIf: (LandVel: 350000)
  BBSprite: Land_1, 3;
  BBSprite: Land_2, 3;
  BBSprite: Land_3, 3;
EndIf:
# LightLand
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
EnableTargetComboCancel: true;
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
EnableTargetComboCancel: true;
EnableDefaultCancel: true;
# PreSquat
BeginIf: (TransitionCached: NoPreSquat, false)
  BBSprite: Start_1, 2;
  BBSprite: Start_2, 2;
EndIf:
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
EnableNandemoCancel: true;
BBSprite: End_1, 4;
BBSprite: End_2, 4;
Exit;


[Rg_AirBrone]
@Trigger:
InAir: true;
return;

@Main:
EnableHardLandCheck: 10, 350000;
EnableTargetComboCancel: true;
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
JumpAdd: -1;
EnableAirMoveX: 150000, true;
SetVelocityY: 250000;
BBSprite: Jump_1, 3;
Gravity: 100000;
EnableGatlingCancel: true;
EnableTargetComboCancel: true;
BBSprite: Jump_2, 3;
BBSprite: Jump_1, 3;
# 跳跃取消
EnableWhiffCancel: true;
WhiffOption: Rg_Jump;
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
SetVelocityX: 0;
BBSprite: Anticipate_1, 2;
BBSprite: Anticipate_2, 1;
# 冲刺取消
EnableWhiffCancel: true;
WhiffOption: Rg_GroundDash;
BBSprite: Anticipate_3, 2;
BBSprite: Anticipate_4, 2;
SetVelocityX: 30000;
BBSprite: Anticipate_5, 2;
SetVelocityX: 50000;
BBSprite: Anticipate_6, 2;
SetVelocityX: 100000;
BBSprite: Anticipate_7, 2;
BBSprite: Active_1, 2;
SetVelocityX: 50000;
BBSprite: Active_1, 2;
SetVelocityX: 30000;
BBSprite: Active_2, 3;
BBSprite: End_1, 2;
SetVelocityX: 0;
# 启动TC窗口
EnableTargetComboCancel: true;
TargetComboOption: Rg_5C, 12;
BBSprite: End_2, 3;
BBSprite: End_3, 3;
EnableNandemoCancel: true;
BBSprite: End_4, 3;
BBSprite: End_5, 3;
Exit;


[Rg_5C]
@TargetComboTrigger:
InputType: 5LPPressed;
InAir: false;
return;

@Trigger:
Accessible: false;
return;

@Main:
# Anticipate
SetVelocityX: 0;
BBSprite: Anticipate_1, 2;
BBSprite: Anticipate_2, 2;
BBSprite: Anticipate_3, 2;
# 冲刺取消
EnableWhiffCancel: true;
WhiffOption: Rg_GroundDash;
BBSprite: Anticipate_4, 2;
BBSprite: Anticipate_5, 2;
SetVelocityX: 80000;
BBSprite: Anticipate_6, 2;
# Active
SetVelocityX: 150000;
BBSprite: Active_1, 3;
SetVelocityX: 80000;
BBSprite: Active_2, 3;
# End
BBSprite: End_1, 2;
SetVelocityX: 30000;
BBSprite: End_1, 3;
SetVelocityX: 0;
# 启动TC窗口
EnableTargetComboCancel: true;
TargetComboOption: Rg_5D, 20;
BBSprite: End_1, 3;
BBSprite: End_2, 3;
EnableNandemoCancel: true;
BBSprite: End_3, 3;
BBSprite: End_4, 3;
BBSprite: End_5, 3;
BBSprite: End_6, 3;
BBSprite: End_7, 3;
BBSprite: End_8, 3;
BBSprite: End_9, 3;
Exit;


[Rg_5D]
@TargetComboTrigger:
InputType: 5LPPressed;
InAir: false;
return;

@Trigger:
Accessible: false;
return;

@Main:
SetVelocityX: 0;
BBSprite: Anticipate_1, 3;
BBSprite: Anticipate_2, 3;
# 冲刺取消
EnableWhiffCancel: true;
WhiffOption: Rg_GroundDash;
BBSprite: Anticipate_3, 3;
BBSprite: Anticipate_4, 3;
SetVelocityX: 50000;
BBSprite: Anticipate_5, 3;
BBSprite: Anticipate_6, 3;
SetVelocityX: 100000;
# 大剑砸地，震屏
ScreenShake: 1800, 500, 12000, 15, 0;
BBSprite: Active_1, 3;
SetVelocityX: 60000;
BBSprite: Active_2, 4;
SetVelocityX: 0;
BBSprite: Active_2, 3;
EnableTargetComboCancel: true;
BBSprite: Active_2, 3;
TargetComboOption: Rg_TCEnd, 40;
EnableNandemoCancel: true;
BBSprite: Active_2, 4;
BBSprite: End_1, 3;
BBSprite: End_2, 3;
BBSprite: End_3, 3;
BBSprite: End_4, 3;
BBSprite: End_5, 3;
BBSprite: End_6, 3;
Exit;


[Rg_TCEnd]
@TargetComboTrigger:
InputType: 5LPPressed;
InAir: false;
return;

@Trigger:
Accessible: false;
return;

@Main:
SetVelocityX: 0;
BBSprite: Anticipate_1, 2;
BBSprite: Anticipate_2, 2;
# 冲刺取消
EnableWhiffCancel: true;
WhiffOption: Rg_GroundDash;
BBSprite: Anticipate_3, 2;
SetVelocityX: 50000;
BBSprite: Anticipate_4, 3;
SetVelocityX: 100000;
BBSprite: Active_1, 3;
BBSprite: Active_2, 4;
# 创建子弹
SetVelocityX: 50000;
ScreenShake: 1200, 1200, 10000, 10, 0;
CreateBullet: DeadSpike
  BulletLocalPosition: -40000, -7500;
EndCreateBullet:
BBSprite: Active_3, 3;
SetVelocityX: 20000;
BBSprite: End_1, 4;
SetVelocityX: 0;
BBSprite: End_2, 4;
EnableNandemoCancel: true;
BBSprite: End_3, 4;
BBSprite: End_4, 4;
BBSprite: End_5, 4;
Exit;


[Rg_JC]
@Trigger:
InAir: true;
InputType: 5LPPressed;
return;

@Main:
EnableJumpMoveX: 150000, true;
BBSprite: Anticipate_1, 2;
BBSprite: Anticipate_2, 2;
BBSprite: Anticipate_3, 2;
BBSprite: Active_1, 2;
BBSprite: Active_2, 2;
EnableWhiffCancel: true;
WhiffOption: Rg_AirDash;
# TC连段
EnableTargetComboCancel: true;
TargetComboOption: Rg_PlungingAttack, 30;
RegistCounter: 23;
BeginLoopAnim: (InAir: true), (Counter: Value > 0)
  LoopSprite: Active_3, 2;
  LoopSprite: End_1, 3;
  LoopSprite: End_2, 3;
  LoopSprite: End_3, 3;
  LoopSprite: End_4, 3;
  LoopSprite: End_5, 3;
  LoopSprite: End_6, 3;
  LoopSprite: End_7, 3;
EndLoopAnim:
EnableJumpMoveX: 0, false;
SetVelocityX: 0;
SetTransition: AirToLand, true;
Exit;


[Rg_PlungingAttack]
@TargetComboTrigger:
InAir: true;
InputType: 5LPPressed;
return;

@Trigger:
InAir: true;
InputType: 2LPPressed;
return;

@Main:
SetVelocity: 0, 0;
Gravity: 0;
SetVelocity: 150000, 150000;
BBSprite: Anticipate_1, 4;
SetVelocity: 100000, 100000;
BBSprite: Anticipate_2, 4;
EnableWhiffCancel: true;
WhiffOption: Rg_AirDash;
SetVelocity: 50000, 50000;
BBSprite: Anticipate_3, 4;
SetVelocityY: 20000;
BBSprite: Anticipate_3, 4;
SetVelocityY: -50000;
BBSprite: Anticipate_4, 2;
SetVelocityY: -350000;
BBSprite: Anticipate_5, 2;
SetVelocityY: -550000;
BBSprite: Active_1, 2;
BeginLoopAnim: (InAir: true)
  LoopSprite: Active_2, 4;
  LoopSprite: Active_3, 4;
EndLoopAnim:
VFX: GDust
  VFX_LocalPosition: 40000, -14000;
  VFX_Scale: 5000, 3000;
EndVFX:
VFX: GDust
  VFX_LocalPosition: -40000, -14000;
  VFX_Scale: -5000, 3000;
EndVFX:
ScreenShake: 1200, 1200, 12000, 15, 0;
Gravity: 100000;
SetVelocity: 0, -1000;
BBSprite: End_1, 5;
EnableWhiffCancel: true;
WhiffOption: Rg_GroundDash;
BBSprite: End_1, 5;
BBSprite: End_2, 4;
BBSprite: End_3, 4;
BBSprite: End_4, 3;
BBSprite: End_5, 3;
BBSprite: End_6, 3;
BBSprite: End_7, 3;
Exit;


[Rg_AirDash]
@Trigger:
InAir: true;
InputType: DashPressed;
CanAirDash: true;
return;

@Main:
AirDashAdd: -1;
# 生成特效
VFX: ADust
  VFX_LocalPosition: 25000, 0;
  VFX_Scale: 8000, 3000;
EndVFX:
# 精准闪避
EnableJustEvadeCheck: 8, 10000, 0, 50000, 40000;
RegistJustEvadeCallback: Rg_AirDash, JustEvadeCallback;
# AirDash
SetVelocity: 300000, 0;
Gravity: 0;
BBSprite: Anticipate_1, 4;
BBSprite: Active_1, 3;
EnableWhiffCancel: true;
WhiffOption: Rg_Jump;
WhiffOption: Rg_PlungingAttack;
EnableTargetComboCancel: true;
TargetComboOption: Rg_AirDashAttack, 9;
SetVelocityX: 200000;
BBSprite: Active_1, 3;
SetVelocityX: 100000;
BBSprite: Active_2, 2;
SetVelocityX: 80000;
BBSprite: Active_2, 2;
# Fall
WhiffOption: Rg_AirDash;
Gravity: 100000;
RegistCounter: 9;
EnableFlip: true;
SetTransition: AirToLand, true;
BeginLoopAnim: (Counter: Value > 0), (InAir: true)
  LoopSprite: End_1, 3;
  LoopSprite: End_2, 3;
  LoopSprite: End_3, 3;
EndLoopAnim:
Exit;

@JustEvadeCallback:
TimeFroze: 20, 5;
VFX: CircleWave
  VFX_Scale: 70000, 70000;
  VFX_LocalPosition: -6000, -10000;
EndVFX:
TacticalTime: 300, 10;
Invincible: 300;
return;

[Rg_AirDashAttack]
@TargetComboTrigger:
InputType: 5LPPressed;
InAir: true;
return;

@Trigger:
Accessible: false;
return;

@Main:
SetVelocity: 80000, 0;
Gravity: 0;
BBSprite: Anticipate_1, 3;
BBSprite: Anticipate_2, 3;
BBSprite: Anticipate_3, 3;
BBSprite: Anticipate_4, 3;
BBSprite: Anticipate_5, 3;
SetVelocityX: 300000;
ScreenShake: 600, 600, 10000, 15, 0;
BBSprite: Active_1, 4;
SetVelocityX: 250000;
BBSprite: Active_2, 2;
SetVelocityX: 200000;
BBSprite: Active_2, 2;
SetVelocityX: 150000;
BBSprite: Active_3, 2;
SetVelocityX: 80000;
EnableWhiffCancel: true;
WhiffOption: Rg_PlungingAttack;
WhiffOption: Rg_Jump;
WhiffOption: Rg_AirDash;
BBSprite: Active_3, 2;
BBSprite: End_1, 3;
BBSprite: End_2, 3;
# Fall
SetVelocityX: 40000;
Gravity: 100000;
RegistCounter: 10;
SetTransition: AirToLand, true;
BeginLoopAnim: (Counter: Value > 0), (InAir: true)
  LoopSprite: End_3, 2;
  LoopSprite: End_4, 2;
  LoopSprite: End_5, 2;
  LoopSprite: End_6, 2;
  LoopSprite: End_7, 2;
EndLoopAnim:
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
# 生成特效
VFX: GDust
  VFX_LocalPosition: 40000, -12000;
  VFX_Scale: 6000, 4000;
EndVFX:
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
VFX: CircleWave
  VFX_Scale: 70000, 70000;
  VFX_LocalPosition: -6000, -10000;
EndVFX:
TacticalTime: 300, 10;
Invincible: 300;
return;


[Rg_GroundDashAttack]
@Trigger:
# Accessible: false;
InputType: 5LPPressed;
InAir: false;
return;

@Main:
BBSprite: Anticipate_1, 3;
BBSprite: Anticipate_2, 3;
BBSprite: Anticipate_3, 3;
BBSprite: Anticipate_4, 3;
# 技能特效，该技能中断时需要销毁这个特效
# SkillVFX: HellsFang;
BBSprite: Anticipate_5, 3;
BBSprite: Anticipate_6, 3;
BBSprite: Active_1, 4;
BBSprite: Active_2, 4;
BBSprite: End_1, 3;
BBSprite: End_2, 3;
BBSprite: End_3, 3;
BBSprite: End_4, 3;
BBSprite: End_5, 3;
BBSprite: End_6, 3;
BBSprite: End_7, 3;
BBSprite: End_8, 3;
BBSprite: End_9, 3;
BBSprite: End_10, 3;
BBSprite: End_11, 3;
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