# Delta Robot 控制系统

## 项目概述

Delta Robot 控制系统是一个基于C#开发的并联机器人控制软件，主要用于控制Delta型并联机器人的运动。该系统通过IMC运动控制卡与机器人硬件进行通信，实现了多种控制模式，包括脉冲控制、角度控制和距离控制等功能。

## 系统特点

- **多种控制模式**：支持脉冲控制模式、角度控制模式和距离控制模式
- **运动学计算**：实现了Delta并联机器人的正向和逆向运动学计算
- **轨迹规划**：支持点到点运动、直线插补和圆弧插补等轨迹规划功能
- **白板控制**：内置白板控制功能，可以通过绘制轨迹控制机器人运动
- **多轴协调**：支持多轴协调运动控制
- **实时监控**：提供实时位置、速度和错误状态监控

## 系统架构

系统主要由以下几个部分组成：

1. **用户界面**：基于Windows Forms开发的图形用户界面
2. **运动控制**：基于IMC运动控制卡的运动控制模块
3. **运动学计算**：Delta机器人的正逆运动学计算模块
4. **轨迹规划**：多种轨迹规划算法实现
5. **白板控制**：用于轨迹绘制和控制的模块

## 主要功能模块

### 1. 设备控制

- 设备连接与断开
- 设备参数配置
- 设备状态监控

### 2. 运动控制

- 点动控制（Jog）
- 点到点运动（P2P）
- 直线插补（Linear Interpolation）
- 圆弧插补（Circular Interpolation）
- 归零操作（Home）
- 急停控制（Emergency Stop）

### 3. 运动学计算

- 正向运动学：根据关节角度计算末端执行器位置
- 逆向运动学：根据末端执行器位置计算关节角度

### 4. 轨迹规划

- S曲线速度规划
- 多段轨迹规划
- 轨迹平滑处理

### 5. 白板控制

- 轨迹绘制
- 轨迹记录
- 轨迹回放
- 多段轨迹处理

## 技术参数

- **静平台半径**：173.205 mm
- **动平台半径**：34 mm
- **连杆长度**：270 mm
- **最大速度**：100 mm/s
- **最大加速度**：100 mm/s²
- **最大加加速度**：100 mm/s³

## 控制模式参数设置

### 脉冲控制模式

| 参数       | 值    |
| ---------- | ----- |
| 细分值     | 10000 |
| 转脉冲数   | 10000 |
| 输入脉冲数 | 10000 |
| 初始位置   | 0     |
| 最终位置   | 10000 |

### 角度控制模式

| 参数     | 值    |
| -------- | ----- |
| 细分值   | 10000 |
| 转脉冲数 | 10000 |
| 输入角度 | 360° |
| 初始位置 | 0°   |
| 最终位置 | 360° |

### 距离控制模式

| 参数     | 值    |
| -------- | ----- |
| 细分值   | 10000 |
| 转脉冲数 | 10000 |
| 输入距离 | 10 mm |
| 初始位置 | 0 mm  |
| 最终位置 | 10 mm |

## 系统依赖

- .NET Framework 4.5+
- IMC运动控制卡驱动程序
- IMC_Pkg4xxx.dll
- IMCnet4xxx.dll

## 文件结构

```
delta_robot/
├── delta_robot/             # 主项目目录
│   ├── bin/                 # 编译输出目录
│   ├── obj/                 # 编译中间文件
│   ├── Properties/          # 项目属性
│   ├── Form1.cs             # 主窗体
│   ├── robotarm.cs          # 机器人运动学计算
│   ├── WhiteboardControl.cs # 白板控制
│   ├── Global.cs            # 全局变量和函数
│   ├── delta_robot.csproj   # 项目文件
│   └── ...                  # 其他功能模块
├── packages/                # NuGet包
├── IMCnet4xxx.dll           # IMC控制卡DLL
├── IMC_Pkg4xxx.dll          # IMC包装DLL
├── IMC_Pkg4xxx.cs           # IMC接口定义
├── IMC_cfg4xxx.ini          # IMC配置文件
└── delta_robot.sln          # 解决方案文件
```

## 使用说明

### 1. 设备连接

1. 启动软件
2. 选择网卡和设备ID
3. 点击"打开设备"按钮连接设备

### 2. 机器人归零

1. 确保设备已连接
2. 点击"归零"按钮开始归零操作
3. 等待归零完成

### 3. 点动控制

1. 在点动控制面板中设置速度和步长
2. 点击方向按钮控制机器人运动

### 4. 点到点运动

1. 在点到点运动面板中设置目标位置
2. 设置运动参数（速度、加速度等）
3. 点击"开始运动"按钮

### 5. 轨迹绘制与执行

1. 在白板控制面板中绘制轨迹
2. 点击"记录轨迹"按钮记录轨迹
3. 点击"执行轨迹"按钮让机器人执行绘制的轨迹

### 6. 急停操作

- 在任何情况下，如需紧急停止机器人，点击"急停"按钮

## 注意事项

1. 使用前请确保机器人处于安全位置
2. 首次使用时需要进行归零操作
3. 运动参数设置不当可能导致机器人运动异常
4. 在进行任何操作前，请确保了解机器人的工作空间和限制
5. 定期检查机器人的机械结构和电气连接

## 故障排除

1. **设备无法连接**

   - 检查网络连接
   - 确认设备ID设置正确
   - 检查IMC控制卡驱动是否正确安装
2. **运动异常**

   - 检查运动参数设置
   - 确认机器人是否已归零
   - 检查是否超出工作空间
3. **位置误差**

   - 校准机器人参数
   - 检查运动学计算参数
   - 确认编码器设置正确

## 开发信息

本系统基于C#和.NET Framework开发，使用Visual Studio作为开发环境。系统使用IMC运动控制卡作为硬件接口，通过调用IMC提供的API实现对机器人的控制。

主要开发技术：

- C# 编程语言
- Windows Forms 用户界面
- IMC运动控制API
- 并联机器人运动学算法
- S曲线轨迹规划算法

## 版本历史

- v1.0.0：初始版本，实现基本控制功能
- v1.1.0：添加白板控制功能
- v1.2.0：优化运动学计算和轨迹规划算法
- v1.3.0：添加多段轨迹功能

## 联系方式

如有任何问题或建议，请联系开发团队。
