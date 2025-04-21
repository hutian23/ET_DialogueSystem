[Root]
@RootInit:
BulletInit;
# 注册动作
RegistMove: (DeadSpike_Idle)
  MoveType: None;
EndMove:
# 进入默认动作
GotoBehavior: DeadSpike_Idle;
return;

[DeadSpike_Idle]
@Trigger:
return;

@Main:
BBSprite: Frame_1, 3;
SetVelocityX: 500000;
BBSprite: Frame_2, 3;
BBSprite: Frame_3, 5;
SetVelocityX: 400000;
BBSprite: Frame_3, 5;
SetVelocityX: 150000;
BBSprite: Frame_4, 3;
SetVelocityX: 80000;
BBSprite: Frame_5, 3;
SetVelocityX: 20000;
BBSprite: Frame_6, 4;
SetVelocityX: 0;
BBSprite: Frame_7, 4;
BBSprite: Frame_8, 4;
BBSprite: Frame_9, 4;
Dispose;