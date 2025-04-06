[Root]
@RootInit:
#初始化，添加依赖的组件
PlayerInit;
# 跳转到Idle.Main
GotoBehavior: 'Idle';

[Idle]
@Trigger:
return;

@Main:
LogWarning: 'HelloWorld';
SetMarker: 'Loop';
WaitFrame: 20;
LogWarning: 'Idle';
WaitFrame: 20;
# 逻辑跳转
GotoMarker: 'Loop';
return;

[Test]
@Test:
LogWarning: 'Test';
return;

@Test1:
LogWarning: 'Test1';
WaitFrame: 10;
LogWarning: 'Test1_1';
return;

@Test2:
return;