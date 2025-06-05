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
SetVelocityX: 200000;
BBSprite: Anticipate_2, 3;
SetVelocityX: 250000;
BBSprite: Active_1, 3;
SetVelocityX: 200000;
BBSprite: Active_1, 3;
SetVelocityX: 100000;
BBSprite: Active_1, 4;
SetVelocityX: 50000;
BBSprite: End_1, 4;
SetVelocityX: 0;
BBSprite: End_2, 4;
BBSprite: End_3, 4;
BBSprite: End_4, 4;
BBSprite: End_5, 4;
Dispose;