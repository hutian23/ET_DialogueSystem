[Root]
@RootInit:
BulletInit;
RegistMove: (GlinBullet_Idle)
  MoveType: None;
EndMove:
GotoBehavior: GlinBullet_Idle;
return;

[GlinBullet_Idle]
@Trigger:
return;

@Main:
BBSprite: Frame_1, 5;
RegistCounter: 100;
BeginLoopAnim: (Counter: Value > 0)
  LoopSprite: Frame_2, 4;
  LoopSprite: Frame_3, 4;
  LoopSprite: Frame_4, 4;
  LoopSprite: Frame_5, 4;
EndLoopAnim:
Dispose;