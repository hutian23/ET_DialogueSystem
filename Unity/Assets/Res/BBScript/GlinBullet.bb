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
RegistCounter: 40;
BeginLoop: (Counter: Value > 0)
  BBSprite: Frame_2, 5;
  BBSprite: Frame_3, 5;
  BBSprite: Frame_4, 5;
  BBSprite: Frame_5, 5;
EndLoop:
BBSprite: Frame_6, 5;
Dispose;