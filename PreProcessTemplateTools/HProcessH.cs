using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PreProcessTemplateTools
{

    class Operator  // 操作表
    {
        public int Index = 0;

        public Dictionary<HTuple[], string> UsedRegion = new Dictionary<HTuple[], string>();

        public void AddOperation(HTuple[] pos, string color)
        {
            UsedRegion.Add(pos, color);
            Index++;
        }

        public void SubOperation()
        {
            try
            {
                UsedRegion.Remove(UsedRegion.Last().Key);
                Index--;
            }
            catch
            {
                Index = 0;
            }
        }
    }

    // halcon处理模块
    class HProcessH
    {
        public HObject Image = new HObject();  // 原始对象
        HObject GrayImage = new HObject();  // 灰度图
        public HObject TempImage;  // 临时操作对象
        HObject ho_Rectangle;

        public Operator opt = new Operator();

        public List<HObject> OperationList = new List<HObject>();


        string OriginPath = "";
        public TikaModel tikaModel = new TikaModel();
        public HWindowControl HWindowControl_0 = null;

        string savePath = Application.StartupPath + "\\savePath\\";  // 保存路径

        public double zoomValue { get; set; }

        public HProcessH(HWindowControl HWC, string OriginPath)
        {
            this.HWindowControl_0 = HWC;  // 窗口控件
            this.OriginPath = OriginPath;  // 模板路径
        }


        public void LoadModel()
        {
            Image.Dispose();
            HTuple hv_Width, hv_Height;
            HOperatorSet.ReadImage(out Image, OriginPath);
            HOperatorSet.GetImageSize(Image,out hv_Width,out hv_Height);
            HOperatorSet.Rgb1ToGray(Image, out GrayImage);  // 转成全局灰度图

            TempImage = new HObject(GrayImage);

            if (tikaModel.OriginPath == null)
            {
                tikaModel.OriginPath = OriginPath;
            }
            if (tikaModel.Width == null && tikaModel.Height == null)
            {
                tikaModel.Width = hv_Width;
                tikaModel.Height = hv_Height;
            }

            hv_Width.Dispose();
            hv_Height.Dispose();
        }


        public void ProcessTikaModel(HTuple a, HTuple b)
        {
            HTuple hv_MeanB2, hv_MeanT, hv_MeanW, hv_res;

            Enhance_Template_Image(GrayImage, out TempImage, 2, a, b, out hv_MeanB2, out hv_MeanT, out hv_MeanW, out hv_res);

            if (tikaModel.MeanGrayValue == null)
            {
                tikaModel.MeanGrayValue = hv_MeanT;  // 平均灰度值
            }
            if (tikaModel.MinGrayValue == null)
            {
                tikaModel.MinGrayValue = hv_MeanB2;  // 最小灰度值
            }
            if (tikaModel.MaxGrayValue == null)
            {
                tikaModel.MaxGrayValue = hv_MeanW;  // 最大灰度值
            }
            if (tikaModel.Status == null)
            {
                tikaModel.Status = hv_res;  // 状态
            }

            hv_MeanT.Dispose();
            hv_MeanB2.Dispose();
            hv_MeanW.Dispose();
            hv_res.Dispose();
        }

        public void ProcessTikaModel()
        {
            HTuple hv_MeanB2, hv_MeanT, hv_MeanW, hv_res;

            Enhance_Template_Image(GrayImage, out TempImage, 2, 0, 0, out hv_MeanB2, out hv_MeanT, out hv_MeanW, out hv_res);

            if (tikaModel.MeanGrayValue == null)
            {
                tikaModel.MeanGrayValue = hv_MeanT;  // 平均灰度值
            }
            if (tikaModel.MinGrayValue == null)
            {
                tikaModel.MinGrayValue = hv_MeanB2;  // 最小灰度值
            }
            if (tikaModel.MaxGrayValue == null)
            {
                tikaModel.MaxGrayValue = hv_MeanW;  // 最大灰度值
            }
            if (tikaModel.Status == null)
            {
                tikaModel.Status = hv_res;  // 状态
            }

            hv_MeanT.Dispose();
            hv_MeanB2.Dispose();
            hv_MeanW.Dispose();
            hv_res.Dispose();
        }


        public void ShowImage()  // 自适应显示/缩放
        {
            HTuple hv_Width = tikaModel.Width;
            HTuple hv_Height = tikaModel.Height;
            HTuple row1, column1, row2, column2;

            CalScaleValue(hv_Width, hv_Height, HWindowControl_0, out row1, out column1, out row2, out column2);  // 计算缩放比

            HWindowControl_0.HalconWindow.ClearWindow();
            HDevWindowStack.Push(HWindowControl_0.HalconWindow);
            HOperatorSet.SetColor(HDevWindowStack.GetActive(), "blue");
            HOperatorSet.SetPart(HDevWindowStack.GetActive(), row1, column1, row2, column2);
            HOperatorSet.DispObj(TempImage, HDevWindowStack.GetActive());

            hv_Width.Dispose();
            hv_Height.Dispose();
            row1.Dispose();
            column1.Dispose();
            row2.Dispose();
            column2.Dispose();
        }


        public void ShowImage(HTuple rowMove, HTuple columnMove)  // 图像拖动
        {
            HTuple row1, column1, row2, column2;

            HOperatorSet.GetPart(HWindowControl_0.HalconWindow, out row1, out column1, out row2, out column2);  //根据HWindow控件在当前状态下显示图像的位置

            HWindowControl_0.HalconWindow.ClearWindow();
            HDevWindowStack.Push(HWindowControl_0.HalconWindow);
            HOperatorSet.SetColor(HDevWindowStack.GetActive(), "blue");
            HOperatorSet.SetPart(HDevWindowStack.GetActive(), row1 + rowMove, column1 + columnMove, row2 + rowMove, column2 + columnMove);  // 计算拖动距离调整HWindows控件显示图像的位置
            HOperatorSet.DispObj(TempImage, HDevWindowStack.GetActive());

            row1.Dispose();
            column1.Dispose();
            row2.Dispose();
            column2.Dispose();
        }

        public void ShowImage(char i)  // 不改变位置刷新
        {
            HTuple row1, column1, row2, column2;

            HOperatorSet.GetPart(HWindowControl_0.HalconWindow, out row1, out column1, out row2, out column2);

            HWindowControl_0.HalconWindow.ClearWindow();
            HDevWindowStack.Push(HWindowControl_0.HalconWindow);
            HOperatorSet.SetColor(HDevWindowStack.GetActive(), "blue");
            HOperatorSet.SetPart(HDevWindowStack.GetActive(), row1, column1, row2, column2);
            HOperatorSet.DispObj(TempImage, HDevWindowStack.GetActive());


            row1.Dispose();
            column1.Dispose();
            row2.Dispose();
            column2.Dispose();
        }


        // 计算缩放比
        public void CalScaleValue(HTuple Width, HTuple Height, HWindowControl hv_WindowControl, out HTuple row1, out HTuple column1, out HTuple row2, out HTuple column2)
        {
            if (hv_WindowControl.Width == 0 || hv_WindowControl.Height == 0)
            {
                row1 = 1;
                column1 = 1;
                row2 = 1;
                column2 = 1;
                return;
            }

            HTuple ScaleWidth = Width / (hv_WindowControl.Width * zoomValue);
            HTuple ScaleHeight = Height / (hv_WindowControl.Height * zoomValue);

            // 计算缩放比
            if (ScaleWidth >= ScaleHeight)
            {
                row1 = -(1.0) * ((hv_WindowControl.Width * ScaleWidth) - Height) / 2;
                column1 = 0;
                row2 = row1 + hv_WindowControl.Height * ScaleWidth;
                column2 = column1 + hv_WindowControl.Width * ScaleWidth;
            }
            else
            {
                row1 = 0;
                column1 = -(1.0) * ((hv_WindowControl.Width * ScaleHeight) - Width) / 2;
                row2 = row1 + hv_WindowControl.Height * ScaleHeight;
                column2 = column1 + hv_WindowControl.Width * ScaleHeight;
            }

            ScaleWidth.Dispose();
            ScaleHeight.Dispose();
        }

        public void SetZoomValue(double value)
        {
            zoomValue = value;
        }

        public void SaveImage()
        {
            try
            {
                if (System.IO.Directory.Exists(savePath) == false)
                {
                    System.IO.Directory.CreateDirectory(savePath);
                }
                this.tikaModel.SavePath = savePath + tikaModel.OriginPath.Split('\\').Last();
                HOperatorSet.WriteImage(TempImage, "jpg", -1, this.tikaModel.SavePath);  // 保存的对象应该是栈顶元素
            }
            catch
            {
                Console.WriteLine("保存失败");
            }

        }


        public void DrawRectangle1(HTuple hv_Row1, HTuple hv_Column1, HTuple hv_Row2, HTuple hv_Column2)
        {
            HOperatorSet.GenEmptyObj(out ho_Rectangle);
            ho_Rectangle.Dispose();
            HOperatorSet.GenRectangle1(out ho_Rectangle, hv_Row1, hv_Column1, hv_Row2, hv_Column2);
            HOperatorSet.OverpaintRegion(TempImage, ho_Rectangle, 5, "fill");

            HTuple[] pos = new HTuple[4];
            pos[0] = hv_Row1;
            pos[1] = hv_Column1;
            pos[2] = hv_Row2;
            pos[3] = hv_Column2;
            opt.AddOperation(pos, "black");

            HOperatorSet.DispObj(TempImage, HDevWindowStack.GetActive());

        }


        public void DrawWhiteBlock(HTuple hv_Row1, HTuple hv_Column1, HTuple hv_Row2, HTuple hv_Column2)
        {
            HOperatorSet.GenEmptyObj(out ho_Rectangle);
            ho_Rectangle.Dispose();
            HOperatorSet.GenRectangle1(out ho_Rectangle, hv_Row1, hv_Column1, hv_Row2, hv_Column2);
            HOperatorSet.OverpaintRegion(TempImage, ho_Rectangle, 251, "fill");

            HTuple[] pos = new HTuple[4];
            pos[0] = hv_Row1;
            pos[1] = hv_Column1;
            pos[2] = hv_Row2;
            pos[3] = hv_Column2;
            opt.AddOperation(pos, "white");

            HOperatorSet.DispObj(TempImage, HDevWindowStack.GetActive());

        }

        public void ReDraw(Dictionary<HTuple[], string> pos)
        {
            //TempImage.Dispose();
            TempImage = TempImage;
            foreach (var item in pos)
            {
                if (item.Value == "white")
                {
                    ho_Rectangle.Dispose();
                    HOperatorSet.GenRectangle1(out ho_Rectangle, item.Key[0], item.Key[1], item.Key[2], item.Key[3]);
                    HOperatorSet.OverpaintRegion(TempImage, ho_Rectangle, 251, "fill");
                }
                else if (item.Value == "black")
                {
                    ho_Rectangle.Dispose();
                    HOperatorSet.GenRectangle1(out ho_Rectangle, item.Key[0], item.Key[1], item.Key[2], item.Key[3]);
                    HOperatorSet.OverpaintRegion(TempImage, ho_Rectangle, 5, "fill");
                }
            }
            ShowImage('o');
        }

        public void Dispose()
        {
            Image.Dispose();
            GrayImage.Dispose();

            // 清空堆栈
            //foreach (HObject item in ImageStack)
            //{
            //    item.Dispose();
            //}
            //ImageStack.Clear();
        }

        public void Enhance_Template_Image(HObject ho_Image, out HObject ho_ImageScaled,
    HTuple hv_chose, HTuple hv_a, HTuple hv_b, out HTuple hv_MeanB2, out HTuple hv_MeanT,
    out HTuple hv_MeanW, out HTuple hv_res)
        {




            // Local iconic variables 

            HObject ho_GrayImageT, ho_ROI_0 = null, ho_ImageReducedT = null;
            HObject ho_RegionB = null, ho_RegionB1 = null, ho_ImageReducedT1 = null;
            HObject ho_RegionB2 = null, ho_ImageReducedT12 = null, ho_RegionW = null;
            HObject ho_ImageReducedW = null, ho_RegionW1 = null, ho_ImageReducedW2 = null;
            HObject ho_Region = null, ho_RegionOpening1 = null, ho_Rectangle = null;
            HObject ho_RegionDifference = null, ho_Image1 = null, ho_Image2 = null;
            HObject ho_Image3 = null, ho_Region1 = null, ho_Region2 = null;
            HObject ho_Region3 = null;

            // Local control variables 

            HTuple hv_lenthT = new HTuple(), hv_Width_T = new HTuple();
            HTuple hv_Height_T = new HTuple(), hv_DeviationT = new HTuple();
            HTuple hv_MeanB = new HTuple(), hv_DeviationB = new HTuple();
            HTuple hv_DeviationB2 = new HTuple(), hv_MeanB3 = new HTuple();
            HTuple hv_DeviationB3 = new HTuple(), hv_DeviationW = new HTuple();
            HTuple hv_MeanW2 = new HTuple(), hv_DeviationW2 = new HTuple();
            HTuple hv_min1 = new HTuple(), hv_max1 = new HTuple();
            HTuple hv_mutil = new HTuple(), hv_M1 = new HTuple(), hv_M2 = new HTuple();
            HTuple hv_add = new HTuple(), hv_Width = new HTuple();
            HTuple hv_Height = new HTuple(), hv_Channels = new HTuple();
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_ImageScaled);
            HOperatorSet.GenEmptyObj(out ho_GrayImageT);
            HOperatorSet.GenEmptyObj(out ho_ROI_0);
            HOperatorSet.GenEmptyObj(out ho_ImageReducedT);
            HOperatorSet.GenEmptyObj(out ho_RegionB);
            HOperatorSet.GenEmptyObj(out ho_RegionB1);
            HOperatorSet.GenEmptyObj(out ho_ImageReducedT1);
            HOperatorSet.GenEmptyObj(out ho_RegionB2);
            HOperatorSet.GenEmptyObj(out ho_ImageReducedT12);
            HOperatorSet.GenEmptyObj(out ho_RegionW);
            HOperatorSet.GenEmptyObj(out ho_ImageReducedW);
            HOperatorSet.GenEmptyObj(out ho_RegionW1);
            HOperatorSet.GenEmptyObj(out ho_ImageReducedW2);
            HOperatorSet.GenEmptyObj(out ho_Region);
            HOperatorSet.GenEmptyObj(out ho_RegionOpening1);
            HOperatorSet.GenEmptyObj(out ho_Rectangle);
            HOperatorSet.GenEmptyObj(out ho_RegionDifference);
            HOperatorSet.GenEmptyObj(out ho_Image1);
            HOperatorSet.GenEmptyObj(out ho_Image2);
            HOperatorSet.GenEmptyObj(out ho_Image3);
            HOperatorSet.GenEmptyObj(out ho_Region1);
            HOperatorSet.GenEmptyObj(out ho_Region2);
            HOperatorSet.GenEmptyObj(out ho_Region3);
            hv_MeanB2 = new HTuple();
            hv_MeanT = new HTuple();
            hv_MeanW = new HTuple();
            hv_res = new HTuple();

            //stop ()
            hv_lenthT.Dispose();
            hv_lenthT = 30;
            //**预处理图片
            ho_GrayImageT.Dispose();
            HOperatorSet.Rgb1ToGray(ho_Image, out ho_GrayImageT);
            hv_Width_T.Dispose(); hv_Height_T.Dispose();
            HOperatorSet.GetImageSize(ho_Image, out hv_Width_T, out hv_Height_T);

            if ((int)((new HTuple(hv_Width_T.TupleGreater(hv_lenthT))).TupleAnd(new HTuple(hv_Height_T.TupleGreater(
                hv_lenthT)))) != 0)
            {
                if ((int)((new HTuple(hv_Width_T.TupleLess(2 * hv_lenthT))).TupleOr(new HTuple(hv_Height_T.TupleLess(
                    2 * hv_lenthT)))) != 0)
                {
                    hv_lenthT.Dispose();
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        hv_lenthT = (((hv_Width_T.TupleConcat(
                            hv_Height_T))).TupleMin()) / 4;
                    }
                }


                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    ho_ROI_0.Dispose();
                    HOperatorSet.GenRectangle1(out ho_ROI_0, hv_lenthT, hv_lenthT, hv_Height_T - hv_lenthT,
                        hv_Width_T - hv_lenthT);
                }

                ho_ImageReducedT.Dispose();
                HOperatorSet.ReduceDomain(ho_GrayImageT, ho_ROI_0, out ho_ImageReducedT);
                hv_MeanT.Dispose(); hv_DeviationT.Dispose();
                HOperatorSet.Intensity(ho_ROI_0, ho_GrayImageT, out hv_MeanT, out hv_DeviationT);
                //*     if (MeanT>230)

                //*获取字体灰度值
                ho_RegionB.Dispose();
                HOperatorSet.Threshold(ho_ImageReducedT, out ho_RegionB, 0, hv_MeanT);
                ho_ImageReducedT.Dispose();
                HOperatorSet.ReduceDomain(ho_GrayImageT, ho_RegionB, out ho_ImageReducedT);
                hv_MeanB.Dispose(); hv_DeviationB.Dispose();
                HOperatorSet.Intensity(ho_RegionB, ho_ImageReducedT, out hv_MeanB, out hv_DeviationB);

                ho_RegionB1.Dispose();
                HOperatorSet.Threshold(ho_ImageReducedT, out ho_RegionB1, 0, hv_MeanB);
                ho_ImageReducedT1.Dispose();
                HOperatorSet.ReduceDomain(ho_GrayImageT, ho_RegionB1, out ho_ImageReducedT1);
                hv_MeanB2.Dispose(); hv_DeviationB2.Dispose();
                HOperatorSet.Intensity(ho_RegionB1, ho_ImageReducedT1, out hv_MeanB2, out hv_DeviationB2);

                ho_RegionB2.Dispose();
                HOperatorSet.Threshold(ho_ImageReducedT, out ho_RegionB2, 0, hv_MeanB2);
                ho_ImageReducedT12.Dispose();
                HOperatorSet.ReduceDomain(ho_GrayImageT, ho_RegionB2, out ho_ImageReducedT12);
                hv_MeanB3.Dispose(); hv_DeviationB3.Dispose();
                HOperatorSet.Intensity(ho_RegionB2, ho_ImageReducedT12, out hv_MeanB3, out hv_DeviationB3);


                //*获取背景灰度值
                ho_RegionW.Dispose();
                HOperatorSet.Threshold(ho_GrayImageT, out ho_RegionW, hv_MeanT, 255);
                ho_ImageReducedW.Dispose();
                HOperatorSet.ReduceDomain(ho_GrayImageT, ho_RegionW, out ho_ImageReducedW);
                hv_MeanW.Dispose(); hv_DeviationW.Dispose();
                HOperatorSet.Intensity(ho_RegionW, ho_ImageReducedW, out hv_MeanW, out hv_DeviationW);

                ho_RegionW1.Dispose();
                HOperatorSet.Threshold(ho_GrayImageT, out ho_RegionW1, hv_MeanW, 255);
                ho_ImageReducedW2.Dispose();
                HOperatorSet.ReduceDomain(ho_GrayImageT, ho_RegionW1, out ho_ImageReducedW2);
                hv_MeanW2.Dispose(); hv_DeviationW2.Dispose();
                HOperatorSet.Intensity(ho_RegionW1, ho_ImageReducedW2, out hv_MeanW2, out hv_DeviationW2);


                //min1 := MeanB2+DeviationB2
                //max1 := MeanW-2*DeviationW

                if ((int)((new HTuple(hv_a.TupleEqual(0))).TupleAnd(new HTuple(hv_b.TupleEqual(0)))) != 0)
                {


                    if ((int)((new HTuple(hv_MeanB3.TupleGreater(45))).TupleOr(new HTuple(hv_MeanW2.TupleLess(249)))) != 0)
                    {
                        //chose := 1
                        //DeviationW2 := 15
                        if ((int)(new HTuple(hv_chose.TupleEqual(1))) != 0)
                        {
                            hv_min1.Dispose();
                            using (HDevDisposeHelper dh = new HDevDisposeHelper())
                            {
                                hv_min1 = hv_MeanB2 + hv_DeviationB2;
                            }
                            hv_max1.Dispose();
                            using (HDevDisposeHelper dh = new HDevDisposeHelper())
                            {
                                hv_max1 = hv_MeanW2 + hv_DeviationW2;
                            }
                        }
                        else if ((int)(new HTuple(hv_chose.TupleEqual(2))) != 0)
                        {
                            hv_min1.Dispose();
                            using (HDevDisposeHelper dh = new HDevDisposeHelper())
                            {
                                hv_min1 = hv_MeanB2 + hv_DeviationB2;
                            }
                            hv_max1.Dispose();
                            using (HDevDisposeHelper dh = new HDevDisposeHelper())
                            {
                                hv_max1 = hv_MeanW - (2 * hv_DeviationW);
                            }
                        }
                        else if ((int)(new HTuple(hv_chose.TupleEqual(3))) != 0)
                        {
                            hv_min1.Dispose();
                            using (HDevDisposeHelper dh = new HDevDisposeHelper())
                            {
                                hv_min1 = hv_MeanB2 - hv_DeviationB2;
                            }
                            hv_max1.Dispose();
                            using (HDevDisposeHelper dh = new HDevDisposeHelper())
                            {
                                hv_max1 = hv_MeanW2 + hv_DeviationW2;
                            }
                        }
                        else if ((int)(new HTuple(hv_chose.TupleEqual(4))) != 0)
                        {
                            hv_min1.Dispose();
                            using (HDevDisposeHelper dh = new HDevDisposeHelper())
                            {
                                hv_min1 = hv_MeanB2 - hv_DeviationB2;
                            }
                            hv_max1.Dispose();
                            using (HDevDisposeHelper dh = new HDevDisposeHelper())
                            {
                                hv_max1 = hv_MeanW2 - hv_DeviationW2;
                            }
                        }

                        if ((int)(new HTuple(((hv_max1 - hv_min1)).TupleGreater(10))) != 0)
                        {
                            hv_mutil.Dispose();
                            using (HDevDisposeHelper dh = new HDevDisposeHelper())
                            {
                                hv_mutil = 255 / (hv_max1 - hv_min1);
                            }
                            hv_M1.Dispose();
                            using (HDevDisposeHelper dh = new HDevDisposeHelper())
                            {
                                hv_M1 = hv_min1 * hv_mutil;
                            }
                            hv_M2.Dispose();
                            using (HDevDisposeHelper dh = new HDevDisposeHelper())
                            {
                                hv_M2 = hv_max1 * hv_mutil;
                            }
                            hv_add.Dispose();
                            using (HDevDisposeHelper dh = new HDevDisposeHelper())
                            {
                                hv_add = -hv_M1;
                            }
                        }
                        else if ((int)(new HTuple(hv_max1.TupleLessEqual(hv_min1))) != 0)
                        {
                            hv_min1.Dispose();
                            hv_min1 = 5;
                            hv_max1.Dispose();
                            hv_max1 = 250;
                            hv_mutil.Dispose();
                            using (HDevDisposeHelper dh = new HDevDisposeHelper())
                            {
                                hv_mutil = 255 / (hv_max1 - hv_min1);
                            }
                            hv_M1.Dispose();
                            using (HDevDisposeHelper dh = new HDevDisposeHelper())
                            {
                                hv_M1 = hv_min1 * hv_mutil;
                            }
                            hv_M2.Dispose();
                            using (HDevDisposeHelper dh = new HDevDisposeHelper())
                            {
                                hv_M2 = hv_max1 * hv_mutil;
                            }
                            hv_add.Dispose();
                            using (HDevDisposeHelper dh = new HDevDisposeHelper())
                            {
                                hv_add = -hv_M1;
                            }
                        }
                        else
                        {
                            hv_min1.Dispose();
                            using (HDevDisposeHelper dh = new HDevDisposeHelper())
                            {
                                hv_min1 = hv_max1 - 10;
                            }
                            {
                                HTuple
                                  ExpTmpLocalVar_max1 = new HTuple(hv_max1);
                                hv_max1.Dispose();
                                hv_max1 = ExpTmpLocalVar_max1;
                            }
                            hv_mutil.Dispose();
                            using (HDevDisposeHelper dh = new HDevDisposeHelper())
                            {
                                hv_mutil = 255 / (hv_max1 - hv_min1);
                            }
                            hv_M1.Dispose();
                            using (HDevDisposeHelper dh = new HDevDisposeHelper())
                            {
                                hv_M1 = hv_min1 * hv_mutil;
                            }
                            hv_M2.Dispose();
                            using (HDevDisposeHelper dh = new HDevDisposeHelper())
                            {
                                hv_M2 = hv_max1 * hv_mutil;
                            }
                            hv_add.Dispose();
                            using (HDevDisposeHelper dh = new HDevDisposeHelper())
                            {
                                hv_add = -hv_M1;
                            }

                        }


                        ho_ImageScaled.Dispose();
                        HOperatorSet.ScaleImage(ho_Image, out ho_ImageScaled, hv_mutil, hv_add);
                        ho_Region.Dispose();
                        HOperatorSet.Threshold(ho_ImageScaled, out ho_Region, 0, 220);
                        ho_RegionOpening1.Dispose();
                        HOperatorSet.OpeningCircle(ho_Region, out ho_RegionOpening1, 1);

                        hv_Width.Dispose(); hv_Height.Dispose();
                        HOperatorSet.GetImageSize(ho_Image, out hv_Width, out hv_Height);
                        using (HDevDisposeHelper dh = new HDevDisposeHelper())
                        {
                            ho_Rectangle.Dispose();
                            HOperatorSet.GenRectangle1(out ho_Rectangle, 0, 0, hv_Height - 1, hv_Width - 1);
                        }


                        ho_RegionDifference.Dispose();
                        HOperatorSet.Difference(ho_Rectangle, ho_RegionOpening1, out ho_RegionDifference
                            );
                        //stop ()
                        hv_Channels.Dispose();
                        HOperatorSet.CountChannels(ho_ImageScaled, out hv_Channels);

                        if ((int)(new HTuple(hv_Channels.TupleEqual(3))) != 0)
                        {
                            ho_Image1.Dispose(); ho_Image2.Dispose(); ho_Image3.Dispose();
                            HOperatorSet.Decompose3(ho_ImageScaled, out ho_Image1, out ho_Image2,
                                out ho_Image3);

                            HOperatorSet.OverpaintRegion(ho_Image1, ho_RegionDifference, 250, "fill");
                            HOperatorSet.OverpaintRegion(ho_Image2, ho_RegionDifference, 250, "fill");
                            HOperatorSet.OverpaintRegion(ho_Image3, ho_RegionDifference, 250, "fill");

                            ho_ImageScaled.Dispose();
                            HOperatorSet.Compose3(ho_Image1, ho_Image2, ho_Image3, out ho_ImageScaled
                                );
                        }
                        else if ((int)(new HTuple(hv_Channels.TupleEqual(1))) != 0)
                        {
                            HOperatorSet.OverpaintRegion(ho_ImageScaled, ho_RegionDifference, 250,
                                "fill");
                        }

                        {
                            HObject
                              ExpTmpLocalVar_ImageScaled = new HObject(ho_ImageScaled);
                            ho_ImageScaled.Dispose();
                            ho_ImageScaled = ExpTmpLocalVar_ImageScaled;
                        }
                    }
                    else
                    {
                        ho_ImageScaled.Dispose();
                        ho_ImageScaled = new HObject(ho_Image);
                    }
                    hv_res.Dispose();
                    hv_res = "ok";

                }
                else
                {
                    hv_min1.Dispose();
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        hv_min1 = hv_a;
                    }
                    hv_max1.Dispose();
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        hv_max1 = hv_b;
                    }

                    if ((int)(new HTuple(hv_min1.TupleGreaterEqual(hv_max1))) != 0)
                    {
                        hv_res.Dispose();
                        hv_res = new HTuple("MeanB2+a >= MeanW-b, 减小a值，或者增大b值");
                        ho_GrayImageT.Dispose();
                        ho_ROI_0.Dispose();
                        ho_ImageReducedT.Dispose();
                        ho_RegionB.Dispose();
                        ho_RegionB1.Dispose();
                        ho_ImageReducedT1.Dispose();
                        ho_RegionB2.Dispose();
                        ho_ImageReducedT12.Dispose();
                        ho_RegionW.Dispose();
                        ho_ImageReducedW.Dispose();
                        ho_RegionW1.Dispose();
                        ho_ImageReducedW2.Dispose();
                        ho_Region.Dispose();
                        ho_RegionOpening1.Dispose();
                        ho_Rectangle.Dispose();
                        ho_RegionDifference.Dispose();
                        ho_Image1.Dispose();
                        ho_Image2.Dispose();
                        ho_Image3.Dispose();
                        ho_Region1.Dispose();
                        ho_Region2.Dispose();
                        ho_Region3.Dispose();

                        hv_lenthT.Dispose();
                        hv_Width_T.Dispose();
                        hv_Height_T.Dispose();
                        hv_DeviationT.Dispose();
                        hv_MeanB.Dispose();
                        hv_DeviationB.Dispose();
                        hv_DeviationB2.Dispose();
                        hv_MeanB3.Dispose();
                        hv_DeviationB3.Dispose();
                        hv_DeviationW.Dispose();
                        hv_MeanW2.Dispose();
                        hv_DeviationW2.Dispose();
                        hv_min1.Dispose();
                        hv_max1.Dispose();
                        hv_mutil.Dispose();
                        hv_M1.Dispose();
                        hv_M2.Dispose();
                        hv_add.Dispose();
                        hv_Width.Dispose();
                        hv_Height.Dispose();
                        hv_Channels.Dispose();

                        return;
                    }
                    else
                    {
                        hv_mutil.Dispose();
                        using (HDevDisposeHelper dh = new HDevDisposeHelper())
                        {
                            hv_mutil = 255 / (hv_max1 - hv_min1);
                        }
                        hv_M1.Dispose();
                        using (HDevDisposeHelper dh = new HDevDisposeHelper())
                        {
                            hv_M1 = hv_min1 * hv_mutil;
                        }
                        hv_M2.Dispose();
                        using (HDevDisposeHelper dh = new HDevDisposeHelper())
                        {
                            hv_M2 = hv_max1 * hv_mutil;
                        }
                        hv_add.Dispose();
                        using (HDevDisposeHelper dh = new HDevDisposeHelper())
                        {
                            hv_add = -hv_M1;
                        }

                        ho_ImageScaled.Dispose();
                        HOperatorSet.ScaleImage(ho_Image, out ho_ImageScaled, hv_mutil, hv_add);
                        ho_Region.Dispose();
                        HOperatorSet.Threshold(ho_ImageScaled, out ho_Region, 0, 220);
                        ho_RegionOpening1.Dispose();
                        HOperatorSet.OpeningCircle(ho_Region, out ho_RegionOpening1, 1);

                        hv_Width.Dispose(); hv_Height.Dispose();
                        HOperatorSet.GetImageSize(ho_Image, out hv_Width, out hv_Height);
                        using (HDevDisposeHelper dh = new HDevDisposeHelper())
                        {
                            ho_Rectangle.Dispose();
                            HOperatorSet.GenRectangle1(out ho_Rectangle, 0, 0, hv_Height - 1, hv_Width - 1);
                        }

                        ho_RegionDifference.Dispose();
                        HOperatorSet.Difference(ho_Rectangle, ho_RegionOpening1, out ho_RegionDifference
                            );
                        //stop ()
                        hv_Channels.Dispose();
                        HOperatorSet.CountChannels(ho_ImageScaled, out hv_Channels);

                        if ((int)(new HTuple(hv_Channels.TupleEqual(3))) != 0)
                        {
                            ho_Image1.Dispose(); ho_Image2.Dispose(); ho_Image3.Dispose();
                            HOperatorSet.Decompose3(ho_ImageScaled, out ho_Image1, out ho_Image2,
                                out ho_Image3);

                            HOperatorSet.OverpaintRegion(ho_Image1, ho_RegionDifference, 250, "fill");
                            HOperatorSet.OverpaintRegion(ho_Image2, ho_RegionDifference, 250, "fill");
                            HOperatorSet.OverpaintRegion(ho_Image3, ho_RegionDifference, 250, "fill");

                            ho_ImageScaled.Dispose();
                            HOperatorSet.Compose3(ho_Image1, ho_Image2, ho_Image3, out ho_ImageScaled
                                );
                        }
                        else if ((int)(new HTuple(hv_Channels.TupleEqual(1))) != 0)
                        {
                            HOperatorSet.OverpaintRegion(ho_ImageScaled, ho_RegionDifference, 250,
                                "fill");
                        }
                        {
                            HObject
                              ExpTmpLocalVar_ImageScaled = new HObject(ho_ImageScaled);
                            ho_ImageScaled.Dispose();
                            ho_ImageScaled = ExpTmpLocalVar_ImageScaled;
                        }

                        hv_res.Dispose();
                        hv_res = "ok";
                    }
                }
                hv_Channels.Dispose();
                HOperatorSet.CountChannels(ho_ImageScaled, out hv_Channels);

                if ((int)(new HTuple(hv_Channels.TupleEqual(3))) != 0)
                {
                    ho_Image1.Dispose(); ho_Image2.Dispose(); ho_Image3.Dispose();
                    HOperatorSet.Decompose3(ho_ImageScaled, out ho_Image1, out ho_Image2, out ho_Image3
                        );
                    ho_Region1.Dispose();
                    HOperatorSet.Threshold(ho_Image1, out ho_Region1, 0, 2);
                    HOperatorSet.OverpaintRegion(ho_Image1, ho_Region1, 3, "fill");
                    ho_Region1.Dispose();
                    HOperatorSet.Threshold(ho_Image1, out ho_Region1, 254, 255);
                    HOperatorSet.OverpaintRegion(ho_Image1, ho_Region1, 253, "fill");

                    ho_Region2.Dispose();
                    HOperatorSet.Threshold(ho_Image2, out ho_Region2, 0, 2);
                    HOperatorSet.OverpaintRegion(ho_Image2, ho_Region2, 3, "fill");
                    ho_Region2.Dispose();
                    HOperatorSet.Threshold(ho_Image2, out ho_Region2, 254, 255);
                    HOperatorSet.OverpaintRegion(ho_Image2, ho_Region2, 253, "fill");

                    ho_Region3.Dispose();
                    HOperatorSet.Threshold(ho_Image3, out ho_Region3, 0, 2);
                    HOperatorSet.OverpaintRegion(ho_Image3, ho_Region3, 3, "fill");
                    ho_Region3.Dispose();
                    HOperatorSet.Threshold(ho_Image3, out ho_Region3, 254, 255);
                    HOperatorSet.OverpaintRegion(ho_Image3, ho_Region3, 253, "fill");

                    ho_ImageScaled.Dispose();
                    HOperatorSet.Compose3(ho_Image1, ho_Image2, ho_Image3, out ho_ImageScaled
                        );
                }
                else if ((int)(new HTuple(hv_Channels.TupleEqual(1))) != 0)
                {

                    ho_Region1.Dispose();
                    HOperatorSet.Threshold(ho_ImageScaled, out ho_Region1, 0, 2);
                    HOperatorSet.OverpaintRegion(ho_ImageScaled, ho_Region1, 3, "fill");
                    ho_Region1.Dispose();
                    HOperatorSet.Threshold(ho_ImageScaled, out ho_Region1, 254, 255);
                    HOperatorSet.OverpaintRegion(ho_ImageScaled, ho_Region1, 253, "fill");
                }

                //stop ()
            }
            ho_GrayImageT.Dispose();
            ho_ROI_0.Dispose();
            ho_ImageReducedT.Dispose();
            ho_RegionB.Dispose();
            ho_RegionB1.Dispose();
            ho_ImageReducedT1.Dispose();
            ho_RegionB2.Dispose();
            ho_ImageReducedT12.Dispose();
            ho_RegionW.Dispose();
            ho_ImageReducedW.Dispose();
            ho_RegionW1.Dispose();
            ho_ImageReducedW2.Dispose();
            ho_Region.Dispose();
            ho_RegionOpening1.Dispose();
            ho_Rectangle.Dispose();
            ho_RegionDifference.Dispose();
            ho_Image1.Dispose();
            ho_Image2.Dispose();
            ho_Image3.Dispose();
            ho_Region1.Dispose();
            ho_Region2.Dispose();
            ho_Region3.Dispose();

            hv_lenthT.Dispose();
            hv_Width_T.Dispose();
            hv_Height_T.Dispose();
            hv_DeviationT.Dispose();
            hv_MeanB.Dispose();
            hv_DeviationB.Dispose();
            hv_DeviationB2.Dispose();
            hv_MeanB3.Dispose();
            hv_DeviationB3.Dispose();
            hv_DeviationW.Dispose();
            hv_MeanW2.Dispose();
            hv_DeviationW2.Dispose();
            hv_min1.Dispose();
            hv_max1.Dispose();
            hv_mutil.Dispose();
            hv_M1.Dispose();
            hv_M2.Dispose();
            hv_add.Dispose();
            hv_Width.Dispose();
            hv_Height.Dispose();
            hv_Channels.Dispose();

            return;
        }
    }

    class Extract
    {

        public Extract(string folderpath, int KHlength, int saveTXT=1)
        {
            this.get_errimg_KH_TH_2(folderpath, KHlength, saveTXT);
        }
        public void get_errimg_KH_TH(HTuple hv_folderpath, HTuple hv_KHlength, HTuple hv_saveTXT)
        {



            // Local iconic variables 

            // Local control variables 

            HTuple hv_ImageFiles = new HTuple(), hv_KH_TH_All = new HTuple();
            HTuple hv_i = new HTuple(), hv_ImgPath = new HTuple();
            HTuple hv_ImgPath_S = new HTuple(), hv_ImgName = new HTuple();
            HTuple hv_ImgName_S = new HTuple(), hv_KH_TH = new HTuple();
            HTuple hv_Sorted = new HTuple(), hv_DeRplAll = new HTuple();
            HTuple hv_lastN = new HTuple(), hv_KHN = new HTuple();
            HTuple hv_KTList = new HTuple(), hv_KTCurrent = new HTuple();
            HTuple hv_kaohaoNum = new HTuple(), hv_KT = new HTuple();
            HTuple hv_Number = new HTuple(), hv_a = new HTuple(), hv_b = new HTuple();
            HTuple hv_KH = new HTuple(), hv_Number2 = new HTuple();
            HTuple hv_TH = new HTuple(), hv_filename = new HTuple();
            HTuple hv_FileHandle = new HTuple();
            HTuple hv_KHlength_COPY_INP_TMP = new HTuple(hv_KHlength);

            // Initialize local and output iconic variables 
            HTuple hv_KH_TH_List = new HTuple();

            //*手阅打分栏-框 分类错误图片 考号题号提取


            //stop ()
            //*KHlength  考号长度
            //*folderpath   图片地址
            //*saveTXT  是否要存储list  1为是


            {
                HTuple
                  ExpTmpLocalVar_KHlength = new HTuple(hv_KHlength_COPY_INP_TMP);
                hv_KHlength_COPY_INP_TMP.Dispose();
                hv_KHlength_COPY_INP_TMP = ExpTmpLocalVar_KHlength;
            }
            //Image Acquisition 01: Code generated by Image Acquisition 01
            hv_ImageFiles.Dispose();
            HOperatorSet.ListFiles(hv_folderpath, ((new HTuple("files")).TupleConcat("follow_links")).TupleConcat(
                "recursive"), out hv_ImageFiles);

            {
                HTuple ExpTmpOutVar_0;
                HOperatorSet.TupleRegexpSelect(hv_ImageFiles, (new HTuple("\\.(tif|tiff|gif|bmp|jpg|jpeg|jp2|png|pcx|pgm|ppm|pbm|xwd|ima|hobj)$")).TupleConcat(
                    "ignore_case"), out ExpTmpOutVar_0);
                hv_ImageFiles.Dispose();
                hv_ImageFiles = ExpTmpOutVar_0;
            }

            //aa := 1
            hv_KH_TH_All.Dispose();
            hv_KH_TH_All = new HTuple();

            for (hv_i = 0; (int)hv_i <= (int)((new HTuple(hv_ImageFiles.TupleLength())) - 1); hv_i = (int)hv_i + 1)
            {

                hv_ImgPath.Dispose();
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    hv_ImgPath = hv_ImageFiles.TupleSelect(
                        hv_i);
                }
                hv_ImgPath_S.Dispose();
                HOperatorSet.TupleSplit(hv_ImgPath, "\\", out hv_ImgPath_S);
                hv_ImgName.Dispose();
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    hv_ImgName = hv_ImgPath_S.TupleSelect(
                        (new HTuple(hv_ImgPath_S.TupleLength())) - 1);
                }
                hv_ImgName_S.Dispose();
                HOperatorSet.TupleSplit(hv_ImgName, "-", out hv_ImgName_S);
                hv_KH_TH.Dispose();
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    hv_KH_TH = hv_ImgName_S.TupleSelect(
                        1);
                }

                if (hv_KH_TH_All == null)
                    hv_KH_TH_All = new HTuple();
                hv_KH_TH_All[new HTuple(hv_KH_TH_All.TupleLength())] = hv_KH_TH;

            }
            //*paixu
            hv_Sorted.Dispose();
            HOperatorSet.TupleSort(hv_KH_TH_All, out hv_Sorted);

            if ((int)(new HTuple((new HTuple(hv_Sorted.TupleLength())).TupleGreater(0))) != 0)
            {
                hv_DeRplAll.Dispose();
                hv_DeRplAll = new HTuple();
                if (hv_DeRplAll == null)
                    hv_DeRplAll = new HTuple();
                hv_DeRplAll[0] = hv_Sorted.TupleSelect(0);

                //*quchong
                for (hv_i = 0; (int)hv_i <= (int)((new HTuple(hv_Sorted.TupleLength())) - 1); hv_i = (int)hv_i + 1)
                {
                    hv_lastN.Dispose();
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        hv_lastN = hv_DeRplAll.TupleSelect(
                            (new HTuple(hv_DeRplAll.TupleLength())) - 1);
                    }
                    if ((int)(new HTuple(((hv_Sorted.TupleSelect(hv_i))).TupleEqual(hv_lastN))) != 0)
                    {
                        continue;
                    }
                    else
                    {
                        if (hv_DeRplAll == null)
                            hv_DeRplAll = new HTuple();
                        hv_DeRplAll[new HTuple(hv_DeRplAll.TupleLength())] = hv_Sorted.TupleSelect(
                            hv_i);
                    }

                }

                //stop ()

                //*获取考号与题号
                hv_KHN.Dispose();
                hv_KHN = 0;

                hv_KTList.Dispose();
                hv_KTList = new HTuple();
                hv_KTCurrent.Dispose();
                hv_KTCurrent = "";

                hv_kaohaoNum.Dispose();
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    hv_kaohaoNum = (new HTuple(10)).TuplePow(
                        hv_KHlength_COPY_INP_TMP);
                }

                for (hv_i = 0; (int)hv_i <= (int)((new HTuple(hv_DeRplAll.TupleLength())) - 1); hv_i = (int)hv_i + 1)
                {
                    hv_KT.Dispose();
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        hv_KT = hv_DeRplAll.TupleSelect(
                            hv_i);
                    }
                    hv_Number.Dispose();
                    HOperatorSet.TupleNumber(hv_KT, out hv_Number);

                    hv_a.Dispose();
                    hv_a = 1;
                    while ((int)(new HTuple(hv_Number.TupleGreater(hv_kaohaoNum))) != 0)
                    {
                        using (HDevDisposeHelper dh = new HDevDisposeHelper())
                        {
                            {
                                HTuple
                                  ExpTmpLocalVar_a = hv_a * 10;
                                hv_a.Dispose();
                                hv_a = ExpTmpLocalVar_a;
                            }
                        }
                        hv_b.Dispose();
                        using (HDevDisposeHelper dh = new HDevDisposeHelper())
                        {
                            hv_b = hv_Number / 10;
                        }
                        hv_Number.Dispose();
                        hv_Number = new HTuple(hv_b);
                    }
                    hv_KH.Dispose();
                    hv_KH = new HTuple(hv_Number);
                    hv_Number2.Dispose();
                    HOperatorSet.TupleNumber(hv_KT, out hv_Number2);
                    hv_TH.Dispose();
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        hv_TH = hv_Number2 - (hv_KH * hv_a);
                    }

                    //*判断和上一个考号是否相同，若相同则字符串后面加题号，若不同则一个学生完成更换考号和题号
                    if ((int)(new HTuple(hv_KHN.TupleEqual(hv_KH))) != 0)
                    {
                        using (HDevDisposeHelper dh = new HDevDisposeHelper())
                        {
                            {
                                HTuple
                                  ExpTmpLocalVar_KTCurrent = (hv_KTCurrent + new HTuple(", ")) + hv_TH;
                                hv_KTCurrent.Dispose();
                                hv_KTCurrent = ExpTmpLocalVar_KTCurrent;
                            }
                        }
                    }
                    else
                    {
                        if ((int)(new HTuple(hv_KHN.TupleEqual(0))) != 0)
                        {
                            hv_KHN.Dispose();
                            hv_KHN = new HTuple(hv_KH);
                            hv_KTCurrent.Dispose();
                            using (HDevDisposeHelper dh = new HDevDisposeHelper())
                            {
                                hv_KTCurrent = (hv_KH + " : ") + hv_TH;
                            }
                        }
                        else
                        {
                            if (hv_KTList == null)
                                hv_KTList = new HTuple();
                            hv_KTList[new HTuple(hv_KTList.TupleLength())] = hv_KTCurrent;
                            hv_KHN.Dispose();
                            hv_KHN = new HTuple(hv_KH);
                            hv_KTCurrent.Dispose();
                            using (HDevDisposeHelper dh = new HDevDisposeHelper())
                            {
                                hv_KTCurrent = (hv_KH + " : ") + hv_TH;
                            }
                        }
                    }

                }


                //stop ()

                hv_KH_TH_List.Dispose();
                hv_KH_TH_List = new HTuple(hv_KTList);

                if ((int)(new HTuple(hv_saveTXT.TupleEqual(1))) != 0)
                {
                    hv_filename.Dispose();
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        hv_filename = (hv_folderpath + "\\") + "KH-TH-list.txt";
                    }
                    hv_FileHandle.Dispose();
                    HOperatorSet.OpenFile(hv_filename, "append", out hv_FileHandle);
                    for (hv_i = 0; (int)hv_i <= (int)((new HTuple(hv_KH_TH_List.TupleLength())) - 1); hv_i = (int)hv_i + 1)
                    {
                        using (HDevDisposeHelper dh = new HDevDisposeHelper())
                        {
                            HOperatorSet.FwriteString(hv_FileHandle, hv_KH_TH_List.TupleSelect(hv_i));
                        }
                        HOperatorSet.FnewLine(hv_FileHandle);
                    }
                    HOperatorSet.CloseFile(hv_FileHandle);

                    MessageBox.Show("提取完成", "提示", MessageBoxButtons.OK, MessageBoxIcon.None);
                    //stop ()
                }

            }
            else
            {
                hv_KH_TH_List.Dispose();
                hv_KH_TH_List = new HTuple();
                if ((int)(new HTuple(hv_saveTXT.TupleEqual(1))) != 0)
                {
                    hv_filename.Dispose();
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        hv_filename = (hv_folderpath + "\\") + "KH-TH-list.txt";
                    }
                    hv_FileHandle.Dispose();
                    HOperatorSet.OpenFile(hv_filename, "append", out hv_FileHandle);
                    HOperatorSet.FwriteString(hv_FileHandle, "列表为空！");
                    HOperatorSet.CloseFile(hv_FileHandle);

                    MessageBox.Show("目录为空！", "提示", MessageBoxButtons.OK, MessageBoxIcon.None);
                    //stop ()
                }

            }









            hv_KHlength_COPY_INP_TMP.Dispose();
            hv_ImageFiles.Dispose();
            hv_KH_TH_All.Dispose();
            hv_i.Dispose();
            hv_ImgPath.Dispose();
            hv_ImgPath_S.Dispose();
            hv_ImgName.Dispose();
            hv_ImgName_S.Dispose();
            hv_KH_TH.Dispose();
            hv_Sorted.Dispose();
            hv_DeRplAll.Dispose();
            hv_lastN.Dispose();
            hv_KHN.Dispose();
            hv_KTList.Dispose();
            hv_KTCurrent.Dispose();
            hv_kaohaoNum.Dispose();
            hv_KT.Dispose();
            hv_Number.Dispose();
            hv_a.Dispose();
            hv_b.Dispose();
            hv_KH.Dispose();
            hv_Number2.Dispose();
            hv_TH.Dispose();
            hv_filename.Dispose();
            hv_FileHandle.Dispose();

            return;
        }

        public void get_errimg_KH_TH_2(HTuple hv_folderpath, HTuple hv_KHlength, HTuple hv_saveTXT)
        {



            // Local iconic variables 

            // Local control variables 

            HTuple hv_KHlengthOut = new HTuple(), hv_ImageFiles = new HTuple();
            HTuple hv_KH_TH_All = new HTuple(), hv_KH1 = new HTuple();
            HTuple hv_TH1 = new HTuple(), hv_i = new HTuple(), hv_ImgPath = new HTuple();
            HTuple hv_ImgPath_S = new HTuple(), hv_ImgName = new HTuple();
            HTuple hv_ImgName_S = new HTuple(), hv_Sorted = new HTuple();
            HTuple hv_DeRplAll = new HTuple(), hv_lastN = new HTuple();
            HTuple hv_KHN = new HTuple(), hv_KTList = new HTuple();
            HTuple hv_KTCurrent = new HTuple(), hv_kaohaoNum = new HTuple();
            HTuple hv_KT = new HTuple(), hv_Number = new HTuple();
            HTuple hv_a = new HTuple(), hv_b = new HTuple(), hv_KH = new HTuple();
            HTuple hv_Number2 = new HTuple(), hv_TH = new HTuple();
            HTuple hv_filename = new HTuple(), hv_FileHandle = new HTuple();
            // Initialize local and output iconic variables 
            HTuple hv_KH_TH_List = new HTuple();
            hv_KHlengthOut.Dispose();
            hv_KHlengthOut = new HTuple(hv_KHlength);



            //*手阅打分栏-框 分类错误图片 考号题号提取


            //stop ()
            //*KHlength  考号长度
            //*folderpath   图片地址
            //*saveTXT  是否要存储list  1为是


            {
                HTuple
                  ExpTmpLocalVar_KHlengthOut = new HTuple(hv_KHlengthOut);
                hv_KHlengthOut.Dispose();
                hv_KHlengthOut = ExpTmpLocalVar_KHlengthOut;
            }
            //Image Acquisition 01: Code generated by Image Acquisition 01
            hv_ImageFiles.Dispose();
            HOperatorSet.ListFiles(hv_folderpath, ((new HTuple("files")).TupleConcat("follow_links")).TupleConcat(
                "recursive"), out hv_ImageFiles);

            {
                HTuple ExpTmpOutVar_0;
                HOperatorSet.TupleRegexpSelect(hv_ImageFiles, (new HTuple("\\.(tif|tiff|gif|bmp|jpg|jpeg|jp2|png|pcx|pgm|ppm|pbm|xwd|ima|hobj)$")).TupleConcat(
                    "ignore_case"), out ExpTmpOutVar_0);
                hv_ImageFiles.Dispose();
                hv_ImageFiles = ExpTmpOutVar_0;
            }

            //aa := 1
            hv_KH_TH_All.Dispose();
            hv_KH_TH_All = new HTuple();
            //KH1 := []
            //TH1 := []

            for (hv_i = 0; (int)hv_i <= (int)((new HTuple(hv_ImageFiles.TupleLength())) - 1); hv_i = (int)hv_i + 1)
            {

                hv_ImgPath.Dispose();
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    hv_ImgPath = hv_ImageFiles.TupleSelect(
                        hv_i);
                }
                hv_ImgPath_S.Dispose();
                HOperatorSet.TupleSplit(hv_ImgPath, "\\", out hv_ImgPath_S);
                hv_ImgName.Dispose();
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    hv_ImgName = hv_ImgPath_S.TupleSelect(
                        (new HTuple(hv_ImgPath_S.TupleLength())) - 1);
                }
                hv_ImgName_S.Dispose();
                HOperatorSet.TupleSplit(hv_ImgName, "-", out hv_ImgName_S);
                //KH_TH := ImgName_S[1]
                hv_KH1.Dispose();
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    hv_KH1 = hv_ImgName_S.TupleSelect(
                        1);
                }
                hv_TH1.Dispose();
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    hv_TH1 = hv_ImgName_S.TupleSelect(
                        2);
                }

                //*     KH_TH_All[|KH_TH_All|]:=KH_TH
                if (hv_KH_TH_All == null)
                    hv_KH_TH_All = new HTuple();
                hv_KH_TH_All[new HTuple(hv_KH_TH_All.TupleLength())] = hv_KH1 + hv_TH1;
            }
            //*paixu
            hv_Sorted.Dispose();
            HOperatorSet.TupleSort(hv_KH_TH_All, out hv_Sorted);

            if ((int)(new HTuple((new HTuple(hv_Sorted.TupleLength())).TupleGreater(0))) != 0)
            {
                hv_DeRplAll.Dispose();
                hv_DeRplAll = new HTuple();
                if (hv_DeRplAll == null)
                    hv_DeRplAll = new HTuple();
                hv_DeRplAll[0] = hv_Sorted.TupleSelect(0);

                //*quchong
                for (hv_i = 0; (int)hv_i <= (int)((new HTuple(hv_Sorted.TupleLength())) - 1); hv_i = (int)hv_i + 1)
                {
                    hv_lastN.Dispose();
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        hv_lastN = hv_DeRplAll.TupleSelect(
                            (new HTuple(hv_DeRplAll.TupleLength())) - 1);
                    }
                    if ((int)(new HTuple(((hv_Sorted.TupleSelect(hv_i))).TupleEqual(hv_lastN))) != 0)
                    {
                        continue;
                    }
                    else
                    {
                        if (hv_DeRplAll == null)
                            hv_DeRplAll = new HTuple();
                        hv_DeRplAll[new HTuple(hv_DeRplAll.TupleLength())] = hv_Sorted.TupleSelect(
                            hv_i);
                    }

                }

                //stop ()

                //*获取考号与题号
                hv_KHN.Dispose();
                hv_KHN = 0;

                hv_KTList.Dispose();
                hv_KTList = new HTuple();
                hv_KTCurrent.Dispose();
                hv_KTCurrent = "";

                hv_kaohaoNum.Dispose();
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    hv_kaohaoNum = (new HTuple(10)).TuplePow(
                        hv_KHlengthOut);
                }

                for (hv_i = 0; (int)hv_i <= (int)((new HTuple(hv_DeRplAll.TupleLength())) - 1); hv_i = (int)hv_i + 1)
                {
                    hv_KT.Dispose();
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        hv_KT = hv_DeRplAll.TupleSelect(
                            hv_i);
                    }
                    hv_Number.Dispose();
                    HOperatorSet.TupleNumber(hv_KT, out hv_Number);

                    hv_a.Dispose();
                    hv_a = 1;
                    while ((int)(new HTuple(hv_Number.TupleGreater(hv_kaohaoNum))) != 0)
                    {
                        using (HDevDisposeHelper dh = new HDevDisposeHelper())
                        {
                            {
                                HTuple
                                  ExpTmpLocalVar_a = hv_a * 10;
                                hv_a.Dispose();
                                hv_a = ExpTmpLocalVar_a;
                            }
                        }
                        hv_b.Dispose();
                        using (HDevDisposeHelper dh = new HDevDisposeHelper())
                        {
                            hv_b = hv_Number / 10;
                        }
                        hv_Number.Dispose();
                        hv_Number = new HTuple(hv_b);
                    }
                    hv_KH.Dispose();
                    hv_KH = new HTuple(hv_Number);
                    hv_Number2.Dispose();
                    HOperatorSet.TupleNumber(hv_KT, out hv_Number2);
                    hv_TH.Dispose();
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        hv_TH = hv_Number2 - (hv_KH * hv_a);
                    }

                    //*判断和上一个考号是否相同，若相同则字符串后面加题号，若不同则一个学生完成更换考号和题号
                    if ((int)(new HTuple(hv_KHN.TupleEqual(hv_KH))) != 0)
                    {
                        using (HDevDisposeHelper dh = new HDevDisposeHelper())
                        {
                            {
                                HTuple
                                  ExpTmpLocalVar_KTCurrent = (hv_KTCurrent + new HTuple(", ")) + hv_TH;
                                hv_KTCurrent.Dispose();
                                hv_KTCurrent = ExpTmpLocalVar_KTCurrent;
                            }
                        }
                    }
                    else
                    {
                        if ((int)(new HTuple(hv_KHN.TupleEqual(0))) != 0)
                        {
                            hv_KHN.Dispose();
                            hv_KHN = new HTuple(hv_KH);
                            hv_KTCurrent.Dispose();
                            using (HDevDisposeHelper dh = new HDevDisposeHelper())
                            {
                                hv_KTCurrent = (hv_KH + " : ") + hv_TH;
                            }
                        }
                        else
                        {
                            if (hv_KTList == null)
                                hv_KTList = new HTuple();
                            hv_KTList[new HTuple(hv_KTList.TupleLength())] = hv_KTCurrent;
                            hv_KHN.Dispose();
                            hv_KHN = new HTuple(hv_KH);
                            hv_KTCurrent.Dispose();
                            using (HDevDisposeHelper dh = new HDevDisposeHelper())
                            {
                                hv_KTCurrent = (hv_KH + " : ") + hv_TH;
                            }
                        }
                    }

                }


                //stop ()

                hv_KH_TH_List.Dispose();
                hv_KH_TH_List = new HTuple(hv_KTList);

                if ((int)(new HTuple(hv_saveTXT.TupleEqual(1))) != 0)
                {
                    hv_filename.Dispose();
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        hv_filename = (hv_folderpath + "\\") + "KH-TH-list.txt";
                    }
                    hv_FileHandle.Dispose();
                    HOperatorSet.OpenFile(hv_filename, "append", out hv_FileHandle);
                    for (hv_i = 0; (int)hv_i <= (int)((new HTuple(hv_KH_TH_List.TupleLength())) - 1); hv_i = (int)hv_i + 1)
                    {
                        using (HDevDisposeHelper dh = new HDevDisposeHelper())
                        {
                            HOperatorSet.FwriteString(hv_FileHandle, hv_KH_TH_List.TupleSelect(hv_i));
                        }
                        HOperatorSet.FnewLine(hv_FileHandle);
                    }
                    HOperatorSet.CloseFile(hv_FileHandle);

                    MessageBox.Show("提取完成", "提示", MessageBoxButtons.OK, MessageBoxIcon.None);
                    //stop ()
                }

            }
            else
            {
                hv_KH_TH_List.Dispose();
                hv_KH_TH_List = new HTuple();
                if ((int)(new HTuple(hv_saveTXT.TupleEqual(1))) != 0)
                {
                    hv_filename.Dispose();
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        hv_filename = (hv_folderpath + "\\") + "KH-TH-list.txt";
                    }
                    hv_FileHandle.Dispose();
                    HOperatorSet.OpenFile(hv_filename, "append", out hv_FileHandle);
                    HOperatorSet.FwriteString(hv_FileHandle, "列表为空！");
                    HOperatorSet.CloseFile(hv_FileHandle);

                    MessageBox.Show("目录为空！", "提示", MessageBoxButtons.OK, MessageBoxIcon.None);
                    //stop ()
                }

            }









            hv_KHlengthOut.Dispose();
            hv_ImageFiles.Dispose();
            hv_KH_TH_All.Dispose();
            hv_KH1.Dispose();
            hv_TH1.Dispose();
            hv_i.Dispose();
            hv_ImgPath.Dispose();
            hv_ImgPath_S.Dispose();
            hv_ImgName.Dispose();
            hv_ImgName_S.Dispose();
            hv_Sorted.Dispose();
            hv_DeRplAll.Dispose();
            hv_lastN.Dispose();
            hv_KHN.Dispose();
            hv_KTList.Dispose();
            hv_KTCurrent.Dispose();
            hv_kaohaoNum.Dispose();
            hv_KT.Dispose();
            hv_Number.Dispose();
            hv_a.Dispose();
            hv_b.Dispose();
            hv_KH.Dispose();
            hv_Number2.Dispose();
            hv_TH.Dispose();
            hv_filename.Dispose();
            hv_FileHandle.Dispose();

            return;



            hv_KHlengthOut.Dispose();
            hv_ImageFiles.Dispose();
            hv_KH_TH_All.Dispose();
            hv_KH1.Dispose();
            hv_TH1.Dispose();
            hv_i.Dispose();
            hv_ImgPath.Dispose();
            hv_ImgPath_S.Dispose();
            hv_ImgName.Dispose();
            hv_ImgName_S.Dispose();
            hv_Sorted.Dispose();
            hv_DeRplAll.Dispose();
            hv_lastN.Dispose();
            hv_KHN.Dispose();
            hv_KTList.Dispose();
            hv_KTCurrent.Dispose();
            hv_kaohaoNum.Dispose();
            hv_KT.Dispose();
            hv_Number.Dispose();
            hv_a.Dispose();
            hv_b.Dispose();
            hv_KH.Dispose();
            hv_Number2.Dispose();
            hv_TH.Dispose();
            hv_filename.Dispose();
            hv_FileHandle.Dispose();

            return;
        }
    }
}
