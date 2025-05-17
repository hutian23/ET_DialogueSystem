[Root]
@RootInit:
GlinInit;
EnableAirCheck;
# bullet由对象池管理
PoolObject: GlinBullet, 5;
PoolObject: GlinSpike, 1;
PoolObject: GlinSpike_Step2, 3;
PoolObject: GlinFireball, 20;
PoolObject: ADust, 1;
PoolObject: GDust, 1;
# 注册行为
# Step1
RegistMove: (Glin_Idle)
  MoveType: Normal;
EndMove:
RegistMove: (Glin_Bow)
  MoveType: Normal;
EndMove:
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
# Step2
RegistMove: (Glin_Evade)
  MoveType: Normal;
EndMove:
RegistMove: (Glin_Roar)
  MoveType: Normal;
EndMove:
RegistMove: (Glin_Step2_FeintSlash)
  MoveType: Normal;
EndMove:
RegistMove: (Glin_Step2_CastSpike)
  MoveType: Normal;
EndMove:
RegistMove: (Glin_Step2_Slash)
  MoveType: Normal;
EndMove:
RegistMove: (Glin_Step2_Cast)
  MoveType: Normal;
EndMove:
GotoBehavior: Glin_Step2_Slash;

[Glin_Idle]
@Trigger:
return;

@Main:
EnableRangeCheck: true, 80000, 0, 0;
SetPos: 90000, -95000;
BeginLoop: (InRange: false)
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
EnableRangeCheck: false, 0, 0, 0;
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
GotoBehavior: Glin_Bow;

[Glin_Bow]
@Trigger:
return;

@Main:
# 受攻击切换到二阶段
HurtNotify: Once
  GotoBehavior: Glin_Roar;
EndNotify:
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

[Glin_Roar]
@Trigger:
return;

@Main:
BBSprite: InitFrame, 1;
EnableGlinShake: true, 550, 550, 10000;
RegistCounter: 100;
BeginLoop: (Counter: Value > 0)
  BBSprite: Roar_1, 5;
  BBSprite: Roar_2, 5;
  BBSprite: Roar_3, 5;
EndLoop:
EnableGlinShake: false, 0, 0, 0;
GotoBehavior: Glin_TeleportOut;

[Glin_Evade]
@Trigger:
return;

@Main:
SetPos: 0, -100000;
BBSprite: Start_1, 4;
BBSprite: Start_2, 4;
RegistCounter: 15;
SetVelocityX: -300000;
BeginLoop: (Counter: Value > 0)
  BBSprite: Active_1, 4;
  BBSprite: Active_2, 4;
  BBSprite: Active_3, 4;
EndLoop:
SetVelocityX: -100000;
BBSprite: Start_2, 4;
SetVelocityX: 0;
BBSprite: Start_1, 4;
GotoBehavior: Glin_Evade;

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
CastGlinSpike: -20000, -2000;
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
# 1. 悬停在空中
BBSprite: AirDash_Anticipate_1, 5; # 播放对应的动画帧，包括Sprite、Hitbox，第二个参数表示动画帧的持续帧数
BBSprite: AirDash_Anticipate_2, 5;
BBSprite: AirDash_Anticipate_3, 5;
BBSprite: AirDash_Anticipate_4, 5;
BBSprite: AirDash_Anticipate_5, 5;
BBSprite: AirDash_Anticipate_6, 5;
BBSprite: AirDash_Anticipate_7, 5;
EnableGlinChase: true, -450000, 450000, 80000; # 悬停在空中，空箭始终朝向玩家(调整Rotate)
EnableGlinShake: true, 450, 450, 8000; # 空箭蓄力过程中 屏幕振动
RegistCounter: 60; # 空箭蓄力60帧
BeginLoop: (Counter: Value > 0)
  BBSprite: AirDash_Active_1, 5;
  BBSprite: AirDash_Active_2, 5;
  BBSprite: AirDash_Active_3, 5;
EndLoop:
EnableGlinShake: false, 0, 0, 0;
EnableGlinChase: false, 0, 0, 0;
# 2. 下冲
AirDashVelocity: -700000;
SpawnADust: 0, 27000, 8000, 12000; # 冲刺起始，生成AirDust特效(根据unit当前Rotate调整对应的rotate)
EnableAirDashToGroundCheck: true;  # 下冲过程中每帧检测是否和地面碰撞，符合条件则退出loop协程
BeginLoop: (AirDashToGround: false)
  BBSprite: AirDash_Active_1, 4;
  BBSprite: AirDash_Active_2, 4;
  BBSprite: AirDash_Active_3, 4;
EndLoop:
# 3. 落地
EnableAirDashToGroundCheck: false;
SetRotate: 0;    
EnemyUpdateFlip; # 根据玩家当前位置调整地面冲刺朝向
SpawnGDust: -65000, -20000, -8000, 4000; # 生成Ground Dust特效
SpawnGDust: 65000, -20000, 8000, 4000;
SetVelocity: 0, -50000;
ScreenShake: 1550, 550, 10000, 20; # 落地的振动效果
# 4. 地面冲刺
# 地面冲刺起始期
BBSprite: GroundDash_Anticipate_1, 15;
BBSprite: GroundDash_Anticipate_2, 5;
BBSprite: GroundDash_Anticipate_3, 15;
BBSprite: GroundDash_Anticipate_4, 5;
# 地面冲刺攻击判定持续期
BBSprite: GroundDash_Active_1, 5;
SpawnGDust: 80000, -15000, 10000, 6000;
SetVelocity: 700000, 0; # 地面冲刺初始速度
BBSprite: GroundDash_Active_2, 5;
AccelX: 700000, 15, -2800000; # 减速
BBSprite: GroundDash_Active_3, 5;
BBSprite: GroundDash_Active_4, 5;
BBSprite: GroundDash_Anticipate_1, 5;
SetVelocityX: 0;
BBSprite: GroundDash_Anticipate_2, 5;
BBSprite: GroundDash_Anticipate_3, 5;
BBSprite: GroundDash_Anticipate_4, 5;
# 5. 隐身，然后切换到下一个动作
GotoBehavior: Glin_TeleportOut;

[Glin_Cast]
@Trigger:
return;

@Main:
# Start
BBSprite: Start_1, 5;
BBSprite: Start_2, 5;
RegistCounter: 20;
BeginLoop: (Counter: Value > 0) 
  BBSprite: Start_3, 5;
  BBSprite: Start_4, 5;
EndLoop: 
# Active
BBSprite: Active_1, 5;
BBSprite: Active_2, 5;
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
BBSprite: Anticipate_1, 5;
BBSprite: Anticipate_2, 5;
EnableGlinShake: true, 250, 250, 8000;
RegistCounter: 40;
BeginLoop: (Counter: Value > 0)
  BBSprite: Active_1, 5;
  BBSprite: Active_2, 5;
  BBSprite: Active_3, 5;
EndLoop:
# 纵向飞弹，y轴速度不变，x轴速度飞行过程中略微增大
# 横向飞弹，x轴速度不变，y轴速度逐渐趋于0
Enable_CastGlinFireball: true, 35, -85000, 20000, 160000;
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
# 1. AirDash
BeginIf: (Random: ran1 >= 0), (Random: ran1 < 20)
  AirDashPos: 15000;
  SetFlip: Left;
EndIf:
# 2. Slash
BeginIf: (Random: ran1 >= 20), (Random: ran1 < 40)
  GlinPos: -140000, 140000, 60000, -95000;
EndIf:
# 3. Cast
BeginIf: (Random: ran1 >= 40), (Random: ran1 < 70)
  GlinPos: -140000, 140000, 80000, -95000;
EndIf:
# 4. CapeSpike
BeginIf: (Random: ran1 >= 70), (Random: ran1 < 90)
  SetPos: 0, -95000;
  SetFlip: Left;
EndIf:
# 5. Ballon
BeginIf: (Random: ran1 >= 90), (Random: ran1 <= 100)
  SetPos: 0, 0;
  SetFlip: Left;
EndIf:
# Teleport In
BBSprite: Frame_1, 5;
BBSprite: Frame_2, 5;
ScreenShake: 750, 750, 10000, 15;
BBSprite: Frame_3, 5;
BBSprite: Frame_4, 5;
# Enter Next Behavior
# 1. AirDash
BeginIf: (Random: ran1 >= 0), (Random: ran1 < 20)
  GotoBehavior: Glin_AirDash;
EndIf:
# 2. Slash
BeginIf: (Random: ran1 >= 20), (Random: ran1 < 40)
  GotoBehavior: Glin_Slash;
EndIf:
# 3. Cast
BeginIf: (Random: ran1 >= 40), (Random: ran1 < 70)
  GotoBehavior: Glin_Cast;
EndIf:
# 4. CapeSpike
BeginIf: (Random: ran1 >= 70), (Random: ran1 < 90)
  GotoBehavior: Glin_Capespike;
EndIf:
# 5. Ballon
BeginIf: (Random: ran1 >= 90), (Random: ran1 <= 100)
  GotoBehavior: Glin_Ballon;
EndIf:

[Glin_Step2_FeintSlash]
@Trigger:
return;

@Main:
SetPos: -70000, -95000;
# 假动作
BBSprite: Feint_1, 5;
BBSprite: Feint_2, 5; 
RegistCounter: 25;
BeginLoop: (Counter: Value > 0)
  BBSprite: Feint_3, 6;
  BBSprite: Feint_4, 6;
EndLoop:
# 后撤步
BBSprite: Evade_1, 3;
BBSprite: Evade_2, 3;
RegistCounter: 15;
SetVelocityX: -480000;
BeginLoop: (Counter: Value > 0)
  BBSprite: Evade_3, 3;
  BBSprite: Evade_4, 3;
  BBSprite: Evade_5, 3;
EndLoop:
SetVelocityX: -50000;
BBSprite: Evade_6, 4;
SetVelocityX: 0;
BBSprite: Evade_7, 4;
# 生成飞弹 
BBSprite: Cast_Start_1, 4;
BBSprite: Cast_Start_2, 4;
BBSprite: Cast_Start_3, 4;
BBSprite: Cast_Start_4, 4;
BBSprite: Cast_Active_1, 4;
BBSprite: Cast_Active_2, 4;
CallSubCoroutine: CastGlinBullet;
RegistCounter: 120;
BeginLoop: (Counter: Value > 0)
  BBSprite: Cast_Active_3, 7;
  BBSprite: Cast_Active_4, 7;
EndLoop:
BBSprite: Cast_End_1, 4;
BBSprite: Cast_End_2, 4;
BBSprite: Cast_End_3, 6;
GotoBehavior: Glin_Step2_Cast;

# 飞弹协程
@CastGlinBullet:
# 1
ScreenShake: 550, 550, 10000, 10;
CreateBullet: GlinBullet
  BulletFlip: Left;
  BulletVelocity: -400000, 0;
  BulletPosition: -20000, 10000;
  BulletAccelY: -20000, 100, 150000;
EndCreateBullet:
WaitFrame: 20;
# 2
ScreenShake: 550, 550, 10000, 10;
CreateBullet: GlinBullet
  BulletFlip: Left;
  BulletVelocity: -400000, 0;
  BulletPosition: -15000, 5000;
  BulletAccelY: -20000, 100, 150000;
EndCreateBullet:
WaitFrame: 20;
# 3
ScreenShake: 550, 550, 10000, 10;
CreateBullet: GlinBullet
  BulletFlip: Left;
  BulletVelocity: -400000, 0;
  BulletPosition: -10000, 0;
  BulletAccelY: -20000, 100, 150000;
EndCreateBullet:
WaitFrame: 20;
# 4
ScreenShake: 550, 550, 10000, 10;
CreateBullet: GlinBullet
  BulletFlip: Left;
  BulletVelocity: -400000, 0;
  BulletPosition: -5000, -5000;
  BulletAccelY: -20000, 100, 150000;
EndCreateBullet:
WaitFrame: 20;
# 5
ScreenShake: 550, 550, 10000, 10;
CreateBullet: GlinBullet
  BulletFlip: Left;
  BulletVelocity: -400000, 0;
  BulletPosition: -0, -10000;
  BulletAccelY: -20000, 100, 150000;
EndCreateBullet:
WaitFrame: 20;
return;

[Glin_Step2_CastSpike]
@Trigger:
return;

@Main:
BBSprite: Anticipate_1, 5;
CallSubCoroutine: CastGlinSpike;
RegistCounter: 200;
BeginLoop: (Counter: Value > 0)
  BBSprite: Active_1, 5;
  BBSprite: Active_2, 5;
  BBSprite: Active_3, 5;
EndLoop:
BBSprite: End_1, 5;
BBSprite: End_2, 5;
BBSprite: End_3, 5;
GotoBehavior: Glin_TeleportOut;

@CastGlinSpike:
# 1
WaitFrame: 10;
CreateBullet: GlinSpike_Step2
  BulletPosition: -180000, -115000;
EndCreateBullet:
# 2
WaitFrame: 10;
CreateBullet: GlinSpike_Step2
  BulletPosition: -150000, -115000;
EndCreateBullet:
# 3
WaitFrame: 10;
CreateBullet: GlinSpike_Step2
  BulletPosition: -120000, -115000;
EndCreateBullet:
# 4
WaitFrame: 10;
CreateBullet: GlinSpike_Step2
  BulletPosition: -90000, -115000;
EndCreateBullet:
# 5
WaitFrame: 10;
CreateBullet: GlinSpike_Step2
  BulletPosition: -60000, -115000;
EndCreateBullet:
# 6
WaitFrame: 10;
CreateBullet: GlinSpike_Step2
  BulletPosition: -30000, -115000;
EndCreateBullet:
# 7
WaitFrame: 10;
CreateBullet: GlinSpike_Step2
  BulletPosition: 0, -115000;
EndCreateBullet:
# 8
WaitFrame: 10;
CreateBullet: GlinSpike_Step2
  BulletPosition: 30000, -115000;
EndCreateBullet:
# 9
WaitFrame: 10;
CreateBullet: GlinSpike_Step2
  BulletPosition: 60000, -115000;
EndCreateBullet:
# 10
WaitFrame: 10;
CreateBullet: GlinSpike_Step2
  BulletPosition: 90000, -115000;
EndCreateBullet:
# 11
WaitFrame: 10;
CreateBullet: GlinSpike_Step2
  BulletPosition: 120000, -115000;
EndCreateBullet:
# 12
WaitFrame: 10;
CreateBullet: GlinSpike_Step2
  BulletPosition: 150000, -115000;
EndCreateBullet:
# 13
WaitFrame: 10;
CreateBullet: GlinSpike_Step2
  BulletPosition: 180000, -115000;
EndCreateBullet:
return;

[Glin_Step2_Slash]
@Trigger:
return;

@Main:
# 1. Slash
SetPos: 50000, -100000;
BBSprite: Slash_Start_1, 4;
BBSprite: Slash_Start_2, 4;
RegistCounter: 20;
BeginLoop: (Counter: Value > 0)
  BBSprite: Slash_Start_3, 5;
  BBSprite: Slash_Start_4, 5;
EndLoop:
SetVelocity: 700000, 0;
BBSprite: Slash_Active_1, 4;
AccelX: 700000, 12, -3500000;
BBSprite: Slash_Active_2, 4;
BBSprite: Slash_Active_3, 4;
BBSprite: Slash_Active_4, 4;
SetVelocity: 0, 0;
# 2. UpperCut
BBSprite: UpperCut_Start_1, 3;
BBSprite: UpperCut_Start_2, 5;
BBSprite: UpperCut_Start_3, 5;
BBSprite: UpperCut_Start_4, 5;
BBSprite: UpperCut_Start_5, 3;
SetVelocity: 100000, 700000;
BBSprite: UpperCut_Active_1, 4;
SetVelocity: 80000, 600000;
BBSprite: UpperCut_Active_2, 5;
SetVelocity: 50000, 300000;
BBSprite: UpperCut_Active_2, 5;
SetVelocity: 30000, 100000;
BBSprite: UpperCut_Active_2, 5;
SetVelocity: 0, 0;
BBSprite: UpperCut_End_1, 4;
GotoBehavior: Glin_TeleportOut;


[Glin_Step2_Cast]
@Trigger:
return;

@Main:
# 1. Evade
EnableRangeCheck: true, 50000, 0, 0;
WaitFrame: 5;
BeginIf: (InRange: true)
  # Evade_Start
  BBSprite: Evade_1, 4;
  BBSprite: Evade_2, 4;
  SetVelocityX: -320000;
  BBSprite: Evade_3, 3;
  BBSprite: Evade_4, 3;
  BBSprite: Evade_5, 3;
  BBSprite: Evade_4, 3;
  BBSprite: Evade_3, 3;
  # Evade_End
  SetVelocityX: -50000;
  BBSprite: Evade_6, 4;
  SetVelocityX: 0;
  BBSprite: Evade_7, 4;
EndIf:
EnableRangeCheck: false, 0, 0, 0;
# 2. Cast
# Cast_Start
BBSprite: Cast_Start_1, 5;
BBSprite: Cast_Start_2, 5;
BBSprite: Cast_Start_3, 5;
BBSprite: Cast_Start_4, 5;
# Cast_Active
BBSprite: Cast_Active_1, 5;
BBSprite: Cast_Active_2, 5;
RegistCounter: 130;
CallSubCoroutine: CastGlinBullet;
BeginLoop: (Counter: Value > 0)
  BBSprite: Cast_Active_3, 8;
  BBSprite: Cast_Active_4, 8;
EndLoop:
# Cast_End
BBSprite: Cast_End_1, 5;
BBSprite: Cast_End_2, 5;
BBSprite: Cast_End_3, 5;
GotoBehavior: Glin_TeleportOut;

# 飞弹协程
@CastGlinBullet:
# 1
ScreenShake: 550, 550, 10000, 10;
CreateBullet: GlinBullet
  BulletFlip: Left;
  BulletVelocity: -350000, 0;
  BulletPosition: -20000, 10000;
  BulletAccelY: -20000, 100, 150000;
EndCreateBullet:
WaitFrame: 25;
# 2
ScreenShake: 550, 550, 10000, 10;
CreateBullet: GlinBullet
  BulletFlip: Left;
  BulletVelocity: -350000, 0;
  BulletPosition: -15000, 5000;
  BulletAccelY: -20000, 100, 150000;
EndCreateBullet:
WaitFrame: 25;
# 3
ScreenShake: 550, 550, 10000, 10;
CreateBullet: GlinBullet
  BulletFlip: Left;
  BulletVelocity: -350000, 0;
  BulletPosition: -10000, 0;
  BulletAccelY: -20000, 100, 150000;
EndCreateBullet:
WaitFrame: 25;
# 4
ScreenShake: 550, 550, 10000, 10;
CreateBullet: GlinBullet
  BulletFlip: Left;
  BulletVelocity: -350000, 0;
  BulletPosition: -5000, -5000;
  BulletAccelY: -20000, 100, 150000;
EndCreateBullet:
WaitFrame: 25;
# 5
ScreenShake: 550, 550, 10000, 10;
CreateBullet: GlinBullet
  BulletFlip: Left;
  BulletVelocity: -350000, 0;
  BulletPosition: -0, -10000;
  BulletAccelY: -20000, 100, 150000;
EndCreateBullet:
return;
