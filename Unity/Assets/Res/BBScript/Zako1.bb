[Root]
@RootInit:
GlinInit;
PoolObject: ADust, 1;
PoolObject: GlinFireball, 5;
PoolObject: GlinSpike_Step2, 5;
RegistMove: (Zako1_Idle)
  MoveType: None;
EndMove:
RegistMove: (Zako1_TeleportIn)
  MoveType: None;
EndMove:
RegistMove: (Zako1_Dash)
  MoveType: None;
EndMove:
RegistMove: (Zako1_Throw)
  MoveType: None;
EndMove:
RegistMove: (Zako1_CastSpike)
  MoveType: None;
EndMove:
RegistMove: (Zako1_Death)
  MoveType: HitStun;
EndMove:
GotoBehavior: Zako1_CastSpike;
return;

[Zako1_Idle]
@Trigger:
return;

@Main:
# 更新判定框
EnableWaitFrameCallback: true, 50, Zako1_Idle, WaitFrameCallback;
BBSprite: Idle_0, 1;
SetMarker: Loop;
# Idle
EnableEnemyFlipCheck: true;
BeginLoop: (EnemyFlipChange: false)
  BBSprite: Idle_1, 6;
  BBSprite: Idle_2, 6;
  BBSprite: Idle_3, 6;
  BBSprite: Idle_4, 6;
  BBSprite: Idle_5, 6;
  BBSprite: Idle_6, 6;
  BBSprite: Idle_7, 6;
  BBSprite: Idle_8, 6;
  BBSprite: Idle_9, 6;
  BBSprite: Idle_10, 6;
EndLoop:
EnableEnemyFlipCheck: false;
FlipReverse;
# Turn
BBSprite: Turn_1, 5;
BBSprite: Turn_2, 5;
BBSprite: Turn_3, 5;
GotoMarker: Loop;
Exit;

@WaitFrameCallback:
GotoBehavior: Zako1_Throw;
return;

[Zako1_TeleportIn]
@Trigger:
return;

@Main:
PlayTimeline: 0, 40;
GotoBehavior: Zako1_Idle;

[Zako1_Dash]
@Trigger:
return;

@Main:
SetPos: 0, -10000;
SetRotate: 0;
BBSprite: Anticipate_1, 5;
BBSprite: Anticipate_2, 5;
BBSprite: Anticipate_3, 5;
BBSprite: Anticipate_4, 5;
BBSprite: Anticipate_5, 10;
SetRotate: 450000;
SetVelocity: 350000, -350000;
ScreenShake: 1050, 1050, 10000, 15;
SpawnADust: 28000, 0, 8000, 5000, 0;
BBSprite: Active_1, 4;
# 注册回调
EnableGroundCollisionCallback: true, Zako1_Dash, GroundCollisionCallback;
RegistCounter: 200;
BeginLoop: (Counter: Value > 0)
  BBSprite: Active_2, 3;
  BBSprite: Active_3, 3;
  BBSprite: Active_4, 3;
  BBSprite: Active_5, 3;
EndLoop:
EnableGroundCollisionCallback: false, 0, 0;
SetRotate: 0;
SetVelocity: 0, 0;
BBSprite: End_1, 5;
BBSprite: End_2, 5;
BBSprite: End_3, 5;
BBSprite: End_4, 5;
BBSprite: End_5, 5;
GotoBehavior: Zako1_Idle;

@GroundCollisionCallback:
ScreenShake: 450, 450, 10000, 10;
SpawnADust: 8000, 3000, 5000, 5000, 0;
GroundCollisionVelocity;
return;

[Zako1_Throw]
@Trigger:
return;

@Main:
BBSprite: Anticipate_1, 5;
BBSprite: Anticipate_2, 5;
BBSprite: Anticipate_3, 6;
BBSprite: Anticipate_4, 6;
BBSprite: Anticipate_5, 10;
# 1
CreateBullet: GlinFireball
  BulletVelocity: 180000, -150000;
  BulletPosition: -12000, -12000;
EndCreateBullet:
# 2
CreateBullet: GlinFireball
  BulletVelocity: 120000, -200000;
  BulletPosition: -12000, -12000;
EndCreateBullet:
# 3
CreateBullet: GlinFireball
  BulletVelocity: 240000, -100000;
  BulletPosition: -12000, -12000;
EndCreateBullet:
BBSprite: Active_1, 10;
BBSprite: Active_2, 5;
BBSprite: End_1, 5;
GotoBehavior: Zako1_Idle;

[Zako1_Death]
@Trigger:
return;

@Main:
RegistCounter: 100;
BeginLoop: (Counter: Value > 0)
  BBSprite: Death_1, 5;
  BBSprite: Death_2, 5;
  BBSprite: Death_3, 5;
EndLoop:
BBSprite: End_1, 5;
BBSprite: End_2, 5;
BBSprite: End_3, 5;
BBSprite: End_4, 5;
BBSprite: End_5, 5;
BBSprite: End_6, 5;
BBSprite: End_7, 5;
Exit;

[Zako1_CastSpike]
@Trigger:
return;

@Main:
RegistCounter: 200;
CallSubCoroutine: Zako1_CastSpike, CastSpikeCor;
BeginLoop: (Counter: Value > 0)
  BBSprite: Death_1, 5;
  BBSprite: Death_2, 5;
  BBSprite: Death_3, 5;
EndLoop:
Exit;

@CastSpikeCor:
WaitFrame: 50;
CreateBullet: GlinSpike_Step2
  BulletPosition: 50000, -115000;
EndCreateBullet:
WaitFrame: 50;
CreateBullet: GlinSpike_Step2
  BulletPosition: 0, -115000;
EndCreateBullet:
WaitFrame: 50;
CreateBullet: GlinSpike_Step2
  BulletPosition: -50000, -115000;
EndCreateBullet:
WaitFrame: 50;
CreateBullet: GlinSpike_Step2
  BulletPosition: -100000, -115000;
EndCreateBullet:
return;