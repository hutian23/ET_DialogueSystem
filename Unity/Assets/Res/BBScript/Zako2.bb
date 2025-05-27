[Root]
@RootInit:
PoolObject: GDust, 1;
EnemyInit;
HP: 200;
EnableAirCheck;
EnableGravityCheck: 100000, 150000, 450000;    
RegistMove: (Zako2_Spawn)
  MoveType: None;
EndMove:
RegistMove: (Zako2_Patrol)
  MoveType: None;
EndMove:
RegistMove: (Zako2_LoseTarget)
  MoveType: None;
EndMove:
RegistMove: (Zako2_Startle)
  MoveType: None;
EndMove:
RegistMove: (Zako2_Chase)
  MoveType: None;
EndMove:
RegistMove: (Zako2_Charge)
  MoveType: None;
EndMove:
RegistMove: (Zako2_Attack)
  MoveType: None;
EndMove:
RegistMove: (Zako2_JumpAttack)
  MoveType: None;
EndMove:
RegistMove: (Zako2_BattleIdle)
  MoveType: None;
EndMove:
RegistMove: (Zako2_Death)
  MoveType: None;
EndMove:
GotoBehavior: Zako2_Spawn;
return;

@HPWatcher:
BeginIf: (HP: Value <= 0)
  GotoBehavior: Zako2_Death;
EndIf:
return;

[Zako2_Spawn]
@Trigger:
return;

@Main:
PlayTimeline: 0, 35;
GotoBehavior: Zako2_Patrol;

[Zako2_Patrol]
@Trigger:
return;

@Main: 
EnablePatrol: -193000, 193000;
EnableTargetCheck: 0, 10000, 120000, 100000; 
RegistFindTargetCallback: Zako2_Patrol, FindTargetCallback;
SetMarker: Loop;
SetVelocityX: 40000;
# Walk
BeginLoopAnim: (PatrolReached: false)
  LoopSprite: Walk_1, 5;
  LoopSprite: Walk_2, 5;
  LoopSprite: Walk_3, 5;
  LoopSprite: Walk_4, 5;
  LoopSprite: Walk_5, 5;
  LoopSprite: Walk_6, 5;
  LoopSprite: Walk_7, 5;
  LoopSprite: Walk_8, 5;
EndLoopAnim:
# Turn
SetVelocityX: 0;
BBSprite: Turn_1, 5;
BBSprite: Turn_2, 5;
FlipReverse;
GotoMarker: Loop;
return;

@FindTargetCallback:
GotoBehavior: Zako2_Startle;
return;

[Zako2_LoseTarget]
@Trigger:
return;

@Main:
SetVelocityX: 0;
# 待机期间玩家回到攻击范围内
EnableTargetCheck: 0, 10000, 120000, 100000; 
RegistFindTargetCallback: Zako2_LoseTarget, FindTargetCallback;
# 待机一段时间，然后切换到巡逻行为
RegistCounter: 150;
BeginLoopAnim: (Counter: Value > 0)
  LoopSprite: Idle_1, 5;
  LoopSprite: Idle_2, 5;
  LoopSprite: Idle_3, 5;
  LoopSprite: Idle_4, 5;
  LoopSprite: Idle_5, 5;
  LoopSprite: Idle_6, 5;
EndLoopAnim:
GotoBehavior: Zako2_Patrol;

@FindTargetCallback:
GotoBehavior: Zako2_Startle;
return;

[Zako2_Startle]
@Trigger:
return;

@Main:
EnemyUpdateFlip;
SetVelocity: 0, 0;
BBSprite: Startle_1, 5;
BBSprite: Startle_2, 5;
BBSprite: Startle_3, 10;
BBSprite: Startle_4, 5;
Random: ran1, 0, 100;
BeginIf: (Random: ran1 >= 0), (Random: ran1 < 50)
  GotoBehavior: Zako2_Chase;
EndIf:
BeginIf: (Random: ran1 >= 50), (Random: ran1 <= 100)
  GotoBehavior: Zako2_Charge;
EndIf:

[Zako2_Chase]
@Trigger:
return;

@Main:
# 丢失目标，回到Patrol行为
EnableTargetCheck: 0, 0, 400000, 100000;
RegistLoseTargetCallback: Zako2_Chase, LoseTargetCallback;
# 攻击范围内，释放攻击技能
EnableInRangeCheck: true, 35000, 0, 0;
RegistInRangeCallback: Zako2_Chase, InRangeCallback;
SetVelocityX: 80000;
SetMarker: Loop;
# 朝向改变，中止Run循环
BeginLoopAnim: (EnemyFlipChange: false)
  LoopSprite: Run_1, 5;
  LoopSprite: Run_2, 5;
  LoopSprite: Run_3, 5;
  LoopSprite: Run_4, 5;
  LoopSprite: Run_5, 5;
  LoopSprite: Run_6, 5;
  LoopSprite: Run_7, 5;
EndLoopAnim:
BBSprite: Turn_1, 5;
BBSprite: Turn_2, 5;
FlipReverse;
GotoMarker: Loop;
return;

@LoseTargetCallback:
GotoBehavior: Zako2_LoseTarget;
return;

@InRangeCallback:
Random: ran1, 0, 100;
BeginIf: (Random: ran1 >= 0), (Random: ran1 < 50)
  GotoBehavior: Zako2_Attack;
EndIf:
BeginIf: (Random: ran1 >= 50), (Random: ran1 <= 100)
  GotoBehavior: Zako2_JumpAttack;
EndIf:
return;

[Zako2_Charge]
@Trigger:
return;

@Main:
# 蓄力
SetVelocityX: 0;
BBSprite: Anticipate_1, 5;
BBSprite: Anticipate_2, 5;
BBSprite: Anticipate_3, 5;
BBSprite: Anticipate_4, 5;
BBSprite: Anticipate_5, 5;
BBSprite: Anticipate_6, 5;
BBSprite: Anticipate_7, 5;
BBSprite: Anticipate_8, 5;
# 冲刺
SpawnGDust: 35000, -15000, 5000, 2000;
SetVelocityX: 250000;
RegistCounter: 30;
BeginLoopAnim: (Counter: Value > 0)
  LoopSprite: Active_1, 5;
  LoopSprite: Active_2, 5;
  LoopSprite: Active_3, 5;
  LoopSprite: Active_4, 5;
  LoopSprite: Active_5, 5;
  LoopSprite: Active_6, 5;
EndLoopAnim:
# 刹车
SetVelocityX: 100000;
BBSprite: End_1, 5;
BBSprite: End_2, 5;
SetVelocityX: 50000;
BBSprite: End_3, 5;
BBSprite: End_4, 5;
SetVelocityX: 0;
BBSprite: End_5, 5;
GotoBehavior: Zako2_BattleIdle;

[Zako2_Attack]
@Trigger:
return;

@Main:
SetVelocityX: 0;
BBSprite: Anticipate_1, 4;
BBSprite: Anticipate_2, 4;
BBSprite: Anticipate_3, 4;
BBSprite: Anticipate_4, 4;
BBSprite: Anticipate_5, 4;
BBSprite: Anticipate_6, 8;
BBSprite: Anticipate_7, 4;
BBSprite: Active_1, 4;
ScreenShake: 1000, 1000, 12000, 15, 0;
BBSprite: Active_2, 4;
BBSprite: End_1, 4;
BBSprite: End_2, 4;
BBSprite: End_3, 4;
GotoBehavior: Zako2_BattleIdle;

[Zako2_JumpAttack]
@Trigger:
return;

@Main:
# PreJump
SetVelocityX: 0;
BBSprite: Anticipate_1, 4;
BBSprite: Anticipate_2, 4;
BBSprite: Anticipate_3, 8;
# Jump
Random: ran1, 0, 100;
BeginIf: (Random: ran1 >= 0), (Random: ran1 < 40)
  SetVelocityX: 80000;
EndIf:
BeginIf: (Random: ran1 >= 40), (Random: ran1 < 70)
  SetVelocityX: 0;
EndIf:
BeginIf: (Random: ran1 >= 70), (Random: ran1 <= 100)
  SetVelocityX: -30000;
EndIf:
SetVelocityY: 200000;
Gravity: 0;
BBSprite: Jump_1, 4;
Gravity: 120000;
BBSprite: Jump_2, 4;
# Fall
BBSprite: Jump_3, 3;
BBSprite: Jump_4, 3;
BBSprite: Jump_5, 3;
BBSprite: Jump_6, 3;
BBSprite: Jump_7, 3;
BBSprite: Jump_8, 3;
BeginLoopAnim: (InAir: true)
  LoopSprite: Jump_8, 5;
EndLoopAnim:
# Land
SpawnGDust: 5000, -15000, 4000, 2000;
SpawnGDust: -50000, -15000, -4000, 2000;
Gravity: 100000;
SetVelocity: 0, 0;
ScreenShake: 1600, 1600, 10000, 25, 0;
BBSprite: Land_1, 4;
BBSprite: Land_2, 15;
GotoBehavior: Zako2_BattleIdle;

[Zako2_BattleIdle]
@Trigger:
return;

@Main:
#1. 待机
SetVelocityX: 0;
EnableTargetCheck: 0, 0, 400000, 100000;
RegistLoseTargetCallback: Zako2_BattleIdle, LoseTargetCallback;
EnableInRangeCheck: true, 35000, 0, 0;
RegistCounter: 15;
BeginLoopAnim: (Counter: Value > 0)
  LoopSprite: Idle_1, 5;
  LoopSprite: Idle_2, 5;
  LoopSprite: Idle_3, 5;
  LoopSprite: Idle_4, 5;
  LoopSprite: Idle_5, 5;
  LoopSprite: Idle_6, 5;
EndLoopAnim:
#2. 切换进下一个动作前，先转向
BeginIf: (EnemyFlipChange: true)
  BBSprite: Turn_1, 5;
  BBSprite: Turn_2, 5;
  FlipReverse;
EndIf:
#3. 切换动作
Random: ran1, 0, 100;
BeginIf: (Random: ran1 >= 0), (Random: ran1 < 30)
  GotoBehavior: Zako2_Charge;
EndIf:
# 不在攻击范围内，先进行位移
BeginIf: (InRange: false)
  GotoBehavior: Zako2_Chase;
EndIf:
BeginIf: (Random: ran1 >= 30), (Random: ran1 < 60)
  GotoBehavior: Zako2_Attack;
EndIf:
BeginIf: (Random: ran1 >= 60), (Random: ran1 <= 100)
  GotoBehavior: Zako2_JumpAttack;
EndIf:

@LoseTargetCallback:
GotoBehavior: Zako2_LoseTarget;
return;

[Zako2_Death]
@Trigger:
return;

@Main:
# Hit
Gravity: 0;
WaitFrame: 1;
SetVelocity: 0, 0;
Shake: 1400, 1400, 10000, 25, 0;
BBSprite: Air_1, 25;
SetVelocity: -70000, 180000;
BBSprite: Air_2, 5;
# Fall
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
return;