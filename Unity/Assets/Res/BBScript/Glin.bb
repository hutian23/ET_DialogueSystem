[Root]
@RootInit:
GlinInit;
EnableAirCheck;
# bullet由对象池管理
PoolObject: GlinBullet, 5;
PoolObject: GlinSpike, 3;
PoolObject: GlinFireball, 20;
RegistMove: (Glin_Slash)
  MoveType: Normal;
EndMove:
RegistMove: (Glin_Capespike)
  MoveType: Normal;
EndMove:
RegistMove: (Glin_AirDash)
  MoveType: Normal;
EndMove:
RegistMove: (Glin_Cast)
  MoveType: Normal;
EndMove:
RegistMove: (Glin_Ballon)
  MoveType: Normal;
EndMove:
RegistMove: (Glin_TeleportOut)
  MoveType: Normal;
EndMove:
RegistMove: (Glin_Bow)
  MoveType: Normal;
EndMove:
# Test
Visible: true;
GotoBehavior: Glin_Bow;
return;

[Glin_Slash]
@Trigger:
return;

@Main:
SetPos: 0, -100000;
# Teleport In
BBSprite: Teleport_1, 4;
BBSprite: Teleport_2, 4;
BBSprite: Teleport_3, 4;
BBSprite: Teleport_4, 4;
# Slash
BBSprite: Slash_1, 6;
BBSprite: Slash_2, 4;
BBSprite: Slash_3, 4;
BBSprite: Slash_4, 8;
# Dash
SetVelocityX: 600000;
BBSprite: Slash_5, 3;
SetVelocityX: 400000;
BBSprite: Slash_6, 3;
SetVelocityX: 200000;
BBSprite: Slash_7, 4;
SetVelocityX: 100000;
BBSprite: Slash_8, 2;
SetVelocityX: 50000;
BBSprite: Slash_8, 2;
SetVelocityX: 0;
BBSprite: Slash_9, 5;
# UpperCut_Start
BBSprite: UpperCut_Start_1, 4;
BBSprite: UpperCut_Start_2, 5;
BBSprite: UpperCut_Start_3, 5;
BBSprite: UpperCut_Start_4, 5;
BBSprite: UpperCut_Start_5, 8;
# UpperCut Active
SetVelocity: 100000, 900000;
BBSprite: UpperCut_Active_1, 5;
SetVelocity: 50000, 600000;
BBSprite: UpperCut_Active_1, 4;
SetVelocity: 40000, 300000;
BBSprite: UpperCut_Active_1, 3;
SetVelocity: 10000, 100000;
BBSprite: UpperCut_Active_1, 3;
BBSprite: UpperCut_Active_1, 3;
# UpperCut End
SetVelocity: 0, 0;
BBSprite: UpperCut_End_1, 5;
BBSprite: UpperCut_End_2, 4;
GotoBehavior: Glin_TeleportOut;

[Glin_Capespike]
@Trigger:
return;

@Main:
SetPos: 50000, -100000;
# Teleport In
BBSprite: Teleport_1, 4;
BBSprite: Teleport_2, 4;
BBSprite: Teleport_3, 4;
BBSprite: Teleport_4, 4;
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
SetPos: 0, 0;
# Teleport In
BBSprite: Teleport_1, 4;
BBSprite: Teleport_2, 4;
BBSprite: Teleport_3, 4;
# AirDash Start
BBSprite: AirDash_Anticipate_1, 3;
BBSprite: AirDash_Anticipate_2, 3;
BBSprite: AirDash_Anticipate_3, 3;
BBSprite: AirDash_Anticipate_4, 3;
BBSprite: AirDash_Anticipate_5, 3;
BBSprite: AirDash_Anticipate_6, 3;
BBSprite: AirDash_Anticipate_7, 3;
# AirDash Active
ApplyRotation: true;
SetVelocity: 450000, -700000;
BeginLoop: (InAir: true)
  BBSprite: AirDash_Active_1, 4;
  BBSprite: AirDash_Active_2, 4;
  BBSprite: AirDash_Active_3, 4;
EndLoop:
ApplyRotation: false;
# GroundDash Start
SetVelocity: 0, 0;
ScreenShake: 1550, 550, 10000, 15;
BBSprite: GroundDash_Anticipate_1, 5;
BBSprite: GroundDash_Anticipate_2, 5;
BBSprite: GroundDash_Anticipate_3, 5;
BBSprite: GroundDash_Anticipate_4, 10;
# GroundDash Active
SetVelocityX: 100000;
BBSprite: GroundDash_Active_1, 1;
SetVelocityX: 400000;
BBSprite: GroundDash_Active_1, 1;
SetVelocityX: 700000;
BBSprite: GroundDash_Active_1, 1;
SetVelocityX: 800000;
BBSprite: GroundDash_Active_1, 1;
SetVelocityX: 850000;
BBSprite: GroundDash_Active_2, 4;
BBSprite: GroundDash_Active_3, 4;
BBSprite: GroundDash_Active_4, 4;
SetVelocityX: 200000;
BBSprite: GroundDash_Anticipate_1, 5;
SetVelocityX: 80000;
BBSprite: GroundDash_Anticipate_2, 5;
SetVelocityX: 0;
BBSprite: GroundDash_Anticipate_3, 4;
BBSprite: GroundDash_Anticipate_4, 4;
GotoBehavior: Glin_TeleportOut;

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
SetPos: 0, 0;
# Teleport In
BBSprite: Teleport_1, 4;
BBSprite: Teleport_2, 4;
BBSprite: Teleport_3, 4;
BBSprite: Anticipate_1, 7;
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
BBSprite: Bow_6, 5;
BBSprite: Bow_7, 40;
# Bow_2
BBSprite: Bow_6, 4;
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
Visible: false;
WaitFrame: 50;
Visible: true;
# Teleport In
ScreenShake: 750, 750, 10000, 15;
Random: ran1, 0, 100;
BeginIf: (Random: ran1 >= 0), (Random: ran1 < 10)
  GotoBehavior: Glin_Ballon;
EndIf:
BeginIf: (Random: ran1 >= 10), (Random: ran1 < 50)
  GotoBehavior: Glin_Slash;
EndIf:
BeginIf: (Random: ran1 >= 50), (Random: ran1 < 80)
  GotoBehavior: Glin_Cast;
EndIf:
BeginIf: (Random: ran1 >= 80), (Random: ran1 < 90)
  GotoBehavior: Glin_Capespike;
EndIf:
GotoBehavior: Glin_AirDash;