using System;


namespace imcpkg
{

//***********************************************************************************************************/
//       ��ͷ�ļ�ΪiMC������ַ�ĺ궨��                                                                      */
//       ��ʽ��                                                                                             */
//       //����                                                                                             */
//       public const Int16   ������Loc =      ��ַ     // ���ȫ�ֲ���  ���ݸ�ʽ                           */
//                                                                                                          */
//       ���С�����������ָ��iMC�Ĳ�����������Loc��ʾ�ò����ĵ�ַ                                         */
//                                                                                                          */
//       ��ĸA��ʾ���������ĸG��ʾȫ�ֲ���                                                                 */
//                                                                                                          */
//       iMC�й������¼������ݸ�ʽ�����ͣ�                                                                  */
//       S32��32λ������λ����������С�����ֵ�λ��Ϊ0������ֵ�ķ�ΧΪ��[-2147483648,2147483647]��           */
//       U32��32λ�޷���λ����������С�����ֵ�λ��Ϊ0������ֵ�ķ�ΧΪ��[0, 4294967295]��                    */
//       S16��16λ������λ����������С�����ֵ�λ��Ϊ0������ֵ�ķ�ΧΪ��[-32768,32767]��                     */
//       U16��16λ�޷���λ����������С�����ֵ�λ��Ϊ0������ֵ�ķ�ΧΪ��[0, 65535]��                         */
//       F16��16λ��־��������ȡ����ֵ��0��FFFFh��                                                          */
//       R16��16λ�Ĵ�������λ����о�������壬����λ�������á�                                            */
//       S16.32��48λ������λ����16λΪ�������֣���32λΪС�����֣�����ֵ�ķ�Χ��[-32768.0,32767.999999999767] */
//       U16.32��48λ�޷���λ����16λΪ�������֣���32λΪС�����֣�����ֵ�ķ�Χ��[0.0,65535.999999999767]   */
//       S16.16��32λ������λ����16λΪ�������֣���16λΪС�����֣�����ֵ�ķ�Χ��[-32768.0,32767.999984741211]  */
//       U16.16��32λ�޷���λ����16λΪ�������֣���32λΪС�����֣�����ֵ�ķ�Χ��[0.0,65535.999984741211]�� */
//       S0.16��16λ�з���λ��16λΪС�����֡�                                                              */
//       ��ˣ�iMC�еĲ���ֵ����ʾ��ʵ��ֵΪ��                                                              */
//       ʵ��ֵ=����ֵ/2^n                                                                                  */
//       ����nΪС�����ֵ�λ����                                                                            */
//       ���磬����һ��S32��ʽ��������00018000h����h��ʾʮ�����Ʊ�ʾ����ʾ��ʮ���Ƶ�ֵΪ98304/2^0 = 98304�� */
//       ����һ��S16.16��ʽ��������00018000h����ʾ��ʮ���Ƶ�ֵΪ98304/2^16 = 1.5��                          */
//***********************************************************************************************************/

    class ParamDef
    {
	/*********************************************************************************************************/
		#region �㵽���˶�����
	/*********************************************************************************************************/
		//�����ٶ��˶���Ŀ���ٶȣ�
		public const Int16 mcstgvelLoc = 		6;			//A		S16.16
		
		//������ϵ�Ƿ����ٶ�б�������У����ٻ���٣���FFFFh�������ٶ�б�����̣�0������Ŀ���ٶ����С�
		//�����ӳ����˶�ʱ�������жϴӶ����Ƿ��Ѵﵽ�����ٶȣ����Ƿ�ﵽ�������ٶȳ��Դ������ʵ�ֵ��0���ﵽ��FFFFh��δ�ﵽ
		public const Int16 mcsslopeLoc = 		8;			//A		F16

		//�㵽���˶���ָ��λ�ã�Ŀ��λ�ã�
		public const Int16 mcstgposLoc = 		12;		//A		S32	
		
		//�㵽���˶�����ʼ�ٶ�
		public const Int16 mcsstartvelLoc = 	14;			//A		S16.16	
		
		//�㵽���˶������滮�ٶ�
		public const Int16 mcsmaxvelLoc = 	    16;			//A		S16.16	
		
		//�㵽���˶��ļ��ٶ�
		public const Int16 mcsaccelLoc = 		18;			//A		S16.16
			
		//�㵽���˶��ļ��ٶ�
		public const Int16 mcsdecelLoc = 		20;			//A		S16.16 
		
		//�㵽���˶���ģʽ��0����ͨģʽ��FFFFh������ģʽ
		public const Int16 mcstrackLoc = 		22;			//A		F16			
		
		//д����������㵽���˶���д���㣬ֹͣ��ǰ�ĵ㵽���˶�
		public const Int16 mcsgoLoc = 		    23;			//A		F16	
		
		//�㵽���˶����յ��ٶ�
		public const Int16 mcsendvelLoc = 	    26;			//A	
		
		//Ŀ���ƶ����룬д���mcsdis���벻Ϊ0
		public const Int16 mcsdistLoc = 		56;			//A		S32	

		#endregion
	/*********************************************************************************************************/
		#region ����ʽ�㵽���˶�
	/*********************************************************************************************************/
		//����ʽ�㵽���˶��������ٶȵ���Դ���
		public const Int16 incmoveaxisLoc = 	194;			//A		S16				
		
		 //�����㣬ʹ��ĳ�ٶ�ֵ�����˶�����д��encsvel��ַʱ�Ǹ�����������˶�
		public const Int16 incmoveptrLoc = 	    195;			//A		F16			
		
		//�����˶�����
		public const Int16 incmoverateLoc = 	196;			//A		S32			 

		#endregion
	/*********************************************************************************************************/
		#region �����϶�����Ĳ���
	/*********************************************************************************************************/
		//��϶���������Բ�����ʼ�ٶȡ�
		public const Int16 cpstartvelLoc = 	    41;			//A		S16.16
		
		//��϶���������Բ�������ٶȡ�
		public const Int16 cpmaxvelLoc = 		43;			//A		S16.16

		//��϶���������Բ������ٶȡ�
		public const Int16 cpaccelLoc = 		45;			//A		S16.16

		//��϶���������Բ������ٶȡ�
		public const Int16 cpdecelLoc = 		47;			//A		S16.16

		//����ʹ�ܡ�д�����ֵ��ʹ�ܼ�϶���������Բ������������ֹ��
		public const Int16 enacompenLoc = 	    49;			//A		F16

		//��϶���������Բ�����ֹ�ٶȡ�
		public const Int16 cpendvelLoc = 		53;			//A		S16.16
		
		//�����϶����С����λ�����塣
		public const Int16 backlashLoc = 		229;			//A		S32
		
		#endregion


	/*********************************************************************************************************/
		#region ����������ز���
	/*********************************************************************************************************/
		//��ǰ���������ٶ�
		public const Int16 encpvelLoc = 		75;			//A		S16.16 				
		
		//���������ļ����Ĵ���
		public const Int16 encpLoc = 			78;			//A		S32						
		
		//�����������ƼĴ���
		public const Int16 encpctrLoc = 		539;			//A					

		#endregion
	/*********************************************************************************************************/
		#region ����������ز���
	/*********************************************************************************************************/
		//���������ı�������
		public const Int16 encsfactorLoc = 	    343;			//G		S16.16 			
		
		//��ǰ���������ٶ�
		public const Int16 encsvelLoc = 		345;			//G		S16.16 				
		
		//���������ļ����Ĵ���
		public const Int16 encsLoc = 			348;			//G		S32						
		
		//�����������Ŀ��ƼĴ���
		public const Int16 encsctrLoc = 		531;			//G		R16					
		
		//д��������encs
		public const Int16 clrencsLoc = 		532;			//G		S16					
		
		#endregion
	/*********************************************************************************************************/
		#region ��ԭ��
	/*********************************************************************************************************/
		//����ԭ��ʱ����ԭ��ķ���(��϶��������,0:��������ʱ������FFFFh:������ʱ����)
		public const Int16 homedirLoc = 		235;			//A		
		
		//ԭ���ƫ��λ�ã����û�еԭ��ʱ����ֵ��������ָ��λ�üĴ���curpos�ͱ������Ĵ���encp�С�
		public const Int16 homeposLoc = 		145;			//A		S32				

		//��Ѱԭ��ʱ���ٶε��ٶ�
		public const Int16 lowvelLoc = 		    147;			//A		S16.16 			

		//��Ѱԭ��ʱ���ٶε��ٶ�
		public const Int16 highvelLoc = 		149;			//A		S16.16 			
		
		//��Ѱԭ��Ĺ��̺ͷ�ʽ���ƼĴ���
		public const Int16 homeseqLoc = 		151;			//A		R16					
		
		//������ʼִ����Ѱԭ�㣬������ֹͣ
		public const Int16 gohomeLoc = 		    152;			//A		F16					
		
		//����д�����ֵ���赱ǰλ��Ϊԭ��
		public const Int16 sethomeLoc = 		153;			//A		F16					
		
		//�����ʾ����Ѱ��ԭ��
		public const Int16 homedLoc = 		    154;			//A		F16						
		
		//��¼����ʱ�߹��ľ���
		public const Int16 homemovedistLoc = 	163;			//A		S32				

		#endregion
		
	/*********************************************************************************************************/
		#region ���������	
	/*********************************************************************************************************/

		//д��������и��ᣬ0��ֹͣ���и���
		public const Int16 runLoc = 			128;			//A		F16						
		
		//����Ĵ���
		public const Int16 errorLoc = 		    130;			//A		R16						
		
		//�����������λ�����
		public const Int16 poserrlimLoc = 	    131;			//A		S16					
		
		//ƽ������
		public const Int16 smoothLoc = 		    132;			//A		S16					
		
		//������ͣ��
		public const Int16 axispauseatonceLoc = 133;			//A		F16		
		
		//��ֹ����
		public const Int16 settlewinLoc = 	    134;			//A		S16					
		
		//��ֹͣ�˶�����ֹ��ʱ�䣨���ڸ�����
		public const Int16 settlenLoc = 		135;		//A		S16					
		
		//����ʱ��ֹͣ���˼Ĵ���
		public const Int16 stopfiltLoc = 		136;			//A		R16					
		
		//����ʱ������ͣ����
		public const Int16 stopmodeLoc = 		137;			//A		R16		
		
		//����ʱ���˳����й��˼Ĵ���
		public const Int16 exitfiltLoc = 		138;			//A		R16				
		
		//��������λ��
		public const Int16 psoftlimLoc = 		139;			//A		S32					
		
		//��������λ��
		public const Int16 nsoftlimLoc = 		141;			//A		S32		
		
		//д�����ֵ�Ը���ʵʩ����������������ı�����������ָ��λ�á�Ŀ��λ�õȸ���λ�ò�����
		//	�Լ������˶�״̬��־��mcspos��mcstgpos��curpos��encp��pcspos��pcstgpos��status��
		//	error��emstop��hpause��events��encs��ticks��aiolat��
		public const Int16 clearLoc = 		    157;			//A		S16		
		
		//���ø��������ٶ����ơ����ۺ����˶�ģʽ��ֻҪʵ���ٶȳ����˼���ֵ������λ����Ĵ���error�е�λVELLIM��
		//	�˴��󲻿����Σ����ֻҪ�����˴��������˳��������С�ע�⣺����Ϊ��ֵ��
		public const Int16 vellimLoc = 		    158;			//A		S16.16 			
			
		//���ø���������ٶ����ơ����ۺ����˶�ģʽ��ֻҪʵ�ʼ��ٶȳ����˼���ֵ������λ����Ĵ���error�е�ACCLIMλ��
		//	�˴��󲻿����Σ����ֻҪ�����˴��������˳��������С�ע�⣺accellim����Ϊ��ֵ��
		public const Int16 accellimLoc = 		160;			//A		S16.16 			
		
		//��ֹ�����ٶ�
		public const Int16 fixvelLoc = 		    162;			//A		S0.16 				
			
		#endregion	
	/*********************************************************************************************************/
		#region ���ӳ�����صĲ���
	/*********************************************************************************************************/
		//���ӳ����˶�ģʽ������������
		public const Int16 masterLoc = 		    169;			//A		S16				
		
		//ָ�룬ָ��Ӷ����������������Ĳ���
		public const Int16 gearsrcLoc = 		170;			//A		S16					
		
		//д�����ֵ��ʼ�Ӻϣ���������������
		public const Int16 engearLoc = 	    	171;			//A		F16				

		//��������  
		public const Int16 gearratioLoc =   	175;		//A		S16.32			
		#endregion	
	/*********************************************************************************************************/
		#region ��������ز���
	/*********************************************************************************************************/
		//���û���������λ��  
		public const Int16 cirposLoc = 		    184;			//A		S32			
		
		//���ø���Ϊ����������ᡣ��ciraxisΪ0������Ϊ�����᣻��ΪFFFFh����������Ϊ�����ᡣ
		public const Int16 ciraxisLoc = 		186;			//A		S16			
		
		//�����˫���α�־��
		//��Ϊ0��Ϊ�����Σ�λ�÷�ΧΪ[0,cirpos)����Ϊ���㣬Ϊ˫���Σ�λ�÷�ΧΪ(-cirpos,cirpos)��
		public const Int16 biciraxisLoc = 	    187;			//A		S16		 
		
		//��¼ѭ�����������������1�����������1
		public const Int16 cirswapLoc = 		214;			//A		S16			
		#endregion	
	/*********************************************************************************************************/
		#region ״̬��־������ֻ��������
	/*********************************************************************************************************/
		//�߼�λ��
        public const Int16 logicposLoc = 225;			//A		S32

        //��ǰָ���ٶ�
        public const Int16 logicvelLoc = 227;			//A		S16.16

		//��ǰָ��λ��
        public const Int16 curposLoc = 59;			//A		S32

		//��ǰָ���ٶ�
		public const Int16 curvelLoc = 		    73;			//A		S16.16

		//��־�Ƿ�滮�˶�  0���滮�˶���ֹͣ��FFFFh���滮�˶��У�����������ϵ���Լ������˶���
		public const Int16 profilingLoc = 	    215;			//A		F16			
		
		//��־�Ƿ������������˶���FFFFh�������˶��У�0�������˶��ѽ�����CFIFO�ѿա�
		public const Int16 contouringLoc =  	217;			//A		F16			
		
		//��־�Ƿ�滮�˶��Լ��˶�ƽ�������У�
		//0��ֹͣ�滮�˶���ֹͣƽ������FFFFh���滮�˶�δ��ɻ��˶�ƽ����������С�
		public const Int16 movingLoc = 		    218;			//A		F16				
		
		//����Ƿ�ֹ��0���滮�˶�����ɣ��ҵ���Ѿ�ֹ��FFFFh���滮�˶�δ��ɣ�����������˶��滮���������δ��ֹ��
		public const Int16 motionLoc = 		    219;			//A		F16				
		
		//λ�����Խ����ֹ���ڱ�־����outsettle=FFFFh��������ǰλ�����poserr���ھ�ֹ���ڲ���settlewin��
		public const Int16 outsettleLoc = 	    220;			//A		F16				
		
		//��ǰλ�����ֵ��ָ��λ����ʵ��λ�ã�����ֵ��֮�poserr=curpos-encp��
		public const Int16 poserrLoc = 		    223;			//A		S16					
		#endregion
	/*********************************************************************************************************/
		#region ָ��������ز���
	/*********************************************************************************************************/
		//�������ģʽ���źż������üĴ���	
		public const Int16 stepmodLoc = 		615;			//A		F16				
		
		//���÷����źű仯���ӳ�ʱ�䣬��λ��ϵͳ��ʱ������
		public const Int16 dirtimeLoc = 		618;			//A		S16				
		
		//�趨������Ч��ƽ��ȣ���λ��ϵͳ��ʱ������
		public const Int16 steptimeLoc = 		619;			//A		S16				
		#endregion	
	/*********************************************************************************************************/
		#region ̽���index������ز���
	/*********************************************************************************************************/
		//̽���index�ļ���ֵ
		public const Int16 counterLoc = 		541;			//A		S16				
		
		//д����������̽���index�ļ���ֵ
		public const Int16 clrcounterLoc =  	541;			//A		S16			
		#endregion
	/*********************************************************************************************************/
		#region �����˶���صĲ���
	/*********************************************************************************************************/
		//���������˶�
		public const Int16 startgroupLoc =  	256;			//G		F16				
		
		//���������˶�������
		public const Int16 groupnumLoc = 		257;			//G		S16				
		
		//�����˶��������У�X���Ӧ�����
		public const Int16 group_xLoc = 		258;			//G		S16				
		
		//�����˶��������У�Y���Ӧ�����
		public const Int16 group_yLoc = 		259;			//G		S16				
		
		//�����˶��������У�Z���Ӧ�����
		public const Int16 group_zLoc = 		260;			//G		S16				
		
		//�����˶��������У�A���Ӧ�����
		public const Int16 group_aLoc = 		261;			//G		S16				
		
		//�����˶��������У�B���Ӧ�����
		public const Int16 group_bLoc = 		262;			//G		S16				
		
		//�����˶��������У�C���Ӧ�����
		public const Int16 group_cLoc = 		263;			//G		S16				
		
		//�����˶��������У�D���Ӧ�����
		public const Int16 group_dLoc = 		264;			//G		S16				
		
		//�����˶��������У�E���Ӧ�����
		public const Int16 group_eLoc = 		265;			//G		S16				
		
		//�����˶��������У�F���Ӧ�����
		public const Int16 group_fLoc = 		266;			//G		S16				
		
		//�����˶��������У�G���Ӧ�����
		public const Int16 group_gLoc = 		267;			//G		S16				
		
		//�����˶��������У�H���Ӧ�����
		public const Int16 group_hLoc = 		268;			//G		S16				
		
		//�����˶��������У�I���Ӧ�����
		public const Int16 group_iLoc = 		269;			//G		S16				
		
		//�����˶��������У�J���Ӧ�����
		public const Int16 group_jLoc = 		270;			//G		S16				
		
		//�����˶��������У�K���Ӧ�����
		public const Int16 group_kLoc = 		271;			//G		S16				
		
		//�����˶��������У�L���Ӧ�����
		public const Int16 group_lLoc = 		272;			//G		S16				
		
		//�����˶��������У�M���Ӧ�����
		public const Int16 group_mLoc = 		273;			//G		S16				
		
		//�����˶���ƽ�����ʱ�䣬��λΪ��������
		public const Int16 groupsmoothLoc = 	274;			//G		S16		  

		#endregion
	/*********************************************************************************************************/
		#region �����˶�ר��CFIFO����������ز���
	/*********************************************************************************************************/
		//CFIFO�����ݣ�WORD���ĸ���
		public const Int16 CFIFOcntLoc = 		519;			//G		S16				
		
		//д�������CFIFO
		public const Int16 clrCFIFOLoc = 		519;			//G		S16				
		
		#endregion
	/*********************************************************************************************************/
		#region  IFIFO/QFIFO��������ز���
	/*********************************************************************************************************/
		//д�������IFIFO
		public const Int16 clrififoLoc = 		513;			//G		S16				
		
		//IFIFO�����ݣ�WORD���ĸ���
		public const Int16 ififocntLoc = 		513;			//G		S16				
		
		//д�������QFIFO
		public const Int16 clrqfifoLoc = 		521;			//G		S16				
		
		//QFIFO�����ݣ�WORD���ĸ���
		public const Int16 qfifocntLoc = 		521;			//G		S16				
		
		//QFIFO�ĵȴ�ָ��ĳ�ʱʱ��
		public const Int16 qwaittimeLoc = 	    492;			//G		S32			
		#endregion	
	/*********************************************************************************************************/
		#region �岹�˶���ز���
	/*********************************************************************************************************/		
		//�岹�˶��������
		
		//���öε����������Ծ���ֵ�������ֵ��ʾ�� 0�������ݱ�ʾ�������ֵ�����㣺�����ݱ�ʾ���Ǿ���ֵ
		public const Int16 pathabsLoc = 		205;			//A		S16				
		
		//��ǰִ�жε��յ�
		public const Int16 segendLoc = 	    	202;			//A		S32				
		
		//��ǰִ�жε���ʼ��
		public const Int16 segstartLoc = 		200;			//A		S32				
		
		//�����Ƿ����岹�ռ�1�Ĳ岹
		public const Int16 moveinpath1Loc = 	204;			//A		F16			
		
		//�����Ƿ����岹�ռ�2�Ĳ岹
		public const Int16 moveinpath2Loc = 	165;			//A		F16			
		#endregion
	/*********************************************************************************************************/		
		#region �岹�ռ�1�Ĳ���
	/*********************************************************************************************************/		
		//д����㿪ʼִ��·���˶�
		public const Int16 startpath1Loc =  	352;			//G		F16			
		
		//��־�Ƿ�����ִ�в岹
		public const Int16 pathmoving1Loc = 	354;			//G		F16			
		
		//��ǰִ��Բ���εķ���0��˳ʱ�룬���㣺��ʱ��
		public const Int16 arcdir1Loc = 		355;			//G		S16			

		//�岹·���滮�ٶȷ�ʽ
		//ָ���ٶȹ滮�Ƿ���ڸöκϳ�·���ĳ��ȣ���ĳ�����ڸöε��ƶ����롣	
		//��asseglenΪ0���ٶȹ滮����X��Y��Z����ĺϳ�·�����ȣ���pathvel�Ǻϳ�·�����ٶȡ�
		//��Ȼ����pathaxisnumС��3ʱ����ֻ��X���X��Y��ϳ�·�����ȡ�
		//��asseglen���㣬asseglen����Ϊ1~pathaxisnum��Χ��һ��ֵ����ʾ����segmap_x��segmap_y����ӳ������
		//�ƶ���������ٶȹ滮����1��ʾ����X����ƶ�����滮�ٶȣ����pathvel��ΪX����ٶȡ�
		public const Int16 asseglen1Loc = 	    361;			//G		S16			
		
		//��ǰ·���ٶ�
		public const Int16 pathvel1Loc = 		362;			//G		S16.16				
		
		//·�����ٶ�
		public const Int16 pathacc1Loc = 		366;			//G		S16.16				
		
		//����·���˶�������
		public const Int16 pathaxisnum1Loc = 	370;			//G		S16		 
		
		//ӳ��ΪX������
		public const Int16 segmap_x1Loc =   	371;			//G		S16			
		
		//ӳ��ΪY������
		public const Int16 segmap_y1Loc =   	372;			//G		S16			
		
		//ӳ��ΪZ������
		public const Int16 segmap_z1Loc = 	    373;			//G		S16			
		
		//ӳ��ΪA������
		public const Int16 segmap_a1Loc =   	374;			//G		S16			
		
		//ӳ��ΪB������
		public const Int16 segmap_b1Loc =   	375;			//G		S16			
		
		//ӳ��ΪC������
		public const Int16 segmap_c1Loc =   	376;			//G		S16			
		
		//ӳ��ΪD������
		public const Int16 segmap_d1Loc =   	377;			//G		S16			
		
		//ӳ��ΪE������
		public const Int16 segmap_e1Loc =   	378;			//G		S16			
		
		//ӳ��ΪF������
		public const Int16 segmap_f1Loc =   	379;			//G		S16			
		
		//ӳ��ΪG������
		public const Int16 segmap_g1Loc =    	380;			//G		S16			
		
		//ӳ��ΪH������
		public const Int16 segmap_h1Loc =   	381;			//G		S16			
		
		//ӳ��ΪI������
		public const Int16 segmap_i1Loc =   	382;			//G		S16			
		
		//ӳ��ΪJ������
		public const Int16 segmap_j1Loc =   	383;			//G		S16			
		
		//ӳ��ΪK������
		public const Int16 segmap_k1Loc =   	384;			//G		S16			
		
		//ӳ��ΪL������
		public const Int16 segmap_l1Loc =   	385;			//G		S16			
		
		//ӳ��ΪM������
		public const Int16 segmap_m1Loc =   	386;			//G		S16			
		
		//�ε�Ŀ�������ٶ�
		public const Int16 segtgvel1Loc = 	    387;			//G		S16.16			
		
		//�εĶ�ĩ�ٶ�
		public const Int16 segendvel1Loc =  	389;			//G		S16.16			
		
		//�ε�ID�����ڱ�ʶ����ִ�еڼ��Σ�ÿִ��һ�Σ���ID��1
		public const Int16 segID1Loc = 		    391;			//G		S32				
		
		//��ǰִ�жεĳ���
		public const Int16 seglen1Loc = 		393;			//G		S32				
		
		//��ǰִ��Բ���εİ뾶
		public const Int16 radius1Loc = 		395;			//G		S32				
		#endregion	
	/*********************************************************************************************************/		
		#region PFIFO1��������ز���
	/*********************************************************************************************************/
		//PFIFO1�����ݣ�WORD������
		public const Int16 PFIFOcnt1Loc = 	    565;			//G		S16			
		
		//д�������PFIFO1
		public const Int16 clrPFIFO1Loc = 	    565;	    	//G		S16			
		
		//PFIFO1�ȴ�ָ�ʱʱ��
		public const Int16 pwaittime1Loc =  	399;			//G		S16			
		#endregion
	/*********************************************************************************************************/		
		#region �岹�ռ�2�Ĳ���
	/*********************************************************************************************************/		
		//д����㿪ʼִ��·���˶�
		public const Int16 startpath2Loc =  	405;			//G		F16		
		
		//��־�Ƿ�����ִ�в岹
		public const Int16 pathmoving2Loc = 	407;			//G		F16			
		
		//��ǰִ��Բ���εķ���0��˳ʱ�룬���㣺��ʱ��
		public const Int16 arcdir2Loc = 		408;			//G		S16				
		
		//�岹·���滮�ٶȷ�ʽ
		//ָ���ٶȹ滮�Ƿ���ڸöκϳ�·���ĳ��ȣ���ĳ�����ڸöε��ƶ����롣	
		//��asseglenΪ0���ٶȹ滮����X��Y��Z����ĺϳ�·�����ȣ���pathvel�Ǻϳ�·�����ٶȡ�
		//��Ȼ����pathaxisnumС��3ʱ����ֻ��X���X��Y��ϳ�·�����ȡ�
		//��asseglen���㣬asseglen����Ϊ1~pathaxisnum��Χ��һ��ֵ����ʾ����segmap_x��segmap_y����ӳ������
		//�ƶ���������ٶȹ滮����1��ʾ����X����ƶ�����滮�ٶȣ����pathvel��ΪX����ٶȡ�
		public const Int16 asseglen2Loc =   	414;			//G		S16			
		
		//��ǰ·���ٶ�
		public const Int16 pathvel2Loc = 		415;			//G		S16.16				
		
		//·�����ٶ�
		public const Int16 pathacc2Loc = 		419;			//G		S16.16				
		
		//����·���˶�������
		public const Int16 pathaxisnum2Loc = 	423;			//G		S16			
		
		//ӳ��ΪX������
		public const Int16 segmap_x2Loc = 	    424;			//G		S16			
		
		//ӳ��ΪY������
		public const Int16 segmap_y2Loc = 	    425;			//G		S16			
		
		//ӳ��ΪZ������
		public const Int16 segmap_z2Loc = 	    426;			//G		S16			
		
		//ӳ��ΪA������
		public const Int16 segmap_a2Loc = 	    427;			//G		S16			
		
		//ӳ��ΪB������
		public const Int16 segmap_b2Loc = 	    428;			//G		S16			
		
		//ӳ��ΪC������
		public const Int16 segmap_c2Loc = 	    429;			//G		S16			
		
		//ӳ��ΪD������
		public const Int16 segmap_d2Loc =   	430;			//G		S16			
		
		//ӳ��ΪE������
		public const Int16 segmap_e2Loc =   	431;			//G		S16			
		
		//ӳ��ΪF������
		public const Int16 segmap_f2Loc = 	    432;			//G		S16			
		
		//ӳ��ΪG������
		public const Int16 segmap_g2Loc = 	    433;			//G		S16			
		
		//ӳ��ΪH������
		public const Int16 segmap_h2Loc =   	434;			//G		S16			
		
		//ӳ��ΪI������
		public const Int16 segmap_i2Loc = 	    435;			//G		S16			
		
		//ӳ��ΪJ������
		public const Int16 segmap_j2Loc = 	    436;			//G		S16			
		
		//ӳ��ΪK������
		public const Int16 segmap_k2Loc = 	    437;			//G		S16			
		
		//ӳ��ΪL������
		public const Int16 segmap_l2Loc = 	    438;			//G		S16			
		
		//ӳ��ΪM������
		public const Int16 segmap_m2Loc =   	439;			//G		S16			
		
		//�ε�Ŀ�������ٶ�
		public const Int16 segtgvel2Loc =   	440;			//G		S16.16			
		
		//�εĶ�ĩ�ٶ�
		public const Int16 segendvel2Loc =  	442;			//G		S16.16			
		
		//�ε�ID�����ڱ�ʶ����ִ�еڼ��Σ�ÿִ��һ�Σ���ID��1
		public const Int16 segID2Loc = 		    444;			//G		S32				
		
		//��ǰִ�жεĳ���
		public const Int16 seglen2Loc = 		446;			//G		S32				
		
		//��ǰִ��Բ���εİ뾶
		public const Int16 radius2Loc = 		448;			//G		S32				

		#endregion	
	/*********************************************************************************************************/		
		#region PFIFO2��������ز���	 
	/*********************************************************************************************************/		
		//PFIFO2�����ݸ�����WORD��
		public const Int16 PFIFOcnt2Loc = 	    685;			//G		S16		
		
		//д����������PFIFO2
		public const Int16 clrPFIFO2Loc =   	685;			//G		S16			
		
		//PFIFO2�ȴ�ָ�ʱʱ��
		public const Int16 pwaittime2Loc =  	452;			//G		S16			

		#endregion	
	/*********************************************************************************************************/		
		#region ���������I/O����ز���
	/*********************************************************************************************************/		
		//���������������ʹ�ܡ�д��FFFFh��ʹ�����������ʹ����������ע�⣺д��FFFFh��������ʹ���ź�Ϊ�͵�ƽ��
		//д��0��Ϊ�ߵ�ƽ�����轫����Ϊ������(���������������������)��������ena��
		//�����������ж�����������������Ƿ�ʹ�ܣ�0�������������������ֹ��FFFFh�����������������ʹ�ܡ�
		public const Int16 enaLoc = 			550;			//A		F16				
		
		//��IO��ƽ״̬�Ĵ���
		//��I/O���ݼĴ������������Ƕ�Ӧ�ܽŵ�ʵʱ�ź�ֵ��
		public const Int16 aioLoc = 			683;			//A		R16				
		
		//��IO���üĴ���
		public const Int16 aioctrLoc = 	    	680;			//A		R16			
		
		//��IO����Ĵ���
		public const Int16 aiolatLoc = 		    682;			//A		R16			
		
		//��Ӧλ��д��1�������λ��
		public const Int16 clraiolatLoc = 	    682;			//A		R16		 
		
		//ȫ�ֿ��������gout1
		public const Int16 gout1Loc = 		    560;			//G		R16			
		
		//ȫ�ֿ��������gout2 
		public const Int16 gout2Loc = 	    	561;			//G		R16			
		
		//ȫ�ֿ��������gout3
		public const Int16 gout3Loc = 	    	555;			//G		R16			
		
		//ȫ�ֿ���������gin1
		public const Int16 gin1Loc = 			706;			//G		R16				
		
		//ȫ�ֿ���������gin2
		public const Int16 gin2Loc = 			707;			//G		R16				
		
		//����ȫ�ֿ���������gin1����Ч����
		public const Int16 gin1latLoc = 		612;			//G		R16			
		
		//����ȫ�ֿ���������gin2����Ч����
		public const Int16 gin2latLoc = 		613;			//G		R16			
		
		//�ŷ��������������
		public const Int16 srstLoc = 			551;			//A		R16				
		
		//���õ�ֹͣ���ص�״̬��д������ֹͣ���ص���Ч����
		public const Int16 stopinLoc = 		    563;			//G		R16			
		
		//���뿪�����������˲����üĴ���
		public const Int16 swfilterLoc = 		548;			//G		S16			
		
		//ʹ��srst��Ϊ�ŷ������������������������
		public const Int16 srstctrLoc = 		552;			//A		R16		  
			
		#endregion	
	/*********************************************************************************************************/		
		#region ��ʱ��Ԫ��صĲ���
	/*********************************************************************************************************/		
		//�����ʱ��д���������ÿ�����1
		public const Int16 delaymsLoc = 		704;			//G		S32	
		
		//delayms����ʱΪ0��ò���Ϊ0
		public const Int16 delayoutLoc = 		704;			//G		S16			
		
		//����ʱ��������д����㿪ʼÿ���ڼ�1
		public const Int16 timerLoc = 		    481;			//G		S16			
		
		//�������ڼ�������ÿ�������ڼ�1
		public const Int16 ticksLoc = 		    502;			//G		S32			

		#endregion	
	/*********************************************************************************************************/		
		#region �¼�ָ����ز���
	/*********************************************************************************************************/		
		//�¼�ָ���������������ֹ�����¼�ָ��
		public const Int16 eventsLoc = 		    489;			//G		S16	
		#endregion	

	/*********************************************************************************************************/		
		#region ��ͣ/��ͣ��ز���
	/*********************************************************************************************************/		
		//ĳЩλ������λ����ʹ�������error�Ĵ�����Ӧλ����λ
		public const Int16 emstopLoc = 	    	500;			//G		R16		
		
		//��������ͣ
		public const Int16 hpauseLoc = 	    	501;			//G		F16		
		#endregion		

	/*********************************************************************************************************/		
		#region  ϵͳ���� Read only
	/*********************************************************************************************************/	
		//���Ʒ�Ƶ
		public const Int16 clkdivLoc = 	    	509;			//G		S16			
		
		//firmware �汾
		public const Int16 fwversionLoc =   	511;			//G		S16			
		
		//ϵͳ��׼ʱ��
		public const Int16 sysclkLoc = 	    	628;			//G		S32				
		
		//���ͺ�iMC֧�ֵ�����
		public const Int16 naxisLoc = 		    634;			//G		S16				
		
		//Ӳ���汾
		public const Int16 hwversionLoc =   	635;			//G		S16			
		
		//�������FIFO�Ͳ����ĵȴ�ָ���
		public const Int16 clearimcLoc = 		494;			//G		S16		
		#endregion	

	/*********************************************************************************************************/		
		#region  Ԥ�����û�����
	/*********************************************************************************************************/		
		public const Int16 user16b0Loc = 		307;			//G		S16		  	//16bit��Ԥ�����û�����0
		public const Int16 user16b1Loc = 		308;			//G		S16			//16bit��Ԥ�����û�����1
		public const Int16 user16b2Loc = 		309;			//G		S16			//16bit��Ԥ�����û�����2
		public const Int16 user16b3Loc = 		310;			//G		S16			//16bit��Ԥ�����û�����3
		public const Int16 user16b4Loc = 		311;			//G		S16			//16bit��Ԥ�����û�����4
		public const Int16 user16b5Loc = 		312;			//G		S16			//16bit��Ԥ�����û�����5
		public const Int16 user16b6Loc = 		313;			//G		S16			//16bit��Ԥ�����û�����6
		public const Int16 user16b7Loc = 		314;			//G		S16			//16bit��Ԥ�����û�����7
		public const Int16 user16b8Loc = 		315;			//G		S16			//16bit��Ԥ�����û�����8
		public const Int16 user16b9Loc = 		316;			//G		S16			//16bit��Ԥ�����û�����9

		public const Int16 user32b0Loc = 		317;			//G		S32			//32bit��Ԥ�����û�����0
		public const Int16 user32b1Loc = 		319;			//G		S32			//32bit��Ԥ�����û�����1
		public const Int16 user32b2Loc = 		321;			//G		S32			//32bit��Ԥ�����û�����2
		public const Int16 user32b3Loc = 		323;			//G		S32			//32bit��Ԥ�����û�����3
		public const Int16 user32b4Loc = 		325;			//G		S32			//32bit��Ԥ�����û�����4
		public const Int16 user32b5Loc = 		327;			//G		S32			//32bit��Ԥ�����û�����5
		public const Int16 user32b6Loc = 		329;			//G		S32			//32bit��Ԥ�����û�����6
		public const Int16 user32b7Loc = 		331;			//G		S32			//32bit��Ԥ�����û�����7
		public const Int16 user32b8Loc = 		333;			//G		S32			//32bit��Ԥ�����û�����8
		public const Int16 user32b9Loc = 		335;			//G		S32			//32bit��Ԥ�����û�����9

		public const Int16 user48b0Loc = 		337;			//G		S48			//48bit��Ԥ�����û�����0
		public const Int16 user48b1Loc = 		340;			//G		S48			//48bit��Ԥ�����û�����1

		#endregion

	/*********************************************************************************************************/		
		#region  λ�ò������
	/*********************************************************************************************************/		
		//д���������λ�ò��񻺴���capfifo
		public const Int16 clrcapfifoLoc =  	540;			//A		S16		
		
		//capfifo����ѹ���λ�����ݸ���
		public const Int16 capfifocntLoc =  	540;			//A		S16			

		public const Int16 compdistLoc = 		542;			//A		S32		
		#endregion
			
	/*********************************************************************************************************/		
		#region  ˫��������
	/*********************************************************************************************************/		
		//д��curvel�ĵ�ַ��ʹ�ܸ���������Ÿ�����������
		public const Int16 gantrymainptrLoc =   188;			//A		S16		
		
		//�����������
		public const Int16 gantrymainaxisLoc =  189;			//A		S16				
		
		//�������������ֵ������ |��poserr-��poserr|>=gantryerrlim,��λ����error
		public const Int16 gantryerrlimLoc = 	190;			//A		S16			 	
		
		//������������kp
		public const Int16 gantrykpLoc = 		191;			//A		S16				
		
		//��������ʱ���Ӷ���Ĵ�����Щbitӳ�䵽����Ĵ���Ĵ���
		public const Int16 gantryerrormapLoc =  192;			//A		S16				

		#endregion
	/*********************************************************************************************************/		
		#region  tgpos proportion follow
	/*********************************************************************************************************/		
		//����Ĳ���
		public const Int16 propfollowptrLoc =   103;			//A		S16		
		
		//����Ĳ������ڵ����
		public const Int16 propfollowaxisLoc =  174;			//A		S16			
		
		//����Ĳ�����ʼֵ
		public const Int16 propfollowfromLoc =  104;			//A		S32				
		
		//����Ĳ����ս�ֵ
		public const Int16 propfollowtoLoc =    106;			//A		S32				
		
		//��Ӧ�ڸ��������ʼֵʱ�����ֵ
		public const Int16 propstartLoc = 	    108;			//A		S32				
		
		//��Ӧ�ڸ�������ս�ֵʱ�����ֵ
		public const Int16 propendLoc = 		110;			//A		S32				
		
		//�������ҹ��Ĳ���
		public const Int16 prophookptrLoc = 	172;			//A		S16			
		
		//�������ҹ��Ĳ������ڵ����
		public const Int16 prophookaxisLoc = 	173;			//A		S16			
		
		
		public const Int16 propshiftLoc = 	    178;			//A		S32		
		
		#endregion
		
	/*********************************************************************************************************/		
		#region  DA/PWM output proportion follow
	/*********************************************************************************************************/	
		//����Ĳ���
		public const Int16 PWMfollowptrLoc = 	155;			//A		S16		
		
		//����Ĳ������ڵ����
		public const Int16 PWMfollowaxisLoc =   156;			//A		S16			
		
		//����Ĳ���
		public const Int16 DAfollowptrLoc = 	143;			//A		S16				
		
		//����Ĳ������ڵ����
		public const Int16 DAfollowaxisLoc = 	144;			//A		S16			
		
		//����Ĳ�����ʼֵ
		public const Int16 DAfollowfromLoc = 	112;			//A		S32				
		
		//����Ĳ����ս�ֵ
		public const Int16 DAfollowtoLoc = 	    114;			//A		S32				
		
		//��Ӧ�ڸ��������ʼֵʱ��pwmֵ(Ƶ�ʻ�ռ�ձ�)
		public const Int16 DAstartLoc = 		116;			//A		S32				
		
		//��Ӧ�ڸ�������ս�ֵʱ��pwmֵ
		public const Int16 DAendLoc = 		    118;			//A		S32				
		
		//DA���ƫ��ֵ
		public const Int16 DAshiftLoc = 		216;			//A		S16				
		
		//PWM���ƫ��ֵ
		public const Int16 PWMshiftLoc = 		221;			//A		S32				
		
		//DAʹ��
		public const Int16 DAenaLoc = 		    700;			//A		S16				
		
		//PWMʹ��
		public const Int16 PWMctrLoc = 		    702;			//A		S16				
		
		//�趨PWM��Ƶ��,pwmfreq = (f/1000)*65536��fΪpwm��ʵ��Ƶ�ʣ�����/�룩��
		public const Int16 PWMfreqLoc = 		708;			//A		S16.16		

		//PWM�����ռ�ձ�
		public const Int16 PWMpropLoc = 		129;			//A		S16	

        #endregion
		
	/*********************************************************************************************************/		
		#region Fix proportion follow
	/*********************************************************************************************************/		
		//������������Ĳ���
		public const Int16 fixfollowptrLoc = 	166;			//A		S16		
		
		//������������Ĳ������ڵ����
		public const Int16 fixfollowaxisLoc = 167;			//A		S16			
		
		//������������Ĳ�����ʼֵ
		public const Int16 fixfollowfromLoc = 120;			//A		S32				
		
		//����Ĳ����ս�ֵ
		public const Int16 fixfollowtoLoc = 	122;			//A		S32				
		
		//��Ӧ�ڸ��������ʼֵʱ�Ĳ���ֵ
		public const Int16 fixstartLoc = 		124;			//A		S32				
		
		//��Ӧ�ڸ�������ս�ֵʱ�Ĳ���ֵ
		public const Int16 fixendLoc = 		126;			//A		S32				
		
        #endregion
		
	/*********************************************************************************************************/		
		#region AD����
	/*********************************************************************************************************/		
		//ADͨ��ʹ�ܣ�bit0~bit7�е�bitΪ1,��Ӧ��ͨ��ʹ��
		public const Int16 ADchannelLoc = 	    614;			//G		S16			
		
		//16bit��AD���ݣ�ֻ��
		public const Int16 ADdataLoc = 		    696;			//A		S16			
		
		//32bit��AD���ݣ����ڸ����
		public const Int16 ADdata32Loc = 		696;			//A		S32			
				
        #endregion
			
		
	/*********************************************************************************************************/		
		#region ����ͨ�ż��
		public const Int16 comdogLoc = 		    703;			//G		S16							

        #endregion
		//ȫ�ֽ�������	���������ڴ���Ԥ��Ϊ 000010000h
		public const Int16 feedrateLoc = 		350;			//G		S16.16
	


    }

}