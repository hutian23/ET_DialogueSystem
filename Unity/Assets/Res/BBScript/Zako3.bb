[Root]
@RootInit:
PoolObject: GDust, 1;
PoolObject: Javelin, 5;
EnemyInit;
HP: 300;
RegistMove: (Zako3_Spawn)
  MoveType: None;
EndMove:
RegistMove: (Zako3_Patrol)
  MoveType: None;
EndMove:
RegistMove: (Zako3_BattleIdle)
  MoveType: None;
EndMove:
RegistMove: (Zako3_Charge)
  MoveType: None;
EndMove:
RegistMove: (Zako3_ThrowAttack)
  MoveType: None;
EndMove:
RegistMove: (Zako3_Death)
  MoveType: Death;
EndMove:
SetPos: 0, 65000;
GotoBehavior: Zako3_Spawn;
return;

@HPWatcher:
BeginIf: (HP: Value <= 0)
  GotoBehavior: Zako3_Death;
EndIf:
return;

[Zako3_Spawn]
@Trigger:
return;

@Main:
AccelX: 0, 30, 100000;
AccelY: -350000, 40, 500000;
RegistCounter: 50;
BeginLoopAnim: (Counter: Value > 0)
  LoopSprite: Idle_1, 5;
  LoopSprite: Idle_2, 5;
  LoopSprite: Idle_3, 5;
  LoopSprite: Idle_4, 5;
  LoopSprite: Idle_5, 5;
  LoopSprite: Idle_6, 5;
  LoopSprite: Idle_7, 5;
EndLoopAnim:
GotoBehavior: Zako3_BattleIdle;

[Zako3_Patrol]
@Trigger:
return;

@Main:
SetMarker: Loop;
# Patrol
AirPatrolVelocity: 10000;
Random: ran1, 0, 100;
BeginIf: (Random: ran1 >= 0), (Random: ran1 < 50)
  RegistCounter: 35;
EndIf:
BeginIf: (Random: ran1 >= 50), (Random: ran1 <= 75)
  RegistCounter: 70;
EndIf:
BeginIf: (Random: ran1 >= 75), (Random: ran1 <= 100)
  RegistCounter: 105;
EndIf:
BeginLoopAnim: (Counter: Value > 0)
  LoopSprite: Idle_1, 5;
  LoopSprite: Idle_2, 5;
  LoopSprite: Idle_3, 5;
  LoopSprite: Idle_4, 5;
  LoopSprite: Idle_5, 5;
  LoopSprite: Idle_6, 5;
  LoopSprite: Idle_7, 5;
EndLoopAnim:
# Turn
SetVelocity: 0, 0;
Random: ran2, 0, 100;
BeginIf: (Random: ran2 >= 0), (Random: ran2 <= 40)
  BBSprite: Turn_1, 5;
  BBSprite: Turn_2, 5;
  FlipReverse;
EndIf:
GotoMarker: Loop;
return;

[Zako3_BattleIdle]
@Trigger:
return;

@Main:
# 冲刺衔接到待机动作后，向上攀升一段距离
BeginIf: (TransitionCached: ChargeToBattleIdle, true), (EnemyFlipChange: true)
  # 转向
  SetVelocity: 70000, 30000;
  BBSprite: Turn_1, 5;
  BBSprite: Turn_2, 5;
  FlipReverse;
  SetVelocity: -70000, 50000;
  BBSprite: Idle_1, 5;
  BBSprite: Idle_2, 5;
  SetVelocity: -30000, 80000;
  BBSprite: Idle_3, 5;
  BBSprite: Idle_4, 5;
  SetVelocity: -10000, 40000;
  BBSprite: Idle_5, 5;
  SetVelocity: -5000, 20000;
  BBSprite: Idle_6, 5;
  BBSprite: Idle_7, 5;
  RemoveTransitionCached: ChargeToBattleIdle;
EndIf:
BeginIf: (TransitionCached: ChargeToBattleIdle, true)
  SetVelocity: 70000, 50000;
  BBSprite: Idle_1, 5;
  BBSprite: Idle_2, 5;
  SetVelocity: 50000, 80000;
  BBSprite: Idle_3, 5;
  BBSprite: Idle_4, 5;
  SetVelocity: 20000, 50000;
  BBSprite: Idle_5, 5;
  SetVelocity: 5000, 20000;
  BBSprite: Idle_6, 5;
  BBSprite: Idle_7, 5;
  RemoveTransitionCached: ChargeToBattleIdle;
EndIf:
# 等待一段时间释放技能
EnableWaitFrameCallback: true, 80, Zako3_BattleIdle, WaitFrameCallback;
SetMarker: Loop;
SetVelocity: 30000, 0;
# Idle
BeginLoopAnim: (EnemyFlipChange: false)
  LoopSprite: Idle_1, 5;
  LoopSprite: Idle_2, 5;
  LoopSprite: Idle_3, 5;
  LoopSprite: Idle_4, 5;
  LoopSprite: Idle_5, 5;
  LoopSprite: Idle_6, 5;
  LoopSprite: Idle_7, 5;
EndLoopAnim:
# Turn
BBSprite: Turn_1, 5;
BBSprite: Turn_2, 5;
FlipReverse;
GotoMarker: Loop;
return;

@WaitFrameCallback:
Random: ran, 0, 100;
BeginIf: (Random: ran >= 0), (Random: ran < 50)
  GotoBehavior: Zako3_ThrowAttack;
EndIf:
BeginIf: (Random: ran >= 50), (Random: ran <= 100)
  GotoBehavior: Zako3_Charge;
EndIf:
return;

[Zako3_Charge]
@Trigger:
return;

@Main:
EnemyUpdateFlip;
# Anticipate
SetVelocity: -80000, 30000;
BBSprite: Anticipate_1, 4;
SetVelocity: -40000, 10000;
BBSprite: Anticipate_2, 4;
SetVelocity: -20000, 5000;
BBSprite: Anticipate_3, 4;
BBSprite: Anticipate_3, 4;
# Active
BBSprite: Active_1, 4;
SetVelocityX: 300000;
AccelY: -150000, 20, 350000;
BBSprite: Active_2, 5;
RegistCounter: 35;
BeginLoopAnim: (Counter: Value > 0)
  LoopSprite: Active_3, 5;
  LoopSprite: Active_4, 5;
EndLoopAnim:
# End 
SetVelocity: 150000, 0;
BBSprite: End_1, 5;
SetVelocity: 70000, 0;
BBSprite: End_2, 8;
BBSprite: End_3, 5;
SetTransition: ChargeToBattleIdle, true;
GotoBehavior: Zako3_BattleIdle;

[Zako3_ThrowAttack]
@Trigger:
return;

@Main:
EnemyUpdateFlip;
SetVelocity: 0, 0;
BBSprite: Anticipate_1, 4;
BBSprite: Anticipate_2, 4;
BBSprite: Anticipate_3, 4;
BBSprite: Anticipate_4, 4;
BBSprite: Anticipate_5, 4;
BBSprite: Anticipate_6, 8;
# 生成飞矛
CreateBullet: Javelin
  BulletFaceToPlayer: 100000, 450000, 500000;
  BulletLocalPosition: -25000, -4500;
EndCreateBullet:
BBSprite: Throw, 8;
BBSprite: End_1, 4;
BBSprite: End_2, 4;
BBSprite: End_3, 4;
BBSprite: End_4, 4;
BBSprite: End_5, 4;
BBSprite: End_6, 4;
GotoBehavior: Zako3_BattleIdle;

[Zako3_Death]
@Trigger:
return;

@Main:
# Hit
SetVelocity: 0, 0;
Shake: 1400, 1400, 10000, 25, 0;
EnableAirCheck;
EnableGravityCheck: 100000, 150000, 500000;
Gravity: 0;
BBSprite: Air_1, 25;
# Fall
SetVelocity: -70000, 120000;
BBSprite: Air_2, 5;
Gravity: 100000;
BeginLoopAnim: (InAir: true)
  LoopSprite: Air_2, 5;
EndLoopAnim:
# Land
SetVelocity: 0, 0;
Shake: 800, 800, 12000, 10, 0;
SetVelocity: -15000, 120000;
BBSprite: Land_1, 10;
BBSprite: Land_2, 5;
SetVelocity: 0, 0;
BBSprite: Land_3, 5;
BBSprite: Land_4, 5;
return;