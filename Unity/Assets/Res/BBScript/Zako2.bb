[Root]
@RootInit:
EnemyInit;
PoolObject: GDust, 1;
RegistMove: (Zako2_Spawn)
  MoveType: None;
EndMove:
RegistMove: (Zako2_Idle)  
  MoveType: None;
EndMove:
RegistMove: (Zako2_Walk)
  MoveType: None;
EndMove:
RegistMove: (Zako2_Startle)
  MoveType: None;
EndMove:
RegistMove: (Zako2_Run)
  MoveType: None;
EndMove:
RegistMove: (Zako2_Charge)
  MoveType: None;
EndMove:
GotoBehavior: Zako2_Spawn;
return;

[Zako2_Spawn]
@Trigger:
return;

@Main:
SetPos: -50000, -110000;
PlayTimeline: 0, 35;
GotoBehavior: Zako2_Walk;

[Zako2_Idle]
@Trigger:
return;

@Main:
EnableTargetCheck: -20000, 10000, 80000, 60000; 
RegistFindTargetCallback: Zako2_Idle, FindTargetCallback;
RegistCounter: 150;
BeginLoop: (Counter: Value > 0)
BBSprite: Idle_1, 5;
BBSprite: Idle_2, 5;
BBSprite: Idle_3, 5;
BBSprite: Idle_4, 5;
BBSprite: Idle_5, 5;
BBSprite: Idle_6, 5;
EndLoop:
GotoBehavior: Zako2_Walk;

@FindTargetCallback:
GotoBehavior: Zako2_Startle;
return;

[Zako2_Walk]
@Trigger:
return;

@Main: 
EnablePatrol: -193000, 193000;
EnableTargetCheck: -20000, 10000, 80000, 60000; 
RegistFindTargetCallback: Zako2_Walk, FindTargetCallback;
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
# Random: ran1, 0, 100;
# BeginIf: (Random: ran1 > 0), (Random: ran1 < 50)
#   GotoBehavior: Zako2_Charge;
# EndIf:
# BeginIf: (Random: ran1 >= 50), (Random: ran1 <= 100)
#   GotoBehavior: Zako2_Run;
# EndIf:
GotoBehavior: Zako2_Run;

[Zako2_Run]
@Trigger:
return;

@Main:
EnableEnemyFlipCheck: true;
# 丢失目标，回到站立状态，然后重新Patrol
EnableTargetCheck: 0, 10000, 200000, 60000; 
RegistLoseTargetCallback: Zako2_Run, LoseTargetCallback;
SetVelocityX: 80000;
SetMarker: Loop;
BeginLoop: (EnemyFlipChange: false)
  BBSprite: Run_1, 5;
  BBSprite: Run_2, 5;
  BBSprite: Run_3, 5;
  BBSprite: Run_4, 5;
  BBSprite: Run_5, 5;
  BBSprite: Run_6, 5;
  BBSprite: Run_7, 5;
EndLoop:
BBSprite: Turn_1, 5;
BBSprite: Turn_2, 5;
FlipReverse;
BBSprite: Run_1, 1;
GotoMarker: Loop;
Exit;

@LoseTargetCallback:
SetVelocityX: 0;
GotoBehavior: Zako2_Idle;
return;

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
GotoBehavior: Zako2_Run;