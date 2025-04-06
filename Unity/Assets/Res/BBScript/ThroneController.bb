[Root]
@RootInit:
Thrones_Init;
Thrones_Goto: Thrones_Step1_Entry;
return;

# 一阶段
[Thrones_Step1_Entry]
@Main:
Thrones_Param: 1, PosX, 200;
Thrones_Param: 1, PosY, 11850;
Thrones_Param: 1, Flip, Right;
Thrones_SubState: 1, Boss_Gesture;
Thrones_Param: 2, PosX, 7200;
Thrones_Param: 2, PosY, 10300;
Thrones_Param: 2, Flip, Right;
Thrones_SubState: 2, Boss_Gesture;
Thrones_Param: 3, PosX, -7200;
Thrones_Param: 3, PosY, 10300;
Thrones_Param: 3, Flip, Left;
Thrones_SubState: 3, Boss_Gesture;
Thrones_WaitFrame: 100;
Thrones_SubState: 1, Boss_Entry;
Thrones_WaitFrame: 150;
# 测试
ThronesTest:
  BeginIf: (ThronesTest_Input: A)
    Thrones_SubState: 1, Boss_Dead;
    Thrones_WaitFrame: 60;
    Thrones_Goto: Thrones_Step2_Entry;
    EndIf:
  EndThronesTest:
# Exit
Random: ran, 0, 100;
BeginIf: (Random: ran < 20)
  Thrones_Goto: Thrones_Step1_Dstab;
  EndIf:
BeginIf: (Random: ran < 40)
  Thrones_Goto: Thrones_Step1_WallThrow;
  EndIf:
BeginIf: (Random: ran <= 100)
  Thrones_Goto: Thrones_Step1_Dash;
  EndIf:

[Thrones_Step1_Dstab]
@Main:
Thrones_SubState: 1, Boss_Dstab;
Thrones_WaitFrame: 120;
# Exit
Random: ran, 0, 100;
BeginIf: (Random: ran < 20)
  Thrones_Goto: Thrones_Step1_Dstab;
  EndIf:
BeginIf: (Random: ran < 30)
  Thrones_Goto: Thrones_Step1_WallThrow;
  EndIf:
BeginIf: (Random: ran <= 100)
  Thrones_Goto: Thrones_Step1_Dash;
  EndIf:

[Thrones_Step1_Dash]
@Main:
Random: ran, 0, 1;
BeginIf: (Random: ran == 0)
  Thrones_Param: 1, Flip, Left;
  Thrones_Param: 1, PosX, -10500;
  Thrones_Param: 1, PosY, 3100;
  Thrones_Param: 1, VelX, -100000;
  Thrones_Param: 1, ReachX, 9500;
  Thrones_Param: 1, Direction, Right;
  EndIf:
BeginIf: (Random: ran == 1)
  Thrones_Param: 1, Flip, Right;
  Thrones_Param: 1, PosX, 10500;
  Thrones_Param: 1, PosY, 3100;
  Thrones_Param: 1, VelX, -100000;
  Thrones_Param: 1, ReachX, -9500;
  Thrones_Param: 1, Direction, Left;
  EndIf:
Thrones_SubState: 1, Boss_Dash;
Thrones_WaitFrame: 120;
# Exit
Random: ran, 0, 100;
BeginIf: (Random: ran < 20)
  Thrones_Goto: Thrones_Step1_Dash;
  EndIf:
BeginIf: (Random: ran < 45)
  Thrones_Goto: Thrones_Step1_WallThrow;
  EndIf:
BeginIf: (Random: ran <= 100)
  Thrones_Goto: Thrones_Step1_Dstab;
  EndIf:

[Thrones_Step1_WallThrow]
@Main:
Random: ran, 0, 1;
BeginIf: (Random: ran == 0)
  Thrones_Param: 1, Flip, Left;
  Thrones_Param: 1, PosX, 14600;
  Thrones_Param: 1, PosY, 5000;
  EndIf:
BeginIf: (Random: ran == 1)
  Thrones_Param: 1, Flip, Right;
  Thrones_Param: 1, PosX, -14600;
  Thrones_Param: 1, PosY, 5000;
  EndIf:
Thrones_SubState: 1, Boss_WallThrow;
Thrones_WaitFrame: 120;
# Exit
Random: ran, 0, 100;
BeginIf: (Random: ran < 40)
  Thrones_Goto: Thrones_Step1_Dstab;
  EndIf:
BeginIf: (Random: ran <= 100)
  Thrones_Goto: Thrones_Step1_Dash;
  EndIf:

# 二阶段
[Thrones_Step2_Entry]
@Main:
#Throne_1
Thrones_Param: 1, PosX, 200;
Thrones_Param: 1, PosY, 11850;
Thrones_Param: 1, Flip, Right;
Thrones_SubState: 1, Boss_Wounded;
#Throne_2
Thrones_Param: 2, PosX, 7200;
Thrones_Param: 2, PosY, 10300;
Thrones_Param: 2, Flip, Right;
Thrones_SubState: 2, Boss_Gesture;
#Throne_3
Thrones_Param: 3, PosX, -7200;
Thrones_Param: 3, PosY, 10300;
Thrones_Param: 3, Flip, Left;
Thrones_SubState: 3, Boss_Gesture;
Thrones_WaitFrame: 100;
Thrones_SubState: 2, Boss_TrippleEntry;
Thrones_SubState: 3, Boss_TrippleEntry;
Thrones_WaitFrame: 60;
Thrones_SubState: 1, Boss_TrippleEntry_1;
Thrones_WaitFrame: 300;
# 测试1
ThronesTest: 
  BeginIf: (ThronesTest_Input: A)
    Thrones_SubState: 1, Boss_Dead;
    Thrones_WaitFrame: 60;
    Thrones_Param: 1, PosX, -7200;
    Thrones_Param: 1, PosY, 10300;
    Thrones_Param: 1, Flip, Left;
    Thrones_SubState: 1, Boss_Wounded;
    ThronesTest_DeadFlag: 1;
    Thrones_WaitFrame: 50;
    Thrones_Goto: Thrones_Step2_Random;
    EndIf:
  BeginIf: (ThronesTest_Input: S)
    Thrones_SubState: 2, Boss_Dead;
    Thrones_WaitFrame: 60;
    Thrones_Param: 2, PosX, 7200;
    Thrones_Param: 2, PosY, 10300;
    Thrones_Param: 2, Flip, Right;
    Thrones_SubState: 2, Boss_Wounded;
    ThronesTest_DeadFlag: 2;
    Thrones_WaitFrame: 50;
    Thrones_Goto: Thrones_Step2_Random;
    EndIf:
  BeginIf: (ThronesTest_Input: D)
    Thrones_SubState: 3, Boss_Dead;
    Thrones_WaitFrame: 60;
    Thrones_Param: 3, PosX, 200;
    Thrones_Param: 3, PosY, 11850;
    Thrones_Param: 3, Flip, Right;
    Thrones_SubState: 3, Boss_Wounded;
    ThronesTest_DeadFlag: 3;
    ThronesTest_Exit;
    Thrones_WaitFrame: 100;
    Thrones_Goto: Thrones_Exit;
    EndIf:
  EndThronesTest:
Thrones_Goto: Thrones_Step2_Random;

[Thrones_Step2_TwoWallThrow]
@Main:
#Throne_1
Thrones_Param: 1, Flip, Left;
Thrones_Param: 1, PosX, 14600;
Thrones_Param: 1, PosY, 5000;
Thrones_SubState: 1, Boss_WallThrow;
#Throne_2
Thrones_Param: 2, Flip, Right;
Thrones_Param: 2, PosX, -14600;
Thrones_Param: 2, PosY, 5000;
Thrones_SubState: 2, Boss_WallThrow;
Thrones_WaitFrame: 75;
#Throne_3
Thrones_SubState: 3, Boss_Dstab;
Thrones_WaitFrame: 100;
Thrones_Goto: Thrones_Step2_Random;

[Thrones_Step2_TwoDash]
@Main:
#Throne_1
Thrones_Param: 1, Flip, Left;
Thrones_Param: 1, PosX, -10500;
Thrones_Param: 1, PosY, 3100;
Thrones_Param: 1, VelX, -100000;
Thrones_Param: 1, ReachX, 9500;
Thrones_Param: 1, Direction, Right;
Thrones_SubState: 1, Boss_Dash;
#Throne_2
Thrones_Param: 2, Flip, Right;
Thrones_Param: 2, PosX, 10500;
Thrones_Param: 2, PosY, 3100;
Thrones_Param: 2, VelX, -100000;
Thrones_Param: 2, ReachX, -9500;
Thrones_Param: 2, Direction, Left;
Thrones_SubState: 2, Boss_Dash;
Thrones_WaitFrame: 50;
#Throne_3
Thrones_SubState: 3, Boss_Dstab;
Thrones_WaitFrame: 100;
Thrones_Goto: Thrones_Step2_Random;

[Thrones_Step2_Dstab]
@Main:
#Thrones_1
Thrones_SubState: 1, Boss_Dstab;
Thrones_WaitFrame: 50;
#Thrones_2
Thrones_SubState: 2, Boss_Dstab;
Thrones_WaitFrame: 50;
#Thrones_3
Thrones_SubState: 3, Boss_Dstab;
Thrones_WaitFrame: 100;
Thrones_Goto: Thrones_Step2_Random;

[Thrones_Step2_Random]
@Main:
#Throne_1
Random: ran, 0, 100;
#Throne_1_Dstab
BeginIf: (Random: ran < 40)
  Thrones_SubState: 1, Boss_Dstab;
  EndIf:
#Throne_1_Dash
BeginIf: (Random: ran < 80), (Random: ran >= 40)
  Random: ran_1, 0, 1;
  #DashLeft
  BeginIf: (Random: ran_1 == 0)
    Thrones_Param: 1, Flip, Left;
    Thrones_Param: 1, PosX, -10500;
    Thrones_Param: 1, PosY, 3100;
    Thrones_Param: 1, VelX, -100000;
    Thrones_Param: 1, ReachX, 9500;
    Thrones_Param: 1, Direction, Right;
    EndIf:
  #DashRight
  BeginIf: (Random: ran_1 == 1)
    Thrones_Param: 1, Flip, Right;
    Thrones_Param: 1, PosX, 10500;
    Thrones_Param: 1, PosY, 3100;
    Thrones_Param: 1, VelX, -100000;
    Thrones_Param: 1, ReachX, -9500;
    Thrones_Param: 1, Direction, Left;
    EndIf:
  Thrones_SubState: 1, Boss_Dash;
  EndIf:
#Throne_1_WallThrow
BeginIf: (Random: ran <= 100), (Random: ran >= 80)
  Random: ran_1, 0, 1;
  #WallThrowLeft
  BeginIf: (Random: ran_1 == 0)
    Thrones_Param: 1, Flip, Left;
    Thrones_Param: 1, PosX, 14600;
    Thrones_Param: 1, PosY, 5000;
    EndIf:
  #WallThrowRight
  BeginIf: (Random: ran_1 == 1)
    Thrones_Param: 1, Flip, Right;
    Thrones_Param: 1, PosX, -14600;
    Thrones_Param: 1, PosY, 5000;
    EndIf:
  Thrones_SubState: 1, Boss_WallThrow;
  EndIf:
#Throne_2
Thrones_WaitFrame: 50;
Random: ran, 0, 100;
#Throne_2_Dstab
BeginIf: (Random: ran < 40)
  Thrones_SubState: 2, Boss_Dstab;
  EndIf:
#Throne_2_Dash
BeginIf: (Random: ran < 80), (Random: ran >= 40)
  Random: ran_1, 0, 1;
  #DashLeft
  BeginIf: (Random: ran_1 == 0)
    Thrones_Param: 2, Flip, Left;
    Thrones_Param: 2, PosX, -10500;
    Thrones_Param: 2, PosY, 3100;
    Thrones_Param: 2, VelX, -100000;
    Thrones_Param: 2, ReachX, 9500;
    Thrones_Param: 2, Direction, Right;
    EndIf:
  #DashRight
  BeginIf: (Random: ran_1 == 1)
    Thrones_Param: 2, Flip, Right;
    Thrones_Param: 2, PosX, 10500;
    Thrones_Param: 2, PosY, 3100;
    Thrones_Param: 2, VelX, -100000;
    Thrones_Param: 2, ReachX, -9500;
    Thrones_Param: 2, Direction, Left;
    EndIf:
  Thrones_SubState: 2, Boss_Dash;
  EndIf:
#Throne_2_WallThrow
BeginIf: (Random: ran <= 100), (Random: ran >= 80)
  Random: ran_1, 0, 1;
  #WallThrowLeft
  BeginIf: (Random: ran_1 == 0)
    Thrones_Param: 2, Flip, Left;
    Thrones_Param: 2, PosX, 14600;
    Thrones_Param: 2, PosY, 9000;
    EndIf:
  #WallThrowRight
  BeginIf: (Random: ran_1 == 1)
    Thrones_Param: 2, Flip, Right;
    Thrones_Param: 2, PosX, -14600;
    Thrones_Param: 2, PosY, 9000;
    EndIf:
  Thrones_SubState: 2, Boss_WallThrow;
  EndIf:
#Throne_3
Thrones_WaitFrame: 50;
Random: ran, 0, 100;
#Throne_3_Dstab
BeginIf: (Random: ran < 40)
  Thrones_SubState: 3, Boss_Dstab;
  EndIf:
#Throne_3_Dash
BeginIf: (Random: ran < 80), (Random: ran >= 40)
  Random: ran_1, 0, 1;
  #DashLeft
  BeginIf: (Random: ran_1 == 0)
    Thrones_Param: 3, Flip, Left;
    Thrones_Param: 3, PosX, -10500;
    Thrones_Param: 3, PosY, 3100;
    Thrones_Param: 3, VelX, -100000;
    Thrones_Param: 3, ReachX, 9500;
    Thrones_Param: 3, Direction, Right;
    EndIf:
  #DashRight
  BeginIf: (Random: ran_1 == 1)
    Thrones_Param: 3, Flip, Right;
    Thrones_Param: 3, PosX, 10500;
    Thrones_Param: 3, PosY, 3100;
    Thrones_Param: 3, VelX, -100000;
    Thrones_Param: 3, ReachX, -9500;
    Thrones_Param: 3, Direction, Left;
    EndIf:
  Thrones_SubState: 3, Boss_Dash;
  EndIf:
#Throne_2_WallThrow
BeginIf: (Random: ran <= 100), (Random: ran >= 80)
  Random: ran_1, 0, 1;
  #WallThrowLeft
  BeginIf: (Random: ran_1 == 0)
    Thrones_Param: 3, Flip, Left;
    Thrones_Param: 3, PosX, 14600;
    Thrones_Param: 3, PosY, 5000;
    EndIf:
  #WallThrowRight
  BeginIf: (Random: ran_1 == 1)
    Thrones_Param: 3, Flip, Right;
    Thrones_Param: 3, PosX, -14600;
    Thrones_Param: 3, PosY, 5000;
    EndIf:
  Thrones_SubState: 3, Boss_WallThrow;
  EndIf:
Thrones_WaitFrame: 100;
#Exit
# 注册一个随机变量ran
Random: ran, 0, 100;
# Ran < 50，Boss进入Thrones_Step2_Random这个技能
BeginIf: (Random: ran < 50)
  Thrones_Goto: Thrones_Step2_Random;
EndIf:
# 50 <= Ran < 70
BeginIf: (Random: ran < 70), (Random: ran >= 50)
  Thrones_Goto: Thrones_Step2_TwoDash;
EndIf:
BeginIf: (Random: ran < 85), (Random: ran >= 70)
  Thrones_Goto: Thrones_Step2_TwoWallThrow;
EndIf:
BeginIf: (Random: ran <= 100), (Random: ran >= 85)
  Thrones_Goto: Thrones_Step2_Dstab;
EndIf:

[Thrones_Exit]
@Main:
Thrones_SubState: 1, Boss_Bow;
Thrones_SubState: 2, Boss_Bow;
Thrones_SubState: 3, Boss_Bow;
return;