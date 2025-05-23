[Root]
@RootInit:
PoolObject: GDust, 1;
EnemyInit;
RegistMove: (Javelin_Idle)
  MoveType: None;
EndMove:
RegistMove: (Javelin_Land)
  MoveType: None;
EndMove:
GotoBehavior: Javelin_Idle;
return;


[Javelin_Idle]
@Trigger:
return;

@Main:
EnableGroundCollisionCheck: true;
BBSprite: Attack_1, 1;
BeginLoopAnim: (GroundCollision: false)
  LoopSprite: Attack_2, 4;
  LoopSprite: Attack_3, 4;
  LoopSprite: Attack_4, 4;
EndLoopAnim:
EnableGroundCollisionCheck: false;
SetVelocity: 0, 0;
Shake: 200, 600, 12000, 25;
BBSprite: Land, 200;
BBSprite: End_1, 4;
BBSprite: End_2, 4;
BBSprite: End_3, 4;
BBSprite: End_4, 4;
Dispose;