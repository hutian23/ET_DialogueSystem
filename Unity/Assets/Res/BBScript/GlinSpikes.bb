[Root]
@RootInit:
BulletInit;
RegistMove: (GlinSpikes_Idle)
  MoveType: None;
EndMove:
GotoBehavior: GlinSpikes_Idle;
return;

[GlinSpikes_Idle]
@Trigger:
return;

@Main:
BBSprite: Ready_1, 5;
BBSprite: Ready_2, 5;
BBSprite: Ready_3, 30;
BBSprite: Anticipate_1, 3;
BBSprite: Anticipate_2, 3;
BBSprite: Anticipate_3, 3;
BBSprite: Anticipate_4, 3;
BBSprite: Up_1, 3;
ScreenShake: 1050, 250, 10000, 20, 0;
BBSprite: Up_2, 60;
BBSprite: Down_1, 4;
BBSprite: Down_2, 4;
BBSprite: Down_3, 4;
BBSprite: Down_4, 4;
BBSprite: Down_5, 4;
Dispose;