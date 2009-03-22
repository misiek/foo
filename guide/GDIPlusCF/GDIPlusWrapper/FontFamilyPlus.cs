using System;
using System.Collections.Generic;
using System.Text;

namespace OpenNETCF.GDIPlus
{
    public class FontFamilyPlus
    {




        //FontFamilyPlus(
        //    GpFontFamily nativeOrig,
        //    GpStatus status
        //)
        //{
        //    lastResult    = status;
        //    nativeFamily = nativeOrig;
        //}


        // ~FontFamilyPlus()
        //{
        //}


        // ushort 
        //GetEmHeight(int style) 
        //{
        //    ushort  EmHeight;

        //    SetStatus(NativeMethods.GdipGetEmHeight(nativeFamily, style, &EmHeight));

        //    return EmHeight;
        //}

        // ushort 
        //GetCellAscent(int style) 
        //{
        //    ushort  CellAscent;

        //    SetStatus(NativeMethods.GdipGetCellAscent(nativeFamily, style, &CellAscent));

        //    return CellAscent;
        //}

        // ushort 
        //GetCellDescent(int style) 
        //{
        //    ushort  CellDescent;

        //    SetStatus(NativeMethods.GdipGetCellDescent(nativeFamily, style, &CellDescent));

        //    return CellDescent;
        //}


        // ushort 
        //GetLineSpacing(int style) 
        //{
        //    ushort  LineSpacing;

        //    SetStatus(NativeMethods.GdipGetLineSpacing(nativeFamily, style, &LineSpacing));

        //    return LineSpacing;

        //}

        // GpStatus 
        //GetLastStatus() 
        //{
        //    GpStatus lastStatus = lastResult;
        //    lastResult = GpStatus.Ok;

        //    return lastStatus;
        //}

        // GpStatus
        //SetStatus(GpStatus status) 
        //{
        //    if (status != GpStatus.Ok)
        //        return (lastResult = status);
        //    else
        //        return status;
        //}
        internal GpFontFamily nativeFamily;
        private GpStatus lastResult;
    }
}
