[Root]
@RootInit:
PoolObject: GDust, 1;
EnemyInit;
RegistMove: (Zako3_Idle)
  MoveType: None;
EndMove:
RegistMove: (Zako3_Charge)
  MoveType: None;
EndMove:
GotoBehavior: Zako3_Idle;
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
