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
SetVelocityX: -300000;
AccelY: 20000, 80, 50000;
BBSprite: Frame_1, 5;
RegistCounter: 100;
BeginLoop: (Counter: Value > 0)
  BBSprite: Frame_2, 5;
  BBSprite: Frame_3, 5;
  BBSprite: Frame_4, 5;
  BBSprite: Frame_5, 5;
EndLoop:
BBSprite: Frame_6, 5;
Dispose;