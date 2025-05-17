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
BeginLoop: (Counter: Value > 0)
  BBSprite: Frame_2, 4;
  BBSprite: Frame_3, 4;
  BBSprite: Frame_4, 4;
  BBSprite: Frame_5, 4;
EndLoop:
Dispose;