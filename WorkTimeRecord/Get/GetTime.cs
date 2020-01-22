using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Threading;

namespace Get
{
    class GetTime
    {
        #region 获取标准北京时间
        ///<summary>
        /// 获取标准北京时间
        ///</summary>
        ///<returns></returns>
        public static DateTime GetBeijingTime()
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://www.baidu.com");
            /*或者试试下面这些
             * https://www.baidu.com
             * time.windows.com
             * time.asia.apple.com
             * asia.pool.ntp.org
             * ntp.nict.jp
             */
            request.Method = "HEAD";
            request.AllowAutoRedirect = false;
            HttpWebResponse reponse = (HttpWebResponse)request.GetResponse();
            string cc = reponse.GetResponseHeader("date");
            reponse.Close();
            DateTime time;
            bool s = GMTStrParse(cc, out time);
            return time.AddHours(8); //GMT要加8个小时才是北京时间
        }
        public static bool GMTStrParse(string gmtStr, out DateTime gmtTime)  //抓取的date是GMT格式的字符串，这里转成datetime
        {
            CultureInfo enUS = new CultureInfo("en-US");
            bool s = DateTime.TryParseExact(gmtStr, "r", enUS, DateTimeStyles.None, out gmtTime);
            return s;
        }
        #endregion

        #region 调用CMD获取局域网指定电脑时间
        public static string GetLocalNetworkTime()
        {
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.UseShellExecute = false;    //是否使用操作系统shell启动
            p.StartInfo.RedirectStandardInput = true;//接受来自调用程序的输入信息
            p.StartInfo.RedirectStandardOutput = true;//由调用程序获取输出信息
            p.StartInfo.RedirectStandardError = true;//重定向标准错误输出
            p.StartInfo.CreateNoWindow = true;//不显示程序窗口
            p.Start();//启动程序

            //向cmd窗口发送输入信息
            if (@GlobalVariables.LocalIP == "net_time")
            {
                p.StandardInput.WriteLine(@"net time " + "&exit"); //设置文件的格式: net_time
            }
            else
            {
                p.StandardInput.WriteLine(@"net time " + @GlobalVariables.LocalIP + " &exit"); //设置文件的格式：\\10.8.0.6 或者 \\asdf
            }

            p.StandardInput.AutoFlush = true;
            //p.StandardInput.WriteLine("exit");
            //向标准输入写入要执行的命令。这里使用&是批处理命令的符号，表示前面一个命令不管是否执行成功都执行后面(exit)命令，如果不执行exit命令，后面调用ReadToEnd()方法会假死
            //同类的符号还有&&和||前者表示必须前一个命令执行成功才会执行后面的命令，后者表示必须前一个命令执行失败才会执行后面的命令

            //获取cmd窗口的输出信息
            string output = p.StandardOutput.ReadToEnd();

            //StreamReader reader = p.StandardOutput;
            //string line=reader.ReadLine();
            //while (!reader.EndOfStream)
            //{
            //    str += line + "  ";
            //    line = reader.ReadLine();
            //}

            p.WaitForExit();//等待程序执行完退出进程
            p.Close();

            switch(System.Threading.Thread.CurrentThread.CurrentCulture.Name)
            {
                case "zh-CN":
                    System.Windows.Forms.MessageBox.Show("只支持日语系统");
                    break;
                case "ja-JP":
                    output = output.Split(new char[] { 'は', 'で' }, StringSplitOptions.RemoveEmptyEntries)[1];
                    break;
                case "en-US":
                    System.Windows.Forms.MessageBox.Show("只支持日语系统");
                    break;
                default:
                    System.Windows.Forms.MessageBox.Show("只支持日语系统");
                    break;
            }
            return output;
        }
        #endregion

        /// <summary>
        /// 刷新 NowTime
        /// </summary>
        public static void GetSelectedTime()
        {

            if(GlobalVariables.TimeObtain == "1")
            {
                NowTime.FullTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:sss");
            }
            else if(GlobalVariables.TimeObtain == "2")
            {
                NowTime.FullTime = GetLocalNetworkTime();
            }
            else
            {
                NowTime.FullTime = GetBeijingTime().ToString();
            }

            string[] ArrayTime = NowTime.FullTime.Split(new char[] { '/', ' ', ':' }, StringSplitOptions.RemoveEmptyEntries);
            NowTime.YearMonthDay = ArrayTime[0] +"/"+ ArrayTime[1] +"/"+ ArrayTime[2];
            NowTime.HourMinuteSecond = ArrayTime[3] + "：" + ArrayTime[4] + "：" + ArrayTime[5];
        }
    }
}

//#region
//using System;
//using System.IO;
//using Common.Utility.CLR;
//using Option.Utility.CLR.PFare;

//namespace Option.Utility.CLR.IcPlanningMaster
//{
//    /// <summary>
//    /// IC企画券解析用クラス
//    /// </summary>
//    public class IcPlanningMasterDebug
//    {
//        /// <summary>企画券データ解析ログファイル名</summary>
//        private static readonly string IcPlanningMasterLogFileName = SettingData.Dump + "IcPlanningMasterData.log";

//        /// <summary>企画券データデバッグ用インプットファイル名</summary>
//        private static readonly string IcPlanningMasterDebugFileName = SettingData.Dump + "IcPlanningMasterInputData.txt";

//        /// <summary>
//        /// インスタンス
//        /// </summary>
//        private static IcPlanningMasterDebug instance = new IcPlanningMasterDebug();

//        /// <summary>
//        /// インスタンス取得
//        /// </summary>
//        /// <returns>インスタンス</returns>
//        public static IcPlanningMasterDebug GetInstance()
//        {
//            return instance;
//        }

//        /// <summary>
//        /// 企画券マスタデバッグ
//        /// </summary>
//        public void IcPlanningMasterApiDebugStart()
//        {
//            try
//            {
//                // ファイル存在チェック
//                if (File.Exists(IcPlanningMasterDebugFileName))
//                {
//                    // ファイル読込
//                    using (StreamReader reader = new StreamReader(IcPlanningMasterDebugFileName, System.Text.Encoding.GetEncoding("shift_jis")))
//                    {
//                        int count = 1;
//                        // ファイルに保持しているデータを内部データに保持
//                        while (!reader.EndOfStream)
//                        {
//                            IcPlanningAccount planningAccountData = new IcPlanningAccount();
//                            string line = reader.ReadLine();
//                            char[] separete = { ',' };
//                            string[] saveData = line.Split(separete);
//                            WriteMessage("NO." + count);
//                            WriteMessage("実施種別．　：　　　　　　" + saveData[0].Trim());　　　        // 実施種別.
//                            switch (saveData[0].Trim())
//                            {
//                                case "1":
//                                    {
//                                        WriteMessage("企画券Ｎｏ．：　　　　　　" + saveData[1].Trim());　　　        // 企画券No.
//                                        WriteMessage("実施日：　　　　　　　　　" + saveData[2].ToString().Trim());　 // 実施日
//                                        try
//                                        {
//                                            planningAccountData = IcPlanningGenerationManage.GetAccountData(saveData[1].ToInt32(), ConvertToDateInputCLR(saveData[2].ToString().Trim().PadLeft(12, '0')));
//                                        }
//                                        catch
//                                        {
//                                            // WriteMessage("解析異常");
//                                        }
//                                        if (planningAccountData != null && planningAccountData.AdultType != null)
//                                        {
//                                            LogOut(planningAccountData);
//                                        }
//                                        else
//                                        {
//                                            WriteMessage("解析異常");
//                                        }
//                                    }
//                                    break;
//                                case "2":
//                                    WriteMessage("発駅(線区)．：　　　　　　" + saveData[1].Trim());　　　        // 発駅(線区)
//                                    WriteMessage("発駅(駅順)．：　　　　　　" + saveData[2].Trim());　　　        // 発駅(駅順)
//                                    WriteMessage("実施日：　　　　　　　　　" + saveData[3].ToString().Trim());　 // 実施日
//                                    try
//                                    {
//                                        planningAccountData = IcPlanningGenerationManage.GetAccountDataFromCode((byte)saveData[1].ToInt32(), (byte)saveData[2].ToInt32(), new DateInputCLR(ConvertToDateInputCLR(saveData[3].ToString().Trim().PadLeft(8, '0'), false)));
//                                    }
//                                    catch
//                                    {
//                                        // WriteMessage("解析異常");
//                                    }
//                                    if (planningAccountData != null && planningAccountData.AdultType != null)
//                                    {
//                                        LogOut(planningAccountData);
//                                    }
//                                    else
//                                    {
//                                        WriteMessage("解析異常");
//                                    }
//                                    break;
//                                case "3":
//                                    WriteMessage("実施日：　　　　　　　　　" + saveData[1].ToString().Trim());　 // 実施日
//                                    IcPlanningGeneration planningGenerationData = new IcPlanningGeneration();
//                                    try
//                                    {
//                                        planningGenerationData = IcPlanningGenerationManage.GetGeneration(ConvertToDateInputCLR(saveData[1].ToString().Trim().PadRight(10, '0')));
//                                    }
//                                    catch
//                                    {
//                                        WriteMessage("解析異常");
//                                    }
//                                    if (planningGenerationData != null)
//                                    {
//                                        WriteMessage("世代．：　　　　　 　　   " + planningGenerationData.GenerationID.Trim());　　　        // 世代
//                                        WriteMessage("適用日．：　　　　 　　　 " + planningGenerationData.Date.ToString("yyyy/MM/dd"));　　　        // 適用日
//                                    }
//                                    else
//                                    {
//                                        WriteMessage("解析異常");
//                                    }
//                                    break;
//                            }
//                            count++;
//                            WriteMessage(string.Empty);
//                        }
//                    }
//                }
//            }
//            catch (System.IO.IOException)
//            {
//                WriteMessage("解析異常");
//            }
//        }

//        /// <summary>
//        /// IC企画券マスタデータ出力
//        /// </summary>
//        /// <param name="planningAccountData">企画券データ</param>
//        private void LogOut(IcPlanningAccount planningAccountData)
//        {
//            WriteMessage("■■■■■■企画券口座情報解析開始■■■■■■");
//            WriteMessage("企画券Ｎｏ．：　　　　　　" + planningAccountData.TicketNo.ToString().Trim());　　　           // 企画券No.
//            WriteMessage("企画券商品コード：　　　　" + planningAccountData.CommodityCode.ToString().Trim());　　        // 企画券商品コード
//            WriteMessage("発売終了日(運用日付)：    " + Convert.ToString(planningAccountData.EndDate).Substring(0, 10)); // 発売終了日
//            WriteMessage("有効日数：　　　　　　　　" + planningAccountData.PlanningValidDay.ToString().Trim());　　     // 有効日数
//            WriteMessage("２４時間判定：　　　　　　" + planningAccountData.TwentyFourHourJudge.ToString().Trim());　    // ２４時間判定
//            WriteMessage("発駅コード：　　　　　　　" + planningAccountData.StartingLineCode1.ToString().Trim() + ";" + planningAccountData.StartingStationCode1.ToString().Trim());　 // 発駅コード
//            WriteMessage("着駅コード：　　　　　　　" + planningAccountData.ArrivalLineCode1.ToString().Trim() + ";" + planningAccountData.ArrivalStationCode1.ToString().Trim());　　 // 着駅コード
//            WriteMessage("経由駅１コード：　　　　　" + planningAccountData.ConnectionLine1.ToString().Trim() + ";" + planningAccountData.ConnectionStation1.ToString().Trim());　　   // 経由駅1コード
//            WriteMessage("経由駅２コード：　　　　　" + planningAccountData.ConnectionLine2.ToString().Trim() + ";" + planningAccountData.ConnectionStation2.ToString().Trim());　　   // 経由駅2コード
//            WriteMessage("経由駅３コード：　　　　　" + planningAccountData.ConnectionLine3.ToString().Trim() + ";" + planningAccountData.ConnectionStation3.ToString().Trim());　　   // 経由駅3コード
//            WriteMessage("経由駅４コード：　　　　　" + planningAccountData.ConnectionLine4.ToString().Trim() + ";" + planningAccountData.ConnectionStation4.ToString().Trim());　   　// 経由駅4コード
//            WriteMessage("経由駅５コード：　　　　　" + planningAccountData.ConnectionLine5.ToString().Trim() + ";" + planningAccountData.ConnectionStation5.ToString().Trim());　　   // 経由駅5コード
//            WriteMessage("経由駅６コード：　　　　　" + planningAccountData.ConnectionLine6.ToString().Trim() + ";" + planningAccountData.ConnectionStation6.ToString().Trim());　　   // 経由駅6コード
//            WriteMessage("経由駅７コード：　　　　　" + planningAccountData.ConnectionLine7.ToString().Trim() + ";" + planningAccountData.ConnectionStation7.ToString().Trim());　　   // 経由駅7コード
//            WriteMessage("経由駅８コード：　　　　　" + planningAccountData.ConnectionLine8.ToString().Trim() + ";" + planningAccountData.ConnectionStation8.ToString().Trim());　　   // 経由駅8コード
//            WriteMessage("経由駅９コード：　　　　　" + planningAccountData.ConnectionLine9.ToString().Trim() + ";" + planningAccountData.ConnectionStation9.ToString().Trim());　     // 経由駅9コード
//            WriteMessage("経由駅１０コード：　　　　" + planningAccountData.ConnectionLine10.ToString().Trim() + ";" + planningAccountData.ConnectionStation10.ToString().Trim());     // 経由駅10コード
//            WriteMessage("前売り設定：　　　　　　　" + planningAccountData.IsPreSale.ToString().Trim());　          // 前売り設定
//            WriteMessage("ご案内用紙有無選択：　　　" + planningAccountData.IsReferenceButton.ToString().Trim());　　// ご案内用紙有無選択
//            WriteMessage("注意文：　　　　　　　　　" + planningAccountData.IsGuideView.ToString().Trim());　　      // 注意文
//            WriteMessage("案内券枚数（外国人向け）：" + planningAccountData.ReferenceCount.ToString().Trim());       // 案内券枚数（外国人向け）
//            WriteMessage("口座種類(大人)：　　　　　" + planningAccountData.AdultId.ToString().Trim());　　          // 口座種類(大人)
//            WriteMessage("口座種別(大人)：　　　　　" + planningAccountData.AdultType);　　                          // 口座種別(大人)
//            WriteMessage("着社運賃区分(大人)：　　　" + planningAccountData.AdultKubun.ToString().Trim());　　       // 着社運賃区分(大人)
//            WriteMessage("発社運賃区数(大人)：　　　" + planningAccountData.AdultKusu.ToString().Trim());　　        // 発社運賃区数(大人)
//            WriteMessage("着社運賃区間キロ程(大人)：" + planningAccountData.AdultDis.ToString().Trim());　　         // 着社運賃区間キロ程(大人)
//            WriteMessage("設備(大人)：　　　　　　　" + planningAccountData.AdultMac.ToString().Trim());　　         // 設備(大人)
//            WriteMessage("発社社コード(大人)：　　　" + planningAccountData.AdultOwnCode.ToString().Trim());　　     // 発社社コード(大人)
//            WriteMessage("発社単価(大人)：　　　　　" + planningAccountData.AdultOwnPrice.ToString().Trim());　      // 発社単価(大人)
//            WriteMessage("通過社１社コード(大人)：　" + planningAccountData.AdultVia1Code.ToString().Trim());　　    // 通過社１社コード(大人)
//            WriteMessage("通過社１単価(大人)：　　　" + planningAccountData.AdultVia1Price.ToString().Trim());　　   // 通過社１単価(大人)
//            WriteMessage("通過社２社コード(大人)：　" + planningAccountData.AdultVia2Code.ToString().Trim());　　    // 通過社２社コード(大人)
//            WriteMessage("通過社２単価(大人)：　　　" + planningAccountData.AdultVia2Price.ToString().Trim());　　   // 通過社２単価(大人)
//            WriteMessage("着社社コード(大人)：　　　" + planningAccountData.AdultEndCode.ToString().Trim());　　     // 着社社コード(大人)
//            WriteMessage("着社単価(大人)：　　　　　" + planningAccountData.AdultEndPrice.ToString().Trim());　　    // 着社単価(大人)
//            WriteMessage("口座種類(小児)：　　　　　" + planningAccountData.ChildId.ToString().Trim());　            // 口座種類(小児)
//            WriteMessage("口座種別(小児)：　　　　　" + planningAccountData.ChildType.ToString().Trim());　　        // 口座種別(小児)
//            WriteMessage("着社運賃区分(小児)：　　　" + planningAccountData.ChildKubun.ToString().Trim());　　       // 着社運賃区分(小児)
//            WriteMessage("発社運賃区数(小児)：　　　" + planningAccountData.ChildKusu.ToString().Trim());　　        // 発社運賃区数(小児)
//            WriteMessage("着社運賃区間キロ程(小児)：" + planningAccountData.ChildDis.ToString().Trim());　　         // 着社運賃区間キロ程(小児)
//            WriteMessage("設備(小児)：　　　　　　　" + planningAccountData.ChildMac.ToString().Trim());　　         // 設備(小児)
//            WriteMessage("発社社コード(小児)：　　　" + planningAccountData.ChildOwnCode.ToString().Trim());　　     // 発社社コード(小児)
//            WriteMessage("発社単価(小児)：　　　　　" + planningAccountData.ChildOwnPrice.ToString().Trim());　　    // 発社単価(小児)
//            WriteMessage("通過社１社コード(小児)：　" + planningAccountData.ChildVia1Code.ToString().Trim());　　    // 通過社１社コード(小児)
//            WriteMessage("通過社１単価(小児)：　　　" + planningAccountData.ChildVia1Price.ToString().Trim());　　   // 通過社１単価(小児)
//            WriteMessage("通過社２社コード(小児)：　" + planningAccountData.ChildVia2Code.ToString().Trim());　　    // 通過社２社コード(小児)
//            WriteMessage("通過社２単価(小児)：　　　" + planningAccountData.ChildVia2Price.ToString().Trim());　　   // 通過社２単価(小児)
//            WriteMessage("着社社コード(小児)：　　　" + planningAccountData.ChildEndCode.ToString().Trim());　　     // 着社社コード(小児)
//            WriteMessage("着社単価(小児)：　　　　　" + planningAccountData.ChildEndPrice.ToString().Trim());　　    // 着社単価(小児)
//            WriteMessage("■■■■■■企画券口座情報解析終了■■■■■■");
//            WriteMessage(string.Empty);
//        }

//        /// <summary>
//        /// ログ出力
//        /// </summary>
//        /// <param name="msg">ログ内容</param>
//        private void WriteMessage(string msg)
//        {
//            using (FileStream fs = new FileStream(IcPlanningMasterLogFileName, FileMode.OpenOrCreate, FileAccess.Write))
//            {
//                using (StreamWriter sw = new StreamWriter(fs))
//                {
//                    sw.BaseStream.Seek(0, SeekOrigin.End);
//                    sw.WriteLine("{0}", msg);
//                    sw.Flush();
//                }
//            }
//        }

//        /// <summary>
//        /// 日付情報型に変換
//        /// </summary>
//        /// <param name="source">変換元文字列</param>
//        /// <param name="isAddTime">時間付け</param>
//        /// <returns>日付情報</returns>
//        private DateTime ConvertToDateInputCLR(string source, bool isAddTime = true)
//        {
//            var date = new DateInputCLR();
//            DateTime dataTime = new DateTime();
//            if (isAddTime)
//            {
//                int hour = 0;
//                int minute = 0;
//                int second = 0;
//                if (14 == source.Length)
//                {
//                    ushort buf = 0;
//                    // yyyy
//                    if (ushort.TryParse(source.Substring(0, 4), out buf))
//                    {
//                        date.Year = buf;
//                    }
//                    if (ushort.TryParse(source.Substring(4, 2), out buf))
//                    {
//                        date.Month = buf;
//                    }
//                    if (ushort.TryParse(source.Substring(6, 2), out buf))
//                    {
//                        date.Day = buf;
//                    }
//                    if (ushort.TryParse(source.Substring(8, 2), out buf))
//                    {
//                        hour = buf;
//                    }
//                    if (ushort.TryParse(source.Substring(10, 2), out buf))
//                    {
//                        minute = buf;
//                    }
//                    if (ushort.TryParse(source.Substring(12, 2), out buf))
//                    {
//                        second = buf;
//                    }
//                    dataTime = new DateTime(date.Year, date.Month, date.Day, hour, minute, second);
//                }
//            }
//            else
//            {
//                if (8 == source.Length)
//                {
//                    ushort buf = 0;
//                    // yyyy
//                    if (ushort.TryParse(source.Substring(0, 4), out buf))
//                    {
//                        date.Year = buf;
//                    }
//                    if (ushort.TryParse(source.Substring(4, 2), out buf))
//                    {
//                        date.Month = buf;
//                    }
//                    if (ushort.TryParse(source.Substring(6, 2), out buf))
//                    {
//                        date.Day = buf;
//                    }
//                    dataTime = new DateTime(date.Year, date.Month, date.Day);
//                }
//            }
//            return dataTime;
//        }
//    }
//}

//#endregion
