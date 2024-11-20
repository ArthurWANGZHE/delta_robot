using System;


namespace imcpkg
{

//***********************************************************************************************************/
//       此头文件为iMC参数地址的宏定义                                                                      */
//       格式：                                                                                             */
//       //描述                                                                                             */
//       public const Int16   参数名Loc =      地址     // 轴或全局参数  数据格式                           */
//                                                                                                          */
//       其中“参数名”是指在iMC的参数名，其后加Loc表示该参数的地址                                         */
//                                                                                                          */
//       字母A表示轴参数，字母G表示全局参数                                                                 */
//                                                                                                          */
//       iMC中共有以下几种数据格式的类型：                                                                  */
//       S32：32位带符号位的整数，即小数部分的位数为0，参数值的范围为：[-2147483648,2147483647]。           */
//       U32：32位无符号位的整数，即小数部分的位数为0，参数值的范围为：[0, 4294967295]。                    */
//       S16：16位带符号位的整数，即小数部分的位数为0，参数值的范围为：[-32768,32767]。                     */
//       U16：16位无符号位的整数，即小数部分的位数为0，参数值的范围为：[0, 65535]。                         */
//       F16：16位标志参数，仅取两个值：0或FFFFh。                                                          */
//       R16：16位寄存器，各位域具有具体的意义，部分位域需设置。                                            */
//       S16.32：48位带符号位，高16位为整数部分，低32位为小数部分，参数值的范围：[-32768.0,32767.999999999767] */
//       U16.32：48位无符号位，高16位为整数部分，低32位为小数部分，参数值的范围：[0.0,65535.999999999767]   */
//       S16.16：32位带符号位，高16位为整数部分，低16位为小数部分，参数值的范围：[-32768.0,32767.999984741211]  */
//       U16.16：32位无符号位，高16位为整数部分，低32位为小数部分，参数值的范围：[0.0,65535.999984741211]。 */
//       S0.16：16位有符号位，16位为小数部分。                                                              */
//       因此，iMC中的参数值所表示的实际值为：                                                              */
//       实际值=参数值/2^n                                                                                  */
//       其中n为小数部分的位数。                                                                            */
//       例如，对于一个S32格式的数，“00018000h”（h表示十六进制表示）表示的十进制的值为98304/2^0 = 98304； */
//       对于一个S16.16格式的数，“00018000h”表示的十进制的值为98304/2^16 = 1.5。                          */
//***********************************************************************************************************/

    class ParamDef
    {
	/*********************************************************************************************************/
		#region 点到点运动参数
	/*********************************************************************************************************/
		//连续速度运动的目标速度（
		public const Int16 mcstgvelLoc = 		6;			//A		S16.16
		
		//主坐标系是否处于速度斜升过程中（加速或减速）。FFFFh：处于速度斜升过程，0：等于目标速度运行。
		//另：电子齿轮运动时，可以判断从动轴是否已达到传动速度，即是否达到主动轴速度乘以传动比率的值。0：达到，FFFFh：未达到
		public const Int16 mcsslopeLoc = 		8;			//A		F16

		//点到点运动的指令位置（目标位置）
		public const Int16 mcstgposLoc = 		12;		//A		S32	
		
		//点到点运动的起始速度
		public const Int16 mcsstartvelLoc = 	14;			//A		S16.16	
		
		//点到点运动的最大规划速度
		public const Int16 mcsmaxvelLoc = 	    16;			//A		S16.16	
		
		//点到点运动的加速度
		public const Int16 mcsaccelLoc = 		18;			//A		S16.16
			
		//点到点运动的减速度
		public const Int16 mcsdecelLoc = 		20;			//A		S16.16 
		
		//点到点运动的模式，0：普通模式，FFFFh：跟踪模式
		public const Int16 mcstrackLoc = 		22;			//A		F16			
		
		//写入非零启动点到点运动，写入零，停止当前的点到点运动
		public const Int16 mcsgoLoc = 		    23;			//A		F16	
		
		//点到点运动的终点速度
		public const Int16 mcsendvelLoc = 	    26;			//A	
		
		//目标移动距离，写入的mcsdis必须不为0
		public const Int16 mcsdistLoc = 		56;			//A		S32	

		#endregion
	/*********************************************************************************************************/
		#region 增量式点到点运动
	/*********************************************************************************************************/
		//增量式点到点运动，增量速度的来源轴号
		public const Int16 incmoveaxisLoc = 	194;			//A		S16				
		
		 //若非零，使能某速度值增量运动，如写入encsvel地址时是该轴跟随手轮运动
		public const Int16 incmoveptrLoc = 	    195;			//A		F16			
		
		//增量运动倍率
		public const Int16 incmoverateLoc = 	196;			//A		S32			 

		#endregion
	/*********************************************************************************************************/
		#region 反向间隙误差补偿的参数
	/*********************************************************************************************************/
		//间隙补偿和线性补偿起始速度。
		public const Int16 cpstartvelLoc = 	    41;			//A		S16.16
		
		//间隙补偿和线性补偿最高速度。
		public const Int16 cpmaxvelLoc = 		43;			//A		S16.16

		//间隙补偿和线性补偿加速度。
		public const Int16 cpaccelLoc = 		45;			//A		S16.16

		//间隙补偿和线性补偿减速度。
		public const Int16 cpdecelLoc = 		47;			//A		S16.16

		//补偿使能。写入非零值即使能间隙补偿和线性补偿，清零则禁止。
		public const Int16 enacompenLoc = 	    49;			//A		F16

		//间隙补偿和线性补偿终止速度。
		public const Int16 cpendvelLoc = 		53;			//A		S16.16
		
		//反向间隙误差大小，单位：脉冲。
		public const Int16 backlashLoc = 		229;			//A		S32
		
		#endregion


	/*********************************************************************************************************/
		#region 主编码器相关参数
	/*********************************************************************************************************/
		//当前主编码器速度
		public const Int16 encpvelLoc = 		75;			//A		S16.16 				
		
		//主编码器的计数寄存器
		public const Int16 encpLoc = 			78;			//A		S32						
		
		//主编码器控制寄存器
		public const Int16 encpctrLoc = 		539;			//A					

		#endregion
	/*********************************************************************************************************/
		#region 辅编码器相关参数
	/*********************************************************************************************************/
		//辅编码器的倍率因子
		public const Int16 encsfactorLoc = 	    343;			//G		S16.16 			
		
		//当前辅编码器速度
		public const Int16 encsvelLoc = 		345;			//G		S16.16 				
		
		//辅编码器的计数寄存器
		public const Int16 encsLoc = 			348;			//G		S32						
		
		//辅助编码器的控制寄存器
		public const Int16 encsctrLoc = 		531;			//G		R16					
		
		//写操作清零encs
		public const Int16 clrencsLoc = 		532;			//G		S16					
		
		#endregion
	/*********************************************************************************************************/
		#region 搜原点
	/*********************************************************************************************************/
		//搜索原点时设置原点的方向(间隙补偿方向,0:负方向走时补偿，FFFFh:正方向时补偿)
		public const Int16 homedirLoc = 		235;			//A		
		
		//原点的偏移位置，设置机械原点时，该值被拷贝到指令位置寄存器curpos和编码器寄存器encp中。
		public const Int16 homeposLoc = 		145;			//A		S32				

		//搜寻原点时低速段的速度
		public const Int16 lowvelLoc = 		    147;			//A		S16.16 			

		//搜寻原点时高速段的速度
		public const Int16 highvelLoc = 		149;			//A		S16.16 			
		
		//搜寻原点的过程和方式控制寄存器
		public const Int16 homeseqLoc = 		151;			//A		R16					
		
		//非零则开始执行搜寻原点，清零则停止
		public const Int16 gohomeLoc = 		    152;			//A		F16					
		
		//主机写入非零值则设当前位置为原点
		public const Int16 sethomeLoc = 		153;			//A		F16					
		
		//非零表示已搜寻到原点
		public const Int16 homedLoc = 		    154;			//A		F16						
		
		//记录搜零时走过的距离
		public const Int16 homemovedistLoc = 	163;			//A		S32				

		#endregion
		
	/*********************************************************************************************************/
		#region 其它轴参数	
	/*********************************************************************************************************/

		//写入非零运行该轴，0：停止运行该轴
		public const Int16 runLoc = 			128;			//A		F16						
		
		//错误寄存器
		public const Int16 errorLoc = 		    130;			//A		R16						
		
		//设置最大允许位置误差
		public const Int16 poserrlimLoc = 	    131;			//A		S16					
		
		//平滑因子
		public const Int16 smoothLoc = 		    132;			//A		S16					
		
		//立刻暂停轴
		public const Int16 axispauseatonceLoc = 133;			//A		F16		
		
		//静止窗口
		public const Int16 settlewinLoc = 	    134;			//A		S16					
		
		//从停止运动到静止的时间（周期个数）
		public const Int16 settlenLoc = 		135;		//A		S16					
		
		//错误时的停止过滤寄存器
		public const Int16 stopfiltLoc = 		136;			//A		R16					
		
		//错误时立刻暂停该轴
		public const Int16 stopmodeLoc = 		137;			//A		R16		
		
		//错误时的退出运行过滤寄存器
		public const Int16 exitfiltLoc = 		138;			//A		R16				
		
		//正向软极限位置
		public const Int16 psoftlimLoc = 		139;			//A		S32					
		
		//负向软极限位置
		public const Int16 nsoftlimLoc = 		141;			//A		S32		
		
		//写入非零值对该轴实施清零操作。清零该轴的编码器计数、指令位置、目标位置等各种位置参数，
		//	以及各种运动状态标志：mcspos、mcstgpos、curpos、encp、pcspos、pcstgpos、status、
		//	error、emstop、hpause、events、encs、ticks、aiolat。
		public const Int16 clearLoc = 		    157;			//A		S16		
		
		//设置该轴的最大速度限制。无论何种运动模式，只要实际速度超出此极限值，将置位错误寄存器error中的位VELLIM域，
		//	此错误不可屏蔽，因此只要发生此错误，立刻退出该轴运行。注意：必须为正值。
		public const Int16 vellimLoc = 		    158;			//A		S16.16 			
			
		//设置该轴的最大加速度限制。无论何种运动模式，只要实际加速度超出此极限值，将置位错误寄存器error中的ACCLIM位域，
		//	此错误不可屏蔽，因此只要发生此错误，立刻退出该轴运行。注意：accellim必须为正值。
		public const Int16 accellimLoc = 		160;			//A		S16.16 			
		
		//静止误差补偿速度
		public const Int16 fixvelLoc = 		    162;			//A		S0.16 				
			
		#endregion	
	/*********************************************************************************************************/
		#region 电子齿轮相关的参数
	/*********************************************************************************************************/
		//电子齿轮运动模式中主动轴的轴号
		public const Int16 masterLoc = 		    169;			//A		S16				
		
		//指针，指向从动轴所跟随的主动轴的参数
		public const Int16 gearsrcLoc = 		170;			//A		S16					
		
		//写入非零值开始接合，清零则脱离啮合
		public const Int16 engearLoc = 	    	171;			//A		F16				

		//传动比率  
		public const Int16 gearratioLoc =   	175;		//A		S16.32			
		#endregion	
	/*********************************************************************************************************/
		#region 环形轴相关参数
	/*********************************************************************************************************/
		//设置环形轴的最大位置  
		public const Int16 cirposLoc = 		    184;			//A		S32			
		
		//设置该轴为线性轴或环形轴。若ciraxis为0，该轴为线性轴；若为FFFFh，该轴设置为环形轴。
		public const Int16 ciraxisLoc = 		186;			//A		S16			
		
		//单向或双向环形标志。
		//若为0，为单向环形，位置范围为[0,cirpos)；若为非零，为双向环形，位置范围为(-cirpos,cirpos)。
		public const Int16 biciraxisLoc = 	    187;			//A		S16		 
		
		//记录循环次数，向上溢出加1；向下溢出减1
		public const Int16 cirswapLoc = 		214;			//A		S16			
		#endregion	
	/*********************************************************************************************************/
		#region 状态标志参数（只读参数）
	/*********************************************************************************************************/
		//逻辑位置
        public const Int16 logicposLoc = 225;			//A		S32

        //当前指令速度
        public const Int16 logicvelLoc = 227;			//A		S16.16

		//当前指令位置
        public const Int16 curposLoc = 59;			//A		S32

		//当前指令速度
		public const Int16 curvelLoc = 		    73;			//A		S16.16

		//标志是否规划运动  0：规划运动已停止，FFFFh：规划运动中（包括主坐标系，以及轮廓运动）
		public const Int16 profilingLoc = 	    215;			//A		F16			
		
		//标志是否正参与轮廓运动，FFFFh：轮廓运动中，0：轮廓运动已结束或CFIFO已空。
		public const Int16 contouringLoc =  	217;			//A		F16			
		
		//标志是否规划运动以及运动平滑处理中，
		//0：停止规划运动且停止平滑处理，FFFFh：规划运动未完成或运动平滑处理进行中。
		public const Int16 movingLoc = 		    218;			//A		F16				
		
		//电机是否静止，0：规划运动已完成，且电机已静止；FFFFh：规划运动未完成，或虽已完成运动规划，但电机尚未静止。
		public const Int16 motionLoc = 		    219;			//A		F16				
		
		//位置误差越出静止窗口标志，若outsettle=FFFFh，表明当前位置误差poserr大于静止窗口参数settlewin。
		public const Int16 outsettleLoc = 	    220;			//A		F16				
		
		//当前位置误差值，指令位置与实际位置（反馈值）之差：poserr=curpos-encp。
		public const Int16 poserrLoc = 		    223;			//A		S16					
		#endregion
	/*********************************************************************************************************/
		#region 指令脉冲相关参数
	/*********************************************************************************************************/
		//脉冲输出模式及信号极性设置寄存器	
		public const Int16 stepmodLoc = 		615;			//A		F16				
		
		//设置方向信号变化的延迟时间，单位是系统的时钟周期
		public const Int16 dirtimeLoc = 		618;			//A		S16				
		
		//设定脉冲有效电平宽度，单位是系统的时钟周期
		public const Int16 steptimeLoc = 		619;			//A		S16				
		#endregion	
	/*********************************************************************************************************/
		#region 探针或index计数相关参数
	/*********************************************************************************************************/
		//探针或index的计数值
		public const Int16 counterLoc = 		541;			//A		S16				
		
		//写操作则清零探针或index的计数值
		public const Int16 clrcounterLoc =  	541;			//A		S16			
		#endregion
	/*********************************************************************************************************/
		#region 轮廓运动相关的参数
	/*********************************************************************************************************/
		//开启轮廓运动
		public const Int16 startgroupLoc =  	256;			//G		F16				
		
		//参与轮廓运动的轴数
		public const Int16 groupnumLoc = 		257;			//G		S16				
		
		//轮廓运动的轴组中，X轴对应的轴号
		public const Int16 group_xLoc = 		258;			//G		S16				
		
		//轮廓运动的轴组中，Y轴对应的轴号
		public const Int16 group_yLoc = 		259;			//G		S16				
		
		//轮廓运动的轴组中，Z轴对应的轴号
		public const Int16 group_zLoc = 		260;			//G		S16				
		
		//轮廓运动的轴组中，A轴对应的轴号
		public const Int16 group_aLoc = 		261;			//G		S16				
		
		//轮廓运动的轴组中，B轴对应的轴号
		public const Int16 group_bLoc = 		262;			//G		S16				
		
		//轮廓运动的轴组中，C轴对应的轴号
		public const Int16 group_cLoc = 		263;			//G		S16				
		
		//轮廓运动的轴组中，D轴对应的轴号
		public const Int16 group_dLoc = 		264;			//G		S16				
		
		//轮廓运动的轴组中，E轴对应的轴号
		public const Int16 group_eLoc = 		265;			//G		S16				
		
		//轮廓运动的轴组中，F轴对应的轴号
		public const Int16 group_fLoc = 		266;			//G		S16				
		
		//轮廓运动的轴组中，G轴对应的轴号
		public const Int16 group_gLoc = 		267;			//G		S16				
		
		//轮廓运动的轴组中，H轴对应的轴号
		public const Int16 group_hLoc = 		268;			//G		S16				
		
		//轮廓运动的轴组中，I轴对应的轴号
		public const Int16 group_iLoc = 		269;			//G		S16				
		
		//轮廓运动的轴组中，J轴对应的轴号
		public const Int16 group_jLoc = 		270;			//G		S16				
		
		//轮廓运动的轴组中，K轴对应的轴号
		public const Int16 group_kLoc = 		271;			//G		S16				
		
		//轮廓运动的轴组中，L轴对应的轴号
		public const Int16 group_lLoc = 		272;			//G		S16				
		
		//轮廓运动的轴组中，M轴对应的轴号
		public const Int16 group_mLoc = 		273;			//G		S16				
		
		//轮廓运动的平滑拟合时间，单位为控制周期
		public const Int16 groupsmoothLoc = 	274;			//G		S16		  

		#endregion
	/*********************************************************************************************************/
		#region 轮廓运动专用CFIFO缓存器的相关参数
	/*********************************************************************************************************/
		//CFIFO中数据（WORD）的个数
		public const Int16 CFIFOcntLoc = 		519;			//G		S16				
		
		//写操作清空CFIFO
		public const Int16 clrCFIFOLoc = 		519;			//G		S16				
		
		#endregion
	/*********************************************************************************************************/
		#region  IFIFO/QFIFO缓存器相关参数
	/*********************************************************************************************************/
		//写操作清空IFIFO
		public const Int16 clrififoLoc = 		513;			//G		S16				
		
		//IFIFO中数据（WORD）的个数
		public const Int16 ififocntLoc = 		513;			//G		S16				
		
		//写操作清空QFIFO
		public const Int16 clrqfifoLoc = 		521;			//G		S16				
		
		//QFIFO中数据（WORD）的个数
		public const Int16 qfifocntLoc = 		521;			//G		S16				
		
		//QFIFO的等待指令的超时时间
		public const Int16 qwaittimeLoc = 	    492;			//G		S32			
		#endregion	
	/*********************************************************************************************************/
		#region 插补运动相关参数
	/*********************************************************************************************************/		
		//插补运动的轴参数
		
		//设置段的坐标数据以绝对值还是相对值表示。 0：段数据表示的是相对值，非零：段数据表示的是绝对值
		public const Int16 pathabsLoc = 		205;			//A		S16				
		
		//当前执行段的终点
		public const Int16 segendLoc = 	    	202;			//A		S32				
		
		//当前执行段的起始点
		public const Int16 segstartLoc = 		200;			//A		S32				
		
		//该轴是否参与插补空间1的插补
		public const Int16 moveinpath1Loc = 	204;			//A		F16			
		
		//该轴是否参与插补空间2的插补
		public const Int16 moveinpath2Loc = 	165;			//A		F16			
		#endregion
	/*********************************************************************************************************/		
		#region 插补空间1的参数
	/*********************************************************************************************************/		
		//写入非零开始执行路径运动
		public const Int16 startpath1Loc =  	352;			//G		F16			
		
		//标志是否正在执行插补
		public const Int16 pathmoving1Loc = 	354;			//G		F16			
		
		//当前执行圆弧段的方向，0：顺时针，非零：逆时针
		public const Int16 arcdir1Loc = 		355;			//G		S16			

		//插补路径规划速度方式
		//指定速度规划是否基于该段合成路径的长度，或某个轴在该段的移动距离。	
		//若asseglen为0，速度规划基于X、Y、Z三轴的合成路径长度，即pathvel是合成路径的速度。
		//当然，当pathaxisnum小于3时，则只有X轴或X、Y轴合成路径长度。
		//若asseglen非零，asseglen必须为1~pathaxisnum范围的一个值，表示采用segmap_x、segmap_y…所映射的轴的
		//移动距离进行速度规划，如1表示采用X轴的移动距离规划速度，因此pathvel即为X轴的速度。
		public const Int16 asseglen1Loc = 	    361;			//G		S16			
		
		//当前路径速度
		public const Int16 pathvel1Loc = 		362;			//G		S16.16				
		
		//路径加速度
		public const Int16 pathacc1Loc = 		366;			//G		S16.16				
		
		//参与路径运动的轴数
		public const Int16 pathaxisnum1Loc = 	370;			//G		S16		 
		
		//映射为X轴的轴号
		public const Int16 segmap_x1Loc =   	371;			//G		S16			
		
		//映射为Y轴的轴号
		public const Int16 segmap_y1Loc =   	372;			//G		S16			
		
		//映射为Z轴的轴号
		public const Int16 segmap_z1Loc = 	    373;			//G		S16			
		
		//映射为A轴的轴号
		public const Int16 segmap_a1Loc =   	374;			//G		S16			
		
		//映射为B轴的轴号
		public const Int16 segmap_b1Loc =   	375;			//G		S16			
		
		//映射为C轴的轴号
		public const Int16 segmap_c1Loc =   	376;			//G		S16			
		
		//映射为D轴的轴号
		public const Int16 segmap_d1Loc =   	377;			//G		S16			
		
		//映射为E轴的轴号
		public const Int16 segmap_e1Loc =   	378;			//G		S16			
		
		//映射为F轴的轴号
		public const Int16 segmap_f1Loc =   	379;			//G		S16			
		
		//映射为G轴的轴号
		public const Int16 segmap_g1Loc =    	380;			//G		S16			
		
		//映射为H轴的轴号
		public const Int16 segmap_h1Loc =   	381;			//G		S16			
		
		//映射为I轴的轴号
		public const Int16 segmap_i1Loc =   	382;			//G		S16			
		
		//映射为J轴的轴号
		public const Int16 segmap_j1Loc =   	383;			//G		S16			
		
		//映射为K轴的轴号
		public const Int16 segmap_k1Loc =   	384;			//G		S16			
		
		//映射为L轴的轴号
		public const Int16 segmap_l1Loc =   	385;			//G		S16			
		
		//映射为M轴的轴号
		public const Int16 segmap_m1Loc =   	386;			//G		S16			
		
		//段的目标运行速度
		public const Int16 segtgvel1Loc = 	    387;			//G		S16.16			
		
		//段的段末速度
		public const Int16 segendvel1Loc =  	389;			//G		S16.16			
		
		//段的ID，用于标识正在执行第几段，每执行一段，该ID加1
		public const Int16 segID1Loc = 		    391;			//G		S32				
		
		//当前执行段的长度
		public const Int16 seglen1Loc = 		393;			//G		S32				
		
		//当前执行圆弧段的半径
		public const Int16 radius1Loc = 		395;			//G		S32				
		#endregion	
	/*********************************************************************************************************/		
		#region PFIFO1缓存器相关参数
	/*********************************************************************************************************/
		//PFIFO1中数据（WORD）个数
		public const Int16 PFIFOcnt1Loc = 	    565;			//G		S16			
		
		//写操作清空PFIFO1
		public const Int16 clrPFIFO1Loc = 	    565;	    	//G		S16			
		
		//PFIFO1等待指令超时时间
		public const Int16 pwaittime1Loc =  	399;			//G		S16			
		#endregion
	/*********************************************************************************************************/		
		#region 插补空间2的参数
	/*********************************************************************************************************/		
		//写入非零开始执行路径运动
		public const Int16 startpath2Loc =  	405;			//G		F16		
		
		//标志是否正在执行插补
		public const Int16 pathmoving2Loc = 	407;			//G		F16			
		
		//当前执行圆弧段的方向，0：顺时针，非零：逆时针
		public const Int16 arcdir2Loc = 		408;			//G		S16				
		
		//插补路径规划速度方式
		//指定速度规划是否基于该段合成路径的长度，或某个轴在该段的移动距离。	
		//若asseglen为0，速度规划基于X、Y、Z三轴的合成路径长度，即pathvel是合成路径的速度。
		//当然，当pathaxisnum小于3时，则只有X轴或X、Y轴合成路径长度。
		//若asseglen非零，asseglen必须为1~pathaxisnum范围的一个值，表示采用segmap_x、segmap_y…所映射的轴的
		//移动距离进行速度规划，如1表示采用X轴的移动距离规划速度，因此pathvel即为X轴的速度。
		public const Int16 asseglen2Loc =   	414;			//G		S16			
		
		//当前路径速度
		public const Int16 pathvel2Loc = 		415;			//G		S16.16				
		
		//路径加速度
		public const Int16 pathacc2Loc = 		419;			//G		S16.16				
		
		//参与路径运动的轴数
		public const Int16 pathaxisnum2Loc = 	423;			//G		S16			
		
		//映射为X轴的轴号
		public const Int16 segmap_x2Loc = 	    424;			//G		S16			
		
		//映射为Y轴的轴号
		public const Int16 segmap_y2Loc = 	    425;			//G		S16			
		
		//映射为Z轴的轴号
		public const Int16 segmap_z2Loc = 	    426;			//G		S16			
		
		//映射为A轴的轴号
		public const Int16 segmap_a2Loc = 	    427;			//G		S16			
		
		//映射为B轴的轴号
		public const Int16 segmap_b2Loc = 	    428;			//G		S16			
		
		//映射为C轴的轴号
		public const Int16 segmap_c2Loc = 	    429;			//G		S16			
		
		//映射为D轴的轴号
		public const Int16 segmap_d2Loc =   	430;			//G		S16			
		
		//映射为E轴的轴号
		public const Int16 segmap_e2Loc =   	431;			//G		S16			
		
		//映射为F轴的轴号
		public const Int16 segmap_f2Loc = 	    432;			//G		S16			
		
		//映射为G轴的轴号
		public const Int16 segmap_g2Loc = 	    433;			//G		S16			
		
		//映射为H轴的轴号
		public const Int16 segmap_h2Loc =   	434;			//G		S16			
		
		//映射为I轴的轴号
		public const Int16 segmap_i2Loc = 	    435;			//G		S16			
		
		//映射为J轴的轴号
		public const Int16 segmap_j2Loc = 	    436;			//G		S16			
		
		//映射为K轴的轴号
		public const Int16 segmap_k2Loc = 	    437;			//G		S16			
		
		//映射为L轴的轴号
		public const Int16 segmap_l2Loc = 	    438;			//G		S16			
		
		//映射为M轴的轴号
		public const Int16 segmap_m2Loc =   	439;			//G		S16			
		
		//段的目标运行速度
		public const Int16 segtgvel2Loc =   	440;			//G		S16.16			
		
		//段的段末速度
		public const Int16 segendvel2Loc =  	442;			//G		S16.16			
		
		//段的ID，用于标识正在执行第几段，每执行一段，该ID加1
		public const Int16 segID2Loc = 		    444;			//G		S32				
		
		//当前执行段的长度
		public const Int16 seglen2Loc = 		446;			//G		S32				
		
		//当前执行圆弧段的半径
		public const Int16 radius2Loc = 		448;			//G		S32				

		#endregion	
	/*********************************************************************************************************/		
		#region PFIFO2缓存器相关参数	 
	/*********************************************************************************************************/		
		//PFIFO2中数据个数（WORD）
		public const Int16 PFIFOcnt2Loc = 	    685;			//G		S16		
		
		//写操作则清零PFIFO2
		public const Int16 clrPFIFO2Loc =   	685;			//G		S16			
		
		//PFIFO2等待指令超时时间
		public const Int16 pwaittime2Loc =  	452;			//G		S16			

		#endregion	
	/*********************************************************************************************************/		
		#region 输入输出（I/O）相关参数
	/*********************************************************************************************************/		
		//脉冲输出及驱动器使能。写入FFFFh则使能脉冲输出及使能驱动器。注意：写入FFFFh，驱动器使能信号为低电平，
		//写入0则为高电平。若需将轴作为虚拟轴(运行正常，但不输出脉冲)，可清零ena。
		//另：可以用于判断脉冲输出和驱动器是否使能，0：脉冲输出和驱动器禁止，FFFFh：脉冲输出和驱动器使能。
		public const Int16 enaLoc = 			550;			//A		F16				
		
		//轴IO电平状态寄存器
		//轴I/O数据寄存器。读出的是对应管脚的实时信号值；
		public const Int16 aioLoc = 			683;			//A		R16				
		
		//轴IO设置寄存器
		public const Int16 aioctrLoc = 	    	680;			//A		R16			
		
		//轴IO锁存寄存器
		public const Int16 aiolatLoc = 		    682;			//A		R16			
		
		//相应位域写入1则清零该位域
		public const Int16 clraiolatLoc = 	    682;			//A		R16		 
		
		//全局开关量输出gout1
		public const Int16 gout1Loc = 		    560;			//G		R16			
		
		//全局开关量输出gout2 
		public const Int16 gout2Loc = 	    	561;			//G		R16			
		
		//全局开关量输出gout3
		public const Int16 gout3Loc = 	    	555;			//G		R16			
		
		//全局开关量输入gin1
		public const Int16 gin1Loc = 			706;			//G		R16				
		
		//全局开关量输入gin2
		public const Int16 gin2Loc = 			707;			//G		R16				
		
		//锁存全局开关量输入gin1的有效边沿
		public const Int16 gin1latLoc = 		612;			//G		R16			
		
		//锁存全局开关量输入gin2的有效边沿
		public const Int16 gin2latLoc = 		613;			//G		R16			
		
		//伺服驱动器误差清零
		public const Int16 srstLoc = 			551;			//A		R16				
		
		//读得到停止开关的状态，写则设置停止开关的有效极性
		public const Int16 stopinLoc = 		    563;			//G		R16			
		
		//输入开关量的数字滤波设置寄存器
		public const Int16 swfilterLoc = 		548;			//G		S16			
		
		//使能srst作为伺服脉冲清除，并设置其脉冲宽度
		public const Int16 srstctrLoc = 		552;			//A		R16		  
			
		#endregion	
	/*********************************************************************************************************/		
		#region 计时单元相关的参数
	/*********************************************************************************************************/		
		//毫秒计时，写入毫秒数，每毫秒减1
		public const Int16 delaymsLoc = 		704;			//G		S32	
		
		//delayms倒计时为0后该参数为0
		public const Int16 delayoutLoc = 		704;			//G		S16			
		
		//倒计时计数器，写入非零开始每周期减1
		public const Int16 timerLoc = 		    481;			//G		S16			
		
		//控制周期计数器，每控制周期加1
		public const Int16 ticksLoc = 		    502;			//G		S32			

		#endregion	
	/*********************************************************************************************************/		
		#region 事件指令相关参数
	/*********************************************************************************************************/		
		//事件指令数量，清零则禁止所有事件指令
		public const Int16 eventsLoc = 		    489;			//G		S16	
		#endregion	

	/*********************************************************************************************************/		
		#region 急停/暂停相关参数
	/*********************************************************************************************************/		
		//某些位域若置位，则使所有轴的error寄存器相应位域置位
		public const Int16 emstopLoc = 	    	500;			//G		R16		
		
		//非零则暂停
		public const Int16 hpauseLoc = 	    	501;			//G		F16		
		#endregion		

	/*********************************************************************************************************/		
		#region  系统参数 Read only
	/*********************************************************************************************************/	
		//控制分频
		public const Int16 clkdivLoc = 	    	509;			//G		S16			
		
		//firmware 版本
		public const Int16 fwversionLoc =   	511;			//G		S16			
		
		//系统基准时钟
		public const Int16 sysclkLoc = 	    	628;			//G		S32				
		
		//该型号iMC支持的轴数
		public const Int16 naxisLoc = 		    634;			//G		S16				
		
		//硬件版本
		public const Int16 hwversionLoc =   	635;			//G		S16			
		
		//清空所有FIFO和残留的等待指令等
		public const Int16 clearimcLoc = 		494;			//G		S16		
		#endregion	

	/*********************************************************************************************************/		
		#region  预定义用户参数
	/*********************************************************************************************************/		
		public const Int16 user16b0Loc = 		307;			//G		S16		  	//16bit的预定义用户参数0
		public const Int16 user16b1Loc = 		308;			//G		S16			//16bit的预定义用户参数1
		public const Int16 user16b2Loc = 		309;			//G		S16			//16bit的预定义用户参数2
		public const Int16 user16b3Loc = 		310;			//G		S16			//16bit的预定义用户参数3
		public const Int16 user16b4Loc = 		311;			//G		S16			//16bit的预定义用户参数4
		public const Int16 user16b5Loc = 		312;			//G		S16			//16bit的预定义用户参数5
		public const Int16 user16b6Loc = 		313;			//G		S16			//16bit的预定义用户参数6
		public const Int16 user16b7Loc = 		314;			//G		S16			//16bit的预定义用户参数7
		public const Int16 user16b8Loc = 		315;			//G		S16			//16bit的预定义用户参数8
		public const Int16 user16b9Loc = 		316;			//G		S16			//16bit的预定义用户参数9

		public const Int16 user32b0Loc = 		317;			//G		S32			//32bit的预定义用户参数0
		public const Int16 user32b1Loc = 		319;			//G		S32			//32bit的预定义用户参数1
		public const Int16 user32b2Loc = 		321;			//G		S32			//32bit的预定义用户参数2
		public const Int16 user32b3Loc = 		323;			//G		S32			//32bit的预定义用户参数3
		public const Int16 user32b4Loc = 		325;			//G		S32			//32bit的预定义用户参数4
		public const Int16 user32b5Loc = 		327;			//G		S32			//32bit的预定义用户参数5
		public const Int16 user32b6Loc = 		329;			//G		S32			//32bit的预定义用户参数6
		public const Int16 user32b7Loc = 		331;			//G		S32			//32bit的预定义用户参数7
		public const Int16 user32b8Loc = 		333;			//G		S32			//32bit的预定义用户参数8
		public const Int16 user32b9Loc = 		335;			//G		S32			//32bit的预定义用户参数9

		public const Int16 user48b0Loc = 		337;			//G		S48			//48bit的预定义用户参数0
		public const Int16 user48b1Loc = 		340;			//G		S48			//48bit的预定义用户参数1

		#endregion

	/*********************************************************************************************************/		
		#region  位置捕获参数
	/*********************************************************************************************************/		
		//写操作则清空位置捕获缓存器capfifo
		public const Int16 clrcapfifoLoc =  	540;			//A		S16		
		
		//capfifo中已压入的位置数据个数
		public const Int16 capfifocntLoc =  	540;			//A		S16			

		public const Int16 compdistLoc = 		542;			//A		S32		
		#endregion
			
	/*********************************************************************************************************/		
		#region  双龙门驱动
	/*********************************************************************************************************/		
		//写入curvel的地址后，使能该轴进入龙门跟随主轴驱动
		public const Int16 gantrymainptrLoc =   188;			//A		S16		
		
		//龙门主轴轴号
		public const Int16 gantrymainaxisLoc =  189;			//A		S16				
		
		//龙门主从误差限值，即若 |从poserr-主poserr|>=gantryerrlim,置位主从error
		public const Int16 gantryerrlimLoc = 	190;			//A		S16			 	
		
		//龙门误差补偿增益kp
		public const Int16 gantrykpLoc = 		191;			//A		S16				
		
		//龙门驱动时，从动轴的错误哪些bit映射到主轴的错误寄存器
		public const Int16 gantryerrormapLoc =  192;			//A		S16				

		#endregion
	/*********************************************************************************************************/		
		#region  tgpos proportion follow
	/*********************************************************************************************************/		
		//跟随的参数
		public const Int16 propfollowptrLoc =   103;			//A		S16		
		
		//跟随的参数所在的轴号
		public const Int16 propfollowaxisLoc =  174;			//A		S16			
		
		//跟随的参数起始值
		public const Int16 propfollowfromLoc =  104;			//A		S32				
		
		//跟随的参数终结值
		public const Int16 propfollowtoLoc =    106;			//A		S32				
		
		//对应于跟随参数起始值时的输出值
		public const Int16 propstartLoc = 	    108;			//A		S32				
		
		//对应于跟随参数终结值时的输出值
		public const Int16 propendLoc = 		110;			//A		S32				
		
		//跟随结果挂钩的参数
		public const Int16 prophookptrLoc = 	172;			//A		S16			
		
		//跟随结果挂钩的参数所在的轴号
		public const Int16 prophookaxisLoc = 	173;			//A		S16			
		
		
		public const Int16 propshiftLoc = 	    178;			//A		S32		
		
		#endregion
		
	/*********************************************************************************************************/		
		#region  DA/PWM output proportion follow
	/*********************************************************************************************************/	
		//跟随的参数
		public const Int16 PWMfollowptrLoc = 	155;			//A		S16		
		
		//跟随的参数所在的轴号
		public const Int16 PWMfollowaxisLoc =   156;			//A		S16			
		
		//跟随的参数
		public const Int16 DAfollowptrLoc = 	143;			//A		S16				
		
		//跟随的参数所在的轴号
		public const Int16 DAfollowaxisLoc = 	144;			//A		S16			
		
		//跟随的参数起始值
		public const Int16 DAfollowfromLoc = 	112;			//A		S32				
		
		//跟随的参数终结值
		public const Int16 DAfollowtoLoc = 	    114;			//A		S32				
		
		//对应于跟随参数起始值时的pwm值(频率或占空比)
		public const Int16 DAstartLoc = 		116;			//A		S32				
		
		//对应于跟随参数终结值时的pwm值
		public const Int16 DAendLoc = 		    118;			//A		S32				
		
		//DA输出偏移值
		public const Int16 DAshiftLoc = 		216;			//A		S16				
		
		//PWM输出偏移值
		public const Int16 PWMshiftLoc = 		221;			//A		S32				
		
		//DA使能
		public const Int16 DAenaLoc = 		    700;			//A		S16				
		
		//PWM使能
		public const Int16 PWMctrLoc = 		    702;			//A		S16				
		
		//设定PWM的频率,pwmfreq = (f/1000)*65536，f为pwm的实际频率（脉冲/秒）。
		public const Int16 PWMfreqLoc = 		708;			//A		S16.16		

		//PWM输出的占空比
		public const Int16 PWMpropLoc = 		129;			//A		S16	

        #endregion
		
	/*********************************************************************************************************/		
		#region Fix proportion follow
	/*********************************************************************************************************/		
		//线性误差补偿跟随的参数
		public const Int16 fixfollowptrLoc = 	166;			//A		S16		
		
		//线性误差补偿跟随的参数所在的轴号
		public const Int16 fixfollowaxisLoc = 167;			//A		S16			
		
		//线性误差补偿跟随的参数起始值
		public const Int16 fixfollowfromLoc = 120;			//A		S32				
		
		//跟随的参数终结值
		public const Int16 fixfollowtoLoc = 	122;			//A		S32				
		
		//对应于跟随参数起始值时的补偿值
		public const Int16 fixstartLoc = 		124;			//A		S32				
		
		//对应于跟随参数终结值时的补偿值
		public const Int16 fixendLoc = 		126;			//A		S32				
		
        #endregion
		
	/*********************************************************************************************************/		
		#region AD采样
	/*********************************************************************************************************/		
		//AD通道使能，bit0~bit7中的bit为1,对应的通道使能
		public const Int16 ADchannelLoc = 	    614;			//G		S16			
		
		//16bit的AD数据，只读
		public const Int16 ADdataLoc = 		    696;			//A		S16			
		
		//32bit的AD数据，用于跟随等
		public const Int16 ADdata32Loc = 		696;			//A		S32			
				
        #endregion
			
		
	/*********************************************************************************************************/		
		#region 设置通信监测
		public const Int16 comdogLoc = 		    703;			//G		S16							

        #endregion
		//全局进给倍率	——需在内存中预设为 000010000h
		public const Int16 feedrateLoc = 		350;			//G		S16.16
	


    }

}