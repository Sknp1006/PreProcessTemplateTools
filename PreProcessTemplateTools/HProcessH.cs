using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PreProcessTemplateTools
{
    // halcon处理模块
    class HProcessH
    {
        public HObject Image = new HObject();  // 操作对象
        HObject TempImage = new HObject();  // 临时操作对象
        
        string OriginPath = "";
        public TikaModel tikaModel = new TikaModel();
        public HWindowControl HWindowControl_0 = null;

        string savePath = Application.StartupPath + "\\savePath\\";  // 保存路径

        public Stack<HObject> ImageStack = new Stack<HObject>();  // 操作栈，栈中的数据会被外部修改

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
            HOperatorSet.Rgb1ToGray(Image, out Image);

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

            Enhance_Template_Image(Image,out Image, 2, a, b,out hv_MeanB2,out hv_MeanT,out hv_MeanW,out hv_res);

            if (tikaModel.MeanGrayValue == null)
            {
                tikaModel.MeanGrayValue = hv_MeanT;
            }
            if (tikaModel.MinGrayValue == null)
            {
                tikaModel.MinGrayValue = hv_MeanB2;
            }
            if (tikaModel.MaxGrayValue == null)
            {
                tikaModel.MaxGrayValue = hv_MeanW;
            }
            if (tikaModel.Status == null)
            {
                tikaModel.Status = hv_res;
            }

            hv_MeanT.Dispose();
            hv_MeanB2.Dispose();
            hv_MeanW.Dispose();
            hv_res.Dispose();
        }
        

        public void ShowImage()  // 自适应显示
        {
            HTuple hv_Width = tikaModel.Width;
            HTuple hv_Height = tikaModel.Height;
            HTuple row1, column1, row2, column2;

            CalScaleValue(hv_Width, hv_Height, HWindowControl_0, out row1, out column1, out row2, out column2);  // 计算缩放比

            HWindowControl_0.HalconWindow.ClearWindow();
            HDevWindowStack.Push(HWindowControl_0.HalconWindow);
            HOperatorSet.SetColor(HDevWindowStack.GetActive(), "blue");
            HOperatorSet.SetPart(HDevWindowStack.GetActive(), row1, column1, row2, column2);
            HOperatorSet.DispObj(Image, HDevWindowStack.GetActive());

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
            HOperatorSet.DispObj(Image, HDevWindowStack.GetActive());

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
            HOperatorSet.DispObj(Image, HDevWindowStack.GetActive());

            row1.Dispose();
            column1.Dispose();
            row2.Dispose();
            column2.Dispose();
        }


        // 计算缩放比
        public void CalScaleValue(HTuple Width, HTuple Height, HWindowControl hv_WindowControl, out HTuple row1, out HTuple column1, out HTuple row2, out HTuple column2)
        {
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
            
            if (System.IO.Directory.Exists(savePath) == false)
            {
                System.IO.Directory.CreateDirectory(savePath);
            }
            this.tikaModel.SavePath = savePath + tikaModel.OriginPath.Split('\\').Last();
            HOperatorSet.WriteImage(Image, "jpg", -1, this.tikaModel.SavePath);
        }

        public void PaintImage()
        {
            HObject tempImage = new HObject(Image);  // 绘制前现创建新对象
            ImageStack.Push(tempImage);  // 入栈
            TempImage.Dispose();
            TempImage = tempImage;

             
        }


        public void Dispose()
        {
            Image.Dispose();

            // 清空堆栈
            foreach (HObject item in ImageStack)
            {
                item.Dispose();
            }
            ImageStack.Clear();
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
}
