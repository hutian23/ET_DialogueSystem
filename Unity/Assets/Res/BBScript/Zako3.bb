[Root]
@RootInit:
PoolObject: GDust, 1;
PoolObject: Javelin, 1;
EnemyInit;
RegistMove: (Zako3_Idle)
  MoveType: None;
EndMove:
RegistMove: (Zako3_Charge)
  MoveType: None;
EndMove:
RegistMove: (Zako3_ThrowAttack)
  MoveType: None;
EndMove:
GotoBehavior: Zako3_Idle;
return;

[Zako3_Idle]
@Trigger:
return;

@Main:
WaitFrame: 10;
Test;
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
# BeginLoopAnim: (Counter: Value > 0)
#   LoopSprite: Idle_1, 5;
#   LoopSprite: Idle_2, 5;
#   LoopSprite: Idle_3, 5;
#   LoopSprite: Idle_4, 5;
#   LoopSprite: Idle_5, 5;
#   LoopSprite: Idle_6, 5;
#   LoopSprite: Idle_7, 5;
# EndLoopAnim:
WaitFrame: 1000;
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

[Zako3_ThrowAttack]
@Trigger:
return;

@Main:
BBSprite: Anticipate_1, 4;
BBSprite: Anticipate_2, 4;
BBSprite: Anticipate_3, 4;
BBSprite: Anticipate_4, 4;
BBSprite: Anticipate_5, 4;
BBSprite: Anticipate_6, 8;
CreateBullet: Javelin
  BulletVelocity: 500000, 0;
  BulletLocalPosition: -25000, -4500;
EndCreateBullet:
BBSprite: Throw, 8;
BBSprite: End_1, 4;
BBSprite: End_2, 4;
BBSprite: End_3, 4;
BBSprite: End_4, 4;
BBSprite: End_5, 4;
BBSprite: End_6, 4;
Exit;
