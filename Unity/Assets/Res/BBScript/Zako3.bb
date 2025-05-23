[Root]
@RootInit:
PoolObject: GDust, 1;
PoolObject: Javelin, 1;
EnemyInit;
RegistMove: (Zako3_Idle)
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
SetPos: -60000, 0;
GotoBehavior: Zako3_BattleIdle;
return;

[Zako3_Idle]
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
BeginLoop: (Counter: Value > 0)
  BBSprite: Idle_1, 5;
  BBSprite: Idle_2, 5;
  BBSprite: Idle_3, 5;
  BBSprite: Idle_4, 5;
  BBSprite: Idle_5, 5;
  BBSprite: Idle_6, 5;
  BBSprite: Idle_7, 5;
EndLoop:
# Turn
SetVelocity: 0, 0;
Random: ran2, 0, 100;
BeginIf: (Random: ran2 >= 0), (Random: ran2 <= 40)
  BBSprite: Turn_1, 5;
  BBSprite: Turn_2, 5;
  FlipReverse;
  BBSprite: Idle_1, 1;
EndIf:
GotoMarker: Loop;
return;

[Zako3_BattleIdle]
@Trigger:
return;

@Main:
EnableWaitFrameCallback: true, 50, Zako3_BattleIdle, WaitFrameCallback;
SetMarker: Loop;
AirPatrolVelocity: 15000;
RegistCounter: 28;
BeginLoop: (Counter: Value > 0)
  BBSprite: Idle_1, 4;
  BBSprite: Idle_2, 4;
  BBSprite: Idle_3, 4;
  BBSprite: Idle_4, 4;
  BBSprite: Idle_5, 4;
  BBSprite: Idle_6, 4;
  BBSprite: Idle_7, 4;
EndLoop:
GotoMarker: Loop;
return;

@WaitFrameCallback:
GotoBehavior: Zako3_ThrowAttack;
return;

[Zako3_ThrowAttack]
@Trigger:
return;

@Main:
SetVelocity: 0, 0;
BBSprite: Anticipate_1, 4;
BBSprite: Anticipate_2, 4;
BBSprite: Anticipate_3, 4;
BBSprite: Anticipate_4, 4;
BBSprite: Anticipate_5, 4;
BBSprite: Anticipate_6, 8;
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