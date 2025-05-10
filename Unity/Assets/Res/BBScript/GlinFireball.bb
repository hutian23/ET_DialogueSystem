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
BeginLoop: (Counter: Value > 0)
  BBSprite: Frame_2, 4;
  BBSprite: Frame_3, 4;
  BBSprite: Frame_4, 4;
  BBSprite: Frame_5, 4;
  BBSprite: Frame_6, 4;
  BBSprite: Frame_7, 4;
  BBSprite: Frame_8, 4;
  BBSprite: Frame_9, 4;
EndLoop:
Dispose;