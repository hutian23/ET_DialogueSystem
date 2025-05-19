[Root]
@RootInit:
PoolObject: GDust, 1;
EnemyInit;
EnableAirCheck;
EnableGravityCheck: 100000, 150000, 450000;    
RegistMove: (Zako2_Spawn)
  MoveType: None;
EndMove:
RegistMove: (Zako2_Patrol)
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
GotoBehavior: Zako2_Charge;
return;

[Zako2_Spawn]
@Trigger:
return;

@Main:
SetPos: -100000, -110000;
PlayTimeline: 0, 35;
GotoBehavior: Zako2_Patrol;

[Zako2_Patrol]
@Trigger:
return;

@Main: 
EnablePatrol: -193000, 193000;
EnableTargetCheck: 0, 10000, 120000, 60000; 
RegistFindTargetCallback: Zako2_Patrol, FindTargetCallback;
SetMarker: Loop;
SetVelocityX: 40000;
# Walk
BeginLoop: (PatrolReached: false)
  BBSprite: Walk_1, 5;
  BBSprite: Walk_2, 5;
  BBSprite: Walk_3, 5;
  BBSprite: Walk_4, 5;
  BBSprite: Walk_5, 5;
  BBSprite: Walk_6, 5;
  BBSprite: Walk_7, 5;
  BBSprite: Walk_8, 5;
EndLoop:
# Turn
SetVelocityX: 0;
BBSprite: Turn_1, 5;
BBSprite: Turn_2, 5;
FlipReverse;
BBSprite: Walk_1, 1;
GotoMarker: Loop;
Exit;

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
GotoBehavior: Zako2_Chase;

[Zako2_Chase]
@Trigger:
return;

@Main:
EnableEnemyFlipCheck: true;
EnableRangeCheck: true, 35000, 0, 0;
SetMarker: Loop;
SetVelocityX: 80000;
BeginLoop: (InRange: false), (EnemyFlipChange: false)
  BBSprite: Run_1, 5;
  BBSprite: Run_2, 5;
  BBSprite: Run_3, 5;
  BBSprite: Run_4, 5;
  BBSprite: Run_5, 5;
  BBSprite: Run_6, 5;
  BBSprite: Run_7, 5;
EndLoop:
BeginIf: (InRange: true)
  GotoBehavior: Zako2_Attack;
EndIf:
BBSprite: Turn_1, 5;
BBSprite: Turn_2, 5;
FlipReverse;
BBSprite: Run_1, 1;
GotoMarker: Loop;
Exit;

[Zako2_Charge]
@Trigger:
return;

@Main:
SetVelocityX: 0;
BBSprite: Anticipate_1, 5;
BBSprite: Anticipate_2, 5;
BBSprite: Anticipate_3, 5;
BBSprite: Anticipate_4, 5;
BBSprite: Anticipate_5, 5;
BBSprite: Anticipate_6, 5;
BBSprite: Anticipate_7, 5;
BBSprite: Anticipate_8, 5;
SpawnGDust: 35000, -15000, 5000, 2000;
SetVelocityX: 250000;
RegistCounter: 30;
BeginLoop: (Counter: Value > 0)
  BBSprite: Active_1, 5;
  BBSprite: Active_2, 5;
  BBSprite: Active_3, 5;
  BBSprite: Active_4, 5;
  BBSprite: Active_5, 5;
  BBSprite: Active_6, 5;
EndLoop:
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
ScreenShake: 1000, 1000, 12000, 15;
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
SetPos: 0, -120000;
BBSprite: Anticipate_1, 4;
BBSprite: Anticipate_2, 4;
BBSprite: Anticipate_3, 8;
# Jump
SetVelocity: 40000, 200000;
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
BeginLoop: (InAir: true)
  BBSprite: Jump_8, 5;
EndLoop:
# Land
Gravity: 100000;
SetVelocity: 0, 0;
ScreenShake: 1600, 1600, 10000, 15;
BBSprite: Land_1, 4;
BBSprite: Land_2, 10;
GotoBehavior: Zako2_JumpAttack;

[Zako2_BattleIdle]
@Trigger:
return;

@Main:
#1. 待机
SetVelocityX: 0;
EnableEnemyFlipCheck: true;
EnableRangeCheck: true, 35000, 0, 0;
RegistCounter: 15;
BeginLoop: (Counter: Value > 0)
  BBSprite: Idle_1, 5;
  BBSprite: Idle_2, 5;
  BBSprite: Idle_3, 5;
  BBSprite: Idle_4, 5;
  BBSprite: Idle_5, 5;
  BBSprite: Idle_6, 5;
EndLoop:
#2. 切换进下一个动作前，先转向
BeginIf: (EnemyFlipChange: true)
  BBSprite: Turn_1, 5;
  BBSprite: Turn_2, 5;
  FlipReverse;
EndIf:
#3. 切换动作
BeginIf: (InRange: true)
  GotoBehavior: Zako2_Attack;
EndIf:
GotoBehavior: Zako2_Chase;
