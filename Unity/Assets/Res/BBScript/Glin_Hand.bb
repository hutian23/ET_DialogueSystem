[Root]
@RootInit:
EffectInit;
RegistMove: (Glin_Hand_Idle) 
  MoveType: None;
EndMove:
GotoBehavior: Glin_Hand_Idle;
return;

[Glin_Hand_Idle]
@Trigger:
return;

@Main:
BBSprite: Hand_1, 4;
BBSprite: Hand_2, 4;
BBSprite: Hand_3, 4;
BBSprite: Hand_4, 4;
BBSprite: Hand_5, 4;
BBSprite: Hand_6, 4;
BBSprite: Hand_8, 20;
BBSprite: Click_1, 4;
BBSprite: Click_2, 4;
BBSprite: Click_3, 85;
BBSprite: Hand_8, 4;
BBSprite: Hand_5, 6;
BBSprite: Hand_4, 6;
BBSprite: Hand_3, 6;
BBSprite: Hand_2, 6;
BBSprite: Hand_1, 6;
Dispose;