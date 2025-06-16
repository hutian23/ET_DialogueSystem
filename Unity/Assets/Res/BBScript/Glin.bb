[Root]
@RootInit:
EnemyInit;
# bullet由对象池管理
PoolObject: Glin_Hand, 1;
PoolObject: GlinBullet, 5;
PoolObject: GlinSpike, 10;
PoolObject: Goam, 4;
PoolObject: GlinFireball, 40;
PoolObject: ADust, 1;
PoolObject: GDust, 1;
# 数值初始化
HP: 3000; # 初始血量300
HPLock: 1500; # 一阶段锁血
EnableAirCheck;
RegistEndBattleCallback: Root, BattleCallback; # 战斗结束，播放结束动画
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
RegistMove: (Glin_Death)
  MoveType: Death;
EndMove:
RegistMove: (Glin_Exit)
  MoveType: None;
EndMove:
RegistMove: (Glin_Bow2)
  MoveType: None;
EndMove:
GotoBehavior: Glin_Idle;

@HPWatcher:
# 死亡逻辑
BeginIf: (HP: Value <= 0)
  GotoBehavior: Glin_Death;
EndIf:
# 血量低于50%, 进入二阶段
BeginIf: (HP: Value <= 1500)
  GotoBehavior: Glin_Explode;
EndIf:
return;

@BattleCallback:
GotoBehavior: Glin_Bow2;
return;

[Glin_Idle]
@Trigger:
return;

@Main:
SetPos: 90000, -95000;
EnableInRangeCheck: true, 80000, 0, 0;
RegistInRangeCallback: Glin_Idle, InRangeCallback;
SetMarker: Loop;
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
GotoMarker: Loop;
return;

@InRangeCallback:
WaitFrame: 100;
GotoBehavior: Glin_Bow;
return;

[Glin_Bow]
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
BBSprite: Slash_1, 4;
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
CallSubCoroutine: Glin_Capespike, CastSpikes;
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

@CastSpikes:
CreateBullet: GlinSpike
  BulletAngle: 30000;
  BulletAbsolutePosition: -200000, -120000;
EndCreateBullet:
CreateBullet: GlinSpike
  BulletAngle: 20000;
  BulletAbsolutePosition: -160000, -120000;
EndCreateBullet:
CreateBullet: GlinSpike
  BulletAngle: -45000;
  BulletAbsolutePosition: -120000, -120000;
EndCreateBullet:
CreateBullet: GlinSpike
  BulletAngle: 0;
  BulletAbsolutePosition: -80000, -120000;
EndCreateBullet:
CreateBullet: GlinSpike
  BulletAngle: 12000;
  BulletAbsolutePosition: -40000, -120000;
EndCreateBullet:
CreateBullet: GlinSpike
  BulletAngle: -22000;
  BulletAbsolutePosition: 0, -120000;
EndCreateBullet:
CreateBullet: GlinSpike
  BulletAngle: -12000;
  BulletAbsolutePosition: 40000, -120000;
EndCreateBullet:
CreateBullet: GlinSpike
  BulletAngle: -18000;
  BulletAbsolutePosition: 80000, -120000;
EndCreateBullet:
CreateBullet: GlinSpike
  BulletAngle: 12000;
  BulletAbsolutePosition: 120000, -120000;
EndCreateBullet:
CreateBullet: GlinSpike
  BulletAngle: 62000;
  BulletAbsolutePosition: 160000, -120000;
EndCreateBullet:
CreateBullet: GlinSpike
  BulletAbsolutePosition: 200000, -120000;
EndCreateBullet:
return;

[Glin_AirDash]
@Main:
# 1. 悬停在空中
BBSprite: AirDash_Anticipate_1, 5; # 播放对应的动画帧，包括Sprite、Hitbox，第二个参数表示动画帧的持续帧数
BBSprite: AirDash_Anticipate_2, 5;
BBSprite: AirDash_Anticipate_3, 5;
BBSprite: AirDash_Anticipate_4, 5;
BBSprite: AirDash_Anticipate_5, 5;
BBSprite: AirDash_Anticipate_6, 5;
BBSprite: AirDash_Anticipate_7, 5;
EnableGlinChase: true, -450000, 450000, 80000; #添加GlinChase组件，组件作用为每帧检测玩家位置，调整Rotation(此处的参数为万分制)
Shake: 450, 450, 8000, 60, 1; # 蓄力过程中 Boss振动
# 2. 蓄力阶段，在空中悬停60帧
RegistCounter: 60; 
BeginLoopAnim: (Counter: Value > 0) # 循环播放下面三个动画帧
  LoopSprite: AirDash_Active_1, 5;
  LoopSprite: AirDash_Active_2, 5;
  LoopSprite: AirDash_Active_3, 5;
EndLoopAnim:
EnableGlinChase: false, 0, 0, 0; # 移除GlinChase组件
# 3. 向下冲刺
AirDashVelocity: -700000;
SpawnADust: 0, 27000, 8000, 12000, 900000; # 冲刺起始，生成AirDust特效(根据Boss当前Rotate调整特效的rotate)
BeginLoopAnim: (InAir: true) # 每帧检测地面，检测到地面切换到 4
  LoopSprite: AirDash_Active_1, 4;
  LoopSprite: AirDash_Active_2, 4;
  LoopSprite: AirDash_Active_3, 4;
EndLoopAnim:
# 4. 落地
SetAngle: 0; 
EnemyUpdateFlip; # 根据玩家当前位置，调整水平冲刺的方向
SpawnGDust: -65000, -20000, -8000, 4000; # 生成Ground Dust特效
SpawnGDust: 65000, -20000, 8000, 4000;
SetVelocity: 0, -50000;
ScreenShake: 1550, 550, 10000, 20, 0; # 砸地，屏幕振动
# 5. 水平冲刺
# 地面冲刺起始期
BBSprite: GroundDash_Anticipate_1, 10;
BBSprite: GroundDash_Anticipate_2, 5;
BBSprite: GroundDash_Anticipate_3, 10;
BBSprite: GroundDash_Anticipate_4, 5;
# 地面冲刺攻击判定持续期
BBSprite: GroundDash_Active_1, 5;
SpawnGDust: 80000, -15000, 10000, 6000;
SetVelocity: 700000, 0; # 地面冲刺初始速度
BBSprite: GroundDash_Active_2, 5;
AccelX: 700000, -2800000, 15; # 减速
BBSprite: GroundDash_Active_3, 5;
BBSprite: GroundDash_Active_4, 5;
BBSprite: GroundDash_Anticipate_1, 5;
SetVelocityX: 0; # 水平速度归0
BBSprite: GroundDash_Anticipate_2, 5;
BBSprite: GroundDash_Anticipate_3, 5;
BBSprite: GroundDash_Anticipate_4, 5;
# 6. 隐身，然后切换到下一个动作
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
Shake: 300, 300, 10000, 100, 1;
CallSubCoroutine: Glin_Ballon, CastFireBallCoroutine;
RegistCounter: 200;
BeginLoopAnim: (Counter: Value > 0)
  LoopSprite: Active_1, 5;
  LoopSprite: Active_2, 5;
  LoopSprite: Active_3, 5;
EndLoopAnim:
BBSprite: Anticipate_2, 7;
BBSprite: Anticipate_1, 5;
GotoBehavior: Glin_Teleport;

@CastFireBallCoroutine:
WaitFrame: 30;
EnableCastGlinFireball: 160, 40, -85000, 20000, 160000;
return;

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
@Main:
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
# Glin_Explode ---> Glin_Step2_Teleport，不希望执行这部分逻辑
BeginIf: (TransitionCached: NoTeleportOut, false)
  SetVelocity: 0, 0;
  BBSprite: Frame_6, 5;
  BBSprite: Frame_7, 5;
  ScreenShake: 750, 750, 10000, 15, 0;
  BBSprite: Frame_3, 5;
  BBSprite: Frame_2, 5;
  BBSprite: Frame_1, 5;
  SetPos: -1000000, -1000000;
  WaitFrame: 50;
  # Glin_Step2_Slash执行完毕后会生成地刺，等地刺消失才执行下个行为
  BeginIf: (TransitionCached: Step2_Slash, true)
    WaitFrame: 80;
  EndIf:
EndIf:
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
  AirDashPos: 0;
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
# 2. Slash
BeginIf: (Random: ran1 >= 0), (Random: ran1 < 30)
  GotoBehavior: Glin_Step2_Slash;
EndIf:
# 3. Cast
BeginIf: (Random: ran1 >= 30), (Random: ran1 < 50)
  # 和玩家距离太近，避免玩家来不及躲闪子弹，向后移动一段距离
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
BeginIf: (Random: ran1 >= 65), (Random: ran1 < 90)
  GotoBehavior: Glin_Step2_AirDash;
EndIf:
# 6. Ballon
BeginIf: (Random: ran1 >= 90), (Random: ran1 <= 100)
  GotoBehavior: Glin_Step2_Ballon;
EndIf:
return;

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
# 1. 劈砍蓄力
BBSprite: Slash_Start_1, 4;
BBSprite: Slash_Start_2, 4;
RegistCounter: 20;
BeginLoopAnim: (Counter: Value > 0)
  LoopSprite: Slash_Start_3, 5;
  LoopSprite: Slash_Start_4, 5;
EndLoopAnim:
# 2. 有一定概率假动作，过渡到Cast行为中
Random: ran1, 0, 100;
BeginIf: (Random: ran1 > 50)
  SetTransition: Evade, true;
  GotoBehavior: Glin_Step2_Cast;
EndIf:
# 3. 向前方冲刺劈砍
SetVelocity: 700000, 0;
BBSprite: Slash_Active_1, 4;
AccelX: 700000, -3500000, 12;
BBSprite: Slash_Active_2, 4;
BBSprite: Slash_Active_3, 4;
BBSprite: Slash_Active_4, 4;
SetVelocity: 0, 0;
# 4. 升龙
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
# 5. 生成地刺
CallSubCoroutine: Glin_Step2_Slash, SpawnSpikes;
SetTransition: Step2_Slash, true;
GotoBehavior: Glin_Step2_Teleport;

@SpawnSpikes:
CreateBullet: GlinSpike
  BulletAngle: 30000;
  BulletAbsolutePosition: -200000, -120000;
EndCreateBullet:
CreateBullet: GlinSpike
  BulletAngle: 20000;
  BulletAbsolutePosition: -160000, -120000;
EndCreateBullet:
CreateBullet: GlinSpike
  BulletAngle: -45000;
  BulletAbsolutePosition: -120000, -120000;
EndCreateBullet:
CreateBullet: GlinSpike
  BulletAngle: 0;
  BulletAbsolutePosition: -80000, -120000;
EndCreateBullet:
CreateBullet: GlinSpike
  BulletAngle: 12000;
  BulletAbsolutePosition: -40000, -120000;
EndCreateBullet:
CreateBullet: GlinSpike
  BulletAngle: -22000;
  BulletAbsolutePosition: 0, -120000;
EndCreateBullet:
CreateBullet: GlinSpike
  BulletAngle: -12000;
  BulletAbsolutePosition: 40000, -120000;
EndCreateBullet:
CreateBullet: GlinSpike
  BulletAngle: -18000;
  BulletAbsolutePosition: 80000, -120000;
EndCreateBullet:
CreateBullet: GlinSpike
  BulletAngle: 12000;
  BulletAbsolutePosition: 120000, -120000;
EndCreateBullet:
CreateBullet: GlinSpike
  BulletAngle: 62000;
  BulletAbsolutePosition: 160000, -120000;
EndCreateBullet:
CreateBullet: GlinSpike
  BulletAbsolutePosition: 200000, -120000;
EndCreateBullet:
return;

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
# Teleport
Random: ran1, 0, 100;
BeginIf: (Random: ran1 >= 40), (TransitionCached: Evade, false)
  BBSprite: Cast_Start_1, 5;
  BBSprite: Cast_Start_2, 5;
  BBSprite: Cast_Start_3, 5;
  BBSprite: Cast_Start_4, 10;
  BBSprite: Teleport_1, 4;
  BBSprite: Teleport_2, 4;
  BBSprite: Teleport_3, 4;
  BBSprite: Teleport_4, 4;
  SetPos: 100000, 100000;
  WaitFrame: 15;
  GlinPos: -140000, 140000, 90000, -95000;
  BBSprite: Teleport_3, 4;
  BBSprite: Teleport_2, 4;
  BBSprite: Teleport_1, 4;
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
@Main:
# 1. 悬停在空中
BBSprite: AirDash_Anticipate_1, 5; # 播放对应的动画帧，包括Sprite、Hitbox，第二个参数表示动画帧的持续帧数
BBSprite: AirDash_Anticipate_2, 5;
BBSprite: AirDash_Anticipate_3, 5;
BBSprite: AirDash_Anticipate_4, 5;
BBSprite: AirDash_Anticipate_5, 5;
BBSprite: AirDash_Anticipate_6, 5;
BBSprite: AirDash_Active_1, 4;
EnableGlinChase: true, -450000, 450000, 80000;  #添加GlinChase组件，组件作用为每帧检测玩家位置，调整EulerAngle(此处的参数为万分制)
Shake: 850, 850, 12000, 60, 1; # 蓄力过程中 Boss自身振动
# 2. 蓄力阶段，在空中悬停60帧
RegistCounter: 60;
BeginLoopAnim: (Counter: Value > 0) # 循环播放下面三个动画帧
  LoopSprite: AirDash_Active_2, 5;
  LoopSprite: AirDash_Active_3, 5;
  LoopSprite: AirDash_Active_4, 5;
EndLoopAnim:
EnableGlinChase: false, 0, 0, 0; # 移除GlinChase组件
# 3. 向下冲刺
AirDashVelocity: -700000;
VFX: ADust  # 冲刺起始，生成AirDust特效
  VFX_LocalPosition: 0, 27000;
  VFX_Scale: 8000, 12000;
  VFX_LocalAngle: 900000;
EndVFX:
BeginLoopAnim: (InAir: true) # 每帧检测地面，OnGround退出循环
  LoopSprite: AirDash_Active_2, 4;
  LoopSprite: AirDash_Active_3, 4;
  LoopSprite: AirDash_Active_4, 4;
EndLoopAnim:
# 4. 落地
SetAngle: 0; # 落地时，重置EulerAngle为0
EnemyUpdateFlip; # 落地时，根据玩家当前位置，调整水平冲刺的朝向
ScreenShake: 500, 1550, 10000, 25, 0; # 砸地，屏幕振动
SetVelocity: 0, -50000;
VFX: GDust # 落地灰尘特效，一左一右
  VFX_LocalPosition: -65000, -25000;
  VFX_Scale: -8000, 4000;
  VFX_AbsoluteAngle: 0;
EndVFX:
VFX: GDust
  VFX_LocalPosition: 65000, -25000;
  VFX_Scale: 8000, 4000;
  VFX_AbsoluteAngle: 0;
EndVFX:
BBSprite: Land_1, 10;
BBSprite: Land_2, 4;
BBSprite: Land_3, 8;
BBSprite: Land_4, 4;
# 5. 水平冲刺
BBSprite: Dash_Active_1, 4;
VFX: GDust
  VFX_LocalPosition: 80000, -15000;
  VFX_Scale: 10000, 6000;
  VFX_AbsoluteAngle: 0;
EndVFX:
SetVelocity: 600000, 0; # 水平冲刺速度
BBSprite: Dash_Active_2, 5;
AccelX: 700000, -3000000, 15; # 逐渐减速
BBSprite: Dash_Active_3, 5;
BBSprite: Dash_Active_4, 5;
BBSprite: End_1, 5;
SetVelocityX: 0;
BBSprite: End_2, 4;
BBSprite: End_3, 4;
BBSprite: End_4, 4;
# 6. 隐身，然后切换到下一个动作
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
SetAngle: 0;
Shake: 1500, 1500, 10000, 20, 0;
BBSprite: Explode_1, 20;
BBSprite: Explode_2, 4;
ScreenShake: 1250, 1250, 12000, 30, 0; 
BBSprite: Explode_3, 6;
BBSprite: Explode_4, 4;
BBSprite: Explode_5, 4;
SetPos: 1000000, 1000000;
WaitFrame: 100;
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
WaitFrame: 200;
# 二阶段
HPLock: 0;
SetTransition: NoTeleportOut, true;
GotoBehavior: Glin_Step2_Teleport;

[Glin_Death]
@Trigger:
return;

@Main:
Shake: 500, 500, 10000, 70, 1;
RegistCounter: 100;
BeginLoopAnim: (Counter: Value > 0)
  LoopSprite: Stun_1, 5;
  LoopSprite: Stun_2, 5;
  LoopSprite: Stun_3, 5;
EndLoopAnim:
ScreenShake: 1700, 1700, 10000, 30, 0;
BBSprite: Explode_1, 5;
BBSprite: Explode_2, 5;
BBSprite: Explode_3, 5;
SetPos: 1000000, 100000;
WaitFrame: 50;
SetPos: 0, 0;
Exit;

[Glin_Exit]
@Trigger:
return;

@Main:
SetPos: 40000, -120000;
SetVelocity: 0, 0;
SetAngle: 0;
BBSprite: In_1, 5;
ScreenShake: 750, 750, 10000, 15, 0;
BBSprite: In_2, 5;
BBSprite: In_3, 5;
BBSprite: In_4, 5;
CallSubCoroutine: Glin_Exit, OpenDorCoroutine;
RegistCounter: 270;
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
BBSprite: Out_1, 5;
ScreenShake: 750, 750, 10000, 15, 0;
BBSprite: Out_2, 5;
BBSprite: Out_3, 5;
BBSprite: Out_4, 5;
BBSprite: Out_5, 5;
SetPos: 1000000, 1000000;
WaitFrame: 50;
Exit;

@OpenDorCoroutine:
WaitFrame: 50;
# 开门
CreateEffect: Glin_Hand
  CreateEffect_Position: 12900, 6500;
EndCreateEffect:
return;

[Glin_Bow2]
@Trigger:
return;

@Main:
# Teleport
SetVelocity: 0, 0;
SetAngle: 0;
BBSprite: Teleport_1, 4;
BBSprite: Teleport_2, 4;
BBSprite: Teleport_3, 4;
BBSprite: Teleport_4, 4;
BBSprite: Teleport_5, 4;
SetPos: 1000000, 1000000;
WaitFrame: 20;
GlinPos: -140000, 140000, 60000, -95000;
BBSprite: Teleport_4, 4;
BBSprite: Teleport_3, 4;
BBSprite: Teleport_2, 4;
# Idle
RegistCounter: 60;
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
# Bow
BBSprite: Bow_1, 8;
BBSprite: Bow_3, 5;
BBSprite: Bow_4, 5;
BBSprite: Bow_5, 5;
BBSprite: Bow_6, 5;
BBSprite: Bow_7, 5;
return;