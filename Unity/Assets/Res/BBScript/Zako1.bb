[Root]
@RootInit:
GlinInit;
PoolObject: ADust, 1;
RegistMove: (Zako1_Idle)
  MoveType: None;
EndMove:
RegistMove: (Zako1_Teleport)
  MoveType: None;
EndMove:
RegistMove: (Zako1_Dash)
  MoveType: None;
EndMove:
RegistMove: (Zako1_Throw)
  MoveType: None;
EndMove:
RegistMove: (Zako1_Death)
  MoveType: None;
EndMove:
GotoBehavior: Zako1_Idle;
return;

[Zako1_Idle]
@Trigger:
return;

@Main:
# 更新判定框
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

[Zako1_Teleport]
@Trigger:
return;

@Main:
PlayTimeline: 0, 40;
GotoBehavior: Zako1_Death;

[Zako1_Dash]
@Trigger:
return;

@Main:
SetPos: 0, 0;
BBSprite: Anticipate_1, 5;
BBSprite: Anticipate_2, 5;
BBSprite: Anticipate_3, 5;
BBSprite: Anticipate_4, 5;
BBSprite: Anticipate_5, 10;
BBSprite: Active_1, 5;
SpawnADust: 35000, -12000, 8000, 5000, 0;
BBSprite: Active_2, 5;
BBSprite: Active_3, 5;
BBSprite: Active_4, 5;
BBSprite: Active_5, 5;
BBSprite: End_1, 5;
BBSprite: End_2, 5;
BBSprite: End_3, 5;
BBSprite: End_4, 5;
BBSprite: End_5, 5;
GotoBehavior: Zako1_Dash;

[Zako1_Throw]
@Trigger:
return;

@Main:
BBSprite: Anticipate_1, 5;
BBSprite: Anticipate_2, 5;
BBSprite: Anticipate_3, 5;
BBSprite: Anticipate_4, 5;
BBSprite: Anticipate_5, 10;
BBSprite: Active_1, 5;
BBSprite: Active_2, 5;
BBSprite: End_1, 5;
Exit;

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
