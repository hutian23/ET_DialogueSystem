[Root]
@RootInit:
BulletInit;
RegistMove: (GlinFireball_Idle)
  MoveType: None;
EndMove:
GotoBehavior: GlinFireball_Idle;
return;

[GlinFireball_Idle]
@Trigger:
return;

@Main:
BBSprite: Frame_1, 5;
RegistCounter: 140;
BeginLoopAnim: (Counter: Value > 0)
  LoopSprite: Frame_2, 4;
  LoopSprite: Frame_3, 4;
  LoopSprite: Frame_4, 4;
  LoopSprite: Frame_5, 4;
  LoopSprite: Frame_6, 4;
  LoopSprite: Frame_7, 4;
  LoopSprite: Frame_8, 4;
  LoopSprite: Frame_9, 4;
EndLoopAnim:
Dispose;