[Root]
@RootInit:
PoolObject: GDust, 1;
EnemyInit;
RegistMove: (Javelin_Idle)
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
BeginLoop: (GroundCollision: false)
  BBSprite: Attack_2, 4;
  BBSprite: Attack_3, 4;
  BBSprite: Attack_4, 4;
EndLoop:
EnableGroundCollisionCheck: false;
SetVelocity: 0, 0;
Shake: 200, 800, 15000, 25;
BBSprite: Land, 20;
BBSprite: End_1, 4;
BBSprite: End_2, 4;
BBSprite: End_3, 4;
BBSprite: End_4, 4;
Dispose;