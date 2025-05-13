[Root]
@RootInit:
GlinInit;
EnableAirCheck;
# bullet由对象池管理
PoolObject: GlinBullet, 5;
PoolObject: GlinSpike, 3;
PoolObject: GlinFireball, 20;
PoolObject: ADust, 1;
PoolObject: GDust, 1;
RegistMove: (Glin_Slash)
  MoveType: Normal;
EndMove:
RegistMove: (Glin_Capespike)
  MoveType: Normal;
EndMove:
RegistMove: (Glin_AirDash)
  MoveType: Normal;
EndMove:
# RegistMove: (Glin_Cast)
#   MoveType: Normal;
# EndMove:
RegistMove: (Glin_Ballon)
  MoveType: Normal;
EndMove:
RegistMove: (Glin_TeleportOut)
  MoveType: Normal;
EndMove:
# RegistMove: (Glin_Bow)
#   MoveType: Normal;
# EndMove:
GotoBehavior: Glin_AirDash;
return;

[Glin_Slash]
@Trigger:
return;

@Main:
# Slash Start
BBSprite: Slash_1, 10;
BBSprite: Slash_2, 4;
BBSprite: Slash_3, 4;
# 蓄力
RegistCounter: 30;
BeginLoop: (Counter: Value > 0)
  BBSprite: Slash_4, 6;
  BBSprite: Slash_5, 6;
EndLoop:
# Slash Active
SetVelocityX: 700000;
BBSprite: Slash_6, 4;
SetVelocityX: 400000;
BBSprite: Slash_7, 4;
SetVelocityX: 200000;
BBSprite: Slash_8, 4;
SetVelocityX: 50000;
BBSprite: Slash_9, 4;
SetVelocityX: 0;
# UpperCut_Start
BBSprite: UpperCut_Start_1, 4;
BBSprite: UpperCut_Start_2, 8;
BBSprite: UpperCut_Start_3, 4;
BBSprite: UpperCut_Start_4, 4;
BBSprite: UpperCut_Start_5, 4;
# UpperCut Active
SetVelocity: 200000, 800000;
BBSprite: UpperCut_Active_1, 5;
SetVelocity: 100000, 400000;
BBSprite: UpperCut_Active_1, 5;
SetVelocity: 50000, 200000;
BBSprite: UpperCut_Active_1, 4;
SetVelocity: 10000, 20000;
BBSprite: UpperCut_Active_1, 4;
# UpperCut End
SetVelocity: 0, 0;
GotoBehavior: Glin_TeleportOut;

[Glin_Capespike]
@Trigger:
return;

@Main:
# first seven frames reversed for end
# Pre Cast
BBSprite: Capespike_1, 5;
BBSprite: Capespike_2, 5;
BBSprite: Capespike_3, 5;
BBSprite: Capespike_4, 5;
BBSprite: Capespike_5, 5;
BBSprite: Capespike_6, 5;
BBSprite: Capespike_7, 5;
ScreenShake: 450, 0, 8000, 10;
BBSprite: Capespike_8, 5;
# Cast Spike
CastGlinSpike;
RegistCounter: 150;
# CastGlinSpike: 0, -3000;
BeginLoop: (Counter: Value > 0)
  BBSprite: Capespike_9, 7;
  BBSprite: Capespike_10, 7;
EndLoop:
# Cast End
BBSprite: Capespike_8, 5;
BBSprite: Capespike_7, 5;
BBSprite: Capespike_6, 5;
BBSprite: Capespike_5, 5;
BBSprite: Capespike_4, 5;
BBSprite: Capespike_3, 5;
GotoBehavior: Glin_TeleportOut;

[Glin_AirDash]
@Trigger:
return;

@Main:
SetFlip: Left;
AirDashPos: 15000;
# AirDash Start
BBSprite: AirDash_Anticipate_1, 5;
BBSprite: AirDash_Anticipate_2, 5;
BBSprite: AirDash_Anticipate_3, 5;
BBSprite: AirDash_Anticipate_4, 5;
BBSprite: AirDash_Anticipate_5, 5;
BBSprite: AirDash_Anticipate_6, 5;
BBSprite: AirDash_Anticipate_7, 5;
# 悬停在空中，空箭朝向玩家
EnableGlinChase: true, -450000, 450000, 100000;
EnableGlinShake: true, 450, 450, 8000;
RegistCounter: 60;
BeginLoop: (Counter: Value > 0)
  BBSprite: AirDash_Active_1, 5;
  BBSprite: AirDash_Active_2, 5;
  BBSprite: AirDash_Active_3, 5;
EndLoop:
EnableGlinShake: false, 0, 0, 0;
EnableGlinChase: false, 0, 0, 0;
# AirDash Active
AirDashVelocity: -700000;
SpawnADust: 0, 27000, 8000, 12000;
# 下落过程中持续检测地面
EnableAirDashToGroundCheck: true;
BeginLoop: (AirDashToGround: false)
  BBSprite: AirDash_Active_1, 4;
  BBSprite: AirDash_Active_2, 4;
  BBSprite: AirDash_Active_3, 4;
EndLoop:
EnableAirDashToGroundCheck: false;
SetRotate: 0;
EnemyUpdateFlip;
# GroundDash Start
SpawnGDust: -65000, -20000, -8000, 4000;
SpawnGDust: 65000, -20000, 8000, 4000;
SetVelocity: 0, -10000;
ScreenShake: 1550, 550, 10000, 20;
BBSprite: GroundDash_Anticipate_1, 5;
BBSprite: GroundDash_Anticipate_2, 5;
BBSprite: GroundDash_Anticipate_3, 14;
BBSprite: GroundDash_Anticipate_4, 5;
# GroundDash Active
BBSprite: GroundDash_Active_1, 5;
SpawnGDust: 80000, -15000, 10000, 6000;
SetVelocity: 700000, 0;
BBSprite: GroundDash_Active_2, 5;
AccelX: 700000, 15, -2800000;
BBSprite: GroundDash_Active_3, 5;
BBSprite: GroundDash_Active_4, 5;
BBSprite: GroundDash_Anticipate_1, 5;
SetVelocityX: 0;
BBSprite: GroundDash_Anticipate_2, 5;
BBSprite: GroundDash_Anticipate_3, 5;
BBSprite: GroundDash_Anticipate_4, 5;
# GotoBehavior: Glin_TeleportOut;
GotoBehavior: Glin_AirDash;

[Glin_Cast]
@Trigger:
return;

@Main:
SetPos: 0, -100000;
# Teleport In
BBSprite: Teleport_1, 5;
BBSprite: Teleport_2, 5;
BBSprite: Teleport_3, 5;
BBSprite: Teleport_4, 5;
BBSprite: Teleport_5, 5;
# Start
BBSprite: Start_1, 4;
BBSprite: Start_2, 4;
BBSprite: Start_3, 4;
BBSprite: Start_4, 4;
# Active
BBSprite: Active_1, 4;
BBSprite: Active_2, 4;
CastGlinBullet: 30, -10000, 5000, 7000, 3, 350, 350, 10000, 15;
RegistCounter: 80;
BeginLoop: (Counter: Value > 0)
  BBSprite: Active_3, 8;
  BBSprite: Active_4, 8;
EndLoop:
BBSprite: Start_4, 4;
BBSprite: Start_3, 4;
BBSprite: Start_2, 4;
BBSprite: Start_1, 4;
GotoBehavior: Glin_TeleportOut;

[Glin_Ballon]
@Trigger:
return;

@Main:
# Cast Fireball
# 发射飞弹时屏幕振动
EnableGlinShake: true, 250, 250, 8000;
# 纵向飞弹，y轴速度不变，x轴速度飞行过程中略微增大
# 横向飞弹，x轴速度不变，y轴速度逐渐趋于0
Enable_CastGlinFireball: true, 35, -85000, 20000, 160000;
BBSprite: Anticipate_2, 5;
RegistCounter: 200;
BeginLoop: (Counter: Value > 0)
  BBSprite: Active_1, 5;
  BBSprite: Active_2, 5;
  BBSprite: Active_3, 5;
EndLoop:
# Cast End
EnableGlinShake: false, 0, 0, 0;
Enable_CastGlinFireball: false, 0, 0, 0, 0;
BBSprite: Anticipate_2, 7;
BBSprite: Anticipate_1, 5;
GotoBehavior: Glin_TeleportOut;

[Glin_Bow]
@Trigger:
return;

@Main:
SetPos: 60000, -100000;
# Idle
RegistCounter: 100;
BeginLoop: (Counter: Value > 0)
  BBSprite: Idle_1, 5;
  BBSprite: Idle_2, 5;
  BBSprite: Idle_3, 5;
  BBSprite: Idle_4, 5;
  BBSprite: Idle_5, 5;
  BBSprite: Idle_6, 5;
  BBSprite: Idle_7, 5;
  BBSprite: Idle_8, 5;
  BBSprite: Idle_9, 5;
  BBSprite: Idle_10, 5;
  BBSprite: Idle_11, 5;
  BBSprite: Idle_12, 5;
EndLoop:
# Bow_1
BBSprite: Bow_1, 5;
BBSprite: Bow_2, 5;
BBSprite: Bow_3, 5;
BBSprite: Bow_4, 5;
BBSprite: Bow_5, 5;
BBSprite: Bow_6, 30;
# Bow_2
BBSprite: Bow_5, 4;
BBSprite: Bow_4, 4;
BBSprite: Bow_3, 4;
BBSprite: Bow_2, 4;
BBSprite: Bow_1, 10;
GotoBehavior: Glin_TeleportOut;

[Glin_TeleportOut]
@Trigger:
return;

@Main:
# Teleport Out
SetVelocity: 0, 0;
BBSprite: Frame_6, 5;
BBSprite: Frame_7, 5;
ScreenShake: 750, 750, 10000, 15;
BBSprite: Frame_3, 5;
BBSprite: Frame_2, 5;
BBSprite: Frame_1, 5;
SetPos: -1000000, -1000000;
WaitFrame: 50;
# Select Next Behavior
Random: ran1, 0, 100;
GlinPos: -140000, 140000, 80000, 0; 
# Teleport In
BBSprite: Frame_1, 5;
BBSprite: Frame_2, 5;
ScreenShake: 750, 750, 10000, 15;
BBSprite: Frame_3, 5;
BBSprite: Frame_4, 5;
# Enter Next Behavior
GotoBehavior: Glin_AirDash;