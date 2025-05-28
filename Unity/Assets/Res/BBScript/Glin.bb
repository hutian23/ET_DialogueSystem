[Root]
@RootInit:
EnemyInit;
# bullet由对象池管理
PoolObject: GlinBullet, 5;
PoolObject: GlinSpikes, 1;
PoolObject: GlinSpike, 3;
PoolObject: Goam, 4;
PoolObject: GlinFireball, 30;
PoolObject: ADust, 1;
PoolObject: GDust, 1;
# 数值初始化
HP: 3000; # 初始血量300
HPLock: 1500; # 一阶段锁血
EnableAirCheck;
# 注册行为
# Step1
RegistMove: (Glin_Idle)
  MoveType: None;
EndMove:
RegistMove: (Glin_Bow)
  MoveType: None;
EndMove:
RegistMove: (Glin_Slash)
  MoveType: None;
EndMove:
RegistMove: (Glin_Capespike)
  MoveType: None;
EndMove:
RegistMove: (Glin_AirDash)
  MoveType: None;
EndMove:
RegistMove: (Glin_Cast)
  MoveType: None;
EndMove:
RegistMove: (Glin_Ballon)
  MoveType: None;
EndMove:
RegistMove: (Glin_Teleport)
  MoveType: None;
EndMove:
# Step2
RegistMove: (Glin_Roar)
  MoveType: None;
EndMove:
RegistMove: (Glin_Step2_Teleport)
  MoveType: None;
EndMove:
RegistMove: (Glin_Step2_FeintSlash)
  MoveType: None;
EndMove:
RegistMove: (Glin_Step2_CastSpike)
  MoveType: None;
EndMove:
RegistMove: (Glin_Step2_Slash)
  MoveType: None;
EndMove:
RegistMove: (Glin_Step2_Cast)
  MoveType: None;
EndMove:
RegistMove: (Glin_Step2_AirDash)
  MoveType: None;
EndMove:
RegistMove: (Glin_Step2_Ballon)
  MoveType: None;
EndMove:
RegistMove: (Glin_Explode)
  MoveType: None;
EndMove:
GotoBehavior: Glin_Roar;

@HPWatcher:
# 血量低于50%, 进入二阶段
BeginIf: (HP: Value <= 1500)
  GotoBehavior: Glin_Explode;
EndIf:
return;

[Glin_Idle]
@Trigger:
return;

@Main:
SetPos: 90000, -95000;
EnableInRangeCheck: true, 80000, 0, 0;
BeginLoopAnim: (InRange: false)
  LoopSprite: Idle_1, 5;
  LoopSprite: Idle_2, 5;
  LoopSprite: Idle_3, 5;
  LoopSprite: Idle_4, 5;
  LoopSprite: Idle_5, 5;
  LoopSprite: Idle_6, 5;
  LoopSprite: Idle_7, 5;
  LoopSprite: Idle_8, 5;
  LoopSprite: Idle_9, 5;
  LoopSprite: Idle_10, 5;
  LoopSprite: Idle_11, 5;
  LoopSprite: Idle_12, 5;
EndLoopAnim:
EnableInRangeCheck: false, 0, 0, 0;
RegistCounter: 100;
BeginLoopAnim: (Counter: Value > 0)
  LoopSprite: Idle_1, 5;
  LoopSprite: Idle_2, 5;
  LoopSprite: Idle_3, 5;
  LoopSprite: Idle_4, 5;
  LoopSprite: Idle_5, 5;
  LoopSprite: Idle_6, 5;
  LoopSprite: Idle_7, 5;
  LoopSprite: Idle_8, 5;
  LoopSprite: Idle_9, 5;
  LoopSprite: Idle_10, 5;
  LoopSprite: Idle_11, 5;
  LoopSprite: Idle_12, 5;
EndLoopAnim:
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
GotoBehavior: Glin_Teleport;

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
BeginLoopAnim: (Counter: Value > 0)
  LoopSprite: Slash_4, 6;
  LoopSprite: Slash_5, 6;
EndLoopAnim:
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
GotoBehavior: Glin_Teleport;

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
ScreenShake: 850, 0, 12000, 15, 0;
BBSprite: Capespike_8, 5;
# Cast Spike
CreateBullet: GlinSpikes
  BulletAbsolutePosition: 0, -100000;
EndCreateBullet:
RegistCounter: 150;
BeginLoopAnim: (Counter: Value > 0)
  LoopSprite: Capespike_9, 7;
  LoopSprite: Capespike_10, 7;
EndLoopAnim:
# Cast End
BBSprite: Capespike_8, 5;
BBSprite: Capespike_7, 5;
BBSprite: Capespike_6, 5;
BBSprite: Capespike_5, 5;
BBSprite: Capespike_4, 5;
BBSprite: Capespike_3, 5;
GotoBehavior: Glin_Teleport;

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
Shake: 450, 450, 8000, 60, 1; # 空箭蓄力过程中 屏幕振动
RegistCounter: 60; # 空箭蓄力60帧
BeginLoopAnim: (Counter: Value > 0)
  LoopSprite: AirDash_Active_1, 5;
  LoopSprite: AirDash_Active_2, 5;
  LoopSprite: AirDash_Active_3, 5;
EndLoopAnim:
EnableGlinChase: false, 0, 0, 0;
# 2. 下冲
AirDashVelocity: -700000;
SpawnADust: 0, 27000, 8000, 12000, 900000; # 冲刺起始，生成AirDust特效(根据unit当前Rotate调整对应的rotate)
BeginLoopAnim: (InAir: true)
  LoopSprite: AirDash_Active_1, 4;
  LoopSprite: AirDash_Active_2, 4;
  LoopSprite: AirDash_Active_3, 4;
EndLoopAnim:
# 3. 落地
SetRotate: 0;    
EnemyUpdateFlip; # 根据玩家当前位置调整地面冲刺朝向
SpawnGDust: -65000, -20000, -8000, 4000; # 生成Ground Dust特效
SpawnGDust: 65000, -20000, 8000, 4000;
SetVelocity: 0, -50000;
ScreenShake: 1550, 550, 10000, 20, 0; # 落地的振动效果
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
GotoBehavior: Glin_Teleport;

[Glin_Cast]
@Trigger:
return;

@Main:
# Start
BBSprite: Start_1, 5;
BBSprite: Start_2, 5;
BBSprite: Start_3, 5;
BBSprite: Start_4, 10;
# 发射飞弹后，表现披风被振动的效果
# Cast Bullet_1
ScreenShake: 550, 550, 10000, 15, 0;
BBSprite: Active_1, 5;
BBSprite: Active_2, 4;
CreateBullet: GlinBullet
  BulletFlip: Left;
  BulletVelocity: -400000, 0;
  BulletLocalPosition: -20000, 10000;
  BulletAccelY: -20000, 100, 150000;
EndCreateBullet:
BBSprite: Active_2, 4;
BBSprite: Active_3, 6;
BBSprite: Active_4, 10;
# Cast Bullet_2
ScreenShake: 550, 550, 10000, 15, 0;
CreateBullet: GlinBullet
  BulletFlip: Left;
  BulletVelocity: -400000, 0;
  BulletLocalPosition: -15000, 5000;
  BulletAccelY: -20000, 100, 150000;
EndCreateBullet:
BBSprite: Active_2, 8;
BBSprite: Active_3, 6;
BBSprite: Active_4, 10;
# Cast Bullet_3
ScreenShake: 550, 550, 10000, 15, 0;
CreateBullet: GlinBullet
  BulletFlip: Left;
  BulletVelocity: -400000, 0;
  BulletLocalPosition: -10000, 0;
  BulletAccelY: -20000, 100, 150000;
EndCreateBullet:
BBSprite: Active_2, 8;
BBSprite: Active_3, 6;
BBSprite: Active_4, 10;
# End
BBSprite: Start_4, 4;
BBSprite: Start_3, 4;
BBSprite: Start_2, 4;
BBSprite: Start_1, 4;
GotoBehavior: Glin_Teleport;

[Glin_Ballon]
@Trigger:
return;

@Main:
# Cast Fireball
# 发射飞弹时屏幕振动
BBSprite: Anticipate_1, 5;
BBSprite: Anticipate_2, 5;
RegistCounter: 40;
BeginLoopAnim: (Counter: Value > 0)
  LoopSprite: Active_1, 5;
  LoopSprite: Active_2, 5;
  LoopSprite: Active_3, 5;
EndLoopAnim:
ScreenShake: 250, 250, 8000, 200, 1;
# 纵向飞弹，y轴速度不变，x轴速度飞行过程中略微增大
# 横向飞弹，x轴速度不变，y轴速度逐渐趋于0
Enable_CastGlinFireball: true, 35, -85000, 20000, 160000;
RegistCounter: 200;
BeginLoopAnim: (Counter: Value > 0)
  LoopSprite: Active_1, 5;
  LoopSprite: Active_2, 5;
  LoopSprite: Active_3, 5;
EndLoopAnim:
# Cast End
Enable_CastGlinFireball: false, 0, 0, 0, 0;
BBSprite: Anticipate_2, 7;
BBSprite: Anticipate_1, 5;
GotoBehavior: Glin_Teleport;

[Glin_Teleport]
@Trigger:
return;

@Main:
# Teleport Out
SetVelocity: 0, 0;
BBSprite: Frame_6, 5;
BBSprite: Frame_7, 5;
ScreenShake: 750, 750, 10000, 15, 0;
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
ScreenShake: 750, 750, 10000, 15, 0;
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

[Glin_Roar]
@Trigger:
return;

@Main:
SetPos: 0, -120000;
BBSprite: Anticipate_1, 5;
ScreenShake: 550, 550, 10000, 100, 1;
RegistCounter: 100;
BeginLoopAnim: (Counter: Value > 0)
  LoopSprite: Roar_1, 5;
  LoopSprite: Roar_2, 5;
  LoopSprite: Roar_3, 5;
EndLoopAnim:
GotoBehavior: Glin_Step2_Teleport;

[Glin_Step2_Teleport]
@Trigger:
return;

@Main:
# Teleport Out
SetVelocity: 0, 0;
BBSprite: Frame_6, 5;
BBSprite: Frame_7, 5;
ScreenShake: 750, 750, 10000, 15, 0;
BBSprite: Frame_3, 5;
BBSprite: Frame_2, 5;
BBSprite: Frame_1, 5;
SetPos: -1000000, -1000000;
WaitFrame: 50;
# Select Next Behavior
Random: ran1, 0, 100;
# 1. FeintSlash
BeginIf: (Random: ran1 >= 0), (Random: ran1 < 15)
  GlinPos: -140000, 140000, 60000, -95000;
EndIf:
# 2. Slash
BeginIf: (Random: ran1 >= 15), (Random: ran1 < 30)
  GlinPos: -140000, 140000, 60000, -95000;
EndIf:
# 3. Cast
BeginIf: (Random: ran1 >= 30), (Random: ran1 < 50)
  EnableInRangeCheck: true, 80000, 0, 0;
  GlinPos: -140000, 140000, 80000, -95000;
EndIf:
# 4. CastSpike
BeginIf: (Random: ran1 >= 50), (Random: ran1 < 65)
  SetPos: 0, 0;
  SetFlip: Left;
EndIf:
# 5. AirDash
BeginIf: (Random: ran1 >= 65), (Random: ran1 < 85)
  AirDashPos: 15000;
  SetFlip: Left;
EndIf:
# 6. Ballon
BeginIf: (Random: ran1 >= 85), (Random: ran1 <= 100)
  SetPos: 0, 0;
  SetFlip: Left;
EndIf:
# Teleport In
BBSprite: Frame_1, 5;
BBSprite: Frame_2, 5;
ScreenShake: 750, 750, 10000, 15, 0;
BBSprite: Frame_3, 5;
BBSprite: Frame_4, 5;
# Enter Next Behavior
# 1. FeintSlash
BeginIf: (Random: ran1 >= 0), (Random: ran1 < 10)
  GotoBehavior: Glin_Step2_FeintSlash;
EndIf:
# 2. Slash
BeginIf: (Random: ran1 >= 10), (Random: ran1 < 30)
  GotoBehavior: Glin_Step2_Slash;
EndIf:
# 3. Cast
BeginIf: (Random: ran1 >= 30), (Random: ran1 < 50)
  BeginIf: (InRange: true)
    SetTransition: Evade, true;
  EndIf:
  GotoBehavior: Glin_Step2_Cast;
EndIf:
# 4. CastSpike
BeginIf: (Random: ran1 >= 50), (Random: ran1 < 65)
  GotoBehavior: Glin_Step2_CastSpike;
EndIf:
# 5. AirDash
BeginIf: (Random: ran1 >= 65), (Random: ran1 < 85)
  GotoBehavior: Glin_Step2_AirDash;
EndIf:
# 6. Ballon
BeginIf: (Random: ran1 >= 85), (Random: ran1 <= 100)
  GotoBehavior: Glin_Step2_Ballon;
EndIf:
return;

[Glin_Step2_FeintSlash]
@Trigger:
return;

@Main:
# 假动作
BBSprite: Feint_1, 5;
BBSprite: Feint_2, 5; 
RegistCounter: 25;
BeginLoopAnim: (Counter: Value > 0)
  LoopSprite: Feint_3, 6;
  LoopSprite: Feint_4, 6;
EndLoopAnim:
SetTransition: Evade, true;
GotoBehavior: Glin_Step2_Cast;

[Glin_Step2_CastSpike]
@Trigger:
return;

@Main:
BBSprite: Anticipate_1, 5;
CallSubCoroutine: Glin_Step2_CastSpike, CastGlinSpike;
Shake: 300, 300, 8000, 350, 1;
RegistCounter: 350;
BeginLoopAnim: (Counter: Value > 0)
  LoopSprite: Active_1, 5;
  LoopSprite: Active_2, 5;
  LoopSprite: Active_3, 5;
EndLoopAnim:
BBSprite: End_1, 5;
BBSprite: End_2, 5;
BBSprite: End_3, 5;
GotoBehavior: Glin_Step2_Teleport;

@CastGlinSpike:
# Bullet 1
WaitFrame: 20;
CreateBullet: Goam
EndCreateBullet:
# Bullet 2
WaitFrame: 50;
CreateBullet: Goam
EndCreateBullet:
# Bullet 3
WaitFrame: 50;
CreateBullet: Goam
EndCreateBullet:
# Bullet 4
WaitFrame: 50;
CreateBullet: Goam
EndCreateBullet:
return;

[Glin_Step2_Slash]
@Trigger:
return;

@Main:
# 1. Slash
BBSprite: Slash_Start_1, 4;
BBSprite: Slash_Start_2, 4;
RegistCounter: 20;
BeginLoopAnim: (Counter: Value > 0)
  LoopSprite: Slash_Start_3, 5;
  LoopSprite: Slash_Start_4, 5;
EndLoopAnim:
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
GotoBehavior: Glin_Step2_Teleport;


[Glin_Step2_Cast]
@Trigger:
return;

@Main:
# Evade
BeginIf: (TransitionCached: Evade, true)
  BBSprite: Evade_1, 4;
  BBSprite: Evade_2, 4;
    SetVelocityX: -350000;
  BBSprite: Evade_3, 4;
  BBSprite: Evade_4, 4;
  BBSprite: Evade_5, 4;
  BBSprite: Evade_4, 4;
  SetVelocityX: 0;
  BBSprite: Evade_6, 4;
  BBSprite: Evade_7, 4;
EndIf:
# Start
BBSprite: Cast_Start_1, 5;
BBSprite: Cast_Start_2, 5;
BBSprite: Cast_Start_3, 5;
BBSprite: Cast_Start_4, 10;
# 发射飞弹后，表现披风被振动的效果
# Cast Bullet_1
BBSprite: Cast_Active_1, 5;
BBSprite: Cast_Active_2, 4;
ScreenShake: 550, 550, 10000, 15, 0;
CreateBullet: GlinBullet
  BulletFlip: Left;
  BulletVelocity: -400000, 0;
  BulletLocalPosition: -20000, 10000;
  BulletAccelY: -20000, 100, 150000;
EndCreateBullet:
BBSprite: Cast_Active_2, 4;
BBSprite: Cast_Active_3, 6;
BBSprite: Cast_Active_4, 10;
# Cast Bullet_2
ScreenShake: 550, 550, 10000, 15, 0;
CreateBullet: GlinBullet
  BulletFlip: Left;
  BulletVelocity: -400000, 0;
  BulletLocalPosition: -15000, 5000;
  BulletAccelY: -20000, 100, 150000;
EndCreateBullet:
BBSprite: Cast_Active_2, 8;
BBSprite: Cast_Active_3, 6;
BBSprite: Cast_Active_4, 10;
# Cast Bullet_3
ScreenShake: 550, 550, 10000, 15, 0;
CreateBullet: GlinBullet
  BulletFlip: Left;
  BulletVelocity: -400000, 0;
  BulletLocalPosition: -10000, 0;
  BulletAccelY: -20000, 100, 150000;
EndCreateBullet:
BBSprite: Cast_Active_2, 8;
BBSprite: Cast_Active_3, 6;
BBSprite: Cast_Active_4, 10;
# Cast Bullet_4
ScreenShake: 550, 550, 10000, 15, 0;
CreateBullet: GlinBullet
  BulletFlip: Left;
  BulletVelocity: -400000, 0;
  BulletLocalPosition: -5000, -5000;
  BulletAccelY: -20000, 100, 150000;
EndCreateBullet:
BBSprite: Cast_Active_2, 8;
BBSprite: Cast_Active_3, 6;
BBSprite: Cast_Active_4, 10;
# End
BBSprite: Cast_End_1, 5;
BBSprite: Cast_End_2, 5;
BBSprite: Cast_End_3, 5;
GotoBehavior: Glin_Step2_Teleport;

[Glin_Step2_AirDash]
@Trigger:
return;

@Main:
# AirDash Anticipate
BBSprite: AirDash_Anticipate_1, 4;
BBSprite: AirDash_Anticipate_2, 4;
BBSprite: AirDash_Anticipate_3, 4;
BBSprite: AirDash_Anticipate_4, 4;
BBSprite: AirDash_Anticipate_5, 4;
BBSprite: AirDash_Anticipate_6, 4;
# AirDash Charge
BBSprite: AirDash_Active_1, 4;
EnableGlinChase: true, -450000, 450000, 80000;
Shake: 850, 850, 12000, 60, 1; 
RegistCounter: 60;
BeginLoopAnim: (Counter: Value > 0)
  LoopSprite: AirDash_Active_2, 5;
  LoopSprite: AirDash_Active_3, 5;
  LoopSprite: AirDash_Active_4, 5;
EndLoopAnim:
# Down Dash
EnableGlinChase: false, 0, 0, 0;
AirDashVelocity: -700000;
SpawnADust: 0, 27000, 8000, 12000, 900000;
BeginLoopAnim: (InAir: true)
  LoopSprite: AirDash_Active_2, 4;
  LoopSprite: AirDash_Active_3, 4;
  LoopSprite: AirDash_Active_4, 4;
EndLoopAnim:
# Land 
SetRotate: 0;  
EnemyUpdateFlip;
SpawnGDust: -65000, -20000, -8000, 4000;
SpawnGDust: 65000, -20000, 8000, 4000;
SetVelocity: 0, -50000;
ScreenShake: 1550, 550, 10000, 20, 0;
BBSprite: Land_1, 8;
BBSprite: Land_2, 4;
BBSprite: Land_3, 8;
BBSprite: Land_4, 4;
# GroundDash
BBSprite: Dash_Active_1, 4;
SpawnGDust: 80000, -15000, 10000, 6000;
SetVelocity: 750000, 0;
BBSprite: Dash_Active_2, 5;
AccelX: 750000, 12, -3800000;
BBSprite: Dash_Active_3, 4;
BBSprite: Dash_Active_4, 4;
BBSprite: End_1, 4;
SetVelocityX: 0;
BBSprite: End_2, 4;
BBSprite: End_3, 4;
BBSprite: End_4, 4;
GotoBehavior: Glin_Step2_Teleport;

[Glin_Step2_Ballon]
@Trigger:
return;

@Main:
BBSprite: Anticipate_1, 4;
BBSprite: Anticipate_2, 4;
Shake: 600, 600, 10000, 200, 1;
CallSubCoroutine: Glin_Step2_Ballon, CastFireBallCoroutine;
RegistCounter: 200;
BeginLoopAnim: (Counter: Value > 0)
  LoopSprite: Active_1, 5;
  LoopSprite: Active_2, 5;
  LoopSprite: Active_3, 5;
EndLoopAnim:
BBSprite: End_1, 5;
BBSprite: End_2, 5;
GotoBehavior: Glin_Step2_Teleport;

@CastFireBallCoroutine:
WaitFrame: 30;
EnableCastGlinFireball: 160, 40, -85000, 20000, 160000;
return;

[Glin_Explode]
@Trigger:
return;

@Main:
SetVelocity: 0, 0;
Shake: 1500, 1500, 10000, 20, 0;
BBSprite: Explode_1, 20;
BBSprite: Explode_2, 4;
ScreenShake: 1250, 1250, 12000, 30, 0; 
BBSprite: Explode_3, 6;
BBSprite: Explode_4, 4;
BBSprite: Explode_5, 4;
SetPos: 1000000, 1000000;
WaitFrame: 60;
# 怪物波次_1
MonsterWave_Init;
SpawnEnemy: Zako2
  SpawnEnemy_Position: -100000, -110000;
  MonsterWave_RegistEnemy;
EndSpawnEnemy:
MonsterWave_WaitClear;
WaitFrame: 100;
# 怪物波次_2
MonsterWave_Init;
SpawnEnemy: Zako3
  SpawnEnemy_Position: -100000, 65000;
  SpawnEnemy_Flip: Right;
  MonsterWave_RegistEnemy; # 在怪物unit的deathBuff添加MoveWaveFlag，怪物死亡时销毁deathBuff 
EndSpawnEnemy:
SpawnEnemy: Zako3
  SpawnEnemy_Position: 100000, 65000;
  SpawnEnemy_Flip: Left;
  MonsterWave_RegistEnemy; 
EndSpawnEnemy:
MonsterWave_WaitClear; # 当前波次中的怪物全部消灭
WaitFrame: 100;
SetPos: 0, 0;
GotoBehavior: Glin_Step2_CastSpike;